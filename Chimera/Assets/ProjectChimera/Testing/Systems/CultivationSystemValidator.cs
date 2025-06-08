using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Data.Cultivation;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Data.Genetics;

namespace ProjectChimera.Testing.Systems
{
    /// <summary>
    /// Validates that all cultivation systems are properly configured and accessible.
    /// Performs pre-flight checks before running comprehensive tests.
    /// </summary>
    public class CultivationSystemValidator : MonoBehaviour
    {
        [Header("Validation Configuration")]
        [SerializeField] private bool _validateOnStart = true;
        [SerializeField] private bool _enableDetailedLogging = true;
        [SerializeField] private bool _showValidationGUI = true;
        
        [Header("Required Systems")]
        [SerializeField] private VPDManagementSO _vpdSystem;
        [SerializeField] private EnvironmentalAutomationSO _environmentalSystem;
        [SerializeField] private FertigationSystemSO _fertigationSystem;
        [SerializeField] private CultivationZoneSO _testZone;
        [SerializeField] private PlantStrainSO _testStrain;
        [SerializeField] private GenotypeDataSO _testGenotype;
        
        [Header("Validation Results")]
        [SerializeField] private ValidationResult _lastValidationResult;
        [SerializeField] private List<ValidationIssue> _validationIssues = new List<ValidationIssue>();
        
        private bool _validationComplete = false;
        private GUIStyle _headerStyle;
        private GUIStyle _passStyle;
        private GUIStyle _failStyle;
        
        public System.Action<ValidationResult> OnValidationComplete;
        
        private void Start()
        {
            if (_validateOnStart)
            {
                ValidateAllSystems();
            }
            
            // Initialize GUI styles
            InitializeGUIStyles();
        }
        
        public ValidationResult ValidateAllSystems()
        {
            LogMessage("=== STARTING CULTIVATION SYSTEM VALIDATION ===");
            
            _validationIssues.Clear();
            _lastValidationResult = new ValidationResult();
            _lastValidationResult.ValidationTime = System.DateTime.Now;
            
            // Core System Validation
            ValidateCoreManagers();
            
            // Cultivation Data Assets Validation
            ValidateCultivationAssets();
            
            // System Integration Validation
            ValidateSystemIntegration();
            
            // Scene Setup Validation
            ValidateSceneSetup();
            
            // Calculate final results
            CalculateFinalResults();
            
            _validationComplete = true;
            LogMessage($"=== VALIDATION COMPLETED - {_lastValidationResult.OverallStatus} ===");
            
            OnValidationComplete?.Invoke(_lastValidationResult);
            return _lastValidationResult;
        }
        
        private void ValidateCoreManagers()
        {
            LogMessage("--- Validating Core Managers ---");
            
            // GameManager
            var gameManager = FindAnyObjectByType<GameManager>();
            AddValidationCheck("GameManager Present", gameManager != null, 
                "GameManager found in scene", "GameManager missing from scene", 
                ValidationSeverity.Critical);
            
            if (gameManager != null)
            {
                AddValidationCheck("GameManager Initialized", gameManager.IsInitialized,
                    "GameManager is properly initialized", "GameManager not initialized");
            }
            
            // CultivationManager
            var cultivationManager = FindAnyObjectByType<CultivationManager>();
            AddValidationCheck("CultivationManager Present", cultivationManager != null,
                "CultivationManager found in scene", "CultivationManager missing from scene",
                ValidationSeverity.Critical);
            
            if (cultivationManager != null)
            {
                AddValidationCheck("CultivationManager Initialized", cultivationManager.IsInitialized,
                    "CultivationManager is properly initialized", "CultivationManager not initialized");
            }
            
            // TimeManager
            var timeManager = gameManager?.GetManager<TimeManager>();
            AddValidationCheck("TimeManager Available", timeManager != null,
                "TimeManager accessible through GameManager", "TimeManager not available");
            
            // DataManager
            var dataManager = gameManager?.GetManager<DataManager>();
            AddValidationCheck("DataManager Available", dataManager != null,
                "DataManager accessible through GameManager", "DataManager not available");
        }
        
