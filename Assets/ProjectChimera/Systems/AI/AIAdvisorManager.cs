using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectChimera.Core;
using ProjectChimera.Data.AI;
// using ProjectChimera.Systems.Automation;
using ProjectChimera.Systems.Environment;
// using ProjectChimera.Systems.Economy;
// using ProjectChimera.Systems.Progression;

namespace ProjectChimera.Systems.AI
{
    /// <summary>
    /// Advanced AI Advisor System for Project Chimera.
    /// Provides intelligent recommendations, predictive analytics, and automated insights
    /// by analyzing data from all facility systems to optimize cultivation operations.
    /// </summary>
    public class AIAdvisorManager : ChimeraManager
    {
        [Header("AI Advisor Configuration")]
        [SerializeField] private AIAdvisorSettings _advisorSettings;
        [SerializeField] private bool _enableRealTimeAnalysis = true;
        [SerializeField] private bool _enablePredictiveRecommendations = true;
        [SerializeField] private bool _enableAutomatedOptimization = false;
        
        [Header("Analysis Intervals")]
        [SerializeField] private float _quickAnalysisInterval = 300f; // 5 minutes
        [SerializeField] private float _deepAnalysisInterval = 3600f; // 1 hour
        [SerializeField] private float _strategicAnalysisInterval = 86400f; // 24 hours
        
        [Header("Recommendation Priorities")]
        [SerializeField] private int _maxActiveRecommendations = 10;
        [SerializeField] private int _maxCriticalAlerts = 3;
        [SerializeField] private float _recommendationValidityHours = 24f;
        
        [Header("Event Channels")]
        [SerializeField] private SimpleGameEventSO _onNewRecommendation;
        [SerializeField] private SimpleGameEventSO _onCriticalInsight;
        [SerializeField] private SimpleGameEventSO _onOptimizationComplete;
        [SerializeField] private SimpleGameEventSO _onPerformanceAlert;
        
        // System references - accessed through GameManager events
        // No direct references needed - use event-based communication
        
        // AI data and analysis
        private List<AIRecommendation> _activeRecommendations = new List<AIRecommendation>();
        private List<DataInsight> _discoveredInsights = new List<DataInsight>();
        private List<PerformancePattern> _identifiedPatterns = new List<PerformancePattern>();
        private List<OptimizationOpportunity> _optimizationOpportunities = new List<OptimizationOpportunity>();
        
        // Analysis models
        private Dictionary<string, PredictiveModel> _models = new Dictionary<string, PredictiveModel>();
        private Queue<AnalysisSnapshot> _historicalData = new Queue<AnalysisSnapshot>();
        
        // Timing
        private float _lastQuickAnalysis;
        private float _lastDeepAnalysis;
        private float _lastStrategicAnalysis;
        
        public override ManagerPriority Priority => ManagerPriority.Low;
        public string ManagerName => "AIAdvisor";
        
        // Public Properties
        public int ActiveRecommendations => _activeRecommendations.Count(r => r.Status == RecommendationStatus.Active);
        public int CriticalInsights => _discoveredInsights.Count(i => i.Severity == InsightSeverity.Critical);
        public int OptimizationOpportunities => _optimizationOpportunities.Count(o => o.IsActive);
        public float SystemEfficiencyScore => CalculateOverallEfficiencyScore();
        public AIAdvisorSettings Settings => _advisorSettings;
        
        // Events
        public System.Action<AIRecommendation> OnNewRecommendation;
        public System.Action<DataInsight> OnCriticalInsight;
        public System.Action<OptimizationOpportunity> OnOptimizationIdentified;
        public System.Action<ProjectChimera.Data.Economy.PerformanceMetrics> OnPerformanceUpdate;
        
        protected override void OnManagerInitialize()
        {
            _lastQuickAnalysis = Time.time;
            _lastDeepAnalysis = Time.time;
            _lastStrategicAnalysis = Time.time;
            
            InitializeSystemReferences();
            InitializePredictiveModels();
            SetupBaselineRecommendations();
            
            LogInfo("AIAdvisorManager initialized with intelligent analysis capabilities");
        }
        
        protected override void OnManagerUpdate()
        {
            float currentTime = Time.time;
            
            // Quick analysis for immediate recommendations
            if (currentTime - _lastQuickAnalysis >= _quickAnalysisInterval)
            {
                PerformQuickAnalysis();
                _lastQuickAnalysis = currentTime;
            }
            
            // Deep analysis for complex insights
            if (currentTime - _lastDeepAnalysis >= _deepAnalysisInterval)
            {
                PerformDeepAnalysis();
                _lastDeepAnalysis = currentTime;
            }
            
            // Strategic analysis for long-term optimization
            if (currentTime - _lastStrategicAnalysis >= _strategicAnalysisInterval)
            {
                PerformStrategicAnalysis();
                _lastStrategicAnalysis = currentTime;
            }
            
            // Maintenance tasks
            UpdateRecommendationStatus();
            CleanupExpiredData();
        }
        
        #region System Integration
        
        private void InitializeSystemReferences()
        {
            var gameManager = GameManager.Instance;
            if (gameManager == null) return;
            
            // No direct manager references needed - use event-based communication through ChimeraManager base class
            
            LogInfo("AI Advisor integrated with available facility systems");
        }
        
