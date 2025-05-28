using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using ProjectChimera.Data;

namespace ProjectChimera.Editor
{
    /// <summary>
    /// Generates comprehensive sample data for the cannabis cultivation simulation
    /// </summary>
    public class SampleDataGenerator : EditorWindow
    {
        [MenuItem("Project Chimera/Generate Sample Data")]
        public static void ShowWindow()
        {
            GetWindow<SampleDataGenerator>("Sample Data Generator");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Project Chimera Sample Data Generator", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (GUILayout.Button("Generate All Sample Data"))
            {
                GenerateAllSampleData();
            }
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Generate Genetic Traits"))
            {
                GenerateGeneticTraits();
            }
            
            if (GUILayout.Button("Generate Plant Strains"))
            {
                GeneratePlantStrains();
            }
            
            if (GUILayout.Button("Generate Equipment"))
            {
                GenerateEquipment();
            }
            
            if (GUILayout.Button("Generate Nutrients"))
            {
                GenerateNutrients();
            }
            
            if (GUILayout.Button("Generate Feeding Schedules"))
            {
                GenerateFeedingSchedules();
            }
        }

        private static void GenerateAllSampleData()
        {
            GenerateGeneticTraits();
            GeneratePlantStrains();
            GenerateEquipment();
            GenerateNutrients();
            GenerateFeedingSchedules();
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("All sample data generated successfully!");
        }

        private static void GenerateGeneticTraits()
        {
            string basePath = "Assets/_ProjectChimera/Data/Genetics/";
            EnsureDirectoryExists(basePath);

            // Major Cannabinoids
            CreateGeneticTrait("THC Potential", "Primary psychoactive compound", TraitType.THC_Potential, 0f, 35f, 15f, InheritancePattern.Additive, 0.8f, basePath);
            CreateGeneticTrait("CBD Potential", "Primary therapeutic compound", TraitType.CBD_Potential, 0f, 25f, 1f, InheritancePattern.Additive, 0.8f, basePath);
            CreateGeneticTrait("CBG Potential", "Mother of all cannabinoids", TraitType.CBG_Potential, 0f, 8f, 0.5f, InheritancePattern.Additive, 0.7f, basePath);
            CreateGeneticTrait("CBN Potential", "Degradation product of THC", TraitType.CBN_Potential, 0f, 5f, 0.1f, InheritancePattern.Additive, 0.6f, basePath);
            CreateGeneticTrait("CBC Potential", "Anti-inflammatory compound", TraitType.CBC_Potential, 0f, 3f, 0.2f, InheritancePattern.Additive, 0.7f, basePath);
            CreateGeneticTrait("THCV Potential", "Appetite suppressant", TraitType.THCV_Potential, 0f, 8f, 0.1f, InheritancePattern.Recessive, 0.7f, basePath);

            // Major Terpenes
            CreateGeneticTrait("Myrcene Content", "Sedating, earthy terpene", TraitType.Myrcene_Content, 0f, 2f, 0.5f, InheritancePattern.Additive, 0.75f, basePath);
            CreateGeneticTrait("Limonene Content", "Uplifting, citrus terpene", TraitType.Limonene_Content, 0f, 1.5f, 0.3f, InheritancePattern.Additive, 0.75f, basePath);
            CreateGeneticTrait("Pinene Content", "Alert, pine terpene", TraitType.Pinene_Content, 0f, 1f, 0.2f, InheritancePattern.Additive, 0.75f, basePath);
            CreateGeneticTrait("Linalool Content", "Calming, floral terpene", TraitType.Linalool_Content, 0f, 0.8f, 0.1f, InheritancePattern.Additive, 0.75f, basePath);
            CreateGeneticTrait("Caryophyllene Content", "Spicy, anti-inflammatory", TraitType.Caryophyllene_Content, 0f, 1.2f, 0.3f, InheritancePattern.Additive, 0.75f, basePath);

            // Plant Structure
            CreateGeneticTrait("Plant Height", "Maximum height potential", TraitType.Plant_Height, 0.5f, 4f, 1.5f, InheritancePattern.Additive, 0.9f, basePath);
            CreateGeneticTrait("Stretch Factor", "Vertical growth during flowering", TraitType.Stretch_Factor, 1f, 4f, 2f, InheritancePattern.Additive, 0.8f, basePath);
            CreateGeneticTrait("Node Spacing", "Distance between branches", TraitType.Node_Spacing, 1f, 8f, 4f, InheritancePattern.Additive, 0.85f, basePath);
            CreateGeneticTrait("Branch Density", "Number of lateral branches", TraitType.Branch_Density, 0.5f, 2f, 1f, InheritancePattern.Additive, 0.8f, basePath);

            // Flowering Characteristics
            CreateGeneticTrait("Flowering Time", "Days to complete flowering", TraitType.Flowering_Time, 35f, 120f, 63f, InheritancePattern.Additive, 0.9f, basePath);
            CreateGeneticTrait("Yield Potential", "Harvest weight multiplier", TraitType.Yield_Potential, 0.5f, 3f, 1f, InheritancePattern.Additive, 0.7f, basePath);
            CreateGeneticTrait("Trichome Density", "Resin gland coverage", TraitType.Trichome_Density, 0.3f, 1f, 0.6f, InheritancePattern.Additive, 0.8f, basePath);
            CreateGeneticTrait("Bud Structure", "Flower density and formation", TraitType.Bud_Structure, 0.2f, 1f, 0.6f, InheritancePattern.Codominant, 0.75f, basePath);

            // Environmental Adaptation
            CreateGeneticTrait("Heat Tolerance", "Resistance to high temperatures", TraitType.Heat_Tolerance, 0.2f, 1f, 0.5f, InheritancePattern.Additive, 0.8f, basePath);
            CreateGeneticTrait("Mold Resistance", "Resistance to fungal infections", TraitType.Mold_Resistance, 0.1f, 1f, 0.5f, InheritancePattern.Dominant, 0.85f, basePath);
            CreateGeneticTrait("Nutrient Efficiency", "Uptake and utilization", TraitType.Nutrient_Uptake_Efficiency, 0.5f, 2f, 1f, InheritancePattern.Additive, 0.7f, basePath);

            Debug.Log("Genetic traits generated successfully!");
        }

