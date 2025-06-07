using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Data.Cultivation;

namespace ProjectChimera.Data.Genetics
{
    /// <summary>
    /// Defines a specific cannabis strain with its unique genetic profile, characteristics, and breeding history.
    /// Inherits base parameters from PlantSpeciesSO and adds strain-specific modifications and traits.
    /// </summary>
    [CreateAssetMenu(fileName = "New Plant Strain", menuName = "Project Chimera/Genetics/Plant Strain", order = 2)]
    public class PlantStrainSO : ChimeraScriptableObject
    {
        [Header("Strain Identity")]
        [SerializeField] private PlantSpeciesSO _baseSpecies;
        [SerializeField] private string _strainName;
        [SerializeField] private string _breederName;
        [SerializeField] private string _originRegion;
        [SerializeField, TextArea(3, 6)] private string _strainDescription;
        [SerializeField] private StrainType _strainType = StrainType.Hybrid;

        [Header("Breeding Lineage")]
        [SerializeField] private PlantStrainSO _parentStrain1;
        [SerializeField] private PlantStrainSO _parentStrain2;
        [SerializeField] private int _generationNumber = 1; // F1, F2, etc.
        [SerializeField] private bool _isLandrace = false;
        [SerializeField] private bool _isStabilized = false;
        [SerializeField, Range(0f, 1f)] private float _breedingStability = 0.5f;

        [Header("Genetic Modifiers (Applied to Base Species)")]
        [SerializeField, Range(0.5f, 2f)] private float _heightModifier = 1f;
        [SerializeField, Range(0.5f, 2f)] private float _widthModifier = 1f;
        [SerializeField, Range(0.5f, 2f)] private float _yieldModifier = 1f;
        [SerializeField, Range(0.8f, 1.2f)] private float _growthRateModifier = 1f;

        [Header("Flowering Characteristics")]
        [SerializeField] private PhotoperiodSensitivity _photoperiodSensitivity = PhotoperiodSensitivity.Photoperiod;
        [SerializeField, Range(0.8f, 1.2f)] private float _floweringTimeModifier = 1f;
        [SerializeField] private bool _autoflowering = false;
        [SerializeField] private int _autofloweringTriggerDays = 0;

        [Header("Cannabinoid Profile")]
        [SerializeField] private CannabinoidProfile _cannabinoidProfile;

        [Header("Terpene Profile")]
        [SerializeField] private TerpeneProfile _terpeneProfile;

        [Header("Morphological Traits")]
        [SerializeField] private LeafStructure _leafStructure = LeafStructure.Broad;
        [SerializeField] private BudStructure _budStructure = BudStructure.Dense;
        [SerializeField] private Color _leafColor = Color.green;
        [SerializeField] private Color _budColor = Color.green;
        [SerializeField, Range(0.5f, 2f)] private float _resinProductionModifier = 1f;

        [Header("Strain-Specific Resistances")]
        [SerializeField, Range(-0.3f, 0.3f)] private float _heatToleranceModifier = 0f;
        [SerializeField, Range(-0.3f, 0.3f)] private float _coldToleranceModifier = 0f;
        [SerializeField, Range(-0.3f, 0.3f)] private float _droughtToleranceModifier = 0f;
        [SerializeField, Range(-0.3f, 0.3f)] private float _diseaseResistanceModifier = 0f;

        [Header("Cultivation Difficulty")]
        [SerializeField] private DifficultyLevel _cultivationDifficulty = DifficultyLevel.Intermediate;
        [SerializeField, Range(0f, 1f)] private float _beginerFriendliness = 0.5f;
        [SerializeField, Range(0f, 1f)] private float _environmentalSensitivity = 0.5f;

        [Header("Effects and Medical Properties")]
        [SerializeField] private EffectsProfile _effectsProfile;
        [SerializeField] private List<MedicalApplication> _medicalApplications = new List<MedicalApplication>();

        [Header("Commercial Properties")]
        [SerializeField, Range(0f, 100f)] private float _marketValue = 10f; // per gram
        [SerializeField, Range(0f, 1f)] private float _marketDemand = 0.5f;
        [SerializeField] private bool _seedsAvailable = true;
        [SerializeField] private bool _clonesAvailable = false;

