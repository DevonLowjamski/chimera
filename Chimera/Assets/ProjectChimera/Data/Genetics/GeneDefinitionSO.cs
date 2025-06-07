using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Core;

namespace ProjectChimera.Data.Genetics
{
    /// <summary>
    /// Defines a specific gene that controls one or more plant traits.
    /// Contains information about the gene's location, function, and possible allelic variations.
    /// Forms the foundation of the genetic system for trait inheritance and expression.
    /// </summary>
    [CreateAssetMenu(fileName = "New Gene Definition", menuName = "Project Chimera/Genetics/Gene Definition", order = 10)]
    public class GeneDefinitionSO : ChimeraScriptableObject
    {
        [Header("Gene Identity")]
        [SerializeField] private string _geneName;
        [SerializeField] private string _geneSymbol;
        [SerializeField] private string _geneCode; // e.g., "THC1", "CBD2", "HEIGHT1"
        [SerializeField, TextArea(3, 5)] private string _geneDescription;
        [SerializeField] private GeneCategory _category = GeneCategory.Morphological;

        [Header("Genetic Location")]
        [SerializeField, Range(1, 10)] private int _chromosomeNumber = 1;
        [SerializeField] private string _locusPosition; // e.g., "13.2", "22.1"
        [SerializeField] private float _mapPosition = 0f; // Genetic map position in centiMorgans

        [Header("Gene Function")]
        [SerializeField] private GeneFunction _primaryFunction = GeneFunction.StructuralProtein;
        [SerializeField] private List<TraitInfluence> _influencedTraits = new List<TraitInfluence>();
        [SerializeField] private GeneInteractionType _interactionType = GeneInteractionType.Additive;

        [Header("Allelic Information")]
        [SerializeField] private List<AlleleSO> _knownAlleles = new List<AlleleSO>();
        [SerializeField] private AlleleSO _wildTypeAllele;
        [SerializeField] private bool _hasMultipleAlleles = true;
        [SerializeField, Range(2, 10)] private int _maxAlleleCount = 4;

        [Header("Expression Patterns")]
        [SerializeField] private ExpressionTiming _expressionTiming = ExpressionTiming.Continuous;
        [SerializeField] private List<PlantGrowthStage> _activeStages = new List<PlantGrowthStage>();
        [SerializeField] private TissueExpression _tissueExpression = TissueExpression.WholeEnt;

        [Header("Inheritance Patterns")]
        [SerializeField] private InheritancePattern _inheritancePattern = InheritancePattern.Mendelian;
        [SerializeField] private DominanceType _dominanceType = DominanceType.Incomplete;
        [SerializeField, Range(0f, 1f)] private float _penetrance = 1f; // Probability of expression given genotype
        [SerializeField, Range(0f, 1f)] private float _expressivity = 1f; // Degree of expression variation

        [Header("Environmental Sensitivity")]
        [SerializeField] private bool _environmentallyRegulated = false;
        [SerializeField] private List<EnvironmentalFactor> _regulatingFactors = new List<EnvironmentalFactor>();
        [SerializeField, Range(0f, 2f)] private float _environmentalSensitivity = 1f;

        [Header("Epistatic Interactions")]
        [SerializeField] private bool _hasEpistaticEffects = false;
        [SerializeField] private List<GeneDefinitionSO> _epistaticPartners = new List<GeneDefinitionSO>();
        [SerializeField] private EpistaticType _epistaticType = EpistaticType.Complementary;

        [Header("Mutation Properties")]
        [SerializeField, Range(0f, 0.01f)] private float _mutationRate = 0.0001f; // Per generation
        [SerializeField] private bool _hasBeneficialMutations = false;
        [SerializeField] private bool _hasLethalMutations = false;

        [Header("Breeding Applications")]
        [SerializeField] private bool _isBreedingTarget = true;
        [SerializeField] private bool _canBeSelected = true;
        [SerializeField] private DifficultyLevel _selectionDifficulty = DifficultyLevel.Intermediate;
        [SerializeField, Range(0f, 1f)] private float _heritability = 0.7f; // Narrow-sense heritability

