using UnityEngine;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Testing.Systems;
using ProjectChimera.Testing.Integration;
using ProjectChimera.Testing.Performance;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Automated scene setup for Project Chimera testing environment.
    /// Creates all required test components and core systems in the scene.
    /// </summary>
    public class AutoTestSceneSetup : MonoBehaviour
    {
        [Header("Setup Configuration")]
        [SerializeField] private bool _autoRunAfterSetup = true;
        [SerializeField] private bool _organizeInHierarchy = true;
        [SerializeField] private bool _linkComponentsAutomatically = true;
        [SerializeField] private bool _addCoreSystemComponents = true;
        
        [Header("Setup Status")]
        [SerializeField] private bool _sceneSetupComplete = false;
        [SerializeField] private int _componentsCreated = 0;
        
        // Component references for verification
        private GameObject _testingRoot;
        private GameObject _coreSystemsRoot;
        private AutomatedTestManager _automatedTestManager;
        private CultivationTestCoordinator _testCoordinator;
        private AdvancedCultivationTestRunner _testRunner;
        private CultivationIntegrationTests _integrationTests;
        private CultivationPerformanceTests _performanceTests;
        private TestReportGenerator _reportGenerator;
        private CultivationSystemValidator _systemValidator;
        private CultivationManager _cultivationManager;
        
        [ContextMenu("Setup Complete Testing Scene")]
        public void SetupCompleteTestingScene()
        {
            Debug.Log("=== STARTING PROJECT CHIMERA TEST SCENE SETUP ===");
            
            _componentsCreated = 0;
            
            // Step 1: Create organizational structure
            CreateHierarchyStructure();
            
            // Step 2: Create core system components
            if (_addCoreSystemComponents)
            {
                CreateCoreSystemComponents();
            }
            
            // Step 3: Create testing components
            CreateTestingComponents();
            
            // Step 4: Link components together
            if (_linkComponentsAutomatically)
            {
                LinkTestComponents();
            }
            
            // Step 5: Finalize setup
            FinalizeSetup();
            
            Debug.Log($"=== SETUP COMPLETE: {_componentsCreated} components created ===");
            Debug.Log("You can now run tests using the AutomatedTestManager!");
        }
        
        private void CreateHierarchyStructure()
        {
            if (_organizeInHierarchy)
            {
                // Create root folders for organization
                _testingRoot = CreateOrFindGameObject("=== TESTING FRAMEWORK ===", null);
                _coreSystemsRoot = CreateOrFindGameObject("=== CORE SYSTEMS ===", null);
                
                Debug.Log("Created hierarchy structure");
            }
        }
        
        private void CreateCoreSystemComponents()
        {
            Debug.Log("Creating core system components...");
            
            // CultivationManager - Core system for cultivation operations
            if (FindAnyObjectByType<CultivationManager>() == null)
            {
                var cultivationManagerGO = CreateOrFindGameObject("CultivationManager", _coreSystemsRoot);
                _cultivationManager = cultivationManagerGO.GetComponent<CultivationManager>();
                if (_cultivationManager == null)
                {
                    _cultivationManager = cultivationManagerGO.AddComponent<CultivationManager>();
                }
                _componentsCreated++;
                Debug.Log("✅ Created CultivationManager");
            }
            else
            {
                _cultivationManager = FindAnyObjectByType<CultivationManager>();
                Debug.Log("✅ Found existing CultivationManager");
            }
        }
        
        private void CreateTestingComponents()
        {
            Debug.Log("Creating testing components...");
            
            // AutomatedTestManager - Main test orchestrator
            if (FindAnyObjectByType<AutomatedTestManager>() == null)
            {
                var testManagerGO = CreateOrFindGameObject("AutomatedTestManager", _testingRoot);
                _automatedTestManager = testManagerGO.AddComponent<AutomatedTestManager>();
                _componentsCreated++;
                Debug.Log("✅ Created AutomatedTestManager");
            }
            else
            {
                _automatedTestManager = FindAnyObjectByType<AutomatedTestManager>();
                Debug.Log("✅ Found existing AutomatedTestManager");
            }
            
            // CultivationTestCoordinator - Coordinates test phases
            if (FindAnyObjectByType<CultivationTestCoordinator>() == null)
            {
                var coordinatorGO = CreateOrFindGameObject("CultivationTestCoordinator", _testingRoot);
                _testCoordinator = coordinatorGO.AddComponent<CultivationTestCoordinator>();
                _componentsCreated++;
                Debug.Log("✅ Created CultivationTestCoordinator");
            }
            else
            {
                _testCoordinator = FindAnyObjectByType<CultivationTestCoordinator>();
                Debug.Log("✅ Found existing CultivationTestCoordinator");
            }
            
            // AdvancedCultivationTestRunner - Core cultivation tests
            if (FindAnyObjectByType<AdvancedCultivationTestRunner>() == null)
            {
                var testRunnerGO = CreateOrFindGameObject("AdvancedCultivationTestRunner", _testingRoot);
                _testRunner = testRunnerGO.AddComponent<AdvancedCultivationTestRunner>();
                _componentsCreated++;
                Debug.Log("✅ Created AdvancedCultivationTestRunner");
            }
            else
            {
                _testRunner = FindAnyObjectByType<AdvancedCultivationTestRunner>();
                Debug.Log("✅ Found existing AdvancedCultivationTestRunner");
            }
            
            // CultivationSystemValidator - System validation
            if (FindAnyObjectByType<CultivationSystemValidator>() == null)
            {
                var validatorGO = CreateOrFindGameObject("CultivationSystemValidator", _testingRoot);
                _systemValidator = validatorGO.AddComponent<CultivationSystemValidator>();
                _componentsCreated++;
                Debug.Log("✅ Created CultivationSystemValidator");
            }
            else
            {
                _systemValidator = FindAnyObjectByType<CultivationSystemValidator>();
                Debug.Log("✅ Found existing CultivationSystemValidator");
            }
            
            // CultivationIntegrationTests - Integration testing
            if (FindAnyObjectByType<CultivationIntegrationTests>() == null)
            {
                var integrationGO = CreateOrFindGameObject("CultivationIntegrationTests", _testingRoot);
                _integrationTests = integrationGO.AddComponent<CultivationIntegrationTests>();
                _componentsCreated++;
                Debug.Log("✅ Created CultivationIntegrationTests");
            }
            else
            {
                _integrationTests = FindAnyObjectByType<CultivationIntegrationTests>();
                Debug.Log("✅ Found existing CultivationIntegrationTests");
            }
            
            // CultivationPerformanceTests - Performance testing
            if (FindAnyObjectByType<CultivationPerformanceTests>() == null)
            {
                var performanceGO = CreateOrFindGameObject("CultivationPerformanceTests", _testingRoot);
                _performanceTests = performanceGO.AddComponent<CultivationPerformanceTests>();
                _componentsCreated++;
                Debug.Log("✅ Created CultivationPerformanceTests");
            }
            else
            {
                _performanceTests = FindAnyObjectByType<CultivationPerformanceTests>();
                Debug.Log("✅ Found existing CultivationPerformanceTests");
            }
            
            // TestReportGenerator - Report generation
            if (FindAnyObjectByType<TestReportGenerator>() == null)
            {
                var reportGO = CreateOrFindGameObject("TestReportGenerator", _testingRoot);
                _reportGenerator = reportGO.AddComponent<TestReportGenerator>();
                _componentsCreated++;
                Debug.Log("✅ Created TestReportGenerator");
            }
            else
            {
                _reportGenerator = FindAnyObjectByType<TestReportGenerator>();
                Debug.Log("✅ Found existing TestReportGenerator");
            }
        }
        
        private void LinkTestComponents()
        {
            Debug.Log("Linking test components...");
            
            // Note: Most components use auto-discovery via FindAnyObjectByType,
            // but we can manually link them here if needed for better performance
            
            if (_automatedTestManager != null)
            {
                // AutomatedTestManager will auto-discover components in Start()
                Debug.Log("✅ AutomatedTestManager will auto-discover components");
            }
            
            if (_testCoordinator != null)
            {
                // CultivationTestCoordinator will auto-discover components in Start()
                Debug.Log("✅ CultivationTestCoordinator will auto-discover components");
            }
            
            Debug.Log("✅ Component linking configured");
        }
        
        private void FinalizeSetup()
        {
            _sceneSetupComplete = true;
            
            // Configure AutomatedTestManager for immediate use
            if (_automatedTestManager != null && _autoRunAfterSetup)
            {
                Debug.Log("⚡ AutomatedTestManager configured for auto-run");
                Debug.Log("💡 Enter Play Mode to start tests automatically, or use Context Menu to run manually");
            }
            
            // Mark this setup script for cleanup
            Debug.Log("💡 You can now delete this AutoTestSceneSetup GameObject if desired");
        }
        
        private GameObject CreateOrFindGameObject(string name, GameObject parent)
        {
            // Try to find existing GameObject first
            GameObject existing = GameObject.Find(name);
            if (existing != null)
            {
                return existing;
            }
            
            // Create new GameObject
            GameObject newGO = new GameObject(name);
            
            if (parent != null)
            {
                newGO.transform.SetParent(parent.transform);
            }
            
            return newGO;
        }
        
        [ContextMenu("Verify Scene Setup")]
        public void VerifySceneSetup()
        {
            Debug.Log("=== VERIFYING TEST SCENE SETUP ===");
            
            int foundComponents = 0;
            System.Text.StringBuilder report = new System.Text.StringBuilder();
            
            // Check core systems
            report.AppendLine("CORE SYSTEMS:");
            if (FindAnyObjectByType<CultivationManager>() != null)
            {
                report.AppendLine("✅ CultivationManager");
                foundComponents++;
            }
            else
            {
                report.AppendLine("❌ CultivationManager - MISSING");
            }
            
            // Check testing components
            report.AppendLine("\nTESTING COMPONENTS:");
            
            if (FindAnyObjectByType<AutomatedTestManager>() != null)
            {
                report.AppendLine("✅ AutomatedTestManager");
                foundComponents++;
            }
            else
            {
                report.AppendLine("❌ AutomatedTestManager - MISSING");
            }
            
            if (FindAnyObjectByType<CultivationTestCoordinator>() != null)
            {
                report.AppendLine("✅ CultivationTestCoordinator");
                foundComponents++;
            }
            else
            {
                report.AppendLine("❌ CultivationTestCoordinator - MISSING");
            }
            
            if (FindAnyObjectByType<AdvancedCultivationTestRunner>() != null)
            {
                report.AppendLine("✅ AdvancedCultivationTestRunner");
                foundComponents++;
            }
            else
            {
                report.AppendLine("❌ AdvancedCultivationTestRunner - MISSING");
            }
            
            if (FindAnyObjectByType<CultivationSystemValidator>() != null)
            {
                report.AppendLine("✅ CultivationSystemValidator");
                foundComponents++;
            }
            else
            {
                report.AppendLine("❌ CultivationSystemValidator - MISSING");
            }
            
            if (FindAnyObjectByType<CultivationIntegrationTests>() != null)
            {
                report.AppendLine("✅ CultivationIntegrationTests");
                foundComponents++;
            }
            else
            {
                report.AppendLine("❌ CultivationIntegrationTests - MISSING");
            }
            
            if (FindAnyObjectByType<CultivationPerformanceTests>() != null)
            {
                report.AppendLine("✅ CultivationPerformanceTests");
                foundComponents++;
            }
            else
            {
                report.AppendLine("❌ CultivationPerformanceTests - MISSING");
            }
            
            if (FindAnyObjectByType<TestReportGenerator>() != null)
            {
                report.AppendLine("✅ TestReportGenerator");
                foundComponents++;
            }
            else
            {
                report.AppendLine("❌ TestReportGenerator - MISSING");
            }
            
            report.AppendLine($"\nSUMMARY: {foundComponents}/7 components found");
            
            if (foundComponents >= 7)
            {
                report.AppendLine("🎉 SCENE READY FOR COMPREHENSIVE TESTING!");
            }
            else
            {
                report.AppendLine("⚠️  Some components missing - run 'Setup Complete Testing Scene'");
            }
            
            Debug.Log(report.ToString());
        }
        
        [ContextMenu("Clean Up Setup Script")]
        public void CleanUpSetupScript()
        {
            Debug.Log("Removing AutoTestSceneSetup - setup is complete!");
            DestroyImmediate(gameObject);
        }
    }
    