        private static void GeneratePlantStrains()
        {
            string basePath = "Assets/_ProjectChimera/Data/PlantStrains/";
            EnsureDirectoryExists(basePath);

            // Indica Dominant Strains
            CreatePlantStrain("Granddaddy Purple", "Classic indica with grape and berry flavors", StrainType.Indica, 
                new Dictionary<TraitType, float> {
                    { TraitType.THC_Potential, 20f },
                    { TraitType.CBD_Potential, 0.1f },
                    { TraitType.Myrcene_Content, 1.2f },
                    { TraitType.Plant_Height, 1.2f },
                    { TraitType.Flowering_Time, 56f },
                    { TraitType.Yield_Potential, 1.3f }
                }, DifficultyLevel.Beginner, 22f, 50f, 6.0f, 180, basePath);

            CreatePlantStrain("Northern Lights", "Legendary indica for relaxation", StrainType.Indica,
                new Dictionary<TraitType, float> {
                    { TraitType.THC_Potential, 18f },
                    { TraitType.CBD_Potential, 0.2f },
                    { TraitType.Myrcene_Content, 1.5f },
                    { TraitType.Plant_Height, 1f },
                    { TraitType.Flowering_Time, 49f },
                    { TraitType.Mold_Resistance, 0.8f }
                }, DifficultyLevel.Beginner, 20f, 45f, 6.2f, 150, basePath);

            // Sativa Dominant Strains
            CreatePlantStrain("Jack Herer", "Energizing sativa with pine and spice", StrainType.Sativa,
                new Dictionary<TraitType, float> {
                    { TraitType.THC_Potential, 24f },
                    { TraitType.CBD_Potential, 0.1f },
                    { TraitType.Pinene_Content, 0.8f },
                    { TraitType.Plant_Height, 2.5f },
                    { TraitType.Flowering_Time, 70f },
                    { TraitType.Stretch_Factor, 3f }
                }, DifficultyLevel.Intermediate, 26f, 55f, 6.5f, 200, basePath);

            CreatePlantStrain("Green Crack", "Uplifting daytime strain", StrainType.Sativa,
                new Dictionary<TraitType, float> {
                    { TraitType.THC_Potential, 22f },
                    { TraitType.Limonene_Content, 1.1f },
                    { TraitType.Plant_Height, 2f },
                    { TraitType.Flowering_Time, 63f },
                    { TraitType.Yield_Potential, 1.4f }
                }, DifficultyLevel.Intermediate, 25f, 50f, 6.3f, 170, basePath);

            // Hybrid Strains
            CreatePlantStrain("Girl Scout Cookies", "Balanced hybrid with sweet flavors", StrainType.Balanced_Hybrid,
                new Dictionary<TraitType, float> {
                    { TraitType.THC_Potential, 28f },
                    { TraitType.CBD_Potential, 0.2f },
                    { TraitType.Caryophyllene_Content, 0.9f },
                    { TraitType.Plant_Height, 1.3f },
                    { TraitType.Flowering_Time, 63f },
                    { TraitType.Trichome_Density, 0.9f }
                }, DifficultyLevel.Advanced, 24f, 55f, 6.4f, 250, basePath);

            CreatePlantStrain("Wedding Cake", "Potent hybrid with vanilla notes", StrainType.Hybrid_Indica_Dominant,
                new Dictionary<TraitType, float> {
                    { TraitType.THC_Potential, 25f },
                    { TraitType.Limonene_Content, 0.7f },
                    { TraitType.Plant_Height, 1.4f },
                    { TraitType.Flowering_Time, 56f },
                    { TraitType.Yield_Potential, 1.2f }
                }, DifficultyLevel.Advanced, 23f, 50f, 6.1f, 280, basePath);

            // CBD Strains
            CreatePlantStrain("Charlotte's Web", "High CBD therapeutic strain", StrainType.Hybrid_Sativa_Dominant,
                new Dictionary<TraitType, float> {
                    { TraitType.THC_Potential, 0.3f },
                    { TraitType.CBD_Potential, 20f },
                    { TraitType.Pinene_Content, 0.6f },
                    { TraitType.Plant_Height, 1.8f },
                    { TraitType.Flowering_Time, 70f }
                }, DifficultyLevel.Intermediate, 24f, 55f, 6.5f, 300, basePath);

            CreatePlantStrain("ACDC", "Balanced CBD to THC ratio", StrainType.Hybrid_Sativa_Dominant,
                new Dictionary<TraitType, float> {
                    { TraitType.THC_Potential, 6f },
                    { TraitType.CBD_Potential, 14f },
                    { TraitType.Myrcene_Content, 0.4f },
                    { TraitType.Plant_Height, 1.6f },
                    { TraitType.Flowering_Time, 63f }
                }, DifficultyLevel.Intermediate, 22f, 50f, 6.3f, 220, basePath);

            Debug.Log("Plant strains generated successfully!");
        }

