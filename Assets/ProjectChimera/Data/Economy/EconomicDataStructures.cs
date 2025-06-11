using UnityEngine;
using System.Collections.Generic;

namespace ProjectChimera.Data.Economy
{
    /// <summary>
    /// Supporting data structures for the economic system including relationships, 
    /// reputation, business issues, and complex market dynamics.
    /// </summary>
    
    [System.Serializable]
    public class BusinessProfile
    {
        [Range(1f, 50f)] public float YearsInBusiness = 5f;
        [Range(1, 1000)] public int EmployeeCount = 10;
        [Range(10000f, 100000000f)] public float AnnualRevenue = 1000000f;
        [Range(0f, 1f)] public float BusinessStressLevel = 0.3f;
        [Range(0f, 1f)] public float GrowthRate = 0.1f; // Annual growth rate
        public BusinessScale BusinessScale = BusinessScale.Small;
        public OperationType OperationType = OperationType.B2B;
    }
    
    [System.Serializable]
    public class BusinessActivity
    {
        public string ActivityName;
        public BusinessActivityType ActivityType;
        [Range(0f, 1f)] public float ProfitabilityScore = 0.5f;
        [Range(0f, 1f)] public float ExpertiseLevel = 0.7f;
        public bool IsPrimaryActivity = false;
        public string ActivityDescription;
    }
    
    [System.Serializable]
    public class FinancialProfile
    {
        [Range(10000f, 10000000f)] public float MaxContractValue = 100000f;
        [Range(0f, 2f)] public float LiquidityRatio = 1.2f;
        [Range(0f, 5f)] public float DebtToEquityRatio = 0.5f;
        [Range(0f, 1f)] public float PaymentReliability = 0.9f;
        [Range(0f, 100f)] public float CreditScore = 750f;
        public CreditRating CreditRating = CreditRating.Good_670_739;
        public float AveragePaymentDays = 30f;
    }
    
    [System.Serializable]
    public class RelationshipProfile
    {
        [Range(-1f, 1f)] public float LoyaltyTendency = 0.5f;
        [Range(0f, 1f)] public float RelationshipVolatility = 0.3f;
        [Range(0f, 2f)] public float TrustBuildingRate = 1f;
        [Range(0f, 2f)] public float TrustDecayRate = 0.5f;
        public bool PrefersLongTermRelationships = true;
        public bool IsInfluencedByReputation = true;
        [Range(0f, 1f)] public float ForgivenessLevel = 0.6f;
    }
    
    [System.Serializable]
    public class RelationshipModifier
    {
        public string ModifierName;
        public RelationshipFactorType FactorType;
        [Range(-1f, 1f)] public float ImpactStrength = 0.1f;
        public bool IsPositive = true;
        public string ModifierDescription;
    }
    
    [System.Serializable]
    public class MarketPreferences
    {
        public List<ProductCategory> PreferredCategories = new List<ProductCategory>();
        public List<MarketTier> PreferredTiers = new List<MarketTier>();
        [Range(0f, 100f)] public float PriceSensitivity = 50f;
        [Range(0f, 100f)] public float QualitySensitivity = 70f;
        [Range(0f, 100f)] public float BrandLoyalty = 40f;
        public bool PrefersExclusiveDeals = false;
        public bool PrefersVolumeDiscounts = true;
    }
    
    [System.Serializable]
    public class ProductPreference
    {
        public ProductType ProductType;
        [Range(0f, 1f)] public float PreferenceStrength = 0.7f;
        public Vector2 PreferredPriceRange = new Vector2(10f, 30f);
        public Vector2 PreferredQualityRange = new Vector2(0.7f, 1f);
        public List<string> PreferredAttributes = new List<string>();
        public string PreferenceReason;
    }
    
    [System.Serializable]
    public class QualityExpectations
    {
        [Range(0f, 1f)] public float MinimumQualityThreshold = 0.7f;
        [Range(0f, 1f)] public float PreferredQualityLevel = 0.85f;
        public bool RequiresConsistentQuality = true;
        public bool AcceptsVariableQuality = false;
        [Range(0f, 1f)] public float QualityToleranceRange = 0.1f;
        public List<QualityAttribute> CriticalAttributes = new List<QualityAttribute>();
    }
    
    [System.Serializable]
    public class QualityAttribute
    {
        public string AttributeName;
        [Range(0f, 1f)] public float MinimumValue = 0.7f;
        [Range(0f, 1f)] public float ImportanceWeight = 1f;
        public bool IsCritical = false;
        public string AttributeDescription;
    }
    
