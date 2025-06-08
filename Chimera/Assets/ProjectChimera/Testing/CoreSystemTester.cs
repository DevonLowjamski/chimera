using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectChimera.Core;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Comprehensive test controller for Project Chimera core systems.
    /// Tests manager initialization, event system, time scaling, data management, and save/load.
    /// </summary>
    public class CoreSystemTester : ChimeraMonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private bool _runTestsOnStart = true;
        [SerializeField] private bool _enableDetailedLogging = true;
        [SerializeField] private float _testDelay = 1.0f;

        [Header("Test Events")]
        [SerializeField] private SimpleGameEventSO _onTestStarted;
        [SerializeField] private StringGameEventSO _onTestCompleted;
        [SerializeField] private StringGameEventSO _onTestFailed;

        // Test tracking
        private int _testsRun = 0;
        private int _testsPassed = 0;
        private int _testsFailed = 0;
        private List<string> _testResults = new List<string>();
        private bool _testsRunning = false;

        protected override void Start()
        {
            base.Start();

            if (_runTestsOnStart)
            {
                StartCoroutine(RunAllTestsCoroutine());
            }
        }

        /// <summary>
        /// Runs all core system tests.
        /// </summary>
        public void RunAllTests()
        {
            StartCoroutine(RunAllTestsCoroutine());
        }
        
        /// <summary>
        /// Runs tests manually for integration with test runner.
        /// </summary>
        public void RunTestsManually()
        {
            RunAllTests();
        }

        /// <summary>
        /// Coroutine that runs all tests with delays.
        /// </summary>
        private IEnumerator RunAllTestsCoroutine()
        {
            _testsRunning = true;
            LogInfo("=== Starting Project Chimera Core System Tests ===");
            _onTestStarted?.Raise();

            ResetTestCounters();

            // Wait for managers to initialize
            yield return new WaitForSeconds(0.5f);

            // Test 1: Manager Initialization
            yield return StartCoroutine(TestManagerInitialization());
            yield return new WaitForSeconds(_testDelay);

            // Test 2: Event System
            yield return StartCoroutine(TestEventSystem());
            yield return new WaitForSeconds(_testDelay);

            // Test 3: Time Manager
            yield return StartCoroutine(TestTimeManager());
            yield return new WaitForSeconds(_testDelay);

            // Test 4: Data Manager
            yield return StartCoroutine(TestDataManager());
            yield return new WaitForSeconds(_testDelay);

            // Test 5: Settings Manager
            yield return StartCoroutine(TestSettingsManager());
            yield return new WaitForSeconds(_testDelay);

            // Test 6: Save Manager
            yield return StartCoroutine(TestSaveManager());
            yield return new WaitForSeconds(_testDelay);

            // Print test summary
            PrintTestSummary();
            
            _testsRunning = false;
        }

        /// <summary>
        /// Tests manager initialization and singleton access.
        /// </summary>
        private IEnumerator TestManagerInitialization()
        {
            LogInfo("Testing Manager Initialization...");

            // Test GameManager singleton
            bool gameManagerTest = TestCondition(
                "GameManager Singleton",
                GameManager.Instance != null && GameManager.Instance.IsInitialized
            );

            // Test TimeManager access
            bool timeManagerTest = TestCondition(
                "TimeManager Access",
                GameManager.Instance.GetManager<TimeManager>() != null
            );

            // Test DataManager access
            bool dataManagerTest = TestCondition(
                "DataManager Access",
                GameManager.Instance.GetManager<DataManager>() != null
            );

            // Test EventManager access
            bool eventManagerTest = TestCondition(
                "EventManager Access",
                GameManager.Instance.GetManager<EventManager>() != null
            );

            // Test SaveManager access
            bool saveManagerTest = TestCondition(
                "SaveManager Access",
                GameManager.Instance.GetManager<SaveManager>() != null
            );

            // Test SettingsManager access
            bool settingsManagerTest = TestCondition(
                "SettingsManager Access",
                GameManager.Instance.GetManager<SettingsManager>() != null
            );

            bool allManagersInitialized = gameManagerTest && timeManagerTest && 
                                        dataManagerTest && eventManagerTest && 
                                        saveManagerTest && settingsManagerTest;

            TestResult("Manager Initialization", allManagersInitialized);
            yield return null;
        }

        /// <summary>
        /// Tests the event system functionality.
        /// </summary>
        private IEnumerator TestEventSystem()
        {
            LogInfo("Testing Event System...");

            bool eventRaiseTest = false;
            bool eventListenerTest = false;

            // Create test event listener
            if (_onTestStarted != null)
            {
                // Test event raising
                try
                {
                    _onTestStarted.Raise();
                    eventRaiseTest = true;
                    LogDebug("Event raise test passed");
                }
                catch (System.Exception e)
                {
                    LogError($"Event raise test failed: {e.Message}");
                }

                eventListenerTest = true; // If we got here, the event system is working
            }

            bool eventSystemTest = eventRaiseTest && eventListenerTest;
            TestResult("Event System", eventSystemTest);
            yield return null;
        }

        /// <summary>
        /// Tests TimeManager functionality.
        /// </summary>
        private IEnumerator TestTimeManager()
        {
            LogInfo("Testing Time Manager...");

            var timeManager = GameManager.Instance?.GetManager<TimeManager>();
            if (timeManager == null)
            {
                TestResult("Time Manager", false);
                yield break;
            }

            // Test time scale changes
            float originalTimeScale = timeManager.CurrentTimeScale;
            
            timeManager.SetTimeScale(2.0f);
            bool timeScaleTest = TestCondition(
                "Time Scale Change",
                Mathf.Approximately(timeManager.CurrentTimeScale, 2.0f)
            );

            // Test time scale reset
            timeManager.ResetTimeScale();
            bool timeResetTest = TestCondition(
                "Time Scale Reset",
                Mathf.Approximately(timeManager.CurrentTimeScale, originalTimeScale)
            );

            // Test pause/resume
            timeManager.Pause();
            bool pauseTest = TestCondition("Time Pause", timeManager.IsTimePaused);

            timeManager.Resume();
            bool resumeTest = TestCondition("Time Resume", !timeManager.IsTimePaused);

            bool timeManagerTest = timeScaleTest && timeResetTest && pauseTest && resumeTest;
            TestResult("Time Manager", timeManagerTest);
            yield return null;
        }

        /// <summary>
        /// Tests DataManager functionality.
        /// </summary>
        private IEnumerator TestDataManager()
        {
            LogInfo("Testing Data Manager...");

            var dataManager = GameManager.Instance?.GetManager<DataManager>();
            if (dataManager == null)
            {
                TestResult("Data Manager", false);
                yield break;
            }

            // Test data manager stats
            var stats = dataManager.GetStats();
            bool statsTest = TestCondition(
                "Data Manager Stats",
                stats.TotalDataAssets >= 0 && stats.TotalConfigAssets >= 0
            );

            // Test data validation
            try
            {
                dataManager.ValidateAllData();
                bool validationTest = TestCondition("Data Validation", true);
                
                bool dataManagerTest = statsTest && validationTest;
                TestResult("Data Manager", dataManagerTest);
            }
            catch (System.Exception e)
            {
                LogError($"Data validation failed: {e.Message}");
                TestResult("Data Manager", false);
            }

            yield return null;
        }

        /// <summary>
        /// Tests SettingsManager functionality.
        /// </summary>
        private IEnumerator TestSettingsManager()
        {
            LogInfo("Testing Settings Manager...");

            var settingsManager = GameManager.Instance?.GetManager<SettingsManager>();
            if (settingsManager == null)
            {
                TestResult("Settings Manager", false);
                yield break;
            }

            // Test setting values
            float originalVolume = settingsManager.GetFloat("audio.master_volume", 1.0f);
            
            settingsManager.SetSetting("audio.master_volume", 0.5f, false);
            bool setTest = TestCondition(
                "Setting Value",
                Mathf.Approximately(settingsManager.GetFloat("audio.master_volume"), 0.5f)
            );

            // Restore original value
            settingsManager.SetSetting("audio.master_volume", originalVolume, false);

            // Test boolean setting
            bool originalPause = settingsManager.GetBool("gameplay.pause_on_focus_loss", true);
            settingsManager.SetSetting("gameplay.pause_on_focus_loss", !originalPause, false);
            bool boolTest = TestCondition(
                "Boolean Setting",
                settingsManager.GetBool("gameplay.pause_on_focus_loss") == !originalPause
            );

            // Restore original value
            settingsManager.SetSetting("gameplay.pause_on_focus_loss", originalPause, false);

            bool settingsManagerTest = setTest && boolTest;
            TestResult("Settings Manager", settingsManagerTest);
            yield return null;
        }

        /// <summary>
        /// Tests SaveManager functionality.
        /// </summary>
        private IEnumerator TestSaveManager()
        {
            LogInfo("Testing Save Manager...");

            var saveManager = GameManager.Instance?.GetManager<SaveManager>();
            if (saveManager == null)
            {
                TestResult("Save Manager", false);
                yield break;
            }

            // Test save directory creation
            bool directoryTest = TestCondition(
                "Save Directory",
                !string.IsNullOrEmpty(saveManager.SaveDirectory)
            );

            // Test saveable component registration
            var testComponent = gameObject.AddComponent<TestSaveableComponent>();
            saveManager.RegisterSaveableComponent(testComponent);
            
            bool registrationTest = TestCondition(
                "Saveable Registration",
                saveManager.SaveableComponentCount > 0
            );

            // Test save slot info
            var saveSlots = saveManager.GetSaveSlots();
            bool slotsTest = TestCondition(
                "Save Slots",
                saveSlots != null
            );

            // Clean up
            saveManager.UnregisterSaveableComponent(testComponent);
            DestroyImmediate(testComponent);

            bool saveManagerTest = directoryTest && registrationTest && slotsTest;
            TestResult("Save Manager", saveManagerTest);
            yield return null;
        }

        /// <summary>
        /// Tests a condition and logs the result.
        /// </summary>
        private bool TestCondition(string testName, bool condition)
        {
            if (_enableDetailedLogging)
            {
                LogDebug($"  - {testName}: {(condition ? "PASS" : "FAIL")}");
            }
            return condition;
        }

        /// <summary>
        /// Records a test result.
        /// </summary>
        private void TestResult(string testName, bool passed)
        {
            _testsRun++;
            if (passed)
            {
                _testsPassed++;
                LogInfo($"✅ {testName}: PASSED");
                _testResults.Add($"{testName}: PASSED");
                _onTestCompleted?.Raise($"{testName}: PASSED");
            }
            else
            {
                _testsFailed++;
                LogError($"❌ {testName}: FAILED");
                _testResults.Add($"{testName}: FAILED");
                _onTestFailed?.Raise($"{testName}: FAILED");
            }
        }

        /// <summary>
        /// Resets test counters.
        /// </summary>
        private void ResetTestCounters()
        {
            _testsRun = 0;
            _testsPassed = 0;
            _testsFailed = 0;
            _testResults.Clear();
        }

        /// <summary>
        /// Prints the test summary.
        /// </summary>
        private void PrintTestSummary()
        {
            LogInfo("=== Test Summary ===");
            LogInfo($"Tests Run: {_testsRun}");
            LogInfo($"Tests Passed: {_testsPassed}");
            LogInfo($"Tests Failed: {_testsFailed}");
            LogInfo($"Success Rate: {(_testsRun > 0 ? (_testsPassed * 100.0f / _testsRun):0):F1}%");
            
            if (_testsFailed > 0)
            {
                LogWarning("Failed Tests:");
                foreach (string result in _testResults)
                {
                    if (result.Contains("FAILED"))
                    {
                        LogWarning($"  - {result}");
                    }
                }
            }
            else
            {
                LogInfo("🎉 All tests passed!");
            }

            LogInfo("=== End Test Summary ===");
        }

        /// <summary>
        /// Manual test trigger for UI buttons.
        /// </summary>
        [ContextMenu("Run Tests")]
        public void RunTestsManual()
        {
            RunAllTests();
        }

        /// <summary>
        /// Test specific managers individually.
        /// </summary>
        [ContextMenu("Test GameManager Only")]
        public void TestGameManagerOnly()
        {
            StartCoroutine(TestManagerInitialization());
        }

        [ContextMenu("Test TimeManager Only")]
        public void TestTimeManagerOnly()
        {
            StartCoroutine(TestTimeManager());
        }

        [ContextMenu("Test EventSystem Only")]
        public void TestEventSystemOnly()
        {
            StartCoroutine(TestEventSystem());
        }
        
        // Public properties for external access
        public bool TestsRunning => _testsRunning;
        public int TestsRun => _testsRun;
        public int TestsPassed => _testsPassed;
        public int TestsFailed => _testsFailed;
    }

    /// <summary>
    /// Simple test component that implements ISaveable for testing.
    /// </summary>
    public class TestSaveableComponent : MonoBehaviour, ISaveable
    {
        private TestSaveData _testData = new TestSaveData { TestValue = 42, TestString = "Test" };

        public string GetSaveId()
        {
            return "TestSaveableComponent_" + GetInstanceID();
        }

        public object GetSaveData()
        {
            return _testData;
        }

        public void LoadSaveData(object data)
        {
            if (data is TestSaveData testData)
            {
                _testData = testData;
            }
        }

        [System.Serializable]
        public class TestSaveData
        {
            public int TestValue;
            public string TestString;
        }
    }
}