# Project Chimera - Developer Documentation

## 🏗️ Architecture Overview

Project Chimera is built on Unity 6000.2.0b2 with a sophisticated ScriptableObject-driven architecture that emphasizes modularity, maintainability, and extensibility. This document provides comprehensive guidance for developers working on the codebase.

---

## 🔧 Core Architecture Patterns

### Manager Pattern
**Central Coordination**: The GameManager serves as the singleton coordinator for all systems.

```csharp
// Accessing managers throughout the codebase
var plantManager = GameManager.Instance.GetManager<PlantManager>();
var economyManager = GameManager.Instance.GetManager<EconomyManager>();
```

**Manager Lifecycle**:
1. **Initialization**: Managers initialize in dependency order
2. **Registration**: Automatic registration with GameManager
3. **Update Cycles**: Managed update timing and performance optimization
4. **Shutdown**: Clean shutdown with proper resource cleanup

### ScriptableObject-Driven Data
**Configuration Through Assets**: All game data uses ScriptableObjects for modularity.

```csharp
[CreateAssetMenu(fileName = "New Plant Strain", menuName = "Project Chimera/Genetics/Plant Strain")]
public class PlantStrainSO : ChimeraDataSO
{
    [Header("Basic Information")]
    public string StrainName;
    public string Description;
    public Sprite StrainImage;
    
    [Header("Genetic Properties")]
    public GeneticTraitRange ThcContent;
    public GeneticTraitRange CbdContent;
    public TerpeneProfile TerpeneProfile;
}
```

**Benefits**:
- **Designer-Friendly**: Non-programmers can create and modify game content
- **Runtime Safety**: Immutable data prevents accidental modifications
- **Version Control**: Asset-based data integrates well with Git
- **Performance**: Efficient serialization and loading

### Event-Driven Architecture
**Loose Coupling**: Systems communicate through ScriptableObject-based events.

```csharp
[CreateAssetMenu(fileName = "Plant Harvested Event", menuName = "Project Chimera/Events/Plant Event")]
public class PlantHarvestedEventSO : GameEventSO<HarvestResults>
{
    // Event-specific functionality
}

// Triggering events
_onPlantHarvested?.Raise(harvestResults);

// Listening to events
public void OnEnable()
{
    _onPlantHarvested.RegisterListener(HandlePlantHarvested);
}
```

---

## 📁 Assembly Structure

### Core Assemblies

#### ProjectChimera.Core
**Purpose**: Base classes, managers, and fundamental systems
**Dependencies**: None (foundation layer)

**Key Components**:
- `ChimeraManager`: Base class for all managers
- `GameManager`: Central coordination singleton
- `EventManager`: Global event system coordination
- `ChimeraScriptableObject`: Base SO class with validation

#### ProjectChimera.Data
**Purpose**: ScriptableObject data definitions and configuration
**Dependencies**: Core

**Organization**:
```
Data/
├── Genetics/           # Plant strains, genes, alleles
├── Environment/        # Environmental conditions, equipment
├── Economy/           # Market data, financial structures
├── Progression/       # Skills, achievements, research
├── Automation/        # IoT devices, sensors, schedules
└── UI/               # Interface themes, announcements
```

#### ProjectChimera.Systems
**Purpose**: Gameplay systems and business logic
**Dependencies**: Core, Data

**Major Systems**:
- **Cultivation**: Plant lifecycle and growth simulation
- **Genetics**: Breeding mechanics and trait inheritance
- **Environment**: HVAC, lighting, and climate control
- **Economy**: Markets, trading, and financial management
- **Automation**: IoT integration and intelligent control
- **Analytics**: Data collection and business intelligence

#### ProjectChimera.UI
**Purpose**: User interface components and controllers
**Dependencies**: Core, Data, Systems

**UI Architecture**:
- **UI Toolkit**: Modern, responsive interface system
- **MVVM Pattern**: Clean separation of view and logic
- **Event-Driven Updates**: Reactive UI responding to system changes
- **Performance Optimization**: Efficient rendering and state management

#### ProjectChimera.Testing
**Purpose**: Comprehensive testing framework and QA automation
**Dependencies**: All other assemblies (editor-only)