        private void ValidateCultivationAssets()
        {
            LogMessage("--- Validating Cultivation Assets ---");
            
            // VPD Management System
            AddValidationCheck("VPD Management System", _vpdSystem != null,
                "VPD Management System asset assigned", "VPD Management System asset missing",
                ValidationSeverity.High);
            
            if (_vpdSystem != null)
            {
                // Test basic VPD calculation functionality
                try
                {
                    float testVPD = _vpdSystem.CalculateVPD(24f, 65f, -2f);
                    AddValidationCheck("VPD Calculation Function", testVPD > 0f,
                        $"VPD calculation working (test result: {testVPD:F3} kPa)", "VPD calculation failed");
                }
                catch (System.Exception e)
                {
                    AddValidationCheck("VPD Calculation Function", false,
                        "VPD calculation threw exception", $"Error: {e.Message}", ValidationSeverity.High);
                }
            }
            
            // Environmental Automation System
            AddValidationCheck("Environmental Automation System", _environmentalSystem != null,
                "Environmental Automation System asset assigned", "Environmental Automation System asset missing",
                ValidationSeverity.High);
            
            // Fertigation System
            AddValidationCheck("Fertigation System", _fertigationSystem != null,
                "Fertigation System asset assigned", "Fertigation System asset missing",
                ValidationSeverity.High);
            
            // Test Zone
            AddValidationCheck("Test Cultivation Zone", _testZone != null,
                "Test cultivation zone asset assigned", "Test cultivation zone asset missing",
                ValidationSeverity.Medium);
            
            // Test Strain
            AddValidationCheck("Test Plant Strain", _testStrain != null,
                "Test plant strain asset assigned", "Test plant strain asset missing",
                ValidationSeverity.Medium);
            
            // Test Genotype
            AddValidationCheck("Test Genotype", _testGenotype != null,
                "Test genotype asset assigned", "Test genotype asset missing",
                ValidationSeverity.Medium);
        }
        
        private void ValidateSystemIntegration()
        {
            LogMessage("--- Validating System Integration ---");
            
            // Check if all three main systems can work together
            bool hasAllSystems = _vpdSystem != null && _environmentalSystem != null && _fertigationSystem != null;
            AddValidationCheck("All Core Systems Available", hasAllSystems,
                "VPD, Environmental, and Fertigation systems all present", 
                "One or more core systems missing", ValidationSeverity.High);
            
            if (hasAllSystems)
            {
                // Test integration functionality
                try
                {
                    var testEnvironment = EnvironmentalConditions.CreateIndoorDefault();
                    float vpdTest = _vpdSystem.CalculateVPD(testEnvironment.Temperature, testEnvironment.Humidity, testEnvironment.LeafSurfaceTemperature);
                    
                    AddValidationCheck("System Integration Test", vpdTest > 0f,
                        "Systems can work with shared environmental data", "System integration failed");
                }
                catch (System.Exception e)
                {
                    AddValidationCheck("System Integration Test", false,
                        "System integration threw exception", $"Error: {e.Message}", ValidationSeverity.Medium);
                }
            }
            
            // Check for test components
            var cultivationTester = FindAnyObjectByType<AdvancedCultivationTester>();
            AddValidationCheck("Advanced Cultivation Tester", cultivationTester != null,
                "AdvancedCultivationTester component found", "AdvancedCultivationTester component missing");
            
            var testRunner = FindAnyObjectByType<AdvancedCultivationTestRunner>();
            AddValidationCheck("Test Runner", testRunner != null,
                "AdvancedCultivationTestRunner component found", "AdvancedCultivationTestRunner component missing");
        }
        
        private void ValidateSceneSetup()
        {
            LogMessage("--- Validating Scene Setup ---");
            
            // Check for main camera
            var mainCamera = Camera.main;
            AddValidationCheck("Main Camera", mainCamera != null,
                "Main camera present in scene", "Main camera missing from scene");
            
            // Check for testing objects structure
            var testingObjects = GameObject.Find("Testing");
            if (testingObjects == null)
            {
                // Try to find any testing-related objects
                var testers = FindObjectsOfType<MonoBehaviour>();
                bool hasTesters = false;
                foreach (var tester in testers)
                {
                    if (tester.GetType().Name.Contains("Test"))
                    {
                        hasTesters = true;
                        break;
                    }
                }
                AddValidationCheck("Testing Objects", hasTesters,
                    "Testing components found in scene", "No testing components found");
            }
            else
            {
                AddValidationCheck("Testing Objects", true,
                    "Testing objects hierarchy found", "");
            }
            
            // Check Unity version
            bool unityVersionOK = Application.unityVersion.StartsWith("6000.");
            AddValidationCheck("Unity Version", unityVersionOK,
                $"Unity 6 detected ({Application.unityVersion})", 
                $"Unity 6 required, found {Application.unityVersion}", ValidationSeverity.Medium);
        }
        
