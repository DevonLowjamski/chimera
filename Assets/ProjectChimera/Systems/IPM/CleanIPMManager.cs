using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Data.IPM;

namespace ProjectChimera.Systems.IPM
{
    /// <summary>
    /// Clean, minimal IPM manager that eliminates circular dependencies
    /// Uses only shared types from ProjectChimera.Data.IPM namespace
    /// </summary>
    public class CleanIPMManager : ChimeraManager
    {
        [Header("Clean IPM Configuration")]
        [SerializeField] private bool _enablePestDetection = true;
        [SerializeField] private bool _enableTreatments = true;
        [SerializeField] private bool _enableAnalytics = true;
        [SerializeField] private bool _enableAutomation = false;
        
        // Clean data collections
        private List<CleanIPMPestData> _detectedPests = new List<CleanIPMPestData>();
        private List<CleanIPMTreatment> _availableTreatments = new List<CleanIPMTreatment>();
        private List<CleanIPMBattleResult> _battleHistory = new List<CleanIPMBattleResult>();
        private CleanIPMConfiguration _ipmConfiguration = new CleanIPMConfiguration();
        private CleanIPMAnalytics _analytics = new CleanIPMAnalytics();
        
        // Simple state tracking
        private bool _isInitialized = false;
        private float _totalPestsDetected = 0f;
        private float _totalTreatmentsApplied = 0f;
        private float _currentThreatLevel = 0f;
        
        #region Manager Lifecycle
        
        public override string ManagerName => "Clean IPM Manager";
        
        protected override void OnManagerInitialize()
        {
            if (_isInitialized)
            {
                Debug.LogWarning("CleanIPMManager already initialized");
                return;
            }
            
            InitializeBasicSystems();
            _isInitialized = true;
            
            Debug.Log("CleanIPMManager initialized successfully");
        }
        
        protected override void OnManagerShutdown()
        {
            _isInitialized = false;
            _detectedPests.Clear();
            _availableTreatments.Clear();
            _battleHistory.Clear();
            
            Debug.Log("CleanIPMManager shutdown successfully");
        }
        
        private void InitializeBasicSystems()
        {
            // Initialize with minimal data
            if (_enableTreatments)
            {
                CreateSampleTreatments();
            }
            
            if (_enablePestDetection)
            {
                _ipmConfiguration.EnableAutomaticDetection = true;
                _ipmConfiguration.DetectionSensitivity = 0.5f;
            }
        }
        
        #endregion
        
        #region Public API
        
        public bool IsSystemReady => _isInitialized;
        
        public float GetTotalPestsDetected() => _totalPestsDetected;
        public float GetTotalTreatmentsApplied() => _totalTreatmentsApplied;
        public float GetCurrentThreatLevel() => _currentThreatLevel;
        public float GetSuccessRate() => _analytics.SuccessRate;
        
        public List<CleanIPMPestData> GetDetectedPests() => new List<CleanIPMPestData>(_detectedPests);
        public List<CleanIPMTreatment> GetAvailableTreatments() => new List<CleanIPMTreatment>(_availableTreatments);
        public List<CleanIPMBattleResult> GetBattleHistory() => new List<CleanIPMBattleResult>(_battleHistory);
        public CleanIPMConfiguration GetConfiguration() => _ipmConfiguration;
        
        #endregion
        
        #region Pest Detection System
        
        public bool DetectPest(string pestName, IPMPestType pestType, IPMSeverityLevel severity)
        {
            if (!_isInitialized || !_enablePestDetection)
                return false;
            
            var pest = new CleanIPMPestData
            {
                PestID = System.Guid.NewGuid().ToString(),
                PestName = pestName,
                PestType = pestType,
                SeverityLevel = severity,
                IsActive = true,
                DetectionDate = System.DateTime.Now,
                DamageRate = UnityEngine.Random.Range(0.1f, 0.8f)
            };
            
            _detectedPests.Add(pest);
            _totalPestsDetected++;
            UpdateThreatLevel();
            
            Debug.Log($"Pest detected: {pestName} (Severity: {severity})");
            return true;
        }
        
        public void UpdateThreatLevel()
        {
            if (_detectedPests.Count == 0)
            {
                _currentThreatLevel = 0f;
                return;
            }
            
            float totalThreat = 0f;
            foreach (var pest in _detectedPests)
            {
                if (pest.IsActive)
                {
                    float severityMultiplier = (int)pest.SeverityLevel / 3f;
                    totalThreat += pest.DamageRate * severityMultiplier;
                }
            }
            
            _currentThreatLevel = Mathf.Clamp01(totalThreat / _detectedPests.Count);
        }
        