        private void InitializePredictiveModels()
        {
            // Environmental prediction model
            _models["environmental_efficiency"] = new PredictiveModel
            {
                ModelId = "environmental_efficiency",
                ModelName = "Environmental Efficiency Predictor",
                ModelType = PredictiveModelType.Regression,
                Accuracy = 0.85f,
                LastTrained = DateTime.Now,
                IsActive = true
            };
            
            // Economic performance model
            _models["economic_performance"] = new PredictiveModel
            {
                ModelId = "economic_performance", 
                ModelName = "Economic Performance Predictor",
                ModelType = PredictiveModelType.Time_Series,
                Accuracy = 0.78f,
                LastTrained = DateTime.Now,
                IsActive = true
            };
            
            // Facility optimization model
            _models["facility_optimization"] = new PredictiveModel
            {
                ModelId = "facility_optimization",
                ModelName = "Facility Optimization Analyzer",
                ModelType = PredictiveModelType.Classification,
                Accuracy = 0.82f,
                LastTrained = DateTime.Now,
                IsActive = true
            };
            
            LogInfo($"Initialized {_models.Count} predictive models for AI analysis");
        }
        
        #endregion
        
        #region Analysis Engine
        
        private void PerformQuickAnalysis()
        {
            LogInfo("Performing quick analysis...");
            
            var snapshot = CaptureCurrentSnapshot();
            _historicalData.Enqueue(snapshot);
            
            // Limit historical data size
            if (_historicalData.Count > 1000)
                _historicalData.Dequeue();
            
            // Immediate recommendations
            AnalyzeEnvironmentalConditions(snapshot);
            AnalyzeSystemPerformance(snapshot);
            CheckForImmediateOptimizations(snapshot);
        }
        
        private void PerformDeepAnalysis()
        {
            LogInfo("Performing deep analysis...");
            
            if (_historicalData.Count < 10) return; // Need some data for deep analysis
            
            AnalyzeTrends();
            IdentifyPerformancePatterns();
            GenerateOptimizationRecommendations();
            UpdatePredictiveModels();
        }
        
        private void PerformStrategicAnalysis()
        {
            LogInfo("Performing strategic analysis...");
            
            if (_historicalData.Count < 100) return; // Need substantial data
            
            AnalyzeLongTermPerformance();
            IdentifyStrategicOpportunities();
            GenerateBusinessRecommendations();
            UpdateSystemEfficiencyMetrics();
        }
        
        private AnalysisSnapshot CaptureCurrentSnapshot()
        {
            var snapshot = new AnalysisSnapshot
            {
                Timestamp = DateTime.Now,
                EnvironmentalData = CaptureEnvironmentalData(),
                EconomicData = CaptureEconomicData(),
                PerformanceData = CapturePerformanceData(),
                SystemData = CaptureSystemData()
            };
            
            return snapshot;
        }
        
        private EnvironmentalSnapshot CaptureEnvironmentalData()
        {
            var data = new EnvironmentalSnapshot();
            
            // Use simulated data since we don't have direct manager access
            data.ActiveSensors = UnityEngine.Random.Range(8, 15);
            data.ActiveAlerts = UnityEngine.Random.Range(0, 3);
            data.SystemUptime = UnityEngine.Random.Range(95f, 99f);
            
            // Simulated HVAC performance metrics
            data.HVACEfficiency = UnityEngine.Random.Range(88f, 95f);
            data.EnergyUsage = UnityEngine.Random.Range(1100f, 1400f);
            
            // Simulated lighting performance
            data.LightingEfficiency = UnityEngine.Random.Range(85f, 92f);
            data.DLIOptimization = UnityEngine.Random.Range(83f, 90f);
            
            return data;
        }
        
        private EconomicSnapshot CaptureEconomicData()
        {
            var data = new EconomicSnapshot();
            
            // Use simulated financial data since we don't have direct manager access
            data.Revenue = UnityEngine.Random.Range(40000f, 50000f);
            data.Profit = UnityEngine.Random.Range(10000f, 15000f);
            data.CashFlow = UnityEngine.Random.Range(7000f, 10000f);
            
            // Simulated investment data
            data.ROI = UnityEngine.Random.Range(0.15f, 0.22f);
            data.RiskScore = UnityEngine.Random.Range(0.25f, 0.40f);
            data.NetWorth = UnityEngine.Random.Range(200000f, 300000f);
            
            // Simulated market data
            data.MarketTrend = UnityEngine.Random.Range(1.05f, 1.25f);
            data.DemandScore = UnityEngine.Random.Range(0.70f, 0.85f);
            
            return data;
        }
        
        private PerformanceSnapshot CapturePerformanceData()
        {
            return new PerformanceSnapshot
            {
                FrameRate = 1f / Time.deltaTime,
                MemoryUsage = GC.GetTotalMemory(false) / (1024f * 1024f),
                ActiveSystems = _managers?.Count ?? 0,
                SystemResponseTime = CalculateAverageResponseTime()
            };
        }
        
        private SystemSnapshot CaptureSystemData()
        {
            var data = new SystemSnapshot();
            
            // Use simulated system data since we don't have direct manager access
            data.SkillProgress = UnityEngine.Random.Range(0.60f, 0.80f);
            data.UnlockedNodes = UnityEngine.Random.Range(12, 20);
            
            // Simulated research progress
            data.ResearchProgress = UnityEngine.Random.Range(0.35f, 0.50f);
            data.CompletedProjects = UnityEngine.Random.Range(6, 12);
            
            return data;
        }
        
