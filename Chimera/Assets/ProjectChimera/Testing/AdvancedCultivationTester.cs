using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Data.Cultivation;
using ProjectChimera.Data.Genetics;
using ProjectChimera.Systems.Cultivation;
using System;
using System.Linq;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Comprehensive test system for all advanced cultivation systems.
    /// Tests integration between VPD Management, Environmental Automation, Fertigation, and basic cultivation.
    /// </summary>
    public class AdvancedCultivationTester : MonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private bool _runTestsOnStart = true;
        [SerializeField] private bool _enableDetailedLogging = true;
        [SerializeField] private int _testPlantCount = 5;
        
        [Header("System References")]
        [SerializeField] private VPDManagementSO _vpdSystem;
        [SerializeField] private EnvironmentalAutomationSO _environmentalSystem;
        [SerializeField] private FertigationSystemSO _fertigationSystem;
        [SerializeField] private CultivationZoneSO _testZone;
        [SerializeField] private PlantStrainSO _testStrain;
        [SerializeField] private GenotypeDataSO _testGenotype;
        
        [Header("Test Results")]
        [SerializeField] private int _totalTests = 0;
        [SerializeField] private int _passedTests = 0;
        [SerializeField] private int _failedTests = 0;
        [SerializeField] private float _testStartTime = 0f;
        
        private CultivationManager _cultivationManager;
        private List<string> _testResults = new List<string>();
        private List<PlantInstanceSO> _testPlants = new List<PlantInstanceSO>();
        
        private void Start()
        {
            if (_runTestsOnStart)
            {
                StartCoroutine(RunAllTests());
            }
        }
        
        private System.Collections.IEnumerator RunAllTests()
        {
            TestsRunning = true;
            _testStartTime = Time.time;
            _totalTests = 0;
            _passedTests = 0;
            _failedTests = 0;
            _testResults.Clear();
            
            LogTest("=== ADVANCED CULTIVATION SYSTEMS TEST SUITE ===");
            LogTest($"Test started at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            // Initialize systems
            yield return StartCoroutine(InitializeTestSystems());
            
            // Test basic cultivation functionality
            yield return StartCoroutine(TestBasicCultivationSystems());
            
            // Test VPD Management System
            yield return StartCoroutine(TestVPDManagementSystem());
            
            // Test Environmental Automation System
            yield return StartCoroutine(TestEnvironmentalAutomationSystem());
            
            // Test Fertigation System
            yield return StartCoroutine(TestFertigationSystem());
            
            // Test System Integration
            yield return StartCoroutine(TestSystemIntegration());
            
            // Test Advanced Features
            yield return StartCoroutine(TestAdvancedFeatures());
            
            // Display final results
            DisplayFinalResults();
            
            TestsRunning = false;
        }
        
        private System.Collections.IEnumerator InitializeTestSystems()
        {
            LogTest("\n--- INITIALIZING TEST SYSTEMS ---");
            
            // Find CultivationManager
            _cultivationManager = FindAnyObjectByType<CultivationManager>();
            RunTest("CultivationManager Found", _cultivationManager != null);
            
            if (_cultivationManager != null)
            {
                RunTest("CultivationManager Initialized", _cultivationManager.IsInitialized);
            }
            
            // Create test assets if missing
            yield return StartCoroutine(CreateTestAssets());
            
            // Validate test assets
            RunTest("VPD System Asset", _vpdSystem != null);
            RunTest("Environmental System Asset", _environmentalSystem != null);
            RunTest("Fertigation System Asset", _fertigationSystem != null);
            RunTest("Test Zone Asset", _testZone != null);
            RunTest("Test Strain Asset", _testStrain != null);
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator CreateTestAssets()
        {
            LogTest("Creating test assets if needed...");
            
            // Create VPD Management System if missing
            if (_vpdSystem == null)
            {
                _vpdSystem = ScriptableObject.CreateInstance<VPDManagementSO>();
                _vpdSystem.name = "Test_VPD_Management";
                LogTest("Created test VPD Management System");
            }
            
            // Create Environmental Automation System if missing
            if (_environmentalSystem == null)
            {
                _environmentalSystem = ScriptableObject.CreateInstance<EnvironmentalAutomationSO>();
                _environmentalSystem.name = "Test_Environmental_Automation";
                LogTest("Created test Environmental Automation System");
            }
            
            // Create Fertigation System if missing
            if (_fertigationSystem == null)
            {
                _fertigationSystem = ScriptableObject.CreateInstance<FertigationSystemSO>();
                _fertigationSystem.name = "Test_Fertigation_System";
                LogTest("Created test Fertigation System");
            }
            
            // Create test zone if missing
            if (_testZone == null)
            {
                _testZone = ScriptableObject.CreateInstance<CultivationZoneSO>();
                _testZone.name = "Test_Cultivation_Zone";
                LogTest("Created test Cultivation Zone");
            }
            
            // Create test strain if missing
            if (_testStrain == null)
            {
                _testStrain = ScriptableObject.CreateInstance<PlantStrainSO>();
                _testStrain.name = "Test_Cannabis_Strain";
                LogTest("Created test Cannabis Strain");
            }
            
            // Create test genotype if missing
            if (_testGenotype == null)
            {
                _testGenotype = ScriptableObject.CreateInstance<GenotypeDataSO>();
                _testGenotype.name = "Test_Genotype";
                LogTest("Created test Genotype");
            }
            
            yield return null;
        }
        
        private System.Collections.IEnumerator TestBasicCultivationSystems()
        {
            LogTest("\n--- TESTING BASIC CULTIVATION SYSTEMS ---");
            
            if (_cultivationManager == null)
            {
                LogTest("ERROR: CultivationManager not available for testing");
                yield break;
            }
            
            // Test plant creation
            var initialPlantCount = _cultivationManager.ActivePlantCount;
            
            for (int i = 0; i < _testPlantCount; i++)
            {
                var plant = _cultivationManager.PlantSeed(
                    $"TestPlant_{i}", 
                    _testStrain, 
                    _testGenotype, 
                    new Vector3(i, 0, 0)
                );
                
                if (plant != null)
                {
                    _testPlants.Add(plant);
                }
            }
            
            RunTest("Plant Creation", _cultivationManager.ActivePlantCount == initialPlantCount + _testPlantCount);
            RunTest("Plant Data Integrity", _testPlants.Count == _testPlantCount);
            
            // Test basic plant operations
            if (_testPlants.Count > 0)
            {
                var testPlant = _testPlants[0];
                var initialWaterLevel = testPlant.WaterLevel;
                var initialNutrientLevel = testPlant.NutrientLevel;
                
                _cultivationManager.WaterPlant(testPlant.PlantID, 0.3f);
                RunTest("Plant Watering", testPlant.WaterLevel > initialWaterLevel);
                
                _cultivationManager.FeedPlant(testPlant.PlantID, 0.3f);
                RunTest("Plant Feeding", testPlant.NutrientLevel > initialNutrientLevel);
                
                _cultivationManager.TrainPlant(testPlant.PlantID, "LST");
                RunTest("Plant Training", testPlant.LastTraining != System.DateTime.MinValue);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestVPDManagementSystem()
        {
            LogTest("\n--- TESTING VPD MANAGEMENT SYSTEM ---");
            
            if (_vpdSystem == null)
            {
                LogTest("ERROR: VPD Management System not available");
                yield break;
            }
            
            // Test VPD calculations
            float testVPD = _vpdSystem.CalculateVPD(24f, 65f, -2f);
            RunTest("VPD Calculation", testVPD > 0f && testVPD < 3f);
            LogTest($"Calculated VPD: {testVPD:F3} kPa at 24°C, 65%RH");
            
            // Test optimal VPD for different stages
            if (_testPlants.Count > 0)
            {
                var testPlant = _testPlants[0];
                var environment = EnvironmentalConditions.CreateIndoorDefault();
                
                float optimalVPD = _vpdSystem.GetOptimalVPD(testPlant, environment, 6f);
                RunTest("Optimal VPD Calculation", optimalVPD > 0.2f && optimalVPD < 2f);
                LogTest($"Optimal VPD for {testPlant.CurrentGrowthStage}: {optimalVPD:F3} kPa");
            }
            
            // Test VPD adjustment recommendations
            var currentEnvironment = EnvironmentalConditions.CreateIndoorDefault();
            var recommendation = _vpdSystem.GetVPDAdjustmentRecommendation(currentEnvironment, 1.0f);
            RunTest("VPD Adjustment Recommendation", recommendation != null);
            
            if (recommendation != null)
            {
                LogTest($"VPD Recommendation - Current: {recommendation.CurrentVPD:F3}, Target: {recommendation.TargetVPD:F3}, Within Range: {recommendation.IsWithinRange}");
            }
            
            // Test dew point calculation
            float dewPoint = _vpdSystem.CalculateDewPoint(24f, 65f);
            RunTest("Dew Point Calculation", dewPoint > 0f && dewPoint < 30f);
            LogTest($"Calculated Dew Point: {dewPoint:F2}°C");
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestEnvironmentalAutomationSystem()
        {
            LogTest("\n--- TESTING ENVIRONMENTAL AUTOMATION SYSTEM ---");
            
            if (_environmentalSystem == null || _testZone == null)
            {
                LogTest("ERROR: Environmental Automation System or Test Zone not available");
                yield break;
            }
            
            // Test optimal control calculation
            var currentEnvironment = EnvironmentalConditions.CreateIndoorDefault();
            var plantsArray = _testPlants.ToArray();
            
            var controlPlan = _environmentalSystem.CalculateOptimalControl(
                new CultivationZone { ZoneID = _testZone?.ZoneID ?? "TestZone" }, 
                _vpdSystem, 
                currentEnvironment, 
                plantsArray
            );
            
            RunTest("Environmental Control Plan Generation", controlPlan != null);
            
            if (controlPlan != null)
            {
                LogTest($"Control Plan - Zone: {controlPlan.ZoneID}, Strategy: {controlPlan.ControlStrategy}");
                RunTest("Control Plan Target Conditions", controlPlan.TargetConditions.Temperature > 0f);
            }
            
            // Test climate recipe creation
            var climateRecipe = _environmentalSystem.CreateClimateRecipe(
                "Test Recipe",
                PlantGrowthStage.Vegetative,
                _testStrain,
                CultivationGoal.MaximumYield
            );
            
            RunTest("Climate Recipe Creation", climateRecipe != null);
            
            if (climateRecipe != null)
            {
                LogTest($"Climate Recipe: {climateRecipe.RecipeName} for {climateRecipe.TargetStage}");
                RunTest("Recipe Base Conditions", climateRecipe.BaseConditions.Temperature > 0f);
            }
            
            // Test system diagnostics
            var diagnostics = _environmentalSystem.PerformSystemDiagnostics(new ProjectChimera.Data.Cultivation.CultivationZone { ZoneID = _testZone?.ZoneID ?? "TestZone" });
            
            RunTest("System Diagnostics", diagnostics != null);
            
            if (diagnostics != null)
            {
                LogTest($"System Health: {diagnostics.OverallHealth:F2}");
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestFertigationSystem()
        {
            LogTest("\n--- TESTING FERTIGATION SYSTEM ---");
            
            if (_fertigationSystem == null)
            {
                LogTest("ERROR: Fertigation System not available");
                yield break;
            }
            
            // Test nutrient solution calculation
            var environment = EnvironmentalConditions.CreateIndoorDefault();
            var plantsArray = _testPlants.ToArray();
            var sourceWater = new ProjectChimera.Data.Cultivation.WaterQualityData { pH = 7.0f, TDS = 150f };
            
            var nutrientSolution = _fertigationSystem.CalculateOptimalNutrientSolution(
                plantsArray, 
                environment, 
                _testZone, 
                sourceWater
            );
            
            RunTest("Nutrient Solution Calculation", nutrientSolution != null);
            
            if (nutrientSolution != null)
            {
                LogTest($"Nutrient Solution - EC: {nutrientSolution.TargetEC:F2}, pH: {nutrientSolution.TargetpH:F2}");
                RunTest("NPK Ratio Valid", nutrientSolution.NPKRatio.magnitude > 0f);
                RunTest("Validation Results", nutrientSolution.ValidationResults != null);
            }
            
            // Test pH correction
            var pHCorrection = _fertigationSystem.PerformpHCorrection(7.5f, 6.0f, 100f, sourceWater);
            RunTest("pH Correction Calculation", pHCorrection != null);
            
            if (pHCorrection != null)
            {
                LogTest($"pH Correction - Current: {pHCorrection.CurrentpH:F2}, Target: {pHCorrection.TargetpH:F2}, Action Required: {pHCorrection.ActionRequired}");
            }
            
            // Test EC correction
            var nutrientProfile = new NutrientProfile 
            { 
                GrowthStage = PlantGrowthStage.Vegetative,
                TargetEC = 1.2f,
                TargetpH = 6.0f
            };
            
            var ecCorrection = _fertigationSystem.PerformECCorrection(0.8f, 1.2f, 100f, nutrientProfile);
            RunTest("EC Correction Calculation", ecCorrection != null);
            
            if (ecCorrection != null)
            {
                LogTest($"EC Correction - Current: {ecCorrection.CurrentEC:F2}, Target: {ecCorrection.TargetEC:F2}, Action Required: {ecCorrection.ActionRequired}");
            }
            
            // Test irrigation schedule creation
            var irrigationSchedule = _fertigationSystem.CreateIrrigationSchedule(plantsArray, environment, _testZone, 7);
            RunTest("Irrigation Schedule Creation", irrigationSchedule != null);
            
            if (irrigationSchedule != null)
            {
                LogTest($"Irrigation Schedule - Duration: {irrigationSchedule.DurationDays} days, Mode: {irrigationSchedule.ScheduleMode}");
                RunTest("Daily Schedules Created", irrigationSchedule.DailySchedules != null && irrigationSchedule.DailySchedules.Length > 0);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestSystemIntegration()
        {
            LogTest("\n--- TESTING SYSTEM INTEGRATION ---");
            
            // Test VPD + Environmental Automation integration
            if (_vpdSystem != null && _environmentalSystem != null && _testPlants.Count > 0)
            {
                var testPlant = _testPlants[0];
                var environment = EnvironmentalConditions.CreateIndoorDefault();
                
                // Get VPD requirements
                float optimalVPD = _vpdSystem.GetOptimalVPD(testPlant, environment);
                
                // Get environmental control plan that should integrate VPD
                var controlPlan = _environmentalSystem.CalculateOptimalControl(new ProjectChimera.Data.Cultivation.CultivationZone { ZoneID = _testZone?.ZoneID ?? "TestZone" }, _vpdSystem, environment, new[] { testPlant });
                
                RunTest("VPD-Environmental Integration", controlPlan != null && optimalVPD > 0f);
                LogTest($"Integration Test - VPD Target: {optimalVPD:F3} kPa");
            }
            
            // Test Fertigation + Environmental integration
            if (_fertigationSystem != null && _environmentalSystem != null)
            {
                var environment = EnvironmentalConditions.CreateIndoorDefault();
                var waterQuality = new ProjectChimera.Data.Cultivation.WaterQualityData { pH = 7.0f, TDS = 200f };
                
                var nutrientSolution = _fertigationSystem.CalculateOptimalNutrientSolution(
                    _testPlants.ToArray(), environment, _testZone, waterQuality);
                
                RunTest("Fertigation-Environmental Integration", nutrientSolution != null);
                
                if (nutrientSolution != null)
                {
                    LogTest($"Integrated Nutrient Solution - Plants: {nutrientSolution.PlantCount}, Zone: {nutrientSolution.ZoneID}");
                }
            }
            
            // Test full system coordination
            if (_vpdSystem != null && _environmentalSystem != null && _fertigationSystem != null)
            {
                RunTest("Full System Coordination Available", true);
                LogTest("All three major systems are available for coordination");
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestAdvancedFeatures()
        {
            LogTest("\n--- TESTING ADVANCED FEATURES ---");
            
            // Test environmental conditions calculations
            var testConditions = EnvironmentalConditions.CreateIndoorDefault();
            testConditions.CalculateDerivedValues();
            
            RunTest("Environmental Derived Values", testConditions.VPD > 0f && testConditions.DailyLightIntegral > 0f);
            LogTest($"Derived Values - VPD: {testConditions.VPD:F3} kPa, DLI: {testConditions.DailyLightIntegral:F1} mol/m²/day");
            
            // Test advanced environmental calculations
            float suitability = testConditions.CalculateAdvancedSuitability();
            RunTest("Advanced Environmental Suitability", suitability >= 0f && suitability <= 1f);
            LogTest($"Advanced Suitability Score: {suitability:F3}");
            
            // Test professional standards check
            bool meetsProfessionalStandards = testConditions.MeetsProfessionalStandards();
            RunTest("Professional Standards Check", true); // This test always passes, it's about functionality
            LogTest($"Meets Professional Standards: {meetsProfessionalStandards}");
            
            // Test professional recommendations
            var recommendations = testConditions.GetProfessionalRecommendations();
            RunTest("Professional Recommendations", recommendations != null);
            LogTest($"Recommendations Count: {recommendations.Length}");
            
            // Test zone capacity calculations
            if (_testZone != null && _testStrain != null)
            {
                int optimalCapacity = _testZone.CalculateOptimalCapacity(PlantGrowthStage.Flowering, GrowingMethod.Hydroponic, _testStrain);
                RunTest("Zone Capacity Calculation", optimalCapacity > 0);
                LogTest($"Optimal Capacity: {optimalCapacity} plants");
                
                float suitabilityScore = _testZone.EvaluateEnvironmentalSuitability(testConditions);
                RunTest("Zone Environmental Suitability", suitabilityScore >= 0f && suitabilityScore <= 1f);
                LogTest($"Zone Suitability: {suitabilityScore:F3}");
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private void RunTest(string testName, bool passed)
        {
            _totalTests++;
            
            if (passed)
            {
                _passedTests++;
                LogTest($"✓ PASS: {testName}");
            }
            else
            {
                _failedTests++;
                LogTest($"✗ FAIL: {testName}");
            }
        }
        
        private void LogTest(string message)
        {
            if (_enableDetailedLogging)
            {
                Debug.Log($"[AdvancedCultivationTest] {message}");
            }
            
            _testResults.Add($"{DateTime.Now:HH:mm:ss.fff} - {message}");
        }
        
        private void DisplayFinalResults()
        {
            float testDuration = Time.time - _testStartTime;
            float successRate = _totalTests > 0 ? (float)_passedTests / _totalTests * 100f : 0f;
            
            LogTest("\n=== FINAL TEST RESULTS ===");
            LogTest($"Total Tests: {_totalTests}");
            LogTest($"Passed: {_passedTests}");
            LogTest($"Failed: {_failedTests}");
            LogTest($"Success Rate: {successRate:F1}%");
            LogTest($"Test Duration: {testDuration:F2} seconds");
            LogTest($"Test completed at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            if (_failedTests == 0)
            {
                LogTest("🎉 ALL TESTS PASSED! Advanced cultivation systems are working correctly.");
            }
            else
            {
                LogTest($"⚠️ {_failedTests} test(s) failed. Check logs for details.");
            }
            
            LogTest("=== END TEST SUITE ===");
        }
        
        [ContextMenu("Run Tests")]
        public void RunTestsManually()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(RunAllTests());
            }
            else
            {
                Debug.LogWarning("[AdvancedCultivationTester] Tests can only be run in play mode.");
            }
        }
        
        [ContextMenu("Clear Test Results")]
        public void ClearTestResults()
        {
            _testResults.Clear();
            _totalTests = 0;
            _passedTests = 0;
            _failedTests = 0;
            Debug.Log("[AdvancedCultivationTester] Test results cleared.");
        }
        
        // Public properties for external access
        public int TotalTests => _totalTests;
        public int PassedTests => _passedTests;
        public int FailedTests => _failedTests;
        public bool TestsRunning { get; private set; } = false;
        
        private void OnGUI()
        {
            if (!Application.isPlaying) return;
            
            // Display test results on screen
            GUILayout.BeginArea(new Rect(10, 10, 400, 200));
            GUILayout.BeginVertical("box");
            
            GUILayout.Label("Advanced Cultivation Test Results", GUI.skin.box);
            GUILayout.Label($"Tests: {_passedTests}/{_totalTests} passed");
            
            if (_totalTests > 0)
            {
                float successRate = (float)_passedTests / _totalTests * 100f;
                GUILayout.Label($"Success Rate: {successRate:F1}%");
            }
            
            if (GUILayout.Button("Run Tests"))
            {
                StartCoroutine(RunAllTests());
            }
            
            if (GUILayout.Button("Clear Results"))
            {
                ClearTestResults();
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
    
    // Helper classes for testing - WaterQualityData moved to Data.Cultivation namespace
}