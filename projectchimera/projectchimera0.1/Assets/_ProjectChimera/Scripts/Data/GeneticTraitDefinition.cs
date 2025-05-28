using UnityEngine;

namespace ProjectChimera.Data
{
    /// <summary>
    /// Defines a specific genetic trait that can be inherited and expressed
    /// </summary>
    [CreateAssetMenu(fileName = "New Genetic Trait", menuName = "Project Chimera/Genetics/Genetic Trait")]
    public class GeneticTraitDefinition : ScriptableObject
    {
        [Header("Basic Information")]
        [SerializeField] private string traitName;
        [SerializeField] [TextArea(3, 5)] private string description;
        
        [Header("Trait Properties")]
        [SerializeField] private TraitType traitType;
        [SerializeField] private float minimumValue;
        [SerializeField] private float maximumValue;
        [SerializeField] private float defaultValue;
        
        [Header("Inheritance")]
        [SerializeField] private InheritancePattern inheritancePattern;
        [SerializeField] [Range(0f, 1f)] private float heritability = 0.7f;
        
        [Header("Environmental Interaction")]
        [SerializeField] private bool affectedByEnvironment = true;
        [SerializeField] private AnimationCurve environmentalResponseCurve = AnimationCurve.Linear(0, 0, 1, 1);
        
        // Public Properties
        public string TraitName => traitName;
        public string Description => description;
        public TraitType Type => traitType;
        public float MinValue => minimumValue;
        public float MaxValue => maximumValue;
        public float DefaultValue => defaultValue;
        public InheritancePattern Inheritance => inheritancePattern;
        public float Heritability => heritability;
        public bool AffectedByEnvironment => affectedByEnvironment;
        public AnimationCurve EnvironmentalResponse => environmentalResponseCurve;
        
        /// <summary>
        /// Calculate the environmental effect on this trait
        /// </summary>
        public float CalculateEnvironmentalEffect(float environmentalFactor)
        {
            if (!affectedByEnvironment) return 1f;
            
            return environmentalResponseCurve.Evaluate(environmentalFactor);
        }
    }
    
    [System.Serializable]
    public enum TraitType
    {
        // Major Cannabinoids
        THC_Potential,
        CBD_Potential,
        CBG_Potential,
        CBN_Potential,
        CBC_Potential,
        THCV_Potential,
        CBDV_Potential,
        Delta8_THC_Potential,
        THCA_Potential,
        CBDA_Potential,
        
        // Major Terpenes
        Myrcene_Content,
        Limonene_Content,
        Pinene_Content,
        Linalool_Content,
        Caryophyllene_Content,
        Humulene_Content,
        Terpinolene_Content,
        Ocimene_Content,
        Bisabolol_Content,
        Camphene_Content,
        
        // Plant Structure & Growth
        Plant_Height,
        Plant_Width,
        Stretch_Factor,
        Node_Spacing,
        Branch_Density,
        Leaf_Size,
        Leaf_Color,
        Stem_Thickness,
        Root_Development,
        
        // Flowering Characteristics
        Flowering_Time,
        Flower_Density,
        Bud_Structure,
        Calyx_Size,
        Pistil_Color,
        Trichome_Density,
        Trichome_Size,
        Resin_Production,
        
        // Yield & Production
        Yield_Potential,
        Flower_to_Leaf_Ratio,
        Seed_Production,
        Clone_Success_Rate,
        
        // Environmental Adaptation
        Heat_Tolerance,
        Cold_Tolerance,
        Humidity_Tolerance,
        Light_Stress_Resistance,
        Drought_Tolerance,
        Salt_Tolerance,
        
        // Disease & Pest Resistance
        Mold_Resistance,
        Mildew_Resistance,
        Bud_Rot_Resistance,
        Spider_Mite_Resistance,
        Aphid_Resistance,
        Thrip_Resistance,
        Root_Rot_Resistance,
        
        // Nutrient Characteristics
        Nutrient_Uptake_Efficiency,
        Nitrogen_Sensitivity,
        Phosphorus_Uptake,
        Potassium_Requirement,
        CalMag_Tolerance,
        pH_Sensitivity,
        EC_Tolerance,
        
        // Quality Factors
        Aroma_Intensity,
        Flavor_Complexity,
        Smoke_Smoothness,
        Bag_Appeal,
        Shelf_Life,
        Cure_Response,
        
        // Autoflowering Traits
        Autoflower_Gene,
        Day_Neutral_Response,
        Photoperiod_Sensitivity
    }
    
    [System.Serializable]
    public enum InheritancePattern
    {
        Additive,           // Simple additive genetics
        Dominant,           // One allele dominates
        Recessive,          // Requires two copies
        Codominant,         // Both alleles expressed
        Epistatic           // One gene affects another
    }
}