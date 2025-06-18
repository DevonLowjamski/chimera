using UnityEngine;
using ProjectChimera.Data.Environment;
using ProjectChimera.Data.Genetics;
using System.Collections.Generic;

namespace ProjectChimera.Systems.Cultivation
{
    /// <summary>
    /// Processes plant updates including growth calculations, health management,
    /// and environmental response processing.
    /// </summary>
    public class PlantUpdateProcessor
    {
        private readonly bool _enableStressSystem;
        private readonly bool _enableGxEInteractions;
        
        public PlantUpdateProcessor(bool enableStressSystem = true, bool enableGxEInteractions = true)
        {
            _enableStressSystem = enableStressSystem;
            _enableGxEInteractions = enableGxEInteractions;
        }
        
        /// <summary>
        /// Updates a single plant's state including growth, health, and environmental responses.
        /// </summary>
        public void UpdatePlant(PlantInstance plant, float deltaTime, float globalGrowthModifier)
        {
            if (plant == null || !plant.IsActive)
                return;
            
            plant.UpdatePlant(deltaTime, globalGrowthModifier);
        }
    }
    
    /// <summary>
    /// Calculates plant growth rates and progression based on multiple factors.
    /// </summary>
    public class PlantGrowthCalculator
    {
        private PlantStrainSO _strain;
        private PhenotypicTraits _traits;
        private AnimationCurve _growthCurve;
        
        public void Initialize(PlantStrainSO strain, PhenotypicTraits traits)
        {
            _strain = strain;
            _traits = traits;
            
            // Initialize growth curve from strain data or use default
            _growthCurve = strain.GrowthCurve ?? CreateDefaultGrowthCurve();
        }
        
        /// <summary>
        /// Calculates the growth rate for the current frame.
        /// </summary>
        public float CalculateGrowthRate(PlantGrowthStage stage, float environmentalFitness, float health, float globalModifier)
        {
            // Base growth rate from strain genetics
            float baseRate = GetBaseGrowthRateForStage(stage);
            
            // Apply environmental fitness
            float environmentalModifier = Mathf.Lerp(0.2f, 1.5f, environmentalFitness);
            
            // Apply health modifier
            float healthModifier = Mathf.Lerp(0.1f, 1.2f, health);
            
            // Apply strain-specific growth characteristics
            float strainModifier = _strain?.GrowthRateModifier ?? 1f;
            
            // Apply phenotypic expression
            float phenotypeModifier = CalculatePhenotypeGrowthModifier(stage);
            
            // Combine all modifiers
            float finalRate = baseRate * environmentalModifier * healthModifier * strainModifier * phenotypeModifier * globalModifier;
            
            return Mathf.Max(0f, finalRate);
        }
        
        /// <summary>
        /// Calculates harvest results based on plant's final state.
        /// </summary>
        public HarvestResults CalculateHarvestResults(float finalHealth, float qualityPotential, PhenotypicTraits traits)
        {
            var results = new HarvestResults
            {
                FinalHealth = finalHealth,
                QualityScore = CalculateQualityScore(finalHealth, qualityPotential),
                FloweringDays = traits.FloweringTime,
                HarvestDate = System.DateTime.Now
            };
            
            // Calculate yield based on multiple factors
            results.TotalYieldGrams = CalculateYield(finalHealth, traits);
            
            // Calculate cannabinoid and terpene profiles
            results.Cannabinoids = CalculateCannabinoidProfile(finalHealth, qualityPotential);
            results.Terpenes = CalculateTerpeneProfile(finalHealth, qualityPotential);
            
            return results;
        }
        
        private float GetBaseGrowthRateForStage(PlantGrowthStage stage)
        {
            // Different stages have different base growth rates
            switch (stage)
            {
                case PlantGrowthStage.Seed:
                    return 0.005f; // Very slow initial growth
                case PlantGrowthStage.Germination:
                    return 0.01f;
                case PlantGrowthStage.Seedling:
                    return 0.015f;
                case PlantGrowthStage.Vegetative:
                    return 0.02f; // Peak growth rate
                case PlantGrowthStage.Flowering:
                    return 0.012f; // Slower during flowering
                case PlantGrowthStage.Harvest:
                    return 0f; // No growth when ready for harvest
                default:
                    return 0.01f;
            }
        }
        
