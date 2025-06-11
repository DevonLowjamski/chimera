using UnityEngine;
using ProjectChimera.Data.Equipment;
using ProjectChimera.Data.Genetics;
using System.Collections.Generic;

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
    
    [System.Serializable]
    public class LearningPath
    {
        public string PathName;
        public LearningPathType PathType;
        public List<SkillNodeSO> RecommendedSkillOrder = new List<SkillNodeSO>();
        public List<ResearchProjectSO> RecommendedResearch = new List<ResearchProjectSO>();
        public int EstimatedCompletionDays = 90;
        [TextArea(3, 5)] public string PathDescription;
        public Sprite PathIcon;
    }
    
    [System.Serializable]
    public class SkillTreeBranch
    {
        public string BranchName;
        public SkillCategory PrimaryCategory;
        public List<SkillDomain> RelatedDomains = new List<SkillDomain>();
        public List<SkillNodeSO> CoreSkills = new List<SkillNodeSO>();
        public List<SkillNodeSO> AdvancedSkills = new List<SkillNodeSO>();
        public List<SkillNodeSO> MasterySkills = new List<SkillNodeSO>();
        public Vector2 BranchPosition; // For UI positioning
        public Color BranchColor = Color.white;
    }
    
    [System.Serializable]
    public class ExpertiseArea
    {
        public string AreaName;
        public ExpertiseType ExpertiseType;
        public List<SkillCategory> RelevantCategories = new List<SkillCategory>();
        public int RequiredSkillCount = 5;
        public int RequiredMasteryCount = 2;
        public float ExpertiseThreshold = 0.8f;
        public List<ExpertiseBenefit> Benefits = new List<ExpertiseBenefit>();
        [TextArea(2, 4)] public string AreaDescription;
    }
    
    [System.Serializable]
    public class ExpertiseBenefit
    {
        public string BenefitName;
        public ExpertiseBenefitType BenefitType;
        public float BenefitValue;
        public bool IsPercentage = true;
        public string BenefitDescription;
    }
    
    [System.Serializable]
    public class MentorshipProgram
    {
        public string ProgramName;
        public MentorshipLevel RequiredLevel;
        public List<SkillCategory> TeachableCategories = new List<SkillCategory>();
        public int MaxStudents = 3;
        public float TeachingEfficiencyBonus = 1.5f;
        public List<MentorshipBenefit> StudentBenefits = new List<MentorshipBenefit>();
        public List<MentorshipBenefit> MentorBenefits = new List<MentorshipBenefit>();
    }
    
    [System.Serializable]
    public class CollaborationOpportunity
    {
        public string OpportunityName;
        public CollaborationType CollaborationType;
        public List<SkillCategory> RequiredExpertise = new List<SkillCategory>();
        public int MinimumSkillLevel = 5;
        public float DurationDays = 30f;
        public List<CollaborationBenefit> Benefits = new List<CollaborationBenefit>();
        public List<ResearchProjectSO> AvailableProjects = new List<ResearchProjectSO>();
    }
    
    [System.Serializable]
    public class CollaborationBenefit
    {
        public string BenefitName;
        public CollaborationBenefitType BenefitType;
        public float BenefitValue;
        public bool AppliestoAllParticipants = true;
        public string BenefitDescription;
    }
    
    [System.Serializable]
    public class ProgressionMilestone
    {
        public string MilestoneName;
        public MilestoneType MilestoneType;
        public MilestoneRequirements Requirements;
        public List<MilestoneReward> Rewards = new List<MilestoneReward>();
        public bool IsCareerDefining = false;
        public string UnlockedTitle;
        [TextArea(2, 4)] public string MilestoneDescription;
    }
    
    [System.Serializable]
    public class MilestoneRequirements
    {
        public int RequiredPlayerLevel = 25;
        public int RequiredSkillsMastered = 3;
        public int RequiredResearchCompleted = 5;
        public float RequiredExpertiseScore = 0.8f;
        public List<string> RequiredAchievements = new List<string>();
        public bool RequiresTeachingExperience = false;
    }
    
    [System.Serializable]
    public class MilestoneReward
    {
        public MilestoneRewardType RewardType;
        public float RewardValue;
        public string RewardDescription;
        public SkillNodeSO UnlockedSkillNode;
        public string UnlockedFeature;
        public bool IsPermanentBonus = true;
    }
    
    [System.Serializable]
    public class SkillSynergy
    {
        public SkillNodeSO PrimarySkill;
        public SkillNodeSO SecondarySkill;
        public SynergyType SynergyType;
        public float SynergyStrength = 0.15f;
        public List<SynergyEffect> SynergyEffects = new List<SynergyEffect>();
        public string SynergyDescription;
    }
    
    [System.Serializable]
    public class SynergyEffect
    {
        public SkillEffectType EffectType;
        public float BonusMultiplier = 1.2f;
        public bool RequiresBothSkills = true;
        public int MinimumCombinedLevel = 10;
        public string EffectDescription;
    }
    
    [System.Serializable]
    public class LearningAccelerator
    {
        public string AcceleratorName;
        public AcceleratorType AcceleratorType;
        public List<SkillCategory> ApplicableCategories = new List<SkillCategory>();
        public float SpeedMultiplier = 1.5f;
        public float CostMultiplier = 2f;
        public int DurationDays = 30;
        public List<AcceleratorRequirement> Requirements = new List<AcceleratorRequirement>();
        [TextArea(2, 3)] public string AcceleratorDescription;
    }
    
    [System.Serializable]
    public class AcceleratorRequirement
    {
        public AcceleratorRequirementType RequirementType;
        public float RequiredValue;
        public string RequirementDescription;
    }
    
    [System.Serializable]
    public class ProgressionChallenge
    {
        public string ChallengeName;
        public ChallengeType ChallengeType;
        public ChallengeDifficulty Difficulty;
        public List<ChallengeObjective> Objectives = new List<ChallengeObjective>();
        public List<ChallengeReward> Rewards = new List<ChallengeReward>();
        public int TimeLimitDays = 0; // 0 = no time limit
        public bool IsSeasonalChallenge = false;
        [TextArea(3, 5)] public string ChallengeDescription;
    }
    
    [System.Serializable]
    public class ChallengeObjective
    {
        public string ObjectiveName;
        public ObjectiveType ObjectiveType;
        public float TargetValue;
        public string TargetId;
        public bool IsOptional = false;
        public string ObjectiveDescription;
    }
    
    [System.Serializable]
    public class ChallengeReward
    {
        public ChallengeRewardType RewardType;
        public float RewardValue;
        public SkillNodeSO UnlockedSkill;
        public ResearchProjectSO UnlockedResearch;
        public string RewardDescription;
    }
    
    // Enums for progression data structures
    
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
        Legendary
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
    
    public enum LearningPathType
    {
        Beginner_Guide,
        Specialization_Track,
        Master_Craftsman,
        Research_Scientist,
        Business_Leader,
        Innovation_Pioneer,
        Teaching_Master,
        Renaissance_Cultivator
    }
    
    public enum ExpertiseType
    {
        Technical_Expert,
        Creative_Innovator,
        Scientific_Researcher,
        Business_Strategist,
        Quality_Specialist,
        Efficiency_Master,
        Teaching_Authority,
        Industry_Leader
    }
    
    public enum ExpertiseBenefitType
    {
        Skill_Bonus,
        Research_Speed,
        Quality_Bonus,
        Cost_Reduction,
        Experience_Multiplier,
        Teaching_Efficiency,
        Innovation_Rate,
        Leadership_Influence
    }
    
    public enum MentorshipLevel
    {
        Apprentice_Guide,
        Skilled_Teacher,
        Master_Mentor,
        Renowned_Authority,
        Legendary_Master
    }
    
    public enum CollaborationBenefitType
    {
        Shared_Experience,
        Resource_Pooling,
        Knowledge_Exchange,
        Accelerated_Research,
        Risk_Sharing,
        Innovation_Boost,
        Network_Expansion,
        Quality_Improvement
    }
    
    public enum MilestoneType
    {
        Skill_Mastery,
        Research_Breakthrough,
        Teaching_Excellence,
        Innovation_Achievement,
        Business_Success,
        Quality_Recognition,
        Leadership_Role,
        Industry_Recognition
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
    
    public enum AcceleratorType
    {
        Intensive_Training,
        Research_Grant,
        Mentorship_Program,
        Technology_Access,
        Collaboration_Opportunity,
        Resource_Investment,
        Time_Acceleration,
        Focus_Enhancement
    }
    
    public enum AcceleratorRequirementType
    {
        Money_Investment,
        Time_Commitment,
        Skill_Level,
        Research_Prerequisites,
        Facility_Access,
        Equipment_Requirements,
        Partnership_Needed,
        Reputation_Level
    }
    
    public enum ChallengeType
    {
        Skill_Challenge,
        Research_Challenge,
        Quality_Challenge,
        Efficiency_Challenge,
        Innovation_Challenge,
        Collaboration_Challenge,
        Teaching_Challenge,
        Business_Challenge
    }
    
    public enum ChallengeDifficulty
    {
        Novice,
        Intermediate,
        Advanced,
        Expert,
        Master,
        Legendary
    }
    
    public enum ChallengeRewardType
    {
        Experience_Boost,
        Skill_Unlock,
        Research_Unlock,
        Equipment_Unlock,
        Feature_Unlock,
        Bonus_Multiplier,
        Special_Recognition,
        Unique_Title
    }
    
    public enum Season
    {
        Spring,
        Summer,
        Autumn,
        Winter
    }
    
    public enum SynergyType
    {
        Domain_Synergy,
        Category_Synergy,
        Cross_Domain,
        Complementary,
        Specialization
    }
    
    public enum SkillEffectType
    {
        Yield_Bonus,
        Quality_Bonus,
        Growth_Speed,
        Disease_Resistance,
        Nutrient_Efficiency,
        Energy_Efficiency,
        Cost_Reduction,
        Time_Reduction,
        Success_Rate,
        Learning_Speed,
        Research_Speed,
        Innovation_Rate,
        Efficiency
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
        Mentorship
    }
}