using UnityEngine;
using System;
using System.Collections.Generic;
using ProjectChimera.Core;

namespace ProjectChimera.Data.Community
{
    // Enhanced Leaderboard System
    [System.Serializable]
    public enum LeaderboardType
    {
        TotalYield,
        THCPotency,
        CBDContent,
        TerpeneProfile,
        FacilityEfficiency,
        BreedingInnovation,
        EconomicSuccess,
        SustainabilityScore,
        SpeedRun,
        QualityScore,
        CommunityContribution,
        ResearchPoints,
        Global,
        Regional,
        Friends,
        Guild,
        Seasonal
    }

    [System.Serializable]
    public enum LeaderboardCategory
    {
        Cultivation,
        Genetics,
        Economics,
        Efficiency,
        Innovation,
        Community,
        Competition
    }

    [System.Serializable]
    public enum TimePeriod
    {
        AllTime,
        Monthly,
        Weekly,
        Daily,
        Seasonal
    }

    [System.Serializable]
    public enum ScoreOrder
    {
        Descending,
        Ascending
    }

    [System.Serializable]
    public enum ChallengeType
    {
        Daily,
        Weekly,
        Monthly,
        Seasonal,
        Special,
        Community,
        Speedrun,
        Quality,
        Innovation,
        Collaboration
    }

    [System.Serializable]
    public enum ReputationTier
    {
        Novice,
        Apprentice,
        Cultivator,
        Expert,
        Master,
        Legend
    }

    [System.Serializable]
    public enum ForumCategory
    {
        General,
        Genetics,
        Cultivation,
        Equipment,
        Automation,
        Trading,
        Challenges,
        Showcase,
        Troubleshooting,
        Research,
        Community,
        Announcements
    }

    // Enhanced Forum System Enums
    [System.Serializable]
    public enum TopicType
    {
        Discussion,
        Question,
        Guide,
        Showcase,
        Bug_Report,
        Feature_Request,
        Announcement
    }

    [System.Serializable]
    public enum TopicStatus
    {
        Open,
        Closed,
        Archived,
        Deleted
    }

    [System.Serializable]
    public enum PostStatus
    {
        Published,
        Moderated,
        Hidden,
        Deleted
    }

    [System.Serializable]
    public enum VoteType
    {
        None,
        UpVote,
        DownVote
    }

    [System.Serializable]
    public enum AttachmentType
    {
        Image,
        Video,
        Document,
        Save_File
    }

    // Reputation System Enums
    [System.Serializable]
    public enum ReputationType
    {
        Helpful_Posts,
        Quality_Content,
        Community_Participation,
        Event_Performance,
        Mentorship,
        Bug_Reports,
        Feedback,
        Moderation_Help
    }

    [System.Serializable]
    public enum PlayerStatus
    {
        Online,
        Away,
        Busy,
        Invisible,
        Offline
    }

    [System.Serializable]
    public enum PrivacyLevel
    {
        Public,
        Friends,
        Private
    }

    [System.Serializable]
    public class PlayerProfile
    {
        [Header("Player Identity")]
        public string PlayerID;
        public string DisplayName;
        public string ProfileImageURL;
        public DateTime JoinDate;
        public DateTime LastActive;
        public PlayerStatus Status = PlayerStatus.Offline;
        
        [Header("Player Progression")]
        public int Level = 1;
        public int Experience = 0;
        
        [Header("Reputation System")]
        public float ReputationScore;
        public int ReputationPoints;
        public ReputationTier CurrentTier;
        public int TotalVotes;
        public int PositiveVotes;
        public float TrustScore;
        
        [Header("Achievements & Statistics")]
        public int TotalHarvests;
        public float BestYield;
        public float BestTHC;
        public float BestCBD;
        public int StrainsCreated;
        public int TradesCompleted;
        public float TradeSuccessRate;
        
