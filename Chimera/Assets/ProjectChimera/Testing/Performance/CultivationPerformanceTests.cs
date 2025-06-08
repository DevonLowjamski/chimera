using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Data.Cultivation;
using ProjectChimera.Systems.Cultivation;
using System;
using System.Linq;
using UnityEngine.Profiling;

namespace ProjectChimera.Testing.Performance
{
    /// <summary>
    /// Comprehensive performance testing system for Project Chimera cultivation systems.
    /// Monitors memory usage, frame rates, and system scalability under load.
    /// </summary>
    public class CultivationPerformanceTests : MonoBehaviour
    {
        [Header("Performance Test Configuration")]
        [SerializeField] private bool _runOnStart = false;
        [SerializeField] private bool _enableContinuousMonitoring = true;
        [SerializeField] private float _testDuration = 60f;
        [SerializeField] private int _maxPlantCount = 100;
        [SerializeField] private float _targetFrameRate = 60f;
        
        [Header("Performance Thresholds")]
        [SerializeField] private float _maxMemoryUsageMB = 500f;
        [SerializeField] private float _maxFrameTimeMS = 16.67f; // 60 FPS
        [SerializeField] private float _maxGCAllocPerFrame = 1024f; // 1KB per frame
        [SerializeField] private int _maxDrawCalls = 1000;
        
        [Header("Test Results")]
        [SerializeField] private PerformanceTestSuite _testSuite;
        [SerializeField] private List<PerformanceTestResult> _testResults = new List<PerformanceTestResult>();
        [SerializeField] private PerformanceMetrics _currentMetrics;
        
        // Performance monitoring
        private List<float> _frameTimes = new List<float>();
        private List<float> _memoryUsage = new List<float>();
        private List<float> _gcAllocations = new List<float>();
        private List<int> _drawCalls = new List<int>();
        private float _lastFrameTime;
        private bool _testsRunning = false;
        private bool _monitoringActive = false;
        
        // System references
        private GameManager _gameManager;
        private CultivationManager _cultivationManager;
        
        // Events
        public System.Action<PerformanceTestSuite> OnPerformanceTestsComplete;
        public System.Action<PerformanceMetrics> OnMetricsUpdated;
        public System.Action<string> OnPerformanceLogMessage;
        
        private void Start()
        {
            _lastFrameTime = Time.realtimeSinceStartup;
            
            if (_enableContinuousMonitoring)
            {
                StartContinuousMonitoring();
            }
            
            if (_runOnStart)
            {
                StartCoroutine(DelayedTestStart());
            }
        }
        
        private void Update()
        {
            if (_monitoringActive)
            {
                UpdatePerformanceMetrics();
            }
        }
        
        private IEnumerator DelayedTestStart()
        {
            yield return new WaitForSeconds(5f); // Allow systems to stabilize
            StartPerformanceTests();
        }
        
        public void StartPerformanceTests()
        {
            if (_testsRunning)
            {
                LogPerformance("Performance tests already running!");
                return;
            }
            
            LogPerformance("=== STARTING CULTIVATION PERFORMANCE TESTS ===");
            StartCoroutine(RunPerformanceTests());
        }
        
        public void StartContinuousMonitoring()
        {
            _monitoringActive = true;
            LogPerformance("Continuous performance monitoring started");
        }
        
        public void StopContinuousMonitoring()
        {
            _monitoringActive = false;
            LogPerformance("Continuous performance monitoring stopped");
        }
        
