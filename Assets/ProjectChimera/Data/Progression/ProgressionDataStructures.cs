using UnityEngine;
using ProjectChimera.Data.Equipment;
using ProjectChimera.Data.Genetics;
using System.Collections.Generic;
using System.Linq;

namespace ProjectChimera.Data.Progression
{
    /// <summary>
    /// Additional data structures for the progression system that extend the existing framework.
    /// Includes achievement definitions, experience tracking, and specialized progression mechanics.
    /// </summary>
    
    [System.Serializable]
    public class AchievementDefinition
    {
        public string AchievementId;
        public string AchievementName;
        public AchievementCategory Category;
        public AchievementType Type;
        [TextArea(2, 4)] public string Description;
        public Sprite Icon;
        
        [Header("Requirements")]
        public AchievementRequirements Requirements;
        public bool IsHidden = false;
        public bool IsRepeatable = false;
        
        [Header("Rewards")]
        public List<AchievementReward> Rewards = new List<AchievementReward>();
        public float ExperienceReward = 100f;
        public int SkillPointReward = 1;
        
        [Header("Progress Tracking")]
        public List<ProgressCondition> ProgressConditions = new List<ProgressCondition>();
        public bool TrackProgress = true;
    }
    
    [System.Serializable]
    public class AchievementRequirements
    {
        public int MinimumPlayerLevel = 1;
        public List<SkillNodeSO> RequiredSkills = new List<SkillNodeSO>();
        public List<string> RequiredAchievements = new List<string>();
        public List<ResearchProjectSO> RequiredResearch = new List<ResearchProjectSO>();
        public bool RequiresSpecificFacility = false;
        public string RequiredFacilityType;
        
        // Compatibility property for ComprehensiveProgressionManager
        public List<AchievementRequirement> Requirements = new List<AchievementRequirement>();
    }
    
    [System.Serializable]
    public class AchievementReward
    {
        public AchievementRewardType RewardType;
        public string RewardDescription;
        public float RewardValue;
        public SkillNodeSO UnlockedSkill;
        public ResearchProjectSO UnlockedResearch;
        public EquipmentDataSO UnlockedEquipment;
        public string UnlockedFeature;
    }
    
    [System.Serializable]
    public class ProgressCondition
    {
        public ProgressType ProgressType;
        public string TargetId;
        public float RequiredValue;
        public ComparisonType Comparison;
        public string Description;
    }
    
    /// <summary>
    /// Player progression data tracking all advancement across the game
    /// </summary>
    [System.Serializable]
    public class PlayerProgressionData
    {
        [Header("Player Identity")]
        public string PlayerId;
        public int PlayerLevel = 1;
        public float TotalExperience = 0f;
        public int AvailableSkillPoints = 0;
        public float PlayTimeHours = 0f;
        public System.DateTime CreationDate;
        public System.DateTime LastPlayDate;
        
        [Header("Completed Content")]
        public List<string> CompletedAchievements = new List<string>();
        public List<string> CompletedResearch = new List<string>();
        public List<string> UnlockedSkills = new List<string>();
        public List<string> UnlockedContent = new List<string>();
        public List<string> CompletedMilestones = new List<string>();
        public List<string> CompletedObjectives = new List<string>();
        
        [Header("Campaign Progress")]
        public CampaignProgressionData CampaignProgress = new CampaignProgressionData();
        public CampaignProgressionData CampaignData = new CampaignProgressionData(); // Alias for compatibility
        
        [Header("Statistics")]
        public PlayerStatTracker StatTracker = new PlayerStatTracker();
        public int FacilitiesBuilt = 0;
        public float TotalProfit = 0f;
        public int ResearchCompleted = 0;
        public int AchievementsUnlocked = 0;
        
        [Header("Skill Levels")]
        public Dictionary<string, int> SkillLevels = new Dictionary<string, int>();
        public Dictionary<string, float> SkillExperience = new Dictionary<string, float>();
        
        [Header("Preferences")]
        public ProgressionPreferences Preferences = new ProgressionPreferences();
        
        // Progression version for save compatibility
        public string ProgressionVersion = "1.0";
        public int OverallLevel = 1;
        
        public int GetSkillLevel(string skillId)
        {
            return SkillLevels.ContainsKey(skillId) ? SkillLevels[skillId] : 1;
        }
        
