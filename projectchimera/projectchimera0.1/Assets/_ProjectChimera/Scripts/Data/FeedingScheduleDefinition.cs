using UnityEngine;
using System.Collections.Generic;

namespace ProjectChimera.Data
{
    /// <summary>
    /// Defines a complete feeding schedule for cannabis cultivation
    /// </summary>
    [CreateAssetMenu(fileName = "New Feeding Schedule", menuName = "Project Chimera/Cultivation/Feeding Schedule")]
    public class FeedingScheduleDefinition : ScriptableObject
    {
        [Header("Basic Information")]
        [SerializeField] private string scheduleName;
        [SerializeField] [TextArea(3, 5)] private string description;
        [SerializeField] private ScheduleType scheduleType;
        [SerializeField] private DifficultyLevel difficulty;
        
        [Header("Schedule Parameters")]
        [SerializeField] private List<WeeklyFeedingPlan> weeklyPlans = new List<WeeklyFeedingPlan>();
        [SerializeField] private int totalWeeks = 16;
        [SerializeField] private float baseWaterAmount = 1f; // Liters per plant
        
        [Header("pH and EC Targets")]
        [SerializeField] private float targetPH = 6.2f;
        [SerializeField] private float targetECMin = 1.2f;
        [SerializeField] private float targetECMax = 2.4f;
        
        [Header("Strain Compatibility")]
        [SerializeField] private List<StrainType> compatibleStrains = new List<StrainType>();
        [SerializeField] private bool universalCompatibility = true;
        
        // Public Properties
        public string ScheduleName => scheduleName;
        public string Description => description;
        public ScheduleType Type => scheduleType;
        public DifficultyLevel Difficulty => difficulty;
        public List<WeeklyFeedingPlan> WeeklyPlans => weeklyPlans;
        public int TotalWeeks => totalWeeks;
        public float BaseWaterAmount => baseWaterAmount;
        public float TargetPH => targetPH;
        public float TargetECMin => targetECMin;
        public float TargetECMax => targetECMax;
        public List<StrainType> CompatibleStrains => compatibleStrains;
        public bool UniversalCompatibility => universalCompatibility;
        
        /// <summary>
        /// Get the feeding plan for a specific week
        /// </summary>
        public WeeklyFeedingPlan GetWeeklyPlan(int week)
        {
            if (week < 1 || week > weeklyPlans.Count) return null;
            return weeklyPlans[week - 1];
        }
        
        /// <summary>
        /// Check if this schedule is compatible with a strain type
        /// </summary>
        public bool IsCompatibleWith(StrainType strainType)
        {
            return universalCompatibility || compatibleStrains.Contains(strainType);
        }
    }
    
    [System.Serializable]
    public class WeeklyFeedingPlan
    {
        [Header("Week Information")]
        public int weekNumber;
        public GrowthStage growthStage;
        public string weekDescription;
        
        [Header("Nutrient Dosages")]
        public List<NutrientDosage> nutrients = new List<NutrientDosage>();
        
        [Header("Feeding Parameters")]
        public int feedingsPerWeek = 2;
        public float waterAmountMultiplier = 1f;
        public bool flushWeek = false;
        
        [Header("Environmental Targets")]
        public float recommendedPH = 6.2f;
        public float recommendedEC = 1.6f;
        public float recommendedTemperature = 24f;
        public float recommendedHumidity = 55f;
        
        /// <summary>
        /// Get the dosage for a specific nutrient
        /// </summary>
        public float GetNutrientDosage(NutrientDefinition nutrient)
        {
            var dosage = nutrients.Find(n => n.nutrient == nutrient);
            return dosage?.dosageMultiplier ?? 0f;
        }
    }
    
    [System.Serializable]
    public class NutrientDosage
    {
        public NutrientDefinition nutrient;
        [Range(0f, 3f)] public float dosageMultiplier = 1f;
        public bool optional = false;
        [TextArea(2, 3)] public string notes;
    }
    
    [System.Serializable]
    public enum ScheduleType
    {
        Hydroponic,
        Soil,
        Coco_Coir,
        Organic,
        Living_Soil,
        Deep_Water_Culture,
        NFT,
        Aeroponics
    }
} 