        private void CalculateFinalResults()
        {
            int totalChecks = _validationIssues.Count;
            int passedChecks = 0;
            int criticalIssues = 0;
            int highIssues = 0;
            int mediumIssues = 0;
            int lowIssues = 0;
            
            foreach (var issue in _validationIssues)
            {
                if (issue.Passed)
                {
                    passedChecks++;
                }
                else
                {
                    switch (issue.Severity)
                    {
                        case ValidationSeverity.Critical: criticalIssues++; break;
                        case ValidationSeverity.High: highIssues++; break;
                        case ValidationSeverity.Medium: mediumIssues++; break;
                        case ValidationSeverity.Low: lowIssues++; break;
                    }
                }
            }
            
            _lastValidationResult.TotalChecks = totalChecks;
            _lastValidationResult.PassedChecks = passedChecks;
            _lastValidationResult.FailedChecks = totalChecks - passedChecks;
            _lastValidationResult.CriticalIssues = criticalIssues;
            _lastValidationResult.HighIssues = highIssues;
            _lastValidationResult.MediumIssues = mediumIssues;
            _lastValidationResult.LowIssues = lowIssues;
            
            // Determine overall status
            if (criticalIssues > 0)
            {
                _lastValidationResult.OverallStatus = ValidationStatus.Failed;
            }
            else if (highIssues > 0)
            {
                _lastValidationResult.OverallStatus = ValidationStatus.Warning;
            }
            else if (mediumIssues > 0)
            {
                _lastValidationResult.OverallStatus = ValidationStatus.Minor;
            }
            else
            {
                _lastValidationResult.OverallStatus = ValidationStatus.Passed;
            }
            
            float successRate = totalChecks > 0 ? (float)passedChecks / totalChecks * 100f : 0f;
            _lastValidationResult.SuccessRate = successRate;
            
            LogMessage($"Validation Summary: {passedChecks}/{totalChecks} checks passed ({successRate:F1}%)");
            LogMessage($"Issues: {criticalIssues} Critical, {highIssues} High, {mediumIssues} Medium, {lowIssues} Low");
        }
        
        private void AddValidationCheck(string checkName, bool passed, string successMessage, string failureMessage, ValidationSeverity severity = ValidationSeverity.Low)
        {
            var issue = new ValidationIssue
            {
                CheckName = checkName,
                Passed = passed,
                Message = passed ? successMessage : failureMessage,
                Severity = severity
            };
            
            _validationIssues.Add(issue);
            
            string statusIcon = passed ? "✓" : "✗";
            string severityTag = passed ? "" : $" [{severity}]";
            LogMessage($"{statusIcon} {checkName}: {issue.Message}{severityTag}");
        }
        
        private void LogMessage(string message)
        {
            if (_enableDetailedLogging)
            {
                Debug.Log($"[CultivationValidator] {message}");
            }
        }
        
        private void InitializeGUIStyles()
        {
            _headerStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
            
            _passStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.green }
            };
            
            _failStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.red }
            };
        }
        
        private void OnGUI()
        {
            if (!_showValidationGUI || !_validationComplete) return;
            
            GUILayout.BeginArea(new Rect(Screen.width - 420, 10, 400, 300));
            GUILayout.BeginVertical("box");
            
            GUILayout.Label("Cultivation System Validation", _headerStyle);
            
            if (_lastValidationResult != null)
            {
                GUILayout.Label($"Status: {_lastValidationResult.OverallStatus}");
                GUILayout.Label($"Success Rate: {_lastValidationResult.SuccessRate:F1}%");
                GUILayout.Label($"Checks: {_lastValidationResult.PassedChecks}/{_lastValidationResult.TotalChecks}");
                
                if (_lastValidationResult.FailedChecks > 0)
                {
                    GUILayout.Label($"Issues: {_lastValidationResult.CriticalIssues}C {_lastValidationResult.HighIssues}H {_lastValidationResult.MediumIssues}M {_lastValidationResult.LowIssues}L");
                }
            }
            
            if (GUILayout.Button("Re-run Validation"))
            {
                ValidateAllSystems();
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
        
        [ContextMenu("Validate Systems")]
        public void ValidateSystemsManual()
        {
            ValidateAllSystems();
        }
        
        [ContextMenu("Clear Validation Results")]
        public void ClearValidationResults()
        {
            _validationIssues.Clear();
            _lastValidationResult = null;
            _validationComplete = false;
            LogMessage("Validation results cleared.");
        }
        
        // Public properties
        public ValidationResult LastValidationResult => _lastValidationResult;
        public List<ValidationIssue> ValidationIssues => _validationIssues;
        public bool ValidationComplete => _validationComplete;
    }
    
    // Data structures for validation
    [System.Serializable]
    public class ValidationResult
    {
        public System.DateTime ValidationTime;
        public System.DateTime StartTime;
        public System.DateTime EndTime;
        public ValidationStatus OverallStatus;
        public int TotalChecks;
        public int PassedChecks;
        public int FailedChecks;
        public float SuccessRate;
        public int CriticalIssues;
        public int HighIssues;
        public int MediumIssues;
        public int LowIssues;
    }
    
    [System.Serializable]
    public class ValidationIssue
    {
        public string CheckName;
        public bool Passed;
        public string Message;
        public ValidationSeverity Severity;
    }
    
    public enum ValidationStatus
    {
        Passed,
        Minor,
        Warning,
        Failed
    }
    
    public enum ValidationSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }
} 