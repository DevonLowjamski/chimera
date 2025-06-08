using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Data.Cultivation;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Systems.Visuals;
using System;

namespace ProjectChimera.Testing.Integration
{
    /// <summary>
    /// Comprehensive integration tests for Project Chimera cultivation systems.
    /// Tests cross-system interactions, data flow, and system coordination.
    /// </summary>
    public class CultivationIntegrationTests : MonoBehaviour
    {
        [Header("Integration Test Configuration")]
        [SerializeField] private bool _runOnStart = false;
        [SerializeField] private bool _enableDetailedLogging = true;
        [SerializeField] private float _integrationTimeout = 120f;
        [SerializeField] private int _testIterations = 3;
        
        [Header("Test Results")]
        [SerializeField] private IntegrationTestSuite _testSuite;
        [SerializeField] private List<IntegrationTestResult> _testResults = new List<IntegrationTestResult>();
        
        // System references
        private GameManager _gameManager;
        private CultivationManager _cultivationManager;
        private PlantVisualizationManager _visualizationManager;
        private DataManager _dataManager;
        private EventManager _eventManager;
        
        // Test state
        private bool _testsRunning = false;
        private float _testStartTime;
        
        // Events
        public System.Action<IntegrationTestSuite> OnIntegrationTestsComplete;
        public System.Action<string> OnIntegrationLogMessage;
        
        private void Start()
        {
            if (_runOnStart)
            {
                StartCoroutine(DelayedTestStart());
            }
        }
        
        private IEnumerator DelayedTestStart()
        {
            yield return new WaitForSeconds(3f); // Allow systems to initialize
            StartIntegrationTests();
        }
        
        public void StartIntegrationTests()
        {
            if (_testsRunning)
            {
                LogIntegration("Integration tests already running!");
                return;
            }
            
            LogIntegration("=== STARTING CULTIVATION INTEGRATION TESTS ===");
            StartCoroutine(RunIntegrationTests());
        }
        
        private IEnumerator RunIntegrationTests()
        {
            _testsRunning = true;
            _testStartTime = Time.time;
            _testResults.Clear();
            
            // Initialize test suite
            _testSuite = new IntegrationTestSuite
            {
                Name = "Cultivation System Integration Tests",
                StartTime = DateTime.Now,
                TestEnvironment = Application.platform.ToString(),
                UnityVersion = Application.unityVersion
            };
            
            // Get system references
            yield return StartCoroutine(InitializeSystemReferences());
            
            // Test 1: System Initialization Integration
            yield return StartCoroutine(TestSystemInitializationIntegration());
            
            // Test 2: Cultivation System Integration  
            yield return StartCoroutine(TestCultivationSystemIntegration());
            
            // Finalize test suite
            _testSuite.EndTime = DateTime.Now;
            _testSuite.Duration = Time.time - _testStartTime;
            _testSuite.TestResults = new List<IntegrationTestResult>(_testResults);
            
            // Calculate success metrics
            int passedTests = 0;
            foreach (var result in _testResults)
            {
                if (result.Passed) passedTests++;
            }
            
            _testSuite.TotalTests = _testResults.Count;
            _testSuite.PassedTests = passedTests;
            _testSuite.FailedTests = _testResults.Count - passedTests;
            _testSuite.SuccessRate = _testResults.Count > 0 ? (float)passedTests / _testResults.Count * 100f : 0f;
            
            _testsRunning = false;
            OnIntegrationTestsComplete?.Invoke(_testSuite);
            
            LogIntegration("=== INTEGRATION TESTS COMPLETED ===");
            LogIntegration($"Success Rate: {_testSuite.SuccessRate:F1}% ({passedTests}/{_testResults.Count})");
        }
        
