using UnityEngine;
using System;
using System.Collections.Generic;
using ProjectChimera.Core;

namespace ProjectChimera.Data.AI
{
    /// <summary>
    /// Comprehensive data structures for AI Advisor system in Project Chimera.
    /// Handles recommendations, insights, predictions, and optimization opportunities
    /// for intelligent facility management and decision support.
    /// </summary>

    [System.Serializable]
    public class AIAdvisorSettings
    {
        [Range(1, 100)] public int MaxActiveRecommendations = 10;
        [Range(1, 50)] public int MaxCriticalAlerts = 5;
        [Range(1f, 72f)] public float RecommendationValidityHours = 24f;
        [Range(0.1f, 10f)] public float AnalysisFrequencyMultiplier = 1f;
        [Range(0.5f, 1f)] public float MinimumConfidenceThreshold = 0.7f;
        public bool EnablePredictiveAnalysis = true;
        public bool EnableAutomatedOptimization = false;
        public bool EnableLearningFromUserActions = true;
    }

    [System.Serializable]
    public class AIRecommendation
    {
        public string RecommendationId;
        public string Title;
        public string Summary;
        public string Description;
        public RecommendationType Type;
        public RecommendationPriority Priority;
        public string Category;
        public DateTime CreatedAt;
        public DateTime ExpiresAt;
        public DateTime? ImplementedAt;
        public RecommendationStatus Status;
        public float ConfidenceScore;
        public float ImpactScore;
        public string ImplementationComplexity;
        public float EstimatedBenefit;
        public List<string> RequiredActions = new List<string>();
        public List<string> RelatedSystems = new List<string>();
        public string DismissalReason;
        public Dictionary<string, object> Metadata = new Dictionary<string, object>();
    }

    [System.Serializable]
    public class DataInsight
    {
        public string InsightId;
        public string Title;
        public string Description;
        public InsightSeverity Severity;
        public string Category;
        public DateTime DiscoveredAt;
        public int DataPoints;
        public float ConfidenceLevel;
        public List<string> SupportingEvidence = new List<string>();
        public List<string> RelatedMetrics = new List<string>();
        public string ActionableAdvice;
        public Dictionary<string, float> ImpactMetrics = new Dictionary<string, float>();
    }

    [System.Serializable]
    public class OptimizationOpportunity
    {
        public string OpportunityId;
        public string Title;
        public string Description;
        public string ImplementationPlan;
        public OptimizationType Type;
        public float BenefitScore;
        public OptimizationComplexity Complexity;
        public float EstimatedROI;
        public DateTime DiscoveredAt;
        public bool IsActive;
        public List<string> RequiredSystems = new List<string>();
        public List<OptimizationStep> ImplementationSteps = new List<OptimizationStep>();
        public Dictionary<string, float> ExpectedOutcomes = new Dictionary<string, float>();
        public List<string> Risks = new List<string>();
        public float ImplementationCost;
        public int EstimatedTimeToImplement; // days
    }

    [System.Serializable]
    public class OptimizationStep
    {
        public string StepName;
        public string Description;
        public int Order;
        public bool IsCompleted;
        public DateTime? CompletedAt;
        public List<string> Prerequisites = new List<string>();
        public float EstimatedDuration; // hours
        public string ResponsibleSystem;
    }

    [System.Serializable]
    public class PredictiveModel
    {
        public string ModelId;
        public string ModelName;
        public PredictiveModelType ModelType;
        public float Accuracy;
        public DateTime LastTrained;
        public int TrainingDataPoints;
        public bool IsActive;
        public List<string> InputFeatures = new List<string>();
        public string TargetVariable;
        public TimeSpan PredictionHorizon;
        public Dictionary<string, float> FeatureImportance = new Dictionary<string, float>();
        public ModelPerformanceMetrics Performance;
    }

    [System.Serializable]
    public class ModelPerformanceMetrics
    {
        public float Precision;
        public float Recall;
        public float F1Score;
        public float MeanAbsoluteError;
        public float RootMeanSquareError;
        public DateTime LastEvaluated;
        public int ValidationSamples;
    }

    [System.Serializable]
    public class AnalysisSnapshot
    {
        public DateTime Timestamp;
        public EnvironmentalSnapshot EnvironmentalData;
        public EconomicSnapshot EconomicData;
        public PerformanceSnapshot PerformanceData;
        public SystemSnapshot SystemData;
        public Dictionary<string, float> CustomMetrics = new Dictionary<string, float>();
    }

