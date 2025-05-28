using UnityEngine;
using System.Collections.Generic;

namespace ProjectChimera.Data
{
    /// <summary>
    /// Defines cultivation equipment with stats and effects
    /// </summary>
    [CreateAssetMenu(fileName = "New Equipment", menuName = "Project Chimera/Equipment/Equipment Definition")]
    public class EquipmentDefinition : ScriptableObject
    {
        [Header("Basic Information")]
        [SerializeField] private string equipmentName;
        [SerializeField] [TextArea(3, 5)] private string description;
        [SerializeField] private Sprite equipmentIcon;
        [SerializeField] private GameObject equipmentPrefab;
        
        [Header("Equipment Properties")]
        [SerializeField] private EquipmentType equipmentType;
        [SerializeField] private EquipmentTier tier;
        [SerializeField] private int cost;
        [SerializeField] private float powerConsumption;
        [SerializeField] private float durability = 100f;
        
        [Header("Effects")]
        [SerializeField] private List<EquipmentEffect> effects = new List<EquipmentEffect>();
        
        [Header("Requirements")]
        [SerializeField] private int requiredLevel = 1;
        [SerializeField] private List<EquipmentDefinition> prerequisites = new List<EquipmentDefinition>();
        
        // Public Properties
        public string EquipmentName => equipmentName;
        public string Description => description;
        public Sprite EquipmentIcon => equipmentIcon;
        public GameObject EquipmentPrefab => equipmentPrefab;
        public EquipmentType Type => equipmentType;
        public EquipmentTier Tier => tier;
        public int Cost => cost;
        public float PowerConsumption => powerConsumption;
        public float Durability => durability;
        public List<EquipmentEffect> Effects => effects;
        public int RequiredLevel => requiredLevel;
        public List<EquipmentDefinition> Prerequisites => prerequisites;
    }
    
    [System.Serializable]
    public class EquipmentEffect
    {
        public EffectType effectType;
        public float value;
        public bool isPercentage;
    }
    
    [System.Serializable]
    public enum EquipmentType
    {
        Lighting,
        Ventilation,
        Climate_Control,
        Irrigation,
        Monitoring,
        Security,
        Processing,
        Storage
    }
    
    [System.Serializable]
    public enum EquipmentTier
    {
        Basic,
        Standard,
        Professional,
        Industrial,
        Experimental
    }
    
    [System.Serializable]
    public enum EffectType
    {
        Light_Intensity,
        Light_Spectrum,
        Temperature_Control,
        Humidity_Control,
        Air_Circulation,
        CO2_Enhancement,
        Nutrient_Efficiency,
        Water_Efficiency,
        Growth_Rate,
        Yield_Bonus,
        Quality_Bonus,
        Pest_Protection,
        Disease_Protection
    }
}