**Testing Categories**:
- **Unit Tests**: Individual component validation
- **Integration Tests**: System interaction verification
- **Performance Tests**: Benchmark compliance and optimization
- **QA Automation**: Regression testing and quality gates

---

## 🎯 Development Guidelines

### Coding Standards

#### Naming Conventions
```csharp
// Private fields
private float _currentTemperature;
private PlantManager _plantManager;

// Public properties
public float OptimalTemperature { get; set; }
public string StrainName { get; private set; }

// Methods
public void CalculateGrowthRate() { }
private void _UpdatePlantHealth() { }

// ScriptableObjects
public class PlantStrainSO : ChimeraDataSO { }
public class EnvironmentConfigSO : ChimeraConfigSO { }
```

#### Design Patterns

**Single Responsibility**: Each class has one clear purpose
```csharp
// Good: Focused responsibility
public class TemperatureController
{
    public void SetTargetTemperature(float temperature) { }
    public float GetCurrentTemperature() { }
    public bool IsWithinRange(float tolerance) { }
}

// Avoid: Multiple responsibilities
public class EnvironmentController // Too broad
{
    // Temperature, humidity, lighting, CO2 all in one class
}
```

**Dependency Injection**: Use the manager registry for system access
```csharp
public class CultivationSystem : ChimeraManager
{
    private EnvironmentalManager _environmentManager;
    private EconomyManager _economyManager;
    
    protected override void OnManagerInitialize()
    {
        _environmentManager = GameManager.Instance.GetManager<EnvironmentalManager>();
        _economyManager = GameManager.Instance.GetManager<EconomyManager>();
    }
}
```

### Performance Considerations

#### Update Optimization
```csharp
// Batch processing for large collections
private void UpdatePlants()
{
    int plantsToProcess = Mathf.Min(_maxPlantsPerUpdate, _plantsToUpdate.Count);
    
    for (int i = _currentUpdateIndex; i < _currentUpdateIndex + plantsToProcess; i++)
    {
        var plant = _plantsToUpdate[i % _plantsToUpdate.Count];
        _updateProcessor.UpdatePlant(plant, deltaTime);
    }
    
    _currentUpdateIndex = (_currentUpdateIndex + plantsToProcess) % _plantsToUpdate.Count;
}
```

#### Memory Management
```csharp
// Object pooling for frequently created objects
public class HarvestResultsPool : MonoBehaviour
{
    private Queue<HarvestResults> _pool = new Queue<HarvestResults>();
    
    public HarvestResults Get()
    {
        return _pool.Count > 0 ? _pool.Dequeue() : new HarvestResults();
    }
    
    public void Return(HarvestResults item)
    {
        item.Reset();
        _pool.Enqueue(item);
    }
}
```

#### Data Structure Optimization
```csharp
// Use structs for small, immutable data
[System.Serializable]
public struct EnvironmentalReading
{
    public float Temperature;
    public float Humidity;
    public float CO2Level;
    public DateTime Timestamp;
}

// Use classes for complex, mutable objects
[System.Serializable]
public class PlantInstance : MonoBehaviour
{
    // Complex state management and behavior
}
```

---

## 🧪 Testing Framework

### Test Categories

#### Unit Tests
**Purpose**: Validate individual components in isolation

```csharp
[Test]
public void PlantInstance_CalculateGrowthRate_ReturnsCorrectValue()
{
    // Arrange
    var plant = CreateTestPlant();
    var environmentalConditions = CreateOptimalConditions();
    
    // Act
    var growthRate = plant.CalculateGrowthRate(environmentalConditions);
    
    // Assert
    Assert.That(growthRate, Is.GreaterThan(0));
    Assert.That(growthRate, Is.LessThanOrEqualTo(1.0f));
}
```

#### Integration Tests
**Purpose**: Verify system interactions and data flow

```csharp
[UnityTest]
public IEnumerator PlantManager_CreateAndHarvestPlant_CompletesSuccessfully()
{
    // Arrange
    var plantManager = GameManager.Instance.GetManager<PlantManager>();
    var testStrain = CreateTestStrain();
    
    // Act
    var plant = plantManager.CreatePlant(testStrain, Vector3.zero);
    yield return AdvancePlantToHarvest(plant);
    var harvestResults = plantManager.HarvestPlant(plant.PlantID);
    
    // Assert
    Assert.IsNotNull(harvestResults);
    Assert.Greater(harvestResults.TotalYieldGrams, 0);
}
```

