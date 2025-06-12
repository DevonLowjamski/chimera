# Project Chimera - API Reference

## 📚 Comprehensive API Documentation

This reference provides detailed information about all public APIs, interfaces, and extension points in Project Chimera. Use this guide for development, modding, and integration purposes.

---

## 🔧 Core Manager APIs

### GameManager
**Central coordination singleton for all game systems**

```csharp
public class GameManager : ChimeraManager
{
    public static GameManager Instance { get; }
    public GameState CurrentGameState { get; }
    public bool IsGamePaused { get; }
    public System.DateTime GameStartTime { get; }
    public System.TimeSpan TotalGameTime { get; }
    
    // Manager access
    public T GetManager<T>() where T : ChimeraManager;
    public void RegisterManager<T>(T manager) where T : ChimeraManager;
    
    // Game control
    public void PauseGame();
    public void ResumeGame();
    public void ManualSave();
}
```

**Usage Examples**:
```csharp
// Access any manager
var plantManager = GameManager.Instance.GetManager<PlantManager>();
var economyManager = GameManager.Instance.GetManager<EconomyManager>();

// Game state management
if (GameManager.Instance.CurrentGameState == GameState.Running)
{
    // Perform game operations
}

// Pause handling
GameManager.Instance.PauseGame();
```

### TimeManager
**Game time control and offline progression**

```csharp
public class TimeManager : ChimeraManager
{
    public float TimeScale { get; set; }
    public System.DateTime GameTime { get; }
    public System.TimeSpan SessionDuration { get; }
    public bool IsOfflineProgressionEnabled { get; set; }
    
    // Time utilities
    public float GetScaledDeltaTime();
    public System.DateTime ConvertToGameTime(System.DateTime realTime);
    public void SetTimeScale(float scale);
    public void ProcessOfflineTime(System.TimeSpan offlineTime);
}
```

**Usage Examples**:
```csharp
// Get scaled time for calculations
float deltaTime = _timeManager.GetScaledDeltaTime();

// Time acceleration
_timeManager.SetTimeScale(2.0f); // 2x speed

// Offline progression
_timeManager.ProcessOfflineTime(System.TimeSpan.FromHours(8));
```

---

## 🌱 Cultivation System APIs

### PlantManager
**Core plant lifecycle management**

```csharp
public class PlantManager : ChimeraManager
{
    public int ActivePlantCount { get; }
    public float GlobalGrowthModifier { get; set; }
    
    // Plant creation and management
    public PlantInstance CreatePlant(PlantStrainSO strain, Vector3 position, Transform parent = null);
    public List<PlantInstance> CreatePlants(PlantStrainSO strain, List<Vector3> positions, Transform parent = null);
    public void RegisterPlant(PlantInstance plant);
    public void UnregisterPlant(string plantID, PlantRemovalReason reason = PlantRemovalReason.Other);
    
    // Plant access
    public PlantInstance GetPlant(string plantID);
    public List<PlantInstance> GetPlantsInStage(PlantGrowthStage stage);
    public List<PlantInstance> GetHarvestablePlants();
    public List<PlantInstance> GetPlantsNeedingAttention();
    
    // Operations
    public void UpdateEnvironmentalConditions(EnvironmentalConditions newConditions);
    public void ApplyEnvironmentalStress(EnvironmentalStressSO stressSource, float intensity);
    public HarvestResults HarvestPlant(string plantID);
    public PlantManagerStatistics GetStatistics();
}
```

### PlantInstance
**Individual plant state and behavior**

```csharp
public class PlantInstance : MonoBehaviour
{
    public string PlantID { get; }
    public string PlantName { get; set; }
    public PlantStrainSO Strain { get; }
    public PlantGrowthStage CurrentGrowthStage { get; }
    public float CurrentHealth { get; }
    public float StressLevel { get; }
    public float GrowthProgress { get; }
    public bool IsActive { get; }
    
    // Events
    public event System.Action<PlantInstance> OnGrowthStageChanged;
    public event System.Action<PlantInstance> OnHealthChanged;
    public event System.Action<PlantInstance> OnPlantDied;
    
    // Operations
    public void UpdateEnvironmentalConditions(EnvironmentalConditions conditions);
    public bool ApplyStress(EnvironmentalStressSO stressSource, float intensity);
    public HarvestResults Harvest();
    public bool HasActiveStressors();
    
    // Static factory
    public static PlantInstance CreateFromStrain(PlantStrainSO strain, Vector3 position, Transform parent = null);
}
```

