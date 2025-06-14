using UnityEngine;
using UnityEngine.UIElements;
using ProjectChimera.Core;
using ProjectChimera.Data;
using ProjectChimera.Systems;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

namespace ProjectChimera.UI.Core
{
    /// <summary>
    /// Main UI controller for Project Chimera using Unity UI Toolkit.
    /// Manages all game interfaces, HUD elements, panels, and user interactions
    /// with comprehensive integration to all game systems.
    /// </summary>
    public class MainGameUIController : ChimeraManager
    {
        [Header("UI Configuration")]
        [SerializeField] private UIDocument _mainUIDocument;
        [SerializeField] private VisualTreeAsset _hudTemplate;
        [SerializeField] private VisualTreeAsset _panelTemplate;
        [SerializeField] private VisualTreeAsset _widgetTemplate;
        [SerializeField] private List<StyleSheet> _globalStyleSheets;
        
        [Header("UI Libraries")]
        [SerializeField] private UIPrefabLibrary _uiPrefabLibrary;
        [SerializeField] private EffectsPrefabLibrary _effectsLibrary;
        
        [Header("Interface Settings")]
        [SerializeField] private bool _enableDynamicUI = true;
        [SerializeField] private bool _enableUIAnimations = true;
        [SerializeField] private bool _enableAccessibility = true;
        [SerializeField] private float _uiUpdateInterval = 0.5f;
        [SerializeField] private UITheme _activeTheme;
        
        // Core UI Elements
        private VisualElement _rootElement;
        private VisualElement _hudContainer;
        private VisualElement _panelContainer;
        private VisualElement _overlayContainer;
        private VisualElement _notificationContainer;
        
        // HUD Components
        private EnvironmentalStatusHUD _environmentalHUD;
        private PlantStatusHUD _plantStatusHUD;
        private SystemPerformanceHUD _systemPerformanceHUD;
        private ConstructionProgressHUD _constructionHUD;
        private EconomicOverviewHUD _economicHUD;
        
        // Active Panels
        private Dictionary<string, UIPanel> _activePanels = new Dictionary<string, UIPanel>();
        private Dictionary<string, UIWidget> _activeWidgets = new Dictionary<string, UIWidget>();
        
        // Data Management
        private UIDataManager _dataManager;
        private UIEventHandler _eventHandler;
        private UIAnimationController _animationController;
        private UIAccessibilityManager _accessibilityManager;
        
        // System References - Using generic interfaces/base classes
        private ChimeraManager _plantManager;
        private ChimeraManager _environmentalManager;
        private MonoBehaviour[] _lightSystems;
        private ChimeraManager _facilityConstructor;
        private ChimeraManager _researchManager;
        private ChimeraManager _marketManager;
        
        // UI State
        private UIState _currentState = UIState.Game;
        private bool _isUIInitialized = false;
        private bool _isPaused = false;
        private float _lastUIUpdate = 0f;
        
        // Events
        public System.Action<UIPanel> OnPanelOpened;
        public System.Action<UIPanel> OnPanelClosed;
        public System.Action<UIWidget> OnWidgetCreated;
        public System.Action<UIState> OnUIStateChanged;
        public System.Action<string> OnUIError;
        
        // Properties
        public bool IsUIInitialized => _isUIInitialized;
        public UIState CurrentState => _currentState;
        public Dictionary<string, UIPanel> ActivePanels => _activePanels;
        public VisualElement RootElement => _rootElement;
        
        protected override void OnManagerInitialize()
        {
            InitializeUISystem();
            InitializeDataManagement();
            SetupUIElements();
            ConnectToGameSystems();
            StartUIUpdateLoop();
        }
        
        private void Update()
        {
            if (!_isUIInitialized) return;
            
            float currentTime = Time.time;
            if (currentTime - _lastUIUpdate >= _uiUpdateInterval)
            {
                UpdateUIElements();
                _lastUIUpdate = currentTime;
            }
            
            ProcessUIEvents();
            UpdateAnimations();
        }
        
        #region Initialization
        