        // Public Properties
        public string GeneName => _geneName;
        public string GeneSymbol => _geneSymbol;
        public string GeneCode => _geneCode;
        public string GeneDescription => _geneDescription;
        public GeneCategory Category => _category;

        // Location
        public int ChromosomeNumber => _chromosomeNumber;
        public string LocusPosition => _locusPosition;
        public float MapPosition => _mapPosition;

        // Function
        public GeneFunction PrimaryFunction => _primaryFunction;
        public List<TraitInfluence> InfluencedTraits => _influencedTraits;
        public GeneInteractionType InteractionType => _interactionType;

        // Alleles
        public List<AlleleSO> KnownAlleles => _knownAlleles;
        public AlleleSO WildTypeAllele => _wildTypeAllele;
        public bool HasMultipleAlleles => _hasMultipleAlleles;
        public int MaxAlleleCount => _maxAlleleCount;

        // Expression
        public ExpressionTiming ExpressionTiming => _expressionTiming;
        public List<PlantGrowthStage> ActiveStages => _activeStages;
        public TissueExpression TissueExpression => _tissueExpression;

        // Inheritance
        public InheritancePattern InheritancePattern => _inheritancePattern;
        public DominanceType DominanceType => _dominanceType;
        public float Penetrance => _penetrance;
        public float Expressivity => _expressivity;

        // Environment
        public bool EnvironmentallyRegulated => _environmentallyRegulated;
        public List<EnvironmentalFactor> RegulatingFactors => _regulatingFactors;
        public float EnvironmentalSensitivity => _environmentalSensitivity;

        // Epistasis
        public bool HasEpistaticEffects => _hasEpistaticEffects;
        public List<GeneDefinitionSO> EpistaticPartners => _epistaticPartners;
        public EpistaticType EpistaticType => _epistaticType;

        // Mutation
        public float MutationRate => _mutationRate;
        public bool HasBeneficialMutations => _hasBeneficialMutations;
        public bool HasLethalMutations => _hasLethalMutations;

        // Breeding
        public bool IsBreedingTarget => _isBreedingTarget;
        public bool CanBeSelected => _canBeSelected;
        public DifficultyLevel SelectionDifficulty => _selectionDifficulty;
        public float Heritability => _heritability;

        /// <summary>
        /// Calculates the phenotypic effect of a given genotype for this gene.
        /// </summary>
        /// <param name="allele1">First allele</param>
        /// <param name="allele2">Second allele</param>
        /// <param name="environment">Environmental conditions</param>
        /// <returns>Phenotypic effect value</returns>
        public float CalculatePhenotypicEffect(AlleleSO allele1, AlleleSO allele2, ProjectChimera.Core.EnvironmentalConditions environment = null)
        {
            if (allele1 == null || allele2 == null) return 0f;

            float baseEffect = 0f;

            switch (_dominanceType)
            {
                case DominanceType.Complete:
                    baseEffect = CalculateCompleteDominance(allele1, allele2);
                    break;
                case DominanceType.Incomplete:
                    baseEffect = CalculateIncompleteDominance(allele1, allele2);
                    break;
                case DominanceType.Codominant:
                    baseEffect = CalculateCodominance(allele1, allele2);
                    break;
                case DominanceType.Overdominant:
                    baseEffect = CalculateOverdominance(allele1, allele2);
                    break;
            }

            // Apply penetrance and expressivity
            baseEffect *= _penetrance;
            if (_expressivity < 1f)
            {
                float variation = Random.Range(0f, 1f - _expressivity);
                baseEffect *= (1f - variation);
            }

            // Apply environmental effects if applicable
            if (_environmentallyRegulated && environment != null)
            {
                baseEffect *= CalculateEnvironmentalModifier(environment);
            }

            return baseEffect;
        }

        private float CalculateCompleteDominance(AlleleSO allele1, AlleleSO allele2)
        {
            // Return the effect of the dominant allele
            if (allele1.IsDominant && allele2.IsRecessive) return allele1.EffectStrength;
            if (allele2.IsDominant && allele1.IsRecessive) return allele2.EffectStrength;
            if (allele1.IsDominant && allele2.IsDominant) return Mathf.Max(allele1.EffectStrength, allele2.EffectStrength);
            return Mathf.Max(allele1.EffectStrength, allele2.EffectStrength); // Both recessive
        }