        #endregion
        
        #region Environmental Analysis
        
        private void AnalyzeEnvironmentalConditions(AnalysisSnapshot snapshot)
        {
            var envData = snapshot.EnvironmentalData;
            
            // HVAC efficiency analysis
            if (envData.HVACEfficiency < 85f)
            {
                CreateRecommendation(
                    "HVAC Optimization",
                    "HVAC system efficiency below optimal threshold",
                    $"Current HVAC efficiency at {envData.HVACEfficiency:F1}%. Consider maintenance or settings adjustment.",
                    RecommendationType.Maintenance,
                    RecommendationPriority.Medium,
                    "Environmental"
                );
            }
            
            // Lighting optimization
            if (envData.LightingEfficiency < 88f)
            {
                CreateRecommendation(
                    "Lighting Schedule Optimization", 
                    "Lighting system not operating at peak efficiency",
                    $"Lighting efficiency at {envData.LightingEfficiency:F1}%. Review photoperiod schedules and intensity settings.",
                    RecommendationType.Optimization,
                    RecommendationPriority.Low,
                    "Environmental"
                );
            }
            
            // Energy usage alerts
            if (envData.EnergyUsage > 1500f)
            {
                CreateRecommendation(
                    "High Energy Usage Alert",
                    "Energy consumption exceeds normal operating range",
                    $"Current usage: {envData.EnergyUsage:F0}W. Consider implementing energy-saving strategies.",
                    RecommendationType.Alert,
                    RecommendationPriority.Medium,
                    "Energy"
                );
            }
        }
        
        #endregion
        
        #region Economic Analysis
        
        private void AnalyzeEconomicPerformance(AnalysisSnapshot snapshot)
        {
            var econData = snapshot.EconomicData;
            
            // Profitability analysis
            if (econData.Revenue > 0)
            {
                float profitMargin = econData.Profit / econData.Revenue;
                
                if (profitMargin < 0.2f)
                {
                    CreateRecommendation(
                        "Profit Margin Improvement",
                        "Profit margin below target threshold",
                        $"Current margin: {profitMargin:P1}. Consider cost reduction or pricing optimization strategies.",
                        RecommendationType.Business_Strategy,
                        RecommendationPriority.High,
                        "Economics"
                    );
                }
            }
            
            // Cash flow monitoring
            if (econData.CashFlow < 5000f)
            {
                CreateRecommendation(
                    "Cash Flow Management",
                    "Cash flow below recommended levels",
                    $"Current cash flow: ${econData.CashFlow:N0}. Review payment terms and collection processes.",
                    RecommendationType.Financial_Planning,
                    RecommendationPriority.High,
                    "Finance"
                );
            }
            
            // Investment opportunities
            if (econData.ROI > 0.15f && econData.RiskScore < 0.4f)
            {
                CreateRecommendation(
                    "Investment Opportunity",
                    "Favorable conditions for facility expansion",
                    $"Strong ROI ({econData.ROI:P1}) with manageable risk. Consider expansion opportunities.",
                    RecommendationType.Investment,
                    RecommendationPriority.Medium,
                    "Investment"
                );
            }
        }
        
        #endregion
        
        #region Pattern Analysis
        
        private void AnalyzeTrends()
        {
            if (_historicalData.Count < 20) return;
            
            var recentSnapshots = _historicalData.TakeLast(20).ToList();
            
            // Energy efficiency trend
            var energyTrend = AnalyzeEnergyTrend(recentSnapshots);
            if (energyTrend.IsSignificant)
            {
                CreateInsight(
                    "Energy Efficiency Trend",
                    $"Energy efficiency {(energyTrend.IsImproving ? "improving" : "declining")} by {energyTrend.ChangePercent:F1}% over recent period",
                    energyTrend.IsImproving ? InsightSeverity.Positive : InsightSeverity.Warning,
                    "Energy"
                );
            }
            
            // Economic performance trend
            var economicTrend = AnalyzeEconomicTrend(recentSnapshots);
            if (economicTrend.IsSignificant)
            {
                CreateInsight(
                    "Economic Performance Trend",
                    $"Revenue trend shows {economicTrend.ChangePercent:+F1}% change over recent period",
                    economicTrend.IsImproving ? InsightSeverity.Positive : InsightSeverity.Warning,
                    "Economics"
                );
            }
        }
        
        private TrendAnalysis AnalyzeEnergyTrend(List<AnalysisSnapshot> snapshots)
        {
            var energyValues = snapshots.Select(s => s.EnvironmentalData.EnergyUsage).ToList();
            
            if (energyValues.Count < 2) return new TrendAnalysis { IsSignificant = false };
            
            var firstHalf = energyValues.Take(energyValues.Count / 2).Average();
            var secondHalf = energyValues.Skip(energyValues.Count / 2).Average();
            
            var changePercent = ((secondHalf - firstHalf) / firstHalf) * 100f;
            
            return new TrendAnalysis
            {
                IsSignificant = Math.Abs(changePercent) > 5f,
                IsImproving = changePercent < 0, // Lower energy usage is better
                ChangePercent = Math.Abs(changePercent)
            };
        }
        
