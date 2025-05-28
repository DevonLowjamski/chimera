using UnityEngine;

namespace ProjectChimera.Data
{
    /// <summary>
    /// Defines nutrients and their effects on plant growth
    /// </summary>
    [CreateAssetMenu(fileName = "New Nutrient", menuName = "Project Chimera/Cultivation/Nutrient Definition")]
    public class NutrientDefinition : ScriptableObject
    {
        [Header("Basic Information")]
        [SerializeField] private string nutrientName;
        [SerializeField] [TextArea(3, 5)] private string description;
        [SerializeField] private Sprite nutrientIcon;
        
        [Header("Nutrient Properties")]
        [SerializeField] private NutrientType nutrientType;
        [SerializeField] private float nitrogenContent;
        [SerializeField] private float phosphorusContent;
        [SerializeField] private float potassiumContent;
        [SerializeField] private float cost;
        
        [Header("Growth Effects")]
        [SerializeField] private float growthRateModifier = 1f;
        [SerializeField] private float yieldModifier = 1f;
        [SerializeField] private float qualityModifier = 1f;
        [SerializeField] private float healthModifier = 1f;
        
        [Header("Usage")]
        [SerializeField] private GrowthStage applicableStages = GrowthStage.All;
        [SerializeField] private float recommendedDosage = 1f;
        [SerializeField] private float maxDosage = 2f;
        
        // Public Properties
        public string NutrientName => nutrientName;
        public string Description => description;
        public Sprite NutrientIcon => nutrientIcon;
        public NutrientType Type => nutrientType;
        public float NitrogenContent => nitrogenContent;
        public float PhosphorusContent => phosphorusContent;
        public float PotassiumContent => potassiumContent;
        public float Cost => cost;
        public float GrowthRateModifier => growthRateModifier;
        public float YieldModifier => yieldModifier;
        public float QualityModifier => qualityModifier;
        public float HealthModifier => healthModifier;
        public GrowthStage ApplicableStages => applicableStages;
        public float RecommendedDosage => recommendedDosage;
        public float MaxDosage => maxDosage;
    }
    
    [System.Serializable]
    public enum NutrientType
    {
        Base_Nutrient,
        Supplement,
        Additive,
        pH_Adjuster,
        Organic,
        Synthetic
    }
    
    [System.Flags]
    public enum GrowthStage
    {
        Seedling = 1,
        Vegetative = 2,
        Flowering = 4,
        All = Seedling | Vegetative | Flowering
    }
}