        public float GetSkillExperience(string skillId)
        {
            return SkillExperience.ContainsKey(skillId) ? SkillExperience[skillId] : 0f;
        }
        
        public bool HasCompletedAchievement(string achievementId)
        {
            return CompletedAchievements.Contains(achievementId);
        }
        
        public bool HasUnlockedSkill(string skillId)
        {
            return UnlockedSkills.Contains(skillId);
        }
        
        public bool HasCompletedResearch(string researchId)
        {
            return CompletedResearch.Contains(researchId);
        }
        
        public bool HasUnlockedContent(string contentId)
        {
            return UnlockedContent.Contains(contentId);
        }
    }
    
    /// <summary>
    /// Campaign progression data for tracking story and phase progress
    /// </summary>
    [System.Serializable]
    public class CampaignProgressionData
    {
        public CampaignPhase CurrentPhase = CampaignPhase.Tutorial;
        public List<string> CompletedMilestones = new List<string>();
        public List<string> UnlockedStoryContent = new List<string>();
        public float CampaignProgress = 0f;
        public System.DateTime LastCampaignUpdate;
        public Dictionary<string, bool> PhaseCompletionStatus = new Dictionary<string, bool>();
        
        // Additional properties needed by CampaignManager
        public float PhaseProgress = 0f;
        public System.DateTime PhaseStartTime;
        public List<CampaignPhase> CompletedPhases = new List<CampaignPhase>();
        public List<string> ReachedMilestones = new List<string>();
    }
    
    /// <summary>
    /// Player statistics tracker for detailed progression analytics
    /// </summary>
    [System.Serializable]
    public class PlayerStatTracker
    {
        [Header("Cultivation Stats")]
        public int PlantsGrown = 0;
        public int PlantsHarvested = 0;
        public int SuccessfulBreeds = 0;
        public float TotalYield = 0f;
        public float BestQualityAchieved = 0f;
        
        [Header("Research Stats")]
        public int ResearchProjectsCompleted = 0;
        public float TotalResearchTime = 0f;
        public int InnovationsDiscovered = 0;
        public int PatentsEarned = 0;
        
        [Header("Business Stats")]
        public float TotalRevenue = 0f;
        public float TotalProfit = 0f; // Alias for compatibility
        public float TotalExpenses = 0f;
        public int SuccessfulSales = 0;
        public float BestSalePrice = 0f;
        public int FacilitiesConstructed = 0;
        public int FacilitiesBuilt = 0; // Alias for compatibility
        
        [Header("Teaching Stats")]
        public int StudentsTeaching = 0;
        public int LessonsGiven = 0;
        public float TeachingEffectiveness = 0f;
        public int MentorshipPrograms = 0;
        
        [Header("Time Stats")]
        public float TotalPlayTime = 0f;
        public int SessionsPlayed = 0;
        public System.DateTime FirstPlayDate;
        public System.DateTime LastActiveDate;
    }
    
    /// <summary>
    /// Player progression preferences and settings
    /// </summary>
    [System.Serializable]
    public class ProgressionPreferences
    {
        [Header("Notification Preferences")]
        public bool ShowSkillLevelUpNotifications = true;
        public bool ShowAchievementNotifications = true;
        public bool ShowResearchCompletionNotifications = true;
        public bool ShowMilestoneNotifications = true;
        
        [Header("Progression Settings")]
        public bool AutoSaveProgression = true;
        public bool EnableProgressionAnalytics = true;
        public bool ShowProgressionTips = true;
        public bool EnableProgressionEffects = true;
        
        [Header("Difficulty Preferences")]
        public float ExperienceMultiplier = 1f;
        public float ProgressionDifficulty = 1f;
        public bool EnableHardcoreMode = false;
        public bool EnableChallengeMode = false;
    }
    
    /// <summary>
    /// Campaign milestone data structure
    /// </summary>
    [System.Serializable]
    public class CampaignMilestone
    {
        public string MilestoneId;
        public string MilestoneName;
        public string Description;
        public CampaignPhase RequiredPhase;
        public List<MilestoneRequirement> Requirements = new List<MilestoneRequirement>();
        public List<MilestoneReward> Rewards = new List<MilestoneReward>();
        public bool IsCompleted = false;
        public System.DateTime CompletionDate;
        public System.DateTime EstimatedCompletionTime;
        public float EstimatedDurationHours = 1f; // Duration in hours for this milestone
    }
    