        private void InitializeUISystem()
        {
            // Get or create main UI document
            if (_mainUIDocument == null)
            {
                _mainUIDocument = GetComponent<UIDocument>();
                if (_mainUIDocument == null)
                {
                    _mainUIDocument = gameObject.AddComponent<UIDocument>();
                }
            }
            
            // Initialize UI libraries
            if (_uiPrefabLibrary != null)
            {
                _uiPrefabLibrary.InitializeDefaults();
            }
            
            if (_effectsLibrary != null)
            {
                _effectsLibrary.InitializeDefaults();
            }
            
            // Setup root element
            _rootElement = _mainUIDocument.rootVisualElement;
            
            // Apply global stylesheets
            foreach (var styleSheet in _globalStyleSheets)
            {
                if (styleSheet != null)
                {
                    _rootElement.styleSheets.Add(styleSheet);
                }
            }
            
            LogInfo("UI System initialized");
        }
        
        private void InitializeDataManagement()
        {
            _dataManager = new UIDataManager();
            _eventHandler = new UIEventHandler();
            
            if (_enableUIAnimations)
            {
                _animationController = new UIAnimationController();
            }
            
            if (_enableAccessibility)
            {
                _accessibilityManager = new UIAccessibilityManager();
            }
        }
        
        private void SetupUIElements()
        {
            CreateUIContainers();
            InitializeHUDComponents();
            SetupUITheme();
            
            _isUIInitialized = true;
            LogInfo("UI Elements setup complete");
        }
        
        private void CreateUIContainers()
        {
            // Create main containers
            _hudContainer = new VisualElement();
            _hudContainer.name = "hud-container";
            _hudContainer.AddToClassList("hud-container");
            _rootElement.Add(_hudContainer);
            
            _panelContainer = new VisualElement();
            _panelContainer.name = "panel-container";
            _panelContainer.AddToClassList("panel-container");
            _rootElement.Add(_panelContainer);
            
            _overlayContainer = new VisualElement();
            _overlayContainer.name = "overlay-container";
            _overlayContainer.AddToClassList("overlay-container");
            _rootElement.Add(_overlayContainer);
            
            _notificationContainer = new VisualElement();
            _notificationContainer.name = "notification-container";
            _notificationContainer.AddToClassList("notification-container");
            _rootElement.Add(_notificationContainer);
        }
        
        private void InitializeHUDComponents()
        {
            // Environmental Status HUD
            _environmentalHUD = new EnvironmentalStatusHUD();
            var envHUDElement = _environmentalHUD.CreateHUDElement();
            envHUDElement.AddToClassList("environmental-hud");
            _hudContainer.Add(envHUDElement);
            
            // Plant Status HUD
            _plantStatusHUD = new PlantStatusHUD();
            var plantHUDElement = _plantStatusHUD.CreateHUDElement();
            plantHUDElement.AddToClassList("plant-hud");
            _hudContainer.Add(plantHUDElement);
            
            // System Performance HUD
            _systemPerformanceHUD = new SystemPerformanceHUD();
            var systemHUDElement = _systemPerformanceHUD.CreateHUDElement();
            systemHUDElement.AddToClassList("system-hud");
            _hudContainer.Add(systemHUDElement);
            
            // Construction Progress HUD
            _constructionHUD = new ConstructionProgressHUD();
            var constructionHUDElement = _constructionHUD.CreateHUDElement();
            constructionHUDElement.AddToClassList("construction-hud");
            _hudContainer.Add(constructionHUDElement);
            
            // Economic Overview HUD
            _economicHUD = new EconomicOverviewHUD();
            var economicHUDElement = _economicHUD.CreateHUDElement();
            economicHUDElement.AddToClassList("economic-hud");
            _hudContainer.Add(economicHUDElement);
        }
        
        private void SetupUITheme()
        {
            if (_activeTheme != null)
            {
                ApplyTheme(_activeTheme);
            }
        }
        