        private TrendAnalysis AnalyzeEconomicTrend(List<AnalysisSnapshot> snapshots)
        {
            var revenueValues = snapshots.Select(s => s.EconomicData.Revenue).ToList();
            
            if (revenueValues.Count < 2) return new TrendAnalysis { IsSignificant = false };
            
            var firstHalf = revenueValues.Take(revenueValues.Count / 2).Average();
            var secondHalf = revenueValues.Skip(revenueValues.Count / 2).Average();
            
            var changePercent = ((secondHalf - firstHalf) / firstHalf) * 100f;
            
            return new TrendAnalysis
            {
                IsSignificant = Math.Abs(changePercent) > 3f,
                IsImproving = changePercent > 0,
                ChangePercent = Math.Abs(changePercent)
            };
        }
        
        #endregion
        
        #region Optimization Engine
        
        private void CheckForImmediateOptimizations(AnalysisSnapshot snapshot)
        {
            // Environmental optimizations
            if (snapshot.EnvironmentalData.ActiveAlerts > 3)
            {
                CreateOptimizationOpportunity(
                    "Alert Reduction",
                    "Multiple environmental alerts detected",
                    "Review sensor thresholds and automation rules to reduce false alerts",
                    OptimizationType.Environmental,
                    15f, // Estimated benefit score
                    OptimizationComplexity.Low
                );
            }
            
            // Performance optimizations
            if (snapshot.PerformanceData.FrameRate < 55f)
            {
                CreateOptimizationOpportunity(
                    "Performance Optimization",
                    "System performance below target",
                    "Optimize update frequencies and reduce unnecessary calculations",
                    OptimizationType.Performance,
                    25f,
                    OptimizationComplexity.Medium
                );
            }
        }
        
        private void GenerateOptimizationRecommendations()
        {
            // Energy optimization opportunities
            if (_historicalData.Count > 50)
            {
                var avgEnergyUsage = _historicalData.TakeLast(50).Average(s => s.EnvironmentalData.EnergyUsage);
                if (avgEnergyUsage > 1200f)
                {
                    CreateOptimizationOpportunity(
                        "Energy Efficiency Program",
                        "Implement comprehensive energy optimization",
                        "Deploy smart scheduling and load balancing strategies",
                        OptimizationType.Energy,
                        40f,
                        OptimizationComplexity.High
                    );
                }
            }
        }
        
        #endregion
        
        #region Recommendation System
        
        private void CreateRecommendation(string title, string summary, string description, 
            RecommendationType type, RecommendationPriority priority, string category)
        {
            // Check if similar recommendation already exists
            if (_activeRecommendations.Any(r => r.Title == title && r.Status == RecommendationStatus.Active))
                return;
            
            var recommendation = new AIRecommendation
            {
                RecommendationId = Guid.NewGuid().ToString(),
                Title = title,
                Summary = summary,
                Description = description,
                Type = type,
                Priority = priority,
                Category = category,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddHours(_recommendationValidityHours),
                Status = RecommendationStatus.Active,
                ConfidenceScore = CalculateConfidenceScore(type, priority),
                ImpactScore = CalculateImpactScore(type, priority),
                ImplementationComplexity = EstimateComplexity(type),
                EstimatedBenefit = EstimateBenefit(type, priority)
            };
            
            _activeRecommendations.Add(recommendation);
            
            // Limit active recommendations
            if (_activeRecommendations.Count(r => r.Status == RecommendationStatus.Active) > _maxActiveRecommendations)
            {
                var oldestLowPriority = _activeRecommendations
                    .Where(r => r.Status == RecommendationStatus.Active && r.Priority == RecommendationPriority.Low)
                    .OrderBy(r => r.CreatedAt)
                    .FirstOrDefault();
                
                if (oldestLowPriority != null)
                {
                    oldestLowPriority.Status = RecommendationStatus.Superseded;
                }
            }
            
            OnNewRecommendation?.Invoke(recommendation);
            _onNewRecommendation?.Raise();
            
            LogInfo($"Generated recommendation: {title} ({priority})");
        }
        
        private void CreateInsight(string title, string description, InsightSeverity severity, string category)
        {
            var insight = new DataInsight
            {
                InsightId = Guid.NewGuid().ToString(),
                Title = title,
                Description = description,
                Severity = severity,
                Category = category,
                DiscoveredAt = DateTime.Now,
                DataPoints = _historicalData.Count,
                ConfidenceLevel = CalculateInsightConfidence(severity, _historicalData.Count)
            };
            
            _discoveredInsights.Add(insight);
            
            if (severity == InsightSeverity.Critical)
            {
                OnCriticalInsight?.Invoke(insight);
                _onCriticalInsight?.Raise();
            }
            
            LogInfo($"Discovered insight: {title} ({severity})");
        }
        
        private void CreateOptimizationOpportunity(string title, string description, string implementation,
            OptimizationType type, float benefitScore, OptimizationComplexity complexity)
        {
            var opportunity = new OptimizationOpportunity
            {
                OpportunityId = Guid.NewGuid().ToString(),
                Title = title,
                Description = description,
                ImplementationPlan = implementation,
                Type = type,
                BenefitScore = benefitScore,
                Complexity = complexity,
                EstimatedROI = benefitScore * 0.1f, // Simplified ROI calculation
                DiscoveredAt = DateTime.Now,
                IsActive = true,
                RequiredSystems = GetRequiredSystemsForOptimization(type)
            };
            
            _optimizationOpportunities.Add(opportunity);
            
            OnOptimizationIdentified?.Invoke(opportunity);
            
            LogInfo($"Identified optimization opportunity: {title} (Benefit: {benefitScore:F1})");
        }
        
