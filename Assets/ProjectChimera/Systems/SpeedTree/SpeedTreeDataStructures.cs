using UnityEngine;
using System.Collections.Generic;
using System;

#if UNITY_SPEEDTREE
using SpeedTree;
#endif

namespace ProjectChimera.Systems.SpeedTree
{
    /// <summary>
    /// Comprehensive data structures for SpeedTree integration with Project Chimera.
    /// Includes cannabis-specific genetics, growth patterns, environmental responses,
    /// and performance optimization systems.
    /// </summary>
    
    // Core SpeedTree Enums
    public enum SpeedTreeQualityLevel
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Ultra = 3
    }
    
    public enum CannabisStrainType
    {
        Indica,
        Sativa,
        Hybrid,
        Ruderalis,
        CBD,
        Autoflower
    }
    
    public enum PlantMorphology
    {
        Compact,
        Medium,
        Tall,
        Bushy,
        Lanky,
        Christmas_Tree
    }
    
    public enum TrichromeType
    {
        Bulbous,
        Capitate_Sessile,
        Capitate_Stalked
    }
    
    // Cannabis Strain Asset
    [CreateAssetMenu(fileName = "New Cannabis Strain", menuName = "Project Chimera/SpeedTree/Cannabis Strain")]
    public class CannabisStrainAsset : ScriptableObject
    {
        [Header("Strain Identity")]
        public string StrainId;
        public string StrainName;
        public string Breeder;
        public CannabisStrainType StrainType;
        public PlantMorphology Morphology;
        
        [Header("SpeedTree Integration")]
        public string SpeedTreeAssetPath;
        public Material[] CustomMaterials;
        public Texture2D[] LeafTextures;
        public Texture2D[] BudTextures;
        public Texture2D[] BarkTextures;
        
        [Header("Genetic Characteristics")]
        [Range(0f, 1f)] public float IndicaDominance = 0.5f;
        [Range(0f, 1f)] public float SativaDominance = 0.5f;
        [Range(0f, 1f)] public float RuderalisInfluence = 0f;
        
        [Header("Growth Characteristics")]
        public Vector2 HeightRange = new Vector2(0.5f, 2.5f); // meters
        public Vector2 WidthRange = new Vector2(0.3f, 1.5f);  // meters
        public Vector2 FloweringTimeRange = new Vector2(7f, 12f); // weeks
        public Vector2 YieldRange = new Vector2(50f, 500f); // grams
        
        [Header("Visual Characteristics")]
        public Color LeafColorBase = Color.green;
        public Color LeafColorVariation = Color.yellow;
        public Color BudColorBase = Color.green;
        public Color BudColorVariation = Color.purple;
        public Color PistilColor = Color.white;
        
        [Header("Morphological Traits")]
        [Range(0f, 2f)] public float StemThickness = 1f;
        [Range(0f, 2f)] public float BranchDensity = 1f;
        [Range(0f, 2f)] public float LeafSize = 1f;
        [Range(0f, 2f)] public float LeafDensity = 1f;
        [Range(0f, 2f)] public float BudDensity = 1f;
        [Range(0f, 2f)] public float InternodeSpacing = 1f;
        
        [Header("Trichrome Characteristics")]
        public TrichromeType TrichromeType = TrichromeType.Capitate_Stalked;
        [Range(0f, 1f)] public float TrichromeAmount = 0.5f;
        [Range(0f, 1f)] public float TrichromeSize = 0.5f;
        public Color TrichromeColor = Color.white;
        
        [Header("Environmental Preferences")]
        public Vector2 OptimalTemperatureRange = new Vector2(20f, 28f);
        public Vector2 OptimalHumidityRange = new Vector2(40f, 70f);
        public Vector2 OptimalLightRange = new Vector2(400f, 1000f); // PPFD
        public Vector2 OptimalCO2Range = new Vector2(400f, 1200f); // ppm
        
        [Header("Stress Responses")]
        public AnimationCurve TemperatureStressCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve HumidityStressCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve LightStressCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve WaterStressCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        
        [Header("Wind Response")]
        [Range(0f, 2f)] public float WindSensitivity = 1f;
        [Range(0f, 2f)] public float BranchFlexibility = 1f;
        [Range(0f, 2f)] public float LeafMovement = 1f;
    }
    
    // Plant Instance Data
    [System.Serializable]
    public class SpeedTreePlantInstance
    {
        public int InstanceId;
        public InteractivePlantComponent PlantComponent;
        public CannabisStrainAsset StrainAsset;
        
        // Transform Data
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale = Vector3.one;
        
        // Growth Data
        public PlantGrowthStage GrowthStage;
        public float Age; // in days
        public float Health;
        public float GrowthProgress; // 0-1 for current stage
        public float TotalGrowthProgress; // 0-1 for entire lifecycle
        
        // SpeedTree Components
#if UNITY_SPEEDTREE
        public SpeedTreeRenderer Renderer;
#else
        public Renderer Renderer;
#endif
        public CannabisGeneticData GeneticData;
        
        // Environmental State
        public Dictionary<string, float> EnvironmentalModifiers = new Dictionary<string, float>();
        public EnvironmentalStressData StressData = new EnvironmentalStressData();
        
        // Animation State
        public float WindInfluence = 0f;
        public Vector3 WindDirection = Vector3.zero;
        public float GrowthAnimationTime = 0f;
        
        // Performance Data
        public float LastUpdateTime;
        public bool IsVisible = true;
        public bool IsActive = true;
        public int LODLevel = 0;
        public float DistanceToCamera = 0f;
        public float CreationTime;
        
        // Cannabis-Specific Data
        public float FloweringProgress = 0f;
        public float TrichromeAmount = 0f;
        public float BudMass = 0f;
        public Color CurrentLeafColor = Color.green;
        public Color CurrentBudColor = Color.green;
    }
    
    // Cannabis Genetics System
    [System.Serializable]
    public class CannabisGeneticData
    {
        [Header("Size Genetics")]
        public float PlantSize = 1f;
        public float StemThickness = 1f;
        public float BranchDensity = 1f;
        public float InternodeSpacing = 1f;
        
        [Header("Leaf Genetics")]
        public float LeafSize = 1f;
        public float LeafDensity = 1f;
        public Color LeafColor = Color.green;
        public float LeafSerration = 1f;
        public int LeafletCount = 7;
        
        [Header("Bud Genetics")]
        public float BudDensity = 1f;
        public float BudSize = 1f;
        public Color BudColor = Color.green;
        public float PistilLength = 1f;
        public Color PistilColor = Color.white;
        
        [Header("Trichrome Genetics")]
        public float TrichromeAmount = 0.5f;
        public float TrichromeSize = 1f;
        public Color TrichromeColor = Color.white;
        public TrichromeType TrichromeType = TrichromeType.Capitate_Stalked;
        
        [Header("Visual Genetics")]
        public float ColorVariation = 0.1f;
        public float Saturation = 1f;
        public float Brightness = 1f;
        public float Glossiness = 0.5f;
        
        [Header("Environmental Genetics")]
        public float HeatTolerance = 1f;
        public float ColdTolerance = 1f;
        public float DroughtTolerance = 1f;
        public float MoldResistance = 1f;
        public float PestResistance = 1f;
        
        [Header("Growth Genetics")]
        public float GrowthRate = 1f;
        public float FloweringSpeed = 1f;
        public float YieldPotential = 1f;
        public float PotencyPotential = 1f;
        
        [Header("Wind Response Genetics")]
        public float WindResistance = 1f;
        public float BranchFlexibility = 1f;
        public float StemStiffness = 1f;
    }
    
    // Environmental Stress System
    [System.Serializable]
    public class EnvironmentalStressData
    {
        public float TemperatureStress = 0f;
        public float HumidityStress = 0f;
        public float LightStress = 0f;
        public float WaterStress = 0f;
        public float CO2Stress = 0f;
        public float NutrientStress = 0f;
        public float WindStress = 0f;
        
        public float OverallStress => (TemperatureStress + HumidityStress + LightStress + 
                                      WaterStress + CO2Stress + NutrientStress + WindStress) / 7f;
        
        public Color GetStressColor()
        {
            float stress = OverallStress;
            if (stress < 0.2f) return Color.green;
            if (stress < 0.5f) return Color.yellow;
            if (stress < 0.8f) return Color.orange;
            return Color.red;
        }
    }
    
    // Growth Stage Configuration
    [System.Serializable]
    public class GrowthStageConfiguration
    {
        public PlantGrowthStage Stage;
        public float SizeMultiplier = 1f;
        public float BranchDensityMultiplier = 1f;
        public float LeafDensityMultiplier = 1f;
        public float BudDensityMultiplier = 1f;
        public Color LeafColorMultiplier = Color.white;
        public Color BudColorMultiplier = Color.white;
        public float AnimationSpeed = 1f;
        public List<string> EnabledFeatures = new List<string>();
        public List<string> DisabledFeatures = new List<string>();
    }
    
    // Wind Settings
    [System.Serializable]
    public class SpeedTreeWindSettings
    {
        public float WindMain = 1f;
        public float WindTurbulence = 0.5f;
        public float WindPulseMagnitude = 0.1f;
        public float WindPulseFrequency = 0.01f;
        public Vector3 WindDirection = Vector3.forward;
        public float WindGustiness = 0.5f;
        public AnimationCurve WindCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    }
    
    // Performance Metrics
    [System.Serializable]
    public class SpeedTreePerformanceMetrics
    {
        public int ActivePlants;
        public int VisiblePlants;
        public int MaxVisiblePlants;
        public float CullingDistance;
        public SpeedTreeQualityLevel QualityLevel;
        public bool GPUInstancingEnabled;
        public bool DynamicBatchingEnabled;
        public float FrameRate;
        public float MemoryUsage; // MB
        public int BatchCount;
        public int DrawCalls;
        public DateTime LastUpdate;
        
        // Detailed Performance Data
        public float AverageGrowthUpdateTime;
        public float AverageEnvironmentalUpdateTime;
        public float AverageWindUpdateTime;
        public float AverageLODUpdateTime;
        public int LODTransitions;
        public int CulledPlants;
        public float TotalRenderTime;
    }
    
    // System Report
    [System.Serializable]
    public class SpeedTreeSystemReport
    {
        public SpeedTreePerformanceMetrics PerformanceMetrics;
        public int PlantInstanceCount;
        public int LoadedAssetCount;
        public int RegisteredStrainCount;
        public Dictionary<string, bool> ActiveSystemsStatus;
        public Dictionary<PlantGrowthStage, int> PlantsByStage;
        public Dictionary<CannabisStrainType, int> PlantsByStrain;
        public List<string> PerformanceWarnings;
        public float SystemEfficiency;
    }
    
    // Configuration ScriptableObjects
    [CreateAssetMenu(fileName = "SpeedTree Library", menuName = "Project Chimera/SpeedTree/Library")]
    public class SpeedTreeLibrarySO : ScriptableObject
    {
        [SerializeField] private List<string> _assetPaths = new List<string>();
        [SerializeField] private List<CannabisStrainAsset> _strainAssets = new List<CannabisStrainAsset>();
        
        public List<string> GetAllAssetPaths() => _assetPaths;
        public List<CannabisStrainAsset> GetAllStrains() => _strainAssets;
        
        public CannabisStrainAsset GetStrain(string strainId)
        {
            return _strainAssets.Find(s => s.StrainId == strainId);
        }
    }
    
    [CreateAssetMenu(fileName = "SpeedTree Shader Config", menuName = "Project Chimera/SpeedTree/Shader Config")]
    public class SpeedTreeShaderConfigSO : ScriptableObject
    {
        [Header("Cannabis Shader Properties")]
        public Material BaseCannabisShader;
        public Material LeafShader;
        public Material BudShader;
        public Material StemShader;
        public Material TrichromeShader;
        
        [Header("Seasonal Materials")]
        public Material SpringMaterial;
        public Material SummerMaterial;
        public Material FallMaterial;
        public Material WinterMaterial;
        
        [Header("Stress Visualization")]
        public Material HealthyMaterial;
        public Material StressedMaterial;
        public Material CriticalMaterial;
        public Gradient StressColorGradient;
        
        [Header("Growth Stage Materials")]
        public Material SeedlingMaterial;
        public Material VegetativeMaterial;
        public Material FloweringMaterial;
        public Material HarvestMaterial;
    }
    
    [CreateAssetMenu(fileName = "SpeedTree Wind Config", menuName = "Project Chimera/SpeedTree/Wind Config")]
    public class SpeedTreeWindConfigSO : ScriptableObject
    {
        [Header("Global Wind Settings")]
        public SpeedTreeWindSettings GlobalWind;
        
        [Header("Cannabis-Specific Wind Response")]
        public AnimationCurve LeafWindResponse = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve BranchWindResponse = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve StemWindResponse = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        
        [Header("Wind Zones")]
        public List<WindZoneConfiguration> WindZones = new List<WindZoneConfiguration>();
    }
    
    [System.Serializable]
    public class WindZoneConfiguration
    {
        public string ZoneName;
        public Bounds ZoneBounds;
        public SpeedTreeWindSettings WindSettings;
        public bool AffectsGrowth = false;
        public float GrowthModifier = 1f;
    }
    
    [CreateAssetMenu(fileName = "SpeedTree LOD Config", menuName = "Project Chimera/SpeedTree/LOD Config")]
    public class SpeedTreeLODConfigSO : ScriptableObject
    {
        [Header("LOD Distance Settings")]
        public float LOD0Distance = 25f;
        public float LOD1Distance = 50f;
        public float LOD2Distance = 100f;
        public float BillboardDistance = 200f;
        public float CullDistance = 500f;
        
        [Header("Quality-Based LOD")]
        public LODQualitySettings LowQuality;
        public LODQualitySettings MediumQuality;
        public LODQualitySettings HighQuality;
        public LODQualitySettings UltraQuality;
        
        [Header("Cannabis-Specific LOD")]
        public bool PreserveBudDetails = true;
        public bool PreserveLeafShape = true;
        public float DetailPreservationDistance = 10f;
    }
    
    [System.Serializable]
    public class LODQualitySettings
    {
        public float LOD0Distance;
        public float LOD1Distance;
        public float LOD2Distance;
        public float BillboardDistance;
        public bool EnableSmoothTransitions;
        public bool EnableCrossFading;
        public float TransitionSpeed;
    }
    
    [CreateAssetMenu(fileName = "SpeedTree Genetics Config", menuName = "Project Chimera/SpeedTree/Genetics Config")]
    public class SpeedTreeGeneticsConfigSO : ScriptableObject
    {
        [Header("Genetic Variation")]
        public Vector2 SizeVariationRange = new Vector2(0.8f, 1.2f);
        public Vector2 ColorVariationRange = new Vector2(0.9f, 1.1f);
        public Vector2 MorphologyVariationRange = new Vector2(0.85f, 1.15f);
        
        [Header("Inheritance Rules")]
        public float DominantTraitProbability = 0.7f;
        public float MutationProbability = 0.05f;
        public float HybridVigorBonus = 0.1f;
        
        [Header("Trait Correlations")]
        public List<TraitCorrelation> TraitCorrelations = new List<TraitCorrelation>();
        
        [Header("Environmental Adaptation")]
        public float AdaptationRate = 0.01f;
        public float MaxAdaptation = 0.3f;
        public bool EnableEpigeneticChanges = true;
    }
    
    [System.Serializable]
    public class TraitCorrelation
    {
        public string Trait1;
        public string Trait2;
        public float CorrelationStrength; // -1 to 1
        public string Description;
    }
    
    [CreateAssetMenu(fileName = "SpeedTree Growth Config", menuName = "Project Chimera/SpeedTree/Growth Config")]
    public class SpeedTreeGrowthConfigSO : ScriptableObject
    {
        [Header("Growth Timing")]
        public float SeedlingDuration = 1f; // weeks
        public float VegetativeDuration = 4f; // weeks
        public float FloweringDuration = 8f; // weeks
        public float HarvestWindow = 2f; // weeks
        
        [Header("Growth Curves")]
        public AnimationCurve HeightGrowthCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve WidthGrowthCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve LeafDensityCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public AnimationCurve BudDevelopmentCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        
        [Header("Stage Configurations")]
        public List<GrowthStageConfiguration> StageConfigurations = new List<GrowthStageConfiguration>();
        
        public GrowthStageConfiguration GetStageConfiguration(PlantGrowthStage stage)
        {
            return StageConfigurations.Find(s => s.Stage == stage);
        }
    }
    
    // Subsystem Classes
    public class CannabisGeneticsProcessor
    {
        private SpeedTreeGeneticsConfigSO _config;
        private Dictionary<string, CannabisStrainAsset> _registeredStrains = new Dictionary<string, CannabisStrainAsset>();
        
        public CannabisGeneticsProcessor(SpeedTreeGeneticsConfigSO config)
        {
            _config = config;
        }
        
        public void RegisterStrain(CannabisStrainAsset strain)
        {
            _registeredStrains[strain.StrainId] = strain;
        }
        
        public CannabisGeneticData GenerateGeneticVariation(CannabisStrainAsset baseStrain, object existingGeneticData = null)
        {
            var genetics = new CannabisGeneticData();
            
            // Apply base strain characteristics
            ApplyBaseStrainGenetics(genetics, baseStrain);
            
            // Apply genetic variation
            ApplyGeneticVariation(genetics);
            
            // Apply inheritance if breeding
            if (existingGeneticData != null)
            {
                ApplyInheritance(genetics, existingGeneticData);
            }
            
            return genetics;
        }
        
        private void ApplyBaseStrainGenetics(CannabisGeneticData genetics, CannabisStrainAsset strain)
        {
            genetics.PlantSize = Mathf.Lerp(strain.HeightRange.x, strain.HeightRange.y, 0.5f) / 2f; // Normalize
            genetics.StemThickness = strain.StemThickness;
            genetics.BranchDensity = strain.BranchDensity;
            genetics.LeafSize = strain.LeafSize;
            genetics.LeafDensity = strain.LeafDensity;
            genetics.BudDensity = strain.BudDensity;
            genetics.LeafColor = strain.LeafColorBase;
            genetics.BudColor = strain.BudColorBase;
            genetics.TrichromeAmount = strain.TrichromeAmount;
        }
        
        private void ApplyGeneticVariation(CannabisGeneticData genetics)
        {
            var sizeVariation = UnityEngine.Random.Range(_config.SizeVariationRange.x, _config.SizeVariationRange.y);
            var colorVariation = UnityEngine.Random.Range(_config.ColorVariationRange.x, _config.ColorVariationRange.y);
            var morphVariation = UnityEngine.Random.Range(_config.MorphologyVariationRange.x, _config.MorphologyVariationRange.y);
            
            genetics.PlantSize *= sizeVariation;
            genetics.ColorVariation = colorVariation - 1f;
            genetics.BranchDensity *= morphVariation;
            genetics.LeafDensity *= morphVariation;
        }
        
        private void ApplyInheritance(CannabisGeneticData genetics, object parentGenetics)
        {
            // Implementation for genetic inheritance and breeding
        }
        
        public void Cleanup() { }
    }
    
    public class CannabisGrowthAnimator
    {
        private SpeedTreeGrowthConfigSO _config;
        private Dictionary<int, PlantGrowthAnimationData> _activeAnimations = new Dictionary<int, PlantGrowthAnimationData>();
        
        public CannabisGrowthAnimator(SpeedTreeGrowthConfigSO config)
        {
            _config = config;
        }
        
        public void InitializePlant(SpeedTreePlantInstance instance)
        {
            var animData = new PlantGrowthAnimationData
            {
                InstanceId = instance.InstanceId,
                StartTime = Time.time,
                CurrentStage = instance.GrowthStage,
                StageProgress = 0f
            };
            
            _activeAnimations[instance.InstanceId] = animData;
        }
        
        public void UpdatePlantGrowth(SpeedTreePlantInstance instance, float deltaTime)
        {
            if (_activeAnimations.TryGetValue(instance.InstanceId, out var animData))
            {
                UpdateGrowthAnimation(instance, animData, deltaTime);
            }
        }
        
        private void UpdateGrowthAnimation(SpeedTreePlantInstance instance, PlantGrowthAnimationData animData, float deltaTime)
        {
            // Update growth progress based on environmental conditions
            float growthRate = CalculateGrowthRate(instance);
            animData.StageProgress += (deltaTime / GetStageDuration(instance.GrowthStage)) * growthRate;
            
            // Update visual properties based on growth progress
            UpdateVisualGrowth(instance, animData);
        }
        
        private float CalculateGrowthRate(SpeedTreePlantInstance instance)
        {
            float baseRate = instance.GeneticData.GrowthRate;
            
            // Apply environmental modifiers
            foreach (var modifier in instance.EnvironmentalModifiers)
            {
                baseRate *= modifier.Value;
            }
            
            // Apply stress penalties
            float stressPenalty = 1f - (instance.StressData.OverallStress * 0.5f);
            baseRate *= stressPenalty;
            
            return Mathf.Max(0.1f, baseRate);
        }
        
        private float GetStageDuration(PlantGrowthStage stage)
        {
            return stage switch
            {
                PlantGrowthStage.Seedling => _config.SeedlingDuration * 7f * 24f * 3600f, // Convert to seconds
                PlantGrowthStage.Vegetative => _config.VegetativeDuration * 7f * 24f * 3600f,
                PlantGrowthStage.Flowering => _config.FloweringDuration * 7f * 24f * 3600f,
                _ => 7f * 24f * 3600f // Default 1 week
            };
        }
        
        private void UpdateVisualGrowth(SpeedTreePlantInstance instance, PlantGrowthAnimationData animData)
        {
            float progress = animData.StageProgress;
            
            // Update size based on growth curves
            float heightMultiplier = _config.HeightGrowthCurve.Evaluate(progress);
            float widthMultiplier = _config.WidthGrowthCurve.Evaluate(progress);
            
            instance.Scale = new Vector3(widthMultiplier, heightMultiplier, widthMultiplier) * instance.GeneticData.PlantSize;
            
            if (instance.Renderer != null)
            {
                instance.Renderer.transform.localScale = instance.Scale;
            }
        }
        
        public void AnimateStageTransition(SpeedTreePlantInstance instance, PlantGrowthStage oldStage, PlantGrowthStage newStage)
        {
            if (_activeAnimations.TryGetValue(instance.InstanceId, out var animData))
            {
                animData.CurrentStage = newStage;
                animData.StageProgress = 0f;
                animData.TransitionStartTime = Time.time;
            }
        }
        
        public void RemovePlant(SpeedTreePlantInstance instance)
        {
            _activeAnimations.Remove(instance.InstanceId);
        }
        
        public void Cleanup()
        {
            _activeAnimations.Clear();
        }
    }
    
    [System.Serializable]
    public class PlantGrowthAnimationData
    {
        public int InstanceId;
        public float StartTime;
        public PlantGrowthStage CurrentStage;
        public float StageProgress; // 0-1
        public float TransitionStartTime;
        public Vector3 StartScale;
        public Vector3 TargetScale;
    }
    
    public class CannabisEnvironmentalProcessor
    {
        public void ApplyEnvironmentalConditions(SpeedTreePlantInstance instance, EnvironmentalConditions conditions)
        {
            CalculateStressFactors(instance, conditions);
            ApplyEnvironmentalVisualEffects(instance, conditions);
            UpdateEnvironmentalModifiers(instance, conditions);
        }
        
        public void UpdateEnvironmentalResponse(SpeedTreePlantInstance instance, EnvironmentalConditions conditions)
        {
            ApplyEnvironmentalConditions(instance, conditions);
        }
        
        private void CalculateStressFactors(SpeedTreePlantInstance instance, EnvironmentalConditions conditions)
        {
            var strain = instance.StrainAsset;
            var stress = instance.StressData;
            
            // Temperature stress
            stress.TemperatureStress = CalculateTemperatureStress(conditions.Temperature, strain.OptimalTemperatureRange);
            
            // Humidity stress
            stress.HumidityStress = CalculateHumidityStress(conditions.Humidity, strain.OptimalHumidityRange);
            
            // Light stress
            stress.LightStress = CalculateLightStress(conditions.LightIntensity, strain.OptimalLightRange);
            
            // CO2 stress
            stress.CO2Stress = CalculateCO2Stress(conditions.CO2Level, strain.OptimalCO2Range);
        }
        
        private float CalculateTemperatureStress(float temperature, Vector2 optimalRange)
        {
            if (temperature >= optimalRange.x && temperature <= optimalRange.y)
                return 0f;
            
            float distance = temperature < optimalRange.x ? 
                optimalRange.x - temperature : 
                temperature - optimalRange.y;
            
            return Mathf.Clamp01(distance / 10f); // 10 degree tolerance
        }
        
        private float CalculateHumidityStress(float humidity, Vector2 optimalRange)
        {
            if (humidity >= optimalRange.x && humidity <= optimalRange.y)
                return 0f;
            
            float distance = humidity < optimalRange.x ? 
                optimalRange.x - humidity : 
                humidity - optimalRange.y;
            
            return Mathf.Clamp01(distance / 30f); // 30% tolerance
        }
        
        private float CalculateLightStress(float lightIntensity, Vector2 optimalRange)
        {
            if (lightIntensity >= optimalRange.x && lightIntensity <= optimalRange.y)
                return 0f;
            
            float distance = lightIntensity < optimalRange.x ? 
                optimalRange.x - lightIntensity : 
                lightIntensity - optimalRange.y;
            
            return Mathf.Clamp01(distance / 500f); // 500 PPFD tolerance
        }
        
        private float CalculateCO2Stress(float co2Level, Vector2 optimalRange)
        {
            if (co2Level >= optimalRange.x && co2Level <= optimalRange.y)
                return 0f;
            
            float distance = co2Level < optimalRange.x ? 
                optimalRange.x - co2Level : 
                co2Level - optimalRange.y;
            
            return Mathf.Clamp01(distance / 400f); // 400 ppm tolerance
        }
        
        private void ApplyEnvironmentalVisualEffects(SpeedTreePlantInstance instance, EnvironmentalConditions conditions)
        {
#if UNITY_SPEEDTREE
            if (instance.Renderer?.materialProperties == null) return;
            
            // Adjust leaf color based on stress
            var stressColor = instance.StressData.GetStressColor();
            var baseColor = instance.GeneticData.LeafColor;
            var adjustedColor = Color.Lerp(baseColor, stressColor, instance.StressData.OverallStress * 0.3f);
            
            instance.Renderer.materialProperties.SetColor("_LeafColor", adjustedColor);
            instance.CurrentLeafColor = adjustedColor;
#endif
        }
        
        private void UpdateEnvironmentalModifiers(SpeedTreePlantInstance instance, EnvironmentalConditions conditions)
        {
            // Update growth rate based on conditions
            float tempModifier = CalculateTemperatureGrowthModifier(conditions.Temperature);
            float lightModifier = CalculateLightGrowthModifier(conditions.LightIntensity);
            float humidityModifier = CalculateHumidityGrowthModifier(conditions.Humidity);
            
            instance.EnvironmentalModifiers["temperature"] = tempModifier;
            instance.EnvironmentalModifiers["light"] = lightModifier;
            instance.EnvironmentalModifiers["humidity"] = humidityModifier;
        }
        
        private float CalculateTemperatureGrowthModifier(float temperature)
        {
            // Optimal temperature range for cannabis is typically 20-28°C
            if (temperature >= 20f && temperature <= 28f)
                return 1f;
            
            if (temperature < 20f)
                return Mathf.Lerp(0.3f, 1f, (temperature - 10f) / 10f);
            
            return Mathf.Lerp(1f, 0.3f, (temperature - 28f) / 10f);
        }
        
        private float CalculateLightGrowthModifier(float lightIntensity)
        {
            // Optimal PPFD for cannabis is typically 400-1000
            if (lightIntensity >= 400f && lightIntensity <= 1000f)
                return 1f;
            
            if (lightIntensity < 400f)
                return Mathf.Lerp(0.2f, 1f, lightIntensity / 400f);
            
            return Mathf.Lerp(1f, 0.8f, (lightIntensity - 1000f) / 500f);
        }
        
        private float CalculateHumidityGrowthModifier(float humidity)
        {
            // Optimal humidity for cannabis is typically 40-70%
            if (humidity >= 40f && humidity <= 70f)
                return 1f;
            
            if (humidity < 40f)
                return Mathf.Lerp(0.5f, 1f, (humidity - 20f) / 20f);
            
            return Mathf.Lerp(1f, 0.6f, (humidity - 70f) / 20f);
        }
        
        public void Cleanup() { }
    }
    
    public class CannabisStressVisualizer
    {
        private bool _enabled;
        
        public CannabisStressVisualizer(bool stressVisualization)
        {
            _enabled = stressVisualization;
        }
        
        public void UpdateStressVisualization(SpeedTreePlantInstance instance)
        {
            if (!_enabled) return;
            
            UpdateHealthVisualization(instance, instance.Health);
        }
        
        public void UpdateHealthVisualization(SpeedTreePlantInstance instance, float health)
        {
#if UNITY_SPEEDTREE
            if (instance.Renderer?.materialProperties == null) return;
            
            // Apply health-based color changes
            Color healthColor = GetHealthColor(health);
            instance.Renderer.materialProperties.SetColor("_HealthTint", healthColor);
            
            // Apply stress-based effects
            float stressLevel = instance.StressData.OverallStress;
            instance.Renderer.materialProperties.SetFloat("_StressAmount", stressLevel);
            
            // Visualize specific stress types
            VisualizeSpecificStress(instance);
#endif
        }
        
        private Color GetHealthColor(float health)
        {
            if (health > 80f) return Color.green;
            if (health > 60f) return Color.yellow;
            if (health > 40f) return Color.orange;
            return Color.red;
        }
        
        private void VisualizeSpecificStress(SpeedTreePlantInstance instance)
        {
#if UNITY_SPEEDTREE
            var stress = instance.StressData;
            
            // Temperature stress - affects leaf curl
            if (stress.TemperatureStress > 0.5f)
            {
                instance.Renderer.materialProperties?.SetFloat("_LeafCurl", stress.TemperatureStress);
            }
            
            // Water stress - affects leaf droop
            if (stress.WaterStress > 0.5f)
            {
                instance.Renderer.materialProperties?.SetFloat("_LeafDroop", stress.WaterStress);
            }
            
            // Light stress - affects leaf burn
            if (stress.LightStress > 0.5f)
            {
                instance.Renderer.materialProperties?.SetFloat("_LeafBurn", stress.LightStress);
            }
#endif
        }
        
        public void Cleanup() { }
    }
    
    // Additional subsystem placeholder classes
    public class SpeedTreeLODManager
    {
        private SpeedTreeLODConfigSO _config;
        
        public SpeedTreeLODManager(SpeedTreeLODConfigSO config) { _config = config; }
        public void ConfigureRenderer(object renderer, SpeedTreePlantInstance instance) { }
        public void Update() { }
        public void UpdateLODs(List<SpeedTreePlantInstance> instances) { }
        public void ApplyQualitySettings(object renderer, SpeedTreeQualityLevel quality) { }
        public void Cleanup() { }
    }
    
    public class SpeedTreeBatchingManager
    {
        public int CurrentBatchCount { get; private set; }
        public int EstimatedDrawCalls { get; private set; }
        
        public SpeedTreeBatchingManager(bool dynamicBatching, bool gpuInstancing) { }
        public void RegisterRenderer(object renderer) { }
        public void UnregisterRenderer(object renderer) { }
        public void ProcessBatching() { }
        public void Cleanup() { }
    }
    
    public class SpeedTreeCullingManager
    {
        public int VisiblePlantCount { get; private set; }
        
        public SpeedTreeCullingManager(float cullingDistance) { }
        public void Update(List<SpeedTreePlantInstance> instances) { }
        public void CullDistantPlants(List<SpeedTreePlantInstance> instances) { }
        public void Cleanup() { }
    }
    
    public class SpeedTreeMemoryManager
    {
        public float GetMemoryUsage() { return 0f; }
        public void Update() { }
        public void Cleanup() { }
    }
    
    public class SpeedTreeWindController
    {
        public SpeedTreeWindController(SpeedTreeWindConfigSO config) { }
        public void ConfigureRenderer(object renderer, SpeedTreePlantInstance instance) { }
        public void UpdateWind() { }
        public void UpdateGlobalWind(Vector3 direction, float strength) { }
        public void ApplyWindSettings(SpeedTreeWindSettings settings) { }
        public void SetEnabled(bool enabled) { }
        public void Cleanup() { }
    }
    
    // Component for SpeedTree GameObjects
    public class SpeedTreeCannabisComponent : MonoBehaviour
    {
        private SpeedTreePlantInstance _instance;
        private AdvancedSpeedTreeManager _manager;
        
        public void Initialize(SpeedTreePlantInstance instance, AdvancedSpeedTreeManager manager)
        {
            _instance = instance;
            _manager = manager;
        }
        
        private void Update()
        {
            if (_instance != null)
            {
                _instance.Age += Time.deltaTime / 86400f; // Convert to days
                _instance.LastUpdateTime = Time.time;
                _instance.DistanceToCamera = Vector3.Distance(transform.position, Camera.main?.transform.position ?? Vector3.zero);
            }
        }
    }
    
    public class SpeedTreePlantInteraction : MonoBehaviour
    {
        private SpeedTreePlantInstance _instance;
        
        public void Initialize(SpeedTreePlantInstance instance)
        {
            _instance = instance;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Handle player interaction with cannabis plants
            if (other.CompareTag("Player"))
            {
                // Show plant information UI
                // Allow harvesting if ready
                // Enable plant care actions
            }
        }
    }
    
    // Missing base types that are referenced
    public class SpeedTreeAsset
    {
        // SpeedTree asset placeholder
    }
}