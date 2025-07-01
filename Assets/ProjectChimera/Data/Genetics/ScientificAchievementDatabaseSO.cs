using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Core;

namespace ProjectChimera.Data
{
    /// <summary>
    /// Scientific Achievement Database - Collection of achievements for Enhanced Scientific Gaming System v2.0
    /// Contains all achievement definitions for genetics, aromatics, competition, and community systems
    /// </summary>
    [CreateAssetMenu(fileName = "New Scientific Achievement Database", menuName = "Project Chimera/Gaming/Scientific Achievement Database")]
    public class ScientificAchievementDatabaseSO : ChimeraDataSO
    {
        [Header("Achievement Collection")]
        public List<ScientificAchievement> Achievements = new List<ScientificAchievement>();
        
        [Header("Achievement Categories")]
        public List<AchievementCategoryData> Categories = new List<AchievementCategoryData>();
        
        [Header("Legacy Achievements")]
        public List<ScientificAchievement> LegacyAchievements = new List<ScientificAchievement>();
        
        #region Runtime Methods
        
        public ScientificAchievement GetAchievement(string achievementId)
        {
            return Achievements.Find(a => a.AchievementID == achievementId);
        }
        
        public List<ScientificAchievement> GetAchievementsByCategory(AchievementCategory category)
        {
            return Achievements.FindAll(a => a.Category == category);
        }
        
        public List<ScientificAchievement> GetLegacyAchievements()
        {
            return LegacyAchievements;
        }
        
        public bool IsLegacyAchievement(string achievementId)
        {
            return LegacyAchievements.Exists(a => a.AchievementID == achievementId);
        }
        
        #endregion
    }
    
    [System.Serializable]
    public class ScientificAchievement
    {
        public string AchievementID;
        public string DisplayName;
        public AchievementCategory Category;
        public List<AchievementCriterion> Criteria = new List<AchievementCriterion>();
        public float ReputationReward;
        public bool IsCrossSystemAchievement;
        public bool IsLegacyAchievement;
        public string Description;
        public Sprite AchievementIcon;
    }
    
    [System.Serializable]
    public class AchievementCriterion
    {
        public string CriterionName;
        public AchievementCriterionType CriterionType;
        public float RequiredValue;
        public string Description;
    }
    
    [System.Serializable]
    public class AchievementCategoryData
    {
        public string CategoryID;
        public string CategoryName;
        public AchievementCategory Category;
        public Color CategoryColor = Color.white;
        public Sprite CategoryIcon;
        public string Description;
    }
}