        private static void GenerateEquipment()
        {
            string basePath = "Assets/_ProjectChimera/Data/Equipment/";
            EnsureDirectoryExists(basePath);

            // Lighting Equipment
            CreateEquipment("LED Quantum Board 240W", "Full spectrum LED for vegetative and flowering", EquipmentType.Lighting, EquipmentTier.Professional,
                new List<EquipmentEffect> {
                    new EquipmentEffect { effectType = EffectType.Light_Intensity, value = 85f, isPercentage = false },
                    new EquipmentEffect { effectType = EffectType.Growth_Rate, value = 20f, isPercentage = true },
                    new EquipmentEffect { effectType = EffectType.Yield_Bonus, value = 15f, isPercentage = true }
                }, 450, 240f, 1, basePath);

            CreateEquipment("HPS 600W", "Traditional high pressure sodium", EquipmentType.Lighting, EquipmentTier.Standard,
                new List<EquipmentEffect> {
                    new EquipmentEffect { effectType = EffectType.Light_Intensity, value = 70f, isPercentage = false },
                    new EquipmentEffect { effectType = EffectType.Yield_Bonus, value = 25f, isPercentage = true }
                }, 180, 600f, 1, basePath);

            CreateEquipment("T5 Fluorescent 4ft", "Low heat vegetative lighting", EquipmentType.Lighting, EquipmentTier.Basic,
                new List<EquipmentEffect> {
                    new EquipmentEffect { effectType = EffectType.Light_Intensity, value = 30f, isPercentage = false },
                    new EquipmentEffect { effectType = EffectType.Growth_Rate, value = 10f, isPercentage = true }
                }, 80, 54f, 1, basePath);

            // Climate Control
            CreateEquipment("AC Infinity 6\" Inline Fan", "Exhaust ventilation system", EquipmentType.Ventilation, EquipmentTier.Professional,
                new List<EquipmentEffect> {
                    new EquipmentEffect { effectType = EffectType.Air_Circulation, value = 80f, isPercentage = false },
                    new EquipmentEffect { effectType = EffectType.Temperature_Control, value = 15f, isPercentage = true },
                    new EquipmentEffect { effectType = EffectType.Disease_Protection, value = 20f, isPercentage = true }
                }, 160, 45f, 1, basePath);

            CreateEquipment("Digital Thermostat Controller", "Precise temperature management", EquipmentType.Climate_Control, EquipmentTier.Standard,
                new List<EquipmentEffect> {
                    new EquipmentEffect { effectType = EffectType.Temperature_Control, value = 25f, isPercentage = true },
                    new EquipmentEffect { effectType = EffectType.Growth_Rate, value = 8f, isPercentage = true }
                }, 120, 5f, 1, basePath);

            // Irrigation
            CreateEquipment("Automated Drip System", "Precision watering system", EquipmentType.Irrigation, EquipmentTier.Professional,
                new List<EquipmentEffect> {
                    new EquipmentEffect { effectType = EffectType.Water_Efficiency, value = 40f, isPercentage = true },
                    new EquipmentEffect { effectType = EffectType.Nutrient_Efficiency, value = 25f, isPercentage = true },
                    new EquipmentEffect { effectType = EffectType.Growth_Rate, value = 15f, isPercentage = true }
                }, 300, 25f, 2, basePath);

            // Monitoring
            CreateEquipment("pH/EC Meter Pro", "Professional monitoring equipment", EquipmentType.Monitoring, EquipmentTier.Professional,
                new List<EquipmentEffect> {
                    new EquipmentEffect { effectType = EffectType.Nutrient_Efficiency, value = 20f, isPercentage = true },
                    new EquipmentEffect { effectType = EffectType.Quality_Bonus, value = 10f, isPercentage = true }
                }, 180, 0f, 1, basePath);

            Debug.Log("Equipment generated successfully!");
        }

