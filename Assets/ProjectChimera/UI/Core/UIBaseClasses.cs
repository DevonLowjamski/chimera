using UnityEngine;
using UnityEngine.UIElements;
using ProjectChimera.UI.Core;
using ProjectChimera.Systems.Construction;
using ProjectChimera.Systems.Cultivation;
using ProjectChimera.Systems.Environment;
using ProjectChimera.Systems.Progression;
using ProjectChimera.Systems.Economy;
using ProjectChimera.Data.Construction;
using System.Collections.Generic;
using System;

namespace ProjectChimera.UI.Core
{
    /// <summary>
    /// Base classes for UI panels, widgets, and HUD components.
    /// Provides common functionality for UI Toolkit integration.
    /// </summary>
    
    // Base UI Panel class
    public abstract class UIBasePanel : IDisposable
    {
        protected VisualElement _rootElement;
        protected UIPrefabEntry _prefabEntry;
        protected object _panelData;
        protected bool _isVisible = true;
        protected bool _isModal = false;
        
        public VisualElement RootElement => _rootElement;
        public bool IsVisible => _isVisible;
        public bool IsModal => _isModal;
        public string PanelId => _prefabEntry?.PrefabId ?? "";
        
        public virtual void Initialize(UIPrefabEntry prefabEntry, object panelData = null)
        {
            _prefabEntry = prefabEntry;
            _panelData = panelData;
            _isModal = prefabEntry?.UIProperties?.IsModal ?? false;
            
            CreatePanelElements();
            BindData();
        }
        
        protected abstract void CreatePanelElements();
        protected abstract void BindData();
        
        public virtual void UpdateData()
        {
            if (_isVisible)
            {
                RefreshDataBindings();
            }
        }
        
        protected virtual void RefreshDataBindings() { }
        
        public virtual void SetVisible(bool visible)
        {
            _isVisible = visible;
            _rootElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        public virtual void Dispose()
        {
            _rootElement?.RemoveFromHierarchy();
            _rootElement = null;
        }
    }
    
    // Base UI Widget class
    public abstract class UIWidget : IDisposable
    {
        protected VisualElement _rootElement;
        protected UIPrefabEntry _prefabEntry;
        protected bool _isVisible = true;
        protected float _updateFrequency = 1f;
        protected float _lastUpdate = 0f;
        
        public VisualElement RootElement => _rootElement;
        public bool IsVisible => _isVisible;
        public string WidgetId => _prefabEntry?.PrefabId ?? "";
        
        public virtual void Initialize(UIPrefabEntry prefabEntry)
        {
            _prefabEntry = prefabEntry;
            _updateFrequency = prefabEntry?.UIProperties?.UpdateFrequency ?? 1f;
            
            CreateWidgetElements();
            SetupInteractions();
        }
        
        protected abstract void CreateWidgetElements();
        protected virtual void SetupInteractions() { }
        
        public virtual void UpdateData()
        {
            if (_isVisible && Time.time - _lastUpdate >= _updateFrequency)
            {
                RefreshWidget();
                _lastUpdate = Time.time;
            }
        }
        
        protected abstract void RefreshWidget();
        
        public virtual void SetVisible(bool visible)
        {
            _isVisible = visible;
            _rootElement.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
        
        public virtual void Dispose()
        {
            _rootElement?.RemoveFromHierarchy();
            _rootElement = null;
        }
    }
    
    // Specific panel implementations
    public class FacilityManagementPanel : UIBasePanel
    {
        private InteractiveFacilityConstructor _facilityConstructor;
        private Label _projectCountLabel;
        private VisualElement _projectListContainer;
        private Button _newProjectButton;
        
        public FacilityManagementPanel(UIPrefabEntry prefabEntry, InteractiveFacilityConstructor facilityConstructor, object panelData = null)
        {
            _facilityConstructor = facilityConstructor;
            Initialize(prefabEntry, panelData);
        }
        
        protected override void CreatePanelElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "facility-management-panel";
            _rootElement.AddToClassList("ui-panel");
            _rootElement.AddToClassList("facility-panel");
            
