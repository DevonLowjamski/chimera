using UnityEngine;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Data.Cultivation;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Validation test for Error Wave 52 - Type conversion and method existence issues
    /// </summary>
    public class Wave52ValidationTest : MonoBehaviour
    {
        [Header("Wave 52 Validation")]
        [SerializeField] private bool _runValidation = false;
        
        private void Start()
        {
            if (_runValidation)
            {
                ValidateWave52Fixes();
            }
        }
        
        private void ValidateWave52Fixes()
        {
            Debug.Log("=== Wave 52 Validation Test ===");
            
            // Test 1: Verify PlantUpdateProcessor can be created
            try
            {
                var processor = new PlantUpdateProcessor();
                Debug.Log("✓ PlantUpdateProcessor creation successful");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ PlantUpdateProcessor creation failed: {e.Message}");
            }
            
            // Test 2: Verify EnvironmentalConditions types are accessible
            try
            {
                var conditions = EnvironmentalConditions.CreateIndoorDefault();
                Debug.Log("✓ EnvironmentalConditions.CreateIndoorDefault() successful");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ EnvironmentalConditions access failed: {e.Message}");
            }
            
            // Test 3: Verify type conversion methods exist (reflection check)
            try
            {
                var processorType = typeof(PlantUpdateProcessor);
                var convertMethod = processorType.GetMethod("ConvertEnvironmentToCultivationConditions", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (convertMethod != null)
                {
                    Debug.Log("✓ ConvertEnvironmentToCultivationConditions method exists");
                }
                else
                {
                    Debug.LogError("✗ ConvertEnvironmentToCultivationConditions method not found");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ Method reflection check failed: {e.Message}");
            }
            
            // Test 4: Verify GetPlantEnvironmentalConditions method exists
            try
            {
                var processorType = typeof(PlantUpdateProcessor);
                var getMethod = processorType.GetMethod("GetPlantEnvironmentalConditions", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (getMethod != null)
                {
                    Debug.Log("✓ GetPlantEnvironmentalConditions method exists");
                }
                else
                {
                    Debug.LogError("✗ GetPlantEnvironmentalConditions method not found");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ GetPlantEnvironmentalConditions reflection check failed: {e.Message}");
            }
            
            // Test 5: Verify type compatibility between Data and Systems EnvironmentalConditions
            try
            {
                var dataConditions = ProjectChimera.Data.Cultivation.EnvironmentalConditions.CreateIndoorDefault();
                var systemsConditions = new ProjectChimera.Systems.Cultivation.EnvironmentalConditions();
                Debug.Log("✓ Both EnvironmentalConditions types can be instantiated");
                Debug.Log($"   Data type: {dataConditions.GetType().FullName}");
                Debug.Log($"   Systems type: {systemsConditions.GetType().FullName}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ Type compatibility test failed: {e.Message}");
            }
            
            // Test 6: Verify PlantInstance.CurrentEnvironment property type
            try
            {
                var plantInstanceType = typeof(PlantInstance);
                var currentEnvProperty = plantInstanceType.GetProperty("CurrentEnvironment");
                if (currentEnvProperty != null)
                {
                    Debug.Log($"✓ PlantInstance.CurrentEnvironment property exists");
                    Debug.Log($"   Property type: {currentEnvProperty.PropertyType.FullName}");
                }
                else
                {
                    Debug.LogError("✗ PlantInstance.CurrentEnvironment property not found");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ PlantInstance property check failed: {e.Message}");
            }
            
            // Test 7: Verify EnvironmentalResponseSystem type compatibility
            try
            {
                var responseSystem = new EnvironmentalResponseSystem();
                var conditions = EnvironmentalConditions.CreateIndoorDefault();
                
                // Test ProcessEnvironmentalChange method
                responseSystem.ProcessEnvironmentalChange(conditions, conditions);
                Debug.Log("✓ EnvironmentalResponseSystem.ProcessEnvironmentalChange type compatibility confirmed");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"✗ EnvironmentalResponseSystem type compatibility failed: {e.Message}");
            }
            
            Debug.Log("=== Wave 52 Validation Complete ===");
        }
    }
}