        private static void GenerateNutrients()
        {
            string basePath = "Assets/_ProjectChimera/Data/Nutrients/";
            EnsureDirectoryExists(basePath);

            // Base Nutrients
            CreateNutrient("Fox Farm Grow Big", "Vegetative growth base nutrient", NutrientType.Base_Nutrient,
                6f, 4f, 4f, 25f, 1.3f, 1.1f, 1f, 1.2f, GrowthStage.Vegetative, 2f, 4f, basePath);

            CreateNutrient("Fox Farm Tiger Bloom", "Flowering base nutrient", NutrientType.Base_Nutrient,
                2f, 8f, 4f, 30f, 1.1f, 1.4f, 1.2f, 1.1f, GrowthStage.Flowering, 2f, 4f, basePath);

            CreateNutrient("General Hydroponics Flora Trio", "Complete 3-part system", NutrientType.Base_Nutrient,
                5f, 5f, 5f, 45f, 1.2f, 1.3f, 1.1f, 1.2f, GrowthStage.All, 1.5f, 3f, basePath);

            // Supplements
            CreateNutrient("Cal-Mag Plus", "Calcium and magnesium supplement", NutrientType.Supplement,
                2f, 0f, 1f, 15f, 1.1f, 1f, 1.2f, 1.3f, GrowthStage.All, 1f, 2f, basePath);

            CreateNutrient("Big Bud", "Flowering enhancer", NutrientType.Supplement,
                0f, 6f, 4f, 35f, 1f, 1.5f, 1.3f, 1.1f, GrowthStage.Flowering, 1f, 2f, basePath);

            CreateNutrient("Rhizotonic", "Root zone enhancer", NutrientType.Supplement,
                0.5f, 1f, 1f, 20f, 1.4f, 1.1f, 1f, 1.3f, GrowthStage.Seedling | GrowthStage.Vegetative, 1f, 2f, basePath);

            // Organic Options
            CreateNutrient("Bat Guano", "Organic flowering nutrient", NutrientType.Organic,
                3f, 10f, 1f, 18f, 1.1f, 1.3f, 1.4f, 1.2f, GrowthStage.Flowering, 1f, 2f, basePath);

            CreateNutrient("Kelp Meal", "Organic growth supplement", NutrientType.Organic,
                1f, 0.5f, 8f, 12f, 1.2f, 1.1f, 1.3f, 1.4f, GrowthStage.All, 1f, 2f, basePath);

            // pH Adjusters
            CreateNutrient("pH Down", "Phosphoric acid pH reducer", NutrientType.pH_Adjuster,
                0f, 2f, 0f, 8f, 1f, 1f, 1f, 1f, GrowthStage.All, 0.5f, 1f, basePath);

            CreateNutrient("pH Up", "Potassium hydroxide pH increaser", NutrientType.pH_Adjuster,
                0f, 0f, 3f, 8f, 1f, 1f, 1f, 1f, GrowthStage.All, 0.5f, 1f, basePath);

            Debug.Log("Nutrients generated successfully!");
        }