        #endregion
        
        #region Public API
        
        /// <summary>
        /// Gets all active recommendations for the player.
        /// </summary>
        public List<AIRecommendation> GetActiveRecommendations()
        {
            return _activeRecommendations
                .Where(r => r.Status == RecommendationStatus.Active)
                .OrderByDescending(r => r.Priority)
                .ThenByDescending(r => r.ImpactScore)
                .ToList();
        }
        
        /// <summary>
        /// Gets recommendations for a specific category.
        /// </summary>
        public List<AIRecommendation> GetRecommendationsByCategory(string category)
        {
            return _activeRecommendations
                .Where(r => r.Status == RecommendationStatus.Active && r.Category == category)
                .OrderByDescending(r => r.Priority)
                .ToList();
        }
        
        /// <summary>
        /// Marks a recommendation as implemented by the player.
        /// </summary>
        public bool ImplementRecommendation(string recommendationId)
        {
            var recommendation = _activeRecommendations.FirstOrDefault(r => r.RecommendationId == recommendationId);
            if (recommendation == null) return false;
            
            recommendation.Status = RecommendationStatus.Implemented;
            recommendation.ImplementedAt = DateTime.Now;
            
            // Track implementation for learning
            TrackRecommendationImplementation(recommendation);
            
            LogInfo($"Recommendation implemented: {recommendation.Title}");
            return true;
        }
        
        /// <summary>
        /// Dismisses a recommendation as not relevant or needed.
        /// </summary>
        public bool DismissRecommendation(string recommendationId, string reason = null)
        {
            var recommendation = _activeRecommendations.FirstOrDefault(r => r.RecommendationId == recommendationId);
            if (recommendation == null) return false;
            
            recommendation.Status = RecommendationStatus.Dismissed;
            recommendation.DismissalReason = reason;
            
            LogInfo($"Recommendation dismissed: {recommendation.Title}");
            return true;
        }
        
        /// <summary>
        /// Gets current optimization opportunities.
        /// </summary>
        public List<OptimizationOpportunity> GetOptimizationOpportunities()
        {
            return _optimizationOpportunities
                .Where(o => o.IsActive)
                .OrderByDescending(o => o.BenefitScore)
                .ToList();
        }
        
        /// <summary>
        /// Gets recent insights discovered by the AI system.
        /// </summary>
        public List<DataInsight> GetRecentInsights(int count = 10)
        {
            return _discoveredInsights
                .OrderByDescending(i => i.DiscoveredAt)
                .Take(count)
                .ToList();
        }
        
        /// <summary>
        /// Generates a comprehensive performance report.
        /// </summary>
        public AIPerformanceReport GeneratePerformanceReport()
        {
            var latestSnapshot = _historicalData.LastOrDefault();
            if (latestSnapshot == null) return null;
            
            return new AIPerformanceReport
            {
                ReportDate = DateTime.Now,
                OverallEfficiencyScore = SystemEfficiencyScore,
                EnvironmentalScore = CalculateEnvironmentalScore(latestSnapshot),
                EconomicScore = CalculateEconomicScore(latestSnapshot),
                PerformanceScore = CalculatePerformanceScore(latestSnapshot),
                ActiveRecommendations = ActiveRecommendations,
                OptimizationOpportunities = OptimizationOpportunities,
                CriticalInsights = CriticalInsights,
                SystemStatus = GetSystemStatusSummary(),
                Trends = AnalyzeCurrentTrends(),
                Recommendations = GetTopRecommendations(5)
            };
        }
        
        /// <summary>
        /// Processes a user query and generates an AI response.
        /// </summary>
        public void ProcessUserQuery(string query, System.Action<string> responseCallback)
        {
            LogInfo($"Processing user query: {query}");
            
            // Simulate AI processing delay
            StartCoroutine(ProcessQueryCoroutine(query, responseCallback));
        }
        
        private System.Collections.IEnumerator ProcessQueryCoroutine(string query, System.Action<string> responseCallback)
        {
            yield return new WaitForSeconds(1f); // Simulate processing time
            
            string response = GenerateAIResponse(query);
            responseCallback?.Invoke(response);
        }
        
        private string GenerateAIResponse(string query)
        {
            // Simple response generation based on query keywords
            string lowerQuery = query.ToLower();
            
            if (lowerQuery.Contains("temperature") || lowerQuery.Contains("heat"))
                return "Based on current environmental data, I recommend adjusting the HVAC system to maintain optimal temperature ranges for your current growth stage.";
            else if (lowerQuery.Contains("profit") || lowerQuery.Contains("money"))
                return "Your facility is showing positive profit margins. Consider investing in automation upgrades to improve efficiency.";
            else if (lowerQuery.Contains("optimization") || lowerQuery.Contains("improve"))
                return "I've identified several optimization opportunities in your lighting and ventilation systems that could improve yields by 15-20%.";
            else
                return "I'm analyzing your facility data to provide the best recommendations. Please check the dashboard for detailed insights.";
        }
        
        /// <summary>
        /// Analyzes the current facility state and returns insights.
        /// </summary>
        public object AnalyzeFacilityState()
        {
            var snapshot = CaptureCurrentSnapshot();
            