        private IEnumerator RunPerformanceTests()
        {
            _testsRunning = true;
            _testResults.Clear();
            
            // Initialize test suite
            _testSuite = new PerformanceTestSuite
            {
                Name = "Cultivation Performance Tests",
                StartTime = DateTime.Now,
                TestEnvironment = Application.platform.ToString(),
                UnityVersion = Application.unityVersion,
                TargetFrameRate = _targetFrameRate
            };
            
            // Get system references
            _gameManager = FindAnyObjectByType<GameManager>();
            _cultivationManager = FindAnyObjectByType<CultivationManager>();
            
            // Test 1: Baseline Performance
            yield return StartCoroutine(TestBaselinePerformance());
            
            // Test 2: Cultivation System Load
            yield return StartCoroutine(TestParameterSystemLoad());
            
            // Test 3: Multiple Plant Simulation
            yield return StartCoroutine(TestMultiplePlantSimulation());
            
            // Test 4: Memory Stress Test
            yield return StartCoroutine(TestMemoryStress());
            
            // Test 5: Rapid Environment Updates
            yield return StartCoroutine(TestRapidParameterUpdates());
            
            // Test 6: System Integration Load
            yield return StartCoroutine(TestSystemIntegrationLoad());
            
            // Test 7: Long Duration Stability
            yield return StartCoroutine(TestLongDurationStability());
            
            // Test 8: Garbage Collection Impact
            yield return StartCoroutine(TestGarbageCollectionImpact());
            
            // Finalize test suite
            _testSuite.EndTime = DateTime.Now;
            _testSuite.TestResults = new List<PerformanceTestResult>(_testResults);
            
            // Calculate overall metrics
            CalculateOverallMetrics();
            
            _testsRunning = false;
            OnPerformanceTestsComplete?.Invoke(_testSuite);
            
            LogPerformance("=== PERFORMANCE TESTS COMPLETED ===");
            LogPerformance(GeneratePerformanceSummary());
        }
        
        private IEnumerator TestBaselinePerformance()
        {
            LogPerformance("\n--- TEST 1: BASELINE PERFORMANCE ---");
            
            var metrics = new PerformanceMetrics();
            var frameTimes = new List<float>();
            var memoryUsages = new List<float>();
            
            float testStartTime = Time.realtimeSinceStartup;
            
            // Collect baseline metrics for 10 seconds
            while (Time.realtimeSinceStartup - testStartTime < 10f)
            {
                float frameTime = Time.deltaTime * 1000f; // Convert to milliseconds
                float memoryUsage = GetMemoryUsageMB();
                
                frameTimes.Add(frameTime);
                memoryUsages.Add(memoryUsage);
                
                yield return null;
            }
            
            // Calculate baseline metrics
            metrics.AverageFrameTime = CalculateAverage(frameTimes);
            metrics.MaxFrameTime = CalculateMax(frameTimes);
            metrics.MinFrameTime = CalculateMin(frameTimes);
            metrics.AverageMemoryUsage = CalculateAverage(memoryUsages);
            metrics.MaxMemoryUsage = CalculateMax(memoryUsages);
            
            bool baselinePassed = metrics.AverageFrameTime < _maxFrameTimeMS && 
                                metrics.AverageMemoryUsage < _maxMemoryUsageMB;
            
            string details = $"Avg Frame: {metrics.AverageFrameTime:F2}ms, Max Frame: {metrics.MaxFrameTime:F2}ms, " +
                           $"Avg Memory: {metrics.AverageMemoryUsage:F1}MB, Max Memory: {metrics.MaxMemoryUsage:F1}MB";
            
            AddPerformanceTestResult("Baseline Performance", baselinePassed, details, metrics);
            yield return new WaitForSeconds(1f);
        }
        
        private IEnumerator TestParameterSystemLoad()
        {
            LogPerformance("\n--- TEST 2: CULTIVATION SYSTEM LOAD ---");
            
            var metrics = new PerformanceMetrics();
            var frameTimes = new List<float>();
            
            if (_cultivationManager == null)
            {
                AddPerformanceTestResult("Cultivation System Load", false, "CultivationManager not available", metrics);
                yield break;
            }
            
            float testStartTime = Time.realtimeSinceStartup;
            int operationCount = 0;
            
            // Stress test cultivation system for 15 seconds
            while (Time.realtimeSinceStartup - testStartTime < 15f)
            {
                // Test multiple cultivation operations each frame
                for (int i = 0; i < 5; i++)
                {
                    // Test environment access
                    var environment = _cultivationManager.GetZoneEnvironment("default");
                    
                    // Test statistics access
                    var stats = _cultivationManager.GetCultivationStats();
                    
                    // Test plant queries
                    var allPlants = _cultivationManager.GetAllPlants();
                    
                    operationCount += 3;
                }
                
                float frameTime = Time.deltaTime * 1000f;
                frameTimes.Add(frameTime);
                
                yield return null;
            }
            
            metrics.AverageFrameTime = CalculateAverage(frameTimes);
            metrics.MaxFrameTime = CalculateMax(frameTimes);
            metrics.ParameterUpdateCount = operationCount;
            
            bool cultivationLoadPassed = metrics.AverageFrameTime < _maxFrameTimeMS * 1.5f; // Allow 50% overhead
            
            string details = $"Cultivation Operations: {operationCount}, " +
                           $"Avg Frame: {metrics.AverageFrameTime:F2}ms, Max Frame: {metrics.MaxFrameTime:F2}ms";
            
            AddPerformanceTestResult("Cultivation System Load", cultivationLoadPassed, details, metrics);
            yield return new WaitForSeconds(2f);
        }
        