        private void ConnectToGameSystems()
        {
            // Get references to game systems
            if (GameManager.Instance != null)
            {
                _plantManager = GameManager.Instance.GetManager<PlantManager>();
                _environmentalManager = GameManager.Instance.GetManager<EnvironmentalManager>();
                _researchManager = GameManager.Instance.GetManager<ResearchManager>();
                _marketManager = GameManager.Instance.GetManager<MarketManager>();
                _facilityConstructor = GameManager.Instance.GetManager<InteractiveFacilityConstructor>();
            }
            
            // Find lighting systems
            _lightSystems = UnityEngine.Object.FindObjectsByType<AdvancedGrowLightSystem>(FindObjectsSortMode.None);
            
            // Connect event handlers
            ConnectSystemEvents();
            
            LogInfo("Connected to game systems");
        }
        
        private void ConnectSystemEvents()
        {
            // Plant Manager Events
            if (_plantManager != null)
            {
                _plantManager.OnPlantAdded += HandlePlantAdded;
                _plantManager.OnPlantRemoved += HandlePlantRemoved;
                _plantManager.OnPlantStageChanged += HandlePlantStageChanged;
            }
            
            // Environmental Manager Events
            if (_environmentalManager != null)
            {
                _environmentalManager.OnConditionsChanged += HandleEnvironmentalChange;
                _environmentalManager.OnAlertTriggered += HandleEnvironmentalAlert;
            }
            
            // Research Manager Events
            if (_researchManager != null)
            {
                _researchManager.OnResearchCompleted += HandleResearchCompleted;
                _researchManager.OnResearchStarted += HandleResearchStarted;
            }
            
            // Market Manager Events
            if (_marketManager != null)
            {
                _marketManager.OnMarketUpdate += HandleMarketUpdate;
                _marketManager.OnTradeCompleted += HandleTradeCompleted;
            }
            
            // Facility Constructor Events
            if (_facilityConstructor != null)
            {
                _facilityConstructor.OnProjectStarted += HandleConstructionStarted;
                _facilityConstructor.OnProjectCompleted += HandleConstructionCompleted;
                _facilityConstructor.OnConstructionIssue += HandleConstructionIssue;
            }
        }
        
        private void StartUIUpdateLoop()
        {
            if (_enableDynamicUI)
            {
                InvokeRepeating(nameof(RefreshAllUIData), 1f, _uiUpdateInterval);
            }
        }
        
        #endregion
        
        #region Panel Management
        
        public void OpenPanel(string panelId, object panelData = null)
        {
            if (_activePanels.ContainsKey(panelId))
            {
                // Panel already open, bring to front
                BringPanelToFront(panelId);
                return;
            }
            
            var panelPrefab = _uiPrefabLibrary?.GetUIPrefab(UIComponentType.Panel, panelId);
            if (panelPrefab == null)
            {
                LogWarning($"Panel prefab not found: {panelId}");
                return;
            }
            
            var panel = CreatePanel(panelPrefab, panelData);
            if (panel != null)
            {
                _activePanels[panelId] = panel;
                _panelContainer.Add(panel.RootElement);
                
                if (_enableUIAnimations)
                {
                    _animationController?.AnimateIn(panel.RootElement);
                }
                
                OnPanelOpened?.Invoke(panel);
                LogInfo($"Opened panel: {panelId}");
            }
        }
        
        public void ClosePanel(string panelId)
        {
            if (!_activePanels.TryGetValue(panelId, out var panel))
            {
                return;
            }
            
            if (_enableUIAnimations)
            {
                _animationController?.AnimateOut(panel.RootElement, () => RemovePanel(panelId));
            }
            else
            {
                RemovePanel(panelId);
            }
        }
        
        private void RemovePanel(string panelId)
        {
            if (_activePanels.TryGetValue(panelId, out var panel))
            {
                _panelContainer.Remove(panel.RootElement);
                panel.Dispose();
                _activePanels.Remove(panelId);
                
                OnPanelClosed?.Invoke(panel);
                LogInfo($"Closed panel: {panelId}");
            }
        }
        
        private void BringPanelToFront(string panelId)
        {
            if (_activePanels.TryGetValue(panelId, out var panel))
            {
                panel.RootElement.BringToFront();
            }
        }
        