        // Public Properties
        public PlantSpeciesSO BaseSpecies => _baseSpecies;
        public string StrainName => _strainName;
        public string BreederName => _breederName;
        public string OriginRegion => _originRegion;
        public string StrainDescription => _strainDescription;
        public StrainType StrainType => _strainType;

        // Breeding Properties
        public PlantStrainSO ParentStrain1 => _parentStrain1;
        public PlantStrainSO ParentStrain2 => _parentStrain2;
        public int GenerationNumber => _generationNumber;
        public bool IsLandrace => _isLandrace;
        public bool IsStabilized => _isStabilized;
        public float BreedingStability => _breedingStability;

        // Genetic Modifiers
        public float HeightModifier => _heightModifier;
        public float WidthModifier => _widthModifier;
        public float YieldModifier => _yieldModifier;
        public float GrowthRateModifier => _growthRateModifier;

        // Flowering
        public PhotoperiodSensitivity PhotoperiodSensitivity => _photoperiodSensitivity;
        public float FloweringTimeModifier => _floweringTimeModifier;
        public bool Autoflowering => _autoflowering;
        public int AutofloweringTriggerDays => _autofloweringTriggerDays;

        // Profiles
        public CannabinoidProfile CannabinoidProfile => _cannabinoidProfile;
        public TerpeneProfile TerpeneProfile => _terpeneProfile;
        public EffectsProfile EffectsProfile => _effectsProfile;

        // Morphology
        public LeafStructure LeafStructure => _leafStructure;
        public BudStructure BudStructure => _budStructure;
        public Color LeafColor => _leafColor;
        public Color BudColor => _budColor;
        public float ResinProductionModifier => _resinProductionModifier;

        // Resistances
        public float HeatToleranceModifier => _heatToleranceModifier;
        public float ColdToleranceModifier => _coldToleranceModifier;
        public float DroughtToleranceModifier => _droughtToleranceModifier;
        public float DiseaseResistanceModifier => _diseaseResistanceModifier;

        // Cultivation
        public DifficultyLevel CultivationDifficulty => _cultivationDifficulty;
        public float BeginnerFriendliness => _beginerFriendliness;
        public float EnvironmentalSensitivity => _environmentalSensitivity;

        // Commercial
        public float MarketValue => _marketValue;
        public float MarketDemand => _marketDemand;
        public bool SeedsAvailable => _seedsAvailable;
        public bool ClonesAvailable => _clonesAvailable;

        /// <summary>
        /// Calculates the modified height range for this strain.
        /// </summary>
        public Vector2 GetModifiedHeightRange()
        {
            if (_baseSpecies == null) return Vector2.zero;
            
            Vector2 baseRange = _baseSpecies.HeightRange;
            return new Vector2(
                baseRange.x * _heightModifier,
                baseRange.y * _heightModifier
            );
        }

        /// <summary>
        /// Calculates the modified yield range for this strain.
        /// </summary>
        public Vector2 GetModifiedYieldRange()
        {
            if (_baseSpecies == null) return Vector2.zero;
            
            Vector2 baseRange = _baseSpecies.YieldPerPlantRange;
            return new Vector2(
                baseRange.x * _yieldModifier,
                baseRange.y * _yieldModifier
            );
        }

        /// <summary>
        /// Calculates the modified flowering time for this strain.
        /// </summary>
        public Vector2 GetModifiedFloweringTime()
        {
            if (_baseSpecies == null) return Vector2.zero;
            
            Vector2 baseRange = _baseSpecies.FloweringDays;
            return new Vector2(
                baseRange.x * _floweringTimeModifier,
                baseRange.y * _floweringTimeModifier
            );
        }