            // Header
            var header = new VisualElement();
            header.AddToClassList("panel-header");
            var title = new Label("Facility Management");
            title.AddToClassList("panel-title");
            header.Add(title);
            _rootElement.Add(header);
            
            // Content
            var content = new VisualElement();
            content.AddToClassList("panel-content");
            
            // Project summary
            var summary = new VisualElement();
            summary.AddToClassList("project-summary");
            _projectCountLabel = new Label("Projects: 0");
            summary.Add(_projectCountLabel);
            content.Add(summary);
            
            // Project list
            _projectListContainer = new VisualElement();
            _projectListContainer.AddToClassList("project-list");
            content.Add(_projectListContainer);
            
            // Controls
            var controls = new VisualElement();
            controls.AddToClassList("panel-controls");
            _newProjectButton = new Button(() => CreateNewProject());
            _newProjectButton.text = "New Project";
            controls.Add(_newProjectButton);
            content.Add(controls);
            
            _rootElement.Add(content);
        }
        
        protected override void BindData()
        {
            if (_facilityConstructor != null)
            {
                RefreshDataBindings();
            }
        }
        
        protected override void RefreshDataBindings()
        {
            if (_facilityConstructor == null) return;
            
            var projects = _facilityConstructor.AllProjects;
            _projectCountLabel.text = $"Projects: {projects.Count}";
            
            _projectListContainer.Clear();
            foreach (var project in projects)
            {
                var projectElement = CreateProjectElement(project);
                _projectListContainer.Add(projectElement);
            }
        }
        
        private VisualElement CreateProjectElement(ConstructionProject project)
        {
            var element = new VisualElement();
            element.AddToClassList("project-item");
            
            var nameLabel = new Label(project.ProjectName);
            nameLabel.AddToClassList("project-name");
            element.Add(nameLabel);
            
            var statusLabel = new Label(project.Status.ToString());
            statusLabel.AddToClassList("project-status");
            element.Add(statusLabel);
            
            return element;
        }
        
        private void CreateNewProject()
        {
            // Implementation for creating new project
        }
    }
    
    public class PlantGeneticsPanel : UIBasePanel
    {
        private PlantManager _plantManager;
        
        public PlantGeneticsPanel(UIPrefabEntry prefabEntry, PlantManager plantManager, object panelData = null)
        {
            _plantManager = plantManager;
            Initialize(prefabEntry, panelData);
        }
        
        protected override void CreatePanelElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "plant-genetics-panel";
            _rootElement.AddToClassList("ui-panel");
            _rootElement.AddToClassList("genetics-panel");
            
            var title = new Label("Plant Genetics & Breeding");
            title.AddToClassList("panel-title");
            _rootElement.Add(title);
            
            // Genetics content would go here
        }
        
        protected override void BindData() { }
    }
    
    public class EnvironmentalControlPanel : UIBasePanel
    {
        private EnvironmentalManager _environmentalManager;
        private AdvancedGrowLightSystem[] _lightSystems;
        
        public EnvironmentalControlPanel(UIPrefabEntry prefabEntry, EnvironmentalManager environmentalManager, 
                                       AdvancedGrowLightSystem[] lightSystems, object panelData = null)
        {
            _environmentalManager = environmentalManager;
            _lightSystems = lightSystems;
            Initialize(prefabEntry, panelData);
        }
        
        protected override void CreatePanelElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "environmental-control-panel";
            _rootElement.AddToClassList("ui-panel");
            _rootElement.AddToClassList("environmental-panel");
            
            var title = new Label("Environmental Control");
            title.AddToClassList("panel-title");
            _rootElement.Add(title);
            
            // Environmental controls would go here
        }
        
