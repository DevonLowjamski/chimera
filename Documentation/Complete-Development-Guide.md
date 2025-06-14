# Project Chimera - Ultimate Development Completion Guide

## 🚀 The Complete Development Manual: From Current State to Finished Game

This comprehensive guide provides exhaustive, step-by-step instructions for completing Project Chimera from its current advanced system state to a fully polished, production-ready cannabis cultivation simulation. Every detail is covered with beginner-friendly explanations.

---

## 📋 Table of Contents

1. [Development Environment Setup](#development-environment-setup)
2. [Unity Project Configuration](#unity-project-configuration)
3. [SpeedTree Integration Implementation](#speedtree-integration-implementation)
4. [3D Asset Creation and Integration](#3d-asset-creation-and-integration)
5. [Scene Construction and Environment Building](#scene-construction-and-environment-building)
6. [Visual Effects and Particle Systems](#visual-effects-and-particle-systems)
7. [Audio System Implementation](#audio-system-implementation)
8. [User Interface Development](#user-interface-development)
9. [Animation and Timeline Systems](#animation-and-timeline-systems)
10. [Performance Optimization and Profiling](#performance-optimization-and-profiling)
11. [Testing and Quality Assurance](#testing-and-quality-assurance)
12. [Build Pipeline and Deployment](#build-pipeline-and-deployment)
13. [Post-Launch Support and Updates](#post-launch-support-and-updates)

---

## 🛠️ Development Environment Setup

### Prerequisites and Software Installation

#### 1. Unity Installation and Configuration
**Step 1: Download Unity Hub**
1. Visit https://unity.com/download
2. Click "Download Unity Hub"
3. Run the installer with administrator privileges
4. Launch Unity Hub after installation

**Step 2: Install Unity 6000.2.0b2 (Unity 6 Beta)**
1. Open Unity Hub
2. Click "Installs" tab
3. Click "Install Editor"
4. Select "Beta" from the dropdown
5. Choose Unity 6000.2.0b2 (or latest Unity 6 Beta)
6. In "Add modules" section, select:
   - **Visual Studio Community 2022** (Windows) or **Visual Studio for Mac** (macOS)
   - **Documentation**
   - **Universal Windows Platform Build Support** (Windows only)
   - **Mac Build Support (Mono)** (Windows only)
   - **Linux Build Support (Mono)** (all platforms)
   - **WebGL Build Support**
7. Click "Install" and wait for completion (30-60 minutes)

**Step 3: Configure Unity for Project Chimera**
1. Open Unity Hub
2. Click "Projects" tab
3. Click "Open" → Navigate to `/Users/devon/Documents/Cursor/Projects/Chimera/Chimera`
4. Select the Chimera folder and click "Open"
5. Wait for Unity to import and compile (5-10 minutes first time)

#### 2. Development Tools Installation

**Step 1: JetBrains Rider IDE (Recommended)**
1. Visit https://www.jetbrains.com/rider/
2. Download 30-day trial or use educational license
3. Install with default settings
4. Launch Rider
5. Open Project Chimera:
   - File → Open → Navigate to Chimera folder
   - Select `Chimera.sln` if it exists, or the project folder
6. Configure Rider for Unity:
   - File → Settings → Languages & Frameworks → Unity
   - Check "Enable Unity integration"
   - Set Unity executable path to installed Unity editor

**Alternative: Visual Studio Community (Free)**
1. Download from https://visualstudio.microsoft.com/vs/community/
2. During installation, select:
   - **Game development with Unity**
   - **.NET desktop development**
   - **Universal Windows Platform development** (optional)
3. Install and launch
4. Configure for Unity integration

**Step 2: Version Control Setup**
1. Install Git if not already installed:
   - Windows: Download from https://git-scm.com/
   - macOS: `brew install git` or Xcode Command Line Tools
   - Linux: `sudo apt install git` or equivalent
2. Configure Git:
   ```bash
   git config --global user.name "Your Name"
   git config --global user.email "your.email@example.com"
   ```
3. Install Git LFS for large files:
   ```bash
   git lfs install
   ```

**Step 3: Additional Development Tools**
1. **Blender (Free 3D Modeling)**
   - Download from https://www.blender.org/download/
   - Install with default settings
   - Used for 3D asset creation and modification

2. **GIMP or Photoshop (Image Editing)**
   - GIMP: Free from https://www.gimp.org/
   - Photoshop: Adobe Creative Cloud subscription
   - Used for texture creation and UI graphics

3. **Audacity (Audio Editing)**
   - Download from https://www.audacityteam.org/
   - Free audio editing for sound effects and music

#### 3. Unity Package Manager Setup

**Step 1: Access Package Manager**
1. In Unity, go to **Window** → **Package Manager**
2. The Package Manager window opens

**Step 2: Install Essential Packages**
For each package below:
1. Click dropdown showing "In Project"
2. Select "Unity Registry"
3. Search for package name
4. Click package name
5. Click "Install" button
6. Wait for installation to complete

**Required Packages:**
- **Universal RP** (17.2.0 or latest)
- **Visual Scripting** (1.9.0 or latest)
- **Timeline** (1.8.0 or latest)
- **Cinemachine** (2.10.0 or latest)
- **Post Processing** (3.3.0 or latest)
- **Addressable Assets** (1.21.0 or latest)
- **Unity Test Framework** (1.3.0 or latest)
- **Input System** (1.7.0 or latest)
- **UI Toolkit** (1.0.0 or latest)

**Optional but Recommended:**
- **SpeedTree** (if available in Unity 6 Beta)
- **ProBuilder** (5.2.0 or latest)
- **ProGrids** (3.0.6 or latest)
- **Burst** (1.8.0 or latest)
- **Mathematics** (1.3.0 or latest)

#### 4. Project Structure Verification

**Step 1: Verify Assembly Definitions**
1. In Unity Project window, navigate to `Assets/ProjectChimera/`
2. Verify these assemblies exist:
   - `Core/ProjectChimera.Core.asmdef`
   - `Data/ProjectChimera.Data.asmdef`
   - `Systems/ProjectChimera.Systems.asmdef`
   - `UI/ProjectChimera.UI.asmdef`
   - `Testing/ProjectChimera.Testing.asmdef`

**Step 2: Check Compilation**
1. Look at Console window (Window → General → Console)
2. Should show 0 errors after initial compilation
3. If errors exist, resolve them before proceeding

---

## ⚙️ Unity Project Configuration

### Render Pipeline Setup

#### 1. Universal Render Pipeline Configuration

**Step 1: Create URP Asset**
1. In Project window, right-click in `Assets/Settings/` (create folder if needed)
2. Create → Rendering → Universal Render Pipeline → Pipeline Asset (Forward Renderer)
3. Name it "ProjectChimera_URP_Asset"
4. Select the created asset
5. In Inspector, configure:
   - **Render Scale**: 1.0
   - **Rendering Path**: Forward
   - **Depth Texture**: Enabled
   - **Opaque Texture**: Enabled
   - **Terrain Holes**: Enabled

**Step 2: Configure Graphics Settings**
1. Go to Edit → Project Settings
2. Select **Graphics** category
3. Set **Scriptable Render Pipeline Settings** to "ProjectChimera_URP_Asset"
4. Configure **Shader Stripping**:
   - **Lightmap Modes**: Mixed and Baked
   - **Fog Modes**: Linear, Exponential, Exponential Squared
   - **Instancing Variants**: Include

**Step 3: Quality Settings Configuration**
1. In Project Settings, select **Quality**
2. For each quality level, configure:
   - **Ultra Quality**:
     - Render Pipeline Asset: ProjectChimera_URP_Asset
     - Texture Quality: Full Res
     - Anisotropic Textures: Per Texture
     - Anti Aliasing: 8x Multi Sampling
     - Shadow Resolution: Very High
   - **High Quality**:
     - Render Pipeline Asset: ProjectChimera_URP_Asset
     - Texture Quality: Full Res
     - Anisotropic Textures: Per Texture
     - Anti Aliasing: 4x Multi Sampling
     - Shadow Resolution: High
   - **Medium Quality**:
     - Render Pipeline Asset: ProjectChimera_URP_Asset
     - Texture Quality: Half Res
     - Anti Aliasing: 2x Multi Sampling
     - Shadow Resolution: Medium

#### 2. Lighting System Setup

**Step 1: Configure Lighting Settings**
1. Go to Window → Rendering → Lighting
2. In **Environment** tab:
   - **Environment Lighting Source**: Skybox
   - **Environment Reflections Source**: Skybox
   - **Sun Source**: Assign main directional light
3. In **Mixed Lighting** tab:
   - **Baked Global Illumination**: Enabled
   - **Realtime Global Illumination**: Disabled (for performance)
   - **Lighting Mode**: Baked Indirect

**Step 2: Lightmapping Settings**
1. In Lighting window, **Lightmapping Settings** tab:
   - **Lightmapper**: Progressive GPU (if available) or Progressive CPU
   - **Max Lightmap Size**: 2048
   - **Compression**: Normal Quality
   - **Ambient Occlusion**: Enabled
   - **Final Gather**: Enabled
   - **Bounce Count**: 2

### Player and Input Settings

#### 1. Player Configuration

**Step 1: Basic Player Settings**
1. Edit → Project Settings → Player
2. **Company Name**: "Project Chimera Development"
3. **Product Name**: "Project Chimera"
4. **Version**: "1.0.0"
5. **Default Icon**: Create and assign 1024x1024 icon

**Step 2: Resolution and Presentation**
1. In Player Settings, **Resolution and Presentation**:
   - **Fullscreen Mode**: Fullscreen Window
   - **Default Screen Width**: 1920
   - **Default Screen Height**: 1080
   - **Resizable Window**: Enabled
   - **Supported Aspect Ratios**: 16:9, 16:10, 21:9

**Step 3: Configuration Settings**
1. **Scripting Backend**: IL2CPP (for better performance)
2. **Api Compatibility Level**: .NET Standard 2.1
3. **Target Architectures**: x86_64 (64-bit)

#### 2. Input System Setup

**Step 1: Enable New Input System**
1. Edit → Project Settings → Player → Configuration
2. **Active Input Handling**: Input System Package (New)
3. Allow Unity to restart when prompted

**Step 2: Create Input Actions**
1. In Project window, right-click in `Assets/Settings/`
2. Create → Input Actions
3. Name it "ProjectChimeraInputActions"
4. Double-click to open Input Actions editor
5. Create Action Maps:
   - **Gameplay**
   - **UI**
   - **Camera**

**Step 3: Configure Input Actions**

**Gameplay Action Map:**
1. Add Actions:
   - **Move**: Vector2, Composite: 2D Vector
     - Up: W, Down: S, Left: A, Right: D
   - **Interact**: Button
     - Binding: E key, Left Mouse Button
   - **Cancel**: Button
     - Binding: Escape key
   - **ContextMenu**: Button
     - Binding: Right Mouse Button

**Camera Action Map:**
1. Add Actions:
   - **Look**: Vector2
     - Binding: Mouse Delta
   - **Zoom**: Axis
     - Binding: Mouse Scroll Y
   - **Pan**: Vector2
     - Binding: Mouse Delta (with modifier Middle Mouse Button)
   - **Orbit**: Vector2
     - Binding: Mouse Delta (with modifier Right Mouse Button)

**UI Action Map:**
1. Add Actions:
   - **Navigate**: Vector2
     - Binding: Arrow Keys, WASD
   - **Submit**: Button
     - Binding: Enter, Space
   - **Cancel**: Button
     - Binding: Escape
   - **Click**: Button
     - Binding: Left Mouse Button

---

## 🌿 SpeedTree Integration Implementation

### SpeedTree Package Installation

#### 1. Installing SpeedTree for Unity

**Step 1: Check SpeedTree Availability**
1. Open Package Manager (Window → Package Manager)
2. Change dropdown to "Unity Registry"
3. Search for "SpeedTree"
4. If available in Unity 6, install the latest version
5. If not available, continue with conditional compilation setup

**Step 2: Alternative SpeedTree Setup**
If SpeedTree package is not available:
1. Download SpeedTree Modeler (trial available at speedtree.com)
2. Create cannabis plant models using SpeedTree Modeler
3. Export as Unity-compatible .st files
4. Import into Unity project

#### 2. SpeedTree Asset Preparation

**Step 1: Cannabis Plant Model Creation**

**Using SpeedTree Modeler:**
1. Launch SpeedTree Modeler
2. Create new project: **Cannabis Plant Template**
3. Configure basic tree structure:
   - **Trunk**: Main stem (0.5-2cm diameter)
   - **Branches**: Secondary branches from nodes
   - **Leaves**: Cannabis leaf shapes (7-11 leaflets)
   - **Flowers**: Cannabis bud structures

**Cannabis-Specific Modeling Guidelines:**
1. **Stem Structure**:
   - Main stem: Thick, sturdy, square-ish cross-section
   - Nodes: Where branches emerge (internode spacing varies by strain)
   - Branch angles: 45-90 degrees from main stem

2. **Leaf Modeling**:
   - Palmate compound leaves
   - 5-11 serrated leaflets per leaf
   - Varying sizes throughout plant height
   - Different shapes for fan leaves vs. sugar leaves

3. **Flower/Bud Modeling**:
   - Dense clusters at branch tips
   - Trichome-covered surfaces
   - Pistil structures (white/orange hairs)
   - Calyx formations

**Step 2: Creating Strain Variations**
Create multiple SpeedTree models for different strains:

1. **Indica Model** (`IndicaPlant.st`):
   - Shorter, bushier structure (1-1.5m height)
   - Wider internodal spacing
   - Broader, darker leaves
   - Dense, compact bud structure

2. **Sativa Model** (`SativaPlant.st`):
   - Taller, lankier structure (1.5-3m height)
   - Longer internodal spacing
   - Narrower, lighter leaves
   - Longer, less dense buds

3. **Hybrid Models** (`HybridPlant01.st`, `HybridPlant02.st`, etc.):
   - Intermediate characteristics
   - Various combinations of indica/sativa traits

#### 3. SpeedTree Material Configuration

**Step 1: Create Cannabis Materials**

**Stem Material:**
1. Create → Material (name: "Cannabis_Stem_Material")
2. Set Shader to "Universal Render Pipeline/Lit"
3. Configure properties:
   - **Albedo**: Green-brown stem texture
   - **Metallic**: 0.0
   - **Smoothness**: 0.2-0.4
   - **Normal Map**: Stem surface detail texture

**Leaf Material:**
1. Create → Material (name: "Cannabis_Leaf_Material")
2. Set Shader to "Universal Render Pipeline/Lit"
3. Configure properties:
   - **Albedo**: Cannabis leaf texture with alpha
   - **Metallic**: 0.0
   - **Smoothness**: 0.1-0.3
   - **Normal Map**: Leaf surface detail
   - **Alpha Clipping**: Enabled (threshold: 0.5)

**Bud Material:**
1. Create → Material (name: "Cannabis_Bud_Material")
2. Set Shader to "Universal Render Pipeline/Lit"
3. Configure properties:
   - **Albedo**: Dense bud texture with trichomes
   - **Metallic**: 0.1 (slight reflection from trichomes)
   - **Smoothness**: 0.3-0.5
   - **Normal Map**: Detailed trichrome surface
   - **Emission**: Slight glow for healthy buds

#### 4. SpeedTree LOD Configuration

**Step 1: LOD Group Setup**
1. Select SpeedTree asset in Project window
2. In Inspector, configure **LOD Group**:
   - **LOD 0** (100%-30%): Full mesh with all details
   - **LOD 1** (30%-15%): Reduced polygon count, simplified branches
   - **LOD 2** (15%-5%): Billboard with 3D stem
   - **LOD 3** (5%-0%): Simple billboard

**Step 2: Performance Optimization**
1. **Culling Distance**: Set maximum render distance
2. **Billboard Generation**: Enable for distant rendering
3. **Wind Settings**: Configure realistic movement
4. **Collision**: Simplified collision meshes for interaction

### SpeedTree Integration Code Implementation

#### 1. AdvancedSpeedTreeManager Implementation

**Step 1: Create Core Manager Script**

Create file: `Assets/ProjectChimera/Systems/SpeedTree/AdvancedSpeedTreeManager.cs`

```csharp
using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Genetics;
using System.Collections.Generic;
using System.Collections;

#if UNITY_SPEEDTREE
using SpeedTree;
#endif

namespace ProjectChimera.Systems.SpeedTree
{
    /// <summary>
    /// Advanced SpeedTree manager for cannabis plant simulation with genetics integration
    /// </summary>
    public class AdvancedSpeedTreeManager : ChimeraManager
    {
        [Header("SpeedTree Configuration")]
        [SerializeField] private SpeedTreeConfigSO _speedTreeConfig;
        [SerializeField] private Transform _plantContainer;
        [SerializeField] private LayerMask _plantLayer = 1 << 8; // Plants layer
        
        [Header("Cannabis Plant Prefabs")]
        [SerializeField] private GameObject _indicaPrefab;
        [SerializeField] private GameObject _sativaPrefab;
        [SerializeField] private GameObject[] _hybridPrefabs;
        
        [Header("Performance Settings")]
        [SerializeField] private int _maxVisiblePlants = 500;
        [SerializeField] private float _cullingDistance = 100f;
        [SerializeField] private bool _enableLODOptimization = true;
        
        // Plant instance management
        private Dictionary<int, SpeedTreePlantInstance> _plantInstances = new Dictionary<int, SpeedTreePlantInstance>();
        private Queue<int> _availableInstanceIds = new Queue<int>();
        private int _nextInstanceId = 1;
        
        // Performance tracking
        private SpeedTreePerformanceMetrics _performanceMetrics;
        private List<SpeedTreePlantInstance> _visiblePlants = new List<SpeedTreePlantInstance>();
        
        // Integration systems
        private CannabisGeneticsEngine _geneticsEngine;
        private SpeedTreeEnvironmentalSystem _environmentalSystem;
        private SpeedTreeGrowthSystem _growthSystem;
        private SpeedTreeOptimizationSystem _optimizationSystem;
        
        // Properties
        public int ActivePlantInstances => _plantInstances.Count;
        public bool SpeedTreeEnabled 
        { 
            get 
            { 
#if UNITY_SPEEDTREE
                return true;
#else
                return false;
#endif
            } 
        }
        public SpeedTreePerformanceMetrics PerformanceMetrics => _performanceMetrics;
        
        protected override void OnManagerInitialize()
        {
            InitializeSpeedTreeSystems();
            InitializePerformanceTracking();
            SetupPlantContainer();
            ConnectToIntegrationSystems();
            
            LogInfo("Advanced SpeedTree Manager initialized");
        }
        
        private void Update()
        {
            UpdatePlantInstances();
            UpdatePerformanceMetrics();
            ProcessVisibilityOptimization();
        }
        
        #region Initialization
        
        private void InitializeSpeedTreeSystems()
        {
            // Initialize SpeedTree subsystems
            _geneticsEngine = gameObject.AddComponent<CannabisGeneticsEngine>();
            _environmentalSystem = gameObject.AddComponent<SpeedTreeEnvironmentalSystem>();
            _growthSystem = gameObject.AddComponent<SpeedTreeGrowthSystem>();
            _optimizationSystem = gameObject.AddComponent<SpeedTreeOptimizationSystem>();
            
            // Initialize performance metrics
            _performanceMetrics = new SpeedTreePerformanceMetrics();
            
            LogInfo("SpeedTree subsystems initialized");
        }
        
        private void InitializePerformanceTracking()
        {
            // Pre-populate instance ID queue
            for (int i = 1; i <= 10000; i++) // Support up to 10,000 plants
            {
                _availableInstanceIds.Enqueue(i);
            }
        }
        
        private void SetupPlantContainer()
        {
            if (_plantContainer == null)
            {
                GameObject containerObj = new GameObject("SpeedTree_Plants_Container");
                _plantContainer = containerObj.transform;
                _plantContainer.parent = transform;
            }
        }
        
        private void ConnectToIntegrationSystems()
        {
            // Connect to game manager systems if available
            if (GameManager.Instance != null)
            {
                // Integration connections would go here
            }
        }
        
        #endregion
        
        #region Plant Instance Management
        
        /// <summary>
        /// Creates a new SpeedTree plant instance with specified genetics
        /// </summary>
        public SpeedTreePlantInstance CreatePlantInstance(CannabisGenotype genotype, Vector3 position, Transform parent = null)
        {
            if (!SpeedTreeEnabled)
            {
                LogWarning("SpeedTree not available - using fallback plant creation");
                return CreateFallbackPlantInstance(genotype, position, parent);
            }
            
            // Get available instance ID
            if (!_availableInstanceIds.TryDequeue(out int instanceId))
            {
                LogError("Maximum plant instances reached");
                return null;
            }
            
            // Select appropriate prefab based on genetics
            GameObject prefab = SelectPrefabForGenotype(genotype);
            if (prefab == null)
            {
                LogError($"No prefab found for genotype: {genotype.StrainName}");
                _availableInstanceIds.Enqueue(instanceId); // Return ID to pool
                return null;
            }
            
            // Instantiate plant
            GameObject plantObj = Instantiate(prefab, position, Quaternion.identity, parent ?? _plantContainer);
            plantObj.layer = Mathf.RoundToInt(Mathf.Log(_plantLayer.value, 2));
            
            // Create plant instance data
            var plantInstance = new SpeedTreePlantInstance
            {
                InstanceId = instanceId,
                PlantName = $"{genotype.StrainName}_{instanceId:D4}",
                Position = position,
                Scale = Vector3.one,
                GrowthStage = PlantGrowthStage.Seedling,
                Health = 100f,
                Age = 0f,
                GeneticData = genotype,
                GameObject = plantObj,
                Renderer = plantObj.GetComponent<Renderer>(),
                LastUpdate = System.DateTime.Now
            };
            
            // Configure SpeedTree component if available
#if UNITY_SPEEDTREE
            ConfigureSpeedTreeComponent(plantInstance);
#endif
            
            // Apply genetic traits
            ApplyGeneticTraits(plantInstance, genotype);
            
            // Register with systems
            _plantInstances[instanceId] = plantInstance;
            RegisterPlantWithSystems(plantInstance);
            
            LogInfo($"Created SpeedTree plant instance: {plantInstance.PlantName}");
            return plantInstance;
        }
        
        private GameObject SelectPrefabForGenotype(CannabisGenotype genotype)
        {
            // Select prefab based on genetic characteristics
            switch (genotype.DominantType)
            {
                case StrainType.Indica:
                    return _indicaPrefab;
                case StrainType.Sativa:
                    return _sativaPrefab;
                case StrainType.Hybrid:
                    return _hybridPrefabs[Random.Range(0, _hybridPrefabs.Length)];
                default:
                    return _hybridPrefabs[0]; // Default fallback
            }
        }
        
#if UNITY_SPEEDTREE
        private void ConfigureSpeedTreeComponent(SpeedTreePlantInstance instance)
        {
            // Configure SpeedTree-specific components
            var speedTreeComponent = instance.GameObject.GetComponent<SpeedTreeWind>();
            if (speedTreeComponent != null)
            {
                // Configure wind response based on genetics
                speedTreeComponent.m_fWindStrength = instance.GeneticData.WindResistance;
                speedTreeComponent.m_fWindTurbulence = 0.5f;
            }
            
            // Configure LOD group
            var lodGroup = instance.GameObject.GetComponent<LODGroup>();
            if (lodGroup != null)
            {
                ConfigureLODGroup(lodGroup, instance);
            }
        }
        
        private void ConfigureLODGroup(LODGroup lodGroup, SpeedTreePlantInstance instance)
        {
            var lods = lodGroup.GetLODs();
            
            // Adjust LOD distances based on plant importance and performance settings
            float lodBias = instance.GeneticData.VisualImportance;
            
            for (int i = 0; i < lods.Length; i++)
            {
                lods[i].screenRelativeTransitionHeight *= lodBias;
            }
            
            lodGroup.SetLODs(lods);
        }
#endif
        
        private SpeedTreePlantInstance CreateFallbackPlantInstance(CannabisGenotype genotype, Vector3 position, Transform parent)
        {
            // Create simple fallback plant when SpeedTree is not available
            GameObject fallbackObj = CreateFallbackPlantObject(genotype, position);
            fallbackObj.transform.parent = parent ?? _plantContainer;
            
            var plantInstance = new SpeedTreePlantInstance
            {
                InstanceId = _availableInstanceIds.Dequeue(),
                PlantName = $"Fallback_{genotype.StrainName}_{System.DateTime.Now.Ticks}",
                Position = position,
                GeneticData = genotype,
                GameObject = fallbackObj,
                Renderer = fallbackObj.GetComponent<Renderer>(),
                LastUpdate = System.DateTime.Now
            };
            
            _plantInstances[plantInstance.InstanceId] = plantInstance;
            return plantInstance;
        }
        
        private GameObject CreateFallbackPlantObject(CannabisGenotype genotype, Vector3 position)
        {
            // Create simple cube with plant material as fallback
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.position = position;
            obj.transform.localScale = new Vector3(0.5f, 1f + genotype.PlantSize, 0.5f);
            
            // Apply basic coloring based on genetics
            var renderer = obj.GetComponent<Renderer>();
            var material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            material.color = Color.Lerp(Color.green, Color.yellow, genotype.LeafColor);
            renderer.material = material;
            
            return obj;
        }
        
        /// <summary>
        /// Applies genetic traits to plant visual appearance
        /// </summary>
        public void ApplyGeneticTraits(SpeedTreePlantInstance instance, CannabisGenotype genotype)
        {
            if (instance.Renderer?.material != null)
            {
                // Apply genetic visual traits
                ApplyColorGenes(instance, genotype);
                ApplyMorphologyGenes(instance, genotype);
                ApplyGrowthGenes(instance, genotype);
            }
        }
        
        private void ApplyColorGenes(SpeedTreePlantInstance instance, CannabisGenotype genotype)
        {
            var material = instance.Renderer.material;
            
            // Leaf color based on genetics
            Color baseColor = Color.green;
            Color leafColor = Color.Lerp(baseColor, genotype.LeafColor, 0.5f);
            material.color = leafColor;
            
            // Bud color for flowering plants
            if (instance.GrowthStage >= PlantGrowthStage.Flowering)
            {
                // Apply bud coloration
                Color budColor = genotype.BudColor;
                material.SetColor("_BudColor", budColor);
            }
        }
        
        private void ApplyMorphologyGenes(SpeedTreePlantInstance instance, CannabisGenotype genotype)
        {
            // Apply size scaling based on genetics
            float sizeFactor = 0.5f + (genotype.PlantSize * 0.5f); // 0.5x to 1.0x scale
            instance.GameObject.transform.localScale = Vector3.one * sizeFactor;
            instance.Scale = Vector3.one * sizeFactor;
        }
        
        private void ApplyGrowthGenes(SpeedTreePlantInstance instance, CannabisGenotype genotype)
        {
            // Set growth rate modifier based on genetics
            instance.GrowthRateModifier = genotype.GrowthRate;
            instance.FloweringTimeModifier = genotype.FloweringSpeed;
        }
        
        public void DestroyPlantInstance(int instanceId)
        {
            if (_plantInstances.TryGetValue(instanceId, out var instance))
            {
                // Unregister from systems
                UnregisterPlantFromSystems(instance);
                
                // Destroy GameObject
                if (instance.GameObject != null)
                {
                    Destroy(instance.GameObject);
                }
                
                // Remove from collections
                _plantInstances.Remove(instanceId);
                _visiblePlants.Remove(instance);
                
                // Return ID to pool
                _availableInstanceIds.Enqueue(instanceId);
                
                LogInfo($"Destroyed plant instance: {instanceId}");
            }
        }
        
        #endregion
        
        #region System Integration
        
        private void RegisterPlantWithSystems(SpeedTreePlantInstance instance)
        {
            _environmentalSystem?.RegisterPlantForMonitoring(instance);
            _growthSystem?.RegisterPlantForGrowth(instance);
            _optimizationSystem?.RegisterPlantForOptimization(instance);
        }
        
        private void UnregisterPlantFromSystems(SpeedTreePlantInstance instance)
        {
            _environmentalSystem?.UnregisterPlant(instance.InstanceId);
            _growthSystem?.UnregisterPlantFromGrowth(instance.InstanceId);
            _optimizationSystem?.UnregisterPlantFromOptimization(instance.InstanceId);
        }
        
        #endregion
        
        #region Update and Performance
        
        private void UpdatePlantInstances()
        {
            foreach (var instance in _plantInstances.Values)
            {
                UpdatePlantInstance(instance);
            }
        }
        
        private void UpdatePlantInstance(SpeedTreePlantInstance instance)
        {
            if (instance.GameObject == null) return;
            
            // Update age
            float deltaTime = Time.deltaTime;
            instance.Age += deltaTime;
            
            // Update position tracking
            instance.Position = instance.GameObject.transform.position;
            instance.LastUpdate = System.DateTime.Now;
        }
        
        private void UpdatePerformanceMetrics()
        {
            _performanceMetrics.VisiblePlants = _visiblePlants.Count;
            _performanceMetrics.TotalPlants = _plantInstances.Count;
            _performanceMetrics.AverageFrameRate = 1f / Time.smoothDeltaTime;
            _performanceMetrics.LastUpdate = System.DateTime.Now;
        }
        
        private void ProcessVisibilityOptimization()
        {
            if (!_enableLODOptimization) return;
            
            // Update visible plants list based on camera frustum
            _visiblePlants.Clear();
            Camera mainCamera = Camera.main;
            if (mainCamera == null) return;
            
            foreach (var instance in _plantInstances.Values)
            {
                if (IsPlantVisible(instance, mainCamera))
                {
                    _visiblePlants.Add(instance);
                }
            }
            
            // Sort by distance for LOD processing
            _visiblePlants.Sort((a, b) => 
                Vector3.Distance(a.Position, mainCamera.transform.position)
                .CompareTo(Vector3.Distance(b.Position, mainCamera.transform.position))
            );
            
            // Apply LOD optimization
            for (int i = 0; i < _visiblePlants.Count && i < _maxVisiblePlants; i++)
            {
                UpdatePlantLOD(_visiblePlants[i], mainCamera);
            }
        }
        
        private bool IsPlantVisible(SpeedTreePlantInstance instance, Camera camera)
        {
            if (instance.GameObject == null) return false;
            
            // Distance culling
            float distance = Vector3.Distance(instance.Position, camera.transform.position);
            if (distance > _cullingDistance) return false;
            
            // Frustum culling
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(frustumPlanes, instance.GameObject.GetComponent<Renderer>().bounds);
        }
        
        private void UpdatePlantLOD(SpeedTreePlantInstance instance, Camera camera)
        {
            float distance = Vector3.Distance(instance.Position, camera.transform.position);
            float lodFactor = distance / _cullingDistance;
            
            // Update LOD group if available
            var lodGroup = instance.GameObject.GetComponent<LODGroup>();
            if (lodGroup != null)
            {
                lodGroup.size = Mathf.Lerp(1f, 0.1f, lodFactor);
            }
        }
        
        #endregion
        
        #region Public Interface
        
        public SpeedTreePlantInstance GetPlantInstance(int instanceId)
        {
            return _plantInstances.TryGetValue(instanceId, out var instance) ? instance : null;
        }
        
        public List<SpeedTreePlantInstance> GetAllActiveInstances()
        {
            return new List<SpeedTreePlantInstance>(_plantInstances.Values);
        }
        
        public SpeedTreeSystemReport GetSystemReport()
        {
            return new SpeedTreeSystemReport
            {
                TotalPlants = _plantInstances.Count,
                VisiblePlants = _visiblePlants.Count,
                PerformanceMetrics = _performanceMetrics,
                SpeedTreeEnabled = SpeedTreeEnabled,
                SystemStatus = "Operational"
            };
        }
        
        #endregion
        
        protected override void OnManagerShutdown()
        {
            // Cleanup all plant instances
            foreach (var instance in _plantInstances.Values)
            {
                if (instance.GameObject != null)
                {
                    Destroy(instance.GameObject);
                }
            }
            
            _plantInstances.Clear();
            _visiblePlants.Clear();
            
            LogInfo("Advanced SpeedTree Manager shutdown complete");
        }
    }
}
```

**Step 2: Create Supporting Data Structures**

Create file: `Assets/ProjectChimera/Data/SpeedTree/SpeedTreeDataStructures.cs`

```csharp
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ProjectChimera.Data.SpeedTree
{
    /// <summary>
    /// SpeedTree plant instance data structure
    /// </summary>
    [System.Serializable]
    public class SpeedTreePlantInstance
    {
        public int InstanceId;
        public string PlantName;
        public Vector3 Position;
        public Vector3 Scale = Vector3.one;
        public PlantGrowthStage GrowthStage;
        public float Health = 100f;
        public float Age = 0f;
        public CannabisGenotype GeneticData;
        public GameObject GameObject;
        public Renderer Renderer;
        public DateTime LastUpdate;
        public float GrowthRateModifier = 1f;
        public float FloweringTimeModifier = 1f;
        public Dictionary<string, float> EnvironmentalModifiers = new Dictionary<string, float>();
    }
    
    /// <summary>
    /// Cannabis genotype data for SpeedTree integration
    /// </summary>
    [System.Serializable]
    public class CannabisGenotype
    {
        public string GenotypeId;
        public string StrainName;
        public StrainType DominantType;
        public float PlantSize = 1f; // 0-1 range
        public float GrowthRate = 1f; // Growth speed multiplier
        public float FloweringSpeed = 1f; // Flowering time multiplier
        public Color LeafColor = Color.green;
        public Color BudColor = Color.green;
        public float WindResistance = 0.5f; // 0-1 range
        public float VisualImportance = 1f; // LOD bias factor
        public Dictionary<string, float> TraitValues = new Dictionary<string, float>();
    }
    
    /// <summary>
    /// Plant growth stages for lifecycle management
    /// </summary>
    public enum PlantGrowthStage
    {
        Seed,
        Germination,
        Seedling,
        Vegetative,
        PreFlowering,
        Flowering,
        Harvest,
        Drying,
        Curing
    }
    
    /// <summary>
    /// Cannabis strain types for genetic classification
    /// </summary>
    public enum StrainType
    {
        Indica,
        Sativa,
        Hybrid
    }
    
    /// <summary>
    /// Performance metrics for SpeedTree system
    /// </summary>
    [System.Serializable]
    public class SpeedTreePerformanceMetrics
    {
        public int TotalPlants;
        public int VisiblePlants;
        public float AverageFrameRate;
        public float MemoryUsage;
        public int DrawCalls;
        public DateTime LastUpdate;
    }
    
    /// <summary>
    /// System status report for SpeedTree manager
    /// </summary>
    [System.Serializable]
    public class SpeedTreeSystemReport
    {
        public int TotalPlants;
        public int VisiblePlants;
        public SpeedTreePerformanceMetrics PerformanceMetrics;
        public bool SpeedTreeEnabled;
        public string SystemStatus;
        public List<string> ActiveOptimizations = new List<string>();
    }
}
```

---

## 🎨 3D Asset Creation and Integration

### Facility Infrastructure Assets

#### 1. Greenhouse Structure Modeling

**Step 1: Setting Up Blender for Cannabis Facility Modeling**

**Blender Installation and Configuration:**
1. Download Blender 4.0+ from https://www.blender.org/download/
2. Install with default settings
3. Launch Blender and delete default cube (X key → Delete → Confirm)
4. Set up workspace for architectural modeling:
   - **Units**: Scene Properties → Units → Metric, Scale 1.0
   - **Grid**: Viewport Overlays → Grid Scale 1.0m
   - **Snap Settings**: Enable snap to grid (Shift+Tab)

**Step 2: Creating Greenhouse Frame Structure**

**Basic Frame Modeling:**
1. **Add Base Rectangle** (Shift+A → Mesh → Cube):
   - Scale to greenhouse dimensions: S → X → 20 → Enter (20m width)
   - S → Y → 40 → Enter (40m length)
   - S → Z → 0.1 → Enter (0.1m thick foundation)
   - Position at ground level: G → Z → -0.05 → Enter

2. **Create Wall Framework**:
   - Add cube (Shift+A → Mesh → Cube)
   - Scale for wall post: S → 0.1 → Enter (0.1m x 0.1m post)
   - S → Z → 2.5 → Enter (5m height)
   - Position at corner: G → X → 10 → Y → 20 → Z → 2.5
   - Duplicate for all corners and intermediate posts:
     - Shift+D → X → -20 (opposite corner)
     - Shift+D → Y → -40 (create grid of posts every 4m)

3. **Roof Structure Creation**:
   - Add cube for roof beam
   - Scale: S → X → 20.2 → Y → 0.15 → Z → 0.15
   - Position at roof height: G → Z → 5
   - Create peaked roof structure:
     - Duplicate beam (Shift+D)
     - Rotate 15 degrees: R → X → 15 → Enter
     - Position for sloped roof sections

**Step 3: Glass Panel Creation**

**Transparent Wall Panels:**
1. **Create Glass Panel Mesh**:
   - Add plane (Shift+A → Mesh → Plane)
   - Scale to panel size: S → X → 1.95 → Y → 2.45 (slightly smaller than frame sections)
   - Add thickness: E → S → 0.02 (2cm thick)

2. **Glass Material Setup**:
   - Switch to Shading workspace
   - Create new material: New → Name "Greenhouse_Glass"
   - Set material properties:
     - **Base Color**: Clear white (1, 1, 1, 1)
     - **Metallic**: 0.0
     - **Roughness**: 0.1
     - **Transmission**: 1.0
     - **Alpha**: 0.9
   - Enable **Screen Space Reflections** in material settings

3. **Panel Array and Placement**:
   - Select glass panel
   - Add Array modifier: Properties → Modifier → Array
   - Count: 5 (for 5 panels per wall section)
   - Offset: X = 2.0m
   - Apply modifier (Ctrl+A)
   - Duplicate for all wall sections

**Step 4: Ventilation and Door Systems**

**Automated Vent Windows:**
1. **Create Vent Frame**:
   - Add cube, scale for vent opening: S → X → 1.0 → Y → 0.8 → Z → 0.05
   - Position in wall section designated for ventilation
   - Inset faces for frame detail: I → 0.05

2. **Moveable Vent Panel**:
   - Create separate mesh for moving vent
   - Add hinge mechanism (simple cylinder for pivot point)
   - Set up for animation (bone system for automation demonstration)

**Entry Door System:**
1. **Door Frame Creation**:
   - Add cube: S → X → 1.2 → Y → 0.15 → Z → 2.4 (standard door size)
   - Use Inset Faces (I) to create frame detail
   - Position at designated entry point

2. **Door Panel with Hardware**:
   - Create door mesh with handle detail
   - Add UV mapping for texture application
   - Create simple hinge mechanism for opening animation

#### 2. Growing Equipment Asset Creation

**Step 1: LED Light Fixture Modeling**

**High-Bay LED Light Creation:**
1. **Light Housing Base**:
   - Add cube: S → X → 1.2 → Y → 0.6 → Z → 0.15
   - Add edge loops for detail: Ctrl+R
   - Bevel edges for realistic appearance: Ctrl+B → 0.02

2. **LED Panel Array**:
   - Create small cube for individual LED: S → 0.02
   - Array modifier: Count 50 (10x5 grid)
   - Offset X: 0.025, Y: 0.025
   - Apply modifier and separate into individual objects

3. **Heat Sink Modeling**:
   - Add cube above LED panel
   - Scale: S → X → 1.2 → Y → 0.6 → Z → 0.3
   - Add Loop Cuts for fin detail: Ctrl+R → 20 cuts
   - Extrude alternate faces: Alt+A → E → 0.02

4. **Mounting Hardware**:
   - Create adjustable cable/chain system
   - Model electrical connections and junction boxes
   - Add realistic power cables (curve objects)

**Step 2: HVAC System Components**

**Air Handling Unit Modeling:**
1. **Main Unit Housing**:
   - Add cube: S → X → 2.0 → Y → 1.5 → Z → 1.2
   - Round corners with Bevel modifier
   - Add access panels with Inset Faces (I)

2. **Fan and Ductwork**:
   - Create cylindrical fan housing
   - Model fan blades using Array modifier on single blade
   - Create flexible ductwork using Curve objects with Bevel

3. **Control Panels**:
   - Model control box with detailed interface
   - Create LED indicators and control buttons
   - Add display screen geometry for UI integration

**Step 3: Growing Container and Pot Systems**

**Hydroponic System Creation:**
1. **Reservoir Tank**:
   - Add cylinder: Mesh → Cylinder
   - Scale: S → X/Y → 1.5 → Z → 0.8 (large reservoir)
   - Add thickness with Solidify modifier: 0.05

2. **Growing Channels**:
   - Create rectangular channel: Add Cube → S → X → 6 → Y → 0.3 → Z → 0.2
   - Hollow out with Inset Faces → Extrude → Scale inward
   - Array for multiple channels: Array modifier, Count: 10, Y-offset: 0.5

3. **Plant Support Net Pots**:
   - Add cylinder: S → 0.08 (8cm diameter)
   - Create mesh pattern with face selection and deletion
   - Duplicate for grid pattern matching growing holes

**Step 4: Monitoring and Sensor Equipment**

**Environmental Sensor Creation:**
1. **Temperature/Humidity Sensor**:
   - Small rectangular housing: Add Cube → S → 0.05
   - Add ventilation grilles with Inset Faces
   - Create mounting bracket system

2. **CO2 Monitoring System**:
   - Larger sensor unit with display
   - Model cable connections and mounting hardware
   - Create indicator LED arrays

**Step 5: Asset Optimization for Unity**

**Polygon Count Optimization:**
1. **LOD Mesh Creation**:
   - For each asset, create 4 versions:
     - **LOD0**: Full detail (original mesh)
     - **LOD1**: 50% polygon reduction (Decimate modifier: Ratio 0.5)
     - **LOD2**: 25% polygons (Decimate modifier: Ratio 0.25)
     - **LOD3**: 10% polygons (Decimate modifier: Ratio 0.1)

2. **UV Mapping Preparation**:
   - Select all faces: A
   - Switch to UV Editing workspace
   - Unwrap with Smart UV Project: U → Smart UV Project
   - Optimize UV islands for texture efficiency
   - Export UV layout: UV → Export UV Layout

**Unity Asset Import Configuration:**
1. **Model Import Settings**:
   - **Scale Factor**: 1
   - **Convert Units**: Enabled
   - **Import BlendShapes**: Disabled (unless needed)
   - **Import Visibility**: Enabled
   - **Import Cameras**: Disabled
   - **Import Lights**: Disabled

2. **Mesh Optimization**:
   - **Mesh Compression**: Medium
   - **Read/Write Enabled**: Disabled (unless needed for runtime modification)
   - **Optimize Mesh**: Enabled
   - **Generate Colliders**: Only for interactive objects

### Texture and Material Creation

#### 1. Cannabis-Specific Texture Development

**Step 1: Plant Surface Textures**

**Leaf Texture Creation in GIMP/Photoshop:**

1. **Base Leaf Texture** (2048x2048):
   - Create new document: 2048x2048, RGB mode
   - **Background Layer**: Medium green (RGB: 45, 82, 42)
   - **Vein Structure**:
     - Create new layer: "Leaf_Veins"
     - Use brush tool with leaf vein brush pattern
     - Color: Darker green (RGB: 30, 55, 28)
     - Apply along palmate leaf structure (5-7 main veins)

2. **Surface Detail Layer**:
   - Add subtle texture with Noise filter
   - **Filter → Noise → Perlin Noise**:
     - Detail: 10
     - X/Y Size: 1.0
     - Turbulence: 0.3
   - Set blend mode to Soft Light, Opacity: 20%

3. **Age Variation Maps**:
   - Create separate textures for different plant ages:
     - **Young_Leaf**: Bright green (RGB: 60, 120, 58)
     - **Mature_Leaf**: Standard green (RGB: 45, 82, 42)
     - **Old_Leaf**: Yellow-green (RGB: 85, 95, 40)
   - Export each as .PNG with alpha channel

**Step 2: Bud and Flower Textures**

**Trichome-Rich Bud Surface:**
1. **Base Bud Texture** (2048x2048):
   - **Base Color**: Dark green with purple hints (RGB: 35, 45, 55)
   - **Trichome Layer**:
     - Create new layer: "Trichomes"
     - Use small, soft white brush (2-3px)
     - Paint thousands of tiny dots across surface
     - Apply Gaussian Blur: 0.5px
     - Add slight glow effect

2. **Normal Map Generation**:
   - Duplicate trichrome layer
   - **Filter → 3D → Generate Normal Map**:
     - Scale: 1.0
     - Wrap: Both directions
   - Save as separate normal map file

3. **Specular/Roughness Maps**:
   - Create roughness map (trichomes = smoother)
   - Generate specular map (trichomes = more reflective)
   - Export as grayscale images

**Step 3: Facility Surface Materials**

**Concrete Floor Texture:**
1. **Seamless Concrete Base** (1024x1024):
   - **Base Color**: Light gray (RGB: 140, 140, 145)
   - Add concrete noise pattern:
     - **Filter → Noise → Add Noise**: Amount 2%, Uniform, Monochromatic
   - Create wear patterns with darker spots
   - Add subtle dirt accumulation near walls

2. **Normal Map Creation**:
   - Use concrete height map
   - Convert to normal map for surface bump detail
   - Adjust intensity for realistic surface variation

**Metal Framework Material:**
1. **Steel Beam Texture** (512x512):
   - **Base Color**: Steel gray (RGB: 180, 180, 185)
   - Add scratches and wear marks
   - Create rust spots for aged appearance
   - Paint bolts and connection details

**Step 4: Material Setup in Unity**

**Cannabis Leaf Material Configuration:**

1. **Create Material in Unity**:
   - Project window → Right-click → Create → Material
   - Name: "Cannabis_Leaf_Material"
   - Shader: Universal Render Pipeline/Lit

2. **Material Property Setup**:
   ```
   Albedo: Cannabis_Leaf_Diffuse.png
   Metallic: 0.0
   Smoothness: 0.3
   Normal Map: Cannabis_Leaf_Normal.png
   Occlusion: Cannabis_Leaf_AO.png
   Alpha Clipping: Enabled, Threshold: 0.5
   ```

3. **Advanced Material Features**:
   - **Two-Sided**: Enabled (for leaf transparency)
   - **Receive Shadows**: Enabled
   - **GPU Instancing**: Enabled (for performance)

**Bud Material with Trichrome Effects:**

1. **Create Advanced Bud Material**:
   - Shader: Universal Render Pipeline/Lit
   - **Special Properties**:
     ```
     Albedo: Cannabis_Bud_Diffuse.png
     Metallic: 0.1 (slight reflection from trichomes)
     Smoothness: 0.6 (trichrome reflectivity)
     Normal Map: Cannabis_Bud_Normal.png
     Emission: Slight green glow (RGB: 0.1, 0.2, 0.1)
     ```

2. **Shader Graph Enhancement** (Advanced):
   - Create custom shader graph for animated trichomes
   - Add sparkle effect for healthy plants
   - Include growth stage color transitions

### Prefab Creation and Organization

#### 1. Modular Facility Prefab System

**Step 1: Creating Modular Building Components**

**Wall Section Prefab:**
1. **Create Empty GameObject**: "WallSection_4m"
2. **Add Components**:
   - Wall frame (4m x 3m section)
   - Glass panels (if greenhouse section)
   - Mounting points for equipment
   - Snap points for modular connection

3. **Prefab Configuration**:
   - **Drag to Project window** to create prefab
   - **Configure Prefab Settings**:
     - Icon: Custom wall section icon
     - Tags: "WallSection", "Modular"
     - Layers: "Facility" (layer 9)

**Step 2: Equipment Mounting System**

**Universal Mounting Prefab:**
1. **Create Mounting Point System**:
   - Empty GameObject: "MountingPoint"
   - **Add Script Component**: MountingPoint.cs
   ```csharp
   public class MountingPoint : MonoBehaviour
   {
       [SerializeField] private MountingType supportedTypes;
       [SerializeField] private Vector3 equipmentOffset;
       [SerializeField] private bool isOccupied = false;
       
       public bool CanMount(EquipmentType equipment)
       {
           return !isOccupied && supportedTypes.HasFlag(equipment);
       }
       
       public void MountEquipment(GameObject equipment)
       {
           equipment.transform.parent = this.transform;
           equipment.transform.localPosition = equipmentOffset;
           isOccupied = true;
       }
   }
   ```

2. **Equipment Prefab Creation**:
   - **LED Light Fixture Prefab**:
     - Model with LOD Group component
     - Light component for illumination
     - Custom script for control integration
     - Mounting compatibility component

**Step 3: Plant Container System**

**Growing Pot Prefab Hierarchy:**
1. **Create Pot Prefab Structure**:
   ```
   GrowingPot_Prefab
   ├── PotMesh (visual container)
   ├── GrowingMedium (soil/substrate)
   ├── PlantAttachPoint (where plant spawns)
   ├── SensorMounts (4 positions for sensors)
   └── DrainageSystem (water management)
   ```

2. **Plant Spawn Point Configuration**:
   - **PlantSpawnPoint Script**:
   ```csharp
   public class PlantSpawnPoint : MonoBehaviour
   {
       [SerializeField] private PlantStrainSO defaultStrain;
       [SerializeField] private bool autoSpawn = false;
       [SerializeField] private float spawnDelay = 0f;
       
       public PlantInstance SpawnPlant(PlantStrainSO strain = null)
       {
           var plantStrain = strain ?? defaultStrain;
           return PlantManager.Instance.CreatePlant(plantStrain, transform.position, transform);
       }
   }
   ```

#### 2. Facility Layout Prefab Templates

**Step 1: Small Grow Room Template**

**Create Complete Small Room Prefab:**
1. **Room Structure** (4m x 6m):
   - **Foundation**: Concrete slab with drainage
   - **Walls**: 4 wall sections with door
   - **Ceiling**: Support structure with light mounts
   - **Ventilation**: 2 intake, 2 exhaust points

2. **Equipment Layout**:
   - **Lighting**: 6 LED fixtures in 2x3 grid
   - **HVAC**: Single air handling unit
   - **Growing**: 24 pot positions in 4x6 grid
   - **Monitoring**: 4 sensor locations

3. **Prefab Script Integration**:
   ```csharp
   public class FacilityRoom : MonoBehaviour
   {
       [Header("Room Configuration")]
       public string roomId;
       public FacilityType roomType;
       public Vector2Int dimensions;
       
       [Header("Systems")]
       public List<EquipmentController> equipment;
       public List<PlantSpawnPoint> plantPositions;
       public EnvironmentalZone environmentalZone;
       
       private void Start()
       {
           RegisterWithFacilityManager();
           InitializeEnvironmentalZone();
           SetupEquipmentConnections();
       }
   }
   ```

**Step 2: Commercial Facility Template**

**Large-Scale Growing Facility:**
1. **Facility Layout** (20m x 40m):
   - **Growing Areas**: 8 separate environmental zones
   - **Processing Area**: Harvest and drying section
   - **Storage**: Secure storage with inventory tracking
   - **Office**: Control room with monitoring systems

2. **Automation Infrastructure**:
   - **Central Control**: Master control room
   - **Sensor Network**: Environmental monitoring grid
   - **Irrigation**: Automated water/nutrient delivery
   - **Security**: Access control and surveillance

#### 3. Prefab Optimization and Performance

**Step 1: LOD Group Configuration**

**Automatic LOD Setup for Facility Prefabs:**
1. **Select Facility Prefab** in Project window
2. **Add LOD Group Component**:
   - **LOD 0** (100%-50%): Full detail with all equipment
   - **LOD 1** (50%-15%): Simplified equipment, full structure
   - **LOD 2** (15%-1%): Basic structure only
   - **Culled** (1%-0%): Not rendered

2. **Equipment LOD Strategy**:
   - **High Detail**: Individual bolts, cables, controls
   - **Medium Detail**: Main shapes, grouped details
   - **Low Detail**: Simple boxes with textures
   - **Billboard**: 2D sprite for very distant viewing

**Step 2: Occlusion Culling Setup**

**Facility Occlusion Baking:**
1. **Mark Static Objects**:
   - Select all walls, permanent equipment
   - Inspector → Static dropdown → Occluder Static
   - Also mark as Navigation Static for pathfinding

2. **Bake Occlusion Data**:
   - Window → Rendering → Occlusion Culling
   - Adjust settings:
     - **Smallest Occluder**: 1.0 (walls will occlude)
     - **Smallest Hole**: 0.25 (openings this size won't occlude)
     - **Backface Threshold**: 100 (optimize backface removal)
   - Click **Bake** and wait for completion

---

## 🏗️ Scene Construction and Environment Building

### Master Scene Setup and Organization

#### 1. Scene Hierarchy Structure

**Step 1: Creating the Master Scene**

**Initial Scene Setup:**
1. **Create New Scene**:
   - File → New Scene
   - Choose "Basic (URP)" template
   - Save as: `Assets/ProjectChimera/Scenes/MasterFacility.unity`

2. **Delete Default Objects**:
   - Delete default "Main Camera" (we'll use Cinemachine)
   - Delete default "Directional Light" (we'll create custom lighting)
   - Keep default plane temporarily as reference

**Step 2: Organized Hierarchy Structure**

**Create Scene Organization GameObjects:**
1. **Root Level Organization** (Create Empty GameObjects):
   ```
   --- ENVIRONMENT ---
   ├── Lighting
   │   ├── GlobalLighting
   │   ├── FacilityLighting
   │   └── PlantLighting
   ├── Architecture
   │   ├── Foundations
   │   ├── WallSystems
   │   ├── RoofSystems
   │   └── Flooring
   ├── Equipment
   │   ├── HVAC_Systems
   │   ├── Electrical_Systems
   │   ├── PlumbingSystems
   │   └── MonitoringSystems
   └── Environmental
       ├── ClimatZones
       ├── AirFlow
       └── SoundSystems
   
   --- CULTIVATION ---
   ├── GrowingAreas
   │   ├── VegetativeRoom
   │   ├── FloweringRoom
   │   ├── SeedlingArea
   │   └── CloneArea
   ├── Plants
   │   ├── ActivePlants
   │   ├── SpawnPoints
   │   └── PlantContainers
   └── ProcessingAreas
       ├── HarvestStation
       ├── DryingArea
       └── CuringRoom
   
   --- SYSTEMS ---
   ├── Managers
   │   ├── GameManager
   │   ├── FacilityManager
   │   ├── PlantManager
   │   └── EnvironmentalManager
   ├── CameraSystems
   │   ├── CinemachineManager
   │   ├── VirtualCameras
   │   └── CameraTargets
   ├── UI_Systems
   │   ├── WorldSpaceUI
   │   ├── InteractionPrompts
   │   └── DebugOverlays
   └── Audio
       ├── EnvironmentalAudio
       ├── EquipmentSounds
       └── AmbientSounds
   
   --- INTERACTION ---
   ├── PlayerSystems
   │   ├── PlayerController
   │   ├── InteractionSystem
   │   └── InventorySystem
   ├── SelectionSystem
   │   ├── PlantSelection
   │   ├── EquipmentSelection
   │   └── AreaSelection
   └── InputHandlers
       ├── MouseInput
       ├── KeyboardInput
       └── TouchInput
   ```

**Step 3: Scene Settings Configuration**

**Lighting Settings Setup:**
1. **Open Lighting Window**: Window → Rendering → Lighting
2. **Environment Tab Configuration**:
   - **Environment Lighting**:
     - Source: Skybox
     - Intensity Multiplier: 1.0
     - Ambient Mode: Realtime
   - **Environment Reflections**:
     - Source: Skybox
     - Resolution: 128 (for performance)
     - Realtime Resolution: 128

3. **Lightmapping Settings**:
   - **Realtime Global Illumination**: Disabled (for performance)
   - **Baked Global Illumination**: Enabled
   - **Auto Generate**: Disabled (manual control)

**Fog and Atmosphere Setup:**
1. **Window → Rendering → Lighting → Environment**:
   - **Fog**: Enabled
   - **Color**: Light blue-gray (RGB: 0.5, 0.6, 0.7, 1.0)
   - **Mode**: Linear
   - **Start**: 50m
   - **End**: 200m
   - **Density**: 0.01

#### 2. Environmental Zone Creation

**Step 1: Climate Zone System**

**Create Environmental Zone Prefab:**
1. **Create Empty GameObject**: "EnvironmentalZone"
2. **Add Box Collider**:
   - **Is Trigger**: Enabled
   - **Size**: Adjust to room dimensions
   - **Center**: Position in room center

3. **Add Environmental Zone Script**:

Create file: `Assets/ProjectChimera/Scripts/Environment/EnvironmentalZone.cs`

```csharp
using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Environment;
using System.Collections.Generic;

namespace ProjectChimera.Systems.Environment
{
    /// <summary>
    /// Defines and manages an environmental zone with specific climate conditions
    /// </summary>
    public class EnvironmentalZone : MonoBehaviour
    {
        [Header("Zone Configuration")]
        [SerializeField] private string zoneId;
        [SerializeField] private ZoneType zoneType;
        [SerializeField] private Color zoneColor = Color.green;
        
        [Header("Target Conditions")]
        [SerializeField] private EnvironmentalConditions targetConditions;
        [SerializeField] private EnvironmentalTolerances tolerances;
        
        [Header("Equipment References")]
        [SerializeField] private List<EnvironmentalEquipment> hvacEquipment;
        [SerializeField] private List<EnvironmentalEquipment> lightingEquipment;
        [SerializeField] private List<EnvironmentalSensor> sensors;
        
        [Header("Visualization")]
        [SerializeField] private bool showZoneVisualization = true;
        [SerializeField] private bool showSensorReadings = true;
        
        // Current state
        private EnvironmentalConditions _currentConditions;
        private List<PlantInstance> _plantsInZone = new List<PlantInstance>();
        private ZoneControlSystem _controlSystem;
        
        // Events
        public System.Action<EnvironmentalConditions> OnConditionsChanged;
        public System.Action<PlantInstance> OnPlantEntered;
        public System.Action<PlantInstance> OnPlantExited;
        
        // Properties
        public string ZoneId => zoneId;
        public ZoneType Type => zoneType;
        public EnvironmentalConditions CurrentConditions => _currentConditions;
        public EnvironmentalConditions TargetConditions => targetConditions;
        public int PlantCount => _plantsInZone.Count;
        
        private void Start()
        {
            InitializeZone();
            RegisterWithEnvironmentalManager();
            SetupVisualization();
        }
        
        private void Update()
        {
            UpdateEnvironmentalConditions();
            ProcessEquipmentControl();
            UpdateVisualization();
        }
        
        #region Initialization
        
        private void InitializeZone()
        {
            // Generate unique zone ID if not set
            if (string.IsNullOrEmpty(zoneId))
            {
                zoneId = $"Zone_{System.Guid.NewGuid().ToString("N")[..8]}";
            }
            
            // Initialize current conditions to target
            _currentConditions = new EnvironmentalConditions(targetConditions);
            
            // Initialize control system
            _controlSystem = new ZoneControlSystem(this);
            
            // Setup equipment references
            ConnectEquipment();
            ConnectSensors();
            
            Debug.Log($"Environmental Zone initialized: {zoneId} ({zoneType})");
        }
        
        private void ConnectEquipment()
        {
            // Auto-discover equipment if not manually assigned
            if (hvacEquipment.Count == 0 || lightingEquipment.Count == 0)
            {
                var equipmentInArea = GetComponentsInChildren<EnvironmentalEquipment>();
                foreach (var equipment in equipmentInArea)
                {
                    switch (equipment.EquipmentType)
                    {
                        case EnvironmentalEquipmentType.HVAC:
                            if (!hvacEquipment.Contains(equipment))
                                hvacEquipment.Add(equipment);
                            break;
                        case EnvironmentalEquipmentType.Lighting:
                            if (!lightingEquipment.Contains(equipment))
                                lightingEquipment.Add(equipment);
                            break;
                    }
                }
            }
            
            // Connect equipment to zone
            foreach (var equipment in hvacEquipment)
            {
                equipment.SetControllingZone(this);
            }
            foreach (var equipment in lightingEquipment)
            {
                equipment.SetControllingZone(this);
            }
        }
        
        private void ConnectSensors()
        {
            // Auto-discover sensors if not manually assigned
            if (sensors.Count == 0)
            {
                var sensorsInArea = GetComponentsInChildren<EnvironmentalSensor>();
                foreach (var sensor in sensorsInArea)
                {
                    sensors.Add(sensor);
                    sensor.OnReadingUpdated += HandleSensorReading;
                }
            }
        }
        
        private void RegisterWithEnvironmentalManager()
        {
            var envManager = FindObjectOfType<EnvironmentalManager>();
            if (envManager != null)
            {
                envManager.RegisterZone(this);
            }
        }
        
        #endregion
        
        #region Environmental Control
        
        private void UpdateEnvironmentalConditions()
        {
            // Collect sensor readings
            var sensorReadings = CollectSensorReadings();
            
            // Update current conditions based on sensors
            if (sensorReadings.Count > 0)
            {
                _currentConditions = AverageSensorReadings(sensorReadings);
                OnConditionsChanged?.Invoke(_currentConditions);
            }
        }
        
        private List<EnvironmentalConditions> CollectSensorReadings()
        {
            var readings = new List<EnvironmentalConditions>();
            
            foreach (var sensor in sensors)
            {
                if (sensor.IsActive && sensor.LastReading != null)
                {
                    readings.Add(sensor.LastReading);
                }
            }
            
            return readings;
        }
        
        private EnvironmentalConditions AverageSensorReadings(List<EnvironmentalConditions> readings)
        {
            if (readings.Count == 0) return _currentConditions;
            
            var averaged = new EnvironmentalConditions();
            
            // Average all sensor values
            foreach (var reading in readings)
            {
                averaged.Temperature += reading.Temperature;
                averaged.Humidity += reading.Humidity;
                averaged.CO2Level += reading.CO2Level;
                averaged.PPFD += reading.PPFD;
                averaged.AirFlow += reading.AirFlow;
            }
            
            // Divide by count to get averages
            int count = readings.Count;
            averaged.Temperature /= count;
            averaged.Humidity /= count;
            averaged.CO2Level /= count;
            averaged.PPFD /= count;
            averaged.AirFlow /= count;
            averaged.Timestamp = System.DateTime.Now;
            
            return averaged;
        }
        
        private void ProcessEquipmentControl()
        {
            _controlSystem?.UpdateEquipmentControl(_currentConditions, targetConditions);
        }
        
        private void HandleSensorReading(EnvironmentalSensor sensor, EnvironmentalConditions reading)
        {
            // Individual sensor reading received
            // Could trigger immediate responses for critical values
            
            if (reading.Temperature > targetConditions.Temperature + tolerances.TemperatureTolerance)
            {
                // Emergency cooling activation
                ActivateEmergencyCooling();
            }
            else if (reading.Temperature < targetConditions.Temperature - tolerances.TemperatureTolerance)
            {
                // Emergency heating activation
                ActivateEmergencyHeating();
            }
        }
        
        private void ActivateEmergencyCooling()
        {
            foreach (var hvac in hvacEquipment)
            {
                if (hvac.CanCool)
                {
                    hvac.SetCoolingOutput(1.0f); // Maximum cooling
                }
            }
            Debug.LogWarning($"Emergency cooling activated in zone {zoneId}");
        }
        
        private void ActivateEmergencyHeating()
        {
            foreach (var hvac in hvacEquipment)
            {
                if (hvac.CanHeat)
                {
                    hvac.SetHeatingOutput(1.0f); // Maximum heating
                }
            }
            Debug.LogWarning($"Emergency heating activated in zone {zoneId}");
        }
        
        #endregion
        
        #region Plant Management
        
        private void OnTriggerEnter(Collider other)
        {
            var plant = other.GetComponent<PlantInstance>();
            if (plant != null && !_plantsInZone.Contains(plant))
            {
                _plantsInZone.Add(plant);
                plant.SetEnvironmentalZone(this);
                OnPlantEntered?.Invoke(plant);
                
                Debug.Log($"Plant {plant.PlantID} entered zone {zoneId}");
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            var plant = other.GetComponent<PlantInstance>();
            if (plant != null && _plantsInZone.Contains(plant))
            {
                _plantsInZone.Remove(plant);
                plant.SetEnvironmentalZone(null);
                OnPlantExited?.Invoke(plant);
                
                Debug.Log($"Plant {plant.PlantID} exited zone {zoneId}");
            }
        }
        
        public void UpdatePlantsWithConditions()
        {
            foreach (var plant in _plantsInZone)
            {
                plant.UpdateEnvironmentalConditions(_currentConditions);
            }
        }
        
        #endregion
        
        #region Visualization
        
        private void SetupVisualization()
        {
            if (showZoneVisualization)
            {
                // Add visual indicators for zone boundaries
                CreateZoneVisualization();
            }
        }
        
        private void CreateZoneVisualization()
        {
            // Create zone boundary visualization
            var visualizer = new GameObject("ZoneVisualizer");
            visualizer.transform.parent = transform;
            visualizer.transform.localPosition = Vector3.zero;
            
            // Add line renderer for zone boundaries
            var lineRenderer = visualizer.AddComponent<LineRenderer>();
            lineRenderer.material = CreateZoneMaterial();
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.useWorldSpace = false;
            
            // Create boundary lines
            var bounds = GetComponent<BoxCollider>().bounds;
            CreateBoundaryLines(lineRenderer, bounds);
        }
        
        private Material CreateZoneMaterial()
        {
            var material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            material.color = new Color(zoneColor.r, zoneColor.g, zoneColor.b, 0.5f);
            return material;
        }
        
        private void CreateBoundaryLines(LineRenderer lineRenderer, Bounds bounds)
        {
            // Create wireframe cube for zone visualization
            var corners = new Vector3[16]; // 8 corners * 2 for line segments
            
            // Calculate corner positions
            var min = bounds.min - transform.position;
            var max = bounds.max - transform.position;
            
            // Bottom face
            corners[0] = new Vector3(min.x, min.y, min.z);
            corners[1] = new Vector3(max.x, min.y, min.z);
            corners[2] = new Vector3(max.x, min.y, max.z);
            corners[3] = new Vector3(min.x, min.y, max.z);
            
            // Top face
            corners[4] = new Vector3(min.x, max.y, min.z);
            corners[5] = new Vector3(max.x, max.y, min.z);
            corners[6] = new Vector3(max.x, max.y, max.z);
            corners[7] = new Vector3(min.x, max.y, max.z);
            
            lineRenderer.positionCount = corners.Length;
            lineRenderer.SetPositions(corners);
        }
        
        private void UpdateVisualization()
        {
            if (showSensorReadings)
            {
                // Update any debug UI or gizmos showing current readings
                UpdateSensorVisualization();
            }
        }
        
        private void UpdateSensorVisualization()
        {
            // This would update any UI elements showing sensor data
            // Could be implemented with world-space UI or debug gizmos
        }
        
        private void OnDrawGizmosSelected()
        {
            // Draw zone boundaries and sensor positions when selected
            Gizmos.color = zoneColor;
            
            var collider = GetComponent<BoxCollider>();
            if (collider != null)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawWireCube(collider.center, collider.size);
                
                // Draw sensor positions
                Gizmos.color = Color.yellow;
                foreach (var sensor in sensors)
                {
                    if (sensor != null)
                    {
                        Gizmos.DrawWireSphere(sensor.transform.position - transform.position, 0.2f);
                    }
                }
            }
        }
        
        #endregion
        
        #region Public Interface
        
        public void SetTargetConditions(EnvironmentalConditions newConditions)
        {
            targetConditions = newConditions;
            Debug.Log($"Zone {zoneId} target conditions updated");
        }
        
        public void SetTolerances(EnvironmentalTolerances newTolerances)
        {
            tolerances = newTolerances;
        }
        
        public EnvironmentalZoneReport GetZoneReport()
        {
            return new EnvironmentalZoneReport
            {
                ZoneId = zoneId,
                ZoneType = zoneType,
                CurrentConditions = _currentConditions,
                TargetConditions = targetConditions,
                PlantCount = _plantsInZone.Count,
                EquipmentCount = hvacEquipment.Count + lightingEquipment.Count,
                SensorCount = sensors.Count,
                LastUpdate = System.DateTime.Now
            };
        }
        
        #endregion
    }
    
    /// <summary>
    /// Zone control system for managing equipment based on conditions
    /// </summary>
    public class ZoneControlSystem
    {
        private EnvironmentalZone _zone;
        private float _lastUpdateTime;
        private const float UPDATE_INTERVAL = 1.0f; // Update every second
        
        public ZoneControlSystem(EnvironmentalZone zone)
        {
            _zone = zone;
        }
        
        public void UpdateEquipmentControl(EnvironmentalConditions current, EnvironmentalConditions target)
        {
            if (Time.time - _lastUpdateTime < UPDATE_INTERVAL) return;
            
            ControlTemperature(current.Temperature, target.Temperature);
            ControlHumidity(current.Humidity, target.Humidity);
            ControlLighting(current.PPFD, target.PPFD);
            ControlAirflow(current.AirFlow, target.AirFlow);
            
            _lastUpdateTime = Time.time;
        }
        
        private void ControlTemperature(float current, float target)
        {
            float difference = target - current;
            float tolerance = 0.5f; // 0.5°C tolerance
            
            if (Mathf.Abs(difference) <= tolerance) return; // Within tolerance
            
            if (difference > 0) // Need heating
            {
                float heatingPower = Mathf.Clamp01(difference / 5.0f); // Ramp up over 5°C difference
                SetHVACHeating(heatingPower);
                SetHVACCooling(0f);
            }
            else // Need cooling
            {
                float coolingPower = Mathf.Clamp01(-difference / 5.0f);
                SetHVACCooling(coolingPower);
                SetHVACHeating(0f);
            }
        }
        
        private void ControlHumidity(float current, float target)
        {
            float difference = target - current;
            float tolerance = 2.0f; // 2% RH tolerance
            
            if (Mathf.Abs(difference) <= tolerance) return;
            
            if (difference > 0) // Need humidification
            {
                float humidificationPower = Mathf.Clamp01(difference / 20.0f);
                SetHumidification(humidificationPower);
                SetDehumidification(0f);
            }
            else // Need dehumidification
            {
                float dehumidificationPower = Mathf.Clamp01(-difference / 20.0f);
                SetDehumidification(dehumidificationPower);
                SetHumidification(0f);
            }
        }
        
        private void ControlLighting(float current, float target)
        {
            float difference = target - current;
            float tolerance = 20.0f; // 20 PPFD tolerance
            
            if (Mathf.Abs(difference) <= tolerance) return;
            
            float lightingPower = Mathf.Clamp01(target / 1000.0f); // Scale to max 1000 PPFD
            SetLightingOutput(lightingPower);
        }
        
        private void ControlAirflow(float current, float target)
        {
            float airflowPower = Mathf.Clamp01(target / 10.0f); // Scale to max 10 m/s
            SetAirflowOutput(airflowPower);
        }
        
        // HVAC control methods
        private void SetHVACHeating(float power)
        {
            // Implementation would control actual HVAC equipment
        }
        
        private void SetHVACCooling(float power)
        {
            // Implementation would control actual HVAC equipment
        }
        
        private void SetHumidification(float power)
        {
            // Implementation would control humidification equipment
        }
        
        private void SetDehumidification(float power)
        {
            // Implementation would control dehumidification equipment
        }
        
        private void SetLightingOutput(float power)
        {
            // Implementation would control lighting equipment
        }
        
        private void SetAirflowOutput(float power)
        {
            // Implementation would control fan equipment
        }
    }
}
```

**Step 2: Implementing Zone Types**

**Create Zone Type Definitions:**

Create file: `Assets/ProjectChimera/Data/Environment/ZoneTypes.cs`

```csharp
using UnityEngine;
using System;

namespace ProjectChimera.Data.Environment
{
    /// <summary>
    /// Different types of environmental zones in the facility
    /// </summary>
    public enum ZoneType
    {
        Seedling,       // Young plants, high humidity, moderate light
        Vegetative,     // Growing plants, moderate conditions, high light
        Flowering,      // Flowering plants, lower humidity, specific light spectrum
        Drying,         // Post-harvest drying, low humidity, air circulation
        Curing,         // Final curing, controlled humidity and temperature
        Storage,        // Long-term storage, stable cool conditions
        Processing,     // Work areas, comfortable human conditions
        Laboratory      // Testing and analysis areas
    }
    
    /// <summary>
    /// Environmental tolerances for precise control
    /// </summary>
    [System.Serializable]
    public class EnvironmentalTolerances
    {
        [Range(0.1f, 5.0f)]
        public float TemperatureTolerance = 1.0f; // ±°C
        
        [Range(1f, 10f)]
        public float HumidityTolerance = 5.0f; // ±%RH
        
        [Range(10f, 100f)]
        public float CO2Tolerance = 50.0f; // ±ppm
        
        [Range(10f, 100f)]
        public float PPFDTolerance = 50.0f; // ±μmol/m²/s
        
        [Range(0.1f, 2.0f)]
        public float AirflowTolerance = 0.5f; // ±m/s
    }
    
    /// <summary>
    /// Zone status report for monitoring and analytics
    /// </summary>
    [System.Serializable]
    public class EnvironmentalZoneReport
    {
        public string ZoneId;
        public ZoneType ZoneType;
        public EnvironmentalConditions CurrentConditions;
        public EnvironmentalConditions TargetConditions;
        public int PlantCount;
        public int EquipmentCount;
        public int SensorCount;
        public bool IsWithinTolerances;
        public DateTime LastUpdate;
        public string[] ActiveAlerts;
    }
}
```

#### 3. Advanced Lighting System Setup

**Step 1: Global Lighting Configuration**

**Master Lighting Setup:**
1. **Create Lighting Control Object**:
   - Empty GameObject: "GlobalLightingController"
   - Position: (0, 0, 0)
   - Add script: GlobalLightingController.cs

**Create Global Lighting Controller:**

```csharp
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using ProjectChimera.Core;
using System.Collections.Generic;

namespace ProjectChimera.Systems.Lighting
{
    /// <summary>
    /// Master lighting controller for facility lighting management
    /// </summary>
    public class GlobalLightingController : MonoBehaviour
    {
        [Header("Global Lighting Settings")]
        [SerializeField] private Light sunLight;
        [SerializeField] private Gradient sunColorCurve;
        [SerializeField] private AnimationCurve sunIntensityCurve;
        
        [Header("Facility Lighting")]
        [SerializeField] private List<FacilityLightGroup> lightGroups;
        [SerializeField] private Volume postProcessVolume;
        
        [Header("Time of Day")]
        [SerializeField] private bool enableTimeOfDay = true;
        [SerializeField] private float timeSpeed = 1.0f;
        [SerializeField] private float currentTime = 12.0f; // 12:00 PM start
        
        // Lighting state management
        private Dictionary<string, FacilityLightGroup> _lightGroupLookup;
        private ColorAdjustments _colorAdjustments;
        private bool _isInitialized = false;
        
        // Properties
        public float CurrentTime => currentTime;
        public bool IsNightTime => currentTime < 6f || currentTime > 20f;
        
        private void Start()
        {
            InitializeLightingSystem();
            SetupPostProcessing();
            InitializeLightGroups();
        }
        
        private void Update()
        {
            if (enableTimeOfDay)
            {
                UpdateTimeOfDay();
                UpdateSunLighting();
            }
            
            UpdateFacilityLighting();
            UpdatePostProcessing();
        }
        
        #region Initialization
        
        private void InitializeLightingSystem()
        {
            // Find or create sun light
            if (sunLight == null)
            {
                CreateSunLight();
            }
            
            // Initialize lookup dictionary
            _lightGroupLookup = new Dictionary<string, FacilityLightGroup>();
            
            // Setup default lighting curves if not assigned
            SetupDefaultLightingCurves();
            
            _isInitialized = true;
            Debug.Log("Global Lighting Controller initialized");
        }
        
        private void CreateSunLight()
        {
            var sunObject = new GameObject("Sun Light");
            sunObject.transform.parent = transform;
            sunObject.transform.rotation = Quaternion.Euler(45f, 30f, 0f);
            
            sunLight = sunObject.AddComponent<Light>();
            sunLight.type = LightType.Directional;
            sunLight.intensity = 2.0f;
            sunLight.color = Color.white;
            sunLight.shadows = LightShadows.Soft;
            sunLight.shadowStrength = 0.8f;
            
            // Configure for URP
            var additionalLightData = sunObject.AddComponent<UniversalAdditionalLightData>();
            additionalLightData.lightType = LightType.Directional;
            additionalLightData.shadowsType = ShadowsType.SoftShadows;
        }
        
        private void SetupDefaultLightingCurves()
        {
            if (sunColorCurve == null || sunColorCurve.colorKeys.Length == 0)
            {
                sunColorCurve = new Gradient();
                var colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(new Color(0.2f, 0.3f, 0.5f), 0.0f),    // Night
                    new GradientColorKey(new Color(1.0f, 0.5f, 0.3f), 0.25f),   // Sunrise
                    new GradientColorKey(new Color(1.0f, 0.95f, 0.8f), 0.5f),   // Noon
                    new GradientColorKey(new Color(1.0f, 0.7f, 0.4f), 0.75f),   // Sunset
                    new GradientColorKey(new Color(0.2f, 0.3f, 0.5f), 1.0f)     // Night
                };
                var alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1.0f, 0.0f),
                    new GradientAlphaKey(1.0f, 1.0f)
                };
                sunColorCurve.SetKeys(colorKeys, alphaKeys);
            }
            
            if (sunIntensityCurve == null || sunIntensityCurve.length == 0)
            {
                sunIntensityCurve = new AnimationCurve();
                sunIntensityCurve.AddKey(0.0f, 0.1f);   // Night
                sunIntensityCurve.AddKey(0.25f, 1.5f);  // Sunrise
                sunIntensityCurve.AddKey(0.5f, 3.0f);   // Noon
                sunIntensityCurve.AddKey(0.75f, 1.5f);  // Sunset
                sunIntensityCurve.AddKey(1.0f, 0.1f);   // Night
            }
        }
        
        private void SetupPostProcessing()
        {
            if (postProcessVolume == null)
            {
                var volumeObject = new GameObject("Global Post Process Volume");
                volumeObject.transform.parent = transform;
                
                postProcessVolume = volumeObject.AddComponent<Volume>();
                postProcessVolume.isGlobal = true;
                postProcessVolume.priority = 1;
                
                // Create or assign post-process profile
                CreatePostProcessProfile();
            }
            
            // Get color adjustments component
            if (postProcessVolume.profile.TryGet<ColorAdjustments>(out _colorAdjustments))
            {
                _colorAdjustments.active = true;
            }
        }
        
        private void CreatePostProcessProfile()
        {
            // This would create a VolumeProfile asset with appropriate settings
            // For now, assign manually in inspector or create via script
            Debug.Log("Post-process profile should be assigned in inspector");
        }
        
        private void InitializeLightGroups()
        {
            // Register all light groups
            foreach (var group in lightGroups)
            {
                if (group != null && !string.IsNullOrEmpty(group.GroupName))
                {
                    _lightGroupLookup[group.GroupName] = group;
                    group.Initialize();
                }
            }
        }
        
        #endregion
        
        #region Time of Day System
        
        private void UpdateTimeOfDay()
        {
            currentTime += (timeSpeed * Time.deltaTime) / 3600f; // Convert seconds to hours
            
            if (currentTime >= 24f)
            {
                currentTime -= 24f; // Wrap around to next day
            }
        }
        
        private void UpdateSunLighting()
        {
            if (sunLight == null) return;
            
            // Calculate normalized time (0-1 for full day cycle)
            float normalizedTime = currentTime / 24f;
            
            // Update sun rotation based on time
            float sunAngle = (normalizedTime * 360f) - 90f; // -90 to start at horizon
            sunLight.transform.rotation = Quaternion.Euler(sunAngle, 30f, 0f);
            
            // Update sun color and intensity
            sunLight.color = sunColorCurve.Evaluate(normalizedTime);
            sunLight.intensity = sunIntensityCurve.Evaluate(normalizedTime);
            
            // Disable sun during night hours
            sunLight.enabled = !IsNightTime;
        }
        
        #endregion
        
        #region Facility Lighting Control
        
        private void UpdateFacilityLighting()
        {
            foreach (var group in lightGroups)
            {
                if (group != null)
                {
                    group.UpdateLighting(currentTime, IsNightTime);
                }
            }
        }
        
        public void SetLightGroupIntensity(string groupName, float intensity)
        {
            if (_lightGroupLookup.TryGetValue(groupName, out var group))
            {
                group.SetIntensity(intensity);
            }
        }
        
        public void SetLightGroupColor(string groupName, Color color)
        {
            if (_lightGroupLookup.TryGetValue(groupName, out var group))
            {
                group.SetColor(color);
            }
        }
        
        public void EnableLightGroup(string groupName, bool enabled)
        {
            if (_lightGroupLookup.TryGetValue(groupName, out var group))
            {
                group.SetEnabled(enabled);
            }
        }
        
        #endregion
        
        #region Post Processing
        
        private void UpdatePostProcessing()
        {
            if (_colorAdjustments == null) return;
            
            // Adjust post-processing based on time of day
            float normalizedTime = currentTime / 24f;
            
            // Night time adjustments
            if (IsNightTime)
            {
                _colorAdjustments.contrast.Override(10f);       // Increase contrast
                _colorAdjustments.saturation.Override(-20f);    // Decrease saturation
                _colorAdjustments.hueShift.Override(-10f);      // Slight blue shift
            }
            else
            {
                _colorAdjustments.contrast.Override(0f);
                _colorAdjustments.saturation.Override(0f);
                _colorAdjustments.hueShift.Override(0f);
            }
        }
        
        #endregion
        
        #region Public Interface
        
        public void SetTimeOfDay(float hour)
        {
            currentTime = Mathf.Clamp(hour, 0f, 24f);
        }
        
        public void SetTimeSpeed(float speed)
        {
            timeSpeed = speed;
        }
        
        public void RegisterLightGroup(FacilityLightGroup group)
        {
            if (group != null && !string.IsNullOrEmpty(group.GroupName))
            {
                if (!_lightGroupLookup.ContainsKey(group.GroupName))
                {
                    lightGroups.Add(group);
                    _lightGroupLookup[group.GroupName] = group;
                    group.Initialize();
                }
            }
        }
        
        public LightingSystemReport GetSystemReport()
        {
            return new LightingSystemReport
            {
                CurrentTime = currentTime,
                IsNightTime = IsNightTime,
                SunIntensity = sunLight?.intensity ?? 0f,
                ActiveLightGroups = lightGroups.Count,
                TotalLights = GetTotalLightCount(),
                LastUpdate = System.DateTime.Now
            };
        }
        
        private int GetTotalLightCount()
        {
            int total = 0;
            foreach (var group in lightGroups)
            {
                total += group?.LightCount ?? 0;
            }
            return total;
        }
        
        #endregion
    }
    
    /// <summary>
    /// Group of lights that can be controlled together
    /// </summary>
    [System.Serializable]
    public class FacilityLightGroup
    {
        [Header("Group Configuration")]
        public string GroupName;
        public LightGroupType GroupType;
        public List<Light> Lights;
        
        [Header("Control Settings")]
        public bool AutoControl = true;
        public AnimationCurve IntensityCurve;
        public Gradient ColorCurve;
        
        private bool _isInitialized = false;
        
        public int LightCount => Lights?.Count ?? 0;
        
        public void Initialize()
        {
            if (_isInitialized) return;
            
            // Validate lights list
            if (Lights == null)
            {
                Lights = new List<Light>();
            }
            
            // Remove null references
            Lights.RemoveAll(light => light == null);
            
            _isInitialized = true;
        }
        
        public void UpdateLighting(float currentTime, bool isNightTime)
        {
            if (!AutoControl || !_isInitialized) return;
            
            float normalizedTime = currentTime / 24f;
            
            // Update based on group type
            switch (GroupType)
            {
                case LightGroupType.Interior:
                    UpdateInteriorLighting(isNightTime);
                    break;
                case LightGroupType.Growing:
                    UpdateGrowingLighting(normalizedTime);
                    break;
                case LightGroupType.Emergency:
                    // Emergency lights always on at low intensity
                    SetIntensity(0.1f);
                    break;
            }
        }
        
        private void UpdateInteriorLighting(bool isNightTime)
        {
            // Interior lights on during night, off during day
            SetEnabled(isNightTime);
            if (isNightTime)
            {
                SetIntensity(1.0f);
                SetColor(Color.white);
            }
        }
        
        private void UpdateGrowingLighting(float normalizedTime)
        {
            // Growing lights follow specific schedule
            // This would be customized based on plant growth requirements
            bool growingHours = normalizedTime >= 0.25f && normalizedTime <= 0.75f; // 6 AM to 6 PM
            
            SetEnabled(growingHours);
            if (growingHours)
            {
                float intensity = IntensityCurve?.Evaluate(normalizedTime) ?? 1.0f;
                Color color = ColorCurve?.Evaluate(normalizedTime) ?? Color.white;
                
                SetIntensity(intensity);
                SetColor(color);
            }
        }
        
        public void SetIntensity(float intensity)
        {
            foreach (var light in Lights)
            {
                if (light != null)
                {
                    light.intensity = intensity;
                }
            }
        }
        
        public void SetColor(Color color)
        {
            foreach (var light in Lights)
            {
                if (light != null)
                {
                    light.color = color;
                }
            }
        }
        
        public void SetEnabled(bool enabled)
        {
            foreach (var light in Lights)
            {
                if (light != null)
                {
                    light.enabled = enabled;
                }
            }
        }
    }
    
    public enum LightGroupType
    {
        Interior,    // General facility lighting
        Growing,     // Plant growing lights
        Emergency,   // Emergency/safety lighting
        Accent,      // Decorative/accent lighting
        Security     // Security lighting
    }
    
    [System.Serializable]
    public class LightingSystemReport
    {
        public float CurrentTime;
        public bool IsNightTime;
        public float SunIntensity;
        public int ActiveLightGroups;
        public int TotalLights;
        public System.DateTime LastUpdate;
    }
}
```

#### 4. Facility Layout Construction System

**Step 1: Modular Building Framework**

**Create Modular Construction Manager:**

Create file: `Assets/ProjectChimera/Scripts/Construction/ModularConstructionManager.cs`

```csharp
using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Construction;
using System.Collections.Generic;
using System.Linq;

namespace ProjectChimera.Systems.Construction
{
    /// <summary>
    /// Manages modular facility construction with snap-to-grid system
    /// </summary>
    public class ModularConstructionManager : ChimeraManager
    {
        [Header("Construction Settings")]
        [SerializeField] private float gridSize = 1.0f;
        [SerializeField] private LayerMask constructionLayer = 1 << 10;
        [SerializeField] private Material previewMaterial;
        [SerializeField] private Material validPlacementMaterial;
        [SerializeField] private Material invalidPlacementMaterial;
        
        [Header("Module Libraries")]
        [SerializeField] private ModuleLibrarySO wallModules;
        [SerializeField] private ModuleLibrarySO floorModules;
        [SerializeField] private ModuleLibrarySO roofModules;
        [SerializeField] private ModuleLibrarySO equipmentModules;
        
        [Header("Construction Area")]
        [SerializeField] private Bounds constructionBounds = new Bounds(Vector3.zero, new Vector3(100, 20, 100));
        [SerializeField] private Transform constructionParent;
        
        // Construction state
        private Dictionary<Vector3Int, PlacedModule> _placedModules = new Dictionary<Vector3Int, PlacedModule>();
        private Dictionary<string, ModuleDefinitionSO> _moduleDefinitions = new Dictionary<string, ModuleDefinitionSO>();
        private ConstructionMode _currentMode = ConstructionMode.None;
        private ModuleDefinitionSO _selectedModule;
        private GameObject _previewObject;
        private Vector3Int _previewPosition;
        private bool _isValidPlacement = false;
        
        // Construction validation
        private ConstructionValidator _validator;
        private List<ConstructionConstraint> _activeConstraints = new List<ConstructionConstraint>();
        
        // Integration systems
        private InteractiveFacilityConstructor _facilityConstructor;
        private EnvironmentalManager _environmentalManager;
        
        // Events
        public System.Action<PlacedModule> OnModulePlaced;
        public System.Action<PlacedModule> OnModuleRemoved;
        public System.Action<ConstructionMode> OnModeChanged;
        
        // Properties
        public ConstructionMode CurrentMode => _currentMode;
        public bool IsConstructing => _currentMode != ConstructionMode.None;
        public int PlacedModuleCount => _placedModules.Count;
        public ModuleDefinitionSO SelectedModule => _selectedModule;
        
        protected override void OnManagerInitialize()
        {
            InitializeConstructionSystem();
            LoadModuleDefinitions();
            SetupConstructionArea();
            ConnectToSystems();
            
            LogInfo("Modular Construction Manager initialized");
        }
        
        private void Update()
        {
            if (_currentMode != ConstructionMode.None)
            {
                UpdateConstructionPreview();
                HandleConstructionInput();
            }
        }
        
        #region Initialization
        
        private void InitializeConstructionSystem()
        {
            // Setup construction parent if not assigned
            if (constructionParent == null)
            {
                var parentObj = new GameObject("ConstructedModules");
                constructionParent = parentObj.transform;
                constructionParent.parent = transform;
            }
            
            // Initialize validator
            _validator = new ConstructionValidator(this);
            
            // Create default materials if not assigned
            CreateDefaultMaterials();
            
            // Setup construction constraints
            SetupDefaultConstraints();
        }
        
        private void LoadModuleDefinitions()
        {
            // Load all module definitions from libraries
            LoadLibraryModules(wallModules);
            LoadLibraryModules(floorModules);
            LoadLibraryModules(roofModules);
            LoadLibraryModules(equipmentModules);
            
            LogInfo($"Loaded {_moduleDefinitions.Count} module definitions");
        }
        
        private void LoadLibraryModules(ModuleLibrarySO library)
        {
            if (library == null) return;
            
            foreach (var module in library.Modules)
            {
                if (module != null && !_moduleDefinitions.ContainsKey(module.ModuleId))
                {
                    _moduleDefinitions[module.ModuleId] = module;
                }
            }
        }
        
        private void SetupConstructionArea()
        {
            // Create visual representation of construction bounds
            CreateConstructionBoundsVisualization();
        }
        
        private void CreateConstructionBoundsVisualization()
        {
            var boundsObj = new GameObject("ConstructionBounds");
            boundsObj.transform.parent = transform;
            boundsObj.transform.position = constructionBounds.center;
            
            // Add wireframe renderer for bounds
            var lineRenderer = boundsObj.AddComponent<LineRenderer>();
            lineRenderer.material = CreateBoundsMaterial();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.useWorldSpace = false;
            
            CreateBoundsWireframe(lineRenderer);
        }
        
        private void CreateDefaultMaterials()
        {
            if (previewMaterial == null)
            {
                previewMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                previewMaterial.color = new Color(1f, 1f, 1f, 0.5f);
                previewMaterial.SetFloat("_Surface", 1); // Transparent
            }
            
            if (validPlacementMaterial == null)
            {
                validPlacementMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                validPlacementMaterial.color = new Color(0f, 1f, 0f, 0.5f);
                validPlacementMaterial.SetFloat("_Surface", 1); // Transparent
            }
            
            if (invalidPlacementMaterial == null)
            {
                invalidPlacementMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                invalidPlacementMaterial.color = new Color(1f, 0f, 0f, 0.5f);
                invalidPlacementMaterial.SetFloat("_Surface", 1); // Transparent
            }
        }
        
        private Material CreateBoundsMaterial()
        {
            var material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            material.color = new Color(1f, 1f, 0f, 0.8f); // Yellow bounds
            return material;
        }
        
        private void CreateBoundsWireframe(LineRenderer lineRenderer)
        {
            var size = constructionBounds.size;
            var positions = new Vector3[]
            {
                // Bottom face
                new Vector3(-size.x/2, -size.y/2, -size.z/2),
                new Vector3(size.x/2, -size.y/2, -size.z/2),
                new Vector3(size.x/2, -size.y/2, size.z/2),
                new Vector3(-size.x/2, -size.y/2, size.z/2),
                new Vector3(-size.x/2, -size.y/2, -size.z/2), // Close bottom
                
                // Move to top
                new Vector3(-size.x/2, size.y/2, -size.z/2),
                
                // Top face
                new Vector3(size.x/2, size.y/2, -size.z/2),
                new Vector3(size.x/2, size.y/2, size.z/2),
                new Vector3(-size.x/2, size.y/2, size.z/2),
                new Vector3(-size.x/2, size.y/2, -size.z/2), // Close top
            };
            
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
        }
        
        private void SetupDefaultConstraints()
        {
            // Add basic construction constraints
            _activeConstraints.Add(new BoundsConstraint(constructionBounds));
            _activeConstraints.Add(new GridAlignmentConstraint(gridSize));
            _activeConstraints.Add(new OverlapConstraint());
            _activeConstraints.Add(new StructuralIntegrityConstraint());
        }
        
        private void ConnectToSystems()
        {
            if (GameManager.Instance != null)
            {
                _facilityConstructor = GameManager.Instance.GetManager<InteractiveFacilityConstructor>();
                _environmentalManager = GameManager.Instance.GetManager<EnvironmentalManager>();
            }
        }
        
        #endregion
        
        #region Construction Mode Management
        
        public void EnterConstructionMode(ConstructionMode mode, string moduleId = null)
        {
            _currentMode = mode;
            
            if (!string.IsNullOrEmpty(moduleId))
            {
                SelectModule(moduleId);
            }
            
            OnModeChanged?.Invoke(_currentMode);
            LogInfo($"Entered construction mode: {mode}");
        }
        
        public void ExitConstructionMode()
        {
            _currentMode = ConstructionMode.None;
            _selectedModule = null;
            
            // Cleanup preview
            if (_previewObject != null)
            {
                DestroyImmediate(_previewObject);
                _previewObject = null;
            }
            
            OnModeChanged?.Invoke(_currentMode);
            LogInfo("Exited construction mode");
        }
        
        public void SelectModule(string moduleId)
        {
            if (_moduleDefinitions.TryGetValue(moduleId, out var module))
            {
                _selectedModule = module;
                CreatePreviewObject();
                LogInfo($"Selected module: {module.ModuleName}");
            }
            else
            {
                LogError($"Module not found: {moduleId}");
            }
        }
        
        #endregion
        
        #region Preview System
        
        private void CreatePreviewObject()
        {
            // Destroy existing preview
            if (_previewObject != null)
            {
                DestroyImmediate(_previewObject);
            }
            
            if (_selectedModule?.ModulePrefab == null) return;
            
            // Create preview instance
            _previewObject = Instantiate(_selectedModule.ModulePrefab);
            _previewObject.name = $"Preview_{_selectedModule.ModuleName}";
            
            // Configure for preview
            SetupPreviewObject(_previewObject);
        }
        
        private void SetupPreviewObject(GameObject preview)
        {
            // Disable colliders
            var colliders = preview.GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                collider.enabled = false;
            }
            
            // Set preview materials
            var renderers = preview.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                var materials = new Material[renderer.materials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = previewMaterial;
                }
                renderer.materials = materials;
            }
            
            // Add preview component
            var previewComponent = preview.AddComponent<ModulePreview>();
            previewComponent.Initialize(this);
        }
        
        private void UpdateConstructionPreview()
        {
            if (_previewObject == null || _selectedModule == null) return;
            
            // Get mouse position in world space
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            if (mouseWorldPos == Vector3.zero) return;
            
            // Snap to grid
            Vector3Int gridPosition = WorldToGridPosition(mouseWorldPos);
            Vector3 snappedWorldPos = GridToWorldPosition(gridPosition);
            
            // Update preview position
            _previewObject.transform.position = snappedWorldPos;
            _previewPosition = gridPosition;
            
            // Validate placement
            _isValidPlacement = _validator.ValidatePlacement(_selectedModule, gridPosition);
            
            // Update preview appearance
            UpdatePreviewAppearance();
        }
        
        private void UpdatePreviewAppearance()
        {
            if (_previewObject == null) return;
            
            Material targetMaterial = _isValidPlacement ? validPlacementMaterial : invalidPlacementMaterial;
            
            var renderers = _previewObject.GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                var materials = new Material[renderer.materials.Length];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = targetMaterial;
                }
                renderer.materials = materials;
            }
        }
        
        private Vector3 GetMouseWorldPosition()
        {
            Camera camera = Camera.main;
            if (camera == null) return Vector3.zero;
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            
            // Raycast against construction plane (Y = 0 for floor level)
            Plane constructionPlane = new Plane(Vector3.up, Vector3.zero);
            
            if (constructionPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }
            
            return Vector3.zero;
        }
        
        #endregion
        
        #region Grid System
        
        public Vector3Int WorldToGridPosition(Vector3 worldPos)
        {
            return new Vector3Int(
                Mathf.RoundToInt(worldPos.x / gridSize),
                Mathf.RoundToInt(worldPos.y / gridSize),
                Mathf.RoundToInt(worldPos.z / gridSize)
            );
        }
        
        public Vector3 GridToWorldPosition(Vector3Int gridPos)
        {
            return new Vector3(
                gridPos.x * gridSize,
                gridPos.y * gridSize,
                gridPos.z * gridSize
            );
        }
        
        public bool IsGridPositionOccupied(Vector3Int gridPos)
        {
            return _placedModules.ContainsKey(gridPos);
        }
        
        public bool IsGridPositionInBounds(Vector3Int gridPos)
        {
            Vector3 worldPos = GridToWorldPosition(gridPos);
            return constructionBounds.Contains(worldPos);
        }
        
        #endregion
        
        #region Module Placement
        
        private void HandleConstructionInput()
        {
            // Handle placement input
            if (Input.GetMouseButtonDown(0)) // Left click to place
            {
                if (_currentMode == ConstructionMode.Place && _isValidPlacement)
                {
                    PlaceModule();
                }
                else if (_currentMode == ConstructionMode.Remove)
                {
                    RemoveModule();
                }
            }
            
            // Handle rotation input
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotatePreview();
            }
            
            // Handle mode switching
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitConstructionMode();
            }
        }
        
        private void PlaceModule()
        {
            if (_selectedModule == null || _previewObject == null) return;
            
            // Final validation
            if (!_validator.ValidatePlacement(_selectedModule, _previewPosition))
            {
                LogWarning("Invalid placement position");
                return;
            }
            
            // Create actual module instance
            GameObject moduleInstance = Instantiate(_selectedModule.ModulePrefab, constructionParent);
            moduleInstance.transform.position = GridToWorldPosition(_previewPosition);
            moduleInstance.transform.rotation = _previewObject.transform.rotation;
            moduleInstance.name = $"{_selectedModule.ModuleName}_{_previewPosition}";
            
            // Configure placed module
            var placedModule = SetupPlacedModule(moduleInstance);
            
            // Register placement
            _placedModules[_previewPosition] = placedModule;
            
            // Trigger events
            OnModulePlaced?.Invoke(placedModule);
            
            // Update connections
            UpdateModuleConnections(placedModule);
            
            LogInfo($"Placed module: {_selectedModule.ModuleName} at {_previewPosition}");
        }
        
        private PlacedModule SetupPlacedModule(GameObject moduleObject)
        {
            var placedModule = new PlacedModule
            {
                ModuleDefinition = _selectedModule,
                GameObject = moduleObject,
                GridPosition = _previewPosition,
                PlacementTime = System.DateTime.Now,
                Connections = new List<ModuleConnection>()
            };
            
            // Add placement component
            var placementComponent = moduleObject.AddComponent<PlacedModuleComponent>();
            placementComponent.Initialize(placedModule, this);
            
            // Setup module systems
            SetupModuleSystems(placedModule);
            
            return placedModule;
        }
        
        private void SetupModuleSystems(PlacedModule module)
        {
            // Add environmental zone if module defines one
            if (module.ModuleDefinition.CreatesEnvironmentalZone)
            {
                SetupEnvironmentalZone(module);
            }
            
            // Add equipment mounting points
            if (module.ModuleDefinition.HasMountingPoints)
            {
                SetupMountingPoints(module);
            }
            
            // Add utility connections
            if (module.ModuleDefinition.HasUtilityConnections)
            {
                SetupUtilityConnections(module);
            }
        }
        
        private void SetupEnvironmentalZone(PlacedModule module)
        {
            var zoneComponent = module.GameObject.AddComponent<EnvironmentalZone>();
            // Configure zone based on module definition
            // This would integrate with the environmental system
        }
        
        private void SetupMountingPoints(PlacedModule module)
        {
            var mountingPoints = module.ModuleDefinition.MountingPoints;
            foreach (var mountPoint in mountingPoints)
            {
                var mountObj = new GameObject($"MountPoint_{mountPoint.MountId}");
                mountObj.transform.parent = module.GameObject.transform;
                mountObj.transform.localPosition = mountPoint.LocalPosition;
                mountObj.transform.localRotation = mountPoint.LocalRotation;
                
                var mountComponent = mountObj.AddComponent<MountingPoint>();
                mountComponent.Configure(mountPoint);
            }
        }
        
        private void SetupUtilityConnections(PlacedModule module)
        {
            // Setup electrical, water, data connections
            var connections = module.ModuleDefinition.UtilityConnections;
            foreach (var connection in connections)
            {
                var connectionObj = new GameObject($"Utility_{connection.ConnectionType}");
                connectionObj.transform.parent = module.GameObject.transform;
                connectionObj.transform.localPosition = connection.LocalPosition;
                
                var utilityComponent = connectionObj.AddComponent<UtilityConnection>();
                utilityComponent.Configure(connection);
            }
        }
        
        private void UpdateModuleConnections(PlacedModule newModule)
        {
            // Check for connections to adjacent modules
            var adjacentPositions = GetAdjacentGridPositions(newModule.GridPosition);
            
            foreach (var adjPos in adjacentPositions)
            {
                if (_placedModules.TryGetValue(adjPos, out var adjacentModule))
                {
                    CreateModuleConnection(newModule, adjacentModule);
                }
            }
        }
        
        private List<Vector3Int> GetAdjacentGridPositions(Vector3Int position)
        {
            return new List<Vector3Int>
            {
                position + Vector3Int.right,
                position + Vector3Int.left,
                position + Vector3Int.forward,
                position + Vector3Int.back,
                position + Vector3Int.up,
                position + Vector3Int.down
            };
        }
        
        private void CreateModuleConnection(PlacedModule module1, PlacedModule module2)
        {
            // Create bidirectional connection
            var connection = new ModuleConnection
            {
                ConnectedModule = module2,
                ConnectionType = DetermineConnectionType(module1, module2),
                ConnectionStrength = 1.0f
            };
            
            module1.Connections.Add(connection);
            
            // Add reverse connection
            var reverseConnection = new ModuleConnection
            {
                ConnectedModule = module1,
                ConnectionType = connection.ConnectionType,
                ConnectionStrength = 1.0f
            };
            
            module2.Connections.Add(reverseConnection);
        }
        
        private ModuleConnectionType DetermineConnectionType(PlacedModule module1, PlacedModule module2)
        {
            // Determine connection type based on module types
            if (module1.ModuleDefinition.ModuleType == ModuleType.Wall && 
                module2.ModuleDefinition.ModuleType == ModuleType.Wall)
            {
                return ModuleConnectionType.Structural;
            }
            else if (module1.ModuleDefinition.ModuleType == ModuleType.Floor || 
                     module2.ModuleDefinition.ModuleType == ModuleType.Floor)
            {
                return ModuleConnectionType.Foundation;
            }
            
            return ModuleConnectionType.Adjacency;
        }
        
        private void RemoveModule()
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            if (mouseWorldPos == Vector3.zero) return;
            
            Vector3Int gridPosition = WorldToGridPosition(mouseWorldPos);
            
            if (_placedModules.TryGetValue(gridPosition, out var module))
            {
                RemoveModuleAtPosition(gridPosition);
            }
        }
        
        public void RemoveModuleAtPosition(Vector3Int gridPosition)
        {
            if (_placedModules.TryGetValue(gridPosition, out var module))
            {
                // Remove connections
                RemoveModuleConnections(module);
                
                // Destroy GameObject
                if (module.GameObject != null)
                {
                    DestroyImmediate(module.GameObject);
                }
                
                // Remove from registry
                _placedModules.Remove(gridPosition);
                
                // Trigger events
                OnModuleRemoved?.Invoke(module);
                
                LogInfo($"Removed module at {gridPosition}");
            }
        }
        
        private void RemoveModuleConnections(PlacedModule module)
        {
            // Remove all connections from this module
            foreach (var connection in module.Connections)
            {
                // Remove reverse connection from connected module
                var connectedModule = connection.ConnectedModule;
                connectedModule.Connections.RemoveAll(c => c.ConnectedModule == module);
            }
            
            module.Connections.Clear();
        }
        
        private void RotatePreview()
        {
            if (_previewObject != null)
            {
                _previewObject.transform.Rotate(0, 90, 0);
            }
        }
        
        #endregion
        
        #region Public Interface
        
        public List<ModuleDefinitionSO> GetAvailableModules(ModuleType type = ModuleType.All)
        {
            if (type == ModuleType.All)
            {
                return _moduleDefinitions.Values.ToList();
            }
            
            return _moduleDefinitions.Values.Where(m => m.ModuleType == type).ToList();
        }
        
        public PlacedModule GetModuleAtPosition(Vector3Int gridPosition)
        {
            return _placedModules.TryGetValue(gridPosition, out var module) ? module : null;
        }
        
        public List<PlacedModule> GetAllPlacedModules()
        {
            return _placedModules.Values.ToList();
        }
        
        public List<PlacedModule> GetModulesByType(ModuleType type)
        {
            return _placedModules.Values.Where(m => m.ModuleDefinition.ModuleType == type).ToList();
        }
        
        public ConstructionReport GetConstructionReport()
        {
            return new ConstructionReport
            {
                TotalModules = _placedModules.Count,
                ModulesByType = GetModuleCountsByType(),
                ConstructionBounds = constructionBounds,
                GridSize = gridSize,
                LastUpdate = System.DateTime.Now
            };
        }
        
        private Dictionary<ModuleType, int> GetModuleCountsByType()
        {
            var counts = new Dictionary<ModuleType, int>();
            
            foreach (var module in _placedModules.Values)
            {
                var type = module.ModuleDefinition.ModuleType;
                counts[type] = counts.GetValueOrDefault(type, 0) + 1;
            }
            
            return counts;
        }
        
        public void SaveConstruction(string constructionName)
        {
            var constructionData = new ConstructionSaveData
            {
                ConstructionName = constructionName,
                SaveDate = System.DateTime.Now,
                PlacedModules = _placedModules.Values.ToList(),
                ConstructionBounds = constructionBounds,
                GridSize = gridSize
            };
            
            // Save to file system or database
            SaveConstructionData(constructionData);
        }
        
        private void SaveConstructionData(ConstructionSaveData data)
        {
            // Implementation would save to persistent storage
            LogInfo($"Construction saved: {data.ConstructionName}");
        }
        
        #endregion
        
        protected override void OnManagerShutdown()
        {
            // Cleanup preview
            if (_previewObject != null)
            {
                DestroyImmediate(_previewObject);
            }
            
            // Clear all placed modules
            foreach (var module in _placedModules.Values)
            {
                if (module.GameObject != null)
                {
                    DestroyImmediate(module.GameObject);
                }
            }
            
            _placedModules.Clear();
            
            LogInfo("Modular Construction Manager shutdown complete");
        }
    }
    
    /// <summary>
    /// Construction validation system
    /// </summary>
    public class ConstructionValidator
    {
        private ModularConstructionManager _manager;
        
        public ConstructionValidator(ModularConstructionManager manager)
        {
            _manager = manager;
        }
        
        public bool ValidatePlacement(ModuleDefinitionSO module, Vector3Int gridPosition)
        {
            // Check bounds
            if (!_manager.IsGridPositionInBounds(gridPosition))
                return false;
            
            // Check overlap
            if (_manager.IsGridPositionOccupied(gridPosition))
                return false;
            
            // Check module-specific constraints
            if (!ValidateModuleConstraints(module, gridPosition))
                return false;
            
            // Check structural integrity
            if (!ValidateStructuralIntegrity(module, gridPosition))
                return false;
            
            return true;
        }
        
        private bool ValidateModuleConstraints(ModuleDefinitionSO module, Vector3Int gridPosition)
        {
            // Validate based on module type
            switch (module.ModuleType)
            {
                case ModuleType.Foundation:
                    return gridPosition.y == 0; // Foundations must be at ground level
                    
                case ModuleType.Wall:
                    return HasFoundationSupport(gridPosition);
                    
                case ModuleType.Roof:
                    return HasWallSupport(gridPosition);
                    
                default:
                    return true;
            }
        }
        
        private bool HasFoundationSupport(Vector3Int position)
        {
            var belowPosition = position + Vector3Int.down;
            var moduleBelow = _manager.GetModuleAtPosition(belowPosition);
            
            return moduleBelow?.ModuleDefinition.ModuleType == ModuleType.Foundation ||
                   moduleBelow?.ModuleDefinition.ModuleType == ModuleType.Floor;
        }
        
        private bool HasWallSupport(Vector3Int position)
        {
            var belowPosition = position + Vector3Int.down;
            var moduleBelow = _manager.GetModuleAtPosition(belowPosition);
            
            return moduleBelow?.ModuleDefinition.ModuleType == ModuleType.Wall;
        }
        
        private bool ValidateStructuralIntegrity(ModuleDefinitionSO module, Vector3Int gridPosition)
        {
            // Check if module will have adequate support
            if (module.RequiresStructuralSupport)
            {
                return HasAdequateSupport(gridPosition);
            }
            
            return true;
        }
        
        private bool HasAdequateSupport(Vector3Int position)
        {
            // Check adjacent positions for support structures
            var adjacentPositions = new Vector3Int[]
            {
                position + Vector3Int.right,
                position + Vector3Int.left,
                position + Vector3Int.forward,
                position + Vector3Int.back,
                position + Vector3Int.down
            };
            
            int supportCount = 0;
            
            foreach (var adjPos in adjacentPositions)
            {
                var module = _manager.GetModuleAtPosition(adjPos);
                if (module?.ModuleDefinition.ProvidesStructuralSupport == true)
                {
                    supportCount++;
                }
            }
            
            return supportCount >= 1; // At least one supporting structure
        }
    }
}
```

**Step 2: Construction Data Structures**

Create file: `Assets/ProjectChimera/Data/Construction/ConstructionDataStructures.cs`

```csharp
using UnityEngine;
using System;
using System.Collections.Generic;

namespace ProjectChimera.Data.Construction
{
    /// <summary>
    /// Construction modes for the modular building system
    /// </summary>
    public enum ConstructionMode
    {
        None,
        Place,
        Remove,
        Edit,
        Inspect
    }
    
    /// <summary>
    /// Types of building modules
    /// </summary>
    public enum ModuleType
    {
        All,
        Foundation,
        Floor,
        Wall,
        Roof,
        Door,
        Window,
        Equipment,
        Decoration
    }
    
    /// <summary>
    /// Types of connections between modules
    /// </summary>
    public enum ModuleConnectionType
    {
        Structural,
        Foundation,
        Adjacency,
        Utility,
        Data
    }
    
    /// <summary>
    /// Defines a placeable building module
    /// </summary>
    [CreateAssetMenu(fileName = "New Module Definition", menuName = "Project Chimera/Construction/Module Definition")]
    public class ModuleDefinitionSO : ScriptableObject
    {
        [Header("Basic Information")]
        public string ModuleId;
        public string ModuleName;
        public string Description;
        public ModuleType ModuleType;
        public GameObject ModulePrefab;
        public Sprite ModuleIcon;
        
        [Header("Construction Properties")]
        public Vector3Int GridSize = Vector3Int.one;
        public bool RequiresStructuralSupport = true;
        public bool ProvidesStructuralSupport = false;
        public float ConstructionTime = 1.0f;
        public float ConstructionCost = 100f;
        
        [Header("Functionality")]
        public bool CreatesEnvironmentalZone = false;
        public bool HasMountingPoints = false;
        public bool HasUtilityConnections = false;
        public bool IsInteractable = false;
        
        [Header("Module Features")]
        public List<MountingPointDefinition> MountingPoints = new List<MountingPointDefinition>();
        public List<UtilityConnectionDefinition> UtilityConnections = new List<UtilityConnectionDefinition>();
        public List<string> RequiredResources = new List<string>();
        
        [Header("Validation")]
        public List<ConstructionConstraint> PlacementConstraints = new List<ConstructionConstraint>();
        
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(ModuleId))
            {
                ModuleId = name.Replace(" ", "_").ToLower();
            }
        }
    }
    
    /// <summary>
    /// Definition for equipment mounting points
    /// </summary>
    [System.Serializable]
    public class MountingPointDefinition
    {
        public string MountId;
        public MountingPointType MountType;
        public Vector3 LocalPosition;
        public Quaternion LocalRotation = Quaternion.identity;
        public Vector3 MaxEquipmentSize = Vector3.one;
        public float MaxWeight = 100f;
        public List<string> CompatibleEquipmentTypes = new List<string>();
    }
    
    public enum MountingPointType
    {
        Ceiling,
        Wall,
        Floor,
        Universal
    }
    
    /// <summary>
    /// Definition for utility connections (power, water, data)
    /// </summary>
    [System.Serializable]
    public class UtilityConnectionDefinition
    {
        public string ConnectionId;
        public UtilityType ConnectionType;
        public Vector3 LocalPosition;
        public ConnectionDirection Direction;
        public float Capacity = 1.0f;
        public bool IsInput = true;
        public bool IsOutput = false;
    }
    
    public enum UtilityType
    {
        Electrical,
        Water,
        Data,
        Gas,
        Ventilation
    }
    
    public enum ConnectionDirection
    {
        Up,
        Down,
        North,
        South,
        East,
        West
    }
    
    /// <summary>
    /// Library of module definitions
    /// </summary>
    [CreateAssetMenu(fileName = "Module Library", menuName = "Project Chimera/Construction/Module Library")]
    public class ModuleLibrarySO : ScriptableObject
    {
        [Header("Library Information")]
        public string LibraryName;
        public string LibraryDescription;
        public string Version = "1.0";
        
        [Header("Modules")]
        public List<ModuleDefinitionSO> Modules = new List<ModuleDefinitionSO>();
        
        [Header("Categories")]
        public List<ModuleCategory> Categories = new List<ModuleCategory>();
        
        public List<ModuleDefinitionSO> GetModulesByType(ModuleType type)
        {
            return Modules.FindAll(m => m.ModuleType == type);
        }
        
        public ModuleDefinitionSO GetModuleById(string moduleId)
        {
            return Modules.Find(m => m.ModuleId == moduleId);
        }
    }
    
    [System.Serializable]
    public class ModuleCategory
    {
        public string CategoryName;
        public string CategoryDescription;
        public List<string> ModuleIds = new List<string>();
        public Color CategoryColor = Color.white;
    }
    
    /// <summary>
    /// Runtime data for a placed module
    /// </summary>
    [System.Serializable]
    public class PlacedModule
    {
        public ModuleDefinitionSO ModuleDefinition;
        public GameObject GameObject;
        public Vector3Int GridPosition;
        public Quaternion Rotation = Quaternion.identity;
        public DateTime PlacementTime;
        public List<ModuleConnection> Connections = new List<ModuleConnection>();
        public Dictionary<string, object> ModuleData = new Dictionary<string, object>();
        public bool IsActive = true;
        public float Health = 100f;
    }
    
    /// <summary>
    /// Connection between two modules
    /// </summary>
    [System.Serializable]
    public class ModuleConnection
    {
        public PlacedModule ConnectedModule;
        public ModuleConnectionType ConnectionType;
        public float ConnectionStrength = 1.0f;
        public bool IsActive = true;
        public Vector3 ConnectionPoint;
    }
    
    /// <summary>
    /// Construction constraint for validation
    /// </summary>
    [System.Serializable]
    public abstract class ConstructionConstraint
    {
        public string ConstraintName;
        public string Description;
        public bool IsEnabled = true;
        
        public abstract bool ValidateConstraint(ModuleDefinitionSO module, Vector3Int position, ModularConstructionManager manager);
    }
    
    /// <summary>
    /// Bounds constraint - modules must be within construction area
    /// </summary>
    [System.Serializable]
    public class BoundsConstraint : ConstructionConstraint
    {
        public Bounds AllowedBounds;
        
        public BoundsConstraint(Bounds bounds)
        {
            AllowedBounds = bounds;
            ConstraintName = "Bounds Constraint";
            Description = "Module must be within construction boundaries";
        }
        
        public override bool ValidateConstraint(ModuleDefinitionSO module, Vector3Int position, ModularConstructionManager manager)
        {
            Vector3 worldPos = manager.GridToWorldPosition(position);
            return AllowedBounds.Contains(worldPos);
        }
    }
    
    /// <summary>
    /// Grid alignment constraint
    /// </summary>
    [System.Serializable]
    public class GridAlignmentConstraint : ConstructionConstraint
    {
        public float GridSize;
        
        public GridAlignmentConstraint(float gridSize)
        {
            GridSize = gridSize;
            ConstraintName = "Grid Alignment";
            Description = "Module must align to construction grid";
        }
        
        public override bool ValidateConstraint(ModuleDefinitionSO module, Vector3Int position, ModularConstructionManager manager)
        {
            // Position is already grid-aligned by the grid system
            return true;
        }
    }
    
    /// <summary>
    /// Overlap constraint - modules cannot occupy same space
    /// </summary>
    [System.Serializable]
    public class OverlapConstraint : ConstructionConstraint
    {
        public OverlapConstraint()
        {
            ConstraintName = "Overlap Prevention";
            Description = "Modules cannot overlap with existing modules";
        }
        
        public override bool ValidateConstraint(ModuleDefinitionSO module, Vector3Int position, ModularConstructionManager manager)
        {
            return !manager.IsGridPositionOccupied(position);
        }
    }
    
    /// <summary>
    /// Structural integrity constraint
    /// </summary>
    [System.Serializable]
    public class StructuralIntegrityConstraint : ConstructionConstraint
    {
        public StructuralIntegrityConstraint()
        {
            ConstraintName = "Structural Integrity";
            Description = "Modules must have adequate structural support";
        }
        
        public override bool ValidateConstraint(ModuleDefinitionSO module, Vector3Int position, ModularConstructionManager manager)
        {
            if (!module.RequiresStructuralSupport)
                return true;
            
            // Check for support below or adjacent
            var supportPositions = new Vector3Int[]
            {
                position + Vector3Int.down,
                position + Vector3Int.right,
                position + Vector3Int.left,
                position + Vector3Int.forward,
                position + Vector3Int.back
            };
            
            foreach (var supportPos in supportPositions)
            {
                var supportModule = manager.GetModuleAtPosition(supportPos);
                if (supportModule?.ModuleDefinition.ProvidesStructuralSupport == true)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
    
    /// <summary>
    /// Construction save data
    /// </summary>
    [System.Serializable]
    public class ConstructionSaveData
    {
        public string ConstructionName;
        public DateTime SaveDate;
        public List<PlacedModule> PlacedModules;
        public Bounds ConstructionBounds;
        public float GridSize;
        public Dictionary<string, object> CustomData = new Dictionary<string, object>();
    }
    
    /// <summary>
    /// Construction system report
    /// </summary>
    [System.Serializable]
    public class ConstructionReport
    {
        public int TotalModules;
        public Dictionary<ModuleType, int> ModulesByType;
        public Bounds ConstructionBounds;
        public float GridSize;
        public DateTime LastUpdate;
    }
}
```

#### 5. Advanced Camera System Implementation

**Step 1: Cinemachine Integration Setup**

**Create Advanced Camera Controller:**

Create file: `Assets/ProjectChimera/Scripts/Camera/AdvancedCameraController.cs`

```csharp
using UnityEngine;
using Cinemachine;
using ProjectChimera.Core;
using ProjectChimera.Data.Camera;
using System.Collections.Generic;
using System.Collections;

namespace ProjectChimera.Systems.Camera
{
    /// <summary>
    /// Advanced camera system with multiple view modes and smooth transitions
    /// </summary>
    public class AdvancedCameraController : ChimeraManager
    {
        [Header("Camera Configuration")]
        [SerializeField] private CameraConfigSO cameraConfig;
        [SerializeField] private Transform cameraRig;
        [SerializeField] private LayerMask interactionLayers = -1;
        
        [Header("Virtual Cameras")]
        [SerializeField] private CinemachineVirtualCamera orbitCamera;
        [SerializeField] private CinemachineVirtualCamera flyCamera;
        [SerializeField] private CinemachineVirtualCamera overheadCamera;
        [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
        [SerializeField] private CinemachineVirtualCamera focusCamera;
        
        [Header("Camera Targets")]
        [SerializeField] private Transform orbitTarget;
        [SerializeField] private Transform flyTarget;
        [SerializeField] private Transform focusTarget;
        
        [Header("Input Settings")]
        [SerializeField] private bool enableMouseOrbit = true;
        [SerializeField] private bool enableKeyboardMovement = true;
        [SerializeField] private bool enableScrollZoom = true;
        [SerializeField] private float mouseSensitivity = 2.0f;
        [SerializeField] private float keyboardSpeed = 10.0f;
        [SerializeField] private float zoomSpeed = 5.0f;
        
        // Camera state
        private CameraMode _currentMode = CameraMode.Orbit;
        private CameraMode _previousMode = CameraMode.Orbit;
        private bool _isTransitioning = false;
        private Transform _currentTarget;
        private CameraState _savedState;
        
        // Input handling
        private Vector2 _mouseInput;
        private Vector3 _keyboardInput;
        private float _scrollInput;
        private bool _isInputEnabled = true;
        
        // Cinemachine components
        private CinemachineBrain _brain;
        private Dictionary<CameraMode, CinemachineVirtualCamera> _virtualCameras;
        
        // Tour system
        private List<Transform> _tourWaypoints = new List<Transform>();
        private Coroutine _tourCoroutine;
        private bool _isOnTour = false;
        
        // Events
        public System.Action<CameraMode> OnCameraModeChanged;
        public System.Action<Transform> OnTargetChanged;
        public System.Action OnTourStarted;
        public System.Action OnTourCompleted;
        
        // Properties
        public CameraMode CurrentMode => _currentMode;
        public bool IsTransitioning => _isTransitioning;
        public Transform CurrentTarget => _currentTarget;
        public CameraControlSettings Settings { get; set; }
        
        protected override void OnManagerInitialize()
        {
            InitializeCameraSystem();
            SetupVirtualCameras();
            ConfigureInputHandling();
            SetInitialCameraMode();
            
            LogInfo("Advanced Camera Controller initialized");
        }
        
        private void Update()
        {
            if (_isInputEnabled && !_isTransitioning)
            {
                HandleCameraInput();
                UpdateCameraMovement();
            }
            
            UpdateCameraTargets();
        }
        
        #region Initialization
        
        private void InitializeCameraSystem()
        {
            // Find or create camera brain
            _brain = FindObjectOfType<CinemachineBrain>();
            if (_brain == null)
            {
                var mainCamera = UnityEngine.Camera.main;
                if (mainCamera != null)
                {
                    _brain = mainCamera.gameObject.AddComponent<CinemachineBrain>();
                }
            }
            
            // Setup camera rig if not assigned
            if (cameraRig == null)
            {
                CreateCameraRig();
            }
            
            // Load camera configuration
            if (cameraConfig != null)
            {
                LoadCameraConfiguration();
            }
            
            // Initialize virtual camera dictionary
            _virtualCameras = new Dictionary<CameraMode, CinemachineVirtualCamera>();
        }
        
        private void CreateCameraRig()
        {
            var rigObject = new GameObject("CameraRig");
            rigObject.transform.parent = transform;
            cameraRig = rigObject.transform;
            
            // Create camera targets
            CreateCameraTargets();
        }
        
        private void CreateCameraTargets()
        {
            // Orbit target
            if (orbitTarget == null)
            {
                var orbitObj = new GameObject("OrbitTarget");
                orbitObj.transform.parent = cameraRig;
                orbitTarget = orbitObj.transform;
            }
            
            // Fly target
            if (flyTarget == null)
            {
                var flyObj = new GameObject("FlyTarget");
                flyObj.transform.parent = cameraRig;
                flyTarget = flyObj.transform;
            }
            
            // Focus target
            if (focusTarget == null)
            {
                var focusObj = new GameObject("FocusTarget");
                focusObj.transform.parent = cameraRig;
                focusTarget = focusObj.transform;
            }
        }
        
        private void LoadCameraConfiguration()
        {
            Settings = new CameraControlSettings
            {
                MouseSensitivity = cameraConfig.MouseSensitivity,
                KeyboardSpeed = cameraConfig.KeyboardSpeed,
                ZoomSpeed = cameraConfig.ZoomSpeed,
                OrbitDistance = cameraConfig.OrbitDistance,
                OrbitHeight = cameraConfig.OrbitHeight,
                FlySpeed = cameraConfig.FlySpeed,
                TransitionDuration = cameraConfig.TransitionDuration
            };
        }
        
        private void SetupVirtualCameras()
        {
            SetupOrbitCamera();
            SetupFlyCamera();
            SetupOverheadCamera();
            SetupFirstPersonCamera();
            SetupFocusCamera();
            
            // Register cameras
            _virtualCameras[CameraMode.Orbit] = orbitCamera;
            _virtualCameras[CameraMode.Fly] = flyCamera;
            _virtualCameras[CameraMode.Overhead] = overheadCamera;
            _virtualCameras[CameraMode.FirstPerson] = firstPersonCamera;
            _virtualCameras[CameraMode.Focus] = focusCamera;
        }
        
        private void SetupOrbitCamera()
        {
            if (orbitCamera == null)
            {
                var orbitObj = new GameObject("OrbitCamera");
                orbitObj.transform.parent = cameraRig;
                orbitCamera = orbitObj.AddComponent<CinemachineVirtualCamera>();
            }
            
            // Configure orbit camera
            orbitCamera.Priority = 10;
            orbitCamera.Follow = orbitTarget;
            orbitCamera.LookAt = orbitTarget;
            
            // Add orbit composer
            var composer = orbitCamera.AddCinemachineComponent<CinemachineComposer>();
            composer.m_TrackedObjectOffset = Vector3.up * 2f;
            composer.m_ScreenX = 0.5f;
            composer.m_ScreenY = 0.5f;
            
            // Add orbital transposer
            var transposer = orbitCamera.AddCinemachineComponent<CinemachineOrbitalTransposer>();
            transposer.m_OrbitStyle = CinemachineOrbitalTransposer.OrbitStyles.ThreeRing;
            transposer.m_Radius = Settings?.OrbitDistance ?? 10f;
            transposer.m_HeightOffset = Settings?.OrbitHeight ?? 5f;
        }
        
        private void SetupFlyCamera()
        {
            if (flyCamera == null)
            {
                var flyObj = new GameObject("FlyCamera");
                flyObj.transform.parent = cameraRig;
                flyCamera = flyObj.AddComponent<CinemachineVirtualCamera>();
            }
            
            // Configure fly camera
            flyCamera.Priority = 5;
            flyCamera.Follow = flyTarget;
            
            // Add transposer for smooth following
            var transposer = flyCamera.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset = Vector3.back * 5f + Vector3.up * 2f;
            transposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        }
        
        private void SetupOverheadCamera()
        {
            if (overheadCamera == null)
            {
                var overheadObj = new GameObject("OverheadCamera");
                overheadObj.transform.parent = cameraRig;
                overheadCamera = overheadObj.AddComponent<CinemachineVirtualCamera>();
            }
            
            // Configure overhead camera
            overheadCamera.Priority = 5;
            overheadCamera.transform.position = Vector3.up * 50f;
            overheadCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            
            // Add composer for orthographic-style viewing
            var composer = overheadCamera.AddCinemachineComponent<CinemachineComposer>();
            composer.m_ScreenX = 0.5f;
            composer.m_ScreenY = 0.5f;
        }
        
        private void SetupFirstPersonCamera()
        {
            if (firstPersonCamera == null)
            {
                var fpObj = new GameObject("FirstPersonCamera");
                fpObj.transform.parent = cameraRig;
                firstPersonCamera = fpObj.AddComponent<CinemachineVirtualCamera>();
            }
            
            // Configure first person camera
            firstPersonCamera.Priority = 5;
            
            // Add POV component for first person look
            var pov = firstPersonCamera.AddCinemachineComponent<CinemachinePOV>();
            pov.m_HorizontalAxis.m_MaxSpeed = Settings?.MouseSensitivity ?? 300f;
            pov.m_VerticalAxis.m_MaxSpeed = Settings?.MouseSensitivity ?? 2f;
            pov.m_VerticalAxis.m_MinValue = -80f;
            pov.m_VerticalAxis.m_MaxValue = 80f;
        }
        
        private void SetupFocusCamera()
        {
            if (focusCamera == null)
            {
                var focusObj = new GameObject("FocusCamera");
                focusObj.transform.parent = cameraRig;
                focusCamera = focusObj.AddComponent<CinemachineVirtualCamera>();
            }
            
            // Configure focus camera
            focusCamera.Priority = 5;
            focusCamera.Follow = focusTarget;
            focusCamera.LookAt = focusTarget;
            
            // Add composer for focus framing
            var composer = focusCamera.AddCinemachineComponent<CinemachineComposer>();
            composer.m_TrackedObjectOffset = Vector3.up;
            composer.m_ScreenX = 0.5f;
            composer.m_ScreenY = 0.4f; // Slightly lower to show object better
            
            // Add transposer for smooth positioning
            var transposer = focusCamera.AddCinemachineComponent<CinemachineTransposer>();
            transposer.m_FollowOffset = Vector3.back * 3f + Vector3.up * 1f;
        }
        
        private void ConfigureInputHandling()
        {
            // Configure Cinemachine input handling
            var inputProvider = FindObjectOfType<CinemachineInputProvider>();
            if (inputProvider == null && _brain != null)
            {
                inputProvider = _brain.gameObject.AddComponent<CinemachineInputProvider>();
            }
            
            if (inputProvider != null)
            {
                inputProvider.XYAxis = new InputAxis
                {
                    inputAxisName = "",
                    inputAxisValue = 0f,
                    gain = Settings?.MouseSensitivity ?? 2f
                };
                
                inputProvider.ZAxis = new InputAxis
                {
                    inputAxisName = "",
                    inputAxisValue = 0f,
                    gain = Settings?.ZoomSpeed ?? 1f
                };
            }
        }
        
        private void SetInitialCameraMode()
        {
            SetCameraMode(CameraMode.Orbit);
            _currentTarget = orbitTarget;
        }
        
        #endregion
        
        #region Camera Mode Control
        
        public void SetCameraMode(CameraMode mode)
        {
            if (_currentMode == mode || _isTransitioning) return;
            
            _previousMode = _currentMode;
            _currentMode = mode;
            
            StartCoroutine(TransitionToMode(mode));
        }
        
        private IEnumerator TransitionToMode(CameraMode mode)
        {
            _isTransitioning = true;
            
            // Deactivate current camera
            if (_virtualCameras.TryGetValue(_previousMode, out var previousCamera))
            {
                previousCamera.Priority = 5;
            }
            
            // Wait for transition
            yield return new WaitForSeconds(0.1f);
            
            // Activate new camera
            if (_virtualCameras.TryGetValue(mode, out var newCamera))
            {
                newCamera.Priority = 10;
                ConfigureCameraForMode(mode);
            }
            
            // Wait for Cinemachine transition
            float transitionDuration = Settings?.TransitionDuration ?? 1f;
            yield return new WaitForSeconds(transitionDuration);
            
            _isTransitioning = false;
            OnCameraModeChanged?.Invoke(mode);
            
            LogInfo($"Camera mode changed to: {mode}");
        }
        
        private void ConfigureCameraForMode(CameraMode mode)
        {
            switch (mode)
            {
                case CameraMode.Orbit:
                    _currentTarget = orbitTarget;
                    break;
                case CameraMode.Fly:
                    _currentTarget = flyTarget;
                    break;
                case CameraMode.Focus:
                    _currentTarget = focusTarget;
                    break;
                case CameraMode.Overhead:
                    _currentTarget = null;
                    break;
                case CameraMode.FirstPerson:
                    _currentTarget = null;
                    break;
            }
        }
        
        public void TransitionToMode(CameraMode mode, float duration)
        {
            if (Settings != null)
            {
                Settings.TransitionDuration = duration;
            }
            SetCameraMode(mode);
        }
        
        #endregion
        
        #region Input Handling
        
        private void HandleCameraInput()
        {
            // Mouse input
            if (enableMouseOrbit)
            {
                _mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
            
            // Keyboard input
            if (enableKeyboardMovement)
            {
                _keyboardInput = new Vector3(
                    Input.GetAxis("Horizontal"),
                    Input.GetKey(KeyCode.Q) ? -1f : Input.GetKey(KeyCode.E) ? 1f : 0f,
                    Input.GetAxis("Vertical")
                );
            }
            
            // Scroll input
            if (enableScrollZoom)
            {
                _scrollInput = Input.GetAxis("Mouse ScrollWheel");
            }
            
            // Mode switching shortcuts
            HandleModeShortcuts();
        }
        
        private void HandleModeShortcuts()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                SetCameraMode(CameraMode.Orbit);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                SetCameraMode(CameraMode.Fly);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                SetCameraMode(CameraMode.Overhead);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                SetCameraMode(CameraMode.FirstPerson);
        }
        
        private void UpdateCameraMovement()
        {
            switch (_currentMode)
            {
                case CameraMode.Orbit:
                    UpdateOrbitMovement();
                    break;
                case CameraMode.Fly:
                    UpdateFlyMovement();
                    break;
                case CameraMode.Overhead:
                    UpdateOverheadMovement();
                    break;
            }
        }
        
        private void UpdateOrbitMovement()
        {
            if (_mouseInput.magnitude > 0.01f && Input.GetMouseButton(1))
            {
                // Orbit around target
                var orbital = orbitCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
                if (orbital != null)
                {
                    orbital.m_XAxis.Value += _mouseInput.x * mouseSensitivity;
                    orbital.m_YAxis.Value += _mouseInput.y * mouseSensitivity * 0.5f;
                }
            }
            
            // Zoom with scroll
            if (Mathf.Abs(_scrollInput) > 0.01f)
            {
                var orbital = orbitCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
                if (orbital != null)
                {
                    orbital.m_Radius = Mathf.Clamp(
                        orbital.m_Radius - _scrollInput * zoomSpeed,
                        1f, 50f
                    );
                }
            }
            
            // Pan with middle mouse
            if (_mouseInput.magnitude > 0.01f && Input.GetMouseButton(2))
            {
                Vector3 panMovement = orbitTarget.right * -_mouseInput.x * keyboardSpeed * Time.deltaTime;
                panMovement += orbitTarget.forward * -_mouseInput.y * keyboardSpeed * Time.deltaTime;
                orbitTarget.position += panMovement;
            }
        }
        
        private void UpdateFlyMovement()
        {
            if (_keyboardInput.magnitude > 0.01f)
            {
                Vector3 movement = flyTarget.right * _keyboardInput.x * keyboardSpeed * Time.deltaTime;
                movement += flyTarget.up * _keyboardInput.y * keyboardSpeed * Time.deltaTime;
                movement += flyTarget.forward * _keyboardInput.z * keyboardSpeed * Time.deltaTime;
                
                flyTarget.position += movement;
            }
            
            // Mouse look
            if (_mouseInput.magnitude > 0.01f && Input.GetMouseButton(1))
            {
                flyTarget.Rotate(Vector3.up, _mouseInput.x * mouseSensitivity, Space.World);
                flyTarget.Rotate(Vector3.right, -_mouseInput.y * mouseSensitivity, Space.Self);
            }
        }
        
        private void UpdateOverheadMovement()
        {
            if (_keyboardInput.magnitude > 0.01f)
            {
                Vector3 movement = Vector3.right * _keyboardInput.x * keyboardSpeed * Time.deltaTime;
                movement += Vector3.forward * _keyboardInput.z * keyboardSpeed * Time.deltaTime;
                
                overheadCamera.transform.position += movement;
            }
            
            // Zoom with scroll
            if (Mathf.Abs(_scrollInput) > 0.01f)
            {
                Vector3 position = overheadCamera.transform.position;
                position.y = Mathf.Clamp(position.y - _scrollInput * zoomSpeed, 10f, 100f);
                overheadCamera.transform.position = position;
            }
        }
        
        private void UpdateCameraTargets()
        {
            // Keep targets within reasonable bounds
            if (_currentTarget != null)
            {
                Vector3 pos = _currentTarget.position;
                pos.x = Mathf.Clamp(pos.x, -100f, 100f);
                pos.z = Mathf.Clamp(pos.z, -100f, 100f);
                pos.y = Mathf.Clamp(pos.y, 0f, 50f);
                _currentTarget.position = pos;
            }
        }
        
        #endregion
        
        #region Target Management
        
        public void SetTarget(Transform target)
        {
            if (target == null) return;
            
            _currentTarget = target;
            
            // Update appropriate camera target based on mode
            switch (_currentMode)
            {
                case CameraMode.Orbit:
                    orbitTarget.position = target.position;
                    break;
                case CameraMode.Fly:
                    flyTarget.position = target.position;
                    break;
                case CameraMode.Focus:
                    focusTarget.position = target.position;
                    break;
            }
            
            OnTargetChanged?.Invoke(target);
        }
        
        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            if (_currentTarget != null)
            {
                _currentTarget.position = position;
                _currentTarget.rotation = rotation;
            }
        }
        
        public void FocusOnObject(GameObject target, float duration = 1f)
        {
            if (target == null) return;
            
            StartCoroutine(FocusOnObjectCoroutine(target, duration));
        }
        
        private IEnumerator FocusOnObjectCoroutine(GameObject target, float duration)
        {
            // Switch to focus mode
            CameraMode originalMode = _currentMode;
            SetCameraMode(CameraMode.Focus);
            
            // Set focus target
            focusTarget.position = target.transform.position;
            
            // Wait for focus duration
            yield return new WaitForSeconds(duration);
            
            // Return to original mode
            SetCameraMode(originalMode);
        }
        
        #endregion
        
        This comprehensive Scene Construction section continues with the modular construction system and advanced camera controls. The next part will cover interactive elements and performance optimization.

Would you like me to continue with the remaining parts of Scene Construction?