        private UIPanel CreatePanel(UIPrefabEntry panelPrefab, object panelData)
        {
            switch (panelPrefab.PrefabId)
            {
                case "panel_facility_management":
                    return new FacilityManagementPanel(panelPrefab, _facilityConstructor, panelData);
                    
                case "panel_plant_genetics":
                    return new PlantGeneticsPanel(panelPrefab, _plantManager, panelData);
                    
                case "panel_environmental_control":
                    return new EnvironmentalControlPanel(panelPrefab, _environmentalManager, _lightSystems, panelData);
                    
                case "panel_research_development":
                    return new ResearchDevelopmentPanel(panelPrefab, _researchManager, panelData);
                    
                case "panel_market_trading":
                    return new MarketTradingPanel(panelPrefab, _marketManager, panelData);
                    
                default:
                    return new GenericUIPanel(panelPrefab, panelData);
            }
        }
        
        #endregion
        
        #region Widget Management
        
        public void CreateWidget(string widgetId, VisualElement parent = null)
        {
            if (_activeWidgets.ContainsKey(widgetId))
            {
                LogWarning($"Widget already exists: {widgetId}");
                return;
            }
            
            var widgetPrefab = _uiPrefabLibrary?.GetUIPrefab(UIComponentType.Widget, widgetId);
            if (widgetPrefab == null)
            {
                LogWarning($"Widget prefab not found: {widgetId}");
                return;
            }
            
            var widget = CreateWidgetInstance(widgetPrefab);
            if (widget != null)
            {
                _activeWidgets[widgetId] = widget;
                
                var targetParent = parent ?? _hudContainer;
                targetParent.Add(widget.RootElement);
                
                OnWidgetCreated?.Invoke(widget);
                LogInfo($"Created widget: {widgetId}");
            }
        }
        
        public void RemoveWidget(string widgetId)
        {
            if (_activeWidgets.TryGetValue(widgetId, out var widget))
            {
                widget.RootElement.RemoveFromHierarchy();
                widget.Dispose();
                _activeWidgets.Remove(widgetId);
                
                LogInfo($"Removed widget: {widgetId}");
            }
        }
        
        private UIWidget CreateWidgetInstance(UIPrefabEntry widgetPrefab)
        {
            switch (widgetPrefab.PrefabId)
            {
                case "widget_realtime_chart":
                    return new RealtimeChartWidget(widgetPrefab);
                    
                case "widget_equipment_status":
                    return new EquipmentStatusWidget(widgetPrefab, _lightSystems);
                    
                case "widget_plant_progress":
                    return new PlantProgressWidget(widgetPrefab, _plantManager);
                    
                default:
                    return new GenericUIWidget(widgetPrefab);
            }
        }
        
        #endregion
        
        #region Data Updates
        
        private void UpdateUIElements()
        {
            UpdateHUDComponents();
            UpdateActivePanels();
            UpdateActiveWidgets();
        }
        
        private void UpdateHUDComponents()
        {
            _environmentalHUD?.UpdateData(GetEnvironmentalData());
            _plantStatusHUD?.UpdateData(GetPlantStatusData());
            _systemPerformanceHUD?.UpdateData(GetSystemPerformanceData());
            _constructionHUD?.UpdateData(GetConstructionData());
            _economicHUD?.UpdateData(GetEconomicData());
        }
        
        private void UpdateActivePanels()
        {
            foreach (var panel in _activePanels.Values)
            {
                panel.UpdateData();
            }
        }
        
        private void UpdateActiveWidgets()
        {
            foreach (var widget in _activeWidgets.Values)
            {
                widget.UpdateData();
            }
        }
        
        private void RefreshAllUIData()
        {
            _dataManager?.RefreshAllData();
            UpdateUIElements();
        }
        
        #endregion
        
        #region Data Collection
        