        private IEnumerator TestMultiplePlantSimulation()
        {
            LogPerformance("\n--- TEST 3: MULTIPLE PLANT SIMULATION ---");
            
            var metrics = new PerformanceMetrics();
            var frameTimes = new List<float>();
            var memoryUsages = new List<float>();
            
            if (_cultivationManager == null)
            {
                AddPerformanceTestResult("Multiple Plant Simulation", false, "CultivationManager not available", metrics);
                yield break;
            }
            
            float initialMemory = GetMemoryUsageMB();
            int initialPlantCount = _cultivationManager.ActivePlantCount;
            
            // Simulate creating multiple plants (this would need actual plant creation methods)
            // For now, we'll simulate the load by creating parameter sets for multiple plants
            var plantParameters = new List<Dictionary<string, object>>();
            
            for (int i = 0; i < _maxPlantCount; i++)
            {
                var plantParams = new Dictionary<string, object>
                {
                    {"height", UnityEngine.Random.Range(0.1f, 2.0f)},
                    {"health", UnityEngine.Random.Range(0.5f, 1.0f)},
                    {"growth_stage", UnityEngine.Random.Range(1, 5)},
                    {"water_level", UnityEngine.Random.Range(0.3f, 1.0f)}
                };
                plantParameters.Add(plantParams);
            }
            
            float testStartTime = Time.realtimeSinceStartup;
            
            // Simulate plant updates for 20 seconds
            while (Time.realtimeSinceStartup - testStartTime < 20f)
            {
                // Update all simulated plants
                foreach (var plantParams in plantParameters)
                {
                    // Simulate plant growth calculations
                    var height = (float)plantParams["height"];
                    var health = (float)plantParams["health"];
                    
                    plantParams["height"] = height + UnityEngine.Random.Range(0.001f, 0.01f);
                    plantParams["health"] = Mathf.Clamp01(health + UnityEngine.Random.Range(-0.01f, 0.01f));
                }
                
                float frameTime = Time.deltaTime * 1000f;
                float memoryUsage = GetMemoryUsageMB();
                
                frameTimes.Add(frameTime);
                memoryUsages.Add(memoryUsage);
                
                yield return null;
            }
            
            float finalMemory = GetMemoryUsageMB();
            
            metrics.AverageFrameTime = CalculateAverage(frameTimes);
            metrics.MaxFrameTime = CalculateMax(frameTimes);
            metrics.AverageMemoryUsage = CalculateAverage(memoryUsages);
            metrics.MaxMemoryUsage = CalculateMax(memoryUsages);
            metrics.MemoryIncrease = finalMemory - initialMemory;
            metrics.PlantCount = _maxPlantCount;
            
            bool multiplePlantsPassed = metrics.AverageFrameTime < _maxFrameTimeMS * 2f && 
                                      metrics.MemoryIncrease < 100f; // Max 100MB increase
            
            string details = $"Plants: {_maxPlantCount}, Avg Frame: {metrics.AverageFrameTime:F2}ms, " +
                           $"Memory Increase: {metrics.MemoryIncrease:F1}MB";
            
            AddPerformanceTestResult("Multiple Plant Simulation", multiplePlantsPassed, details, metrics);
            yield return new WaitForSeconds(2f);
        }
        