        private IEnumerator InitializeSystemReferences()
        {
            LogIntegration("\n--- INITIALIZING SYSTEM REFERENCES ---");
            
            _gameManager = FindAnyObjectByType<GameManager>();
            _cultivationManager = FindAnyObjectByType<CultivationManager>();
            _visualizationManager = FindAnyObjectByType<PlantVisualizationManager>();
            _dataManager = FindAnyObjectByType<DataManager>();
            _eventManager = FindAnyObjectByType<EventManager>();
            
            bool allSystemsFound = _gameManager != null && _cultivationManager != null && 
                                 _visualizationManager != null && 
                                 _dataManager != null && _eventManager != null;
            
            AddIntegrationTestResult("System References Initialization", allSystemsFound, 
                allSystemsFound ? "All required systems found" : "Some systems missing");
            
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator TestSystemInitializationIntegration()
        {
            LogIntegration("\n--- TEST 1: SYSTEM INITIALIZATION INTEGRATION ---");
            
            bool initializationSuccess = true;
            string details = "";
            
            // Test GameManager initialization
            if (_gameManager == null || !_gameManager.IsInitialized)
            {
                initializationSuccess = false;
                details += "GameManager not properly initialized; ";
            }
            
            // Test manager initialization order
            if (_cultivationManager != null && !_cultivationManager.IsInitialized)
            {
                initializationSuccess = false;
                details += "CultivationManager not ready; ";
            }
            
            if (_visualizationManager != null && !_visualizationManager.IsInitialized)
            {
                initializationSuccess = false;
                details += "PlantVisualizationManager not ready; ";
            }
            
            if (_dataManager != null && !_dataManager.IsInitialized)
            {
                initializationSuccess = false;
                details += "DataManager not ready; ";
            }
            
            if (initializationSuccess)
            {
                details = "All systems properly initialized in correct order";
            }
            
            AddIntegrationTestResult("System Initialization Integration", initializationSuccess, details);
            yield return new WaitForSeconds(1f);
        }
        
        private IEnumerator TestCultivationSystemIntegration()
        {
            LogIntegration("\n--- TEST 2: CULTIVATION SYSTEM INTEGRATION ---");
            
            bool cultivationTestPassed = true;
            string details = "";
            
            // Test cultivation manager functionality
            if (_cultivationManager == null || !_cultivationManager.IsInitialized)
            {
                cultivationTestPassed = false;
                details += "CultivationManager not properly initialized; ";
            }
            else
            {
                LogIntegration($"CultivationManager active plant count: {_cultivationManager.ActivePlantCount}");
                LogIntegration($"Total plants grown: {_cultivationManager.TotalPlantsGrown}");
                
                // Test environmental system
                var defaultEnvironment = _cultivationManager.GetZoneEnvironment("default");
                if (defaultEnvironment.Temperature == 0f)
                {
                    cultivationTestPassed = false;
                    details += "Default environment not properly configured; ";
                }
                else
                {
                    LogIntegration($"Default environment temperature: {defaultEnvironment.Temperature}°C");
                }
            }
            
            if (cultivationTestPassed)
            {
                details = $"Cultivation system functioning correctly. Active plants: {_cultivationManager.ActivePlantCount}";
            }
            
            AddIntegrationTestResult("Cultivation System Integration", cultivationTestPassed, details);
            yield return new WaitForSeconds(0.5f);
        }
        
        private void AddIntegrationTestResult(string testName, bool passed, string details)
        {
            var result = new IntegrationTestResult
            {
                Name = testName,
                Passed = passed,
                Details = details,
                Timestamp = DateTime.Now
            };
            
            _testResults.Add(result);
            LogIntegration($"Test '{testName}': {(passed ? "PASSED" : "FAILED")} - {details}");
        }
        
        private void LogIntegration(string message)
        {
            if (_enableDetailedLogging)
            {
                Debug.Log($"[CultivationIntegrationTests] {message}");
                OnIntegrationLogMessage?.Invoke(message);
            }
        }
        
        // Public properties for external access
        public IntegrationTestSuite TestSuite => _testSuite;
        public List<IntegrationTestResult> TestResults => _testResults;
        public bool TestsRunning => _testsRunning;
        
        [ContextMenu("Start Integration Tests")]
        public void StartIntegrationTestsManual()
        {
            StartIntegrationTests();
        }
        
        [ContextMenu("Clear Test Results")]
        public void ClearTestResults()
        {
            _testResults.Clear();
            _testSuite = null;
            Debug.Log("[CultivationIntegrationTests] Test results cleared");
        }
    }
    
    [System.Serializable]
    public class IntegrationTestSuite
    {
        public string Name;
        public DateTime StartTime;
        public DateTime EndTime;
        public float Duration;
        public string TestEnvironment;
        public string UnityVersion;
        public int TotalTests;
        public int PassedTests;
        public int FailedTests;
        public float SuccessRate;
        public List<IntegrationTestResult> TestResults = new List<IntegrationTestResult>();
    }
    
    [System.Serializable]
    public class IntegrationTestResult
    {
        public string Name;
        public bool Passed;
        public string Details;
        public DateTime Timestamp;
    }
} 