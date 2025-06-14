using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Community;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectChimera.Systems.Community
{
    /// <summary>
    /// Comprehensive community management system for Project Chimera.
    /// Handles leaderboards, forums, reputation, community events, and social features
    /// to create an engaging multiplayer experience and foster community interaction.
    /// </summary>
    public class CommunityManager : ChimeraManager
    {
        [Header("Community Configuration")]
        [SerializeField] private bool _enableLeaderboards = true;
        [SerializeField] private bool _enableForum = true;
        [SerializeField] private bool _enableReputationSystem = true;
        [SerializeField] private bool _enableCommunityEvents = true;
        [SerializeField] private int _maxLeaderboardEntries = 1000;
        [SerializeField] private float _leaderboardUpdateInterval = 300f; // 5 minutes
        
        [Header("Event Channels")]
        [SerializeField] private SimpleGameEventSO _onReputationChanged;
        [SerializeField] private SimpleGameEventSO _onLeaderboardUpdated;
        [SerializeField] private SimpleGameEventSO _onForumPostCreated;
        [SerializeField] private SimpleGameEventSO _onCommunityEventStarted;
        [SerializeField] private SimpleGameEventSO _onAchievementUnlocked;
        
        // Core Community Data
        private PlayerProfile _currentPlayerProfile;
        private Dictionary<string, EnhancedLeaderboard> _leaderboards = new Dictionary<string, EnhancedLeaderboard>();
        private List<EnhancedForumTopic> _forumTopics = new List<EnhancedForumTopic>();
        private Dictionary<string, ReputationSystem> _playerReputations = new Dictionary<string, ReputationSystem>();
        private List<EnhancedCommunityEvent> _activeEvents = new List<EnhancedCommunityEvent>();
        private Dictionary<string, Badge> _availableBadges = new Dictionary<string, Badge>();
        
        // Forum Management
        private Dictionary<string, EnhancedForumPost> _forumPosts = new Dictionary<string, EnhancedForumPost>();
        private Dictionary<ForumCategory, List<EnhancedForumTopic>> _topicsByCategory = new Dictionary<ForumCategory, List<EnhancedForumTopic>>();
        private List<string> _moderatorIds = new List<string>();
        
        // Performance and Caching
        private float _lastLeaderboardUpdate = 0f;
        private Dictionary<string, DateTime> _lastPlayerActivity = new Dictionary<string, DateTime>();
        private CommunityMetrics _communityMetrics = new CommunityMetrics();
        
        public override ManagerPriority Priority => ManagerPriority.Normal;
        
        // Public Properties
        public PlayerProfile CurrentPlayer => _currentPlayerProfile;
        public List<EnhancedLeaderboard> ActiveLeaderboards => _leaderboards.Values.Where(l => l.IsActive).ToList();
        public List<EnhancedForumTopic> RecentTopics => _forumTopics.OrderByDescending(t => t.LastReplyDate).Take(10).ToList();
        public List<EnhancedCommunityEvent> UpcomingEvents => _activeEvents.Where(e => e.StartDate > DateTime.Now).ToList();
        public CommunityMetrics Metrics => _communityMetrics;
        public bool IsPlayerOnline => _currentPlayerProfile?.Status == PlayerStatus.Online;
        
        // Events
        public System.Action<PlayerProfile> OnPlayerProfileUpdated;
        public System.Action<EnhancedLeaderboard> OnLeaderboardUpdated;
        public System.Action<EnhancedForumPost> OnForumPostCreated;
        public System.Action<int> OnReputationChanged; // new reputation amount
        public System.Action<EnhancedCommunityEvent> OnEventStarted;
        public System.Action<Badge> OnBadgeEarned;
        
        protected override void OnManagerInitialize()
        {
            InitializePlayerProfile();
            InitializeLeaderboards();
            InitializeForumSystem();
            InitializeReputationSystem();
            InitializeCommunityEvents();
            InitializeBadgeSystem();
            
            LogInfo("CommunityManager initialized successfully");
        }
        
        protected override void OnManagerUpdate()
        {
            float currentTime = Time.time;
            
            // Update leaderboards periodically
            if (_enableLeaderboards && currentTime - _lastLeaderboardUpdate >= _leaderboardUpdateInterval)
            {
                UpdateAllLeaderboards();
                _lastLeaderboardUpdate = currentTime;
            }
            
            // Update community events
            if (_enableCommunityEvents)
            {
                UpdateCommunityEvents();
            }
            
            // Update player activity tracking
            UpdatePlayerActivity();
            
            // Update community metrics
            UpdateCommunityMetrics();
        }
        
        #region Player Profile Management
        
        /// <summary>
        /// Update current player's profile information
        /// </summary>
        public void UpdatePlayerProfile(string displayName, string bio = "", PlayerStatus status = PlayerStatus.Online)
        {
            if (_currentPlayerProfile == null) return;
            
            _currentPlayerProfile.DisplayName = displayName;
            _currentPlayerProfile.Bio = bio;
            _currentPlayerProfile.Status = status;
            _currentPlayerProfile.LastActive = DateTime.Now;
            
            OnPlayerProfileUpdated?.Invoke(_currentPlayerProfile);
            LogInfo($"Player profile updated: {displayName}");
        }
        
        /// <summary>
        /// Add experience points to current player
        /// </summary>
        public void AddExperience(int experiencePoints, string reason = "")
        {
            if (_currentPlayerProfile == null) return;
            
            int oldLevel = _currentPlayerProfile.Level;
            _currentPlayerProfile.Experience += experiencePoints;
            
            // Check for level up (simplified calculation)
            int newLevel = Mathf.FloorToInt(_currentPlayerProfile.Experience / 1000f) + 1;
            if (newLevel > oldLevel)
            {
                _currentPlayerProfile.Level = newLevel;
                LogInfo($"Player leveled up to {newLevel}!");
                
                // Award reputation for leveling up
                AddReputation(ReputationType.Community_Participation, 50, $"Reached Level {newLevel}");
            }
            
            LogInfo($"Added {experiencePoints} experience - {reason}");
        }
        
        /// <summary>
        /// Set player's online status
        /// </summary>
        public void SetPlayerStatus(PlayerStatus status)
        {
            if (_currentPlayerProfile == null) return;
            
            _currentPlayerProfile.Status = status;
            _currentPlayerProfile.LastActive = DateTime.Now;
            
            if (status == PlayerStatus.Online)
            {
                _lastPlayerActivity[_currentPlayerProfile.PlayerId] = DateTime.Now;
            }
            
            LogInfo($"Player status set to: {status}");
        }
        
        #endregion
        
        #region Leaderboard System
        
        /// <summary>
        /// Submit a score to a specific leaderboard
        /// </summary>
        public bool SubmitScore(LeaderboardType leaderboardType, float score, string additionalData = "")
        {
            if (!_enableLeaderboards || _currentPlayerProfile == null) return false;
            
            string leaderboardId = leaderboardType.ToString();
            if (!_leaderboards.TryGetValue(leaderboardId, out var leaderboard))
            {
                LogWarning($"Leaderboard not found: {leaderboardType}");
                return false;
            }
            
            // Check if score meets minimum requirements
            if (score < leaderboard.Settings.MinimumScore)
            {
                LogWarning($"Score {score} below minimum required: {leaderboard.Settings.MinimumScore}");
                return false;
            }
            
            // Create new entry
            var entry = new EnhancedLeaderboardEntry
            {
                PlayerId = _currentPlayerProfile.PlayerId,
                PlayerName = _currentPlayerProfile.DisplayName,
                Score = score,
                FormattedScore = FormatScore(score, leaderboardType),
                SubmissionDate = DateTime.Now,
                AdditionalData = new LeaderboardEntryData
                {
                    Description = additionalData,
                    Tags = new List<string>()
                },
                IsCurrentPlayer = true,
                IsVerified = false
            };
            
            // Add or update entry
            var existingEntry = leaderboard.Entries.FirstOrDefault(e => e.PlayerId == _currentPlayerProfile.PlayerId);
            if (existingEntry != null)
            {
                // Update if score is better
                bool isBetter = leaderboard.Settings.SortOrder == ScoreOrder.Descending ? 
                    score > existingEntry.Score : score < existingEntry.Score;
                
                if (isBetter)
                {
                    leaderboard.Entries.Remove(existingEntry);
                    leaderboard.Entries.Add(entry);
                    UpdateLeaderboardRankings(leaderboard);
                    
                    LogInfo($"Updated leaderboard score: {leaderboardType} - {score}");
                    return true;
                }
                else
                {
                    LogInfo($"Score not better than existing: {existingEntry.Score}");
                    return false;
                }
            }
            else
            {
                leaderboard.Entries.Add(entry);
                UpdateLeaderboardRankings(leaderboard);
                
                LogInfo($"New leaderboard entry: {leaderboardType} - {score}");
                return true;
            }
        }
        
        /// <summary>
        /// Get leaderboard by type
        /// </summary>
        public EnhancedLeaderboard GetLeaderboard(LeaderboardType type)
        {
            string leaderboardId = type.ToString();
            return _leaderboards.TryGetValue(leaderboardId, out var leaderboard) ? leaderboard : null;
        }
        
        /// <summary>
        /// Get player's rank in a specific leaderboard
        /// </summary>
        public int GetPlayerRank(LeaderboardType type, string playerId = null)
        {
            playerId = playerId ?? _currentPlayerProfile?.PlayerId;
            if (string.IsNullOrEmpty(playerId)) return -1;
            
            var leaderboard = GetLeaderboard(type);
            if (leaderboard == null) return -1;
            
            var entry = leaderboard.Entries.FirstOrDefault(e => e.PlayerId == playerId);
            return entry?.Rank ?? -1;
        }
        
        /// <summary>
        /// Get top entries from leaderboard
        /// </summary>
        public List<EnhancedLeaderboardEntry> GetTopEntries(LeaderboardType type, int count = 10)
        {
            var leaderboard = GetLeaderboard(type);
            if (leaderboard == null) return new List<EnhancedLeaderboardEntry>();
            
            return leaderboard.Entries.OrderBy(e => e.Rank).Take(count).ToList();
        }
        
        #endregion
        
        #region Forum System
        
        /// <summary>
        /// Create a new forum topic
        /// </summary>
        public string CreateForumTopic(string title, string initialContent, ForumCategory category, TopicType type = TopicType.Discussion)
        {
            if (!_enableForum || _currentPlayerProfile == null) return null;
            
            string topicId = Guid.NewGuid().ToString();
            string postId = Guid.NewGuid().ToString();
            
            var initialPost = new EnhancedForumPost
            {
                PostId = postId,
                TopicId = topicId,
                AuthorId = _currentPlayerProfile.PlayerId,
                AuthorName = _currentPlayerProfile.DisplayName,
                Content = initialContent,
                PostDate = DateTime.Now,
                Voting = new PostVoting(),
                Status = PostStatus.Published
            };
            
            var topic = new EnhancedForumTopic
            {
                TopicId = topicId,
                Title = title,
                AuthorId = _currentPlayerProfile.PlayerId,
                AuthorName = _currentPlayerProfile.DisplayName,
                CreatedDate = DateTime.Now,
                LastReplyDate = DateTime.Now,
                Status = TopicStatus.Open,
                Type = type,
                Posts = new List<EnhancedForumPost> { initialPost }
            };
            
            _forumTopics.Add(topic);
            _forumPosts[postId] = initialPost;
            
            if (!_topicsByCategory.ContainsKey(category))
                _topicsByCategory[category] = new List<EnhancedForumTopic>();
            _topicsByCategory[category].Add(topic);
            
            // Award reputation for forum participation
            AddReputation(ReputationType.Community_Participation, 10, "Created forum topic");
            
            _onForumPostCreated?.Raise();
            OnForumPostCreated?.Invoke(initialPost);
            
            LogInfo($"Created forum topic: {title}");
            return topicId;
        }
        
        /// <summary>
        /// Reply to a forum topic
        /// </summary>
        public string ReplyToTopic(string topicId, string content)
        {
            if (!_enableForum || _currentPlayerProfile == null) return null;
            
            var topic = _forumTopics.FirstOrDefault(t => t.TopicId == topicId);
            if (topic == null || topic.Status != TopicStatus.Open || topic.IsLocked) return null;
            
            string postId = Guid.NewGuid().ToString();
            var post = new EnhancedForumPost
            {
                PostId = postId,
                TopicId = topicId,
                AuthorId = _currentPlayerProfile.PlayerId,
                AuthorName = _currentPlayerProfile.DisplayName,
                Content = content,
                PostDate = DateTime.Now,
                Voting = new PostVoting(),
                Status = PostStatus.Published
            };
            
            topic.Posts.Add(post);
            topic.ReplyCount++;
            topic.LastReplyDate = DateTime.Now;
            _forumPosts[postId] = post;
            
            // Award reputation for helpful participation
            AddReputation(ReputationType.Community_Participation, 5, "Forum reply");
            
            _onForumPostCreated?.Raise();
            OnForumPostCreated?.Invoke(post);
            
            LogInfo($"Replied to topic: {topic.Title}");
            return postId;
        }
        
        /// <summary>
        /// Vote on a forum post
        /// </summary>
        public bool VoteOnPost(string postId, VoteType voteType)
        {
            if (!_enableForum || _currentPlayerProfile == null) return false;
            
            if (!_forumPosts.TryGetValue(postId, out var post)) return false;
            
            string playerId = _currentPlayerProfile.PlayerId;
            
            // Remove existing vote if any
            if (post.Voting.VotedUserIds.Contains(playerId))
            {
                if (post.Voting.UserVoteType == VoteType.UpVote)
                    post.Voting.UpVotes--;
                else if (post.Voting.UserVoteType == VoteType.DownVote)
                    post.Voting.DownVotes--;
                
                post.Voting.VotedUserIds.Remove(playerId);
            }
            
            // Add new vote
            if (voteType != VoteType.None)
            {
                post.Voting.VotedUserIds.Add(playerId);
                post.Voting.UserVoteType = voteType;
                
                if (voteType == VoteType.UpVote)
                {
                    post.Voting.UpVotes++;
                    // Award reputation to post author for receiving upvote
                    AddReputationToPlayer(post.AuthorId, ReputationType.Quality_Content, 2, "Received upvote");
                }
                else if (voteType == VoteType.DownVote)
                {
                    post.Voting.DownVotes++;
                }
            }
            
            post.Voting.NetScore = post.Voting.UpVotes - post.Voting.DownVotes;
            post.Voting.HasUserVoted = voteType != VoteType.None;
            
            LogInfo($"Voted on post: {voteType}");
            return true;
        }
        
        /// <summary>
        /// Get topics by category
        /// </summary>
        public List<EnhancedForumTopic> GetTopicsByCategory(ForumCategory category, int limit = 20)
        {
            if (_topicsByCategory.TryGetValue(category, out var topics))
            {
                return topics.OrderByDescending(t => t.LastReplyDate).Take(limit).ToList();
            }
            return new List<EnhancedForumTopic>();
        }
        
        #endregion
        
        #region Reputation System
        
        /// <summary>
        /// Add reputation to current player
        /// </summary>
        public void AddReputation(ReputationType type, int points, string reason = "")
        {
            AddReputationToPlayer(_currentPlayerProfile?.PlayerId, type, points, reason);
        }
        
        /// <summary>
        /// Add reputation to specific player
        /// </summary>
        public void AddReputationToPlayer(string playerId, ReputationType type, int points, string reason = "")
        {
            if (string.IsNullOrEmpty(playerId)) return;
            
            if (!_playerReputations.TryGetValue(playerId, out var reputation))
            {
                reputation = new ReputationSystem
                {
                    PlayerId = playerId,
                    Sources = new List<ReputationSource>(),
                    History = new ReputationHistory
                    {
                        Changes = new List<ReputationChange>(),
                        TotalByType = new Dictionary<ReputationType, int>()
                    }
                };
                _playerReputations[playerId] = reputation;
            }
            
            // Add reputation source
            reputation.Sources.Add(new ReputationSource
            {
                Type = type,
                Points = points,
                EarnedDate = DateTime.Now,
                Description = reason,
                SourceId = Guid.NewGuid().ToString()
            });
            
            // Update totals
            int oldTotal = reputation.TotalReputation;
            reputation.TotalReputation += points;
            
            if (!reputation.History.TotalByType.ContainsKey(type))
                reputation.History.TotalByType[type] = 0;
            reputation.History.TotalByType[type] += points;
            
            // Add to history
            reputation.History.Changes.Add(new ReputationChange
            {
                Date = DateTime.Now,
                Type = type,
                PointsChanged = points,
                NewTotal = reputation.TotalReputation,
                Reason = reason,
                SourceId = Guid.NewGuid().ToString()
            });
            
            reputation.LastUpdated = DateTime.Now;
            
            // Check for reputation level changes
            CheckReputationLevelUp(reputation);
            
            // Trigger events if this is current player
            if (playerId == _currentPlayerProfile?.PlayerId)
            {
                _currentPlayerProfile.ReputationPoints = reputation.TotalReputation;
                _onReputationChanged?.Raise();
                OnReputationChanged?.Invoke(reputation.TotalReputation);
            }
            
            LogInfo($"Added {points} reputation ({type}) to {playerId}: {reason}");
        }
        
        /// <summary>
        /// Get player's reputation
        /// </summary>
        public ReputationSystem GetPlayerReputation(string playerId = null)
        {
            playerId = playerId ?? _currentPlayerProfile?.PlayerId;
            if (string.IsNullOrEmpty(playerId)) return null;
            
            return _playerReputations.TryGetValue(playerId, out var reputation) ? reputation : null;
        }
        
        #endregion
        
        #region Community Events
        
        /// <summary>
        /// Create a new community event
        /// </summary>
        public string CreateCommunityEvent(string name, string description, DateTime startDate, DateTime endDate, string eventType = "Challenge")
        {
            if (!_enableCommunityEvents) return null;
            
            string eventId = Guid.NewGuid().ToString();
            var communityEvent = new EnhancedCommunityEvent
            {
                EventId = eventId,
                Name = name,
                Description = description,
                EventType = eventType,
                StartDate = startDate,
                EndDate = endDate,
                Status = startDate > DateTime.Now ? "Upcoming" : "Active",
                Participants = new List<EventParticipant>(),
                Rewards = new EventRewards(),
                Requirements = new EventRequirements(),
                Milestones = new List<EventMilestone>(),
                Leaderboard = new EventLeaderboard { LeaderboardId = $"{eventId}_leaderboard" }
            };
            
            _activeEvents.Add(communityEvent);
            
            _onCommunityEventStarted?.Raise();
            OnEventStarted?.Invoke(communityEvent);
            
            LogInfo($"Created community event: {name}");
            return eventId;
        }
        
        /// <summary>
        /// Join a community event
        /// </summary>
        public bool JoinEvent(string eventId)
        {
            if (!_enableCommunityEvents || _currentPlayerProfile == null) return false;
            
            var communityEvent = _activeEvents.FirstOrDefault(e => e.EventId == eventId);
            if (communityEvent == null) return false;
            
            // Check if already participating
            if (communityEvent.Participants.Any(p => p.PlayerId == _currentPlayerProfile.PlayerId))
            {
                LogWarning("Player already participating in event");
                return false;
            }
            
            // Check requirements
            if (!MeetsEventRequirements(communityEvent.Requirements))
            {
                LogWarning("Player does not meet event requirements");
                return false;
            }
            
            var participant = new EventParticipant
            {
                PlayerId = _currentPlayerProfile.PlayerId,
                PlayerName = _currentPlayerProfile.DisplayName,
                JoinDate = DateTime.Now,
                Data = new EventParticipationData
                {
                    Scores = new Dictionary<string, float>(),
                    CustomData = new Dictionary<string, object>(),
                    CompletedChallenges = new List<string>()
                },
                IsActive = true
            };
            
            communityEvent.Participants.Add(participant);
            
            LogInfo($"Joined event: {communityEvent.Name}");
            return true;
        }
        
        /// <summary>
        /// Submit event score
        /// </summary>
        public bool SubmitEventScore(string eventId, string scoreCategory, float score)
        {
            if (!_enableCommunityEvents || _currentPlayerProfile == null) return false;
            
            var communityEvent = _activeEvents.FirstOrDefault(e => e.EventId == eventId);
            if (communityEvent == null) return false;
            
            var participant = communityEvent.Participants.FirstOrDefault(p => p.PlayerId == _currentPlayerProfile.PlayerId);
            if (participant == null) return false;
            
            participant.Data.Scores[scoreCategory] = score;
            participant.Data.TotalScore = participant.Data.Scores.Values.Sum();
            
            // Update event leaderboard
            UpdateEventLeaderboard(communityEvent);
            
            LogInfo($"Submitted event score: {scoreCategory} = {score}");
            return true;
        }
        
        #endregion
        
        #region Badge System
        
        /// <summary>
        /// Award badge to player
        /// </summary>
        public void AwardBadge(string badgeId, string reason = "")
        {
            if (_currentPlayerProfile == null) return;
            
            if (!_availableBadges.TryGetValue(badgeId, out var badge))
            {
                LogWarning($"Badge not found: {badgeId}");
                return;
            }
            
            if (_currentPlayerProfile.Badges.Contains(badgeId))
            {
                LogInfo($"Player already has badge: {badge.Name}");
                return;
            }
            
            _currentPlayerProfile.Badges.Add(badgeId);
            badge.TotalAwarded++;
            badge.HolderIDs.Add(_currentPlayerProfile.PlayerId);
            
            // Award reputation for earning badge
            AddReputation(ReputationType.Community_Participation, 25, $"Earned badge: {badge.Name}");
            
            OnBadgeEarned?.Invoke(badge);
            
            LogInfo($"Awarded badge: {badge.Name} - {reason}");
        }
        
        /// <summary>
        /// Check and award automatic badges based on player stats
        /// </summary>
        public void CheckAutomaticBadges()
        {
            if (_currentPlayerProfile == null) return;
            
            // Example automatic badge checks
            if (_currentPlayerProfile.TotalHarvests >= 10 && !_currentPlayerProfile.Badges.Contains("first_harvester"))
            {
                AwardBadge("first_harvester", "Completed 10 harvests");
            }
            
            if (_currentPlayerProfile.ReputationPoints >= 1000 && !_currentPlayerProfile.Badges.Contains("respected_member"))
            {
                AwardBadge("respected_member", "Reached 1000 reputation");
            }
            
            if (_currentPlayerProfile.ForumPosts >= 50 && !_currentPlayerProfile.Badges.Contains("forum_contributor"))
            {
                AwardBadge("forum_contributor", "Made 50 forum posts");
            }
        }
        
        #endregion
        
        #region Private Helper Methods
        
        private void InitializePlayerProfile()
        {
            // In a real implementation, this would load from save data
            _currentPlayerProfile = new PlayerProfile
            {
                PlayerID = Guid.NewGuid().ToString(),
                DisplayName = "Player",
                JoinDate = DateTime.Now,
                LastActive = DateTime.Now,
                Status = PlayerStatus.Online,
                Level = 1,
                Experience = 0,
                ReputationPoints = 0,
                Badges = new List<string>(),
                Achievements = new List<string>()
            };
        }
        
        private void InitializeLeaderboards()
        {
            if (!_enableLeaderboards) return;
            
            var leaderboardTypes = Enum.GetValues(typeof(LeaderboardType)).Cast<LeaderboardType>();
            foreach (var type in leaderboardTypes)
            {
                if (type == LeaderboardType.Global || type == LeaderboardType.Regional || 
                    type == LeaderboardType.Friends || type == LeaderboardType.Guild || 
                    type == LeaderboardType.Seasonal) continue; // Skip meta types
                
                var leaderboard = new EnhancedLeaderboard
                {
                    LeaderboardId = type.ToString(),
                    Name = GetLeaderboardDisplayName(type),
                    Description = GetLeaderboardDescription(type),
                    Type = type,
                    Category = GetLeaderboardCategory(type),
                    Period = TimePeriod.AllTime,
                    IsActive = true,
                    MaxEntries = _maxLeaderboardEntries,
                    Entries = new List<EnhancedLeaderboardEntry>(),
                    Settings = new LeaderboardSettings
                    {
                        SortOrder = GetLeaderboardSortOrder(type),
                        MinimumScore = 0,
                        RequireVerification = false,
                        ShowPlayerRank = true,
                        ShowScoreHistory = true
                    }
                };
                
                _leaderboards[type.ToString()] = leaderboard;
            }
        }
        
        private void InitializeForumSystem()
        {
            if (!_enableForum) return;
            
            // Initialize forum categories
            var categories = Enum.GetValues(typeof(ForumCategory)).Cast<ForumCategory>();
            foreach (var category in categories)
            {
                _topicsByCategory[category] = new List<EnhancedForumTopic>();
            }
        }
        
        private void InitializeReputationSystem()
        {
            if (!_enableReputationSystem) return;
            
            // Initialize reputation for current player
            if (_currentPlayerProfile != null)
            {
                _playerReputations[_currentPlayerProfile.PlayerId] = new ReputationSystem
                {
                    PlayerId = _currentPlayerProfile.PlayerId,
                    TotalReputation = 0,
                    Sources = new List<ReputationSource>(),
                    History = new ReputationHistory
                    {
                        Changes = new List<ReputationChange>(),
                        TotalByType = new Dictionary<ReputationType, int>()
                    },
                    LastUpdated = DateTime.Now
                };
            }
        }
        
        private void InitializeCommunityEvents()
        {
            if (!_enableCommunityEvents) return;
            
            // Create sample seasonal event
            CreateCommunityEvent(
                "Spring Growing Challenge",
                "Compete to grow the highest yielding plants this season!",
                DateTime.Now.AddDays(1),
                DateTime.Now.AddDays(30),
                "Seasonal Challenge"
            );
        }
        
        private void InitializeBadgeSystem()
        {
            // Create sample badges
            _availableBadges["first_harvester"] = new Badge
            {
                BadgeID = "first_harvester",
                Name = "First Harvester",
                Description = "Complete your first 10 harvests",
                Category = "Cultivation",
                Rarity = 2,
                TotalAwarded = 0,
                HolderIDs = new List<string>()
            };
            
            _availableBadges["respected_member"] = new Badge
            {
                BadgeID = "respected_member",
                Name = "Respected Member",
                Description = "Reach 1000 reputation points",
                Category = "Community",
                Rarity = 3,
                TotalAwarded = 0,
                HolderIDs = new List<string>()
            };
            
            _availableBadges["forum_contributor"] = new Badge
            {
                BadgeID = "forum_contributor",
                Name = "Forum Contributor",
                Description = "Make 50 helpful forum posts",
                Category = "Community",
                Rarity = 3,
                TotalAwarded = 0,
                HolderIDs = new List<string>()
            };
        }
        
        private void UpdateAllLeaderboards()
        {
            foreach (var leaderboard in _leaderboards.Values.Where(l => l.IsActive))
            {
                UpdateLeaderboardRankings(leaderboard);
                leaderboard.LastUpdated = DateTime.Now;
            }
            
            _onLeaderboardUpdated?.Raise();
        }
        
        private void UpdateLeaderboardRankings(EnhancedLeaderboard leaderboard)
        {
            // Sort entries based on score order
            var sortedEntries = leaderboard.Settings.SortOrder == ScoreOrder.Descending ?
                leaderboard.Entries.OrderByDescending(e => e.Score).ToList() :
                leaderboard.Entries.OrderBy(e => e.Score).ToList();
            
            // Update rankings
            for (int i = 0; i < sortedEntries.Count; i++)
            {
                int oldRank = sortedEntries[i].Rank;
                sortedEntries[i].Rank = i + 1;
                sortedEntries[i].Change = oldRank > 0 ? oldRank - sortedEntries[i].Rank : 0;
            }
            
            // Trim to max entries
            if (sortedEntries.Count > leaderboard.MaxEntries)
            {
                sortedEntries = sortedEntries.Take(leaderboard.MaxEntries).ToList();
            }
            
            leaderboard.Entries = sortedEntries;
            OnLeaderboardUpdated?.Invoke(leaderboard);
        }
        
        private void UpdateCommunityEvents()
        {
            var currentTime = DateTime.Now;
            
            foreach (var communityEvent in _activeEvents.ToList())
            {
                // Update event status
                if (currentTime >= communityEvent.StartDate && currentTime <= communityEvent.EndDate)
                {
                    communityEvent.Status = "Active";
                }
                else if (currentTime > communityEvent.EndDate)
                {
                    communityEvent.Status = "Ended";
                    // Could trigger event completion logic here
                }
                
                // Update event progress
                if (communityEvent.Status == "Active")
                {
                    UpdateEventProgress(communityEvent);
                }
            }
        }
        
        private void UpdateEventProgress(EnhancedCommunityEvent communityEvent)
        {
            if (communityEvent.Participants.Count == 0) return;
            
            // Calculate overall progress based on participant scores
            float totalProgress = communityEvent.Participants.Sum(p => p.Data.TotalScore);
            communityEvent.OverallProgress = totalProgress / communityEvent.Participants.Count;
            
            // Update event leaderboard
            UpdateEventLeaderboard(communityEvent);
        }
        
        private void UpdateEventLeaderboard(EnhancedCommunityEvent communityEvent)
        {
            var sortedParticipants = communityEvent.Participants
                .OrderByDescending(p => p.Data.TotalScore)
                .ToList();
            
            for (int i = 0; i < sortedParticipants.Count; i++)
            {
                sortedParticipants[i].Data.Rank = i + 1;
            }
            
            communityEvent.Leaderboard.Rankings = sortedParticipants;
            communityEvent.Leaderboard.LastUpdated = DateTime.Now;
        }
        
        private void UpdatePlayerActivity()
        {
            if (_currentPlayerProfile != null && _currentPlayerProfile.Status == PlayerStatus.Online)
            {
                _lastPlayerActivity[_currentPlayerProfile.PlayerId] = DateTime.Now;
            }
        }
        
        private void UpdateCommunityMetrics()
        {
            _communityMetrics.TotalActiveUsers = _lastPlayerActivity.Count(kvp => 
                DateTime.Now.Subtract(kvp.Value).TotalHours <= 24.0);
            
            _communityMetrics.TotalForumPosts = _forumPosts.Count;
            _communityMetrics.TotalChallengesCompleted = _activeEvents.Count(e => e.Status == "Ended");
            
            // Update engagement metrics
            _communityMetrics.CommunityHealthScore = CalculateCommunityHealthScore();
        }
        
        private bool MeetsEventRequirements(EventRequirements requirements)
        {
            if (_currentPlayerProfile == null) return false;
            
            if (_currentPlayerProfile.Level < requirements.MinimumLevel) return false;
            if (_currentPlayerProfile.ReputationPoints < requirements.MinimumReputation) return false;
            
            foreach (var requiredBadge in requirements.RequiredBadges)
            {
                if (!_currentPlayerProfile.Badges.Contains(requiredBadge)) return false;
            }
            
            return true;
        }
        
        private void CheckReputationLevelUp(ReputationSystem reputation)
        {
            // Simple reputation level calculation
            int newLevel = Mathf.FloorToInt(reputation.TotalReputation / 500f) + 1;
            if (reputation.Level == null || newLevel > reputation.Level.Level)
            {
                reputation.Level = new ReputationLevel
                {
                    Level = newLevel,
                    Name = GetReputationLevelName(newLevel),
                    RequiredReputation = (newLevel - 1) * 500,
                    LevelColor = GetReputationLevelColor(newLevel)
                };
                
                LogInfo($"Reputation level up: {reputation.Level.Name}");
            }
        }
        
        private float CalculateCommunityHealthScore()
        {
            float score = 0f;
            
            // Factor in active users
            score += Mathf.Min(_communityMetrics.TotalActiveUsers / 100f, 1f) * 25f;
            
            // Factor in forum activity
            score += Mathf.Min(_forumPosts.Count / 1000f, 1f) * 25f;
            
            // Factor in event participation
            float avgParticipation = _activeEvents.Count > 0 ? 
                (float)_activeEvents.Average(e => e.Participants.Count) : 0f;
            score += Mathf.Min(avgParticipation / 50f, 1f) * 25f;
            
            // Factor in positive interactions
            int totalUpvotes = _forumPosts.Values.Sum(p => p.Voting.UpVotes);
            score += Mathf.Min(totalUpvotes / 500f, 1f) * 25f;
            
            return score;
        }
        
        private string GetLeaderboardDisplayName(LeaderboardType type)
        {
            return type.ToString().Replace("_", " ");
        }
        
        private string GetLeaderboardDescription(LeaderboardType type)
        {
            return $"Top performers in {GetLeaderboardDisplayName(type)}";
        }
        
        private LeaderboardCategory GetLeaderboardCategory(LeaderboardType type)
        {
            return type switch
            {
                LeaderboardType.TotalYield or LeaderboardType.QualityScore => LeaderboardCategory.Cultivation,
                LeaderboardType.THCPotency or LeaderboardType.CBDContent or LeaderboardType.TerpeneProfile => LeaderboardCategory.Genetics,
                LeaderboardType.EconomicSuccess => LeaderboardCategory.Economics,
                LeaderboardType.FacilityEfficiency => LeaderboardCategory.Efficiency,
                LeaderboardType.BreedingInnovation => LeaderboardCategory.Innovation,
                LeaderboardType.CommunityContribution => LeaderboardCategory.Community,
                _ => LeaderboardCategory.Competition
            };
        }
        
        private ScoreOrder GetLeaderboardSortOrder(LeaderboardType type)
        {
            return type == LeaderboardType.SpeedRun ? ScoreOrder.Ascending : ScoreOrder.Descending;
        }
        
        private string FormatScore(float score, LeaderboardType type)
        {
            return type switch
            {
                LeaderboardType.TotalYield => $"{score:F1}g",
                LeaderboardType.THCPotency or LeaderboardType.CBDContent => $"{score:F2}%",
                LeaderboardType.EconomicSuccess => $"${score:F0}",
                LeaderboardType.FacilityEfficiency => $"{score:F1}%",
                LeaderboardType.SpeedRun => $"{score:F0} days",
                _ => score.ToString("F1")
            };
        }
        
        private string GetReputationLevelName(int level)
        {
            return level switch
            {
                1 => "Newcomer",
                2 => "Apprentice",
                3 => "Cultivator",
                4 => "Expert",
                5 => "Master",
                _ => "Legend"
            };
        }
        
        private Color GetReputationLevelColor(int level)
        {
            return level switch
            {
                1 => Color.gray,
                2 => Color.green,
                3 => Color.blue,
                4 => Color.magenta,
                5 => Color.yellow,
                _ => Color.red
            };
        }
        
        #endregion
        
        protected override void OnManagerShutdown()
        {
            LogInfo($"CommunityManager shutdown - Active events: {_activeEvents.Count}, Forum topics: {_forumTopics.Count}");
        }
    }
}