        private IEnumerator TestMemoryStress()
        {
            LogPerformance("\n--- TEST 4: MEMORY STRESS TEST ---");
            
            var metrics = new PerformanceMetrics();
            float initialMemory = GetMemoryUsageMB();
            
            // Create temporary objects to stress memory
            var tempObjects = new List<GameObject>();
            var tempData = new List<float[]>();
            
            // Create memory pressure
            for (int i = 0; i < 1000; i++)
            {
                var tempObj = new GameObject($"TempObject_{i}");
                tempObjects.Add(tempObj);
                
                // Create some data arrays
                var dataArray = new float[1000];
                for (int j = 0; j < dataArray.Length; j++)
                {
                    dataArray[j] = UnityEngine.Random.value;
                }
                tempData.Add(dataArray);
                
                if (i % 100 == 0)
                {
                    yield return null; // Allow frame processing
                }
            }
            
            float peakMemory = GetMemoryUsageMB();
            
            // Force garbage collection
            System.GC.Collect();
            yield return new WaitForSeconds(1f);
            
            float postGCMemory = GetMemoryUsageMB();
            
            // Clean up
            foreach (var obj in tempObjects)
            {
                if (obj != null) DestroyImmediate(obj);
            }
            tempObjects.Clear();
            tempData.Clear();
            
            System.GC.Collect();
            yield return new WaitForSeconds(1f);
            
            float finalMemory = GetMemoryUsageMB();
            
            metrics.MaxMemoryUsage = peakMemory;
            metrics.MemoryIncrease = peakMemory - initialMemory;
            metrics.MemoryRecovered = peakMemory - finalMemory;
            metrics.FinalMemoryUsage = finalMemory;
            
            bool memoryStressPassed = peakMemory < _maxMemoryUsageMB * 2f && 
                                    metrics.MemoryRecovered > metrics.MemoryIncrease * 0.8f; // 80% recovery
            
            string details = $"Peak: {peakMemory:F1}MB, Increase: {metrics.MemoryIncrease:F1}MB, " +
                           $"Recovered: {metrics.MemoryRecovered:F1}MB, Final: {finalMemory:F1}MB";
            
            AddPerformanceTestResult("Memory Stress Test", memoryStressPassed, details, metrics);
            
            yield return new WaitForSeconds(2f);
        }
        
        private IEnumerator TestRapidParameterUpdates()
        {
            LogPerformance("\n--- TEST 5: RAPID ENVIRONMENT UPDATES ---");
            
            var metrics = new PerformanceMetrics();
            var frameTimes = new List<float>();
            
            if (_cultivationManager == null)
            {
                AddPerformanceTestResult("Rapid Environment Updates", false, "CultivationManager not available", metrics);
                yield break;
            }
            
            float testStartTime = Time.realtimeSinceStartup;
            int updateCount = 0;
            
            // Rapid environment updates for 10 seconds
            while (Time.realtimeSinceStartup - testStartTime < 10f)
            {
                // Update multiple environmental conditions per frame
                for (int i = 0; i < 20; i++)
                {
                    var newEnvironment = EnvironmentalConditions.CreateIndoorDefault();
                    newEnvironment.Temperature = 20f + UnityEngine.Random.Range(-5f, 10f);
                    newEnvironment.Humidity = 50f + UnityEngine.Random.Range(-20f, 30f);
                    newEnvironment.LightIntensity = 500f + UnityEngine.Random.Range(-200f, 300f);
                    
                    string zoneId = $"test_zone_{i % 5}";
                    _cultivationManager.SetZoneEnvironment(zoneId, newEnvironment);
                    
                    // Retrieve to test system response
                    var retrievedEnvironment = _cultivationManager.GetZoneEnvironment(zoneId);
                    updateCount++;
                }
                
                float frameTime = Time.deltaTime * 1000f;
                frameTimes.Add(frameTime);
                
                yield return null;
            }
            
            metrics.AverageFrameTime = CalculateAverage(frameTimes);
            metrics.MaxFrameTime = CalculateMax(frameTimes);
            metrics.ParameterUpdateCount = updateCount;
            metrics.UpdatesPerSecond = updateCount / 10f;
            
            bool rapidUpdatesPassed = metrics.AverageFrameTime < _maxFrameTimeMS * 3f; // Allow 3x overhead
            
            string details = $"Updates: {updateCount}, Updates/sec: {metrics.UpdatesPerSecond:F0}, " +
                           $"Avg Frame: {metrics.AverageFrameTime:F2}ms";
            
            AddPerformanceTestResult("Rapid Environment Updates", rapidUpdatesPassed, details, metrics);
            yield return new WaitForSeconds(2f);
        }
        