**Usage Examples**:
```csharp
// Create and manage plants
var strain = Resources.Load<PlantStrainSO>("Strains/WhiteWidow");
var plant = _plantManager.CreatePlant(strain, Vector3.zero);

// Monitor plant health
plant.OnHealthChanged += (p) => Debug.Log($"Plant {p.PlantName} health: {p.CurrentHealth}");

// Harvest when ready
if (plant.CurrentGrowthStage == PlantGrowthStage.Harvest)
{
    var results = _plantManager.HarvestPlant(plant.PlantID);
    Debug.Log($"Harvested {results.TotalYieldGrams}g with quality {results.QualityScore}");
}
```

---

## 🧬 Genetics System APIs

### GeneticsManager
**Breeding mechanics and trait inheritance**

```csharp
public class GeneticsManager : ChimeraManager
{
    // Breeding operations
    public BreedingResults BreedPlants(PlantInstance parent1, PlantInstance parent2);
    public PlantStrainSO CreateOffspringStrain(PlantStrainSO parent1, PlantStrainSO parent2, string offspringName);
    public GeneticPrediction PredictOffspring(PlantStrainSO parent1, PlantStrainSO parent2);
    
    // Trait analysis
    public List<GeneticTrait> AnalyzeGenotype(PlantStrainSO strain);
    public float CalculateTraitExpression(GeneticTrait trait, EnvironmentalConditions conditions);
    public Dictionary<string, float> GetPhenotypeProfile(PlantInstance plant);
    
    // Strain management
    public void RegisterCustomStrain(PlantStrainSO strain);
    public List<PlantStrainSO> GetAvailableStrains();
    public PlantStrainSO GetStrainByName(string strainName);
}
```

### PlantStrainSO
**Genetic data structure**

```csharp
[CreateAssetMenu(fileName = "New Plant Strain", menuName = "Project Chimera/Genetics/Plant Strain")]
public class PlantStrainSO : ChimeraDataSO
{
    [Header("Basic Information")]
    public string StrainName;
    public string Description;
    public StrainType Type; // Indica, Sativa, Hybrid
    public Sprite StrainImage;
    
    [Header("Genetic Traits")]
    public GeneticTraitRange ThcContent;
    public GeneticTraitRange CbdContent;
    public GeneticTraitRange YieldPotential;
    public GeneticTraitRange FloweringTime;
    public TerpeneProfile TerpeneProfile;
    
    [Header("Environmental Preferences")]
    public EnvironmentalTolerances EnvironmentalTolerance;
    public DiseaseResistance DiseaseResistance;
    public NutrientRequirements NutrientNeeds;
    
    // Validation and utilities
    public override bool ValidateDataSpecific();
    public float GetTraitValue(string traitName);
    public bool IsCompatibleWith(PlantStrainSO otherStrain);
}
```

**Usage Examples**:
```csharp
// Breed two plants
var parent1 = _plantManager.GetPlant("plant_001");
var parent2 = _plantManager.GetPlant("plant_002");
var breedingResults = _geneticsManager.BreedPlants(parent1, parent2);

// Create new strain from breeding
var offspringStrain = _geneticsManager.CreateOffspringStrain(
    parent1.Strain, 
    parent2.Strain, 
    "Custom Hybrid #1"
);

// Analyze genetics
var traits = _geneticsManager.AnalyzeGenotype(strain);
foreach (var trait in traits)
{
    Debug.Log($"Trait: {trait.Name}, Value: {trait.Expression}");
}
```

---

## 🏭 Environmental System APIs

### EnvironmentalManager
**Climate control and monitoring**