#### Performance Tests
**Purpose**: Ensure system performance meets requirements

```csharp
[Test, Performance]
public void MarketManager_ProcessTransaction_CompletesWithinThreshold()
{
    // Arrange
    var marketManager = GameManager.Instance.GetManager<MarketManager>();
    var product = CreateTestProduct();
    
    // Act & Assert
    Measure.Method(() =>
    {
        marketManager.ProcessSale(product, 100f, 0.9f, false);
    })
    .WarmupCount(5)
    .MeasurementCount(20)
    .ExpectedAllocation(0, "No allocations expected")
    .Run();
}
```

### Testing Best Practices

#### Test Setup and Teardown
```csharp
[SetUp]
public void Setup()
{
    // Initialize test environment
    GameManager.Instance.InitializeForTesting();
    _testDataManager = CreateTestDataManager();
}

[TearDown]
public void Teardown()
{
    // Clean up test environment
    DestroyTestObjects();
    GameManager.Instance.ResetForTesting();
}
```

#### Mock Objects and Test Doubles
```csharp
// Mock complex dependencies for isolated testing
public class MockEnvironmentalManager : IEnvironmentalManager
{
    public EnvironmentalConditions GetCurrentConditions()
    {
        return new EnvironmentalConditions
        {
            Temperature = 24.0f,
            Humidity = 60.0f,
            CO2Level = 400.0f
        };
    }
}
```

---

## 🔌 Extension and Modding

### Modding Architecture
**Plugin System**: Support for community-created content

```csharp
// Mod interface for custom plant strains
public interface ICustomStrainProvider
{
    IEnumerable<PlantStrainSO> GetCustomStrains();
    bool ValidateStrain(PlantStrainSO strain);
}

// Mod registration system
public class ModManager : ChimeraManager
{
    private List<ICustomStrainProvider> _strainProviders = new List<ICustomStrainProvider>();
    
    public void RegisterStrainProvider(ICustomStrainProvider provider)
    {
        _strainProviders.Add(provider);
    }
}
```

### ScriptableObject Extensions
**Custom Data Types**: Extending the data system

```csharp
// Base class for custom equipment
public abstract class CustomEquipmentSO : EquipmentDataSO
{
    [Header("Custom Equipment Properties")]
    public string ModAuthor;
    public string ModVersion;
    
    public abstract void ProcessCustomEffect(PlantInstance plant);
}

// Implementation example
[CreateAssetMenu(fileName = "UV Light System", menuName = "Mods/Custom Equipment/UV Light")]
public class UVLightSystemSO : CustomEquipmentSO
{
    [Range(0f, 100f)]
    public float UVIntensity = 10f;
    
    public override void ProcessCustomEffect(PlantInstance plant)
    {
        // Custom UV light effects on plant
        plant.ApplyUVStress(UVIntensity);
    }
}
```

---

## 🚀 Deployment and Build Process

### Build Configuration
**Platform-Specific Settings**: Optimized builds for different targets

```csharp
#if UNITY_STANDALONE_WIN
    // Windows-specific optimizations
    Application.targetFrameRate = 60;
#elif UNITY_STANDALONE_OSX
    // macOS-specific settings
    Application.targetFrameRate = 60;
#elif UNITY_STANDALONE_LINUX
    // Linux-specific configurations
    Application.targetFrameRate = 60;
#endif
```

### Asset Pipeline
**Addressable Assets**: Dynamic content loading system

```csharp
// Addressable asset loading
public async Task<PlantStrainSO> LoadStrainAsync(string strainKey)
{
    var handle = Addressables.LoadAssetAsync<PlantStrainSO>(strainKey);
    await handle.Task;
    
    if (handle.Status == AsyncOperationStatus.Succeeded)
    {
        return handle.Result;
    }
    
    LogError($"Failed to load strain: {strainKey}");
    return null;
}
```

### Continuous Integration
**Automated Testing**: Build pipeline integration

