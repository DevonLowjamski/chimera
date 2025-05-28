using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ProjectChimera.Data;

namespace ProjectChimera.Examples
{
    /// <summary>
    /// Example code demonstrating how to use the expanded sample data system
    /// </summary>
    public class SampleDataUsageExamples : MonoBehaviour
    {
        [Header("Sample Data References")]
        [SerializeField] private List<GeneticTraitDefinition> availableTraits = new List<GeneticTraitDefinition>();
        [SerializeField] private List<PlantStrainDefinition> availableStrains = new List<PlantStrainDefinition>();
        [SerializeField] private List<EquipmentDefinition> availableEquipment = new List<EquipmentDefinition>();
        [SerializeField] private List<NutrientDefinition> availableNutrients = new List<NutrientDefinition>();
        [SerializeField] private List<FeedingScheduleDefinition> availableSchedules = new List<FeedingScheduleDefinition>();
        
        private void Start()
        {
            // Load all sample data from Resources folders
            LoadSampleData();
            
            // Demonstrate various usage examples
            ExampleGeneticTraitUsage();
            ExampleStrainAnalysis();
            ExampleEquipmentCalculations();
            ExampleNutrientSystem();
            ExampleFeedingSchedule();
            ExampleBreedingSimulation();
        }
        
        /// <summary>
        /// Load all sample data from the project
        /// </summary>
        private void LoadSampleData()
        {
            // In a real implementation, you'd load these from your data management system
            // For demo purposes, we'll assume they're assigned in the inspector
            
            Debug.Log($"Loaded {availableTraits.Count} genetic traits");
            Debug.Log($"Loaded {availableStrains.Count} plant strains");
            Debug.Log($"Loaded {availableEquipment.Count} equipment items");
            Debug.Log($"Loaded {availableNutrients.Count} nutrients");
            Debug.Log($"Loaded {availableSchedules.Count} feeding schedules");
        }
        
        /// <summary>
        /// Example: Working with genetic traits
        /// </summary>
        private void ExampleGeneticTraitUsage()
        {
            Debug.Log("=== Genetic Trait Examples ===");
            
            // Find specific traits
            var thcTrait = availableTraits.FirstOrDefault(t => t.Type == TraitType.THC_Potential);
            var myrceneTrait = availableTraits.FirstOrDefault(t => t.Type == TraitType.Myrcene_Content);
            
            if (thcTrait != null)
            {
                Debug.Log($"THC Trait: {thcTrait.TraitName}");
                Debug.Log($"Range: {thcTrait.MinValue}-{thcTrait.MaxValue}%");
                Debug.Log($"Heritability: {thcTrait.Heritability * 100}%");
                
                // Calculate environmental effect
                float environmentalFactor = 0.8f; // 80% optimal conditions
                float environmentalEffect = thcTrait.CalculateEnvironmentalEffect(environmentalFactor);
                Debug.Log($"Environmental effect at 80% optimal: {environmentalEffect}x");
            }
            
            // Filter traits by category
            var cannabinoidTraits = availableTraits.Where(t => 
                t.Type.ToString().Contains("Potential") && 
                !t.Type.ToString().Contains("Yield")).ToList();
            
            var terpeneTraits = availableTraits.Where(t => 
                t.Type.ToString().Contains("Content")).ToList();
                
            Debug.Log($"Found {cannabinoidTraits.Count} cannabinoid traits");
            Debug.Log($"Found {terpeneTraits.Count} terpene traits");
        }
        
        /// <summary>
        /// Example: Analyzing plant strain characteristics
        /// </summary>
        private void ExampleStrainAnalysis()
        {
            Debug.Log("=== Strain Analysis Examples ===");
            
            foreach (var strain in availableStrains.Take(3)) // Analyze first 3 strains
            {
                Debug.Log($"\nAnalyzing strain: {strain.StrainName}");
                Debug.Log($"Type: {strain.Type}");
                Debug.Log($"Difficulty: {strain.CultivationDifficulty}");
                Debug.Log($"Market Value: ${strain.BaseMarketValue}");
                
                // Get specific trait values
                var thcTrait = availableTraits.FirstOrDefault(t => t.Type == TraitType.THC_Potential);
                if (thcTrait != null)
                {
                    float thcValue = strain.GetTraitValue(thcTrait);
                    Debug.Log($"THC Potential: {thcValue}%");
                }
                
                // Calculate strain potency score
                float potencyScore = CalculateStrainPotency(strain);
                Debug.Log($"Overall Potency Score: {potencyScore:F1}/10");
            }
        }
        
