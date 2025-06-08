using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Testing.Systems;
using ProjectChimera.Testing.Integration;
using ProjectChimera.Testing.Performance;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Automated test manager for Project Chimera that orchestrates all testing systems
    /// and provides comprehensive automated execution with detailed reporting.
    /// </summary>
    public class AutomatedTestManager : MonoBehaviour
    {
        [Header("Automation Configuration")]
        [SerializeField] private bool _runFullSuiteOnStart = false;
        [SerializeField] private bool _enableScheduledTests = false;
        [SerializeField] private float _scheduledTestInterval = 3600f; // 1 hour
        [SerializeField] private bool _enablePerformanceBaseline = true;
        [SerializeField] private bool _enableRegressionTesting = true;
        
        [Header("Test Suite Configuration")]
        [SerializeField] private TestSuiteConfig _testSuiteConfig = new TestSuiteConfig();
        [SerializeField] private bool _generateDetailedReports = true;
        [SerializeField] private bool _exportTestData = true;
        [SerializeField] private string _baselineDataPath = "TestBaselines";
        
        [Header("Test Components")]
        [SerializeField] private CultivationTestCoordinator _coordinator;
        [SerializeField] private AdvancedCultivationTestRunner _testRunner;
        [SerializeField] private CultivationIntegrationTests _integrationTests;
        [SerializeField] private CultivationPerformanceTests _performanceTests;
        [SerializeField] private TestReportGenerator _reportGenerator;
        
        [Header("Automation Results")]
        [SerializeField] private AutomatedTestSuite _currentTestSuite;
        [SerializeField] private List<AutomatedTestRun> _testHistory = new List<AutomatedTestRun>();
        [SerializeField] private TestExecutionStatus _executionStatus = TestExecutionStatus.Idle;
        
        // Automation state
        private bool _automationRunning = false;
        private float _nextScheduledTest = 0f;
        private TestBaselineData _performanceBaseline;
        
        // Events
        public System.Action<AutomatedTestSuite> OnAutomatedTestsComplete;
        public System.Action<TestExecutionStatus> OnExecutionStatusChanged;
        public System.Action<string> OnAutomationLogMessage;
        
        private void Start()
        {
            // Auto-find components if not assigned
            AutoDiscoverTestComponents();
            
            // Load performance baseline if available
            LoadPerformanceBaseline();
            
            if (_runFullSuiteOnStart)
            {
                StartCoroutine(DelayedAutomatedStart());
            }
            
            if (_enableScheduledTests)
            {
                _nextScheduledTest = Time.time + _scheduledTestInterval;
            }
        }
        
        private void Update()
        {
            // Handle scheduled tests
            if (_enableScheduledTests && !_automationRunning && Time.time >= _nextScheduledTest)
            {
                LogAutomation("Starting scheduled automated test run");
                StartAutomatedTestSuite();
                _nextScheduledTest = Time.time + _scheduledTestInterval;
            }
        }
        
        private IEnumerator DelayedAutomatedStart()
        {
            yield return new WaitForSeconds(10f); // Allow all systems to fully initialize
            StartAutomatedTestSuite();
        }
        
        public void StartAutomatedTestSuite()
        {
            if (_automationRunning)
            {
                LogAutomation("Automated test suite already running!");
                return;
            }
            
            LogAutomation("=== STARTING AUTOMATED CULTIVATION TEST SUITE ===");
            StartCoroutine(RunAutomatedTestSuite());
        }
        
        private IEnumerator RunAutomatedTestSuite()
        {
            _automationRunning = true;
            ChangeExecutionStatus(TestExecutionStatus.Running);
            
            // Initialize automated test suite
            _currentTestSuite = new AutomatedTestSuite
            {
                Id = Guid.NewGuid().ToString(),
                StartTime = DateTime.Now,
                TestEnvironment = Application.platform.ToString(),
                UnityVersion = Application.unityVersion,
                SceneName = SceneManager.GetActiveScene().name,
                Configuration = _testSuiteConfig
            };
            
            LogAutomation($"Test Suite ID: {_currentTestSuite.Id}");
            
            // Phase 1: Pre-Test System Validation
            yield return StartCoroutine(RunPreTestValidation());
            
            // Phase 2: Core System Tests
            if (_testSuiteConfig.EnableCoreTests)
            {
                yield return StartCoroutine(RunCoreTestPhase());
            }
            
            // Phase 3: Cultivation System Tests
            if (_testSuiteConfig.EnableCultivationTests)
            {
                yield return StartCoroutine(RunCultivationTestPhase());
            }
            
            // Phase 4: Integration Tests
            if (_testSuiteConfig.EnableIntegrationTests)
            {
                yield return StartCoroutine(RunIntegrationTestPhase());
            }
            
            // Phase 5: Performance Tests
            if (_testSuiteConfig.EnablePerformanceTests)
            {
                yield return StartCoroutine(RunPerformanceTestPhase());
            }
            
            // Phase 6: Regression Testing
            if (_enableRegressionTesting && _performanceBaseline != null)
            {
                yield return StartCoroutine(RunRegressionTestPhase());
            }
            
            // Phase 7: Report Generation
            yield return StartCoroutine(RunReportGenerationPhase());
            
            // Finalize test suite
            _currentTestSuite.EndTime = DateTime.Now;
            _currentTestSuite.TotalDuration = (float)(_currentTestSuite.EndTime - _currentTestSuite.StartTime).TotalSeconds;
            
            // Add to test history
            var testRun = new AutomatedTestRun
            {
                TestSuite = _currentTestSuite,
                Timestamp = DateTime.Now
            };
            _testHistory.Add(testRun);
            
            // Limit history size
            if (_testHistory.Count > 50)
            {
                _testHistory.RemoveAt(0);
            }
            
            // Update performance baseline if enabled
            if (_enablePerformanceBaseline && _currentTestSuite.PerformanceResults != null)
            {
                UpdatePerformanceBaseline();
            }
            
            _automationRunning = false;
            ChangeExecutionStatus(TestExecutionStatus.Completed);
            
            OnAutomatedTestsComplete?.Invoke(_currentTestSuite);
            LogAutomation("=== AUTOMATED TEST SUITE COMPLETED ===");
            LogAutomation(GenerateAutomationSummary());
        }
        
        private IEnumerator RunPreTestValidation()
        {
            LogAutomation("\n--- PHASE 1: PRE-TEST SYSTEM VALIDATION ---");
            
            _currentTestSuite.ValidationResults = new ValidationResult
            {
                StartTime = DateTime.Now
            };
            
            // Check Unity version compatibility
            bool unityVersionValid = Application.unityVersion.StartsWith("6000.") || Application.unityVersion.StartsWith("6.");
            LogAutomation($"Unity Version Check: {(unityVersionValid ? "PASS" : "FAIL")} - {Application.unityVersion}");
            
            // Check memory state
            float initialMemory = GetMemoryUsageMB();
            bool memoryStateGood = initialMemory < 200f; // Initial memory should be reasonable
            LogAutomation($"Initial Memory Check: {(memoryStateGood ? "PASS" : "FAIL")} - {initialMemory:F1}MB");
            
            // Check frame rate stability
            yield return StartCoroutine(CheckFrameRateStability());
            
            // Check system references
            bool allSystemsFound = CheckSystemReferences();
            LogAutomation($"System References Check: {(allSystemsFound ? "PASS" : "FAIL")}");
            
            _currentTestSuite.ValidationResults.OverallStatus = (unityVersionValid && memoryStateGood && allSystemsFound) 
                ? ValidationStatus.Passed 
                : ValidationStatus.Failed;
            _currentTestSuite.ValidationResults.EndTime = DateTime.Now;
            
            if (_currentTestSuite.ValidationResults.OverallStatus == ValidationStatus.Failed)
            {
                LogAutomation("CRITICAL: Pre-test validation failed - aborting automated test suite");
                _currentTestSuite.ExecutionStatus = TestExecutionStatus.Failed;
                yield break;
            }
            
            yield return new WaitForSeconds(1f);
        }
        
        private IEnumerator CheckFrameRateStability()
        {
            LogAutomation("Checking frame rate stability...");
            
            var frameTimes = new List<float>();
            float checkDuration = 5f;
            float startTime = Time.realtimeSinceStartup;
            
            while (Time.realtimeSinceStartup - startTime < checkDuration)
            {
                frameTimes.Add(Time.deltaTime * 1000f);
                yield return null;
            }
            
            float avgFrameTime = CalculateAverage(frameTimes);
            float maxFrameTime = CalculateMax(frameTimes);
            
            bool frameRateStable = avgFrameTime < 20f && maxFrameTime < 50f; // Reasonable thresholds
            LogAutomation($"Frame Rate Stability: {(frameRateStable ? "PASS" : "FAIL")} - Avg: {avgFrameTime:F2}ms, Max: {maxFrameTime:F2}ms");
        }
        
        private bool CheckSystemReferences()
        {
            bool allFound = true;
            
            var gameManager = FindAnyObjectByType<GameManager>();
            LogAutomation($"GameManager: {(gameManager != null ? "FOUND" : "MISSING")}");
            if (gameManager == null) allFound = false;
            
            var cultivationManager = FindAnyObjectByType<CultivationManager>();
            LogAutomation($"CultivationManager: {(cultivationManager != null ? "FOUND" : "MISSING")}");
            if (cultivationManager == null) allFound = false;
            
            return allFound;
        }
        
        private IEnumerator RunCoreTestPhase()
        {
            LogAutomation("\n--- PHASE 2: CORE SYSTEM TESTS ---");
            
            if (_testRunner != null)
            {
                _testRunner.StartTestSuite();
                
                // Wait for tests to complete with timeout
                float timeout = Time.time + 300f; // 5 minute timeout
                while (_testRunner.TestsRunning && Time.time < timeout)
                {
                    yield return new WaitForSeconds(1f);
                }
                
                _currentTestSuite.CoreTestResults = _testRunner.TestSuite;
                LogAutomation($"Core Tests: {(_testRunner.TestSuite != null ? "COMPLETED" : "TIMEOUT")}");
            }
            else
            {
                LogAutomation("Core test runner not available");
            }
        }
        
        private IEnumerator RunCultivationTestPhase()
        {
            LogAutomation("\n--- PHASE 3: CULTIVATION SYSTEM TESTS ---");
            
            if (_coordinator != null)
            {
                _coordinator.StartCoordinatedTesting();
                
                // Wait for coordination to complete
                float timeout = Time.time + 600f; // 10 minute timeout
                while (_coordinator.CoordinationRunning && Time.time < timeout)
                {
                    yield return new WaitForSeconds(2f);
                }
                
                _currentTestSuite.CoordinationResults = _coordinator.CoordinationResult;
                LogAutomation($"Cultivation Tests: {(_coordinator.CoordinationResult != null ? "COMPLETED" : "TIMEOUT")}");
            }
            else
            {
                LogAutomation("Cultivation test coordinator not available");
            }
        }
        
        private IEnumerator RunIntegrationTestPhase()
        {
            LogAutomation("\n--- PHASE 4: INTEGRATION TESTS ---");
            
            if (_integrationTests != null)
            {
                _integrationTests.StartIntegrationTests();
                
                // Wait for integration tests to complete
                float timeout = Time.time + 300f; // 5 minute timeout
                while (_integrationTests.TestsRunning && Time.time < timeout)
                {
                    yield return new WaitForSeconds(1f);
                }
                
                _currentTestSuite.IntegrationResults = _integrationTests.TestSuite;
                LogAutomation($"Integration Tests: {(_integrationTests.TestSuite != null ? "COMPLETED" : "TIMEOUT")}");
            }
            else
            {
                LogAutomation("Integration test component not available");
            }
        }
        
        private IEnumerator RunPerformanceTestPhase()
        {
            LogAutomation("\n--- PHASE 5: PERFORMANCE TESTS ---");
            
            if (_performanceTests != null)
            {
                _performanceTests.StartPerformanceTests();
                
                // Wait for performance tests to complete
                float timeout = Time.time + 600f; // 10 minute timeout
                while (_performanceTests.TestsRunning && Time.time < timeout)
                {
                    yield return new WaitForSeconds(2f);
                }
                
                _currentTestSuite.PerformanceResults = _performanceTests.TestSuite;
                LogAutomation($"Performance Tests: {(_performanceTests.TestSuite != null ? "COMPLETED" : "TIMEOUT")}");
            }
            else
            {
                LogAutomation("Performance test component not available");
            }
        }
        
        private IEnumerator RunRegressionTestPhase()
        {
            LogAutomation("\n--- PHASE 6: REGRESSION TESTING ---");
            
            var regressionResults = new RegressionTestResults
            {
                StartTime = DateTime.Now,
                BaselineData = _performanceBaseline
            };
            
            if (_currentTestSuite.PerformanceResults != null && _performanceBaseline != null)
            {
                // Compare current performance with baseline
                var currentMetrics = _currentTestSuite.PerformanceResults.OverallMetrics;
                var baselineMetrics = _performanceBaseline.BaselineMetrics;
                
                // Frame time regression check
                float frameTimeIncrease = (currentMetrics.AverageFrameTime - baselineMetrics.AverageFrameTime) / baselineMetrics.AverageFrameTime;
                bool frameTimeRegression = frameTimeIncrease > 0.1f; // 10% regression threshold
                
                // Memory usage regression check
                float memoryIncrease = (currentMetrics.AverageMemoryUsage - baselineMetrics.AverageMemoryUsage) / baselineMetrics.AverageMemoryUsage;
                bool memoryRegression = memoryIncrease > 0.15f; // 15% regression threshold
                
                regressionResults.FrameTimeRegression = frameTimeRegression;
                regressionResults.MemoryRegression = memoryRegression;
                regressionResults.OverallRegression = frameTimeRegression || memoryRegression;
                
                LogAutomation($"Frame Time Regression: {(frameTimeRegression ? "DETECTED" : "NONE")} ({frameTimeIncrease:P1})");
                LogAutomation($"Memory Regression: {(memoryRegression ? "DETECTED" : "NONE")} ({memoryIncrease:P1})");
            }
            else
            {
                LogAutomation("No baseline available for regression testing");
                regressionResults.OverallRegression = false;
            }
            
            regressionResults.EndTime = DateTime.Now;
            _currentTestSuite.RegressionResults = regressionResults;
            
            yield return new WaitForSeconds(1f);
        }
        
        private IEnumerator RunReportGenerationPhase()
        {
            LogAutomation("\n--- PHASE 7: REPORT GENERATION ---");
            
            if (_generateDetailedReports && _reportGenerator != null)
            {
                _reportGenerator.GenerateComprehensiveReport();
                LogAutomation("Comprehensive reports generated");
            }
            
            if (_exportTestData)
            {
                ExportTestData();
                LogAutomation("Test data exported");
            }
            
            yield return new WaitForSeconds(1f);
        }
        
        private void AutoDiscoverTestComponents()
        {
            if (_coordinator == null) _coordinator = FindAnyObjectByType<CultivationTestCoordinator>();
            if (_testRunner == null) _testRunner = FindAnyObjectByType<AdvancedCultivationTestRunner>();
            if (_integrationTests == null) _integrationTests = FindAnyObjectByType<CultivationIntegrationTests>();
            if (_performanceTests == null) _performanceTests = FindAnyObjectByType<CultivationPerformanceTests>();
            if (_reportGenerator == null) _reportGenerator = FindAnyObjectByType<TestReportGenerator>();
        }
        
        private void LoadPerformanceBaseline()
        {
            // Implementation for loading performance baseline data
            // This would typically load from PlayerPrefs or file system
            LogAutomation("Performance baseline loading not yet implemented");
        }
        
        private void UpdatePerformanceBaseline()
        {
            if (_currentTestSuite.PerformanceResults?.OverallMetrics != null)
            {
                _performanceBaseline = new TestBaselineData
                {
                    CreatedAt = DateTime.Now,
                    UnityVersion = Application.unityVersion,
                    Platform = Application.platform.ToString(),
                    BaselineMetrics = _currentTestSuite.PerformanceResults.OverallMetrics
                };
                
                LogAutomation("Performance baseline updated");
            }
        }
        
        private void ExportTestData()
        {
            // Implementation for exporting test data to file system
            LogAutomation("Test data export not yet implemented");
        }
        
        private void ChangeExecutionStatus(TestExecutionStatus newStatus)
        {
            _executionStatus = newStatus;
            OnExecutionStatusChanged?.Invoke(newStatus);
        }
        
        private void LogAutomation(string message)
        {
            string logMessage = $"[AutoTest] {message}";
            Debug.Log(logMessage);
            OnAutomationLogMessage?.Invoke(logMessage);
        }
        
        private float GetMemoryUsageMB()
        {
            return UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory() / (1024f * 1024f);
        }
        
        private float CalculateAverage(List<float> values)
        {
            if (values.Count == 0) return 0f;
            float sum = 0f;
            foreach (float value in values)
                sum += value;
            return sum / values.Count;
        }
        
        private float CalculateMax(List<float> values)
        {
            if (values.Count == 0) return 0f;
            float max = values[0];
            foreach (float value in values)
                if (value > max) max = value;
            return max;
        }
        
        private string GenerateAutomationSummary()
        {
            if (_currentTestSuite == null) return "No test suite data available";
            
            var summary = new System.Text.StringBuilder();
            summary.AppendLine($"Test Suite ID: {_currentTestSuite.Id}");
            summary.AppendLine($"Duration: {_currentTestSuite.TotalDuration:F1} seconds");
            summary.AppendLine($"Status: {_currentTestSuite.ExecutionStatus}");
            
            if (_currentTestSuite.ValidationResults != null)
                summary.AppendLine($"Validation: {_currentTestSuite.ValidationResults.OverallStatus}");
            
            if (_currentTestSuite.CoreTestResults != null)
                summary.AppendLine($"Core Tests: {_currentTestSuite.CoreTestResults.Name} - Duration: {_currentTestSuite.CoreTestResults.Duration:F1}s");
            
            if (_currentTestSuite.CoordinationResults != null)
                summary.AppendLine($"Cultivation Tests: {_currentTestSuite.CoordinationResults.OverallStatus}");
            
            if (_currentTestSuite.IntegrationResults != null)
                summary.AppendLine($"Integration Tests: {_currentTestSuite.IntegrationResults.PassedTests}/{_currentTestSuite.IntegrationResults.TotalTests}");
            
            if (_currentTestSuite.PerformanceResults != null)
                summary.AppendLine($"Performance Tests: {_currentTestSuite.PerformanceResults.PassedTests}/{_currentTestSuite.PerformanceResults.TotalTests}");
            
            if (_currentTestSuite.RegressionResults != null)
                summary.AppendLine($"Regression Detected: {_currentTestSuite.RegressionResults.OverallRegression}");
            
            return summary.ToString();
        }
        
        // Public properties
        public AutomatedTestSuite CurrentTestSuite => _currentTestSuite;
        public List<AutomatedTestRun> TestHistory => _testHistory;
        public TestExecutionStatus ExecutionStatus => _executionStatus;
        public bool AutomationRunning => _automationRunning;
        
        // Context menu actions
        [ContextMenu("Start Automated Test Suite")]
        public void StartAutomatedTestSuiteManual()
        {
            StartAutomatedTestSuite();
        }
        
        [ContextMenu("Clear Test History")]
        public void ClearTestHistory()
        {
            _testHistory.Clear();
            LogAutomation("Test history cleared");
        }
        
        [ContextMenu("Generate Summary Report")]
        public void GenerateSummaryReportManual()
        {
            LogAutomation(GenerateAutomationSummary());
        }
    }
    
    // Supporting data structures
    [System.Serializable]
    public class TestSuiteConfig
    {
        public bool EnableCoreTests = true;
        public bool EnableCultivationTests = true;
        public bool EnableIntegrationTests = true;
        public bool EnablePerformanceTests = true;
        public float TestTimeout = 300f;
        public bool EnableDetailedLogging = true;
    }
    
    [System.Serializable]
    public class AutomatedTestSuite
    {
        public string Id;
        public DateTime StartTime;
        public DateTime EndTime;
        public float TotalDuration;
        public string TestEnvironment;
        public string UnityVersion;
        public string SceneName;
        public TestSuiteConfig Configuration;
        public TestExecutionStatus ExecutionStatus = TestExecutionStatus.Running;
        public string ErrorMessage;
        
        // Test results
        public ValidationResult ValidationResults;
        public TestSuite CoreTestResults;
        public TestCoordinationResult CoordinationResults;
        public IntegrationTestSuite IntegrationResults;
        public PerformanceTestSuite PerformanceResults;
        public RegressionTestResults RegressionResults;
    }
    
    [System.Serializable]
    public class AutomatedTestRun
    {
        public AutomatedTestSuite TestSuite;
        public DateTime Timestamp;
    }
    
    [System.Serializable]
    public class TestBaselineData
    {
        public DateTime CreatedAt;
        public string UnityVersion;
        public string Platform;
        public ProjectChimera.Testing.Performance.PerformanceMetrics BaselineMetrics;
    }
    
    [System.Serializable]
    public class RegressionTestResults
    {
        public DateTime StartTime;
        public DateTime EndTime;
        public TestBaselineData BaselineData;
        public bool FrameTimeRegression;
        public bool MemoryRegression;
        public bool OverallRegression;
    }
    
    public enum TestExecutionStatus
    {
        Idle,
        Running,
        Completed,
        Failed,
        Timeout
    }
} 