        [Header("Community Engagement")]
        public int ForumPosts;
        public int HelpfulVotes;
        public int ChallengesCompleted;
        public int ChallengesWon;
        public List<string> Badges = new List<string>();
        public List<string> Achievements = new List<string>();
        
        [Header("Specializations")]
        public List<string> Specialties = new List<string>();
        public Dictionary<string, float> SkillRatings = new Dictionary<string, float>();
        
        public bool IsVerified;
        public bool IsModerator;
        public string Location; // Optional, general region
        public string Bio;
        
        // Compatibility property for CommunityManager
        public string PlayerId => PlayerID;
    }

    [System.Serializable]
    public class LeaderboardEntry
    {
        public string PlayerID;
        public string DisplayName;
        public float Score;
        public int Rank;
        public DateTime SubmissionDate;
        public string AdditionalData; // JSON for extra info
        public bool IsVerified;
        public int VoteCount;
        public string ProofImageURL;
    }

    [System.Serializable]
    public class Leaderboard
    {
        [Header("Leaderboard Configuration")]
        public string LeaderboardID;
        public LeaderboardType Type;
        public string Name;
        public string Description;
        public DateTime StartDate;
        public DateTime EndDate;
        public bool IsActive;
        public int MaxEntries;
        
        [Header("Entries & Rankings")]
        public List<LeaderboardEntry> Entries = new List<LeaderboardEntry>();
        public Dictionary<string, object> Rewards = new Dictionary<string, object>();
        
        [Header("Requirements")]
        public int MinimumLevel;
        public List<string> RequiredAchievements = new List<string>();
        public bool RequiresVerification;
    }

    [System.Serializable]
    public class Challenge
    {
        [Header("Challenge Details")]
        public string ChallengeID;
        public string Name;
        public string Description;
        public ChallengeType Type;
        public DateTime StartDate;
        public DateTime EndDate;
        public bool IsActive;
        
        [Header("Requirements & Rules")]
        public Dictionary<string, object> Parameters = new Dictionary<string, object>();
        public List<string> Requirements = new List<string>();
        public string Rules;
        
        [Header("Rewards & Recognition")]
        public Dictionary<string, object> Rewards = new Dictionary<string, object>();
        public List<string> ParticipantIDs = new List<string>();
        public List<LeaderboardEntry> Submissions = new List<LeaderboardEntry>();
        
        [Header("Community Features")]
        public int ParticipantCount;
        public float AverageScore;
        public string CreatedBy; // For community challenges
        public bool RequiresApproval;
    }

    [System.Serializable]
    public class ForumPost
    {
        [Header("Post Information")]
        public string PostID;
        public string ThreadID;
        public string AuthorID;
        public string Title;
        public string Content;
        public DateTime CreatedDate;
        public DateTime LastModified;
        
        [Header("Engagement")]
        public int Views;
        public int Likes;
        public int Replies;
        public List<string> Tags = new List<string>();
        public ForumCategory Category;
        
        [Header("Media & Attachments")]
        public List<string> ImageURLs = new List<string>();
        public List<string> AttachmentURLs = new List<string>();
        
        [Header("Moderation")]
        public bool IsLocked;
        public bool IsPinned;
        public bool IsDeleted;
        public string ModerationNotes;
        
        [Header("Helpfulness")]
        public int HelpfulVotes;
        public bool IsMarkedSolution;
        public float QualityScore;
    }

    [System.Serializable]
    public class ForumThread
    {
        [Header("Thread Information")]
        public string ThreadID;
        public string OriginalPostID;
        public ForumCategory Category;
        public List<string> PostIDs = new List<string>();
        
        [Header("Activity")]
        public DateTime LastActivity;
        public int TotalViews;
        public int TotalReplies;
        public string LastReplyAuthor;
        
        [Header("Status")]
        public bool IsLocked;
        public bool IsPinned;
        public bool IsSticky;
        public bool HasSolution;
    }