        /// <summary>
        /// Example: Equipment effect calculations
        /// </summary>
        private void ExampleEquipmentCalculations()
        {
            Debug.Log("=== Equipment Examples ===");
            
            // Find lighting equipment
            var lightingEquipment = availableEquipment.Where(e => e.Type == EquipmentType.Lighting).ToList();
            
            foreach (var light in lightingEquipment)
            {
                Debug.Log($"\n{light.EquipmentName} ({light.Tier})");
                Debug.Log($"Cost: ${light.Cost}");
                Debug.Log($"Power: {light.PowerConsumption}W");
                
                // Calculate efficiency (light output per watt)
                var lightIntensityEffect = light.Effects.FirstOrDefault(e => e.effectType == EffectType.Light_Intensity);
                if (lightIntensityEffect != null)
                {
                    float efficiency = lightIntensityEffect.value / light.PowerConsumption;
                    Debug.Log($"Efficiency: {efficiency:F2} intensity per watt");
                }
                
                // Calculate total environmental bonus
                float totalBonus = light.Effects.Where(e => e.isPercentage)
                                               .Sum(e => e.value);
                Debug.Log($"Total Growth Bonus: +{totalBonus}%");
            }
        }
        
        /// <summary>
        /// Example: Nutrient system calculations
        /// </summary>
        private void ExampleNutrientSystem()
        {
            Debug.Log("=== Nutrient System Examples ===");
            
            // Create a nutrient mix for flowering stage
            var floweringNutrients = availableNutrients.Where(n => 
                n.ApplicableStages.HasFlag(GrowthStage.Flowering)).ToList();
            
            Debug.Log($"Found {floweringNutrients.Count} nutrients suitable for flowering");
            
            // Calculate NPK totals for a mix
            float totalN = 0, totalP = 0, totalK = 0;
            float totalCost = 0;
            
            foreach (var nutrient in floweringNutrients.Take(3)) // Use first 3 flowering nutrients
            {
                float dosage = nutrient.RecommendedDosage;
                totalN += nutrient.NitrogenContent * dosage;
                totalP += nutrient.PhosphorusContent * dosage;
                totalK += nutrient.PotassiumContent * dosage;
                totalCost += nutrient.Cost * dosage;
                
                Debug.Log($"{nutrient.NutrientName}: {nutrient.NitrogenContent}-{nutrient.PhosphorusContent}-{nutrient.PotassiumContent} NPK");
            }
            
            Debug.Log($"Total NPK Ratio: {totalN:F1}-{totalP:F1}-{totalK:F1}");
            Debug.Log($"Total Cost per feeding: ${totalCost:F2}");
        }
        
        /// <summary>
        /// Example: Working with feeding schedules
        /// </summary>
        private void ExampleFeedingSchedule()
        {
            Debug.Log("=== Feeding Schedule Examples ===");
            
            var hydroSchedule = availableSchedules.FirstOrDefault(s => s.Type == ScheduleType.Hydroponic);
            
            if (hydroSchedule != null)
            {
                Debug.Log($"Schedule: {hydroSchedule.ScheduleName}");
                Debug.Log($"Total Weeks: {hydroSchedule.TotalWeeks}");
                Debug.Log($"Difficulty: {hydroSchedule.Difficulty}");
                
                // Show first few weeks
                for (int week = 1; week <= Mathf.Min(4, hydroSchedule.TotalWeeks); week++)
                {
                    var weekPlan = hydroSchedule.GetWeeklyPlan(week);
                    if (weekPlan != null)
                    {
                        Debug.Log($"Week {week}: {weekPlan.weekDescription}");
                        Debug.Log($"  Growth Stage: {weekPlan.growthStage}");
                        Debug.Log($"  Feedings per week: {weekPlan.feedingsPerWeek}");
                        Debug.Log($"  Target pH: {weekPlan.recommendedPH}");
                        Debug.Log($"  Target EC: {weekPlan.recommendedEC}");
                    }
                }
            }
        }
        