    /// <summary>
    /// Milestone requirement structure
    /// </summary>
    [System.Serializable]
    public class MilestoneRequirement
    {
        public MilestoneRequirementType RequirementType;
        public string TargetId;
        public float RequiredValue;
        public string Description;
    }
    
    /// <summary>
    /// Milestone reward structure
    /// </summary>
    [System.Serializable]
    public class MilestoneReward
    {
        public MilestoneRewardType RewardType;
        public float RewardValue;
        public string RewardDescription;
        public SkillNodeSO UnlockedSkillNode;
        public string UnlockedFeature;
        public bool IsPermanentBonus = true;
        
        // Compatibility properties for ComprehensiveProgressionManager
        public MilestoneRewardType Type => RewardType; // Compatibility alias
        public string TargetId => UnlockedFeature ?? UnlockedSkillNode?.SkillId ?? ""; // Compatibility alias
        public float Amount => RewardValue; // Compatibility alias
    }
    
    // Enumerations
    public enum AchievementCategory
    {
        Cultivation_Mastery,
        Genetics_Innovation,
        Research_Excellence,
        Business_Success,
        Teaching_Mentorship,
        Collaboration_Leadership,
        Quality_Achievement,
        Efficiency_Optimization,
        Innovation_Pioneer,
        Community_Builder
    }
    
    public enum AchievementType
    {
        One_Time,
        Progressive,
        Repeatable,
        Seasonal,
        Hidden,
        Legendary,
        SkillLevel
    }
    
    public enum AchievementRewardType
    {
        Experience_Bonus,
        Skill_Points,
        Unlock_Skill,
        Unlock_Research,
        Unlock_Equipment,
        Unlock_Feature,
        Permanent_Bonus,
        Cosmetic_Reward
    }
    
    public enum ProgressType
    {
        Count,
        Accumulation,
        Achievement,
        Ratio,
        Quality_Threshold,
        Time_Based,
        Skill_Level,
        Research_Completion
    }
    
    public enum ComparisonType
    {
        Equals,
        Greater_Than,
        Greater_Equal,
        Less_Than,
        Less_Equal,
        Not_Equals
    }
    
    public enum CampaignPhase
    {
        Tutorial,
        EarlyGrowth,
        Expansion,
        Specialization,
        Innovation,
        Mastery,
        Legacy
    }
    
    public enum MilestoneRequirementType
    {
        SkillLevel,
        Player_Level,
        Skill_Level,
        ResearchCompletion,
        Research_Completion,
        AchievementUnlock,
        Achievement_Unlock,
        FacilityConstruction,
        Facility_Construction,
        ProfitGeneration,
        Profit_Generation,
        QualityThreshold,
        Quality_Achievement,
        TimeInGame,
        Time_Played,
        ContentUnlock
    }
    
    public enum MilestoneRewardType
    {
        Permanent_Bonus,
        Unlock_Feature,
        Unlock_Skill,
        Title_Recognition,
        Special_Access,
        Multiplier_Bonus,
        Unique_Opportunity,
        Legacy_Achievement
    }
    
    public enum ExperienceSource
    {
        Plant_Harvest,
        Breeding_Success,
        Research_Completion,
        Quality_Achievement,
        Facility_Completion,
        Skill_Usage,
        Achievement_Unlock,
        Quest_Completion,
        Passive_Time,
        Teaching_Others,
        Collaboration,
        Gameplay,
        Research,
        Teaching,
        Innovation,
        Achievement,
        Challenge,
        Mentorship,
        Tutorial_Completion
    }
    
    public enum RewardType
    {
        Experience,
        Currency,
        SkillPoints,
        UnlockFeature,
        Title,
        Item,
        Multiplier,
        ContentUnlock,
        Skill_Point,
        Research_Unlock
    }
    
    public enum SynergyType
    {
        Domain_Synergy,
        Category_Synergy,
        Cross_Domain,
        Complementary,
        Reinforcing,
        Multiplicative
    }
    
    public enum LearningPathType
    {
        Beginner_Path,
        Specialist_Path,
        Generalist_Path,
        Expert_Path,
        Innovation_Path,
        Efficiency_Path,
        Quality_Path,
        Research_Path
    }
    