        private float CalculateIncompleteDominance(AlleleSO allele1, AlleleSO allele2)
        {
            // Blend the effects based on interaction type
            switch (_interactionType)
            {
                case GeneInteractionType.Additive:
                    return (allele1.EffectStrength + allele2.EffectStrength) * 0.5f;
                case GeneInteractionType.Multiplicative:
                    return Mathf.Sqrt(allele1.EffectStrength * allele2.EffectStrength);
                default:
                    return (allele1.EffectStrength + allele2.EffectStrength) * 0.5f;
            }
        }

        private float CalculateCodominance(AlleleSO allele1, AlleleSO allele2)
        {
            // Both alleles express equally
            return allele1.EffectStrength + allele2.EffectStrength;
        }

        private float CalculateOverdominance(AlleleSO allele1, AlleleSO allele2)
        {
            // Heterozygote has higher effect than either homozygote
            if (allele1.UniqueID == allele2.UniqueID)
            {
                // Homozygote
                return allele1.EffectStrength;
            }
            else
            {
                // Heterozygote - bonus effect
                return (allele1.EffectStrength + allele2.EffectStrength) * 1.2f;
            }
        }

        private float CalculateEnvironmentalModifier(ProjectChimera.Core.EnvironmentalConditions environment)
        {
            float modifier = 1f;

            foreach (var factor in _regulatingFactors)
            {
                switch (factor)
                {
                    case EnvironmentalFactor.Temperature:
                        modifier *= CalculateTemperatureEffect(environment.Temperature);
                        break;
                    case EnvironmentalFactor.Light:
                        modifier *= CalculateLightEffect(environment.LightIntensity);
                        break;
                    case EnvironmentalFactor.Humidity:
                        modifier *= CalculateHumidityEffect(environment.Humidity);
                        break;
                    case EnvironmentalFactor.CO2:
                        modifier *= CalculateCO2Effect(environment.CO2Level);
                        break;
                }
            }

            return Mathf.Lerp(1f, modifier, _environmentalSensitivity);
        }

        private float CalculateTemperatureEffect(float temperature)
        {
            // Optimal around 24°C, reduced at extremes
            float optimal = 24f;
            float deviation = Mathf.Abs(temperature - optimal);
            return Mathf.Max(0.1f, 1f - (deviation / 20f));
        }

        private float CalculateLightEffect(float lightIntensity)
        {
            // Saturating curve - more light up to a point
            return Mathf.Min(1.5f, lightIntensity / 400f);
        }

        private float CalculateHumidityEffect(float humidity)
        {
            // Optimal around 60%, reduced at extremes
            float optimal = 60f;
            float deviation = Mathf.Abs(humidity - optimal);
            return Mathf.Max(0.3f, 1f - (deviation / 40f));
        }

        private float CalculateCO2Effect(float co2Level)
        {
            // Logarithmic response to CO2
            return Mathf.Min(1.3f, Mathf.Log10(co2Level / 400f) + 1f);
        }

        /// <summary>
        /// Determines if this gene is expressed during a specific growth stage.
        /// </summary>
        public bool IsActiveInStage(PlantGrowthStage stage)
        {
            if (_expressionTiming == ExpressionTiming.Continuous) return true;
            return _activeStages.Contains(stage);
        }

        /// <summary>
        /// Gets a random allele from the known alleles list.
        /// </summary>
        public AlleleSO GetRandomAllele()
        {
            if (_knownAlleles.Count == 0) return null;
            return _knownAlleles[Random.Range(0, _knownAlleles.Count)];
        }

        public override bool ValidateData()
        {
            bool isValid = base.ValidateData();

            if (string.IsNullOrEmpty(_geneName))
            {
                Debug.LogWarning($"[Chimera] GeneDefinitionSO '{DisplayName}' has no gene name assigned.", this);
                isValid = false;
            }

            if (_chromosomeNumber < 1 || _chromosomeNumber > 10)
            {
                Debug.LogWarning($"[Chimera] GeneDefinitionSO '{DisplayName}' has invalid chromosome number.", this);
                isValid = false;
            }

            if (_knownAlleles.Count == 0)
            {
                Debug.LogWarning($"[Chimera] GeneDefinitionSO '{DisplayName}' has no known alleles.", this);
                isValid = false;
            }

            return isValid;
        }
    }