    [System.Serializable]
    public class ComplianceExpectation
    {
        public ComplianceType ComplianceType;
        public ComplianceStrictness Strictness = ComplianceStrictness.Standard;
        public bool IsNegotiable = false;
        public float NonComplianceTolerance = 0f;
        public string ExpectationDescription;
    }
    
    [System.Serializable]
    public class ReputationProfile
    {
        [Range(0f, 100f)] public float IndustryReputation = 70f;
        [Range(0f, 100f)] public float LocalReputation = 75f;
        [Range(0f, 100f)] public float QualityReputation = 80f;
        [Range(0f, 100f)] public float ReliabilityReputation = 85f;
        [Range(0f, 100f)] public float InnovationReputation = 60f;
        public bool IsWellKnown = false;
        public bool HasControversies = false;
        public List<string> ReputationTags = new List<string>();
    }
    
    [System.Serializable]
    public class IndustryConnection
    {
        public string ConnectionName;
        public NPCProfileSO ConnectedNPC;
        public ConnectionType ConnectionType;
        [Range(0f, 1f)] public float ConnectionStrength = 0.5f;
        public bool IsPublicConnection = true;
        public string ConnectionDescription;
    }
    
    [System.Serializable]
    public class ReputationFactor
    {
        public string FactorName;
        public ReputationFactorType FactorType;
        [Range(-50f, 50f)] public float ReputationImpact = 0f;
        public bool IsPublicKnowledge = true;
        public string FactorDescription;
    }
    
    [System.Serializable]
    public class BehavioralProfile
    {
        [Range(0f, 1f)] public float Predictability = 0.7f;
        [Range(0f, 1f)] public float Flexibility = 0.6f;
        [Range(0f, 1f)] public float Patience = 0.5f;
        [Range(0f, 1f)] public float Assertiveness = 0.6f;
        public bool IsProactive = true;
        public bool IsReactive = false;
        public ResponsePattern ResponsePattern = ResponsePattern.Measured;
        public StressResponse StressResponse = StressResponse.Rational;
    }
    
    [System.Serializable]
    public class BusinessCycle
    {
        public string CycleName;
        public CycleType CycleType;
        [Range(1, 365)] public int CycleDurationDays = 30;
        [Range(0f, 2f)] public float ActivityMultiplier = 1.2f;
        [Range(0f, 2f)] public float DemandMultiplier = 1.1f;
        public List<CyclePhase> CyclePhases = new List<CyclePhase>();
        public string CycleDescription;
    }
    
    [System.Serializable]
    public class CyclePhase
    {
        public string PhaseName;
        [Range(1, 100)] public int PhaseDurationDays = 10;
        [Range(0f, 2f)] public float ActivityLevel = 1f;
        public string PhaseDescription;
    }
    
    [System.Serializable]
    public class Personality
    {
        public bool IsExtroverted = true;
        public bool IsAnalytical = true;
        public bool IsEmotional = false;
        [Range(0f, 1f)] public float RiskTaking = 0.5f;
        [Range(0f, 1f)] public float Optimism = 0.7f;
        [Range(0f, 1f)] public float Competitiveness = 0.6f;
        public PersonalityTrait DominantTrait = PersonalityTrait.Practical;
        public List<PersonalityTrait> SecondaryTraits = new List<PersonalityTrait>();
    }
    
    [System.Serializable]
    public class ProblemProfile
    {
        [Range(0f, 0.5f)] public float BaseProblemRate = 0.05f; // 5% chance per interaction
        [Range(0f, 1f)] public float ProblemSeverityBias = 0.3f; // Tendency toward severe problems
        public bool CreatesOwnProblems = false;
        public bool AmplifiesExternalProblems = false;
        [Range(0f, 1f)] public float ProblemResolutionSkill = 0.7f;
        public List<ProblemCategory> ProblemTendencies = new List<ProblemCategory>();
    }
    
    [System.Serializable]
    public class PotentialIssue
    {
        public string IssueName;
        public IssueType IssueType;
        public IssueSeverity Severity = IssueSeverity.Minor;
        [Range(0f, 1f)] public float BaseProbability = 0.1f;
        [Range(1, 365)] public int ExpectedResolutionTime = 7; // days
        [Range(0f, 100000f)] public float FinancialImpact = 1000f;
        [Range(-1f, 1f)] public float ReputationImpact = -0.1f;
        public List<IssueCondition> TriggerConditions = new List<IssueCondition>();
        [TextArea(3, 5)] public string Description;
    }
    