        private IEnumerator TestSystemIntegrationLoad()
        {
            LogPerformance("\n--- TEST 6: SYSTEM INTEGRATION LOAD ---");
            
            var metrics = new PerformanceMetrics();
            var frameTimes = new List<float>();
            
            float testStartTime = Time.realtimeSinceStartup;
            
            // Simulate complex system interactions
            while (Time.realtimeSinceStartup - testStartTime < 15f)
            {
                // Simulate environmental changes affecting multiple systems
                if (_cultivationManager != null)
                {
                    var newEnvironment = EnvironmentalConditions.CreateIndoorDefault();
                    newEnvironment.Temperature = 20f + Mathf.Sin(Time.time) * 10f;
                    newEnvironment.Humidity = 50f + Mathf.Cos(Time.time * 0.5f) * 20f;
                    newEnvironment.LightIntensity = 400f + Mathf.Sin(Time.time * 2f) * 200f;
                    
                    _cultivationManager.SetZoneEnvironment("performance_test", newEnvironment);
                }
                
                float frameTime = Time.deltaTime * 1000f;
                frameTimes.Add(frameTime);
                
                yield return null;
            }
            
            metrics.AverageFrameTime = CalculateAverage(frameTimes);
            metrics.MaxFrameTime = CalculateMax(frameTimes);
            
            bool integrationLoadPassed = metrics.AverageFrameTime < _maxFrameTimeMS * 2f;
            
            string details = $"Avg Frame: {metrics.AverageFrameTime:F2}ms, Max Frame: {metrics.MaxFrameTime:F2}ms";
            
            AddPerformanceTestResult("System Integration Load", integrationLoadPassed, details, metrics);
            yield return new WaitForSeconds(2f);
        }
        
        private IEnumerator TestLongDurationStability()
        {
            LogPerformance("\n--- TEST 7: LONG DURATION STABILITY ---");
            
            var metrics = new PerformanceMetrics();
            var frameTimes = new List<float>();
            var memoryUsages = new List<float>();
            
            float testStartTime = Time.realtimeSinceStartup;
            float initialMemory = GetMemoryUsageMB();
            
            // Run stability test for specified duration
            while (Time.realtimeSinceStartup - testStartTime < _testDuration)
            {
                float frameTime = Time.deltaTime * 1000f;
                float memoryUsage = GetMemoryUsageMB();
                
                frameTimes.Add(frameTime);
                memoryUsages.Add(memoryUsage);
                
                // Simulate ongoing cultivation activities
                if (_cultivationManager != null && Time.frameCount % 60 == 0) // Every second at 60 FPS
                {
                    // Test ongoing cultivation operations
                    var stats = _cultivationManager.GetCultivationStats();
                    var environment = _cultivationManager.GetZoneEnvironment("default");
                    
                    // Slight environmental variations to simulate real conditions
                    var newEnvironment = environment;
                    newEnvironment.Temperature += UnityEngine.Random.Range(-0.1f, 0.1f);
                    newEnvironment.Humidity += UnityEngine.Random.Range(-0.5f, 0.5f);
                    
                    _cultivationManager.SetZoneEnvironment("stability_test", newEnvironment);
                }
                
                yield return null;
            }
            
            float finalMemory = GetMemoryUsageMB();
            
            metrics.AverageFrameTime = CalculateAverage(frameTimes);
            metrics.MaxFrameTime = CalculateMax(frameTimes);
            metrics.AverageMemoryUsage = CalculateAverage(memoryUsages);
            metrics.MaxMemoryUsage = CalculateMax(memoryUsages);
            metrics.MemoryIncrease = finalMemory - initialMemory;
            metrics.TestDuration = _testDuration;
            
            // Check for memory leaks and frame time stability
            bool stabilityPassed = metrics.MemoryIncrease < 50f && // Max 50MB increase over time
                                 metrics.AverageFrameTime < _maxFrameTimeMS * 1.2f; // Max 20% overhead
            
            string details = $"Duration: {_testDuration}s, Avg Frame: {metrics.AverageFrameTime:F2}ms, " +
                           $"Memory Increase: {metrics.MemoryIncrease:F1}MB";
            
            AddPerformanceTestResult("Long Duration Stability", stabilityPassed, details, metrics);
            yield return new WaitForSeconds(2f);
        }
        
