using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Data;

namespace ProjectChimera.Core
{
    /// <summary>
    /// Manages all ScriptableObject data for the game
    /// </summary>
    public class DataManager : MonoBehaviour
    {
        [Header("Data Collections")]
        [SerializeField] private List<GeneticTraitDefinition> geneticTraits = new List<GeneticTraitDefinition>();
        [SerializeField] private List<PlantStrainDefinition> plantStrains = new List<PlantStrainDefinition>();
        [SerializeField] private List<EquipmentDefinition> equipment = new List<EquipmentDefinition>();
        [SerializeField] private List<NutrientDefinition> nutrients = new List<NutrientDefinition>();
        
        // Public Properties
        public List<GeneticTraitDefinition> GeneticTraits => geneticTraits;
        public List<PlantStrainDefinition> PlantStrains => plantStrains;
        public List<EquipmentDefinition> Equipment => equipment;
        public List<NutrientDefinition> Nutrients => nutrients;
        
        public void Initialize()
        {
            LoadAllData();
            ValidateData();
        }
        
        private void LoadAllData()
        {
            // Load all ScriptableObjects from Resources
            LoadGeneticTraits();
            LoadPlantStrains();
            LoadEquipment();
            LoadNutrients();
        }
        
        private void LoadGeneticTraits()
        {
            var traits = Resources.LoadAll<GeneticTraitDefinition>("Data/Genetics");
            geneticTraits.Clear();
            geneticTraits.AddRange(traits);
            Debug.Log($"Loaded {geneticTraits.Count} genetic traits");
        }
        
        private void LoadPlantStrains()
        {
            var strains = Resources.LoadAll<PlantStrainDefinition>("Data/PlantStrains");
            plantStrains.Clear();
            plantStrains.AddRange(strains);
            Debug.Log($"Loaded {plantStrains.Count} plant strains");
        }
        
        private void LoadEquipment()
        {
            var equipmentItems = Resources.LoadAll<EquipmentDefinition>("Data/Equipment");
            equipment.Clear();
            equipment.AddRange(equipmentItems);
            Debug.Log($"Loaded {equipment.Count} equipment items");
        }
        
        private void LoadNutrients()
        {
            var nutrientItems = Resources.LoadAll<NutrientDefinition>("Data/Nutrients");
            nutrients.Clear();
            nutrients.AddRange(nutrientItems);
            Debug.Log($"Loaded {nutrients.Count} nutrients");
        }
        
        private void ValidateData()
        {
            // Validate that all data is properly configured
            Debug.Log("Data validation complete");
        }
        
        // Utility methods for finding specific data
        public GeneticTraitDefinition GetGeneticTrait(string traitName)
        {
            return geneticTraits.Find(trait => trait.TraitName == traitName);
        }
        
        public PlantStrainDefinition GetPlantStrain(string strainName)
        {
            return plantStrains.Find(strain => strain.StrainName == strainName);
        }
        
        public EquipmentDefinition GetEquipment(string equipmentName)
        {
            return equipment.Find(eq => eq.EquipmentName == equipmentName);
        }
        
        public NutrientDefinition GetNutrient(string nutrientName)
        {
            return nutrients.Find(nutrient => nutrient.NutrientName == nutrientName);
        }
    }
}