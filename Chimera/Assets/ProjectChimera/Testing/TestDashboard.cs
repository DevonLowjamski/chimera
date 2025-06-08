using UnityEngine;
using System.Collections.Generic;
using ProjectChimera.Testing.Systems;
using ProjectChimera.Testing.Integration;
using ProjectChimera.Testing.Performance;
using System;
using System.Linq;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Comprehensive test dashboard for Project Chimera testing systems.
    /// Provides real-time monitoring, control, and visualization of all test components.
    /// </summary>
    public class TestDashboard : MonoBehaviour
    {
        [Header("Dashboard Configuration")]
        [SerializeField] private bool _showDashboard = true;
        [SerializeField] private bool _showDetailedStats = true;
        [SerializeField] private bool _enableRealTimeUpdates = true;
        [SerializeField] private float _updateInterval = 1f;
        [SerializeField] private int _maxLogEntries = 100;
        
        [Header("UI Configuration")]
        [SerializeField] private Vector2 _dashboardSize = new Vector2(800, 600);
        [SerializeField] private Vector2 _dashboardPosition = new Vector2(10, 10);
        [SerializeField] private float _scrollViewHeight = 150f;
        
        [Header("Test Components")]
        [SerializeField] private AutomatedTestManager _automatedTestManager;
        [SerializeField] private CultivationTestCoordinator _coordinator;
        [SerializeField] private AdvancedCultivationTestRunner _testRunner;
        [SerializeField] private CultivationIntegrationTests _integrationTests;
        [SerializeField] private CultivationPerformanceTests _performanceTests;
        [SerializeField] private TestReportGenerator _reportGenerator;
        
        // Dashboard state
        private List<string> _logMessages = new List<string>();
        private Vector2 _logScrollPosition = Vector2.zero;
        private Vector2 _mainScrollPosition = Vector2.zero;
        private float _lastUpdateTime = 0f;
        private TestDashboardData _dashboardData = new TestDashboardData();
        
        // GUI styles
        private GUIStyle _headerStyle;
        private GUIStyle _subHeaderStyle;
        private GUIStyle _statusStyle;
        private GUIStyle _logStyle;
        private GUIStyle _buttonStyle;
        private bool _stylesInitialized = false;
        
        private void Start()
        {
            // Auto-discover test components
            AutoDiscoverComponents();
            
            // Subscribe to events
            SubscribeToEvents();
            
            LogMessage("Test Dashboard initialized");
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events
            UnsubscribeFromEvents();
        }
        
        private void Update()
        {
            if (_enableRealTimeUpdates && Time.time - _lastUpdateTime >= _updateInterval)
            {
                UpdateDashboardData();
                _lastUpdateTime = Time.time;
            }
        }
        
        private void OnGUI()
        {
            if (!_showDashboard) return;
            
            InitializeStyles();
            
            // Main dashboard window
            GUI.BeginGroup(new Rect(_dashboardPosition.x, _dashboardPosition.y, _dashboardSize.x, _dashboardSize.y));
            
            // Background
            GUI.Box(new Rect(0, 0, _dashboardSize.x, _dashboardSize.y), "Project Chimera Test Dashboard", _headerStyle);
            
            // Main scroll view
            _mainScrollPosition = GUI.BeginScrollView(new Rect(10, 40, _dashboardSize.x - 20, _dashboardSize.y - 50), 
                                                    _mainScrollPosition, 
                                                    new Rect(0, 0, _dashboardSize.x - 40, CalculateContentHeight()));
            
            float yPos = 10f;
            
            // System Status Section
            yPos = DrawSystemStatusSection(yPos);
            yPos += 20f;
            
            // Test Controls Section
            yPos = DrawTestControlsSection(yPos);
            yPos += 20f;
            
            // Current Test Progress Section
            yPos = DrawTestProgressSection(yPos);
            yPos += 20f;
            
            // Performance Metrics Section
            yPos = DrawPerformanceMetricsSection(yPos);
            yPos += 20f;
            
            // Test History Section
            yPos = DrawTestHistorySection(yPos);
            yPos += 20f;
            
            // Log Messages Section
            yPos = DrawLogSection(yPos);
            
            GUI.EndScrollView();
            GUI.EndGroup();
        }
        
        private float DrawSystemStatusSection(float startY)
        {
            float yPos = startY;
            
            GUI.Label(new Rect(10, yPos, 200, 25), "System Status", _subHeaderStyle);
            yPos += 30f;
            
            // Component status
            yPos = DrawComponentStatus("Automated Test Manager", _automatedTestManager != null, 
                                     _automatedTestManager?.AutomationRunning ?? false, yPos);
            yPos = DrawComponentStatus("Test Coordinator", _coordinator != null, 
                                     _coordinator?.CoordinationRunning ?? false, yPos);
            yPos = DrawComponentStatus("Test Runner", _testRunner != null, 
                                     _testRunner?.TestsRunning ?? false, yPos);
            yPos = DrawComponentStatus("Integration Tests", _integrationTests != null, 
                                     _integrationTests?.TestsRunning ?? false, yPos);
            yPos = DrawComponentStatus("Performance Tests", _performanceTests != null, 
                                     _performanceTests?.TestsRunning ?? false, yPos);
            yPos = DrawComponentStatus("Report Generator", _reportGenerator != null, false, yPos);
            
            return yPos;
        }
        
        private float DrawComponentStatus(string componentName, bool exists, bool running, float yPos)
        {
            Color originalColor = GUI.color;
            
            // Status color
            if (!exists)
                GUI.color = Color.red;
            else if (running)
                GUI.color = Color.yellow;
            else
                GUI.color = Color.green;
            
            string status = !exists ? "MISSING" : (running ? "RUNNING" : "READY");
            GUI.Label(new Rect(20, yPos, 150, 20), componentName);
            GUI.Label(new Rect(200, yPos, 80, 20), status, _statusStyle);
            
            GUI.color = originalColor;
            return yPos + 25f;
        }
        
        private float DrawTestControlsSection(float startY)
        {
            float yPos = startY;
            
            GUI.Label(new Rect(10, yPos, 200, 25), "Test Controls", _subHeaderStyle);
            yPos += 30f;
            
            // Automated test controls
            if (GUI.Button(new Rect(20, yPos, 120, 25), "Start Full Suite", _buttonStyle))
            {
                _automatedTestManager?.StartAutomatedTestSuite();
            }
            
            if (GUI.Button(new Rect(150, yPos, 120, 25), "Stop All Tests", _buttonStyle))
            {
                LogMessage("Manual test stop requested");
            }
            yPos += 30f;
            
            // Individual test controls
            if (GUI.Button(new Rect(20, yPos, 100, 25), "Core Tests", _buttonStyle))
            {
                _testRunner?.StartTestSuite();
            }
            
            if (GUI.Button(new Rect(130, yPos, 120, 25), "Cultivation Tests", _buttonStyle))
            {
                _coordinator?.StartCoordinatedTesting();
            }
            
            if (GUI.Button(new Rect(260, yPos, 100, 25), "Integration", _buttonStyle))
            {
                _integrationTests?.StartIntegrationTests();
            }
            
            if (GUI.Button(new Rect(370, yPos, 100, 25), "Performance", _buttonStyle))
            {
                _performanceTests?.StartPerformanceTests();
            }
            yPos += 30f;
            
            // Report controls
            if (GUI.Button(new Rect(20, yPos, 120, 25), "Generate Report", _buttonStyle))
            {
                _reportGenerator?.GenerateComprehensiveReport();
            }
            
            if (GUI.Button(new Rect(150, yPos, 120, 25), "Clear History", _buttonStyle))
            {
                _automatedTestManager?.ClearTestHistory();
                _logMessages.Clear();
            }
            yPos += 30f;
            
            return yPos;
        }
        
        private float DrawTestProgressSection(float startY)
        {
            float yPos = startY;
            
            GUI.Label(new Rect(10, yPos, 200, 25), "Current Test Progress", _subHeaderStyle);
            yPos += 30f;
            
            if (_automatedTestManager?.AutomationRunning == true)
            {
                var currentSuite = _automatedTestManager.CurrentTestSuite;
                if (currentSuite != null)
                {
                    GUI.Label(new Rect(20, yPos, 400, 20), $"Test Suite: {currentSuite.Id}");
                    yPos += 25f;
                    
                    GUI.Label(new Rect(20, yPos, 400, 20), $"Status: {currentSuite.ExecutionStatus}");
                    yPos += 25f;
                    
                    float duration = (float)(DateTime.Now - currentSuite.StartTime).TotalSeconds;
                    GUI.Label(new Rect(20, yPos, 400, 20), $"Duration: {duration:F1}s");
                    yPos += 25f;
                }
            }
            else if (_coordinator?.CoordinationRunning == true)
            {
                GUI.Label(new Rect(20, yPos, 400, 20), $"Coordination Phase: {_coordinator.CurrentPhase}");
                yPos += 25f;
            }
            else if (_testRunner?.TestsRunning == true)
            {
                GUI.Label(new Rect(20, yPos, 400, 20), "Core System Tests Running");
                yPos += 25f;
            }
            else if (_integrationTests?.TestsRunning == true)
            {
                GUI.Label(new Rect(20, yPos, 400, 20), "Integration Tests Running");
                yPos += 25f;
            }
            else if (_performanceTests?.TestsRunning == true)
            {
                GUI.Label(new Rect(20, yPos, 400, 20), "Performance Tests Running");
                yPos += 25f;
            }
            else
            {
                GUI.Label(new Rect(20, yPos, 400, 20), "No tests currently running");
                yPos += 25f;
            }
            
            return yPos;
        }
        
        private float DrawPerformanceMetricsSection(float startY)
        {
            float yPos = startY;
            
            GUI.Label(new Rect(10, yPos, 200, 25), "Performance Metrics", _subHeaderStyle);
            yPos += 30f;
            
            // Current performance data
            float frameTime = Time.deltaTime * 1000f;
            float memoryUsage = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory() / (1024f * 1024f);
            
            GUI.Label(new Rect(20, yPos, 200, 20), $"Frame Time: {frameTime:F2}ms");
            GUI.Label(new Rect(250, yPos, 200, 20), $"Memory: {memoryUsage:F1}MB");
            yPos += 25f;
            
            GUI.Label(new Rect(20, yPos, 200, 20), $"FPS: {1f / Time.deltaTime:F1}");
            GUI.Label(new Rect(250, yPos, 200, 20), $"Unity: {Application.unityVersion}");
            yPos += 25f;
            
            // Performance test results if available
            if (_performanceTests?.TestSuite?.OverallMetrics != null)
            {
                var metrics = _performanceTests.TestSuite.OverallMetrics;
                GUI.Label(new Rect(20, yPos, 400, 20), $"Last Test - Avg Frame: {metrics.AverageFrameTime:F2}ms, Avg Memory: {metrics.AverageMemoryUsage:F1}MB");
                yPos += 25f;
            }
            
            return yPos;
        }
        
        private float DrawTestHistorySection(float startY)
        {
            float yPos = startY;
            
            GUI.Label(new Rect(10, yPos, 200, 25), "Test History", _subHeaderStyle);
            yPos += 30f;
            
            if (_automatedTestManager?.TestHistory != null && _automatedTestManager.TestHistory.Count > 0)
            {
                var history = _automatedTestManager.TestHistory.TakeLast(5).ToList();
                
                foreach (var testRun in history)
                {
                    string statusText = testRun.TestSuite.ExecutionStatus.ToString();
                    Color statusColor = GetStatusColor(testRun.TestSuite.ExecutionStatus);
                    
                    GUI.Label(new Rect(20, yPos, 150, 20), testRun.Timestamp.ToString("HH:mm:ss"));
                    
                    Color originalColor = GUI.color;
                    GUI.color = statusColor;
                    GUI.Label(new Rect(180, yPos, 100, 20), statusText, _statusStyle);
                    GUI.color = originalColor;
                    
                    GUI.Label(new Rect(290, yPos, 100, 20), $"{testRun.TestSuite.TotalDuration:F1}s");
                    yPos += 20f;
                }
            }
            else
            {
                GUI.Label(new Rect(20, yPos, 400, 20), "No test history available");
                yPos += 25f;
            }
            
            return yPos;
        }
        
        private float DrawLogSection(float startY)
        {
            float yPos = startY;
            
            GUI.Label(new Rect(10, yPos, 200, 25), "Log Messages", _subHeaderStyle);
            yPos += 30f;
            
            // Log scroll view
            Rect logViewRect = new Rect(10, yPos, _dashboardSize.x - 60, _scrollViewHeight);
            Rect logContentRect = new Rect(0, 0, _dashboardSize.x - 80, _logMessages.Count * 18f);
            
            _logScrollPosition = GUI.BeginScrollView(logViewRect, _logScrollPosition, logContentRect);
            
            for (int i = 0; i < _logMessages.Count; i++)
            {
                GUI.Label(new Rect(5, i * 18f, logContentRect.width - 10, 18f), _logMessages[i], _logStyle);
            }
            
            GUI.EndScrollView();
            
            return yPos + _scrollViewHeight + 10f;
        }
        
        private void InitializeStyles()
        {
            if (_stylesInitialized) return;
            
            _headerStyle = new GUIStyle(GUI.skin.box);
            _headerStyle.fontSize = 16;
            _headerStyle.fontStyle = FontStyle.Bold;
            _headerStyle.alignment = TextAnchor.MiddleCenter;
            
            _subHeaderStyle = new GUIStyle(GUI.skin.label);
            _subHeaderStyle.fontSize = 14;
            _subHeaderStyle.fontStyle = FontStyle.Bold;
            
            _statusStyle = new GUIStyle(GUI.skin.label);
            _statusStyle.fontSize = 12;
            _statusStyle.fontStyle = FontStyle.Bold;
            
            _logStyle = new GUIStyle(GUI.skin.label);
            _logStyle.fontSize = 10;
            _logStyle.wordWrap = false;
            
            _buttonStyle = new GUIStyle(GUI.skin.button);
            _buttonStyle.fontSize = 11;
            
            _stylesInitialized = true;
        }
        
        private Color GetStatusColor(TestExecutionStatus status)
        {
            switch (status)
            {
                case TestExecutionStatus.Completed:
                    return Color.green;
                case TestExecutionStatus.Failed:
                    return Color.red;
                case TestExecutionStatus.Running:
                    return Color.yellow;
                case TestExecutionStatus.Timeout:
                    return Color.magenta;
                default:
                    return Color.white;
            }
        }
        
        private float CalculateContentHeight()
        {
            // Calculate total content height for scrolling
            float height = 100f; // Base height
            height += 250f; // System status
            height += 150f; // Test controls
            height += 150f; // Test progress
            height += 100f; // Performance metrics
            height += 200f; // Test history
            height += _scrollViewHeight + 50f; // Log section
            return height;
        }
        
        private void UpdateDashboardData()
        {
            _dashboardData.LastUpdate = DateTime.Now;
            _dashboardData.CurrentFrameTime = Time.deltaTime * 1000f;
            _dashboardData.CurrentMemoryUsage = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemory() / (1024f * 1024f);
            _dashboardData.AnyTestsRunning = IsAnyTestRunning();
        }
        
        private bool IsAnyTestRunning()
        {
            return (_automatedTestManager?.AutomationRunning == true) ||
                   (_coordinator?.CoordinationRunning == true) ||
                   (_testRunner?.TestsRunning == true) ||
                   (_integrationTests?.TestsRunning == true) ||
                   (_performanceTests?.TestsRunning == true);
        }
        
        private void AutoDiscoverComponents()
        {
            if (_automatedTestManager == null) _automatedTestManager = FindAnyObjectByType<AutomatedTestManager>();
            if (_coordinator == null) _coordinator = FindAnyObjectByType<CultivationTestCoordinator>();
            if (_testRunner == null) _testRunner = FindAnyObjectByType<AdvancedCultivationTestRunner>();
            if (_integrationTests == null) _integrationTests = FindAnyObjectByType<CultivationIntegrationTests>();
            if (_performanceTests == null) _performanceTests = FindAnyObjectByType<CultivationPerformanceTests>();
            if (_reportGenerator == null) _reportGenerator = FindAnyObjectByType<TestReportGenerator>();
        }
        
        private void SubscribeToEvents()
        {
            if (_automatedTestManager != null)
            {
                _automatedTestManager.OnAutomationLogMessage += LogMessage;
                _automatedTestManager.OnExecutionStatusChanged += OnExecutionStatusChanged;
            }
            
            if (_coordinator != null)
            {
                // Subscribe to coordinator events if available
            }
        }
        
        private void UnsubscribeFromEvents()
        {
            if (_automatedTestManager != null)
            {
                _automatedTestManager.OnAutomationLogMessage -= LogMessage;
                _automatedTestManager.OnExecutionStatusChanged -= OnExecutionStatusChanged;
            }
        }
        
        private void LogMessage(string message)
        {
            string timestampedMessage = $"[{DateTime.Now:HH:mm:ss}] {message}";
            _logMessages.Add(timestampedMessage);
            
            // Limit log size
            if (_logMessages.Count > _maxLogEntries)
            {
                _logMessages.RemoveAt(0);
            }
            
            // Auto-scroll to bottom
            _logScrollPosition.y = Mathf.Max(0, _logMessages.Count * 18f - _scrollViewHeight);
        }
        
        private void OnExecutionStatusChanged(TestExecutionStatus status)
        {
            LogMessage($"Test execution status changed: {status}");
        }
        
        // Public properties
        public bool ShowDashboard
        {
            get => _showDashboard;
            set => _showDashboard = value;
        }
        
        public TestDashboardData DashboardData => _dashboardData;
        
        // Context menu actions
        [ContextMenu("Toggle Dashboard")]
        public void ToggleDashboard()
        {
            _showDashboard = !_showDashboard;
            LogMessage($"Dashboard {(_showDashboard ? "enabled" : "disabled")}");
        }
        
        [ContextMenu("Clear Log")]
        public void ClearLog()
        {
            _logMessages.Clear();
            LogMessage("Log cleared");
        }
        
        [ContextMenu("Refresh Components")]
        public void RefreshComponents()
        {
            AutoDiscoverComponents();
            LogMessage("Test components refreshed");
        }
    }
    
    [System.Serializable]
    public class TestDashboardData
    {
        public DateTime LastUpdate;
        public float CurrentFrameTime;
        public float CurrentMemoryUsage;
        public bool AnyTestsRunning;
        public int TotalTestComponents;
        public int ActiveTestComponents;
    }
} 