    [System.Serializable]
    public class IssueCondition
    {
        public string ConditionName;
        public ConditionType ConditionType;
        public float ThresholdValue = 0.5f;
        public ComparisonType ComparisonType = ComparisonType.LessThan;
        public string ConditionDescription;
    }
    
    [System.Serializable]
    public class PlayerReputation
    {
        [Range(0f, 1f)] public float QualityScore = 0.7f;
        [Range(0f, 1f)] public float ReliabilityScore = 0.8f;
        [Range(0f, 1f)] public float InnovationScore = 0.6f;
        [Range(0f, 1f)] public float ProfessionalismScore = 0.75f;
        [Range(0f, 1f)] public float ComplianceScore = 0.9f;
        public PlayerPersonality PlayerPersonality = new PlayerPersonality();
        public List<ReputationEvent> ReputationHistory = new List<ReputationEvent>();
        
        public float OverallScore => (QualityScore + ReliabilityScore + InnovationScore + ProfessionalismScore + ComplianceScore) / 5f;
    }
    
    [System.Serializable]
    public class PlayerPersonality
    {
        public bool IsExtroverted = true;
        public bool IsAnalytical = true;
        [Range(0f, 1f)] public float RiskTaking = 0.5f;
        [Range(0f, 1f)] public float Aggressiveness = 0.4f;
    }
    
    [System.Serializable]
    public class RelationshipEvent
    {
        public string EventName;
        public RelationshipEventType EventType;
        [Range(-1f, 1f)] public float RelationshipImpact = 0.1f;
        public int DaysAgo = 0;
        public string EventDescription;
    }
    
    [System.Serializable]
    public class ReputationEvent
    {
        public string EventName;
        public ReputationEventType EventType;
        [Range(-0.2f, 0.2f)] public float ReputationChange = 0.05f;
        public int DaysAgo = 0;
        public string EventDescription;
    }
    
    [System.Serializable]
    public class ContractInterest
    {
        [Range(0f, 1f)] public float BaseInterest = 0.5f;
        [Range(0f, 1f)] public float FinancialViability = 0.7f;
        [Range(0f, 1f)] public float QualityAlignment = 0.8f;
        [Range(0f, 1f)] public float MarketTiming = 0.6f;
        [Range(0f, 1f)] public float OverallScore = 0.65f;
    }
    
    [System.Serializable]
    public class BusinessIssue
    {
        public IssueType IssueType;
        public IssueSeverity Severity;
        public string Description;
        public int TimeToResolve;
        public float FinancialImpact;
        public float ReputationImpact;
        public NPCProfileSO SourceNPC;
        public List<ResolutionOption> ResolutionOptions = new List<ResolutionOption>();
    }
    
    [System.Serializable]
    public class ResolutionOption
    {
        public string OptionName;
        public ResolutionType ResolutionType;
        public float SuccessProbability = 0.8f;
        public float CostToResolve = 100f;
        public int TimeToResolve = 3; // days
        public float RelationshipImpact = 0f;
        public string OptionDescription;
    }
    
    // Enums for the economic system
    public enum NPCType
    {
        Buyer,
        Supplier,
        Processor,
        Distributor,
        Retailer,
        Regulator,
        Investor,
        Consultant,
        Competitor,
        Service_Provider
    }
    
    public enum IndustryRole
    {
        Grower,
        Processor,
        Manufacturer,
        Distributor,
        Retailer,
        Tester,
        Consultant,
        Regulator,
        Investor,
        Technology_Provider,
        Equipment_Supplier,
        Service_Provider
    }
    
    public enum MarketPosition
    {
        Startup,
        Emerging,
        Established,
        Dominant,
        Struggling,
        Declining
    }
    
    public enum BusinessPhilosophy
    {
        Quality_Focused,
        Cost_Leadership,
        Innovation_Driven,
        Customer_Centric,
        Profit_Maximization,
        Sustainability_Focused,
        Opportunistic,
        Relationship_Based
    }
    
    public enum PaymentBehavior
    {
        Early_Payer,
        Reliable,
        Slow_Payer,
        Unreliable,
        Negotiates_Terms,
        Cash_Only,
        Credit_Dependent
    }
    
    public enum NegotiationStyle
    {
        Collaborative,
        Competitive,
        Accommodating,
        Avoiding,
        Compromising,
        Aggressive,
        Analytical
    }
    
    public enum RiskTolerance
    {
        Risk_Averse,
        Conservative,
        Moderate,
        Aggressive,
        High_Risk
    }
    
    public enum CommunicationStyle
    {
        Professional,
        Casual,
        Direct,
        Diplomatic,
        Technical,
        Emotional,
        Formal
    }
    