    [System.Serializable]
    public class TraitInfluence
    {
        public PlantTrait TraitType;
        [Range(0f, 2f)] public float InfluenceStrength = 1f;
        public bool IsPositiveEffect = true;
        [TextArea(2, 3)] public string InfluenceDescription;
    }

    public enum GeneCategory
    {
        Morphological,      // Height, structure, leaf shape
        Physiological,      // Metabolism, stress response
        Biochemical,        // Cannabinoids, terpenes
        Development,        // Growth timing, flowering
        Resistance,         // Disease, pest, stress
        Quality,           // Potency, flavor, aroma
        Yield,             // Biomass, flower production
        Regulatory         // Gene expression control
    }

    public enum GeneFunction
    {
        StructuralProtein,
        Enzyme,
        Transcriptionfactor,
        Receptor,
        Transporter,
        RegulatoryRNA,
        MetabolicPathway,
        SignalTransduction,
        StressResponse,
        Development
    }

    public enum GeneInteractionType
    {
        Additive,          // Effects sum linearly
        Multiplicative,    // Effects multiply
        Epistatic,         // One masks the other
        Complementary,     // Both needed for effect
        Suppressor,        // One suppresses the other
        Enhancer          // One enhances the other
    }

    public enum InheritancePattern
    {
        Mendelian,         // Simple dominant/recessive
        Polygenic,         // Multiple genes, additive
        Quantitative,      // Continuous variation
        Maternal,          // Inherited from mother only
        Epigenetic,        // Expression patterns inherited
        Linkage,           // Genes on same chromosome
        SexLinked         // On sex chromosomes
    }

    public enum DominanceType
    {
        Complete,          // One allele masks the other
        Incomplete,        // Blended expression
        Codominant,        // Both alleles express
        Overdominant,      // Heterozygote advantage
        Underdominant      // Heterozygote disadvantage
    }

    public enum ExpressionTiming
    {
        Continuous,        // Always active
        StageSpecific,     // Only in certain stages
        Environmental,     // Triggered by environment
        Developmental,     // Follows development program
        Circadian,         // Daily rhythm
        Seasonal          // Seasonal pattern
    }

    public enum TissueExpression
    {
        WholeEnt,         // Entire plant
        Leaves,            // Leaf tissue only
        Stems,             // Stem tissue only
        Roots,             // Root tissue only
        Flowers,           // Reproductive tissue
        Trichomes,         // Glandular structures
        Vascular,          // Transport tissue
        Meristem          // Growing points
    }

    public enum EnvironmentalFactor
    {
        Temperature,
        Light,
        Humidity,
        CO2,
        Nutrients,
        Water,
        pH,
        Salinity,
        Oxygen,
        Pressure
    }

    public enum EpistaticType
    {
        Complementary,     // Both genes needed
        Supplementary,     // One modifies the other
        Inhibitory,        // One prevents expression
        Duplicate,         // Either gene can provide function
        Recessive,         // Recessive epistasis
        Dominant          // Dominant epistasis
    }

    public enum PlantTrait
    {
        // Morphological
        Height,
        Width,
        LeafSize,
        LeafShape,
        StemThickness,
        BranchingPattern,
        Internode,
        
        // Physiological
        GrowthRate,
        MetabolicRate,
        PhotosyntheticEfficiency,
        WaterUseEfficiency,
        NutrientUptake,
        
        // Biochemical
        THCContent,
        CBDContent,
        TerpeneProfile,
        ChlorophyllContent,
        AntioxidantLevels,
        
        // Development
        FloweringTime,
        SeedGermination,
        MaturationRate,
        Senescence,
        
        // Resistance
        DiseaseResistance,
        PestResistance,
        DroughtTolerance,
        HeatTolerance,
        ColdTolerance,
        
        // Quality
        Potency,
        Flavor,
        Aroma,
        Density,
        Resin,
        
        // Yield
        TotalBiomass,
        FlowerYield,
        SeedProduction,
        BranchingDensity
    }
}