        /// <summary>
        /// Example: Simulating genetic breeding
        /// </summary>
        private void ExampleBreedingSimulation()
        {
            Debug.Log("=== Breeding Simulation Examples ===");
            
            if (availableStrains.Count >= 2)
            {
                var parent1 = availableStrains[0];
                var parent2 = availableStrains[1];
                
                Debug.Log($"Crossing {parent1.StrainName} x {parent2.StrainName}");
                
                // Simulate offspring trait calculation for THC
                var thcTrait = availableTraits.FirstOrDefault(t => t.Type == TraitType.THC_Potential);
                if (thcTrait != null)
                {
                    float parent1THC = parent1.GetTraitValue(thcTrait);
                    float parent2THC = parent2.GetTraitValue(thcTrait);
                    
                    // Simple additive inheritance simulation
                    float offspringTHC = (parent1THC + parent2THC) / 2f;
                    offspringTHC += Random.Range(-2f, 2f); // Add some genetic variation
                    offspringTHC = Mathf.Clamp(offspringTHC, thcTrait.MinValue, thcTrait.MaxValue);
                    
                    Debug.Log($"Parent 1 THC: {parent1THC}%");
                    Debug.Log($"Parent 2 THC: {parent2THC}%");
                    Debug.Log($"Offspring THC: {offspringTHC:F1}%");
                }
            }
        }
        
        /// <summary>
        /// Calculate overall potency score for a strain
        /// </summary>
        private float CalculateStrainPotency(PlantStrainDefinition strain)
        {
            float score = 0f;
            
            // Get THC contribution (40% of score)
            var thcTrait = availableTraits.FirstOrDefault(t => t.Type == TraitType.THC_Potential);
            if (thcTrait != null)
            {
                float thcValue = strain.GetTraitValue(thcTrait);
                score += (thcValue / thcTrait.MaxValue) * 4f;
            }
            
            // Get trichome density contribution (30% of score)
            var trichomeTrait = availableTraits.FirstOrDefault(t => t.Type == TraitType.Trichome_Density);
            if (trichomeTrait != null)
            {
                float trichomeValue = strain.GetTraitValue(trichomeTrait);
                score += (trichomeValue / trichomeTrait.MaxValue) * 3f;
            }
            
            // Get yield contribution (30% of score)
            var yieldTrait = availableTraits.FirstOrDefault(t => t.Type == TraitType.Yield_Potential);
            if (yieldTrait != null)
            {
                float yieldValue = strain.GetTraitValue(yieldTrait);
                score += (yieldValue / yieldTrait.MaxValue) * 3f;
            }
            
            return score;
        }
        
        /// <summary>
        /// Example: Equipment setup optimization
        /// </summary>
        [ContextMenu("Optimize Equipment Setup")]
        public void OptimizeEquipmentSetup()
        {
            Debug.Log("=== Equipment Optimization ===");
            
            // Budget-based equipment selection
            float budget = 1000f;
            var selectedEquipment = new List<EquipmentDefinition>();
            float remainingBudget = budget;
            
            // Prioritize lighting first
            var bestLight = availableEquipment
                .Where(e => e.Type == EquipmentType.Lighting && e.Cost <= remainingBudget)
                .OrderBy(e => e.Cost / e.Effects.Where(eff => eff.effectType == EffectType.Light_Intensity).Sum(eff => eff.value))
                .FirstOrDefault();
                
            if (bestLight != null)
            {
                selectedEquipment.Add(bestLight);
                remainingBudget -= bestLight.Cost;
                Debug.Log($"Selected light: {bestLight.EquipmentName} (${bestLight.Cost})");
            }
            
            // Add ventilation
            var bestVentilation = availableEquipment
                .Where(e => e.Type == EquipmentType.Ventilation && e.Cost <= remainingBudget)
                .OrderByDescending(e => e.Effects.Sum(eff => eff.value))
                .FirstOrDefault();
                
            if (bestVentilation != null)
            {
                selectedEquipment.Add(bestVentilation);
                remainingBudget -= bestVentilation.Cost;
                Debug.Log($"Selected ventilation: {bestVentilation.EquipmentName} (${bestVentilation.Cost})");
            }
            
            Debug.Log($"Total setup cost: ${budget - remainingBudget:F0} / ${budget}");
            Debug.Log($"Remaining budget: ${remainingBudget:F0}");
        }
    }
} 