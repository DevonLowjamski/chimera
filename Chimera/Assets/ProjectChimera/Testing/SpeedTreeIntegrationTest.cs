using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Visuals;
using ProjectChimera.Data.Genetics;
using ProjectChimera.Systems.Visuals;
using PlantGrowthStage = ProjectChimera.Data.Genetics.PlantGrowthStage;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Integration test for SpeedTree architecture without requiring actual SpeedTree assets.
    /// Tests the core integration points and data flow.
    /// </summary>
    public class SpeedTreeIntegrationTest : MonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private bool _runTestOnStart = false;
        [SerializeField] private PlantSpeciesSO _testSpecies;
        [SerializeField] private PlantStrainSO _testStrain;

        public void Start()
        {
            if (_runTestOnStart)
            {
                RunIntegrationTest();
            }
        }

        [ContextMenu("Run SpeedTree Integration Test")]
        public void RunIntegrationTest()
        {
            Debug.Log("[SpeedTree Test] Starting integration test...");

            bool allTestsPassed = true;

            allTestsPassed &= TestManagerInitialization();
            allTestsPassed &= TestParameterCalculation();
            allTestsPassed &= TestGrowthStageConfiguration();
            allTestsPassed &= TestEnvironmentalMapping();

            if (allTestsPassed)
            {
                Debug.Log("[SpeedTree Test] ✅ All integration tests PASSED!");
            }
            else
            {
                Debug.LogError("[SpeedTree Test] ❌ Some integration tests FAILED!");
            }
        }

        private bool TestManagerInitialization()
        {
            Debug.Log("[SpeedTree Test] Testing PlantVisualizationManager initialization...");

            var manager = FindAnyObjectByType<PlantVisualizationManager>();
            if (manager == null)
            {
                Debug.LogError("[SpeedTree Test] PlantVisualizationManager not found in scene!");
                return false;
            }

            if (!manager.IsInitialized)
            {
                Debug.LogError("[SpeedTree Test] PlantVisualizationManager is not initialized!");
                return false;
            }

            Debug.Log("[SpeedTree Test] ✅ Manager initialization test passed");
            return true;
        }

        private bool TestParameterCalculation()
        {
            Debug.Log("[SpeedTree Test] Testing parameter calculation system...");

            // Create test parameter mapper
            var mapper = ScriptableObject.CreateInstance<SpeedTreeParameterMapperSO>();
            
            // Create test cultivation conditions
            var conditions = new ProjectChimera.Data.Visuals.CultivationConditions
            {
                Temperature = 24f,
                Humidity = 55f,
                LightIntensity = 400f,
                CO2Level = 800f,
                NitrogenLevel = 0.8f,
                PhosphorusLevel = 0.8f,
                PotassiumLevel = 0.8f
            };

            try
            {
                var parameters = mapper.CalculateSpeedTreeParameters(conditions, _testStrain);
                
                if (parameters.OverallHealth < 0f || parameters.OverallHealth > 1f)
                {
                    Debug.LogError("[SpeedTree Test] Invalid OverallHealth value: " + parameters.OverallHealth);
                    return false;
                }

                Debug.Log($"[SpeedTree Test] Calculated parameters - Health: {parameters.OverallHealth:F2}, Growth: {parameters.GrowthRate:F2}");
                Debug.Log("[SpeedTree Test] ✅ Parameter calculation test passed");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("[SpeedTree Test] Parameter calculation failed: " + e.Message);
                return false;
            }
            finally
            {
                DestroyImmediate(mapper);
            }
        }

        private bool TestGrowthStageConfiguration()
        {
            Debug.Log("[SpeedTree Test] Testing growth stage configuration...");

            var config = ScriptableObject.CreateInstance<GrowthStageConfigurationSO>();
            
            try
            {
                // Test seasonal parameter calculation
                float seedlingParam = config.GetSeasonalParameterForStage(PlantGrowthStage.Seedling);
                float floweringParam = config.GetSeasonalParameterForStage(PlantGrowthStage.Flowering);
                
                if (seedlingParam < 0f || seedlingParam > 1f || floweringParam < 0f || floweringParam > 1f)
                {
                    Debug.LogError("[SpeedTree Test] Invalid seasonal parameters!");
                    return false;
                }

                Debug.Log($"[SpeedTree Test] Seasonal parameters - Seedling: {seedlingParam:F2}, Flowering: {floweringParam:F2}");
                Debug.Log("[SpeedTree Test] ✅ Growth stage configuration test passed");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("[SpeedTree Test] Growth stage configuration failed: " + e.Message);
                return false;
            }
            finally
            {
                DestroyImmediate(config);
            }
        }

        private bool TestEnvironmentalMapping()
        {
            Debug.Log("[SpeedTree Test] Testing environmental data mapping...");

            if (_testSpecies == null)
            {
                Debug.LogWarning("[SpeedTree Test] No test species assigned, skipping environmental mapping test");
                return true;
            }

            try
            {
                var optimal = _testSpecies.GetOptimalEnvironment();
                
                if (optimal.Temperature <= 0f || optimal.Humidity <= 0f)
                {
                    Debug.LogError("[SpeedTree Test] Invalid optimal environment values!");
                    return false;
                }

                var testEnv = ProjectChimera.Data.Cultivation.EnvironmentalConditions.CreateIndoorDefault();
                float suitability = _testSpecies.EvaluateEnvironmentalSuitability(testEnv);
                
                if (suitability < 0f || suitability > 1f)
                {
                    Debug.LogError("[SpeedTree Test] Invalid suitability score: " + suitability);
                    return false;
                }

                Debug.Log($"[SpeedTree Test] Environmental mapping - Suitability: {suitability:F2}");
                Debug.Log("[SpeedTree Test] ✅ Environmental mapping test passed");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("[SpeedTree Test] Environmental mapping failed: " + e.Message);
                return false;
            }
        }

        [ContextMenu("Test Instance Creation (Mock)")]
        public void TestMockInstanceCreation()
        {
            Debug.Log("[SpeedTree Test] Testing mock instance creation...");

            var manager = FindAnyObjectByType<PlantVisualizationManager>();
            if (manager == null || _testStrain == null)
            {
                Debug.LogError("[SpeedTree Test] Manager or test strain not available!");
                return;
            }

            // This will fail gracefully without SpeedTree assets, but tests the call path
            var instance = manager.CreatePlantInstance("test_001", _testStrain, Vector3.zero);
            
            if (instance == null)
            {
                Debug.LogWarning("[SpeedTree Test] Instance creation failed (expected without SpeedTree assets)");
            }
            else
            {
                Debug.Log("[SpeedTree Test] ✅ Mock instance created successfully!");
            }
        }
    }
}