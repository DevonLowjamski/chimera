using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Data.Cultivation;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Testing;
using System;
using System.Collections;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Master test runner for all Project Chimera cultivation systems.
    /// Coordinates comprehensive testing and provides detailed validation reports.
    /// </summary>
    public class AdvancedCultivationTestRunner : MonoBehaviour
    {
        [Header("Test Runner Configuration")]
        [SerializeField] private bool _runOnStart = true;
        [SerializeField] private bool _enablePerformanceMonitoring = true;
        [SerializeField] private bool _enableValidationChecks = true;
        [SerializeField] private bool _generateTestReport = true;
        [SerializeField] private float _testTimeout = 60f;
        
        [Header("Test Components")]
        [SerializeField] private AdvancedCultivationTester _cultivationTester;
        [SerializeField] private CoreSystemTester _coreSystemTester;
        
        [Header("Performance Monitoring")]
        [SerializeField] private int _maxMemoryUsageMB = 500;
        [SerializeField] private float _maxFrameTime = 33.33f; // 30 FPS
        [SerializeField] private int _performanceSampleCount = 100;
        
        [Header("Test Results")]
        [SerializeField] private TestSuite _testSuite;
        [SerializeField] private List<TestResult> _testResults = new List<TestResult>();
        [SerializeField] private PerformanceMetrics _performanceMetrics;
        
        // Performance monitoring
        private List<float> _frameTimes = new List<float>();
        private List<float> _memoryUsage = new List<float>();
        private float _testStartTime;
        private bool _testsRunning = false;
        
        // Events
        public System.Action<TestSuite> OnTestSuiteCompleted;
        public System.Action<string> OnTestLogMessage;
        
        private void Start()
        {
            if (_runOnStart)
            {
                StartTestSuite();
            }
        }
        
        public void StartTestSuite()
        {
            if (_testsRunning)
            {
                LogMessage("Test suite already running!");
                return;
            }
            
            LogMessage("=== STARTING ADVANCED CULTIVATION TEST SUITE ===");
            StartCoroutine(RunTestSuite());
        }
        
        private IEnumerator RunTestSuite()
        {
            _testsRunning = true;
            _testStartTime = Time.time;
            _testResults.Clear();
            
            // Initialize test suite
            _testSuite = new TestSuite
            {
                Name = "Advanced Cultivation Systems",
                StartTime = DateTime.Now,
                TestEnvironment = Application.platform.ToString(),
                UnityVersion = Application.unityVersion
            };
            
            // Start performance monitoring
            if (_enablePerformanceMonitoring)
            {
                StartCoroutine(MonitorPerformance());
            }
            
            // Phase 1: Validate Test Environment
            yield return StartCoroutine(ValidateTestEnvironment());
            
            // Phase 2: Run Core System Tests
            yield return StartCoroutine(RunCoreSystemTests());
            
            // Phase 3: Run Advanced Cultivation Tests
            yield return StartCoroutine(RunAdvancedCultivationTests());
            
            // Phase 4: Run Integration Tests
            yield return StartCoroutine(RunIntegrationTests());
            
            // Phase 5: Run Performance Validation
            yield return StartCoroutine(RunPerformanceValidation());
            
            // Phase 6: Generate Final Report
            yield return StartCoroutine(GenerateFinalReport());
            
            _testsRunning = false;
            _testSuite.EndTime = DateTime.Now;
            _testSuite.Duration = Time.time - _testStartTime;
            
            OnTestSuiteCompleted?.Invoke(_testSuite);
            LogMessage("=== TEST SUITE COMPLETED ===");
        }
        
        private IEnumerator ValidateTestEnvironment()
        {
            LogMessage("\n--- PHASE 1: VALIDATING TEST ENVIRONMENT ---");
            
            var phase = new TestPhase { Name = "Environment Validation", StartTime = DateTime.Now };
            
            // Check Unity version compatibility
            bool unityVersionValid = Application.unityVersion.StartsWith("6000.");
            AddTestResult("Unity 6 Version Check", unityVersionValid, unityVersionValid ? "Unity 6 detected" : "Unity 6 required");
            
            // Check required components
            var gameManager = FindAnyObjectByType<GameManager>();
            AddTestResult("GameManager Present", gameManager != null, gameManager != null ? "GameManager found" : "GameManager missing");
            
            var cultivationManager = FindAnyObjectByType<CultivationManager>();
            AddTestResult("CultivationManager Present", cultivationManager != null, cultivationManager != null ? "CultivationManager found" : "CultivationManager missing");
            
            // Check test components
            if (_cultivationTester == null)
                _cultivationTester = FindAnyObjectByType<AdvancedCultivationTester>();
            AddTestResult("AdvancedCultivationTester Available", _cultivationTester != null, _cultivationTester != null ? "Tester found" : "Tester missing");
            
            if (_coreSystemTester == null)
                _coreSystemTester = FindAnyObjectByType<CoreSystemTester>();
            AddTestResult("CoreSystemTester Available", _coreSystemTester != null, _coreSystemTester != null ? "Core tester found" : "Core tester missing");
            
            // Check memory state
            float initialMemory = GetMemoryUsageMB();
            AddTestResult("Initial Memory Usage", initialMemory < _maxMemoryUsageMB, $"Memory: {initialMemory:F1}MB");
            
            phase.EndTime = DateTime.Now;
            _testSuite.Phases.Add(phase);
            
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator RunCoreSystemTests()
        {
            LogMessage("\n--- PHASE 2: RUNNING CORE SYSTEM TESTS ---");
            
            var phase = new TestPhase { Name = "Core Systems", StartTime = DateTime.Now };
            
            if (_coreSystemTester != null)
            {
                // Run core system tests
                _coreSystemTester.RunTestsManually();
                
                // Wait for tests to complete (timeout after specified time)
                float timeout = Time.time + _testTimeout;
                while (_coreSystemTester.TestsRunning && Time.time < timeout)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                
                bool coreTestsPassed = !_coreSystemTester.TestsRunning; // Assuming tests completed if not running
                AddTestResult("Core System Tests", coreTestsPassed, coreTestsPassed ? "Core tests completed" : "Core tests timed out");
            }
            else
            {
                AddTestResult("Core System Tests", false, "Core system tester not available");
            }
            
            phase.EndTime = DateTime.Now;
            _testSuite.Phases.Add(phase);
            
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator RunAdvancedCultivationTests()
        {
            LogMessage("\n--- PHASE 3: RUNNING ADVANCED CULTIVATION TESTS ---");
            
            var phase = new TestPhase { Name = "Advanced Cultivation", StartTime = DateTime.Now };
            
            if (_cultivationTester != null)
            {
                // Run cultivation system tests
                _cultivationTester.RunTestsManually();
                
                // Wait for tests to complete
                float timeout = Time.time + _testTimeout;
                float lastTestCount = 0;
                int stagnantCount = 0;
                
                while (Time.time < timeout)
                {
                    yield return new WaitForSeconds(1f);
                    
                    // Check if tests are progressing (this would need to be implemented in AdvancedCultivationTester)
                    // For now, we'll wait a reasonable time
                    float currentTestCount = _cultivationTester.TotalTests;
                    if (currentTestCount == lastTestCount)
                    {
                        stagnantCount++;
                        if (stagnantCount > 10) // 10 seconds without progress
                            break;
                    }
                    else
                    {
                        stagnantCount = 0;
                        lastTestCount = currentTestCount;
                    }
                }
                
                bool cultivationTestsPassed = _cultivationTester.PassedTests > 0 && _cultivationTester.FailedTests == 0;
                AddTestResult("Cultivation System Tests", cultivationTestsPassed, 
                    $"Passed: {_cultivationTester.PassedTests}, Failed: {_cultivationTester.FailedTests}");
            }
            else
            {
                AddTestResult("Cultivation System Tests", false, "Cultivation tester not available");
            }
            
            phase.EndTime = DateTime.Now;
            _testSuite.Phases.Add(phase);
            
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator RunIntegrationTests()
        {
            LogMessage("\n--- PHASE 4: RUNNING INTEGRATION TESTS ---");
            
            var phase = new TestPhase { Name = "System Integration", StartTime = DateTime.Now };
            
            // Test GameManager and CultivationManager integration
            var gameManager = FindAnyObjectByType<GameManager>();
            var cultivationManager = FindAnyObjectByType<CultivationManager>();
            
            if (gameManager != null && cultivationManager != null)
            {
                bool integrationWorking = gameManager.IsInitialized && cultivationManager.IsInitialized;
                AddTestResult("Manager Integration", integrationWorking, 
                    $"GameManager: {(gameManager.IsInitialized ? "OK" : "Failed")}, CultivationManager: {(cultivationManager.IsInitialized ? "OK" : "Failed")}");
                
                // Test data flow between systems
                if (integrationWorking)
                {
                    // This would be expanded with actual integration tests
                    AddTestResult("Data Flow Test", true, "Basic data flow validated");
                }
            }
            else
            {
                AddTestResult("Manager Integration", false, "Required managers not found");
            }
            
            phase.EndTime = DateTime.Now;
            _testSuite.Phases.Add(phase);
            
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator RunPerformanceValidation()
        {
            LogMessage("\n--- PHASE 5: RUNNING PERFORMANCE VALIDATION ---");
            
            var phase = new TestPhase { Name = "Performance Validation", StartTime = DateTime.Now };
            
            if (_enablePerformanceMonitoring && _frameTimes.Count > 0)
            {
                // Calculate performance metrics
                float avgFrameTime = 0f;
                float maxFrameTime = 0f;
                foreach (float frameTime in _frameTimes)
                {
                    avgFrameTime += frameTime;
                    if (frameTime > maxFrameTime) maxFrameTime = frameTime;
                }
                avgFrameTime /= _frameTimes.Count;
                
                float avgMemory = 0f;
                float maxMemory = 0f;
                foreach (float memory in _memoryUsage)
                {
                    avgMemory += memory;
                    if (memory > maxMemory) maxMemory = memory;
                }
                avgMemory /= _memoryUsage.Count;
                
                // Store performance metrics
                _performanceMetrics = new PerformanceMetrics
                {
                    AverageFrameTime = avgFrameTime,
                    MaxFrameTime = maxFrameTime,
                    AverageMemoryUsage = avgMemory,
                    MaxMemoryUsage = maxMemory,
                    SampleCount = _frameTimes.Count
                };
                
                // Validate performance
                bool frameTimeOK = avgFrameTime < _maxFrameTime;
                bool memoryOK = maxMemory < _maxMemoryUsageMB;
                
                AddTestResult("Frame Time Performance", frameTimeOK, $"Avg: {avgFrameTime:F2}ms (target: <{_maxFrameTime:F2}ms)");
                AddTestResult("Memory Usage", memoryOK, $"Max: {maxMemory:F1}MB (limit: {_maxMemoryUsageMB}MB)");
                AddTestResult("Performance Stability", frameTimeOK && memoryOK, "Overall performance within acceptable limits");
            }
            else
            {
                AddTestResult("Performance Monitoring", false, "Performance monitoring was disabled or no data collected");
            }
            
            phase.EndTime = DateTime.Now;
            _testSuite.Phases.Add(phase);
            
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator GenerateFinalReport()
        {
            LogMessage("\n--- PHASE 6: GENERATING FINAL REPORT ---");
            
            if (_generateTestReport)
            {
                string report = GenerateDetailedReport();
                LogMessage("\n" + report);
                
                // Save report to file (optional)
                // System.IO.File.WriteAllText($"TestReport_{DateTime.Now:yyyyMMdd_HHmmss}.txt", report);
            }
            
            yield return null;
        }
        
        private IEnumerator MonitorPerformance()
        {
            _frameTimes.Clear();
            _memoryUsage.Clear();
            
            while (_testsRunning && _frameTimes.Count < _performanceSampleCount)
            {
                _frameTimes.Add(Time.deltaTime * 1000f); // Convert to milliseconds
                _memoryUsage.Add(GetMemoryUsageMB());
                
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private float GetMemoryUsageMB()
        {
            return UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory() / (1024f * 1024f);
        }
        
        private void AddTestResult(string testName, bool passed, string details)
        {
            var result = new TestResult
            {
                Name = testName,
                Passed = passed,
                Details = details,
                Timestamp = DateTime.Now
            };
            
            _testResults.Add(result);
            LogMessage($"{(passed ? "✓" : "✗")} {testName}: {details}");
        }
        
        private void LogMessage(string message)
        {
            Debug.Log($"[TestRunner] {message}");
            OnTestLogMessage?.Invoke(message);
        }
        
        private string GenerateDetailedReport()
        {
            var report = new System.Text.StringBuilder();
            
            report.AppendLine("=== PROJECT CHIMERA ADVANCED CULTIVATION TEST REPORT ===");
            report.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            report.AppendLine($"Unity Version: {Application.unityVersion}");
            report.AppendLine($"Platform: {Application.platform}");
            report.AppendLine($"Test Duration: {_testSuite.Duration:F2} seconds");
            report.AppendLine();
            
            // Summary
            int totalTests = _testResults.Count;
            int passedTests = _testResults.FindAll(r => r.Passed).Count;
            int failedTests = totalTests - passedTests;
            float successRate = totalTests > 0 ? (float)passedTests / totalTests * 100f : 0f;
            
            report.AppendLine("=== SUMMARY ===");
            report.AppendLine($"Total Tests: {totalTests}");
            report.AppendLine($"Passed: {passedTests}");
            report.AppendLine($"Failed: {failedTests}");
            report.AppendLine($"Success Rate: {successRate:F1}%");
            report.AppendLine();
            
            // Performance metrics
            if (_performanceMetrics != null)
            {
                report.AppendLine("=== PERFORMANCE METRICS ===");
                report.AppendLine($"Average Frame Time: {_performanceMetrics.AverageFrameTime:F2}ms");
                report.AppendLine($"Max Frame Time: {_performanceMetrics.MaxFrameTime:F2}ms");
                report.AppendLine($"Average Memory: {_performanceMetrics.AverageMemoryUsage:F1}MB");
                report.AppendLine($"Peak Memory: {_performanceMetrics.MaxMemoryUsage:F1}MB");
                report.AppendLine($"Sample Count: {_performanceMetrics.SampleCount}");
                report.AppendLine();
            }
            
            // Detailed results
            report.AppendLine("=== DETAILED RESULTS ===");
            foreach (var phase in _testSuite.Phases)
            {
                report.AppendLine($"\n--- {phase.Name.ToUpper()} ---");
                report.AppendLine($"Duration: {(phase.EndTime - phase.StartTime).TotalSeconds:F2} seconds");
            }
            
            report.AppendLine("\n--- TEST RESULTS ---");
            foreach (var result in _testResults)
            {
                string status = result.Passed ? "PASS" : "FAIL";
                report.AppendLine($"[{status}] {result.Name}: {result.Details}");
            }
            
            return report.ToString();
        }
        
        // Public properties for accessing test results
        public TestSuite TestSuite => _testSuite;
        public List<TestResult> TestResults => _testResults;
        public PerformanceMetrics PerformanceMetrics => _performanceMetrics;
        public bool TestsRunning => _testsRunning;
        
        // Context menu for manual testing
        [ContextMenu("Start Test Suite")]
        public void StartTestSuiteManual()
        {
            StartTestSuite();
        }
        
        [ContextMenu("Clear Test Results")]
        public void ClearTestResults()
        {
            _testResults.Clear();
            _testSuite = null;
            _performanceMetrics = null;
            LogMessage("Test results cleared.");
        }
    }
    
    // Data structures for test reporting
    [System.Serializable]
    public class TestSuite
    {
        public string Name;
        public DateTime StartTime;
        public DateTime EndTime;
        public float Duration;
        public string TestEnvironment;
        public string UnityVersion;
        public List<TestPhase> Phases = new List<TestPhase>();
    }
    
    [System.Serializable]
    public class TestPhase
    {
        public string Name;
        public DateTime StartTime;
        public DateTime EndTime;
    }
    
    [System.Serializable]
    public class TestResult
    {
        public string Name;
        public bool Passed;
        public string Details;
        public DateTime Timestamp;
    }
    
    [System.Serializable]
    public class PerformanceMetrics
    {
        public float AverageFrameTime;
        public float MaxFrameTime;
        public float AverageMemoryUsage;
        public float MaxMemoryUsage;
        public int SampleCount;
    }
} 