```csharp
public class EnvironmentalManager : ChimeraManager
{
    public EnvironmentalConditions CurrentConditions { get; }
    public bool AutomationEnabled { get; set; }
    public float EnergyEfficiency { get; }
    
    // Environmental control
    public void SetTargetConditions(EnvironmentalConditions targets);
    public void UpdateConditions(EnvironmentalConditions newConditions);
    public void ApplyEnvironmentalStress(EnvironmentalStressSO stress, float intensity);
    
    // Equipment management
    public void RegisterEquipment(EnvironmentalEquipmentSO equipment);
    public void UnregisterEquipment(string equipmentId);
    public List<EnvironmentalEquipmentSO> GetActiveEquipment();
    
    // Monitoring
    public EnvironmentalHistory GetHistoricalData(System.TimeSpan timeRange);
    public EnvironmentalForecast GetForecast(System.TimeSpan duration);
    public List<EnvironmentalAlert> GetActiveAlerts();
}
```

### EnvironmentalConditions
**Environmental state data structure**

```csharp
[System.Serializable]
public class EnvironmentalConditions
{
    [Header("Climate Parameters")]
    [Range(10f, 40f)] public float Temperature = 24f;
    [Range(20f, 80f)] public float Humidity = 60f;
    [Range(300f, 1500f)] public float CO2Level = 400f;
    
    [Header("Lighting")]
    [Range(0f, 2000f)] public float PPFD = 800f;
    [Range(10f, 18f)] public float DailyLightIntegral = 14f;
    public LightSpectrum LightSpectrum;
    
    [Header("Air Quality")]
    [Range(0f, 10f)] public float AirFlow = 5f;
    [Range(0f, 100f)] public float AirPurity = 95f;
    
    // Utilities
    public bool IsWithinOptimalRange(PlantStrainSO strain);
    public float CalculateStressLevel(PlantStrainSO strain);
    public EnvironmentalConditions Lerp(EnvironmentalConditions target, float t);
}
```

**Usage Examples**:
```csharp
// Set environmental targets
var targetConditions = new EnvironmentalConditions
{
    Temperature = 26f,
    Humidity = 65f,
    CO2Level = 800f,
    PPFD = 900f
};
_environmentalManager.SetTargetConditions(targetConditions);

// Monitor conditions
var currentConditions = _environmentalManager.CurrentConditions;
if (!currentConditions.IsWithinOptimalRange(plantStrain))
{
    Debug.LogWarning("Environmental conditions suboptimal for strain");
}
```

---

## 💰 Economic System APIs

### MarketManager
**Market simulation and trading**

```csharp
public class MarketManager : ChimeraManager
{
    public MarketConditions CurrentMarketConditions { get; }
    public PlayerPortfolio PlayerPortfolio { get; }
    public float PlayerReputation { get; }
    
    // Trading operations
    public TransactionResult ProcessSale(ProductSO product, float quantity, float quality, bool isContractSale);
    public TransactionResult ProcessPurchase(ProductSO product, float quantity, float maxPrice);
    public List<MarketOpportunity> GetAvailableOpportunities();
    
    // Market analysis
    public MarketData GetMarketData(ProductSO product);
    public PriceHistory GetPriceHistory(ProductSO product, System.TimeSpan timeRange);
    public MarketForecast GetMarketForecast(ProductSO product);
    
    // Portfolio management
    public PortfolioMetrics GetPortfolioMetrics();
    public FinancialData GetFinancialData();
    public void UpdatePlayerReputation(float change, string reason);
}
```

### EconomyManager
**Financial management and investments**

```csharp
public class EconomyManager : ChimeraManager
{
    public float PlayerCash { get; }
    public float NetWorth { get; }
    public float MonthlyRevenue { get; }
    public float MonthlyExpenses { get; }
    
    // Financial operations
    public bool ProcessTransaction(float amount, string description, TransactionType type);
    public void AddRecurringExpense(string name, float amount, RecurrenceType frequency);
    public void RemoveRecurringExpense(string name);
    
    // Investment management
    public List<InvestmentOpportunity> GetAvailableInvestments();
    public InvestmentResult MakeInvestment(InvestmentOpportunity opportunity, float amount);
    public List<ActiveInvestment> GetActiveInvestments();
    
    // Financial reporting
    public FinancialReport GenerateFinancialReport(System.TimeSpan period);
    public CashFlowProjection ProjectCashFlow(System.TimeSpan duration);
    public List<FinancialAlert> GetFinancialAlerts();
}
```