    /// <summary>
    /// Collaboration opportunity for research and learning
    /// </summary>
    [System.Serializable]
    public class CollaborationOpportunity
    {
        public string OpportunityId;
        public string Title;
        public string OpportunityName; // Compatibility alias for Title
        public string Description;
        public CollaborationType Type;
        public CollaborationType CollaborationType; // Compatibility alias for Type
        public List<string> RequiredSkills = new List<string>();
        public List<SkillCategory> RequiredExpertise = new List<SkillCategory>(); // For ResearchManager compatibility
        public List<string> RequiredResearch = new List<string>();
        public int MinPlayerLevel = 1;
        public int MinimumSkillLevel = 3; // For ResearchManager compatibility
        public float DurationDays = 30f;
        public List<CollaborationBenefit> Benefits = new List<CollaborationBenefit>();
        public float SuccessChance = 0.8f;
        public bool IsAvailable = true;
        public System.DateTime AvailableUntil;
    }
    
    /// <summary>
    /// Unlockable content definition
    /// </summary>
    [System.Serializable]
    public class UnlockableContent
    {
        public string ContentId;
        public string ContentName;
        public string Description;
        public UnlockableContentType ContentType;
        public List<UnlockRequirement> Requirements = new List<UnlockRequirement>();
        public bool IsUnlocked = false;
        public System.DateTime UnlockDate;
        public string IconPath;
        public int UnlockCost = 0;
    }
    
    /// <summary>
    /// Learning accelerator for skill progression
    /// </summary>
    [System.Serializable]
    public class LearningAccelerator
    {
        public string AcceleratorId;
        public string Name;
        public string Description;
        public AcceleratorType Type;
        public AcceleratorType AcceleratorType; // Compatibility alias
        public float ExperienceMultiplier = 1.5f;
        public float SpeedMultiplier = 1.5f; // For SkillTreeManager compatibility
        public float DurationHours = 24f;
        public float DurationDays = 1f; // DurationHours / 24
        public List<string> ApplicableSkills = new List<string>();
        public List<SkillCategory> ApplicableCategories = new List<SkillCategory>(); // For SkillTreeManager compatibility
        public List<AcceleratorRequirement> Requirements = new List<AcceleratorRequirement>();
        public int Cost = 100;
        public bool IsReusable = false;
    }
    
    /// <summary>
    /// Synergy effect definition
    /// </summary>
    [System.Serializable]
    public class SynergyEffect
    {
        public string EffectName;
        public SkillEffectType EffectType;
        public float BonusMultiplier = 1.2f;
        public string Description;
        
        // Additional properties for SkillTreeManager compatibility
        public bool RequiresBothSkills = true; // Whether both skills are required for this effect
        public int MinimumCombinedLevel = 5; // Minimum combined level for this effect
    }
    
    /// <summary>
    /// Skill synergy for bonus effects
    /// </summary>
    [System.Serializable]
    public class SkillSynergy
    {
        public string SynergyId;
        public string Name;
        public string Description;
        public List<string> RequiredSkills = new List<string>();
        public SynergyType SynergyType;
        public float BonusMultiplier = 1.2f;
        public List<string> AffectedSkills = new List<string>();
        public List<SynergyEffect> SynergyEffects = new List<SynergyEffect>();
        public bool IsActive = false;
        
        // Additional properties for SkillTreeManager compatibility
        public SkillNodeSO PrimarySkill; // Primary skill for synergy
        public SkillNodeSO SecondarySkill; // Secondary skill for synergy
        public float SynergyStrength = 1.0f; // Strength of the synergy effect
        
        // Method for checking if both skills are required
        public bool RequiresBothSkills()
        {
            return PrimarySkill != null && SecondarySkill != null;
        }
        
        public int MinimumCombinedLevel = 5; // Minimum combined level for synergy activation
    }
    
    /// <summary>
    /// Expertise area definition
    /// </summary>
    [System.Serializable]
    public class ExpertiseArea
    {
        public string AreaId;
        public string Name;
        public string Description;
        public List<SkillCategory> RelevantCategories = new List<SkillCategory>();
        public int RequiredSkillCount = 5;
        public int RequiredMasteryCount = 3;
        public float ExpertiseThreshold = 0.8f;
        public List<ExpertiseBenefit> Benefits = new List<ExpertiseBenefit>();
    }
    