#if UNITY_EDITOR
    /// <summary>
    /// Custom editor for AutoTestSceneSetup with improved UI
    /// </summary>
    [CustomEditor(typeof(AutoTestSceneSetup))]
    public class AutoTestSceneSetupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            AutoTestSceneSetup setup = (AutoTestSceneSetup)target;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Project Chimera Test Scene Setup", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("This script automatically creates all required testing components in your scene.", MessageType.Info);
            
            EditorGUILayout.Space();
            
            // Big setup button
            if (GUILayout.Button("🚀 Setup Complete Testing Scene", GUILayout.Height(40)))
            {
                setup.SetupCompleteTestingScene();
            }
            
            EditorGUILayout.Space();
            
            // Verification button
            if (GUILayout.Button("🔍 Verify Scene Setup", GUILayout.Height(30)))
            {
                setup.VerifySceneSetup();
            }
            
            EditorGUILayout.Space();
            
            // Show default inspector
            DrawDefaultInspector();
            
            EditorGUILayout.Space();
            
            // Cleanup button
            EditorGUILayout.HelpBox("Once setup is complete, you can safely delete this GameObject.", MessageType.None);
            if (GUILayout.Button("🗑️ Clean Up Setup Script"))
            {
                setup.CleanUpSetupScript();
            }
        }
    }
#endif
} 