**Usage Examples**:
```csharp
// Sell harvested product
var harvestResults = _plantManager.HarvestPlant(plantId);
var product = Resources.Load<ProductSO>("Products/DriedFlower");
var saleResult = _marketManager.ProcessSale(product, harvestResults.TotalYieldGrams, harvestResults.QualityScore, false);

// Check financial status
var financialData = _economyManager.GetFinancialData();
Debug.Log($"Cash: ${financialData.CurrentCash:F2}, Net Worth: ${financialData.NetWorth:F2}");

// Make investment
var investments = _economyManager.GetAvailableInvestments();
var bestInvestment = investments.OrderByDescending(i => i.ExpectedROI).First();
_economyManager.MakeInvestment(bestInvestment, 10000f);
```

---

## 🤖 Automation & AI APIs

### AutomationManager
**IoT integration and intelligent control**

```csharp
public class AutomationManager : ChimeraManager
{
    public bool AutomationEnabled { get; set; }
    public int ConnectedDevices { get; }
    public float SystemEfficiency { get; }
    
    // Device management
    public void RegisterDevice(IoTDeviceSO device);
    public void UnregisterDevice(string deviceId);
    public List<IoTDeviceSO> GetConnectedDevices();
    public IoTDeviceSO GetDevice(string deviceId);
    
    // Automation schedules
    public void CreateSchedule(AutomationScheduleSO schedule);
    public void UpdateSchedule(string scheduleId, AutomationScheduleSO newSchedule);
    public void DeleteSchedule(string scheduleId);
    public List<AutomationScheduleSO> GetActiveSchedules();
    
    // Control operations
    public void ExecuteAutomationRule(AutomationRuleSO rule);
    public void TriggerEmergencyShutdown(string reason);
    public AutomationStatus GetSystemStatus();
}
```

### AIAdvisorManager
**Intelligent recommendations and optimization**

```csharp
public class AIAdvisorManager : ChimeraManager
{
    public bool AIEnabled { get; set; }
    public float ConfidenceLevel { get; }
    public int RecommendationsGenerated { get; }
    
    // AI analysis
    public void ProcessUserQuery(string query, System.Action<string> responseCallback);
    public FacilityAnalysis AnalyzeFacilityState();
    public List<AIRecommendation> GenerateRecommendations();
    public PredictionResults GeneratePredictions();
    
    // Optimization
    public OptimizationSuggestions AnalyzeOptimizationOpportunities();
    public void ApplyOptimizationSuggestion(OptimizationSuggestions suggestion);
    public PerformanceMetrics GetAIPerformanceMetrics();
    
    // Learning system
    public void ProvideFeedback(string recommendationId, FeedbackType feedback);
    public void UpdateLearningModel(UserActionData actionData);
    public AILearningProgress GetLearningProgress();
}
```

**Usage Examples**:
```csharp
// Set up automation
var schedule = ScriptableObject.CreateInstance<AutomationScheduleSO>();
schedule.DeviceId = "hvac_001";
schedule.TriggerConditions.Add(new TemperatureTrigger { Threshold = 28f });
schedule.Actions.Add(new CoolingAction { Intensity = 0.8f });
_automationManager.CreateSchedule(schedule);

// Get AI recommendations
var recommendations = _aiAdvisorManager.GenerateRecommendations();
foreach (var rec in recommendations)
{
    Debug.Log($"AI Recommendation: {rec.Title} - {rec.Description}");
}

// Process natural language query
_aiAdvisorManager.ProcessUserQuery("What's the optimal temperature for flowering?", response =>
{
    Debug.Log($"AI Response: {response}");
});
```

---

## 📊 Analytics & Data APIs

### AnalyticsManager
**Data collection and business intelligence**