        #endregion
        
        #region Treatment System
        
        public bool ApplyTreatment(string pestID, string treatmentID)
        {
            if (!_isInitialized || !_enableTreatments)
                return false;
            
            var pest = _detectedPests.Find(p => p.PestID == pestID);
            var treatment = _availableTreatments.Find(t => t.TreatmentID == treatmentID);
            
            if (pest != null && treatment != null)
            {
                float effectiveness = CalculateTreatmentEffectiveness(pest, treatment);
                bool success = effectiveness > 0.5f;
                
                var battle = new CleanIPMBattleResult
                {
                    BattleID = System.Guid.NewGuid().ToString(),
                    PestID = pestID,
                    TreatmentID = treatmentID,
                    Success = success,
                    EffectivenessScore = effectiveness,
                    BattleDate = System.DateTime.Now
                };
                
                _battleHistory.Add(battle);
                _totalTreatmentsApplied++;
                
                if (success)
                {
                    pest.IsActive = false;
                    pest.SeverityLevel = IPMSeverityLevel.Low;
                    UpdateThreatLevel();
                }
                
                UpdateAnalytics();
                
                Debug.Log($"Treatment {(success ? "successful" : "failed")}: {treatment.TreatmentName} vs {pest.PestName}");
                return success;
            }
            
            return false;
        }
        
        private float CalculateTreatmentEffectiveness(CleanIPMPestData pest, CleanIPMTreatment treatment)
        {
            float baseEffectiveness = treatment.Effectiveness;
            
            // Simple effectiveness calculation
            if (treatment.TargetPests.Contains(pest.PestName) || treatment.TargetPests.Contains(pest.PestType.ToString()))
            {
                baseEffectiveness *= 1.5f;
            }
            
            // Severity affects treatment difficulty
            float severityPenalty = (int)pest.SeverityLevel * 0.1f;
            baseEffectiveness -= severityPenalty;
            
            return Mathf.Clamp01(baseEffectiveness + UnityEngine.Random.Range(-0.2f, 0.2f));
        }
        
        private void CreateSampleTreatments()
        {
            var treatment1 = new CleanIPMTreatment
            {
                TreatmentID = System.Guid.NewGuid().ToString(),
                TreatmentName = "Beneficial Insects",
                TreatmentType = IPMTreatmentType.Biological,
                Effectiveness = 0.7f,
                Cost = 50f,
                IsOrganic = true
            };
            treatment1.TargetPests.Add("Aphids");
            treatment1.TargetPests.Add("Insect");
            
            var treatment2 = new CleanIPMTreatment
            {
                TreatmentID = System.Guid.NewGuid().ToString(),
                TreatmentName = "Neem Oil Spray",
                TreatmentType = IPMTreatmentType.Biological,
                Effectiveness = 0.6f,
                Cost = 25f,
                IsOrganic = true
            };
            treatment2.TargetPests.Add("Spider Mites");
            treatment2.TargetPests.Add("Mite");
            
            _availableTreatments.Add(treatment1);
            _availableTreatments.Add(treatment2);
        }
        
        #endregion
        
        #region Analytics System
        
        private void UpdateAnalytics()
        {
            if (_battleHistory.Count == 0)
            {
                _analytics.SuccessRate = 0f;
                return;
            }
            
            int successfulBattles = 0;
            foreach (var battle in _battleHistory)
            {
                if (battle.Success)
                    successfulBattles++;
            }
            
            _analytics.SuccessRate = (float)successfulBattles / _battleHistory.Count;
            _analytics.TotalPestsDetected = _totalPestsDetected;
            _analytics.TotalTreatmentsApplied = _totalTreatmentsApplied;
        }
        
        #endregion
        
        #region Debug and Testing
        
        [ContextMenu("Test Pest Detection")]
        private void TestPestDetection()
        {
            DetectPest("Test Aphids", IPMPestType.Insect, IPMSeverityLevel.Medium);
        }
        
        [ContextMenu("Test Treatment")]
        private void TestTreatment()
        {
            if (_detectedPests.Count > 0 && _availableTreatments.Count > 0)
            {
                ApplyTreatment(_detectedPests[0].PestID, _availableTreatments[0].TreatmentID);
            }
        }
        
        [ContextMenu("Print Status")]
        private void PrintStatus()
        {
            Debug.Log($"IPM Manager Status: Initialized={_isInitialized}, " +
                     $"Pests={_totalPestsDetected}, Treatments={_totalTreatmentsApplied}, " +
                     $"Threat={_currentThreatLevel:F2}, Success={_analytics.SuccessRate:F2}");
        }
        
        #endregion
    }
}