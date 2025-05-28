using UnityEngine;
using System.Collections.Generic;

namespace ProjectChimera.Data
{
    /// <summary>
    /// Defines a cannabis strain with its genetic makeup and characteristics
    /// </summary>
    [CreateAssetMenu(fileName = "New Plant Strain", menuName = "Project Chimera/Genetics/Plant Strain")]
    public class PlantStrainDefinition : ScriptableObject
    {
        [Header("Basic Information")]
        [SerializeField] private string strainName;
        [SerializeField] [TextArea(3, 5)] private string description;
        [SerializeField] private Sprite strainIcon;
        
        [Header("Genetics")]
        [SerializeField] private StrainType strainType;
        [SerializeField] private List<GeneticTraitValue> baseTraits = new List<GeneticTraitValue>();
        
        [Header("Cultivation Requirements")]
        [SerializeField] private DifficultyLevel cultivationDifficulty;
        [SerializeField] private float optimalTemperature = 24f;
        [SerializeField] private float optimalHumidity = 60f;
        [SerializeField] private float optimalPH = 6.2f;
        
        [Header("Market Information")]
        [SerializeField] private int baseMarketValue = 100;
        [SerializeField] private float rarityMultiplier = 1f;
        
        // Public Properties
        public string StrainName => strainName;
        public string Description => description;
        public Sprite StrainIcon => strainIcon;
        public StrainType Type => strainType;
        public List<GeneticTraitValue> BaseTraits => baseTraits;
        public DifficultyLevel CultivationDifficulty => cultivationDifficulty;
        public float OptimalTemperature => optimalTemperature;
        public float OptimalHumidity => optimalHumidity;
        public float OptimalPH => optimalPH;
        public int BaseMarketValue => baseMarketValue;
        public float RarityMultiplier => rarityMultiplier;
        
        /// <summary>
        /// Get the value of a specific trait for this strain
        /// </summary>
        public float GetTraitValue(GeneticTraitDefinition trait)
        {
            var traitValue = baseTraits.Find(t => t.trait == trait);
            return traitValue?.value ?? trait.DefaultValue;
        }
    }
    
    [System.Serializable]
    public class GeneticTraitValue
    {
        public GeneticTraitDefinition trait;
        public float value;
    }
    
    [System.Serializable]
    public enum StrainType
    {
        Indica,
        Sativa,
        Hybrid_Indica_Dominant,
        Hybrid_Sativa_Dominant,
        Balanced_Hybrid,
        Ruderalis
    }
    
    [System.Serializable]
    public enum DifficultyLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }
}