```csharp
public class AnalyticsManager : ChimeraManager
{
    public bool AnalyticsEnabled { get; set; }
    public bool PrivacyMode { get; set; }
    public long EventsTracked { get; }
    
    // Event tracking
    public void TrackEvent(string eventName, Dictionary<string, object> parameters = null);
    public void TrackCustomEvent(AnalyticsEvent customEvent);
    public void TrackPerformanceMetric(string metricName, float value);
    
    // Data analysis
    public AnalyticsReport GenerateReport(ReportType reportType, System.TimeSpan timeRange);
    public List<AnalyticsInsight> GetBusinessInsights();
    public PlayerBehaviorAnalysis AnalyzePlayerBehavior();
    
    // Experiments
    public void StartABTest(ABTestConfiguration testConfig);
    public void EndABTest(string testId);
    public ABTestResults GetABTestResults(string testId);
    
    // Data export
    public void ExportData(DataExportFormat format, string filePath);
    public AnalyticsDashboardData GetDashboardData();
}
```

### DataManager
**ScriptableObject asset management**

```csharp
public class DataManager : ChimeraManager
{
    // Asset loading
    public T LoadAsset<T>(string assetPath) where T : ChimeraScriptableObject;
    public List<T> LoadAssetsByType<T>() where T : ChimeraScriptableObject;
    public void LoadAssetsAsync<T>(System.Action<List<T>> onComplete) where T : ChimeraScriptableObject;
    
    // Asset validation
    public bool ValidateAsset<T>(T asset) where T : ChimeraScriptableObject;
    public List<DataValidationError> ValidateAllAssets();
    public void RepairAsset<T>(T asset) where T : ChimeraScriptableObject;
    
    // Asset registry
    public void RegisterAsset<T>(T asset) where T : ChimeraScriptableObject;
    public void UnregisterAsset<T>(T asset) where T : ChimeraScriptableObject;
    public Dictionary<System.Type, List<ChimeraScriptableObject>> GetAssetRegistry();
}
```

**Usage Examples**:
```csharp
// Track player actions
_analyticsManager.TrackEvent("PlantHarvested", new Dictionary<string, object>
{
    {"strain", plant.Strain.StrainName},
    {"yield", harvestResults.TotalYieldGrams},
    {"quality", harvestResults.QualityScore},
    {"timeToHarvest", plant.GrowthDuration.TotalDays}
});

// Generate business report
var report = _analyticsManager.GenerateReport(ReportType.WeeklyPerformance, System.TimeSpan.FromDays(7));
Debug.Log($"Weekly Revenue: ${report.TotalRevenue:F2}");

// Load game data
var strains = _dataManager.LoadAssetsByType<PlantStrainSO>();
Debug.Log($"Loaded {strains.Count} plant strains");
```

---

## 🎯 Progression & UI APIs

### ProgressionManager
**Player advancement and skill development**

```csharp
public class ProgressionManager : ChimeraManager
{
    public int PlayerLevel { get; }
    public float CurrentExperience { get; }
    public float ExperienceToNextLevel { get; }
    
    // Experience and leveling
    public void AwardExperience(float amount, string source);
    public void AwardSkillExperience(SkillType skill, float amount);
    public bool CanLevelUp();
    public void LevelUp();
    
    // Skills and unlocks
    public Dictionary<SkillType, float> GetSkillLevels();
    public bool IsUnlocked(string unlockId);
    public void UnlockFeature(string featureId);
    public List<AvailableUnlock> GetAvailableUnlocks();
    
    // Achievements
    public void TriggerAchievement(string achievementId);
    public List<Achievement> GetUnlockedAchievements();
    public float GetAchievementProgress(string achievementId);
}
```

### UI System APIs
**Interface management and customization**

```csharp
// GameUIManager - Main interface controller
public class GameUIManager : ChimeraManager
{
    public bool UIVisible { get; set; }
    public UITheme CurrentTheme { get; set; }
    
    // Panel management
    public void ShowPanel(string panelName);
    public void HidePanel(string panelName);
    public void TogglePanel(string panelName);
    public bool IsPanelVisible(string panelName);
    
    // Notifications
    public void ShowNotification(string message, NotificationType type);
    public void ShowTooltip(string text, Vector2 position);
    public void HideTooltip();
    
    // Themes and customization
    public void ApplyTheme(UITheme theme);
    public List<UITheme> GetAvailableThemes();
    public void CustomizeInterface(UICustomization customization);
}
```