        private IEnumerator TestGarbageCollectionImpact()
        {
            LogPerformance("\n--- TEST 8: GARBAGE COLLECTION IMPACT ---");
            
            var metrics = new PerformanceMetrics();
            var frameTimes = new List<float>();
            var gcAllocations = new List<float>();
            
            float testStartTime = Time.realtimeSinceStartup;
            
            // Monitor GC impact for 10 seconds
            while (Time.realtimeSinceStartup - testStartTime < 10f)
            {
                float frameTime = Time.deltaTime * 1000f;
                float gcAlloc = Profiler.GetTotalAllocatedMemory();
                
                frameTimes.Add(frameTime);
                gcAllocations.Add(gcAlloc);
                
                // Force some allocations to test GC impact
                if (Time.frameCount % 30 == 0) // Every 0.5 seconds at 60 FPS
                {
                    var tempArray = new float[100];
                    for (int i = 0; i < tempArray.Length; i++)
                    {
                        tempArray[i] = UnityEngine.Random.value;
                    }
                }
                
                yield return null;
            }
            
            metrics.AverageFrameTime = CalculateAverage(frameTimes);
            metrics.MaxFrameTime = CalculateMax(frameTimes);
            metrics.AverageGCAllocation = CalculateAverage(gcAllocations);
            metrics.MaxGCAllocation = CalculateMax(gcAllocations);
            
            bool gcImpactPassed = metrics.AverageFrameTime < _maxFrameTimeMS * 1.5f &&
                                metrics.MaxFrameTime < _maxFrameTimeMS * 3f; // Allow spikes during GC
            
            string details = $"Avg Frame: {metrics.AverageFrameTime:F2}ms, Max Frame: {metrics.MaxFrameTime:F2}ms, " +
                           $"Avg GC Alloc: {metrics.AverageGCAllocation / 1024f:F1}KB";
            
            AddPerformanceTestResult("Garbage Collection Impact", gcImpactPassed, details, metrics);
            yield return new WaitForSeconds(2f);
        }
        
        private void UpdatePerformanceMetrics()
        {
            float currentTime = Time.realtimeSinceStartup;
            float frameTime = (currentTime - _lastFrameTime) * 1000f; // Convert to milliseconds
            _lastFrameTime = currentTime;
            
            _frameTimes.Add(frameTime);
            _memoryUsage.Add(GetMemoryUsageMB());
                            _gcAllocations.Add(Profiler.GetTotalAllocatedMemory());
            
            // Keep only recent samples
            if (_frameTimes.Count > 300) // 5 seconds at 60 FPS
            {
                _frameTimes.RemoveAt(0);
                _memoryUsage.RemoveAt(0);
                _gcAllocations.RemoveAt(0);
            }
            
            // Update current metrics
            _currentMetrics = new PerformanceMetrics
            {
                AverageFrameTime = CalculateAverage(_frameTimes),
                MaxFrameTime = CalculateMax(_frameTimes),
                MinFrameTime = CalculateMin(_frameTimes),
                AverageMemoryUsage = CalculateAverage(_memoryUsage),
                MaxMemoryUsage = CalculateMax(_memoryUsage),
                AverageGCAllocation = CalculateAverage(_gcAllocations),
                SampleCount = _frameTimes.Count
            };
            
            OnMetricsUpdated?.Invoke(_currentMetrics);
        }
        
        private void CalculateOverallMetrics()
        {
            if (_testResults.Count == 0) return;
            
            int passedTests = 0;
            foreach (var result in _testResults)
            {
                if (result.Passed) passedTests++;
            }
            
            _testSuite.TotalTests = _testResults.Count;
            _testSuite.PassedTests = passedTests;
            _testSuite.FailedTests = _testResults.Count - passedTests;
            _testSuite.SuccessRate = (float)passedTests / _testResults.Count * 100f;
            
            // Calculate aggregate metrics
            var allMetrics = new List<PerformanceMetrics>();
            foreach (var result in _testResults)
            {
                if (result.Metrics != null)
                {
                    allMetrics.Add(result.Metrics);
                }
            }
            
            if (allMetrics.Count > 0)
            {
                _testSuite.OverallMetrics = new PerformanceMetrics
                {
                    AverageFrameTime = allMetrics.ConvertAll(m => m.AverageFrameTime).Average(),
                    MaxFrameTime = allMetrics.ConvertAll(m => m.MaxFrameTime).Max(),
                    AverageMemoryUsage = allMetrics.ConvertAll(m => m.AverageMemoryUsage).Average(),
                    MaxMemoryUsage = allMetrics.ConvertAll(m => m.MaxMemoryUsage).Max()
                };
            }
        }
        