    [System.Serializable]
    public class EnvironmentalSnapshot
    {
        public int ActiveSensors;
        public int ActiveAlerts;
        public float SystemUptime;
        public float HVACEfficiency;
        public float LightingEfficiency;
        public float EnergyUsage;
        public float DLIOptimization;
        public float TemperatureVariance;
        public float HumidityVariance;
        public float CO2Levels;
        public float VPDOptimization;
    }

    [System.Serializable]
    public class EconomicSnapshot
    {
        public float Revenue;
        public float Profit;
        public float CashFlow;
        public float ROI;
        public float RiskScore;
        public float NetWorth;
        public float MarketTrend;
        public float DemandScore;
        public int ActiveContracts;
        public float CustomerSatisfaction;
        public float OperatingCosts;
    }

    [System.Serializable]
    public class PerformanceSnapshot
    {
        public float FrameRate;
        public float MemoryUsage; // MB
        public int ActiveSystems;
        public float SystemResponseTime; // ms
        public float NetworkLatency; // ms
        public float StorageUsage; // GB
        public int ActiveProcesses;
        public float CPUUtilization;
    }

    [System.Serializable]
    public class SystemSnapshot
    {
        public float SkillProgress;
        public int UnlockedNodes;
        public float ResearchProgress;
        public int CompletedProjects;
        public int ActiveAutomationRules;
        public float AutomationEfficiency;
        public int PlayerLevel;
        public float ExperiencePoints;
    }

    [System.Serializable]
    public class TrendAnalysis
    {
        public bool IsSignificant;
        public bool IsImproving;
        public float ChangePercent;
        public string TrendDirection;
        public float Confidence;
        public List<TrendDataPoint> DataPoints = new List<TrendDataPoint>();
        public DateTime AnalysisDate;
        public TimeSpan AnalysisPeriod;
    }

    [System.Serializable]
    public class TrendDataPoint
    {
        public DateTime Timestamp;
        public float Value;
        public string Label;
        public Dictionary<string, object> Metadata = new Dictionary<string, object>();
    }

    [System.Serializable]
    public class PerformancePattern
    {
        public string PatternId;
        public string PatternName;
        public PatternType Type;
        public DateTime DiscoveredAt;
        public float Confidence;
        public string Description;
        public List<string> TriggerConditions = new List<string>();
        public List<string> ObservedOutcomes = new List<string>();
        public float FrequencyScore;
        public bool IsPositive;
        public List<string> RecommendedActions = new List<string>();
    }

    [System.Serializable]
    public class AIPerformanceReport
    {
        public DateTime ReportDate;
        public float OverallEfficiencyScore;
        public float EnvironmentalScore;
        public float EconomicScore;
        public float PerformanceScore;
        public int ActiveRecommendations;
        public int OptimizationOpportunities;
        public int CriticalInsights;
        public string SystemStatus;
        public List<string> Trends = new List<string>();
        public List<AIRecommendation> Recommendations = new List<AIRecommendation>();
        public List<OptimizationOpportunity> TopOpportunities = new List<OptimizationOpportunity>();
        public Dictionary<string, float> KeyMetrics = new Dictionary<string, float>();
        public List<string> Alerts = new List<string>();
        public ReportSummary Summary;
    }

    [System.Serializable]
    public class ReportSummary
    {
        public string OverallAssessment;
        public List<string> StrengthAreas = new List<string>();
        public List<string> ImprovementAreas = new List<string>();
        public List<string> CriticalActions = new List<string>();
        public float ProjectedImprovementPotential;
        public string RecommendedFocus;
    }

    [System.Serializable]
    public class LearningRecord
    {
        public string RecordId;
        public DateTime Timestamp;
        public string ActionTaken;
        public string Context;
        public string Outcome;
        public float SuccessRating;
        public Dictionary<string, object> Parameters = new Dictionary<string, object>();
        public List<string> LessonsLearned = new List<string>();
    }

    [System.Serializable]
    public class PredictionResult
    {
        public string PredictionId;
        public string ModelId;
        public DateTime PredictionMade;
        public DateTime PredictionTarget;
        public string PredictedVariable;
        public float PredictedValue;
        public float Confidence;
        public Dictionary<string, float> InputFeatures = new Dictionary<string, float>();
        public float? ActualValue;
        public bool WasAccurate;
        public float ErrorMargin;
    }

    [System.Serializable]
    public class AlertConfiguration
    {
        public string AlertName;
        public AlertSeverity Severity;
        public string TriggerCondition;
        public float Threshold;
        public bool IsEnabled;
        public int CooldownMinutes;
        public List<string> Recipients = new List<string>();
        public string CustomMessage;
        public bool RequiresAcknowledgment;
    }