    [System.Serializable]
    public class CommunityEvent
    {
        [Header("Event Details")]
        public string EventID;
        public string Name;
        public string Description;
        public DateTime StartDate;
        public DateTime EndDate;
        public string EventType; // Contest, Collaboration, Research, etc.
        
        [Header("Participation")]
        public List<string> ParticipantIDs = new List<string>();
        public Dictionary<string, object> Requirements = new Dictionary<string, object>();
        public Dictionary<string, object> Rewards = new Dictionary<string, object>();
        
        [Header("Progress Tracking")]
        public float OverallProgress;
        public Dictionary<string, float> IndividualProgress = new Dictionary<string, float>();
        public bool IsCompleted;
    }

    [System.Serializable]
    public class Badge
    {
        [Header("Badge Information")]
        public string BadgeID;
        public string Name;
        public string Description;
        public string IconURL;
        public string Category;
        
        [Header("Requirements")]
        public Dictionary<string, object> UnlockCriteria = new Dictionary<string, object>();
        public bool IsHidden; // Secret badges
        public int Rarity; // 1-5 stars
        
        [Header("Statistics")]
        public int TotalAwarded;
        public DateTime FirstAwarded;
        public List<string> HolderIDs = new List<string>();
    }

    [System.Serializable]
    public class TradeReputation
    {
        [Header("Trading Statistics")]
        public string PlayerID;
        public int TotalTrades;
        public int SuccessfulTrades;
        public int DisputedTrades;
        public float AverageRating;
        
        [Header("Trade Categories")]
        public Dictionary<string, int> TradesByCategory = new Dictionary<string, int>();
        public Dictionary<string, float> CategoryRatings = new Dictionary<string, float>();
        
        [Header("Trust Metrics")]
        public float TrustScore;
        public List<string> TrustedByPlayerIDs = new List<string>();
        public List<string> PositiveFeedback = new List<string>();
        public List<string> NegativeFeedback = new List<string>();
        
        [Header("Recent Activity")]
        public DateTime LastTrade;
        public List<string> RecentTradeIDs = new List<string>();
        public float RecentPerformance; // Last 30 days
    }

    [System.Serializable]
    public class CommunityMetrics
    {
        [Header("Overall Statistics")]
        public int TotalActiveUsers;
        public int TotalForumPosts;
        public int TotalChallengesCompleted;
        public int TotalTradesCompleted;
        
        [Header("Engagement Metrics")]
        public float AverageDailyActiveUsers;
        public float AverageSessionLength;
        public float CommunityHealthScore;
        
        [Header("Content Statistics")]
        public Dictionary<string, int> PostsByCategory = new Dictionary<string, int>();
        public Dictionary<string, int> ChallengesByType = new Dictionary<string, int>();
        public Dictionary<string, float> LeaderboardDistribution = new Dictionary<string, float>();
        
        [Header("Growth Metrics")]
        public int NewUsersThisMonth;
        public float RetentionRate;
        public float EngagementGrowth;
    }

    // Enhanced Forum System Data Structures
    [System.Serializable]
    public class EnhancedForumTopic
    {
        [Header("Topic Information")]
        public string TopicId;
        public string Title;
        public string AuthorId;
        public string AuthorName;
        public DateTime CreatedDate;
        public DateTime LastReplyDate;
        public TopicStatus Status;
        public TopicType Type;
        
        [Header("Engagement")]
        public int ViewCount;
        public int ReplyCount;
        public bool IsSticky;
        public bool IsLocked;
        public List<string> Tags = new List<string>();
        public TopicRating Rating;
        
        [Header("Posts")]
        public List<EnhancedForumPost> Posts = new List<EnhancedForumPost>();
    }

    [System.Serializable]
    public class EnhancedForumPost
    {
        [Header("Post Information")]
        public string PostId;
        public string TopicId;
        public string AuthorId;
        public string AuthorName;
        public string Content;
        public DateTime PostDate;
        public DateTime LastEditDate;
        public string EditedBy;
        