    public enum TrustLevel
    {
        Hostile,
        Suspicious,
        Neutral,
        Friendly,
        Allied
    }
    
    public enum InfluenceLevel
    {
        Local,
        Regional,
        National,
        International,
        Industry_Leader
    }
    
    public enum DecisionMakingStyle
    {
        Impulsive,
        Analytical,
        Collaborative,
        Intuitive,
        Data_Driven,
        Committee_Based,
        Authoritarian
    }
    
    public enum ConflictTendency
    {
        Very_Low,
        Low,
        Moderate,
        High,
        Very_High
    }
    
    public enum CrisisManagement
    {
        Poor,
        Adequate,
        Competent,
        Excellent,
        Exceptional
    }
    
    public enum BusinessScale
    {
        Micro,
        Small,
        Medium,
        Large,
        Enterprise
    }
    
    public enum OperationType
    {
        B2B,
        B2C,
        B2G,
        Mixed
    }
    
    public enum BusinessActivityType
    {
        Cultivation,
        Processing,
        Manufacturing,
        Distribution,
        Retail,
        Testing,
        Consulting,
        Research,
        Equipment_Supply,
        Service_Provision
    }
    
    public enum RelationshipFactorType
    {
        Quality_Performance,
        Delivery_Performance,
        Payment_Behavior,
        Communication,
        Compliance,
        Innovation,
        Market_Reputation,
        Personal_Chemistry
    }
    
    public enum ComplianceStrictness
    {
        Lenient,
        Standard,
        Strict,
        Very_Strict,
        Absolute
    }
    
    public enum ConnectionType
    {
        Business_Partner,
        Competitor,
        Supplier,
        Customer,
        Industry_Association,
        Personal_Friend,
        Former_Employee,
        Investor
    }
    
    public enum ReputationFactorType
    {
        Quality_Issues,
        Innovation_Success,
        Market_Leadership,
        Compliance_Record,
        Community_Relations,
        Environmental_Impact,
        Employee_Relations,
        Financial_Stability
    }
    
    public enum ResponsePattern
    {
        Immediate,
        Measured,
        Delayed,
        Variable,
        Predictable
    }
    
    public enum StressResponse
    {
        Rational,
        Emotional,
        Aggressive,
        Withdrawn,
        Collaborative,
        Defensive
    }
    
    public enum CycleType
    {
        Seasonal,
        Monthly,
        Weekly,
        Project_Based,
        Market_Driven,
        Regulatory_Driven
    }
    
    public enum PersonalityTrait
    {
        Practical,
        Visionary,
        Detail_Oriented,
        Big_Picture,
        Conservative,
        Innovative,
        Relationship_Focused,
        Task_Focused
    }
    
    public enum ProblemCategory
    {
        Financial,
        Quality,
        Delivery,
        Compliance,
        Communication,
        Technical,
        Market,
        Relationship
    }
    
    public enum IssueType
    {
        Payment_Delay,
        Quality_Dispute,
        Delivery_Delay,
        Compliance_Violation,
        Contract_Breach,
        Price_Dispute,
        Relationship_Conflict,
        Technical_Problem,
        Market_Disruption,
        Regulatory_Issue,
        Financial_Dispute,
        Communication_Breakdown
    }
    
    public enum IssueSeverity
    {
        Minor,
        Moderate,
        Major,
        Critical,
        Catastrophic
    }
    
    public enum ConditionType
    {
        Relationship_Score,
        Market_Health,
        Financial_Performance,
        Quality_Score,
        Time_Since_Last_Issue,
        Contract_Value,
        Market_Volatility
    }
    
    public enum ComparisonType
    {
        LessThan,
        LessThanOrEqual,
        Equal,
        GreaterThanOrEqual,
        GreaterThan,
        NotEqual
    }
    
    public enum RelationshipEventType
    {
        Successful_Delivery,
        Failed_Delivery,
        Payment_Issue,
        Quality_Bonus,
        Contract_Completion,
        Dispute_Resolution,
        Communication_Issue,
        Collaboration_Success
    }
    
    public enum ReputationEventType
    {
        Quality_Achievement,
        Innovation_Award,
        Compliance_Violation,
        Market_Success,
        Public_Recognition,
        Negative_Publicity,
        Industry_Leadership,
        Community_Contribution
    }
    
    public enum ResolutionType
    {
        Negotiation,
        Mediation,
        Legal_Action,
        Compensation,
        Process_Improvement,
        Relationship_Building,
        Technical_Solution,
        Policy_Change
    }
}