        private EnvironmentalData GetEnvironmentalData()
        {
            if (_environmentalManager == null) return new EnvironmentalData();
            
            var conditions = _environmentalManager.GetCurrentConditions();
            return new EnvironmentalData
            {
                Temperature = conditions.Temperature,
                Humidity = conditions.Humidity,
                CO2Level = conditions.CO2Level,
                LightIntensity = conditions.LightIntensity,
                AirVelocity = conditions.AirVelocity,
                Timestamp = DateTime.Now
            };
        }
        
        private PlantStatusData GetPlantStatusData()
        {
            if (_plantManager == null) return new PlantStatusData();
            
            var plants = _plantManager.GetAllPlants();
            var data = new PlantStatusData
            {
                TotalPlants = plants.Count,
                HealthyPlants = plants.Count(p => p.Health > 70f),
                PlantsInVeg = plants.Count(p => p.CurrentStage == PlantGrowthStage.Vegetative),
                PlantsInFlower = plants.Count(p => p.CurrentStage == PlantGrowthStage.Flowering),
                ReadyToHarvest = plants.Count(p => p.CurrentStage == PlantGrowthStage.Harvest),
                AverageHealth = plants.Any() ? plants.Average(p => p.Health) : 0f
            };
            
            return data;
        }
        
        private SystemPerformanceData GetSystemPerformanceData()
        {
            var data = new SystemPerformanceData
            {
                TotalPowerConsumption = 0f,
                EnergyEfficiency = 1f,
                SystemLoad = 0.5f,
                ActiveAlerts = 0
            };
            
            // Calculate from lighting systems
            if (_lightSystems != null)
            {
                data.TotalPowerConsumption = _lightSystems.Sum(l => l.PowerConsumption);
                data.EnergyEfficiency = _lightSystems.Any() ? 
                    _lightSystems.Average(l => l.PerformanceMetrics.Efficiency) : 1f;
            }
            
            return data;
        }
        
        private ConstructionData GetConstructionData()
        {
            if (_facilityConstructor == null) return new ConstructionData();
            
            var metrics = _facilityConstructor.Metrics;
            return new ConstructionData
            {
                ActiveProjects = metrics.ActiveProjects,
                CompletedProjects = metrics.CompletedProjects,
                TotalValue = metrics.TotalValue,
                ActiveWorkers = metrics.ActiveWorkers,
                Efficiency = metrics.ConstructionEfficiency
            };
        }
        
        private EconomicData GetEconomicData()
        {
            if (_marketManager == null) return new EconomicData();
            
            return new EconomicData
            {
                TotalFunds = _marketManager.PlayerFunds,
                DailyRevenue = _marketManager.CalculateDailyRevenue(),
                DailyExpenses = _marketManager.CalculateDailyExpenses(),
                MarketTrends = _marketManager.GetMarketTrends()
            };
        }
        
        #endregion
        
        #region Event Handlers
        
        private void HandlePlantAdded(InteractivePlantComponent plant)
        {
            ShowNotification($"New plant added: {plant.PlantData.StrainName}", NotificationType.Info);
            RefreshPlantUI();
        }
        
        private void HandlePlantRemoved(InteractivePlantComponent plant)
        {
            ShowNotification($"Plant removed: {plant.PlantData.StrainName}", NotificationType.Info);
            RefreshPlantUI();
        }
        
        private void HandlePlantStageChanged(InteractivePlantComponent plant, PlantGrowthStage newStage)
        {
            ShowNotification($"{plant.PlantData.StrainName} entered {newStage} stage", NotificationType.Success);
            RefreshPlantUI();
        }
        
        private void HandleEnvironmentalChange(EnvironmentalConditions conditions)
        {
            _environmentalHUD?.UpdateData(GetEnvironmentalData());
        }
        
        private void HandleEnvironmentalAlert(string alertMessage)
        {
            ShowNotification(alertMessage, NotificationType.Warning);
        }
        
        private void HandleResearchCompleted(ResearchProjectSO research)
        {
            ShowNotification($"Research completed: {research.ProjectName}", NotificationType.Success);
        }
        
        private void HandleResearchStarted(ResearchProjectSO research)
        {
            ShowNotification($"Research started: {research.ProjectName}", NotificationType.Info);
        }
        
