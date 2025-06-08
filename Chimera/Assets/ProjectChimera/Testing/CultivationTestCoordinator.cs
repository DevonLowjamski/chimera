using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Testing.Systems;
using System;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Master coordinator for all Project Chimera cultivation system tests.
    /// Orchestrates validation, core tests, cultivation tests, and performance monitoring.
    /// Provides comprehensive reporting and debugging capabilities.
    /// </summary>
    public class CultivationTestCoordinator : MonoBehaviour
    {
        [Header("Coordinator Configuration")]
        [SerializeField] private bool _runFullTestSuiteOnStart = true;
        [SerializeField] private bool _enableComprehensiveLogging = true;
        [SerializeField] private bool _showRealTimeResults = true;
        [SerializeField] private float _coordinatorTimeout = 300f; // 5 minutes max
        
        [Header("Test Component References")]
        [SerializeField] private CultivationSystemValidator _validator;
        [SerializeField] private AdvancedCultivationTestRunner _testRunner;
        [SerializeField] private AdvancedCultivationTester _cultivationTester;
        [SerializeField] private CoreSystemTester _coreSystemTester;
        
        [Header("Coordination Results")]
        [SerializeField] private TestCoordinationResult _coordinationResult;
        [SerializeField] private List<string> _coordinationLog = new List<string>();
        
        // Runtime state
        private bool _coordinationRunning = false;
        private float _coordinationStartTime;
        private TestPhaseStatus _currentPhase = TestPhaseStatus.NotStarted;
        
        // Events
        public System.Action<TestCoordinationResult> OnCoordinationComplete;
        public System.Action<TestPhaseStatus> OnPhaseChanged;
        public System.Action<string> OnLogMessage;
        
        private void Start()
        {
            // Auto-find components if not assigned
            if (_validator == null) _validator = FindAnyObjectByType<CultivationSystemValidator>();
            if (_testRunner == null) _testRunner = FindAnyObjectByType<AdvancedCultivationTestRunner>();
            if (_cultivationTester == null) _cultivationTester = FindAnyObjectByType<AdvancedCultivationTester>();
            if (_coreSystemTester == null) _coreSystemTester = FindAnyObjectByType<CoreSystemTester>();
            
            if (_runFullTestSuiteOnStart)
            {
                // Delay start to allow Unity to fully initialize
                StartCoroutine(DelayedTestStart());
            }
        }
        
        private IEnumerator DelayedTestStart()
        {
            yield return new WaitForSeconds(2f); // Allow scene to fully load
            StartCoordinatedTesting();
        }
        
        public void StartCoordinatedTesting()
        {
            if (_coordinationRunning)
            {
                LogCoordination("Coordination already running!");
                return;
            }
            
            LogCoordination("=== STARTING PROJECT CHIMERA CULTIVATION SYSTEM TEST COORDINATION ===");
            StartCoroutine(RunCoordinatedTests());
        }
        
        private IEnumerator RunCoordinatedTests()
        {
            _coordinationRunning = true;
            _coordinationStartTime = Time.time;
            _coordinationLog.Clear();
            
            // Initialize coordination result
            _coordinationResult = new TestCoordinationResult
            {
                StartTime = DateTime.Now,
                UnityVersion = Application.unityVersion,
                Platform = Application.platform.ToString()
            };
            
            // Phase 1: System Validation
            yield return StartCoroutine(RunValidationPhase());
            
            // Phase 2: Core System Tests (if validation passes)
            if (_coordinationResult.ValidationResult?.OverallStatus != ValidationStatus.Failed)
            {
                yield return StartCoroutine(RunCoreTestPhase());
            }
            
            // Phase 3: Advanced Cultivation Tests (if core tests pass)
            if (_coordinationResult.CoreTestsStatus == TestStatus.Passed)
            {
                yield return StartCoroutine(RunCultivationTestPhase());
            }
            
            // Phase 4: Integration Testing (if cultivation tests pass)
            if (_coordinationResult.CultivationTestsStatus == TestStatus.Passed)
            {
                yield return StartCoroutine(RunIntegrationTestPhase());
            }
            
            // Phase 5: Final Analysis
            yield return StartCoroutine(RunFinalAnalysisPhase());
            
            // Finalize coordination
            _coordinationResult.EndTime = DateTime.Now;
            _coordinationResult.TotalDuration = Time.time - _coordinationStartTime;
            _coordinationRunning = false;
            
            ChangePhase(TestPhaseStatus.Completed);
            OnCoordinationComplete?.Invoke(_coordinationResult);
            
            LogCoordination("=== COORDINATION COMPLETED ===");
            LogCoordination(GenerateCoordinationSummary());
        }
        
        private IEnumerator RunValidationPhase()
        {
            ChangePhase(TestPhaseStatus.Validation);
            LogCoordination("\n--- PHASE 1: SYSTEM VALIDATION ---");
            
            if (_validator == null)
            {
                LogCoordination("WARNING: CultivationSystemValidator not found!");
                _coordinationResult.ValidationResult = new ValidationResult
                {
                    OverallStatus = ValidationStatus.Failed,
                    FailedChecks = 1,
                    TotalChecks = 1
                };
                yield break;
            }
            
            var validationResult = _validator.ValidateAllSystems();
            _coordinationResult.ValidationResult = validationResult;
            
            LogCoordination($"Validation Status: {validationResult.OverallStatus}");
            LogCoordination($"Validation Success Rate: {validationResult.SuccessRate:F1}%");
            
            if (validationResult.OverallStatus == ValidationStatus.Failed)
            {
                LogCoordination("CRITICAL: Validation failed - stopping coordination");
                _coordinationResult.OverallStatus = TestStatus.Failed;
                yield break;
            }
            
            yield return new WaitForSeconds(1f);
        }
        
        private IEnumerator RunCoreTestPhase()
        {
            ChangePhase(TestPhaseStatus.CoreTesting);
            LogCoordination("\n--- PHASE 2: CORE SYSTEM TESTING ---");
            
            if (_coreSystemTester == null)
            {
                LogCoordination("WARNING: CoreSystemTester not found!");
                _coordinationResult.CoreTestsStatus = TestStatus.Failed;
                yield break;
            }
            
            _coreSystemTester.RunTestsManually();
            
            // Wait for core tests to complete
            float timeout = Time.time + 60f; // 1 minute timeout
            while (_coreSystemTester.TestsRunning && Time.time < timeout)
            {
                yield return new WaitForSeconds(0.5f);
            }
            
            if (_coreSystemTester.TestsRunning)
            {
                LogCoordination("WARNING: Core tests timed out");
                _coordinationResult.CoreTestsStatus = TestStatus.Timeout;
            }
            else if (_coreSystemTester.TestsFailed > 0)
            {
                LogCoordination($"Core tests completed with {_coreSystemTester.TestsFailed} failures");
                _coordinationResult.CoreTestsStatus = TestStatus.PartialPass;
            }
            else if (_coreSystemTester.TestsPassed > 0)
            {
                LogCoordination($"Core tests passed: {_coreSystemTester.TestsPassed}/{_coreSystemTester.TestsRun}");
                _coordinationResult.CoreTestsStatus = TestStatus.Passed;
            }
            else
            {
                LogCoordination("No core tests were run");
                _coordinationResult.CoreTestsStatus = TestStatus.NotRun;
            }
            
            _coordinationResult.CoreTestsPassed = _coreSystemTester.TestsPassed;
            _coordinationResult.CoreTestsFailed = _coreSystemTester.TestsFailed;
            
            yield return new WaitForSeconds(1f);
        }
        
        private IEnumerator RunCultivationTestPhase()
        {
            ChangePhase(TestPhaseStatus.CultivationTesting);
            LogCoordination("\n--- PHASE 3: ADVANCED CULTIVATION TESTING ---");
            
            if (_testRunner != null)
            {
                LogCoordination("Using AdvancedCultivationTestRunner");
                _testRunner.StartTestSuite();
                
                // Wait for test runner to complete
                float timeout = Time.time + 180f; // 3 minute timeout
                while (_testRunner.TestsRunning && Time.time < timeout)
                {
                    yield return new WaitForSeconds(1f);
                }
                
                if (_testRunner.TestsRunning)
                {
                    LogCoordination("WARNING: Test runner timed out");
                    _coordinationResult.CultivationTestsStatus = TestStatus.Timeout;
                }
                else
                {
                    var testSuite = _testRunner.TestSuite;
                    if (testSuite != null)
                    {
                        var results = _testRunner.TestResults;
                        int passed = results.FindAll(r => r.Passed).Count;
                        int failed = results.Count - passed;
                        
                        _coordinationResult.CultivationTestsPassed = passed;
                        _coordinationResult.CultivationTestsFailed = failed;
                        _coordinationResult.CultivationTestsStatus = failed == 0 ? TestStatus.Passed : TestStatus.PartialPass;
                        
                        LogCoordination($"Test runner completed: {passed} passed, {failed} failed");
                    }
                    else
                    {
                        _coordinationResult.CultivationTestsStatus = TestStatus.NotRun;
                    }
                }
            }
            else if (_cultivationTester != null)
            {
                LogCoordination("Using AdvancedCultivationTester");
                _cultivationTester.RunTestsManually();
                
                // Wait for cultivation tests to complete
                float timeout = Time.time + 120f; // 2 minute timeout
                while (_cultivationTester.TestsRunning && Time.time < timeout)
                {
                    yield return new WaitForSeconds(1f);
                }
                
                if (_cultivationTester.TestsRunning)
                {
                    LogCoordination("WARNING: Cultivation tests timed out");
                    _coordinationResult.CultivationTestsStatus = TestStatus.Timeout;
                }
                else
                {
                    _coordinationResult.CultivationTestsPassed = _cultivationTester.PassedTests;
                    _coordinationResult.CultivationTestsFailed = _cultivationTester.FailedTests;
                    _coordinationResult.CultivationTestsStatus = _cultivationTester.FailedTests == 0 ? TestStatus.Passed : TestStatus.PartialPass;
                    
                    LogCoordination($"Cultivation tests completed: {_cultivationTester.PassedTests} passed, {_cultivationTester.FailedTests} failed");
                }
            }
            
            yield return new WaitForSeconds(1f);
        }
        
        private IEnumerator RunIntegrationTestPhase()
        {
            ChangePhase(TestPhaseStatus.Integration);
            LogCoordination("\n--- PHASE 4: INTEGRATION TESTING ---");
            
            // Basic integration test - check if all systems can be accessed
            var gameManager = FindAnyObjectByType<GameManager>();
            var cultivationManager = FindAnyObjectByType<CultivationManager>();
            
            bool integrationPassed = true;
            int integrationTests = 0;
            int integrationPasses = 0;
            
            // Test 1: Manager accessibility
            integrationTests++;
            if (gameManager != null && cultivationManager != null)
            {
                integrationPasses++;
                LogCoordination("✓ Manager accessibility test passed");
            }
            else
            {
                integrationPassed = false;
                LogCoordination("✗ Manager accessibility test failed");
            }
            
            // Test 2: System initialization
            integrationTests++;
            if (gameManager?.IsInitialized == true && cultivationManager?.IsInitialized == true)
            {
                integrationPasses++;
                LogCoordination("✓ System initialization test passed");
            }
            else
            {
                integrationPassed = false;
                LogCoordination("✗ System initialization test failed");
            }
            
            // Test 3: Memory stability
            integrationTests++;
            float memoryUsage = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory() / (1024f * 1024f);
            if (memoryUsage < 1000f) // Less than 1GB
            {
                integrationPasses++;
                LogCoordination($"✓ Memory stability test passed ({memoryUsage:F1}MB)");
            }
            else
            {
                integrationPassed = false;
                LogCoordination($"✗ Memory stability test failed ({memoryUsage:F1}MB)");
            }
            
            _coordinationResult.IntegrationTestsPassed = integrationPasses;
            _coordinationResult.IntegrationTestsFailed = integrationTests - integrationPasses;
            _coordinationResult.IntegrationTestsStatus = integrationPassed ? TestStatus.Passed : TestStatus.PartialPass;
            
            yield return new WaitForSeconds(1f);
        }
        
        private IEnumerator RunFinalAnalysisPhase()
        {
            ChangePhase(TestPhaseStatus.Analysis);
            LogCoordination("\n--- PHASE 5: FINAL ANALYSIS ---");
            
            // Calculate overall status
            var statuses = new List<TestStatus>
            {
                _coordinationResult.CoreTestsStatus,
                _coordinationResult.CultivationTestsStatus,
                _coordinationResult.IntegrationTestsStatus
            };
            
            bool hasFailures = statuses.Contains(TestStatus.Failed);
            bool hasTimeouts = statuses.Contains(TestStatus.Timeout);
            bool hasPartialPasses = statuses.Contains(TestStatus.PartialPass);
            bool hasNotRun = statuses.Contains(TestStatus.NotRun);
            
            if (hasFailures)
            {
                _coordinationResult.OverallStatus = TestStatus.Failed;
            }
            else if (hasTimeouts)
            {
                _coordinationResult.OverallStatus = TestStatus.Timeout;
            }
            else if (hasPartialPasses || hasNotRun)
            {
                _coordinationResult.OverallStatus = TestStatus.PartialPass;
            }
            else
            {
                _coordinationResult.OverallStatus = TestStatus.Passed;
            }
            
            // Calculate totals
            _coordinationResult.TotalTestsPassed = 
                _coordinationResult.CoreTestsPassed + 
                _coordinationResult.CultivationTestsPassed + 
                _coordinationResult.IntegrationTestsPassed;
                
            _coordinationResult.TotalTestsFailed = 
                _coordinationResult.CoreTestsFailed + 
                _coordinationResult.CultivationTestsFailed + 
                _coordinationResult.IntegrationTestsFailed;
            
            int totalTests = _coordinationResult.TotalTestsPassed + _coordinationResult.TotalTestsFailed;
            _coordinationResult.OverallSuccessRate = totalTests > 0 ? 
                (float)_coordinationResult.TotalTestsPassed / totalTests * 100f : 0f;
            
            LogCoordination($"Final Analysis Complete - Overall Status: {_coordinationResult.OverallStatus}");
            LogCoordination($"Success Rate: {_coordinationResult.OverallSuccessRate:F1}%");
            
            yield return null;
        }
        
        private void ChangePhase(TestPhaseStatus newPhase)
        {
            _currentPhase = newPhase;
            OnPhaseChanged?.Invoke(newPhase);
        }
        
        private void LogCoordination(string message)
        {
            if (_enableComprehensiveLogging)
            {
                Debug.Log($"[TestCoordinator] {message}");
            }
            
            _coordinationLog.Add($"{DateTime.Now:HH:mm:ss.fff} - {message}");
            OnLogMessage?.Invoke(message);
        }
        
        private string GenerateCoordinationSummary()
        {
            var summary = new System.Text.StringBuilder();
            
            summary.AppendLine("=== PROJECT CHIMERA CULTIVATION SYSTEM TEST COORDINATION SUMMARY ===");
            summary.AppendLine($"Coordination Duration: {_coordinationResult.TotalDuration:F2} seconds");
            summary.AppendLine($"Overall Status: {_coordinationResult.OverallStatus}");
            summary.AppendLine($"Overall Success Rate: {_coordinationResult.OverallSuccessRate:F1}%");
            summary.AppendLine();
            
            summary.AppendLine("PHASE RESULTS:");
            if (_coordinationResult.ValidationResult != null)
            {
                summary.AppendLine($"• Validation: {_coordinationResult.ValidationResult.OverallStatus} ({_coordinationResult.ValidationResult.SuccessRate:F1}%)");
            }
            summary.AppendLine($"• Core Tests: {_coordinationResult.CoreTestsStatus} ({_coordinationResult.CoreTestsPassed}P/{_coordinationResult.CoreTestsFailed}F)");
            summary.AppendLine($"• Cultivation Tests: {_coordinationResult.CultivationTestsStatus} ({_coordinationResult.CultivationTestsPassed}P/{_coordinationResult.CultivationTestsFailed}F)");
            summary.AppendLine($"• Integration Tests: {_coordinationResult.IntegrationTestsStatus} ({_coordinationResult.IntegrationTestsPassed}P/{_coordinationResult.IntegrationTestsFailed}F)");
            summary.AppendLine();
            
            summary.AppendLine($"TOTALS: {_coordinationResult.TotalTestsPassed} Passed, {_coordinationResult.TotalTestsFailed} Failed");
            
            return summary.ToString();
        }
        
        // GUI Display
        private void OnGUI()
        {
            if (!_showRealTimeResults) return;
            
            GUILayout.BeginArea(new Rect(10, Screen.height - 150, 500, 140));
            GUILayout.BeginVertical("box");
            
            GUILayout.Label("Test Coordination Status", GUI.skin.box);
            GUILayout.Label($"Phase: {_currentPhase}");
            
            if (_coordinationRunning)
            {
                float elapsed = Time.time - _coordinationStartTime;
                GUILayout.Label($"Running: {elapsed:F1}s elapsed");
            }
            else if (_coordinationResult != null)
            {
                GUILayout.Label($"Status: {_coordinationResult.OverallStatus}");
                GUILayout.Label($"Success Rate: {_coordinationResult.OverallSuccessRate:F1}%");
            }
            
            if (GUILayout.Button("Start Coordination"))
            {
                StartCoordinatedTesting();
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        
        [ContextMenu("Start Coordinated Testing")]
        public void StartCoordinatedTestingManual()
        {
            StartCoordinatedTesting();
        }
        
        [ContextMenu("Generate Summary Report")]
        public void GenerateSummaryReport()
        {
            if (_coordinationResult != null)
            {
                string summary = GenerateCoordinationSummary();
                Debug.Log(summary);
            }
            else
            {
                Debug.Log("No coordination results available");
            }
        }
        
        // Public properties
        public TestCoordinationResult CoordinationResult => _coordinationResult;
        public bool CoordinationRunning => _coordinationRunning;
        public TestPhaseStatus CurrentPhase => _currentPhase;
    }
    
    // Data structures
    [System.Serializable]
    public class TestCoordinationResult
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public float TotalDuration;
        public string UnityVersion;
        public string Platform;
        public string ErrorMessage;
        
        public ValidationResult ValidationResult;
        
        public TestStatus CoreTestsStatus = TestStatus.NotRun;
        public int CoreTestsPassed;
        public int CoreTestsFailed;
        
        public TestStatus CultivationTestsStatus = TestStatus.NotRun;
        public int CultivationTestsPassed;
        public int CultivationTestsFailed;
        
        public TestStatus IntegrationTestsStatus = TestStatus.NotRun;
        public int IntegrationTestsPassed;
        public int IntegrationTestsFailed;
        
        public TestStatus OverallStatus = TestStatus.NotRun;
        public int TotalTestsPassed;
        public int TotalTestsFailed;
        public float OverallSuccessRate;
    }
    
    public enum TestPhaseStatus
    {
        NotStarted,
        Validation,
        CoreTesting,
        CultivationTesting,
        Integration,
        Analysis,
        Completed
    }
    
    public enum TestStatus
    {
        NotRun,
        Passed,
        PartialPass,
        Failed,
        Timeout
    }
} 