        private float CalculatePhenotypeGrowthModifier(PlantGrowthStage stage)
        {
            if (_traits == null)
                return 1f;
            
            // Different traits affect growth at different stages
            switch (stage)
            {
                case PlantGrowthStage.Vegetative:
                    return Mathf.Lerp(0.8f, 1.3f, _traits.YieldMultiplier);
                case PlantGrowthStage.Flowering:
                    return Mathf.Lerp(0.9f, 1.2f, _traits.PotencyMultiplier);
                default:
                    return 1f;
            }
        }
        
        private float CalculateYield(float health, PhenotypicTraits traits)
        {
            float baseYield = _strain?.BaseYieldGrams ?? 100f;
            float healthModifier = Mathf.Lerp(0.3f, 1.2f, health);
            float traitModifier = traits.YieldMultiplier;
            
            return baseYield * healthModifier * traitModifier;
        }
        
        private float CalculateQualityScore(float health, float qualityPotential)
        {
            float healthComponent = health * 0.4f;
            float potentialComponent = qualityPotential * 0.6f;
            
            return Mathf.Clamp01(healthComponent + potentialComponent);
        }
        
        private CannabinoidProfile CalculateCannabinoidProfile(float health, float quality)
        {
            if (_strain?.CannabinoidProfile == null)
                return new CannabinoidProfile();
            
            var profile = new CannabinoidProfile
            {
                THC = _strain.CannabinoidProfile.ThcPercentage * health * quality,
                CBD = _strain.CannabinoidProfile.CbdPercentage * health * quality,
                CBG = _strain.CannabinoidProfile.CbgPercentage * health * quality,
                CBN = _strain.CannabinoidProfile.CbnPercentage * health * quality
            };
            
            return profile;
        }
        
        private TerpeneProfile CalculateTerpeneProfile(float health, float quality)
        {
            if (_strain?.TerpeneProfile == null)
                return new TerpeneProfile();
            
            var profile = new TerpeneProfile
            {
                Myrcene = _strain.TerpeneProfile.Myrcene * health * quality,
                Limonene = _strain.TerpeneProfile.Limonene * health * quality,
                Pinene = _strain.TerpeneProfile.Pinene * health * quality,
                Linalool = _strain.TerpeneProfile.Linalool * health * quality,
                Caryophyllene = _strain.TerpeneProfile.Caryophyllene * health * quality
            };
            
            return profile;
        }
        
        private AnimationCurve CreateDefaultGrowthCurve()
        {
            var curve = new AnimationCurve();
            curve.AddKey(0f, 0f);
            curve.AddKey(0.2f, 0.1f);
            curve.AddKey(0.5f, 0.5f);
            curve.AddKey(0.8f, 0.9f);
            curve.AddKey(1f, 1f);
            
            return curve;
        }
    }
    
    /// <summary>
    /// Manages plant health, stress responses, and disease resistance.
    /// </summary>
    public class PlantHealthSystem
    {
        private PlantStrainSO _strain;
        private float _currentHealth = 1f;
        private float _maxHealth = 1f;
        private float _stressLevel = 0f;
        private float _diseaseResistance = 1f;
        private float _recoveryRate = 0.1f;
        
        public void Initialize(PlantStrainSO strain, float diseaseResistance)
        {
            _strain = strain;
            _maxHealth = strain?.BaseHealthModifier ?? 1f;
            _currentHealth = _maxHealth;
            _diseaseResistance = diseaseResistance;
            _recoveryRate = strain?.HealthRecoveryRate ?? 0.1f;
        }
        
        /// <summary>
        /// Updates health based on stressors and environmental conditions.
        /// </summary>
        public void UpdateHealth(float deltaTime, List<ActiveStressor> stressors, float environmentalFitness)
        {
            // Calculate stress damage
            float stressDamage = CalculateStressDamage(stressors, deltaTime);
            
            // Calculate environmental health effects
            float environmentalEffect = CalculateEnvironmentalHealthEffect(environmentalFitness, deltaTime);
            
            // Apply natural recovery
            float naturalRecovery = _recoveryRate * deltaTime;
            
            // Update health
            float healthChange = environmentalEffect + naturalRecovery - stressDamage;
            _currentHealth = Mathf.Clamp(_currentHealth + healthChange, 0f, _maxHealth);
            
            // Update stress level
            UpdateStressLevel(stressors);
        }
        
        public float GetCurrentHealth() => _currentHealth;
        public float GetStressLevel() => _stressLevel;
        public float GetHealthPercentage() => _currentHealth / _maxHealth;
        