        [Header("Attachments and Media")]
        public List<ForumAttachment> Attachments = new List<ForumAttachment>();
        
        [Header("Voting and Interaction")]
        public PostVoting Voting;
        public List<ForumReply> Replies = new List<ForumReply>();
        
        [Header("Moderation")]
        public PostStatus Status;
        public bool IsDeleted;
        public string DeleteReason;
    }

    [System.Serializable]
    public class ForumReply
    {
        public string ReplyId;
        public string ParentPostId;
        public string AuthorId;
        public string AuthorName;
        public string Content;
        public DateTime ReplyDate;
        public PostVoting Voting;
        public bool IsDeleted;
    }

    [System.Serializable]
    public class ForumAttachment
    {
        public string AttachmentId;
        public string FileName;
        public string FilePath;
        public AttachmentType Type;
        public long FileSize;
        public DateTime UploadDate;
        public string Description;
    }

    [System.Serializable]
    public class PostVoting
    {
        public int UpVotes;
        public int DownVotes;
        public int NetScore;
        public List<string> VotedUserIds = new List<string>();
        public bool HasUserVoted;
        public VoteType UserVoteType;
    }

    [System.Serializable]
    public class TopicRating
    {
        public float AverageRating;
        public int TotalRatings;
        public Dictionary<int, int> RatingDistribution = new Dictionary<int, int>();
        public bool HasUserRated;
        public int UserRating;
    }

    // Enhanced Leaderboard System
    [System.Serializable]
    public class EnhancedLeaderboard
    {
        [Header("Leaderboard Configuration")]
        public string LeaderboardId;
        public string Name;
        public string Description;
        public LeaderboardType Type;
        public LeaderboardCategory Category;
        public TimePeriod Period;
        public DateTime LastUpdated;
        public bool IsActive;
        public int MaxEntries;
        
        [Header("Entries and Settings")]
        public List<EnhancedLeaderboardEntry> Entries = new List<EnhancedLeaderboardEntry>();
        public LeaderboardSettings Settings;
    }

    [System.Serializable]
    public class EnhancedLeaderboardEntry
    {
        public int Rank;
        public string PlayerId;
        public string PlayerName;
        public float Score;
        public string FormattedScore;
        public DateTime SubmissionDate;
        public LeaderboardEntryData AdditionalData;
        public int Change; // Position change from last period
        public bool IsCurrentPlayer;
        public bool IsVerified;
    }

    [System.Serializable]
    public class LeaderboardEntryData
    {
        public Dictionary<string, object> CustomData = new Dictionary<string, object>();
        public string ScreenshotPath;
        public string Description;
        public List<string> Tags = new List<string>();
    }

    [System.Serializable]
    public class LeaderboardSettings
    {
        public bool AllowTies;
        public ScoreOrder SortOrder;
        public int MinimumScore;
        public bool RequireVerification;
        public TimeSpan UpdateInterval;
        public bool ShowPlayerRank;
        public bool ShowScoreHistory;
    }

    // Enhanced Reputation System
    [System.Serializable]
    public class ReputationSystem
    {
        public string PlayerId;
        public int TotalReputation;
        public ReputationLevel Level;
        public List<ReputationSource> Sources = new List<ReputationSource>();
        public ReputationHistory History;
        public List<ReputationBonus> ActiveBonuses = new List<ReputationBonus>();
        public DateTime LastUpdated;
    }

    [System.Serializable]
    public class ReputationSource
    {
        public ReputationType Type;
        public int Points;
        public DateTime EarnedDate;
        public string Description;
        public string SourceId;
    }

    [System.Serializable]
    public class ReputationHistory
    {
        public List<ReputationChange> Changes = new List<ReputationChange>();
        public Dictionary<ReputationType, int> TotalByType = new Dictionary<ReputationType, int>();
        public int WeeklyGain;
        public int MonthlyGain;
        public float AverageWeeklyGain;
    }

