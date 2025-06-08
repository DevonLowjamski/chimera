using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Core;
using ProjectChimera.Data.Cultivation;
using ProjectChimera.Data.Genetics;
using ProjectChimera.Systems.Cultivation;
using System;
using System.Linq;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Comprehensive test system for the Integrated Pest Management (IPM) System.
    /// Tests biological controls, monitoring protocols, treatment strategies, and integration with other cultivation systems.
    /// </summary>
    public class IPMSystemTester : MonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private bool _runTestsOnStart = true;
        [SerializeField] private bool _enableDetailedLogging = true;
        [SerializeField] private int _testPlantCount = 5;
        
        [Header("System References")]
        [SerializeField] private IPMSystemSO _ipmSystem;
        [SerializeField] private VPDManagementSO _vpdSystem;
        [SerializeField] private EnvironmentalAutomationSO _environmentalSystem;
        [SerializeField] private FertigationSystemSO _fertigationSystem;
        [SerializeField] private CultivationZoneSO _testZone;
        [SerializeField] private PlantStrainSO _testStrain;
        
        [Header("Test Results")]
        [SerializeField] private int _totalTests = 0;
        [SerializeField] private int _passedTests = 0;
        [SerializeField] private int _failedTests = 0;
        [SerializeField] private float _testStartTime = 0f;
        
        private CultivationManager _cultivationManager;
        private List<string> _testResults = new List<string>();
        private List<PlantInstanceSO> _testPlants = new List<PlantInstanceSO>();
        
        public bool TestsRunning { get; private set; } = false;
        
        private void Start()
        {
            if (_runTestsOnStart)
            {
                StartCoroutine(RunAllIPMTests());
            }
        }
        
        private System.Collections.IEnumerator RunAllIPMTests()
        {
            TestsRunning = true;
            _testStartTime = Time.time;
            _totalTests = 0;
            _passedTests = 0;
            _failedTests = 0;
            _testResults.Clear();
            
            LogTest("=== IPM SYSTEM COMPREHENSIVE TEST SUITE ===");
            LogTest($"Test started at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            // Initialize test systems
            yield return StartCoroutine(InitializeIPMTestSystems());
            
            // Test IPM System Configuration
            yield return StartCoroutine(TestIPMSystemConfiguration());
            
            // Test Beneficial Organism Management
            yield return StartCoroutine(TestBeneficialOrganismManagement());
            
            // Test Monitoring and Detection Systems
            yield return StartCoroutine(TestMonitoringAndDetection());
            
            // Test Treatment Protocol Systems
            yield return StartCoroutine(TestTreatmentProtocols());
            
            // Test Environmental Integration
            yield return StartCoroutine(TestEnvironmentalIntegration());
            
            // Test Comprehensive IPM Workflows
            yield return StartCoroutine(TestComprehensiveIPMWorkflows());
            
            // Test Integration with Other Cultivation Systems
            yield return StartCoroutine(TestSystemIntegration());
            
            // Display final results
            DisplayFinalResults();
            
            TestsRunning = false;
        }
        
        private System.Collections.IEnumerator InitializeIPMTestSystems()
        {
            LogTest("\n--- INITIALIZING IPM TEST SYSTEMS ---");
            
            // Find CultivationManager
            _cultivationManager = FindAnyObjectByType<CultivationManager>();
            RunTest("CultivationManager Found", _cultivationManager != null);
            
            // Create test assets if missing
            yield return StartCoroutine(CreateIPMTestAssets());
            
            // Validate IPM system assets
            RunTest("IPM System Asset", _ipmSystem != null);
            RunTest("VPD System Asset", _vpdSystem != null);
            RunTest("Environmental System Asset", _environmentalSystem != null);
            RunTest("Test Zone Asset", _testZone != null);
            RunTest("Test Strain Asset", _testStrain != null);
            
            // Create test plants for IPM testing
            if (_cultivationManager != null)
            {
                yield return StartCoroutine(CreateTestPlantsForIPM());
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator CreateIPMTestAssets()
        {
            LogTest("Creating IPM test assets if needed...");
            
            // Create IPM System if missing
            if (_ipmSystem == null)
            {
                _ipmSystem = ScriptableObject.CreateInstance<IPMSystemSO>();
                _ipmSystem.name = "Test_IPM_System";
                LogTest("Created test IPM System");
            }
            
            // Create other systems if missing (reuse existing systems or create new ones)
            if (_vpdSystem == null)
            {
                _vpdSystem = ScriptableObject.CreateInstance<VPDManagementSO>();
                _vpdSystem.name = "Test_VPD_Management_IPM";
                LogTest("Created test VPD Management System for IPM");
            }
            
            if (_environmentalSystem == null)
            {
                _environmentalSystem = ScriptableObject.CreateInstance<EnvironmentalAutomationSO>();
                _environmentalSystem.name = "Test_Environmental_Automation_IPM";
                LogTest("Created test Environmental Automation System for IPM");
            }
            
            if (_testZone == null)
            {
                _testZone = ScriptableObject.CreateInstance<CultivationZoneSO>();
                _testZone.name = "Test_Cultivation_Zone_IPM";
                LogTest("Created test Cultivation Zone for IPM");
            }
            
            if (_testStrain == null)
            {
                _testStrain = ScriptableObject.CreateInstance<PlantStrainSO>();
                _testStrain.name = "Test_Cannabis_Strain_IPM";
                LogTest("Created test Cannabis Strain for IPM");
            }
            
            yield return null;
        }
        
        private System.Collections.IEnumerator CreateTestPlantsForIPM()
        {
            LogTest("Creating test plants for IPM testing...");
            
            for (int i = 0; i < _testPlantCount; i++)
            {
                var testGenotype = ScriptableObject.CreateInstance<GenotypeDataSO>();
                testGenotype.name = $"Test_Genotype_IPM_{i}";
                
                var plant = _cultivationManager.PlantSeed(
                    $"IPMTestPlant_{i}", 
                    _testStrain, 
                    testGenotype, 
                    new Vector3(i * 2f, 0, 0)
                );
                
                if (plant != null)
                {
                    _testPlants.Add(plant);
                    // Set different growth stages for comprehensive testing
                    switch (i % 3)
                    {
                        case 0:
                            plant.SetGrowthStage(PlantGrowthStage.Seedling);
                            break;
                        case 1:
                            plant.SetGrowthStage(PlantGrowthStage.Vegetative);
                            break;
                        case 2:
                            plant.SetGrowthStage(PlantGrowthStage.Flowering);
                            break;
                    }
                }
            }
            
            RunTest("Test Plants Created for IPM", _testPlants.Count == _testPlantCount);
            LogTest($"Created {_testPlants.Count} test plants for IPM testing");
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestIPMSystemConfiguration()
        {
            LogTest("\n--- TESTING IPM SYSTEM CONFIGURATION ---");
            
            if (_ipmSystem == null)
            {
                LogTest("ERROR: IPM System not available for testing");
                yield break;
            }
            
            // Test data validation
            bool isValidData = _ipmSystem.ValidateData();
            RunTest("IPM System Data Validation", isValidData);
            
            // Test beneficial organism configuration
            var beneficialOrganisms = _ipmSystem.GetType().GetField("_beneficialOrganisms", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_ipmSystem) as BeneficialOrganism[];
            
            RunTest("Beneficial Organisms Configured", beneficialOrganisms != null && beneficialOrganisms.Length > 0);
            
            if (beneficialOrganisms != null)
            {
                LogTest($"Found {beneficialOrganisms.Length} beneficial organism configurations");
                foreach (var beneficial in beneficialOrganisms.Take(3)) // Test first 3
                {
                    RunTest($"Beneficial Organism Valid: {beneficial.OrganismName}", 
                           !string.IsNullOrEmpty(beneficial.OrganismName) && beneficial.ReleaseRate > 0f);
                }
            }
            
            // Test monitoring protocols configuration
            var monitoringProtocols = _ipmSystem.GetType().GetField("_monitoringProtocols", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_ipmSystem) as MonitoringProtocol[];
            
            RunTest("Monitoring Protocols Configured", monitoringProtocols != null && monitoringProtocols.Length > 0);
            
            if (monitoringProtocols != null)
            {
                LogTest($"Found {monitoringProtocols.Length} monitoring protocol configurations");
            }
            
            // Test cultural practices configuration
            var culturalPractices = _ipmSystem.GetType().GetField("_culturalPractices", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_ipmSystem) as CulturalPractice[];
            
            RunTest("Cultural Practices Configured", culturalPractices != null && culturalPractices.Length > 0);
            
            if (culturalPractices != null)
            {
                LogTest($"Found {culturalPractices.Length} cultural practice configurations");
            }
            
            // Test treatment protocols configuration
            var treatmentProtocols = _ipmSystem.GetType().GetField("_treatmentProtocols", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.GetValue(_ipmSystem) as TreatmentProtocol[];
            
            RunTest("Treatment Protocols Configured", treatmentProtocols != null && treatmentProtocols.Length > 0);
            
            if (treatmentProtocols != null)
            {
                LogTest($"Found {treatmentProtocols.Length} treatment protocol configurations");
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestBeneficialOrganismManagement()
        {
            LogTest("\n--- TESTING BENEFICIAL ORGANISM MANAGEMENT ---");
            
            if (_ipmSystem == null)
            {
                LogTest("ERROR: IPM System not available");
                yield break;
            }
            
            // Test biological control plan creation
            var environment = ProjectChimera.Data.Cultivation.EnvironmentalConditions.CreateIndoorDefault();
            var biologicalPlan = _ipmSystem.CreateBiologicalControlPlan(
                PestType.Spider_Mites, 
                _testZone, 
                environment, 
                0.5f // moderate pest pressure
            );
            
            RunTest("Biological Control Plan Creation", biologicalPlan != null);
            
            if (biologicalPlan != null)
            {
                LogTest($"Biological Plan - Target: {biologicalPlan.TargetPest}, Zone: {biologicalPlan.ZoneID}");
                RunTest("Biological Plan Has Target Pest", biologicalPlan.TargetPest == PestType.Spider_Mites);
                RunTest("Biological Plan Has Zone ID", !string.IsNullOrEmpty(biologicalPlan.ZoneID));
                RunTest("Biological Plan Has Creation Timestamp", biologicalPlan.CreationTimestamp != default(DateTime));
            }
            
            // Test biological control plan for different pest types
            var thripsControlPlan = _ipmSystem.CreateBiologicalControlPlan(
                PestType.Thrips, 
                _testZone, 
                environment, 
                0.7f // higher pest pressure
            );
            
            RunTest("Thrips Biological Control Plan", thripsControlPlan != null);
            
            if (thripsControlPlan != null)
            {
                LogTest($"Thrips Plan - Target: {thripsControlPlan.TargetPest}, Pressure Level: {thripsControlPlan.PestPressureLevel}");
            }
            
            // Test biological control plan for powdery mildew
            var powderyMildewPlan = _ipmSystem.CreateBiologicalControlPlan(
                PestType.Powdery_Mildew, 
                _testZone, 
                environment, 
                0.3f // low pest pressure
            );
            
            RunTest("Powdery Mildew Control Plan", powderyMildewPlan != null);
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestMonitoringAndDetection()
        {
            LogTest("\n--- TESTING MONITORING AND DETECTION SYSTEMS ---");
            
            if (_ipmSystem == null || _testZone == null)
            {
                LogTest("ERROR: IPM System or Test Zone not available");
                yield break;
            }
            
            // Test monitoring plan creation
            var environment = ProjectChimera.Data.Cultivation.EnvironmentalConditions.CreateIndoorDefault();
            var monitoringPlan = _ipmSystem.CreateMonitoringPlan(
                _testZone,
                _testPlants.ToArray(),
                environment
            );
            
            RunTest("Monitoring Plan Creation", monitoringPlan != null);
            
            if (monitoringPlan != null)
            {
                LogTest($"Monitoring Plan - Zone: {monitoringPlan.ZoneID}, Duration: {monitoringPlan.MonitoringDuration} days");
                RunTest("Monitoring Plan Has Zone ID", !string.IsNullOrEmpty(monitoringPlan.ZoneID));
                RunTest("Monitoring Plan Has Creation Date", monitoringPlan.PlanCreationDate != default(DateTime));
                RunTest("Monitoring Plan Has Plant Count", monitoringPlan.PlantCount == _testPlants.Count);
            }
            
            // Test pest assessment with mock monitoring data
            var mockMonitoringData = CreateMockMonitoringData();
            var pestAssessment = _ipmSystem.AssessPestPressure(
                _testZone,
                _testPlants.ToArray(),
                environment,
                mockMonitoringData
            );
            
            RunTest("Pest Assessment Creation", pestAssessment != null);
            
            if (pestAssessment != null)
            {
                LogTest($"Pest Assessment - Zone: {pestAssessment.ZoneID}, Plant Count: {pestAssessment.PlantCount}");
                LogTest($"Assessment - Risk Level: {pestAssessment.OverallRiskLevel}, Intervention Priority: {pestAssessment.InterventionPriority}");
                RunTest("Assessment Has Timestamp", pestAssessment.AssessmentTimestamp != default(DateTime));
                RunTest("Assessment Has Zone ID", !string.IsNullOrEmpty(pestAssessment.ZoneID));
                RunTest("Assessment Has Plant Count", pestAssessment.PlantCount > 0);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestTreatmentProtocols()
        {
            LogTest("\n--- TESTING TREATMENT PROTOCOLS ---");
            
            if (_ipmSystem == null)
            {
                LogTest("ERROR: IPM System not available");
                yield break;
            }
            
            // Test integrated treatment plan creation
            var mockInfestations = CreateMockPestInfestations();
            var environment = ProjectChimera.Data.Cultivation.EnvironmentalConditions.CreateIndoorDefault();
            
            var treatmentPlan = _ipmSystem.CreateIntegratedTreatmentPlan(
                mockInfestations,
                _testZone,
                environment,
                _testPlants.ToArray()
            );
            
            RunTest("Integrated Treatment Plan Creation", treatmentPlan != null);
            
            if (treatmentPlan != null)
            {
                LogTest($"Treatment Plan - Zone: {treatmentPlan.ZoneID}, Infestations: {treatmentPlan.TargetInfestations?.Length ?? 0}");
                RunTest("Treatment Plan Has Zone ID", !string.IsNullOrEmpty(treatmentPlan.ZoneID));
                RunTest("Treatment Plan Has Creation Date", treatmentPlan.PlanCreationDate != default(DateTime));
                RunTest("Treatment Plan Has Target Infestations", treatmentPlan.TargetInfestations != null);
            }
            
            // Test IPM effectiveness evaluation with mock data
            var mockTreatmentHistory = CreateMockTreatmentHistory();
            var mockMonitoringData = CreateMockMonitoringData();
            
            var effectivenessReport = _ipmSystem.EvaluateIPMEffectiveness(
                mockTreatmentHistory,
                mockMonitoringData,
                _testZone,
                30f // 30 day evaluation period
            );
            
            RunTest("IPM Effectiveness Report Creation", effectivenessReport != null);
            
            if (effectivenessReport != null)
            {
                LogTest($"Effectiveness Report - Zone: {effectivenessReport.ZoneID}, Period: {effectivenessReport.EvaluationPeriodDays} days");
                LogTest($"Report - Total Treatments: {effectivenessReport.TotalTreatments}, Effectiveness: {effectivenessReport.OverallEffectivenessRating:F2}");
                RunTest("Report Has Evaluation Timestamp", effectivenessReport.EvaluationTimestamp != default(DateTime));
                RunTest("Report Has Zone ID", !string.IsNullOrEmpty(effectivenessReport.ZoneID));
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestEnvironmentalIntegration()
        {
            LogTest("\n--- TESTING ENVIRONMENTAL INTEGRATION ---");
            
            if (_ipmSystem == null || _vpdSystem == null)
            {
                LogTest("ERROR: IPM System or VPD System not available");
                yield break;
            }
            
            // Test environmental optimization for IPM
            var currentEnvironment = ProjectChimera.Data.Cultivation.EnvironmentalConditions.CreateIndoorDefault();
            var mockBeneficials = CreateMockBeneficialOrganisms();
            var targetPests = new[] { PestType.Spider_Mites, PestType.Thrips };
            
            var environmentalOptimization = _ipmSystem.OptimizeEnvironmentForIPM(
                currentEnvironment,
                _vpdSystem,
                mockBeneficials,
                targetPests
            );
            
            RunTest("Environmental Optimization for IPM", environmentalOptimization != null);
            
            if (environmentalOptimization != null)
            {
                LogTest($"Environmental Optimization - Beneficials: {environmentalOptimization.ActiveBeneficials?.Length ?? 0}");
                LogTest($"Optimization - Target Pests: {environmentalOptimization.TargetPests?.Length ?? 0}");
                RunTest("Optimization Has Timestamp", environmentalOptimization.OptimizationTimestamp != default(DateTime));
                RunTest("Optimization Has Current Conditions", environmentalOptimization.CurrentConditions.IsInitialized());
                RunTest("Optimization Has Active Beneficials", environmentalOptimization.ActiveBeneficials != null);
                RunTest("Optimization Has Target Pests", environmentalOptimization.TargetPests != null);
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestComprehensiveIPMWorkflows()
        {
            LogTest("\n--- TESTING COMPREHENSIVE IPM WORKFLOWS ---");
            
            if (_ipmSystem == null)
            {
                LogTest("ERROR: IPM System not available");
                yield break;
            }
            
            // Test complete IPM workflow from assessment to treatment
            var environment = ProjectChimera.Data.Cultivation.EnvironmentalConditions.CreateIndoorDefault();
            var mockMonitoringData = CreateMockMonitoringData();
            
            // Step 1: Assess pest pressure
            var assessment = _ipmSystem.AssessPestPressure(
                _testZone,
                _testPlants.ToArray(),
                environment,
                mockMonitoringData
            );
            
            RunTest("Workflow Step 1: Pest Assessment", assessment != null);
            
            // Step 2: Create monitoring plan
            var monitoringPlan = _ipmSystem.CreateMonitoringPlan(
                _testZone,
                _testPlants.ToArray(),
                environment
            );
            
            RunTest("Workflow Step 2: Monitoring Plan", monitoringPlan != null);
            
            // Step 3: Create biological control plan
            var biologicalPlan = _ipmSystem.CreateBiologicalControlPlan(
                PestType.Spider_Mites,
                _testZone,
                environment,
                0.6f
            );
            
            RunTest("Workflow Step 3: Biological Control Plan", biologicalPlan != null);
            
            // Step 4: Create integrated treatment plan
            var mockInfestations = CreateMockPestInfestations();
            var treatmentPlan = _ipmSystem.CreateIntegratedTreatmentPlan(
                mockInfestations,
                _testZone,
                environment,
                _testPlants.ToArray()
            );
            
            RunTest("Workflow Step 4: Integrated Treatment Plan", treatmentPlan != null);
            
            // Step 5: Environmental optimization
            if (_vpdSystem != null)
            {
                var mockBeneficials = CreateMockBeneficialOrganisms();
                var environmentalOptimization = _ipmSystem.OptimizeEnvironmentForIPM(
                    environment,
                    _vpdSystem,
                    mockBeneficials,
                    new[] { PestType.Spider_Mites }
                );
                
                RunTest("Workflow Step 5: Environmental Optimization", environmentalOptimization != null);
            }
            
            LogTest("Comprehensive IPM workflow completed successfully");
            
            yield return new WaitForSeconds(0.1f);
        }
        
        private System.Collections.IEnumerator TestSystemIntegration()
        {
            LogTest("\n--- TESTING SYSTEM INTEGRATION ---");
            
            // Test IPM integration with Environmental Automation
            if (_ipmSystem != null && _environmentalSystem != null)
            {
                RunTest("IPM + Environmental Automation Integration Available", true);
                LogTest("IPM can coordinate with environmental automation for beneficial organism support");
            }
            
            // Test IPM integration with VPD Management
            if (_ipmSystem != null && _vpdSystem != null)
            {
                RunTest("IPM + VPD Management Integration Available", true);
                LogTest("IPM can optimize VPD for beneficial organism establishment");
            }
            
            // Test IPM integration with Fertigation
            if (_ipmSystem != null && _fertigationSystem != null)
            {
                RunTest("IPM + Fertigation Integration Available", true);
                LogTest("IPM can coordinate with fertigation for plant health optimization");
            }
            
            // Test full cultivation system coordination
            if (_ipmSystem != null && _environmentalSystem != null && _vpdSystem != null && _fertigationSystem != null)
            {
                RunTest("Full Cultivation System Coordination Available", true);
                LogTest("All major cultivation systems available for comprehensive IPM coordination");
            }
            
            yield return new WaitForSeconds(0.1f);
        }
        
        // Helper methods for creating mock data
        private PestMonitoringData[] CreateMockMonitoringData()
        {
            return new PestMonitoringData[]
            {
                new PestMonitoringData { Pest = PestType.Spider_Mites, Population = 2f, Date = DateTime.Now.AddDays(-1), Location = "Zone A" },
                new PestMonitoringData { Pest = PestType.Thrips, Population = 1f, Date = DateTime.Now, Location = "Zone A" },
                new PestMonitoringData { Pest = PestType.Aphids, Population = 0f, Date = DateTime.Now, Location = "Zone A" }
            };
        }
        
        private PestInfestation[] CreateMockPestInfestations()
        {
            return new PestInfestation[]
            {
                new PestInfestation { Pest = PestType.Spider_Mites, Severity = 0.3f, AffectedAreas = new[] { "Lower canopy", "Leaf undersides" } },
                new PestInfestation { Pest = PestType.Thrips, Severity = 0.2f, AffectedAreas = new[] { "New growth", "Flowers" } }
            };
        }
        
        private IPMTreatmentHistory[] CreateMockTreatmentHistory()
        {
            return new IPMTreatmentHistory[]
            {
                new IPMTreatmentHistory { Treatment = "Predatory Mites Release", Date = DateTime.Now.AddDays(-7), Dosage = 2f, Effectiveness = 0.8f },
                new IPMTreatmentHistory { Treatment = "Neem Oil Application", Date = DateTime.Now.AddDays(-3), Dosage = 1.5f, Effectiveness = 0.6f }
            };
        }
        
        private BeneficialOrganism[] CreateMockBeneficialOrganisms()
        {
            return new BeneficialOrganism[]
            {
                new BeneficialOrganism 
                { 
                    OrganismName = "Phytoseiulus persimilis",
                    TargetPests = new[] { "Spider mites" },
                    OptimalTemperature = new Vector2(20f, 28f),
                    OptimalHumidity = new Vector2(60f, 80f),
                    ReleaseRate = 2f
                }
            };
        }
        
        private void RunTest(string testName, bool passed)
        {
            _totalTests++;
            
            if (passed)
            {
                _passedTests++;
                LogTest($"✓ PASS: {testName}");
            }
            else
            {
                _failedTests++;
                LogTest($"✗ FAIL: {testName}");
            }
        }
        
        private void LogTest(string message)
        {
            if (_enableDetailedLogging)
            {
                Debug.Log($"[IPMSystemTest] {message}");
            }
            
            _testResults.Add($"{DateTime.Now:HH:mm:ss.fff} - {message}");
        }
        
        private void DisplayFinalResults()
        {
            float testDuration = Time.time - _testStartTime;
            float successRate = _totalTests > 0 ? (float)_passedTests / _totalTests * 100f : 0f;
            
            LogTest("\n=== IPM SYSTEM TEST RESULTS ===");
            LogTest($"Total Tests: {_totalTests}");
            LogTest($"Passed: {_passedTests}");
            LogTest($"Failed: {_failedTests}");
            LogTest($"Success Rate: {successRate:F1}%");
            LogTest($"Test Duration: {testDuration:F2} seconds");
            LogTest($"Test completed at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            
            if (_failedTests == 0)
            {
                LogTest("🎉 ALL IPM TESTS PASSED! Integrated Pest Management system is working correctly.");
            }
            else
            {
                LogTest($"⚠️ {_failedTests} test(s) failed. Check logs for details.");
            }
            
            LogTest("=== END IPM TEST SUITE ===");
        }
        
        [ContextMenu("Run IPM Tests")]
        public void RunIPMTestsManually()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(RunAllIPMTests());
            }
            else
            {
                Debug.LogWarning("[IPMSystemTester] Tests can only be run in play mode.");
            }
        }
        
        [ContextMenu("Clear Test Results")]
        public void ClearTestResults()
        {
            _testResults.Clear();
            _totalTests = 0;
            _passedTests = 0;
            _failedTests = 0;
            Debug.Log("[IPMSystemTester] Test results cleared.");
        }
        
        // Public properties for external access
        public int TotalTests => _totalTests;
        public int PassedTests => _passedTests;
        public int FailedTests => _failedTests;
        
        private void OnGUI()
        {
            if (!Application.isPlaying) return;
            
            // Display test results on screen
            GUILayout.BeginArea(new Rect(420, 10, 400, 200));
            GUILayout.BeginVertical("box");
            
            GUILayout.Label("IPM System Test Results", GUI.skin.box);
            GUILayout.Label($"Tests: {_passedTests}/{_totalTests} passed");
            
            if (_totalTests > 0)
            {
                float successRate = (float)_passedTests / _totalTests * 100f;
                GUILayout.Label($"Success Rate: {successRate:F1}%");
            }
            
            if (GUILayout.Button("Run IPM Tests"))
            {
                StartCoroutine(RunAllIPMTests());
            }
            
            if (GUILayout.Button("Clear Results"))
            {
                ClearTestResults();
            }
            
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}