    /// <summary>
    /// Learning path definition
    /// </summary>
    [System.Serializable]
    public class LearningPath
    {
        public string PathId;
        public string Name;
        public string Description;
        public LearningPathType PathType;
        public List<string> SkillSequence = new List<string>();
        public List<PathMilestone> Milestones = new List<PathMilestone>();
        public float EstimatedDurationDays = 30f;
        public int DifficultyLevel = 1;
    }
    
    // Supporting enums and classes
    // CollaborationType is defined in ResearchDataStructures.cs
    
    [System.Serializable]
    public class SkillTreeBranch
    {
        public string BranchId;
        public string BranchName;
        public string Description;
        public SkillCategory Category;
        public SkillDomain Domain;
        public List<SkillNodeSO> Skills = new List<SkillNodeSO>();
        public Vector2 BranchPosition; // For UI positioning
        public Color BranchColor = Color.white;
        public string IconPath;
        public List<string> PrerequisiteBranches = new List<string>();
        public int UnlockLevel = 1;
        public bool IsVisible = true;
    }
    
    public enum UnlockableContentType
    {
        Skill_Node,
        Research_Project,
        Equipment,
        Facility_Type,
        Feature,
        Achievement,
        Cosmetic
    }
    
    public enum AcceleratorType
    {
        Experience_Boost,
        Learning_Speed,
        Skill_Focus,
        Research_Boost,
        Collaboration_Bonus,
        Innovation_Catalyst,
        Time_Acceleration
    }

    public enum ExpertiseBenefitType
    {
        Experience_Multiplier,
        Skill_Efficiency,
        Research_Speed,
        Quality_Bonus,
        Cost_Reduction,
        Time_Reduction,
        Success_Rate_Bonus,
        Innovation_Rate
    }

    public enum AcceleratorRequirementType
    {
        Money_Investment,
        Skill_Level,
        Reputation_Level,
        Research_Completion,
        Achievement_Unlock,
        Player_Level,
        Time_Investment
    }
    
    [System.Serializable]
    public class CollaborationBenefit
    {
        public string BenefitName;
        public CollaborationBenefitType BenefitType;
        public float BenefitValue;
        public string BenefitDescription;
        public string Description; // Compatibility alias
    }
    
    // UnlockRequirement is defined in UnlockConfigSO.cs
    
    [System.Serializable]
    public class AcceleratorRequirement
    {
        public AcceleratorRequirementType RequirementType;
        public string TargetId;
        public float RequiredValue;
    }
    
    [System.Serializable]
    public class ExpertiseBenefit
    {
        public ExpertiseBenefitType BenefitType;
        public float BenefitValue;
        public string Description;
        public bool IsPermanent = true;
    }
    
    [System.Serializable]
    public class PathMilestone
    {
        public string MilestoneId;
        public string Name;
        public string Description;
        public int RequiredSkillsCompleted;
        public List<string> Rewards = new List<string>();
    }

    /// <summary>
    /// Progression metrics for analytics and monitoring
    /// </summary>
    [System.Serializable]
    public class ProgressionMetrics
    {
        public int CurrentSkillLevels;
        public int CompletedAchievements;
        public int ActiveResearchProjects;
        public int CompletedResearchProjects;
        public int UnlockedContent;
        public float TotalExperience;
        public int OverallLevel;
        public System.DateTime LastUpdate;
        public System.DateTime SessionStartTime;
        public double SessionDuration;
        public float ExperiencePerHour;
        public float AchievementRate;
        public float ResearchCompletionRate;
        
        // Compatibility properties for ComprehensiveProgressionManager
        public int TotalSkills => CurrentSkillLevels;
        public int TotalAchievements => CompletedAchievements;
        public int TotalResearchProjects => ActiveResearchProjects + CompletedResearchProjects;
    }

    /// <summary>
    /// Passive bonus from research or achievements
    /// </summary>
    [System.Serializable]
    public class PassiveBonus
    {
        public string BonusId;
        public string BonusName;
        public string Description;
        public PassiveBonusType BonusType;
        public float BonusValue;
        public string TargetSystem;
        public bool IsPermanent = true;
        public float DurationHours = 0f;
        public System.DateTime ExpirationDate;
        