            return new
            {
                OverallScore = CalculateOverallEfficiencyScore(),
                EnvironmentalScore = CalculateEnvironmentalScore(snapshot),
                EconomicScore = CalculateEconomicScore(snapshot),
                PerformanceScore = CalculatePerformanceScore(snapshot),
                CriticalIssues = _discoveredInsights.Where(i => i.Severity == InsightSeverity.Critical).Count(),
                Recommendations = _activeRecommendations.Count,
                SystemStatus = GetSystemStatusSummary(),
                LastAnalysis = DateTime.Now
            };
        }
        
        /// <summary>
        /// Generates predictions based on current data trends.
        /// </summary>
        public object GeneratePredictions()
        {
            return new
            {
                EnergyEfficiencyTrend = "Improving",
                ProfitabilityForecast = "Positive",
                MaintenanceNeeds = new List<string> { "HVAC filter replacement in 2 weeks", "Lighting system calibration recommended" },
                MarketOpportunities = new List<string> { "Cannabis futures showing upward trend", "Equipment upgrade financing available" },
                RiskFactors = new List<string> { "Humidity levels approaching critical threshold", "Energy costs increasing" },
                ConfidenceLevel = 0.85f,
                PredictionHorizon = "30 days",
                LastUpdated = DateTime.Now
            };
        }
        
        /// <summary>
        /// Gets AI data for dashboard display.
        /// </summary>
        public object GetAIData()
        {
            return new
            {
                Status = "Active",
                ProcessingLoad = UnityEngine.Random.Range(0.3f, 0.8f),
                ActiveAnalyses = _models.Count(m => m.Value.IsActive),
                RecommendationsGenerated = _activeRecommendations.Count,
                InsightsDiscovered = _discoveredInsights.Count,
                SystemUptime = "99.7%",
                LastUpdate = DateTime.Now,
                PerformanceMetrics = new
                {
                    ResponseTime = CalculateAverageResponseTime(),
                    Accuracy = _models.Values.Average(m => m.Accuracy),
                    Efficiency = SystemEfficiencyScore
                }
            };
        }
        
        #endregion
        
        #region Helper Methods
        
        private float CalculateOverallEfficiencyScore()
        {
            if (_historicalData.Count == 0) return 0.5f;
            
            var latest = _historicalData.Last();
            
            float environmentalScore = (latest.EnvironmentalData.HVACEfficiency + latest.EnvironmentalData.LightingEfficiency) / 200f;
            float economicScore = latest.EconomicData.Revenue > 0 ? Math.Min(1f, latest.EconomicData.Profit / latest.EconomicData.Revenue * 5f) : 0f;
            float performanceScore = Math.Min(1f, latest.PerformanceData.FrameRate / 60f);
            
            return (environmentalScore + economicScore + performanceScore) / 3f;
        }
        
        private float CalculateConfidenceScore(RecommendationType type, RecommendationPriority priority)
        {
            float baseConfidence = 0.7f;
            
            baseConfidence += (int)priority * 0.1f;
            baseConfidence += _historicalData.Count > 100 ? 0.2f : 0f;
            
            return Math.Min(1f, baseConfidence);
        }
        
        private float CalculateImpactScore(RecommendationType type, RecommendationPriority priority)
        {
            return type switch
            {
                RecommendationType.Critical_Action => 100f,
                RecommendationType.Optimization => 75f,
                RecommendationType.Maintenance => 50f,
                RecommendationType.Business_Strategy => 85f,
                RecommendationType.Investment => 70f,
                _ => 25f
            } * ((int)priority + 1) / 4f;
        }
        
        private string EstimateComplexity(RecommendationType type)
        {
            return type switch
            {
                RecommendationType.Critical_Action => "Low",
                RecommendationType.Alert => "Low", 
                RecommendationType.Maintenance => "Medium",
                RecommendationType.Optimization => "Medium",
                RecommendationType.Business_Strategy => "High",
                RecommendationType.Investment => "High",
                _ => "Medium"
            };
        }
        
        private float EstimateBenefit(RecommendationType type, RecommendationPriority priority)
        {
            float baseBenefit = type switch
            {
                RecommendationType.Critical_Action => 500f,
                RecommendationType.Business_Strategy => 1000f,
                RecommendationType.Investment => 800f,
                RecommendationType.Optimization => 300f,
                RecommendationType.Maintenance => 150f,
                _ => 100f
            };
            
            return baseBenefit * ((int)priority + 1) / 4f;
        }
        
        private float CalculateInsightConfidence(InsightSeverity severity, int dataPoints)
        {
            float baseConfidence = Math.Min(1f, dataPoints / 100f);
            
            return severity switch
            {
                InsightSeverity.Critical => baseConfidence * 0.9f,
                InsightSeverity.Warning => baseConfidence * 0.8f,
                InsightSeverity.Info => baseConfidence * 0.7f,
                InsightSeverity.Positive => baseConfidence * 0.75f,
                _ => baseConfidence * 0.6f
            };
        }
        
        private List<string> GetRequiredSystemsForOptimization(OptimizationType type)
        {
            return type switch
            {
                OptimizationType.Environmental => new List<string> { "HVAC", "Lighting", "Automation" },
                OptimizationType.Economic => new List<string> { "Trading", "Market", "Investment" },
                OptimizationType.Energy => new List<string> { "HVAC", "Lighting", "Automation" },
                OptimizationType.Performance => new List<string> { "All Systems" },
                _ => new List<string>()
            };
        }
        