        private float CalculateStressDamage(List<ActiveStressor> stressors, float deltaTime)
        {
            float totalDamage = 0f;
            
            foreach (var stressor in stressors)
            {
                if (!stressor.IsActive)
                    continue;
                
                float damage = stressor.Intensity * stressor.StressSource.DamagePerSecond * deltaTime;
                
                // Apply disease resistance
                if (stressor.StressSource.StressType == ProjectChimera.Data.Environment.StressType.Biotic)
                {
                    damage *= (1f - _diseaseResistance);
                }
                
                totalDamage += damage;
            }
            
            return totalDamage;
        }
        
        private float CalculateEnvironmentalHealthEffect(float environmentalFitness, float deltaTime)
        {
            // Good environmental conditions promote health recovery
            if (environmentalFitness > 0.8f)
            {
                return (environmentalFitness - 0.8f) * 0.5f * deltaTime;
            }
            // Poor conditions cause slow health decline
            else if (environmentalFitness < 0.4f)
            {
                return (environmentalFitness - 0.4f) * 0.2f * deltaTime;
            }
            
            return 0f;
        }
        
        private void UpdateStressLevel(List<ActiveStressor> stressors)
        {
            _stressLevel = 0f;
            
            foreach (var stressor in stressors)
            {
                if (stressor.IsActive)
                {
                    _stressLevel += stressor.Intensity * stressor.StressSource.StressMultiplier;
                }
            }
            
            _stressLevel = Mathf.Clamp01(_stressLevel);
        }
    }
    
    /// <summary>
    /// Handles environmental response calculations and GxE interactions.
    /// </summary>
    public class EnvironmentalResponseSystem
    {
        private PlantStrainSO _strain;
        private GxE_ProfileSO _gxeProfile;
        private float _environmentalFitness = 1f;
        private EnvironmentalConditions _currentConditions;
        
        public void Initialize(PlantStrainSO strain)
        {
            _strain = strain;
            _gxeProfile = strain?.GxEProfile;
        }
        
        /// <summary>
        /// Updates environmental responses and calculates fitness.
        /// </summary>
        public void UpdateEnvironmentalResponse(EnvironmentalConditions conditions, float deltaTime)
        {
            _currentConditions = conditions;
            _environmentalFitness = CalculateEnvironmentalFitness(conditions);
        }
        
        /// <summary>
        /// Processes changes in environmental conditions.
        /// </summary>
        public void ProcessEnvironmentalChange(EnvironmentalConditions previous, EnvironmentalConditions current)
        {
            // Calculate stress from rapid environmental changes
            if (previous.Temperature != 0f) // Valid previous conditions
            {
                float tempChange = Mathf.Abs(current.Temperature - previous.Temperature);
                float humidityChange = Mathf.Abs(current.Humidity - previous.Humidity);
                
                // Rapid changes can cause stress
                if (tempChange > 5f || humidityChange > 20f)
                {
                    // This would trigger stress responses
                    Debug.Log($"Rapid environmental change detected - Temp: {tempChange:F1}°C, Humidity: {humidityChange:F1}%");
                }
            }
        }
        
        /// <summary>
        /// Processes environmental adaptation for the plant.
        /// </summary>
        public void ProcessAdaptation(EnvironmentalConditions conditions, float adaptationRate)
        {
            if (_strain?.GxEProfile == null)
                return;
            
            // Update current conditions
            _currentConditions = conditions;
            
            // Calculate adaptation effects based on environmental fitness and adaptation rate
            float currentFitness = CalculateEnvironmentalFitness(conditions);
            
            // Apply adaptation over time - plants gradually adapt to their environment
            if (currentFitness < _environmentalFitness)
            {
                // Adapting to worse conditions - slower process
                _environmentalFitness = Mathf.Lerp(_environmentalFitness, currentFitness, adaptationRate * 0.5f);
            }
            else
            {
                // Adapting to better conditions - faster process
                _environmentalFitness = Mathf.Lerp(_environmentalFitness, currentFitness, adaptationRate);
            }
            
            // Clamp to ensure valid range
            _environmentalFitness = Mathf.Clamp01(_environmentalFitness);
            
            // Log significant adaptation changes
            if (Mathf.Abs(_environmentalFitness - currentFitness) > 0.1f)
            {
                Debug.Log($"Plant adapting to environment - Current fitness: {currentFitness:F2}, Adapted fitness: {_environmentalFitness:F2}");
            }
        }
        