    [System.Serializable]
    public class ReputationChange
    {
        public DateTime Date;
        public ReputationType Type;
        public int PointsChanged;
        public int NewTotal;
        public string Reason;
        public string SourceId;
    }

    [System.Serializable]
    public class ReputationBonus
    {
        public string BonusId;
        public string Name;
        public float Multiplier;
        public DateTime ExpiryDate;
        public ReputationType AppliesTo;
        public string Description;
    }

    [System.Serializable]
    public class ReputationLevel
    {
        public int Level;
        public string Name;
        public string Description;
        public int RequiredReputation;
        public Color LevelColor;
        public string IconPath;
        public List<ReputationPerk> Perks = new List<ReputationPerk>();
    }

    [System.Serializable]
    public class ReputationPerk
    {
        public string Name;
        public string Description;
        public string Type;
        public object PerkData;
        public bool IsActive;
    }

    // Community Events System
    [System.Serializable]
    public class EnhancedCommunityEvent
    {
        [Header("Event Information")]
        public string EventId;
        public string Name;
        public string Description;
        public string EventType;
        public DateTime StartDate;
        public DateTime EndDate;
        public string Status;
        
        [Header("Participation")]
        public List<EventParticipant> Participants = new List<EventParticipant>();
        public EventRewards Rewards;
        public EventRequirements Requirements;
        public string BannerImagePath;
        
        [Header("Progress and Leaderboard")]
        public List<EventMilestone> Milestones = new List<EventMilestone>();
        public EventLeaderboard Leaderboard;
        public float OverallProgress;
    }

    [System.Serializable]
    public class EventParticipant
    {
        public string PlayerId;
        public string PlayerName;
        public DateTime JoinDate;
        public EventParticipationData Data;
        public List<EventReward> EarnedRewards = new List<EventReward>();
        public bool IsActive;
    }

    [System.Serializable]
    public class EventParticipationData
    {
        public Dictionary<string, float> Scores = new Dictionary<string, float>();
        public Dictionary<string, object> CustomData = new Dictionary<string, object>();
        public int Rank;
        public float TotalScore;
        public List<string> CompletedChallenges = new List<string>();
    }

    [System.Serializable]
    public class EventRewards
    {
        public List<EventReward> Rewards = new List<EventReward>();
        public EventReward GrandPrize;
        public List<EventReward> ParticipationRewards = new List<EventReward>();
        public EventReward DailyRewards;
    }

    [System.Serializable]
    public class EventReward
    {
        public string RewardId;
        public string Name;
        public string Description;
        public string Type;
        public object RewardData;
        public string IconPath;
        public string Rarity;
        public bool IsExclusive;
    }

    [System.Serializable]
    public class EventRequirements
    {
        public int MinimumLevel;
        public int MinimumReputation;
        public List<string> RequiredAchievements = new List<string>();
        public List<string> RequiredBadges = new List<string>();
        public bool RequiresPremium;
        public DateTime RegistrationDeadline;
    }

    [System.Serializable]
    public class EventMilestone
    {
        public string MilestoneId;
        public string Name;
        public string Description;
        public int RequiredScore;
        public EventReward Reward;
        public bool IsReached;
        public DateTime ReachedDate;
    }

    [System.Serializable]
    public class EventLeaderboard
    {
        public string LeaderboardId;
        public List<EventParticipant> Rankings = new List<EventParticipant>();
        public DateTime LastUpdated;
        public LeaderboardDisplaySettings DisplaySettings;
    }

    [System.Serializable]
    public class LeaderboardDisplaySettings
    {
        public bool ShowRealTime;
        public bool ShowHistory;
        public bool ShowPlayerComparison;
        public int MaxDisplayedRanks;
        public bool HighlightPlayerRank;
    }
}