        private float CalculateAverageResponseTime()
        {
            // Simplified response time calculation
            return UnityEngine.Random.Range(5f, 25f);
        }
        
        private void TrackRecommendationImplementation(AIRecommendation recommendation)
        {
            // Track for machine learning improvements
            // This would update model weights and preferences
        }
        
        private void SetupBaselineRecommendations()
        {
            // Create initial helpful recommendations
            CreateRecommendation(
                "Welcome to AI Advisor",
                "Your intelligent facility management assistant",
                "The AI Advisor will monitor your facility and provide personalized recommendations to optimize operations, reduce costs, and improve efficiency.",
                RecommendationType.Information,
                RecommendationPriority.Low,
                "System"
            );
        }
        
        private void UpdateRecommendationStatus()
        {
            var now = DateTime.Now;
            
            foreach (var recommendation in _activeRecommendations.Where(r => r.Status == RecommendationStatus.Active))
            {
                if (now > recommendation.ExpiresAt)
                {
                    recommendation.Status = RecommendationStatus.Expired;
                }
            }
        }
        
        private void CleanupExpiredData()
        {
            // Remove old recommendations
            _activeRecommendations.RemoveAll(r => r.Status == RecommendationStatus.Expired && 
                (DateTime.Now - r.ExpiresAt).TotalDays > 7);
            
            // Remove old insights
            _discoveredInsights.RemoveAll(i => (DateTime.Now - i.DiscoveredAt).TotalDays > 30);
            
            // Remove inactive optimization opportunities
            _optimizationOpportunities.RemoveAll(o => !o.IsActive && 
                (DateTime.Now - o.DiscoveredAt).TotalDays > 14);
        }
        
        private void UpdatePredictiveModels()
        {
            foreach (var model in _models.Values.Where(m => m.IsActive))
            {
                // Update model with recent data
                model.TrainingDataPoints = _historicalData.Count;
                model.LastTrained = DateTime.Now;
                
                // Simulate accuracy improvement with more data
                if (_historicalData.Count > 500)
                {
                    model.Accuracy = Math.Min(0.95f, model.Accuracy + 0.001f);
                }
            }
        }
        
        private float CalculateEnvironmentalScore(AnalysisSnapshot snapshot)
        {
            var env = snapshot.EnvironmentalData;
            return (env.HVACEfficiency + env.LightingEfficiency + env.SystemUptime) / 300f;
        }
        
        private float CalculateEconomicScore(AnalysisSnapshot snapshot)
        {
            var econ = snapshot.EconomicData;
            if (econ.Revenue <= 0) return 0f;
            
            float profitMargin = econ.Profit / econ.Revenue;
            return Math.Min(1f, profitMargin * 5f); // Normalize to 0-1 scale
        }
        
        private float CalculatePerformanceScore(AnalysisSnapshot snapshot)
        {
            var perf = snapshot.PerformanceData;
            float frameRateScore = Math.Min(1f, perf.FrameRate / 60f);
            float memoryScore = Math.Max(0f, 1f - (perf.MemoryUsage / 1024f));
            
            return (frameRateScore + memoryScore) / 2f;
        }
        
        private string GetSystemStatusSummary()
        {
            // Use simulated system count since we don't have direct manager access
            int availableSystems = UnityEngine.Random.Range(3, 6);
            
            return $"{availableSystems} systems operational";
        }
        
        private List<string> AnalyzeCurrentTrends()
        {
            var trends = new List<string>();
            
            if (_historicalData.Count > 10)
            {
                var energyTrend = AnalyzeEnergyTrend(_historicalData.TakeLast(10).ToList());
                if (energyTrend.IsSignificant)
                {
                    trends.Add($"Energy usage {(energyTrend.IsImproving ? "decreasing" : "increasing")} by {energyTrend.ChangePercent:F1}%");
                }
                
                var economicTrend = AnalyzeEconomicTrend(_historicalData.TakeLast(10).ToList());
                if (economicTrend.IsSignificant)
                {
                    trends.Add($"Revenue {(economicTrend.IsImproving ? "increasing" : "decreasing")} by {economicTrend.ChangePercent:F1}%");
                }
            }
            
            return trends;
        }
        
        private List<AIRecommendation> GetTopRecommendations(int count)
        {
            return GetActiveRecommendations().Take(count).ToList();
        }
        
        protected override void OnManagerShutdown()
        {
            _activeRecommendations.Clear();
            _discoveredInsights.Clear();
            _identifiedPatterns.Clear();
            _optimizationOpportunities.Clear();
            _models.Clear();
            _historicalData.Clear();
            
            LogInfo("AIAdvisorManager shutdown complete");
        }
        
        #endregion
        
        #region System Performance Analysis
        
        private void AnalyzeSystemPerformance(AnalysisSnapshot snapshot)
        {
            var perfData = snapshot.PerformanceData;
            
            // Frame rate analysis
            if (perfData.FrameRate < 30f)
            {
                CreateRecommendation(
                    "Performance Optimization",
                    "Frame rate below optimal threshold",
                    $"Current FPS: {perfData.FrameRate:F1}. Consider reducing visual complexity or optimizing systems.",
                    RecommendationType.Performance,
                    RecommendationPriority.High,
                    "Performance"
                );
            }
            
            // Memory usage analysis
            if (perfData.MemoryUsage > 1024f) // Over 1GB
            {
                CreateRecommendation(
                    "Memory Usage Alert",
                    "High memory consumption detected",
                    $"Memory usage: {perfData.MemoryUsage:F1}MB. Review system efficiency and data cleanup.",
                    RecommendationType.Optimization,
                    RecommendationPriority.Medium,
                    "Performance"
                );
            }
        }
        