        public float GetEnvironmentalFitness() => _environmentalFitness;
        
        private float CalculateEnvironmentalFitness(EnvironmentalConditions conditions)
        {
            if (_strain?.BaseSpecies == null)
                return 1f;
            
            float tempFitness = CalculateTemperatureFitness(conditions.Temperature);
            float humidityFitness = CalculateHumidityFitness(conditions.Humidity);
            float lightFitness = CalculateLightFitness(conditions.LightIntensity);
            float co2Fitness = CalculateCO2Fitness(conditions.CO2Level);
            
            // Weighted average of all environmental factors
            float fitness = (tempFitness * 0.3f) + (humidityFitness * 0.25f) + 
                           (lightFitness * 0.3f) + (co2Fitness * 0.15f);
            
            return Mathf.Clamp01(fitness);
        }
        
        private float CalculateTemperatureFitness(float temperature)
        {
            var optimalConditions = _strain.BaseSpecies.GetOptimalEnvironment();
            var optimal = optimalConditions.Temperature;
            var temperatureRange = _strain.BaseSpecies.TemperatureRange;
            
            // Check if temperature is within optimal range
            if (temperature >= temperatureRange.x && temperature <= temperatureRange.y)
                return 1f;
            
            // Calculate distance from nearest edge of optimal range
            float distance = Mathf.Min(Mathf.Abs(temperature - temperatureRange.x), 
                                     Mathf.Abs(temperature - temperatureRange.y));
            
            // Linear falloff beyond optimal range
            float rangeSize = temperatureRange.y - temperatureRange.x;
            float falloffRange = rangeSize * 0.5f;
            float fitness = 1f - (distance / falloffRange);
            
            return Mathf.Clamp01(fitness);
        }
        
        private float CalculateHumidityFitness(float humidity)
        {
            var optimalConditions = _strain.BaseSpecies.GetOptimalEnvironment();
            var optimal = optimalConditions.Humidity;
            var humidityRange = _strain.BaseSpecies.HumidityRange;
            
            // Check if humidity is within optimal range
            if (humidity >= humidityRange.x && humidity <= humidityRange.y)
                return 1f;
            
            // Calculate distance from nearest edge of optimal range
            float distance = Mathf.Min(Mathf.Abs(humidity - humidityRange.x), 
                                     Mathf.Abs(humidity - humidityRange.y));
            
            // Linear falloff beyond optimal range
            float rangeSize = humidityRange.y - humidityRange.x;
            float falloffRange = rangeSize * 0.5f;
            float fitness = 1f - (distance / falloffRange);
            
            return Mathf.Clamp01(fitness);
        }
        
        private float CalculateLightFitness(float lightIntensity)
        {
            var optimalConditions = _strain.BaseSpecies.GetOptimalEnvironment();
            var optimal = optimalConditions.LightIntensity;
            var lightRange = _strain.BaseSpecies.LightIntensityRange;
            
            // Check if light intensity is within optimal range
            if (lightIntensity >= lightRange.x && lightIntensity <= lightRange.y)
                return 1f;
            
            // Calculate distance from nearest edge of optimal range
            float distance = Mathf.Min(Mathf.Abs(lightIntensity - lightRange.x), 
                                     Mathf.Abs(lightIntensity - lightRange.y));
            
            // Linear falloff beyond optimal range
            float rangeSize = lightRange.y - lightRange.x;
            float falloffRange = rangeSize * 0.5f;
            float fitness = 1f - (distance / falloffRange);
            
            return Mathf.Clamp01(fitness);
        }
        
        private float CalculateCO2Fitness(float co2Level)
        {
            var optimalConditions = _strain.BaseSpecies.GetOptimalEnvironment();
            var optimal = optimalConditions.CO2Level;
            var co2Range = _strain.BaseSpecies.Co2Range;
            
            // Check if CO2 level is within optimal range
            if (co2Level >= co2Range.x && co2Level <= co2Range.y)
                return 1f;
            
            // Calculate distance from nearest edge of optimal range
            float distance = Mathf.Min(Mathf.Abs(co2Level - co2Range.x), 
                                     Mathf.Abs(co2Level - co2Range.y));
            
            // Linear falloff beyond optimal range
            float rangeSize = co2Range.y - co2Range.x;
            float falloffRange = rangeSize * 0.5f;
            float fitness = 1f - (distance / falloffRange);
            
            return Mathf.Clamp01(fitness);
        }
    }
}