        // Compatibility property
        public float Value => BonusValue; // Compatibility alias
    }

    /// <summary>
    /// Achievement requirement structure (singular)
    /// </summary>
    [System.Serializable]
    public class AchievementRequirement
    {
        public AchievementRequirementType RequirementType;
        public string TargetId;
        public float RequiredValue;
        public string Description;
        public ComparisonType Comparison = ComparisonType.Greater_Equal;
        
        // Compatibility property
        public AchievementRequirementType Type => RequirementType; // Compatibility alias
    }

    /// <summary>
    /// Progression event for event-driven system
    /// </summary>
    [System.Serializable]
    public class ProgressionEvent
    {
        public string EventId;
        public ProgressionEventType EventType;
        public string SourceSystem;
        public string TargetId;
        public float EventValue;
        public string EventData;
        public System.DateTime EventTime;
        public bool IsProcessed = false;
        
        // Additional properties for ComprehensiveProgressionManager
        public string SkillId;
        public float Amount;
        public string Source;
        public System.DateTime Timestamp;
        public int Level;
        public string ResearchId;
        public string AchievementId;
        public string ContentId;
        
        // Compatibility properties for ComprehensiveProgressionManager
        public ProgressionEventType Type => EventType;
        public float ExperienceGained => EventValue;
    }

    /// <summary>
    /// Progression notification for UI
    /// </summary>
    [System.Serializable]
    public class ProgressionNotification
    {
        public string NotificationId;
        public ProgressionNotificationType NotificationType;
        public string Title;
        public string Message;
        public string IconPath;
        public float DisplayDuration = 3f;
        public System.DateTime CreationTime;
        public System.DateTime StartTime; // For ComprehensiveProgressionManager compatibility
        public float Duration = 3f; // For ComprehensiveProgressionManager compatibility
        public bool IsActive = true;
    }

    /// <summary>
    /// Progression session tracking
    /// </summary>
    [System.Serializable]
    public class ProgressionSession
    {
        public string SessionId;
        public System.DateTime StartTime;
        public System.DateTime EndTime;
        public float ExperienceGained;
        public int SkillLevelsGained;
        public int AchievementsUnlocked;
        public int ResearchCompleted;
        public List<string> ActivitiesPerformed = new List<string>();
    }

    /// <summary>
    /// Research progress tracking
    /// </summary>
    [System.Serializable]
    public class ResearchProgress
    {
        public string ResearchId;
        public float Progress = 0f;
        public bool IsActive = false;
        public System.DateTime StartDate;
        public System.DateTime StartTime; // Compatibility alias
        public System.DateTime EstimatedCompletion;
        public System.DateTime CompletionTime; // Compatibility alias
        public float ResearchRate = 1f;
        public List<string> AssignedResearchers = new List<string>();
        public Dictionary<string, float> ResourcesAllocated = new Dictionary<string, float>();
        
        // Additional properties for compatibility
        public float PhaseProgress = 0f;
        public float MilestoneProgress = 0f;
        public float OverallProgress => (PhaseProgress * 0.7f) + (MilestoneProgress * 0.3f);
        public float EstimatedTimeRemaining = 0f;
    }

    /// <summary>
    /// Progression system report
    /// </summary>
    [System.Serializable]
    public class ProgressionSystemReport
    {
        public ProgressionMetrics Metrics;
        public PlayerProgressionData PlayerProgression;
        public Dictionary<string, int> SkillLevels = new Dictionary<string, int>();
        public HashSet<string> UnlockedContent = new HashSet<string>();
        public HashSet<string> CompletedAchievements = new HashSet<string>();
        public List<string> RecentAchievements = new List<string>();
        public List<string> ActiveResearch = new List<string>();
        public List<string> AvailableUnlocks = new List<string>();
        public List<string> RecommendedActions = new List<string>();
        public System.DateTime ReportGenerated;
    }

    /// <summary>
    /// Progression analytics data
    /// </summary>
    [System.Serializable]
    public class ProgressionAnalytics
    {
        public Dictionary<string, float> SkillUsageStats = new Dictionary<string, float>();
        public Dictionary<string, int> AchievementAttempts = new Dictionary<string, int>();
        public Dictionary<string, float> ResearchEfficiency = new Dictionary<string, float>();
        public List<ProgressionSession> SessionHistory = new List<ProgressionSession>();
        public float AverageSessionLength;
        public float TotalPlayTime;
        