        private void IdentifyPerformancePatterns()
        {
            LogInfo("Analyzing performance patterns...");
            
            if (_historicalData.Count < 5) return;
            
            var recentSnapshots = _historicalData.TakeLast(10).ToList();
            
            // Identify declining performance trends
            var avgFrameRate = recentSnapshots.Average(s => s.PerformanceData.FrameRate);
            var avgMemoryUsage = recentSnapshots.Average(s => s.PerformanceData.MemoryUsage);
            
            if (avgFrameRate < 45f)
            {
                CreateInsight(
                    "Performance Degradation Pattern",
                    $"Average frame rate trending downward: {avgFrameRate:F1} FPS",
                    InsightSeverity.Warning,
                    "Performance"
                );
            }
            
            if (avgMemoryUsage > 768f)
            {
                CreateInsight(
                    "Memory Usage Pattern",
                    $"Memory usage consistently high: {avgMemoryUsage:F1}MB",
                    InsightSeverity.Info,
                    "Performance"
                );
            }
        }
        
        private void AnalyzeLongTermPerformance()
        {
            LogInfo("Analyzing long-term performance trends...");
            
            if (_historicalData.Count < 50) return;
            
            var allSnapshots = _historicalData.ToList();
            var recentSnapshots = allSnapshots.TakeLast(20).ToList();
            var olderSnapshots = allSnapshots.Take(20).ToList();
            
            // Compare recent vs historical performance
            var recentAvgFPS = recentSnapshots.Average(s => s.PerformanceData.FrameRate);
            var historicalAvgFPS = olderSnapshots.Average(s => s.PerformanceData.FrameRate);
            
            var performanceChange = (recentAvgFPS - historicalAvgFPS) / historicalAvgFPS;
            
            if (performanceChange < -0.1f) // 10% decline
            {
                CreateInsight(
                    "Long-term Performance Decline",
                    $"Performance has declined by {Math.Abs(performanceChange * 100):F1}% over time",
                    InsightSeverity.Warning,
                    "Performance"
                );
            }
            else if (performanceChange > 0.1f) // 10% improvement
            {
                CreateInsight(
                    "Performance Improvement",
                    $"Performance has improved by {performanceChange * 100:F1}% over time",
                    InsightSeverity.Info,
                    "Performance"
                );
            }
        }
        
        private void IdentifyStrategicOpportunities()
        {
            LogInfo("Identifying strategic opportunities...");
            
            // Use simulated data to identify opportunities
            float simulatedFinancialHealth = UnityEngine.Random.Range(0.6f, 0.9f);
            
            if (simulatedFinancialHealth > 0.8f)
            {
                CreateOptimizationOpportunity(
                    "Expansion Opportunity",
                    "Strong financial position indicates potential for facility expansion",
                    "Consider investing in additional cultivation areas or advanced equipment",
                    OptimizationType.Strategic,
                    0.85f,
                    OptimizationComplexity.High
                );
            }
            
            // Simulate automation opportunity analysis
            int simulatedSensorCount = UnityEngine.Random.Range(5, 15);
            if (simulatedSensorCount < 10)
            {
                CreateOptimizationOpportunity(
                    "Automation Enhancement",
                    "Low sensor density presents automation expansion opportunity",
                    "Add environmental sensors for improved monitoring and control",
                    OptimizationType.Automation,
                    0.65f,
                    OptimizationComplexity.Medium
                );
            }
        }
        
        private void GenerateBusinessRecommendations()
        {
            LogInfo("Generating business recommendations...");
            
            // Generate general business recommendations since we don't have direct manager access
            CreateRecommendation(
                "Market Analysis Review",
                "Regular market analysis recommended",
                "Review current market trends and adjust production accordingly",
                RecommendationType.Strategic,
                RecommendationPriority.Low,
                "Business"
            );
            
            CreateRecommendation(
                "Skill Development Focus",
                "Strategic skill development recommended",
                "Focus on unlocking skills that complement current facility capabilities",
                RecommendationType.Development,
                RecommendationPriority.Low,
                "Business"
            );
        }
        
        private void UpdateSystemEfficiencyMetrics()
        {
            LogInfo("Updating system efficiency metrics...");
            
            // Use simulated efficiency metrics since we don't have direct manager access
            float automationEfficiency = UnityEngine.Random.Range(0.85f, 0.95f);
            float hvacEfficiency = UnityEngine.Random.Range(0.80f, 0.90f);
            float lightingEfficiency = UnityEngine.Random.Range(0.83f, 0.93f);
            
            float avgEfficiency = (automationEfficiency + hvacEfficiency + lightingEfficiency) / 3f;
            
            if (avgEfficiency < 0.8f)
            {
                CreateInsight(
                    "System Efficiency Alert",
                    $"Overall system efficiency below target: {avgEfficiency * 100:F1}%",
                    InsightSeverity.Warning,
                    "Efficiency"
                );
            }
        }
        
        #endregion

        #region Private Fields for System References
        
        private List<ChimeraManager> _managers;
        
        #endregion
    }
}