        private static void GenerateFeedingSchedules()
        {
            string basePath = "Assets/_ProjectChimera/Data/FeedingSchedules/";
            Debug.Log($"Creating FeedingSchedules directory: {basePath}");
            EnsureDirectoryExists(basePath);

            // Basic Hydroponic Schedule
            CreateFeedingSchedule("General Hydroponics Basic", "Simple 3-part schedule for beginners", ScheduleType.Hydroponic, DifficultyLevel.Beginner, 16, basePath);
            
            // Advanced Soil Schedule
            CreateFeedingSchedule("Fox Farm Soil Trio", "Complete soil feeding program", ScheduleType.Soil, DifficultyLevel.Intermediate, 18, basePath);
            
            // Organic Living Soil
            CreateFeedingSchedule("Living Soil Organic", "Water-only organic approach", ScheduleType.Living_Soil, DifficultyLevel.Advanced, 20, basePath);
            
            // Coco Coir Schedule
            CreateFeedingSchedule("Canna Coco Professional", "High-frequency coco feeding", ScheduleType.Coco_Coir, DifficultyLevel.Advanced, 16, basePath);

            Debug.Log("Feeding schedules generated successfully!");
        }

        private static void CreateGeneticTrait(string name, string description, TraitType type, float min, float max, float defaultVal, InheritancePattern inheritance, float heritability, string basePath)
        {
            var trait = CreateInstance<GeneticTraitDefinition>();
            trait.name = SanitizeFileName(name);
            
            var traitType = trait.GetType();
            traitType.GetField("traitName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(trait, name);
            traitType.GetField("description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(trait, description);
            traitType.GetField("traitType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(trait, type);
            traitType.GetField("minimumValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(trait, min);
            traitType.GetField("maximumValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(trait, max);
            traitType.GetField("defaultValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(trait, defaultVal);
            traitType.GetField("inheritancePattern", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(trait, inheritance);
            traitType.GetField("heritability", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(trait, heritability);
            
            AssetDatabase.CreateAsset(trait, basePath + trait.name + ".asset");
        }

        private static void CreatePlantStrain(string name, string description, StrainType type, Dictionary<TraitType, float> traits, DifficultyLevel difficulty, float temp, float humidity, float ph, int value, string basePath)
        {
            var strain = CreateInstance<PlantStrainDefinition>();
            strain.name = SanitizeFileName(name);
            
            var strainType = strain.GetType();
            strainType.GetField("strainName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(strain, name);
            strainType.GetField("description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(strain, description);
            strainType.GetField("strainType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(strain, type);
            strainType.GetField("cultivationDifficulty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(strain, difficulty);
            strainType.GetField("optimalTemperature", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(strain, temp);
            strainType.GetField("optimalHumidity", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(strain, humidity);
            strainType.GetField("optimalPH", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(strain, ph);
            strainType.GetField("baseMarketValue", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(strain, value);
            
            AssetDatabase.CreateAsset(strain, basePath + strain.name + ".asset");
        }

        private static void CreateEquipment(string name, string description, EquipmentType type, EquipmentTier tier, List<EquipmentEffect> effects, int cost, float power, int level, string basePath)
        {
            var equipment = CreateInstance<EquipmentDefinition>();
            equipment.name = SanitizeFileName(name);
            
            var equipmentType = equipment.GetType();
            equipmentType.GetField("equipmentName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(equipment, name);
            equipmentType.GetField("description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(equipment, description);
            equipmentType.GetField("equipmentType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(equipment, type);
            equipmentType.GetField("tier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(equipment, tier);
            equipmentType.GetField("effects", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(equipment, effects);
            equipmentType.GetField("cost", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(equipment, cost);
            equipmentType.GetField("powerConsumption", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(equipment, power);
            equipmentType.GetField("requiredLevel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(equipment, level);
            
            AssetDatabase.CreateAsset(equipment, basePath + equipment.name + ".asset");
        }

        private static void CreateNutrient(string name, string description, NutrientType type, float n, float p, float k, float cost, float growth, float yield, float quality, float health, GrowthStage stages, float recommended, float max, string basePath)
        {
            var nutrient = CreateInstance<NutrientDefinition>();
            nutrient.name = SanitizeFileName(name);
            
            var nutrientType = nutrient.GetType();
            nutrientType.GetField("nutrientName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, name);
            nutrientType.GetField("description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, description);
            nutrientType.GetField("nutrientType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, type);
            nutrientType.GetField("nitrogenContent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, n);
            nutrientType.GetField("phosphorusContent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, p);
            nutrientType.GetField("potassiumContent", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, k);
            nutrientType.GetField("cost", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, cost);
            nutrientType.GetField("growthRateModifier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, growth);
            nutrientType.GetField("yieldModifier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, yield);
            nutrientType.GetField("qualityModifier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, quality);
            nutrientType.GetField("healthModifier", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, health);
            nutrientType.GetField("applicableStages", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, stages);
            nutrientType.GetField("recommendedDosage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, recommended);
            nutrientType.GetField("maxDosage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(nutrient, max);
            
            AssetDatabase.CreateAsset(nutrient, basePath + nutrient.name + ".asset");
        }

        private static void CreateFeedingSchedule(string name, string description, ScheduleType type, DifficultyLevel difficulty, int weeks, string basePath)
        {
            var schedule = CreateInstance<FeedingScheduleDefinition>();
            schedule.name = SanitizeFileName(name);
            
            var scheduleType = schedule.GetType();
            scheduleType.GetField("scheduleName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(schedule, name);
            scheduleType.GetField("description", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(schedule, description);
            scheduleType.GetField("scheduleType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(schedule, type);
            scheduleType.GetField("difficulty", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(schedule, difficulty);
            scheduleType.GetField("totalWeeks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(schedule, weeks);
            
            // Create basic weekly plans
            var weeklyPlans = new List<WeeklyFeedingPlan>();
            for (int i = 1; i <= weeks; i++)
            {
                var plan = new WeeklyFeedingPlan();
                plan.weekNumber = i;
                plan.growthStage = i <= 4 ? GrowthStage.Vegetative : (i <= weeks - 2 ? GrowthStage.Flowering : GrowthStage.Flowering);
                plan.weekDescription = i <= 4 ? $"Vegetative Week {i}" : $"Flowering Week {i - 4}";
                plan.feedingsPerWeek = type == ScheduleType.Hydroponic ? 7 : (type == ScheduleType.Coco_Coir ? 5 : 2);
                plan.recommendedPH = type == ScheduleType.Hydroponic ? 5.8f : 6.2f;
                plan.recommendedEC = i <= 4 ? 1.2f : 1.8f;
                plan.flushWeek = i >= weeks - 1;
                weeklyPlans.Add(plan);
            }
            
            scheduleType.GetField("weeklyPlans", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(schedule, weeklyPlans);
            
            AssetDatabase.CreateAsset(schedule, basePath + schedule.name + ".asset");
        }

        private static void EnsureDirectoryExists(string path)
        {
            // Remove trailing slash if present
            path = path.TrimEnd('/', '\\');
            
            if (!AssetDatabase.IsValidFolder(path))
            {
                // Split the path into components
                string[] pathParts = path.Split('/');
                string currentPath = pathParts[0]; // Start with "Assets"
                
                // Build path incrementally
                for (int i = 1; i < pathParts.Length; i++)
                {
                    string nextPath = currentPath + "/" + pathParts[i];
                    
                    if (!AssetDatabase.IsValidFolder(nextPath))
                    {
                        try
                        {
                            string guid = AssetDatabase.CreateFolder(currentPath, pathParts[i]);
                            if (string.IsNullOrEmpty(guid))
                            {
                                Debug.LogError($"Failed to create folder: {nextPath}");
                                return;
                            }
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogError($"Exception creating folder {nextPath}: {e.Message}");
                            return;
                        }
                    }
                    
                    currentPath = nextPath;
                }
            }
        }
        
        /// <summary>
        /// Sanitize a string to be used as a valid Unity asset file name
        /// </summary>
        private static string SanitizeFileName(string name)
        {
            // Characters that are invalid in Unity asset file names
            char[] invalidChars = { '<', '>', ':', '"', '|', '?', '*', '\\', '/' };
            
            string sanitized = name;
            
            // Replace invalid characters with underscores
            foreach (char c in invalidChars)
            {
                sanitized = sanitized.Replace(c, '_');
            }
            
            // Replace spaces with underscores
            sanitized = sanitized.Replace(' ', '_');
            
            // Replace apostrophes with nothing
            sanitized = sanitized.Replace('\'', '_');
            
            // Remove any double underscores
            while (sanitized.Contains("__"))
            {
                sanitized = sanitized.Replace("__", "_");
            }
            
            // Trim underscores from start and end
            sanitized = sanitized.Trim('_');
            
            // Ensure the name isn't empty
            if (string.IsNullOrEmpty(sanitized))
            {
                sanitized = "Asset";
            }
            
            return sanitized;
        }
    }
} 