        protected override void BindData() { }
    }
    
    public class ResearchDevelopmentPanel : UIBasePanel
    {
        private ResearchManager _researchManager;
        
        public ResearchDevelopmentPanel(UIPrefabEntry prefabEntry, ResearchManager researchManager, object panelData = null)
        {
            _researchManager = researchManager;
            Initialize(prefabEntry, panelData);
        }
        
        protected override void CreatePanelElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "research-development-panel";
            _rootElement.AddToClassList("ui-panel");
            _rootElement.AddToClassList("research-panel");
            
            var title = new Label("Research & Development");
            title.AddToClassList("panel-title");
            _rootElement.Add(title);
        }
        
        protected override void BindData() { }
    }
    
    public class MarketTradingPanel : UIBasePanel
    {
        private MarketManager _marketManager;
        
        public MarketTradingPanel(UIPrefabEntry prefabEntry, MarketManager marketManager, object panelData = null)
        {
            _marketManager = marketManager;
            Initialize(prefabEntry, panelData);
        }
        
        protected override void CreatePanelElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "market-trading-panel";
            _rootElement.AddToClassList("ui-panel");
            _rootElement.AddToClassList("market-panel");
            
            var title = new Label("Market & Trading");
            title.AddToClassList("panel-title");
            _rootElement.Add(title);
        }
        
        protected override void BindData() { }
    }
    
    public class GenericUIPanel : UIBasePanel
    {
        public GenericUIPanel(UIPrefabEntry prefabEntry, object panelData = null)
        {
            Initialize(prefabEntry, panelData);
        }
        
        protected override void CreatePanelElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = $"generic-panel-{_prefabEntry.PrefabId}";
            _rootElement.AddToClassList("ui-panel");
            _rootElement.AddToClassList("generic-panel");
            
            var title = new Label(_prefabEntry.PrefabName);
            title.AddToClassList("panel-title");
            _rootElement.Add(title);
        }
        
        protected override void BindData() { }
    }
    
    // Widget implementations
    public class RealtimeChartWidget : UIWidget
    {
        private VisualElement _chartContainer;
        private Label _valueLabel;
        
        public RealtimeChartWidget(UIPrefabEntry prefabEntry)
        {
            Initialize(prefabEntry);
        }
        
        protected override void CreateWidgetElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "realtime-chart-widget";
            _rootElement.AddToClassList("ui-widget");
            _rootElement.AddToClassList("chart-widget");
            
            var title = new Label("Real-time Chart");
            title.AddToClassList("widget-title");
            _rootElement.Add(title);
            
            _chartContainer = new VisualElement();
            _chartContainer.AddToClassList("chart-container");
            _rootElement.Add(_chartContainer);
            
            _valueLabel = new Label("--");
            _valueLabel.AddToClassList("chart-value");
            _rootElement.Add(_valueLabel);
        }
        
        protected override void RefreshWidget()
        {
            // Update chart data
            _valueLabel.text = UnityEngine.Random.Range(0f, 100f).ToString("F1");
        }
    }
    
    public class EquipmentStatusWidget : UIWidget
    {
        private AdvancedGrowLightSystem[] _lightSystems;
        private VisualElement _equipmentList;
        
        public EquipmentStatusWidget(UIPrefabEntry prefabEntry, AdvancedGrowLightSystem[] lightSystems)
        {
            _lightSystems = lightSystems;
            Initialize(prefabEntry);
        }
        
        protected override void CreateWidgetElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "equipment-status-widget";
            _rootElement.AddToClassList("ui-widget");
            _rootElement.AddToClassList("equipment-widget");
            
            var title = new Label("Equipment Status");
            title.AddToClassList("widget-title");
            _rootElement.Add(title);
            
            _equipmentList = new VisualElement();
            _equipmentList.AddToClassList("equipment-list");
            _rootElement.Add(_equipmentList);
        }
        
        protected override void RefreshWidget()
        {
            _equipmentList.Clear();
            
            if (_lightSystems != null)
            {
                foreach (var light in _lightSystems)
                {
                    if (light != null)
                    {
                        var item = CreateEquipmentItem(light.name, light.IsOn ? "On" : "Off", light.PowerConsumption);
                        _equipmentList.Add(item);
                    }
                }
            }
        }
        
        private VisualElement CreateEquipmentItem(string name, string status, float power)
        {
            var item = new VisualElement();
            item.AddToClassList("equipment-item");
            
            var nameLabel = new Label(name);
            nameLabel.AddToClassList("equipment-name");
            item.Add(nameLabel);
            
            var statusLabel = new Label(status);
            statusLabel.AddToClassList("equipment-status");
            item.Add(statusLabel);
            
            var powerLabel = new Label($"{power:F0}W");
            powerLabel.AddToClassList("equipment-power");
            item.Add(powerLabel);
            
            return item;
        }
    }
    
    public class PlantProgressWidget : UIWidget
    {
        private PlantManager _plantManager;
        private Label _progressLabel;
        private ProgressBar _progressBar;
        
        public PlantProgressWidget(UIPrefabEntry prefabEntry, PlantManager plantManager)
        {
            _plantManager = plantManager;
            Initialize(prefabEntry);
        }
        
        protected override void CreateWidgetElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "plant-progress-widget";
            _rootElement.AddToClassList("ui-widget");
            _rootElement.AddToClassList("progress-widget");
            
            var title = new Label("Plant Progress");
            title.AddToClassList("widget-title");
            _rootElement.Add(title);
            
            _progressLabel = new Label("Growth Progress");
            _progressLabel.AddToClassList("progress-label");
            _rootElement.Add(_progressLabel);
            
            _progressBar = new ProgressBar();
            _progressBar.AddToClassList("progress-bar");
            _progressBar.lowValue = 0f;
            _progressBar.highValue = 100f;
            _rootElement.Add(_progressBar);
        }
        
        protected override void RefreshWidget()
        {
            if (_plantManager != null)
            {
                var plants = _plantManager.GetAllPlants();
                if (plants.Count > 0)
                {
                    float avgProgress = plants.Average(p => p.GrowthProgress * 100f);
                    _progressBar.value = avgProgress;
                    _progressLabel.text = $"Average Progress: {avgProgress:F0}%";
                }
                else
                {
                    _progressBar.value = 0f;
                    _progressLabel.text = "No plants";
                }
            }
        }
    }
    
    public class GenericUIWidget : UIWidget
    {
        public GenericUIWidget(UIPrefabEntry prefabEntry)
        {
            Initialize(prefabEntry);
        }
        
        protected override void CreateWidgetElements()
        {
            _rootElement = new VisualElement();
            _rootElement.name = $"generic-widget-{_prefabEntry.PrefabId}";
            _rootElement.AddToClassList("ui-widget");
            _rootElement.AddToClassList("generic-widget");
            
            var title = new Label(_prefabEntry.PrefabName);
            title.AddToClassList("widget-title");
            _rootElement.Add(title);
        }
        
        protected override void RefreshWidget() { }
    }
    
    // HUD Components
    public class SystemPerformanceHUD : IDataBinding
    {
        private VisualElement _rootElement;
        private Label _powerConsumptionLabel;
        private Label _efficiencyLabel;
        private Label _systemLoadLabel;
        private Label _alertsLabel;

        public VisualElement CreateHUDElement()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "system-performance-hud";
            _rootElement.AddToClassList("hud-element");
            _rootElement.AddToClassList("performance-hud");

            var title = new Label("System Performance");
            title.AddToClassList("hud-title");
            _rootElement.Add(title);

            var content = new VisualElement();
            content.AddToClassList("hud-content");

            _powerConsumptionLabel = new Label("Power: 0 kW");
            _powerConsumptionLabel.AddToClassList("hud-value");
            content.Add(_powerConsumptionLabel);

            _efficiencyLabel = new Label("Efficiency: 0%");
            _efficiencyLabel.AddToClassList("hud-value");
            content.Add(_efficiencyLabel);

            _systemLoadLabel = new Label("Load: 0%");
            _systemLoadLabel.AddToClassList("hud-value");
            content.Add(_systemLoadLabel);

            _alertsLabel = new Label("Alerts: 0");
            _alertsLabel.AddToClassList("hud-value");
            content.Add(_alertsLabel);

            _rootElement.Add(content);
            return _rootElement;
        }

        public void UpdateData(object data)
        {
            if (data is SystemPerformanceData perfData)
            {
                _powerConsumptionLabel.text = $"Power: {perfData.TotalPowerConsumption:F1} kW";
                _efficiencyLabel.text = $"Efficiency: {perfData.EnergyEfficiency:F1}%";
                _systemLoadLabel.text = $"Load: {perfData.SystemLoad:F1}%";
                _alertsLabel.text = $"Alerts: {perfData.ActiveAlerts}";
            }
        }
    }
    
    public class ConstructionProgressHUD : IDataBinding
    {
        private VisualElement _rootElement;
        private Label _activeProjectsLabel;
        private Label _completedProjectsLabel;
        private Label _totalValueLabel;
        private Label _activeWorkersLabel;
        private Label _efficiencyLabel;

        public VisualElement CreateHUDElement()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "construction-progress-hud";
            _rootElement.AddToClassList("hud-element");
            _rootElement.AddToClassList("construction-hud");

            var title = new Label("Construction Progress");
            title.AddToClassList("hud-title");
            _rootElement.Add(title);

            var content = new VisualElement();
            content.AddToClassList("hud-content");

            _activeProjectsLabel = new Label("Active: 0");
            _activeProjectsLabel.AddToClassList("hud-value");
            content.Add(_activeProjectsLabel);

            _completedProjectsLabel = new Label("Completed: 0");
            _completedProjectsLabel.AddToClassList("hud-value");
            content.Add(_completedProjectsLabel);

            _totalValueLabel = new Label("Value: $0");
            _totalValueLabel.AddToClassList("hud-value");
            content.Add(_totalValueLabel);

            _activeWorkersLabel = new Label("Workers: 0");
            _activeWorkersLabel.AddToClassList("hud-value");
            content.Add(_activeWorkersLabel);

            _efficiencyLabel = new Label("Efficiency: 0%");
            _efficiencyLabel.AddToClassList("hud-value");
            content.Add(_efficiencyLabel);

            _rootElement.Add(content);
            return _rootElement;
        }

        public void UpdateData(object data)
        {
            if (data is ConstructionData constructionData)
            {
                _activeProjectsLabel.text = $"Active: {constructionData.ActiveProjects}";
                _completedProjectsLabel.text = $"Completed: {constructionData.CompletedProjects}";
                _totalValueLabel.text = $"Value: ${constructionData.TotalValue:F0}";
                _activeWorkersLabel.text = $"Workers: {constructionData.ActiveWorkers}";
                _efficiencyLabel.text = $"Efficiency: {constructionData.Efficiency:F1}%";
            }
        }
    }
    
    public class EconomicOverviewHUD : IDataBinding
    {
        private VisualElement _rootElement;
        private Label _fundsLabel;
        private Label _revenueLabel;
        private Label _expensesLabel;
        private Label _profitLabel;

        public VisualElement CreateHUDElement()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "economic-overview-hud";
            _rootElement.AddToClassList("hud-element");
            _rootElement.AddToClassList("economic-hud");

            var title = new Label("Economic Overview");
            title.AddToClassList("hud-title");
            _rootElement.Add(title);

            var content = new VisualElement();
            content.AddToClassList("hud-content");

            _fundsLabel = new Label("Funds: $0");
            _fundsLabel.AddToClassList("hud-value");
            content.Add(_fundsLabel);

            _revenueLabel = new Label("Revenue: $0/day");
            _revenueLabel.AddToClassList("hud-value");
            content.Add(_revenueLabel);

            _expensesLabel = new Label("Expenses: $0/day");
            _expensesLabel.AddToClassList("hud-value");
            content.Add(_expensesLabel);

            _profitLabel = new Label("Profit: $0/day");
            _profitLabel.AddToClassList("hud-value");
            content.Add(_profitLabel);

            _rootElement.Add(content);
            return _rootElement;
        }

        public void UpdateData(object data)
        {
            if (data is EconomicData economicData)
            {
                _fundsLabel.text = $"Funds: ${economicData.TotalFunds:F2}";
                _revenueLabel.text = $"Revenue: ${economicData.DailyRevenue:F2}/day";
                _expensesLabel.text = $"Expenses: ${economicData.DailyExpenses:F2}/day";
                var profit = economicData.DailyRevenue - economicData.DailyExpenses;
                _profitLabel.text = $"Profit: ${profit:F2}/day";
            }
        }
    }

    public class EnvironmentalStatusHUD : IDataBinding
    {
        private VisualElement _rootElement;
        private Label _temperatureLabel;
        private Label _humidityLabel;
        private Label _co2Label;
        private Label _lightLabel;

        public VisualElement CreateHUDElement()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "environmental-status-hud";
            _rootElement.AddToClassList("hud-element");
            _rootElement.AddToClassList("environmental-hud");

            var title = new Label("Environmental Status");
            title.AddToClassList("hud-title");
            _rootElement.Add(title);

            var content = new VisualElement();
            content.AddToClassList("hud-content");

            _temperatureLabel = new Label("Temperature: 0°C");
            _temperatureLabel.AddToClassList("hud-value");
            content.Add(_temperatureLabel);

            _humidityLabel = new Label("Humidity: 0%");
            _humidityLabel.AddToClassList("hud-value");
            content.Add(_humidityLabel);

            _co2Label = new Label("CO2: 0 ppm");
            _co2Label.AddToClassList("hud-value");
            content.Add(_co2Label);

            _lightLabel = new Label("Light: 0 PPFD");
            _lightLabel.AddToClassList("hud-value");
            content.Add(_lightLabel);

            _rootElement.Add(content);
            return _rootElement;
        }

        public void UpdateData(object data)
        {
            if (data is EnvironmentalData envData)
            {
                _temperatureLabel.text = $"Temperature: {envData.Temperature:F1}°C";
                _humidityLabel.text = $"Humidity: {envData.Humidity:F1}%";
                _co2Label.text = $"CO2: {envData.CO2Level:F0} ppm";
                _lightLabel.text = $"Light: {envData.LightIntensity:F0} PPFD";
            }
        }
    }

    public class PlantStatusHUD : IDataBinding
    {
        private VisualElement _rootElement;
        private Label _totalPlantsLabel;
        private Label _healthyPlantsLabel;
        private Label _vegPlantsLabel;
        private Label _flowerPlantsLabel;
        private Label _readyToHarvestLabel;

        public VisualElement CreateHUDElement()
        {
            _rootElement = new VisualElement();
            _rootElement.name = "plant-status-hud";
            _rootElement.AddToClassList("hud-element");
            _rootElement.AddToClassList("plant-hud");

            var title = new Label("Plant Status");
            title.AddToClassList("hud-title");
            _rootElement.Add(title);

            var content = new VisualElement();
            content.AddToClassList("hud-content");

            _totalPlantsLabel = new Label("Total Plants: 0");
            _totalPlantsLabel.AddToClassList("hud-value");
            content.Add(_totalPlantsLabel);

            _healthyPlantsLabel = new Label("Healthy: 0");
            _healthyPlantsLabel.AddToClassList("hud-value");
            content.Add(_healthyPlantsLabel);

            _vegPlantsLabel = new Label("Vegetative: 0");
            _vegPlantsLabel.AddToClassList("hud-value");
            content.Add(_vegPlantsLabel);

            _flowerPlantsLabel = new Label("Flowering: 0");
            _flowerPlantsLabel.AddToClassList("hud-value");
            content.Add(_flowerPlantsLabel);

            _readyToHarvestLabel = new Label("Ready to Harvest: 0");
            _readyToHarvestLabel.AddToClassList("hud-value");
            content.Add(_readyToHarvestLabel);

            _rootElement.Add(content);
            return _rootElement;
        }

        public void UpdateData(object data)
        {
            if (data is PlantStatusData plantData)
            {
                _totalPlantsLabel.text = $"Total Plants: {plantData.TotalPlants}";
                _healthyPlantsLabel.text = $"Healthy: {plantData.HealthyPlants}";
                _vegPlantsLabel.text = $"Vegetative: {plantData.PlantsInVeg}";
                _flowerPlantsLabel.text = $"Flowering: {plantData.PlantsInFlower}";
                _readyToHarvestLabel.text = $"Ready to Harvest: {plantData.ReadyToHarvest}";
            }
        }
    }

    // Interface for data binding
    public interface IDataBinding
    {
        void UpdateData(object data);
    }

    // UI Support Classes
    public class UIDataManager : IDisposable
    {
        private Dictionary<string, object> _cachedData = new Dictionary<string, object>();
        
        public void CacheData(string key, object data)
        {
            _cachedData[key] = data;
        }
        
        public T GetCachedData<T>(string key) where T : class
        {
            return _cachedData.TryGetValue(key, out var data) ? data as T : null;
        }
        
        public void ClearCache()
        {
            _cachedData.Clear();
        }
        
        public void Dispose()
        {
            ClearCache();
        }
    }

    public class UIEventHandler
    {
        private Queue<System.Action> _pendingEvents = new Queue<System.Action>();
        
        public void QueueEvent(System.Action eventAction)
        {
            _pendingEvents.Enqueue(eventAction);
        }
        
        public void ProcessPendingEvents()
        {
            while (_pendingEvents.Count > 0)
            {
                var eventAction = _pendingEvents.Dequeue();
                try
                {
                    eventAction?.Invoke();
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error processing UI event: {ex.Message}");
                }
            }
        }
    }

    public class UIAnimationController : IDisposable
    {
        private List<UIAnimation> _activeAnimations = new List<UIAnimation>();
        
        public void StartAnimation(UIAnimation animation)
        {
            _activeAnimations.Add(animation);
        }
        
        public void Update()
        {
            for (int i = _activeAnimations.Count - 1; i >= 0; i--)
            {
                var animation = _activeAnimations[i];
                animation.Update(Time.deltaTime);
                
                if (animation.IsComplete)
                {
                    _activeAnimations.RemoveAt(i);
                }
            }
        }
        
        public void Dispose()
        {
            _activeAnimations.Clear();
        }
    }

    public class UIAccessibilityManager : IDisposable
    {
        public void ApplyAccessibilitySettings(VisualElement element)
        {
            // Add accessibility features like screen reader support, high contrast, etc.
            element.focusable = true;
        }
        
        public void Dispose()
        {
            // Cleanup accessibility resources
        }
    }

    public class UIAnimation
    {
        public bool IsComplete { get; private set; }
        
        public void Update(float deltaTime)
        {
            // Animation update logic
            IsComplete = true; // Placeholder
        }
    }

    // Data structures referenced in MainGameUIController
    public class EnvironmentalData
    {
        public float Temperature;
        public float Humidity;
        public float CO2Level;
        public float LightIntensity;
        public float AirVelocity;
        public System.DateTime Timestamp;
    }

    public class PlantStatusData
    {
        public int TotalPlants;
        public int HealthyPlants;
        public int PlantsInVeg;
        public int PlantsInFlower;
        public int ReadyToHarvest;
        public float AverageHealth;
    }

    public class SystemPerformanceData
    {
        public float TotalPowerConsumption;
        public float EnergyEfficiency;
        public float SystemLoad;
        public int ActiveAlerts;
    }

    public class ConstructionData
    {
        public int ActiveProjects;
        public int CompletedProjects;
        public float TotalValue;
        public int ActiveWorkers;
        public float Efficiency;
    }

    public class EconomicData
    {
        public float TotalFunds;
        public float DailyRevenue;
        public float DailyExpenses;
        public MarketTrends MarketTrends;
    }

    public class MarketTrends
    {
        public float PriceIndex;
        public float DemandLevel;
        public float SupplyLevel;
        public float Volatility;
        public string TrendDirection; // "Up", "Down", "Stable"
    }
}