        private void HandleMarketUpdate(MarketData marketData)
        {
            _economicHUD?.UpdateData(GetEconomicData());
        }
        
        private void HandleTradeCompleted(TradeTransaction trade)
        {
            ShowNotification($"Trade completed: {trade.ProductName}", NotificationType.Success);
        }
        
        private void HandleConstructionStarted(ConstructionProject project)
        {
            ShowNotification($"Construction started: {project.ProjectName}", NotificationType.Info);
        }
        
        private void HandleConstructionCompleted(ConstructionProject project)
        {
            ShowNotification($"Construction completed: {project.ProjectName}", NotificationType.Success);
        }
        
        private void HandleConstructionIssue(string projectId, ConstructionIssue issue)
        {
            ShowNotification($"Construction issue: {issue.Description}", NotificationType.Warning);
        }
        
        #endregion
        
        #region UI State Management
        
        public void SetUIState(UIState newState)
        {
            if (_currentState == newState) return;
            
            var previousState = _currentState;
            _currentState = newState;
            
            HandleUIStateTransition(previousState, newState);
            OnUIStateChanged?.Invoke(newState);
            
            LogInfo($"UI State changed: {previousState} -> {newState}");
        }
        
        private void HandleUIStateTransition(UIState from, UIState to)
        {
            switch (to)
            {
                case UIState.Menu:
                    ShowMainMenu();
                    break;
                    
                case UIState.Game:
                    ShowGameInterface();
                    break;
                    
                case UIState.Paused:
                    ShowPauseOverlay();
                    break;
                    
                case UIState.Loading:
                    ShowLoadingScreen();
                    break;
            }
        }
        
        private void ShowMainMenu()
        {
            _hudContainer.style.display = DisplayStyle.None;
            OpenPanel("menu_main");
        }
        
        private void ShowGameInterface()
        {
            _hudContainer.style.display = DisplayStyle.Flex;
            ClosePanel("menu_main");
        }
        
        private void ShowPauseOverlay()
        {
            var pauseOverlay = new VisualElement();
            pauseOverlay.name = "pause-overlay";
            pauseOverlay.AddToClassList("pause-overlay");
            _overlayContainer.Add(pauseOverlay);
        }
        
        private void ShowLoadingScreen()
        {
            var loadingScreen = new VisualElement();
            loadingScreen.name = "loading-screen";
            loadingScreen.AddToClassList("loading-screen");
            _overlayContainer.Add(loadingScreen);
        }
        
        #endregion
        
        #region Notifications
        
        public void ShowNotification(string message, NotificationType type = NotificationType.Info, float duration = 3f)
        {
            var notification = CreateNotificationElement(message, type);
            _notificationContainer.Add(notification);
            
            if (_enableUIAnimations)
            {
                _animationController?.AnimateIn(notification);
            }
            
            // Auto-remove after duration
            StartCoroutine(RemoveNotificationAfterDelay(notification, duration));
        }
        
        private VisualElement CreateNotificationElement(string message, NotificationType type)
        {
            var notification = new VisualElement();
            notification.AddToClassList("notification");
            notification.AddToClassList($"notification--{type.ToString().ToLower()}");
            
            var messageLabel = new Label(message);
            messageLabel.AddToClassList("notification__message");
            notification.Add(messageLabel);
            
            var closeButton = new Button(() => RemoveNotification(notification));
            closeButton.text = "×";
            closeButton.AddToClassList("notification__close");
            notification.Add(closeButton);
            
            return notification;
        }
        
        private IEnumerator RemoveNotificationAfterDelay(VisualElement notification, float delay)
        {
            yield return new WaitForSeconds(delay);
            RemoveNotification(notification);
        }
        
        private void RemoveNotification(VisualElement notification)
        {
            if (_enableUIAnimations)
            {
                _animationController?.AnimateOut(notification, () => 
                {
                    _notificationContainer.Remove(notification);
                });
            }
            else
            {
                _notificationContainer.Remove(notification);
            }
        }
        
        #endregion
        
        #region Theming
        