        /// <summary>
        /// Gets the modified environmental suitability for this strain.
        /// </summary>
        public float GetModifiedEnvironmentalSuitability(ProjectChimera.Core.EnvironmentalConditions conditions)
        {
            if (_baseSpecies == null) return 0f;
            
            float baseSuitability = _baseSpecies.EvaluateEnvironmentalSuitability(conditions);
            
            // Apply strain-specific tolerances
            float heatAdjustment = CalculateToleranceAdjustment(conditions.Temperature, _baseSpecies.TemperatureRange, _heatToleranceModifier);
            float coldAdjustment = CalculateToleranceAdjustment(conditions.Temperature, _baseSpecies.TemperatureRange, _coldToleranceModifier);
            
            return Mathf.Clamp01(baseSuitability + heatAdjustment + coldAdjustment);
        }

        private float CalculateToleranceAdjustment(float value, Vector2 range, float toleranceModifier)
        {
            if (toleranceModifier == 0f) return 0f;
            
            if (value < range.x || value > range.y)
            {
                // Outside optimal range - tolerance modifier can help
                return toleranceModifier * 0.2f; // Max 6% improvement
            }
            
            return 0f;
        }

        public override bool ValidateData()
        {
            bool isValid = base.ValidateData();

            if (_baseSpecies == null)
            {
                Debug.LogWarning($"[Chimera] PlantStrainSO '{DisplayName}' has no base species assigned.", this);
                isValid = false;
            }

            if (string.IsNullOrEmpty(_strainName))
            {
                Debug.LogWarning($"[Chimera] PlantStrainSO '{DisplayName}' has no strain name assigned.", this);
                isValid = false;
            }

            if (_autoflowering && _autofloweringTriggerDays <= 0)
            {
                Debug.LogWarning($"[Chimera] Autoflowering strain '{DisplayName}' has invalid trigger days.", this);
                isValid = false;
            }

            return isValid;
        }
    }

    [System.Serializable]
    public class CannabinoidProfile
    {
        [Range(0f, 35f)] public float ThcPercentage = 15f;
        [Range(0f, 25f)] public float CbdPercentage = 1f;
        [Range(0f, 5f)] public float CbgPercentage = 0.5f;
        [Range(0f, 3f)] public float CbnPercentage = 0.1f;
        [Range(0f, 2f)] public float CbcPercentage = 0.1f;
        [Range(0f, 1f)] public float ThcvPercentage = 0.1f;
    }

    [System.Serializable]
    public class TerpeneProfile
    {
        [Range(0f, 3f)] public float Myrcene = 0.5f;
        [Range(0f, 2f)] public float Limonene = 0.3f;
        [Range(0f, 2f)] public float Pinene = 0.2f;
        [Range(0f, 1.5f)] public float Linalool = 0.1f;
        [Range(0f, 1.5f)] public float Caryophyllene = 0.2f;
        [Range(0f, 1f)] public float Humulene = 0.1f;
        [Range(0f, 1f)] public float Terpinolene = 0.1f;
    }

    [System.Serializable]
    public class EffectsProfile
    {
        [Range(0f, 1f)] public float Euphoria = 0.5f;
        [Range(0f, 1f)] public float Relaxation = 0.5f;
        [Range(0f, 1f)] public float Creativity = 0.3f;
        [Range(0f, 1f)] public float Focus = 0.3f;
        [Range(0f, 1f)] public float Energy = 0.4f;
        [Range(0f, 1f)] public float Sedation = 0.3f;
        [Range(0f, 1f)] public float AppetiteStimulation = 0.4f;
        [Range(0f, 1f)] public float PainRelief = 0.3f;
    }

    public enum StrainType
    {
        Indica,
        Sativa,
        Hybrid,
        IndicaDominant,
        SativaDominant,
        Ruderalis
    }

    public enum PhotoperiodSensitivity
    {
        Photoperiod,
        Autoflower,
        SemiAutoflower
    }

    public enum LeafStructure
    {
        Narrow,
        Medium,
        Broad,
        Serrated,
        Smooth
    }

    public enum BudStructure
    {
        Loose,
        Medium,
        Dense,
        Airy,
        Compact
    }

    public enum DifficultyLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert,
        MasterLevel
    }

    public enum MedicalApplication
    {
        PainRelief,
        AnxietyReduction,
        DepressionSupport,
        InsomniaTreatment,
        AppetiteStimulation,
        NauseaReduction,
        InflammationReduction,
        MuscleRelaxation,
        SeizureControl,
        GlaucomaTreatment
    }
}