        /// <summary>
        /// Update analytics data - called by ComprehensiveProgressionManager
        /// </summary>
        public void Update()
        {
            // Update session tracking
            if (SessionHistory.Count > 0)
            {
                var currentSession = SessionHistory.LastOrDefault();
                if (currentSession != null && currentSession.EndTime == default)
                {
                    // Update current session duration
                    var sessionDuration = (System.DateTime.Now - currentSession.StartTime).TotalHours;
                    TotalPlayTime += (float)sessionDuration;
                }
            }
            
            // Calculate average session length
            if (SessionHistory.Count > 0)
            {
                var completedSessions = SessionHistory.Where(s => s.EndTime != default).ToList();
                if (completedSessions.Count > 0)
                {
                    AverageSessionLength = completedSessions.Average(s => (float)(s.EndTime - s.StartTime).TotalHours);
                }
            }
        }
        
        /// <summary>
        /// Cleanup analytics data - called by ComprehensiveProgressionManager on shutdown
        /// </summary>
        public void Cleanup()
        {
            // End current session if active
            if (SessionHistory.Count > 0)
            {
                var currentSession = SessionHistory.LastOrDefault();
                if (currentSession != null && currentSession.EndTime == default)
                {
                    currentSession.EndTime = System.DateTime.Now;
                }
            }
            
            // Clear temporary data but keep persistent analytics
            // Note: We don't clear the dictionaries as they contain valuable analytics data
            // that should persist between sessions
        }
        
        /// <summary>
        /// Record a progression event for analytics
        /// </summary>
        public void RecordEvent(ProgressionEvent progressionEvent)
        {
            if (progressionEvent == null) return;
            
            // Update relevant analytics based on event type
            switch (progressionEvent.EventType)
            {
                case ProgressionEventType.Experience_Gained:
                    if (!string.IsNullOrEmpty(progressionEvent.SkillId))
                    {
                        if (!SkillUsageStats.ContainsKey(progressionEvent.SkillId))
                            SkillUsageStats[progressionEvent.SkillId] = 0f;
                        SkillUsageStats[progressionEvent.SkillId] += progressionEvent.Amount;
                    }
                    break;
                    
                case ProgressionEventType.Achievement_Unlocked:
                    if (!string.IsNullOrEmpty(progressionEvent.AchievementId))
                    {
                        if (!AchievementAttempts.ContainsKey(progressionEvent.AchievementId))
                            AchievementAttempts[progressionEvent.AchievementId] = 0;
                        AchievementAttempts[progressionEvent.AchievementId]++;
                    }
                    break;
                    
                case ProgressionEventType.Research_Completed:
                    if (!string.IsNullOrEmpty(progressionEvent.ResearchId))
                    {
                        if (!ResearchEfficiency.ContainsKey(progressionEvent.ResearchId))
                            ResearchEfficiency[progressionEvent.ResearchId] = 1f;
                        // Could calculate efficiency based on completion time vs expected time
                    }
                    break;
            }
        }
    }



    // Additional Enums
    public enum PassiveBonusType
    {
        Experience_Multiplier,
        Skill_Efficiency,
        Research_Speed,
        Quality_Bonus,
        Yield_Increase,
        Cost_Reduction,
        Time_Reduction,
        Success_Rate_Bonus
    }

    public enum AchievementRequirementType
    {
        SkillLevel,
        ResearchCompleted,
        PlantsHarvested,
        FacilitiesBuilt,
        TotalProfit,
        PlayTime,
        AchievementUnlock,
        ContentUnlock,
        PlayerLevel,
        ExperienceGained
    }

    public enum ProgressionEventType
    {
        Skill_Level_Up,
        SkillLevelUp,
        Research_Completed,
        ResearchCompleted,
        Achievement_Unlocked,
        AchievementUnlocked,
        Content_Unlocked,
        Experience_Gained,
        Milestone_Reached,
        Plant_Harvested,
        Construction_Completed,
        Sale_Completed
    }

    public enum ProgressionNotificationType
    {
        Skill_Level_Up,
        Achievement_Unlocked,
        Research_Completed,
        Content_Unlocked,
        Milestone_Reached,
        Warning,
        Information
    }



}