        public void ApplyTheme(UITheme theme)
        {
            _activeTheme = theme;
            
            if (theme.ThemeStyleSheets != null)
            {
                foreach (var styleSheet in theme.ThemeStyleSheets)
                {
                    if (styleSheet != null)
                    {
                        _rootElement.styleSheets.Add(styleSheet);
                    }
                }
            }
            
            // Apply theme colors to root element
            _rootElement.style.color = theme.TextColor;
            _rootElement.style.backgroundColor = theme.BackgroundColor;
            
            LogInfo($"Applied UI theme: {theme.ThemeName}");
        }
        
        #endregion
        
        #region Utility Methods
        
        private void ProcessUIEvents()
        {
            _eventHandler?.ProcessPendingEvents();
        }
        
        private void UpdateAnimations()
        {
            _animationController?.Update();
        }
        
        private void RefreshPlantUI()
        {
            _plantStatusHUD?.UpdateData(GetPlantStatusData());
            
            if (_activePanels.ContainsKey("panel_plant_genetics"))
            {
                _activePanels["panel_plant_genetics"].UpdateData();
            }
        }
        
        #endregion
        
        #region Public Interface
        
        public void TogglePanel(string panelId)
        {
            if (_activePanels.ContainsKey(panelId))
            {
                ClosePanel(panelId);
            }
            else
            {
                OpenPanel(panelId);
            }
        }
        
        public bool IsPanelOpen(string panelId)
        {
            return _activePanels.ContainsKey(panelId);
        }
        
        public void CloseAllPanels()
        {
            var panelIds = _activePanels.Keys.ToList();
            foreach (var panelId in panelIds)
            {
                ClosePanel(panelId);
            }
        }
        
        public void SetHUDVisible(bool visible)
        {
            _hudContainer.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        public void PauseUI()
        {
            _isPaused = true;
            CancelInvoke(nameof(RefreshAllUIData));
        }
        
        public void ResumeUI()
        {
            _isPaused = false;
            if (_enableDynamicUI)
            {
                InvokeRepeating(nameof(RefreshAllUIData), 0f, _uiUpdateInterval);
            }
        }
        
        #endregion
        
        protected override void OnManagerShutdown()
        {
            // Disconnect event handlers
            DisconnectSystemEvents();
            
            // Cleanup UI elements
            CloseAllPanels();
            
            foreach (var widget in _activeWidgets.Values)
            {
                widget.Dispose();
            }
            _activeWidgets.Clear();
            
            // Cleanup managers
            _dataManager?.Dispose();
            _animationController?.Dispose();
            _accessibilityManager?.Dispose();
            
            CancelInvoke();
            
            LogInfo("Main Game UI Controller shutdown complete");
        }
        
        private void DisconnectSystemEvents()
        {
            if (_plantManager != null)
            {
                _plantManager.OnPlantAdded -= HandlePlantAdded;
                _plantManager.OnPlantRemoved -= HandlePlantRemoved;
                _plantManager.OnPlantStageChanged -= HandlePlantStageChanged;
            }
            
            if (_environmentalManager != null)
            {
                _environmentalManager.OnConditionsChanged -= HandleEnvironmentalChange;
                _environmentalManager.OnAlertTriggered -= HandleEnvironmentalAlert;
            }
            
            if (_researchManager != null)
            {
                _researchManager.OnResearchCompleted -= HandleResearchCompleted;
                _researchManager.OnResearchStarted -= HandleResearchStarted;
            }
            
            if (_marketManager != null)
            {
                _marketManager.OnMarketUpdate -= HandleMarketUpdate;
                _marketManager.OnTradeCompleted -= HandleTradeCompleted;
            }
            
            if (_facilityConstructor != null)
            {
                _facilityConstructor.OnProjectStarted -= HandleConstructionStarted;
                _facilityConstructor.OnProjectCompleted -= HandleConstructionCompleted;
                _facilityConstructor.OnConstructionIssue -= HandleConstructionIssue;
            }
        }
    }

    // Note: UIState and NotificationType enums are defined in UIManager.cs and GameUIManager.cs respectively
    // Data structures are defined in UIBaseClasses.cs
}