**Usage Examples**:
```csharp
// Award experience for achievements
_progressionManager.AwardExperience(500f, "First Harvest");
_progressionManager.AwardSkillExperience(SkillType.Cultivation, 100f);

// Check unlocks
if (_progressionManager.IsUnlocked("AdvancedGenetics"))
{
    // Enable advanced breeding features
}

// UI management
_gameUIManager.ShowPanel("PlantManagement");
_gameUIManager.ShowNotification("Harvest completed successfully!", NotificationType.Success);
```

---

## 🧪 Testing APIs

### TestingManager
**Automated testing and validation**

```csharp
public class TestingManager : ChimeraManager
{
    public bool TestingEnabled { get; set; }
    public TestingConfiguration Configuration { get; set; }
    
    // Test execution
    public string StartTestSession(string sessionName = "Default Session", TestSessionConfiguration config = null);
    public void StopTestSession(string sessionId);
    public TestSessionResults GetTestResults(string sessionId);
    
    // Test management
    public void RegisterTestSuite(TestSuite testSuite);
    public void UnregisterTestSuite(string suiteId);
    public List<TestSuite> GetAvailableTestSuites();
    
    // Validation
    public SystemValidationResults ValidateAllSystems();
    public bool ValidateSystemIntegration();
    public PerformanceTestResults RunPerformanceTests();
}
```

---

## 🔌 Extension and Modding APIs

### Modding Interfaces
**Plugin system for community content**

```csharp
// Core modding interface
public interface IChimeraMod
{
    string ModName { get; }
    string ModVersion { get; }
    string ModAuthor { get; }
    
    void Initialize();
    void Shutdown();
    bool IsCompatible(string gameVersion);
}

// Custom strain provider
public interface ICustomStrainProvider : IChimeraMod
{
    IEnumerable<PlantStrainSO> GetCustomStrains();
    bool ValidateStrain(PlantStrainSO strain);
}

// Custom equipment provider
public interface ICustomEquipmentProvider : IChimeraMod
{
    IEnumerable<EquipmentDataSO> GetCustomEquipment();
    void ProcessEquipmentEffect(EquipmentDataSO equipment, PlantInstance plant);
}
```

**Usage Examples**:
```csharp
// Register custom mod
public class CustomGeneticsProvider : MonoBehaviour, ICustomStrainProvider
{
    public string ModName => "Advanced Genetics Pack";
    public string ModVersion => "1.0.0";
    public string ModAuthor => "ModAuthor";
    
    public void Initialize()
    {
        var modManager = GameManager.Instance.GetManager<ModManager>();
        modManager.RegisterMod(this);
    }
    
    public IEnumerable<PlantStrainSO> GetCustomStrains()
    {
        // Return custom strain definitions
        return _customStrains;
    }
    
    public bool ValidateStrain(PlantStrainSO strain)
    {
        // Validate custom strain data
        return strain != null && !string.IsNullOrEmpty(strain.StrainName);
    }
}
```

---

## 🔍 Utility APIs

### Common Utilities
**Helper functions and data structures**

```csharp
// Event system utilities
public static class ChimeraEvents
{
    public static void Subscribe<T>(GameEventSO<T> eventChannel, System.Action<T> callback);
    public static void Unsubscribe<T>(GameEventSO<T> eventChannel, System.Action<T> callback);
    public static void Raise<T>(GameEventSO<T> eventChannel, T data);
}

// Math utilities
public static class ChimeraMath
{
    public static float CalculateGrowthRate(float baseRate, EnvironmentalConditions conditions);
    public static float ApplyGeneticModifier(float baseValue, float geneticFactor);
    public static Vector2 CalculateOptimalRange(float centerValue, float tolerance);
}

// Validation utilities
public static class ChimeraValidation
{
    public static bool ValidateRange(float value, float min, float max);
    public static bool ValidatePercentage(float value);
    public static bool ValidateAssetReference<T>(T asset) where T : ChimeraScriptableObject;
}
```

This comprehensive API reference provides all the information needed to develop with, extend, or integrate Project Chimera systems. For implementation examples and best practices, refer to the Developer Documentation.