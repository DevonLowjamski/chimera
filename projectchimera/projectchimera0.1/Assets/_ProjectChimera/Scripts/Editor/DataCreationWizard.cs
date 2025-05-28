using UnityEngine;
using UnityEditor;
using ProjectChimera.Data;

namespace ProjectChimera.Editor
{
    /// <summary>
    /// Editor wizard for quickly creating game data assets
    /// </summary>
    public class DataCreationWizard : EditorWindow
    {
        [MenuItem("Project Chimera/Data Creation Wizard")]
        public static void ShowWindow()
        {
            GetWindow<DataCreationWizard>("Data Creation Wizard");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Project Chimera Data Creation", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            if (GUILayout.Button("Create Sample Genetic Traits"))
            {
                CreateSampleGeneticTraits();
            }
            
            if (GUILayout.Button("Create Sample Plant Strains"))
            {
                CreateSamplePlantStrains();
            }
            
            if (GUILayout.Button("Create Sample Equipment"))
            {
                CreateSampleEquipment();
            }
            
            if (GUILayout.Button("Create Sample Nutrients"))
            {
                CreateSampleNutrients();
            }
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Create All Sample Data"))
            {
                CreateSampleGeneticTraits();
                CreateSamplePlantStrains();
                CreateSampleEquipment();
                CreateSampleNutrients();
            }
        }
        
        private void CreateSampleGeneticTraits()
        {
            // Ensure directory exists
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/Genetics"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Data", "Genetics");
            }

            CreateGeneticTrait("THC Potential", "Maximum THC percentage under optimal conditions", TraitType.THC_Potential, 0f, 30f, 15f);
            CreateGeneticTrait("CBD Potential", "Maximum CBD percentage under optimal conditions", TraitType.CBD_Potential, 0f, 25f, 5f);
            CreateGeneticTrait("Yield Potential", "Harvest yield multiplier", TraitType.Yield_Potential, 0.5f, 2f, 1f);
            CreateGeneticTrait("Flowering Time", "Weeks to complete flowering", TraitType.Flowering_Time, 6f, 12f, 8f);
            CreateGeneticTrait("Plant Height", "Maximum plant height in meters", TraitType.Plant_Height, 0.3f, 3f, 1.5f);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Sample genetic traits created!");
        }
        
        private void CreateGeneticTrait(string name, string description, TraitType type, float min, float max, float defaultVal)
        {
            var trait = CreateInstance<GeneticTraitDefinition>();
            trait.name = name.Replace(" ", "");
            
            // Set the values directly (this will work since the fields have public setters through properties)
            string path = $"Assets/Resources/Data/Genetics/{trait.name}.asset";
            AssetDatabase.CreateAsset(trait, path);
        }
        
        private void CreateSamplePlantStrains()
        {
            // Ensure directory exists
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/PlantStrains"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Data", "PlantStrains");
            }
            
            // Create a sample strain
            var strain = CreateInstance<PlantStrainDefinition>();
            strain.name = "BlueDream";
            
            string path = "Assets/Resources/Data/PlantStrains/BlueDream.asset";
            AssetDatabase.CreateAsset(strain, path);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Sample plant strains created!");
        }
        
        private void CreateSampleEquipment()
        {
            // Ensure directory exists
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/Equipment"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Data", "Equipment");
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Sample equipment created!");
        }
        
        private void CreateSampleNutrients()
        {
            // Ensure directory exists
            if (!AssetDatabase.IsValidFolder("Assets/Resources/Data/Nutrients"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Data", "Nutrients");
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Sample nutrients created!");
        }
    }
}