    [System.Serializable]
    public class SmartAlert
    {
        public string AlertId;
        public string Title;
        public string Message;
        public AlertSeverity Severity;
        public DateTime TriggeredAt;
        public DateTime? AcknowledgedAt;
        public AlertStatus Status;
        public string TriggerSource;
        public Dictionary<string, object> Context = new Dictionary<string, object>();
        public List<string> SuggestedActions = new List<string>();
        public bool IsAutomated;
        public float UrgencyScore;
    }

    [System.Serializable]
    public class KnowledgeBase
    {
        public List<KnowledgeItem> Items = new List<KnowledgeItem>();
        public Dictionary<string, List<string>> Categories = new Dictionary<string, List<string>>();
        public DateTime LastUpdated;
        public int TotalItems;
        public float UtilizationScore;
    }

    [System.Serializable]
    public class KnowledgeItem
    {
        public string ItemId;
        public string Title;
        public string Content;
        public string Category;
        public List<string> Tags = new List<string>();
        public float RelevanceScore;
        public DateTime CreatedAt;
        public DateTime LastAccessed;
        public int AccessCount;
        public string Source;
        public bool IsVerified;
    }

    [System.Serializable]
    public class DecisionTree
    {
        public string TreeId;
        public string TreeName;
        public DecisionNode RootNode;
        public string Description;
        public DateTime CreatedAt;
        public float AccuracyRate;
        public int TimesUsed;
        public bool IsActive;
    }

    [System.Serializable]
    public class DecisionNode
    {
        public string NodeId;
        public string Question;
        public string Condition;
        public object Threshold;
        public DecisionNode TrueNode;
        public DecisionNode FalseNode;
        public string Action;
        public bool IsLeaf;
        public float Confidence;
    }

    // Enums for AI system
    public enum RecommendationType
    {
        Critical_Action,
        Alert,
        Maintenance,
        Optimization,
        Performance,
        Strategic,
        Development,
        Business_Strategy,
        Investment,
        Training,
        Information,
        Research,
        Financial_Planning
    }

    public enum RecommendationPriority
    {
        Critical = 3,
        High = 2,
        Medium = 1,
        Low = 0
    }

    public enum RecommendationStatus
    {
        Active,
        Implemented,
        Dismissed,
        Expired,
        Superseded,
        Under_Review
    }

    public enum InsightSeverity
    {
        Critical,
        Warning,
        Info,
        Positive,
        Neutral
    }

    public enum OptimizationType
    {
        Environmental,
        Economic,
        Performance,
        Strategic,
        Automation,
        Energy,
        Process,
        Quality,
        Safety,
        Maintenance
    }

    public enum OptimizationComplexity
    {
        Low,
        Medium,
        High,
        Expert
    }

    public enum PredictiveModelType
    {
        Classification,
        Regression,
        Time_Series,
        Anomaly_Detection,
        Clustering,
        Reinforcement_Learning
    }

    public enum PatternType
    {
        Performance_Cycle,
        Efficiency_Pattern,
        Problem_Sequence,
        Optimization_Opportunity,
        Seasonal_Variation,
        User_Behavior,
        System_Correlation
    }

    public enum AlertSeverity
    {
        Info,
        Warning,
        Alert,
        Critical,
        Emergency
    }

    public enum AlertStatus
    {
        Active,
        Acknowledged,
        Resolved,
        Escalated,
        Expired
    }

    public enum AnalysisType
    {
        Quick_Scan,
        Deep_Analysis,
        Strategic_Review,
        Performance_Audit,
        Trend_Analysis,
        Predictive_Modeling,
        Optimization_Study
    }

    public enum LearningType
    {
        Supervised,
        Unsupervised,
        Reinforcement,
        Transfer,
        Online,
        Batch
    }

    public enum IntelligenceLevel
    {
        Basic_Rules,
        Pattern_Recognition,
        Predictive_Analytics,
        Machine_Learning,
        Deep_Learning,
        Artificial_General_Intelligence
    }

    public enum ConfidenceLevel
    {
        Very_Low,
        Low,
        Medium,
        High,
        Very_High,
        Certain
    }

    public enum DataQuality
    {
        Poor,
        Fair,
        Good,
        Excellent,
        Perfect
    }

    public enum ModelAccuracy
    {
        Unreliable,
        Poor,
        Fair,
        Good,
        Excellent,
        Perfect
    }
}