using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ProjectChimera.Data.IPM
{
    #region Gaming-Specific Data Structures (Non-Duplicates Only)
    
    [Serializable]
    public class IPMBattleSession
    {
        public string SessionId = System.Guid.NewGuid().ToString();
        public string PlayerId = "";
        public DateTime StartTime = DateTime.Now;
        public DateTime EndTime = DateTime.MinValue;
        public bool IsActive = true;
        public float ThreatLevel = 0.5f;
        public PestType PrimaryThreat = PestType.Aphids;
        public List<PestType> SecondaryThreats = new List<PestType>();
        public Dictionary<string, float> PerformanceMetrics = new Dictionary<string, float>();
    }
    
    [Serializable]
    public class IPMMultiplayerSession
    {
        public string SessionId = System.Guid.NewGuid().ToString();
        public string HostPlayerId = "";
        public List<string> ParticipantIds = new List<string>();
        public bool IsActive = false;
        public DateTime StartTime = DateTime.Now;
        public Dictionary<string, object> SessionData = new Dictionary<string, object>();
    }
    
    [Serializable]
    public class IPMInvasionDetector
    {
        public string DetectorId = System.Guid.NewGuid().ToString();
        public Vector3 Position = Vector3.zero;
        public float DetectionRange = 10f;
        public float Sensitivity = 0.8f;
        public bool IsActive = true;
        public List<PestType> DetectablePests = new List<PestType>();
        public DateTime LastDetection = DateTime.MinValue;
    }
    
    [Serializable]
    public class IPMMatchmakingService
    {
        public string ServiceId = System.Guid.NewGuid().ToString();
        public List<string> WaitingPlayers = new List<string>();
        public Dictionary<string, float> PlayerSkillRatings = new Dictionary<string, float>();
        public bool IsActive = true;
        public float MatchmakingTimeout = 30f;
    }
    
    [Serializable]
    public class IPMPerformanceMonitor
    {
        public string MonitorId = System.Guid.NewGuid().ToString();
        public Dictionary<string, float> PerformanceMetrics = new Dictionary<string, float>();
        public DateTime LastUpdate = DateTime.Now;
        public bool IsMonitoring = true;
        public float UpdateInterval = 1f;
    }
    
    #endregion
    
    #region IPM Recommendation Data Structures
    
    [Serializable]
    public class IPMRecommendation
    {
        public string RecommendationId = System.Guid.NewGuid().ToString();
        public string RecommendationType = "Strategy";
        public string Title = "";
        public string Description = "";
        public float Priority = 0.5f;
        public float Confidence = 0.7f;
        public DateTime Generated = DateTime.Now;
        public List<string> ActionItems = new List<string>();
        public Dictionary<string, object> Parameters = new Dictionary<string, object>();
    }
    
    #endregion
    
    #region Utility Classes for Gaming System
    
    public class IPMRecommendationEngine
    {
        public IPMRecommendation GenerateRecommendation(string playerId, Dictionary<PestType, PestAIBehavior> aiBehaviors)
        {
            return new IPMRecommendation
            {
                RecommendationId = System.Guid.NewGuid().ToString(),
                RecommendationType = "Strategy",
                Title = "AI Strategy Recommendation",
                Description = "Recommended strategy based on current conditions",
                Priority = 0.8f,
                Confidence = 0.75f,
                Generated = DateTime.Now
            };
        }
        
        private IPMStrategyType DetermineOptimalStrategy(Dictionary<PestType, PestAIBehavior> aiBehaviors)
        {
            if (aiBehaviors == null || !aiBehaviors.Any())
                return IPMStrategyType.Integrated;
                
            var avgAggression = aiBehaviors.Values.Average(b => b.Aggressiveness);
            
            if (avgAggression > 0.8f)
                return IPMStrategyType.Emergency;
            else if (avgAggression > 0.6f)
                return IPMStrategyType.Integrated;
            else
                return IPMStrategyType.Preventive;
        }
    }
    
    #endregion
} 