```yaml
# GitHub Actions example
name: Unity Build and Test
on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - uses: game-ci/unity-test-runner@v2
        with:
          projectPath: Chimera
          testMode: all
      - uses: game-ci/unity-builder@v2
        with:
          projectPath: Chimera
          targetPlatform: StandaloneWindows64
```

---

## 📊 Analytics and Monitoring

### Performance Monitoring
**Runtime Metrics**: Continuous performance tracking

```csharp
public class PerformanceMonitor : ChimeraManager
{
    private Dictionary<string, PerformanceMetric> _metrics = new Dictionary<string, PerformanceMetric>();
    
    public void TrackMetric(string name, float value)
    {
        if (!_metrics.ContainsKey(name))
        {
            _metrics[name] = new PerformanceMetric(name);
        }
        
        _metrics[name].AddSample(value);
    }
    
    public PerformanceReport GenerateReport()
    {
        return new PerformanceReport(_metrics.Values.ToList());
    }
}
```

### Error Handling and Logging
**Comprehensive Logging**: Structured error reporting

```csharp
public class ChimeraLogger
{
    public static void LogInfo(string message, Object context = null)
    {
        Debug.Log($"[INFO] {message}", context);
        AnalyticsManager.TrackEvent("Log.Info", new { message });
    }
    
    public static void LogError(string message, System.Exception exception = null, Object context = null)
    {
        Debug.LogError($"[ERROR] {message}", context);
        
        if (exception != null)
        {
            Debug.LogException(exception, context);
        }
        
        AnalyticsManager.TrackEvent("Log.Error", new { message, exception?.Message });
    }
}
```

---

## 🔧 Debugging and Profiling

### Debug Utilities
**Development Tools**: Debugging aids and visualization

```csharp
#if UNITY_EDITOR
public class PlantDebugger : MonoBehaviour
{
    [SerializeField] private bool _showGizmos = true;
    [SerializeField] private Color _healthColor = Color.green;
    
    private void OnDrawGizmosSelected()
    {
        if (!_showGizmos) return;
        
        var plant = GetComponent<PlantInstance>();
        if (plant != null)
        {
            // Visualize plant health
            Gizmos.color = Color.Lerp(Color.red, _healthColor, plant.CurrentHealth);
            Gizmos.DrawWireSphere(transform.position, plant.CurrentHealth * 2f);
        }
    }
}
#endif
```

### Profiling Integration
**Performance Analysis**: Built-in profiling support

```csharp
public class ProfiledOperation : System.IDisposable
{
    private string _operationName;
    private System.Diagnostics.Stopwatch _stopwatch;
    
    public ProfiledOperation(string operationName)
    {
        _operationName = operationName;
        _stopwatch = System.Diagnostics.Stopwatch.StartNew();
        UnityEngine.Profiling.Profiler.BeginSample(_operationName);
    }
    
    public void Dispose()
    {
        _stopwatch.Stop();
        UnityEngine.Profiling.Profiler.EndSample();
        
        if (_stopwatch.ElapsedMilliseconds > 10) // Log slow operations
        {
            Debug.LogWarning($"Slow operation: {_operationName} took {_stopwatch.ElapsedMilliseconds}ms");
        }
    }
}

// Usage
using (new ProfiledOperation("Plant Growth Calculation"))
{
    // Performance-critical code
    CalculateGrowthForAllPlants();
}
```

---

## 📚 Additional Resources

### Documentation Standards
- **XML Documentation**: Comprehensive code documentation
- **README Files**: System-specific setup and usage guides
- **Architecture Diagrams**: Visual system relationship maps
- **API Documentation**: Auto-generated reference materials

### Development Environment Setup
- **Unity Version**: 6000.2.0b2 or latest stable Unity 6
- **IDE**: JetBrains Rider or Visual Studio with Unity tools
- **Version Control**: Git with LFS for large assets
- **Package Management**: Unity Package Manager and custom packages

### Community and Support
- **Developer Discord**: Real-time development discussion
- **GitHub Issues**: Bug reports and feature requests
- **Code Reviews**: Pull request review process
- **Knowledge Base**: Searchable development documentation

This developer documentation provides the foundation for contributing to Project Chimera. For specific implementation details, refer to the inline code documentation and system-specific guides.