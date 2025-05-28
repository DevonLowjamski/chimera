using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Data;

namespace ProjectChimera.Cultivation
{
    /// <summary>
    /// Represents an individual plant instance with its own genetics and growth state
    /// </summary>
    public class PlantInstance : MonoBehaviour
    {
        [Header("Plant Identity")]
        [SerializeField] private PlantStrainDefinition strain;
        [SerializeField] private string plantID;
        
        [Header("Current State")]
        [SerializeField] private GrowthStage currentGrowthStage = GrowthStage.Seedling;
        [SerializeField] private float growthProgress = 0f;
        [SerializeField] private float health = 100f;
        [SerializeField] private float size = 0.1f;
        
        [Header("Genetics")]
        [SerializeField] private List<ExpressedTrait> expressedTraits = new List<ExpressedTrait>();
        
        [Header("Environmental Factors")]
        [SerializeField] private float currentTemperature = 24f;
        [SerializeField] private float currentHumidity = 60f;
        [SerializeField] private float currentLightIntensity = 1f;
        [SerializeField] private float currentNutrientLevel = 1f;
        
        // Events
        public System.Action<GrowthStage> OnGrowthStageChanged;
        public System.Action<float> OnHealthChanged;
        
        // Properties
        public PlantStrainDefinition Strain => strain;
        public string PlantID => plantID;
        public GrowthStage CurrentGrowthStage => currentGrowthStage;
        public float GrowthProgress => growthProgress;
        public float Health => health;
        public float Size => size;
        public List<ExpressedTrait> ExpressedTraits => expressedTraits;
        
        private void Start()
        {
            if (string.IsNullOrEmpty(plantID))
                plantID = System.Guid.NewGuid().ToString();
            
            InitializeGenetics();
        }
        
        private void Update()
        {
            UpdateGrowth();
            UpdateHealth();
        }        
        public void Initialize(PlantStrainDefinition plantStrain)
        {
            strain = plantStrain;
            plantID = System.Guid.NewGuid().ToString();
            InitializeGenetics();
        }
        
        private void InitializeGenetics()
        {
            if (strain == null) return;
            
            expressedTraits.Clear();
            foreach (var baseTrait in strain.BaseTraits)
            {
                var expressedTrait = new ExpressedTrait
                {
                    trait = baseTrait.trait,
                    expressedValue = CalculateExpressedValue(baseTrait)
                };
                expressedTraits.Add(expressedTrait);
            }
        }
        
        private float CalculateExpressedValue(GeneticTraitValue baseTrait)
        {
            // Add some genetic variation
            float variation = Random.Range(-0.1f, 0.1f);
            float expressedValue = baseTrait.value + variation;
            
            // Clamp to trait bounds
            expressedValue = Mathf.Clamp(expressedValue, 
                baseTrait.trait.MinValue, 
                baseTrait.trait.MaxValue);
            
            return expressedValue;
        }
        
        private void UpdateGrowth()
        {
            float growthRate = CalculateGrowthRate();
            growthProgress += growthRate * Time.deltaTime;
            
            CheckGrowthStageProgression();
            UpdateSize();
        }
        
        private float CalculateGrowthRate()
        {
            float baseRate = GetTraitValue(TraitType.Yield_Potential) * 0.01f;
            
            // Default to reasonable growth rate if trait is missing
            if (baseRate <= 0f)
                baseRate = 0.01f;
            
            float tempModifier = CalculateTemperatureModifier();
            float humidityModifier = CalculateHumidityModifier();
            float lightModifier = Mathf.Max(0.1f, currentLightIntensity);
            float nutrientModifier = Mathf.Max(0.1f, currentNutrientLevel);
            
            float growthRate = baseRate * tempModifier * humidityModifier * lightModifier * nutrientModifier;
            
            // Ensure we don't get invalid values
            return Mathf.Clamp(growthRate, 0f, 0.1f);
        }        
        private float CalculateTemperatureModifier()
        {
            if (strain == null) return 0.5f;
            
            float optimal = strain.OptimalTemperature;
            float difference = Mathf.Abs(currentTemperature - optimal);
            return Mathf.Max(0.1f, 1f - (difference / 10f));
        }
        
        private float CalculateHumidityModifier()
        {
            if (strain == null) return 0.5f;
            
            float optimal = strain.OptimalHumidity;
            float difference = Mathf.Abs(currentHumidity - optimal);
            return Mathf.Max(0.1f, 1f - (difference / 20f));
        }
        
        private void CheckGrowthStageProgression()
        {
            GrowthStage newStage = currentGrowthStage;
            
            switch (currentGrowthStage)
            {
                case GrowthStage.Seedling:
                    if (growthProgress >= 1f) newStage = GrowthStage.Vegetative;
                    break;
                case GrowthStage.Vegetative:
                    if (growthProgress >= 2f) newStage = GrowthStage.Flowering;
                    break;
            }
            
            if (newStage != currentGrowthStage)
            {
                currentGrowthStage = newStage;
                OnGrowthStageChanged?.Invoke(newStage);
            }
        }
        
        private void UpdateSize()
        {
            float maxSize = GetTraitValue(TraitType.Plant_Height);
            
            // Ensure we have a valid max size (default to 1.5 if trait is missing)
            if (maxSize <= 0f)
                maxSize = 1.5f;
            
            size = Mathf.Lerp(0.1f, maxSize, growthProgress / 3f);
            
            // Clamp size to reasonable bounds
            size = Mathf.Clamp(size, 0.01f, 10f);
            
            // Ensure we have valid scale values
            if (float.IsNaN(size) || float.IsInfinity(size))
                size = 0.1f;
            
            transform.localScale = Vector3.one * size;
        }
        
        private void UpdateHealth()
        {
            float healthDecay = 0.1f * Time.deltaTime;
            float environmentalStress = CalculateEnvironmentalStress();
            healthDecay += environmentalStress * Time.deltaTime;
            
            health = Mathf.Max(0f, health - healthDecay);
            
            if (health <= 0f)
            {
                Debug.Log($"Plant {plantID} has died!");
            }
        }        
        private float CalculateEnvironmentalStress()
        {
            if (strain == null) return 0.5f;
            
            float tempStress = Mathf.Abs(currentTemperature - strain.OptimalTemperature) / 10f;
            float humidityStress = Mathf.Abs(currentHumidity - strain.OptimalHumidity) / 20f;
            
            return Mathf.Clamp(tempStress + humidityStress, 0f, 2f);
        }
        
        public float GetTraitValue(TraitType traitType)
        {
            var trait = expressedTraits.Find(t => t.trait.Type == traitType);
            return trait?.expressedValue ?? 0f;
        }
        
        public void SetEnvironmentalConditions(float temperature, float humidity, float lightIntensity, float nutrientLevel)
        {
            currentTemperature = temperature;
            currentHumidity = humidity;
            currentLightIntensity = lightIntensity;
            currentNutrientLevel = nutrientLevel;
        }
    }
    
    [System.Serializable]
    public class ExpressedTrait
    {
        public GeneticTraitDefinition trait;
        public float expressedValue;
    }
}