        private float GetMemoryUsageMB()
        {
            return Profiler.GetTotalAllocatedMemory() / (1024f * 1024f);
        }
        
        private float CalculateAverage(List<float> values)
        {
            if (values.Count == 0) return 0f;
            float sum = 0f;
            foreach (var value in values)
            {
                sum += value;
            }
            return sum / values.Count;
        }
        
        private float CalculateMax(List<float> values)
        {
            if (values.Count == 0) return 0f;
            float max = values[0];
            foreach (var value in values)
            {
                if (value > max) max = value;
            }
            return max;
        }
        
        private float CalculateMin(List<float> values)
        {
            if (values.Count == 0) return 0f;
            float min = values[0];
            foreach (var value in values)
            {
                if (value < min) min = value;
            }
            return min;
        }
        
        private void AddPerformanceTestResult(string testName, bool passed, string details, PerformanceMetrics metrics)
        {
            var result = new PerformanceTestResult
            {
                Name = testName,
                Passed = passed,
                Details = details,
                Metrics = metrics,
                Timestamp = DateTime.Now
            };
            
            _testResults.Add(result);
            LogPerformance($"{(passed ? "PASS" : "FAIL")}: {testName} - {details}");
        }
        
        private void LogPerformance(string message)
        {
            Debug.Log($"[Performance] {message}");
            OnPerformanceLogMessage?.Invoke(message);
        }
        
        private string GeneratePerformanceSummary()
        {
            if (_testSuite == null) return "No test suite available";
            
            return $"Performance Test Summary:\n" +
                   $"Total Tests: {_testSuite.TotalTests}\n" +
                   $"Passed: {_testSuite.PassedTests}\n" +
                   $"Failed: {_testSuite.FailedTests}\n" +
                   $"Success Rate: {_testSuite.SuccessRate:F1}%\n" +
                   $"Duration: {(_testSuite.EndTime - _testSuite.StartTime).TotalSeconds:F1}s";
        }
        
        // Public properties
        public PerformanceTestSuite TestSuite => _testSuite;
        public List<PerformanceTestResult> TestResults => _testResults;
        public PerformanceMetrics CurrentMetrics => _currentMetrics;
        public bool TestsRunning => _testsRunning;
        public bool MonitoringActive => _monitoringActive;
        
        // Manual test controls
        [ContextMenu("Start Performance Tests")]
        public void StartPerformanceTestsManual()
        {
            StartPerformanceTests();
        }
        
        [ContextMenu("Clear Test Results")]
        public void ClearTestResults()
        {
            _testResults.Clear();
            _testSuite = null;
            LogPerformance("Performance test results cleared");
        }
        
        [ContextMenu("Force Garbage Collection")]
        public void ForceGarbageCollection()
        {
            System.GC.Collect();
            LogPerformance("Forced garbage collection");
        }
    }
    
    [System.Serializable]
    public class PerformanceTestSuite
    {
        public string Name;
        public DateTime StartTime;
        public DateTime EndTime;
        public string TestEnvironment;
        public string UnityVersion;
        public float TargetFrameRate;
        public int TotalTests;
        public int PassedTests;
        public int FailedTests;
        public float SuccessRate;
        public PerformanceMetrics OverallMetrics;
        public List<PerformanceTestResult> TestResults = new List<PerformanceTestResult>();
    }
    
    [System.Serializable]
    public class PerformanceTestResult
    {
        public string Name;
        public bool Passed;
        public string Details;
        public PerformanceMetrics Metrics;
        public DateTime Timestamp;
    }
    
    [System.Serializable]
    public class PerformanceMetrics
    {
        public float AverageFrameTime;
        public float MaxFrameTime;
        public float MinFrameTime;
        public float AverageMemoryUsage;
        public float MaxMemoryUsage;
        public float FinalMemoryUsage;
        public float MemoryIncrease;
        public float MemoryRecovered;
        public float AverageGCAllocation;
        public float MaxGCAllocation;
        public int ParameterUpdateCount;
        public float UpdatesPerSecond;
        public int PlantCount;
        public float TestDuration;
        public int SampleCount;
    }
} 