using UnityEngine;
using ProjectChimera.Core;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

#if UNITY_SPEEDTREE
using SpeedTree;
#endif

namespace ProjectChimera.Systems.SpeedTree
{
    /// <summary>
    /// Comprehensive SpeedTree optimization and performance system for Project Chimera.
    /// Provides advanced LOD management, batching, culling, memory optimization,
    /// and performance monitoring for large-scale cannabis cultivation with hundreds of plants.
    /// </summary>
    public class SpeedTreeOptimizationSystem : ChimeraManager
    {
        [Header("Performance Configuration")]
        [SerializeField] private SpeedTreePerformanceConfigSO _performanceConfig;
        [SerializeField] private LODOptimizationConfigSO _lodConfig;
        [SerializeField] private BatchingConfigSO _batchingConfig;
        [SerializeField] private CullingConfigSO _cullingConfig;
        
        [Header("Performance Targets")]
        [SerializeField] private int _targetFrameRate = 60;
        [SerializeField] private int _maxVisiblePlants = 500;
        [SerializeField] private float _maxMemoryUsageMB = 512f;
        [SerializeField] private int _maxDrawCalls = 100;
        
        [Header("Optimization Settings")]
        [SerializeField] private bool _enableDynamicLOD = true;
        [SerializeField] private bool _enableOcclusionCulling = true;
        [SerializeField] private bool _enableFrustumCulling = true;
        [SerializeField] private bool _enableDistanceCulling = true;
        [SerializeField] private bool _enableBatching = true;
        [SerializeField] private bool _enableGPUInstancing = true;
        
        [Header("Quality Scaling")]
        [SerializeField] private bool _enableDynamicQuality = true;
        [SerializeField] private bool _enableAdaptiveUpdates = true;
        [SerializeField] private bool _enablePerformanceThrottling = true;
        [SerializeField] private float _qualityScaleThreshold = 30f; // FPS
        
        [Header("Memory Management")]
        [SerializeField] private bool _enableMemoryPooling = true;
        [SerializeField] private bool _enableAssetStreaming = true;
        [SerializeField] private bool _enableTextureCompression = true;
        [SerializeField] private int _memoryPoolSize = 1000;
        
        // Core Optimization Systems
        private AdvancedLODManager _lodManager;
        private DynamicBatchingSystem _batchingSystem;
        private SmartCullingManager _cullingManager;
        private MemoryOptimizationManager _memoryManager;
        private PerformanceMonitoringSystem _performanceMonitor;
        
        // Specialized Optimization Components
        private PlantInstancePool _instancePool;
        private TextureStreamingManager _textureManager;
        private ShaderOptimizationManager _shaderManager;
        private RenderQueueOptimizer _renderQueueOptimizer;
        
        // Performance Tracking
        private Dictionary<int, PlantPerformanceData> _plantPerformanceData = new Dictionary<int, PlantPerformanceData>();
        private PerformanceMetrics _currentMetrics = new PerformanceMetrics();
        private List<PerformanceSnapshot> _performanceHistory = new List<PerformanceSnapshot>();
        
        // Optimization State
        private Dictionary<int, OptimizationState> _plantOptimizationStates = new Dictionary<int, OptimizationState>();
        private Dictionary<Camera, CameraOptimizationData> _cameraOptimization = new Dictionary<Camera, CameraOptimizationData>();
        
        // System Integration
        private AdvancedSpeedTreeManager _speedTreeManager;
        private SpeedTreeEnvironmentalSystem _environmentalSystem;
        private SpeedTreeGrowthSystem _growthSystem;
        
        // Update Management
        private UpdateScheduler _updateScheduler;
        private Coroutine _optimizationUpdateCoroutine;
        private Coroutine _performanceMonitoringCoroutine;
        
        // Quality Management
        private QualityLevel _currentQualityLevel = QualityLevel.High;
        private Dictionary<QualityLevel, QualitySettings> _qualitySettings = new Dictionary<QualityLevel, QualitySettings>();
        
        // Events
        public System.Action<PerformanceMetrics> OnPerformanceMetricsUpdated;
        public System.Action<QualityLevel, QualityLevel> OnQualityLevelChanged;
        public System.Action<int> OnPlantCulled;
        public System.Action<int> OnPlantLODChanged;
        public System.Action<OptimizationReport> OnOptimizationReportGenerated;
        
        // Properties
        public PerformanceMetrics CurrentMetrics => _currentMetrics;
        public QualityLevel CurrentQualityLevel => _currentQualityLevel;
        public int VisiblePlantCount => _cullingManager?.VisibleCount ?? 0;
        public float CurrentFrameRate => _currentMetrics.AverageFrameRate;
        public bool OptimizationEnabled => enabled;
        
        protected override void InitializeManager()
        {
            InitializeOptimizationSystems();
            InitializeQualitySettings();
            ConnectToGameSystems();
            InitializePerformanceMonitoring();
            StartOptimizationLoops();
            LogInfo("SpeedTree Optimization System initialized");
        }
        
        private void Update()
        {
            UpdateOptimizationSystems();
            UpdatePerformanceTracking();
            ProcessOptimizationQueue();
        }
        
        private void LateUpdate()
        {
            UpdateLODSystems();
            UpdateCullingSystem();
            UpdateBatchingSystem();
        }
        
        #region Initialization
        
        private void InitializeOptimizationSystems()
        {
            // Initialize core optimization managers
            _lodManager = new AdvancedLODManager(_lodConfig, _enableDynamicLOD);
            _batchingSystem = new DynamicBatchingSystem(_batchingConfig, _enableBatching, _enableGPUInstancing);
            _cullingManager = new SmartCullingManager(_cullingConfig, _enableOcclusionCulling, _enableFrustumCulling, _enableDistanceCulling);
            _memoryManager = new MemoryOptimizationManager(_enableMemoryPooling, _maxMemoryUsageMB);
            _performanceMonitor = new PerformanceMonitoringSystem(_targetFrameRate);
            
            // Initialize specialized components
            _instancePool = new PlantInstancePool(_memoryPoolSize);
            _textureManager = new TextureStreamingManager(_enableAssetStreaming);
            _shaderManager = new ShaderOptimizationManager(_enableTextureCompression);
            _renderQueueOptimizer = new RenderQueueOptimizer();
            
            // Initialize update scheduler
            _updateScheduler = new UpdateScheduler(_enableAdaptiveUpdates);
            
            LogInfo("Optimization subsystems initialized");
        }
        
        private void InitializeQualitySettings()
        {
            // Define quality level settings
            _qualitySettings[QualityLevel.Low] = new QualitySettings
            {
                MaxVisiblePlants = 100,
                LODDistanceMultiplier = 0.5f,
                CullingDistance = 50f,
                ShadowQuality = ShadowQuality.Disable,
                TextureQuality = 0,
                AnisotropicFiltering = AnisotropicFiltering.Disable,
                AntiAliasing = 0,
                VSyncCount = 0,
                TargetFrameRate = 30
            };
            
            _qualitySettings[QualityLevel.Medium] = new QualitySettings
            {
                MaxVisiblePlants = 250,
                LODDistanceMultiplier = 0.75f,
                CullingDistance = 100f,
                ShadowQuality = ShadowQuality.HardOnly,
                TextureQuality = 1,
                AnisotropicFiltering = AnisotropicFiltering.Enable,
                AntiAliasing = 2,
                VSyncCount = 0,
                TargetFrameRate = 45
            };
            
            _qualitySettings[QualityLevel.High] = new QualitySettings
            {
                MaxVisiblePlants = 500,
                LODDistanceMultiplier = 1f,
                CullingDistance = 200f,
                ShadowQuality = ShadowQuality.All,
                TextureQuality = 2,
                AnisotropicFiltering = AnisotropicFiltering.ForceEnable,
                AntiAliasing = 4,
                VSyncCount = 1,
                TargetFrameRate = 60
            };
            
            _qualitySettings[QualityLevel.Ultra] = new QualitySettings
            {
                MaxVisiblePlants = 1000,
                LODDistanceMultiplier = 1.25f,
                CullingDistance = 300f,
                ShadowQuality = ShadowQuality.All,
                TextureQuality = 3,
                AnisotropicFiltering = AnisotropicFiltering.ForceEnable,
                AntiAliasing = 8,
                VSyncCount = 1,
                TargetFrameRate = 60
            };
            
            // Apply initial quality settings
            ApplyQualityLevel(_currentQualityLevel);
        }
        
        private void ConnectToGameSystems()
        {
            if (GameManager.Instance != null)
            {
                _speedTreeManager = GameManager.Instance.GetManager<AdvancedSpeedTreeManager>();
                _environmentalSystem = GameManager.Instance.GetManager<SpeedTreeEnvironmentalSystem>();
                _growthSystem = GameManager.Instance.GetManager<SpeedTreeGrowthSystem>();
            }
            
            ConnectSystemEvents();
        }
        
        private void ConnectSystemEvents()
        {
            if (_speedTreeManager != null)
            {
                _speedTreeManager.OnPlantInstanceCreated += HandlePlantInstanceCreated;
                _speedTreeManager.OnPlantInstanceDestroyed += HandlePlantInstanceDestroyed;
                _speedTreeManager.OnPerformanceMetricsUpdated += HandleSpeedTreePerformanceUpdate;
            }
            
            if (_environmentalSystem != null)
            {
                _environmentalSystem.OnMetricsUpdated += HandleEnvironmentalPerformanceUpdate;
            }
            
            if (_growthSystem != null)
            {
                _growthSystem.OnPerformanceMetricsUpdated += HandleGrowthPerformanceUpdate;
            }
        }
        
        private void InitializePerformanceMonitoring()
        {
            _currentMetrics = new PerformanceMetrics
            {
                FrameTime = 16.67f, // 60 FPS target
                AverageFrameRate = 60f,
                VisiblePlants = 0,
                DrawCalls = 0,
                Batches = 0,
                MemoryUsage = 0f,
                CulledPlants = 0,
                LODTransitions = 0,
                LastUpdate = DateTime.Now
            };
            
            InvokeRepeating(nameof(UpdatePerformanceMetrics), 1f, 1f);
        }
        
        private void StartOptimizationLoops()
        {
            _optimizationUpdateCoroutine = StartCoroutine(OptimizationUpdateLoop());
            _performanceMonitoringCoroutine = StartCoroutine(PerformanceMonitoringLoop());
        }
        
        private IEnumerator OptimizationUpdateLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f); // 10 FPS optimization updates
                
                ProcessLODOptimization();
                ProcessCullingOptimization();
                ProcessBatchingOptimization();
                ProcessMemoryOptimization();
                
                if (_enableDynamicQuality)
                {
                    ProcessDynamicQualityAdjustment();
                }
            }
        }
        
        private IEnumerator PerformanceMonitoringLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f); // Detailed monitoring every 5 seconds
                
                CollectDetailedPerformanceData();
                AnalyzePerformanceTrends();
                GenerateOptimizationRecommendations();
            }
        }
        
        #endregion
        
        #region Plant Registration and Tracking
        
        public void RegisterPlantForOptimization(SpeedTreePlantInstance instance)
        {
            if (instance == null) return;
            
            var performanceData = new PlantPerformanceData
            {
                InstanceId = instance.InstanceId,
                PlantInstance = instance,
                LastVisibleTime = Time.time,
                RenderTime = 0f,
                DistanceToCamera = CalculateDistanceToMainCamera(instance.Position),
                CurrentLODLevel = 0,
                IsCulled = false,
                IsBatched = false,
                MemoryFootprint = CalculateMemoryFootprint(instance),
                OptimizationPriority = CalculateOptimizationPriority(instance)
            };
            
            _plantPerformanceData[instance.InstanceId] = performanceData;
            
            // Initialize optimization state
            var optimizationState = new OptimizationState
            {
                InstanceId = instance.InstanceId,
                IsOptimized = false,
                OptimizationLevel = OptimizationLevel.None,
                LastOptimizationTime = Time.time,
                OptimizationHistory = new List<OptimizationAction>()
            };
            
            _plantOptimizationStates[instance.InstanceId] = optimizationState;
            
            // Register with subsystems
            _lodManager.RegisterPlant(instance);
            _cullingManager.RegisterPlant(instance);
            _batchingSystem.RegisterPlant(instance);
            
            LogInfo($"Registered plant {instance.InstanceId} for optimization tracking");
        }
        
        public void UnregisterPlantFromOptimization(int instanceId)
        {
            _plantPerformanceData.Remove(instanceId);
            _plantOptimizationStates.Remove(instanceId);
            
            _lodManager.UnregisterPlant(instanceId);
            _cullingManager.UnregisterPlant(instanceId);
            _batchingSystem.UnregisterPlant(instanceId);
            
            LogInfo($"Unregistered plant {instanceId} from optimization tracking");
        }
        
        private float CalculateDistanceToMainCamera(Vector3 position)
        {
            var mainCamera = Camera.main;
            if (mainCamera == null) return 1000f; // Very far if no camera
            
            return Vector3.Distance(position, mainCamera.transform.position);
        }
        
        private float CalculateMemoryFootprint(SpeedTreePlantInstance instance)
        {
            // Estimate memory usage based on plant complexity
            var baseMemory = 2f; // MB base per plant
            
            if (instance.GeneticData != null)
            {
                baseMemory *= instance.GeneticData.PlantSize; // Larger plants use more memory
            }
            
            if (instance.GrowthStage == PlantGrowthStage.Flowering || 
                instance.GrowthStage == PlantGrowthStage.Harvest)
            {
                baseMemory *= 1.5f; // Complex flowering plants use more memory
            }
            
            return baseMemory;
        }
        
        private int CalculateOptimizationPriority(SpeedTreePlantInstance instance)
        {
            var priority = 1; // Base priority
            
            // Higher priority for plants closer to camera
            var distance = CalculateDistanceToMainCamera(instance.Position);
            if (distance < 10f) priority += 3;
            else if (distance < 25f) priority += 2;
            else if (distance < 50f) priority += 1;
            
            // Higher priority for flowering/harvest plants (more visually important)
            if (instance.GrowthStage == PlantGrowthStage.Flowering || 
                instance.GrowthStage == PlantGrowthStage.Harvest)
            {
                priority += 2;
            }
            
            // Higher priority for unhealthy plants (might need attention)
            if (instance.Health < 50f)
            {
                priority += 1;
            }
            
            return Mathf.Clamp(priority, 1, 10);
        }
        
        #endregion
        
        #region LOD Optimization
        
        private void ProcessLODOptimization()
        {
            if (!_enableDynamicLOD) return;
            
            var plantsToProcess = GetPlantsForLODUpdate();
            
            foreach (var performanceData in plantsToProcess)
            {
                ProcessPlantLODOptimization(performanceData);
            }
        }
        
        private List<PlantPerformanceData> GetPlantsForLODUpdate()
        {
            return _plantPerformanceData.Values
                .Where(p => !p.IsCulled && ShouldUpdateLOD(p))
                .OrderBy(p => p.DistanceToCamera)
                .Take(GetMaxLODUpdatesPerFrame())
                .ToList();
        }
        
        private bool ShouldUpdateLOD(PlantPerformanceData performanceData)
        {
            var timeSinceLastUpdate = Time.time - performanceData.LastLODUpdateTime;
            var updateFrequency = GetLODUpdateFrequency(performanceData);
            
            return timeSinceLastUpdate >= updateFrequency;
        }
        
        private float GetLODUpdateFrequency(PlantPerformanceData performanceData)
        {
            // Closer plants update LOD more frequently
            if (performanceData.DistanceToCamera < 10f) return 0.1f;  // 10 FPS
            if (performanceData.DistanceToCamera < 25f) return 0.2f;  // 5 FPS
            if (performanceData.DistanceToCamera < 50f) return 0.5f;  // 2 FPS
            return 1f; // 1 FPS for distant plants
        }
        
        private int GetMaxLODUpdatesPerFrame()
        {
            var frameRate = 1f / Time.unscaledDeltaTime;
            
            if (frameRate > 50f) return 20;
            if (frameRate > 30f) return 15;
            if (frameRate > 20f) return 10;
            return 5; // Reduce LOD updates when performance is poor
        }
        
        private void ProcessPlantLODOptimization(PlantPerformanceData performanceData)
        {
            var instance = performanceData.PlantInstance;
            var distance = performanceData.DistanceToCamera;
            
            var newLODLevel = _lodManager.CalculateOptimalLOD(instance, distance);
            
            if (newLODLevel != performanceData.CurrentLODLevel)
            {
                ApplyLODChange(performanceData, newLODLevel);
                OnPlantLODChanged?.Invoke(instance.InstanceId);
            }
            
            performanceData.LastLODUpdateTime = Time.time;
        }
        
        private void ApplyLODChange(PlantPerformanceData performanceData, int newLODLevel)
        {
            var instance = performanceData.PlantInstance;
            
#if UNITY_SPEEDTREE
            if (instance.Renderer != null)
            {
                _lodManager.ApplyLODLevel(instance.Renderer, newLODLevel);
            }
#endif
            
            performanceData.CurrentLODLevel = newLODLevel;
            
            // Record optimization action
            RecordOptimizationAction(performanceData.InstanceId, OptimizationActionType.LODChange, 
                $"Changed to LOD {newLODLevel}");
        }
        
        #endregion
        
        #region Culling Optimization
        
        private void ProcessCullingOptimization()
        {
            UpdateCameraOptimizationData();
            ProcessFrustumCulling();
            ProcessDistanceCulling();
            ProcessOcclusionCulling();
        }
        
        private void UpdateCameraOptimizationData()
        {
            var cameras = Camera.allCameras;
            
            foreach (var camera in cameras)
            {
                if (!_cameraOptimization.ContainsKey(camera))
                {
                    _cameraOptimization[camera] = new CameraOptimizationData
                    {
                        Camera = camera,
                        LastPosition = camera.transform.position,
                        LastRotation = camera.transform.rotation,
                        LastUpdateTime = Time.time
                    };
                }
                
                var cameraData = _cameraOptimization[camera];
                cameraData.HasMoved = Vector3.Distance(camera.transform.position, cameraData.LastPosition) > 0.1f;
                cameraData.HasRotated = Quaternion.Angle(camera.transform.rotation, cameraData.LastRotation) > 1f;
                
                if (cameraData.HasMoved || cameraData.HasRotated)
                {
                    cameraData.LastPosition = camera.transform.position;
                    cameraData.LastRotation = camera.transform.rotation;
                    cameraData.LastUpdateTime = Time.time;
                    cameraData.RequiresUpdate = true;
                }
            }
        }
        
        private void ProcessFrustumCulling()
        {
            if (!_enableFrustumCulling) return;
            
            var mainCamera = Camera.main;
            if (mainCamera == null) return;
            
            var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            
            foreach (var performanceData in _plantPerformanceData.Values)
            {
                var instance = performanceData.PlantInstance;
                var bounds = GetPlantBounds(instance);
                
                var isVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
                
                if (performanceData.IsCulled && isVisible)
                {
                    UnculPlant(performanceData);
                }
                else if (!performanceData.IsCulled && !isVisible)
                {
                    CullPlant(performanceData, CullReason.Frustum);
                }
            }
        }
        
        private void ProcessDistanceCulling()
        {
            if (!_enableDistanceCulling) return;
            
            var cullingDistance = GetCurrentCullingDistance();
            
            foreach (var performanceData in _plantPerformanceData.Values)
            {
                var shouldCull = performanceData.DistanceToCamera > cullingDistance;
                
                if (performanceData.IsCulled && !shouldCull)
                {
                    UnculPlant(performanceData);
                }
                else if (!performanceData.IsCulled && shouldCull)
                {
                    CullPlant(performanceData, CullReason.Distance);
                }
            }
        }
        
        private void ProcessOcclusionCulling()
        {
            if (!_enableOcclusionCulling) return;
            
            // Basic occlusion culling - plants behind other large objects
            // This would use Unity's built-in occlusion culling or custom raycast-based system
            
            foreach (var performanceData in _plantPerformanceData.Values)
            {
                if (performanceData.IsCulled) continue;
                
                var isOccluded = CheckPlantOcclusion(performanceData.PlantInstance);
                
                if (isOccluded)
                {
                    CullPlant(performanceData, CullReason.Occlusion);
                }
            }
        }
        
        private bool CheckPlantOcclusion(SpeedTreePlantInstance instance)
        {
            var mainCamera = Camera.main;
            if (mainCamera == null) return false;
            
            var rayDirection = (instance.Position - mainCamera.transform.position).normalized;
            var distance = Vector3.Distance(mainCamera.transform.position, instance.Position);
            
            // Simple raycast to check for occlusion
            if (Physics.Raycast(mainCamera.transform.position, rayDirection, out var hit, distance - 1f))
            {
                // If we hit something that's not the plant itself, it's occluded
                return !hit.collider.transform.IsChildOf(instance.Renderer?.transform);
            }
            
            return false;
        }
        
        private void CullPlant(PlantPerformanceData performanceData, CullReason reason)
        {
            performanceData.IsCulled = true;
            performanceData.CullReason = reason;
            
            var instance = performanceData.PlantInstance;
            
#if UNITY_SPEEDTREE
            if (instance.Renderer != null)
            {
                instance.Renderer.enabled = false;
            }
#endif
            
            _currentMetrics.CulledPlants++;
            OnPlantCulled?.Invoke(instance.InstanceId);
            
            RecordOptimizationAction(performanceData.InstanceId, OptimizationActionType.Cull, 
                $"Culled: {reason}");
        }
        
        private void UnculPlant(PlantPerformanceData performanceData)
        {
            performanceData.IsCulled = false;
            performanceData.CullReason = CullReason.None;
            
            var instance = performanceData.PlantInstance;
            
#if UNITY_SPEEDTREE
            if (instance.Renderer != null)
            {
                instance.Renderer.enabled = true;
            }
#endif
            
            _currentMetrics.CulledPlants--;
            
            RecordOptimizationAction(performanceData.InstanceId, OptimizationActionType.Uncull, 
                "Unculled: Back in view");
        }
        
        private Bounds GetPlantBounds(SpeedTreePlantInstance instance)
        {
            if (instance.Renderer != null)
            {
                return instance.Renderer.bounds;
            }
            
            // Fallback bounds calculation
            var size = instance.Scale.x * 2f;
            return new Bounds(instance.Position, Vector3.one * size);
        }
        
        private float GetCurrentCullingDistance()
        {
            return _qualitySettings[_currentQualityLevel].CullingDistance;
        }
        
        #endregion
        
        #region Batching Optimization
        
        private void ProcessBatchingOptimization()
        {
            if (!_enableBatching) return;
            
            _batchingSystem.ProcessBatching(_plantPerformanceData.Values.Where(p => !p.IsCulled).ToList());
            
            // Update batching statistics
            _currentMetrics.Batches = _batchingSystem.CurrentBatchCount;
            _currentMetrics.DrawCalls = _batchingSystem.EstimatedDrawCalls;
        }
        
        #endregion
        
        #region Memory Optimization
        
        private void ProcessMemoryOptimization()
        {
            // Monitor memory usage
            var currentMemoryUsage = GC.GetTotalMemory(false) / 1024f / 1024f; // Convert to MB
            _currentMetrics.MemoryUsage = currentMemoryUsage;
            
            // Trigger memory optimization if needed
            if (currentMemoryUsage > _maxMemoryUsageMB * 0.8f) // 80% threshold
            {
                TriggerMemoryOptimization();
            }
            
            // Update texture streaming
            _textureManager.UpdateStreaming(_plantPerformanceData.Values.ToList());
            
            // Process instance pooling
            _instancePool.ProcessPooling();
        }
        
        private void TriggerMemoryOptimization()
        {
            // Reduce texture quality for distant plants
            var distantPlants = _plantPerformanceData.Values
                .Where(p => p.DistanceToCamera > 50f)
                .OrderByDescending(p => p.DistanceToCamera)
                .Take(10);
            
            foreach (var plant in distantPlants)
            {
                _textureManager.ReduceTextureQuality(plant.PlantInstance);
            }
            
            // Force garbage collection if memory is critically high
            if (_currentMetrics.MemoryUsage > _maxMemoryUsageMB * 0.9f)
            {
                GC.Collect();
                RecordOptimizationAction(-1, OptimizationActionType.MemoryCleanup, "Forced GC");
            }
        }
        
        #endregion
        
        #region Dynamic Quality Adjustment
        
        private void ProcessDynamicQualityAdjustment()
        {
            if (!_enableDynamicQuality) return;
            
            var currentFrameRate = 1f / Time.unscaledDeltaTime;
            var targetFrameRate = _qualitySettings[_currentQualityLevel].TargetFrameRate;
            
            // Check if we need to adjust quality
            if (currentFrameRate < targetFrameRate - 5f && _currentQualityLevel > QualityLevel.Low)
            {
                // Reduce quality
                var newQuality = (QualityLevel)((int)_currentQualityLevel - 1);
                ChangeQualityLevel(newQuality);
            }
            else if (currentFrameRate > targetFrameRate + 10f && _currentQualityLevel < QualityLevel.Ultra)
            {
                // Increase quality
                var newQuality = (QualityLevel)((int)_currentQualityLevel + 1);
                ChangeQualityLevel(newQuality);
            }
        }
        
        public void ChangeQualityLevel(QualityLevel newQuality)
        {
            if (newQuality == _currentQualityLevel) return;
            
            var oldQuality = _currentQualityLevel;
            _currentQualityLevel = newQuality;
            
            ApplyQualityLevel(newQuality);
            OnQualityLevelChanged?.Invoke(oldQuality, newQuality);
            
            LogInfo($"Quality level changed from {oldQuality} to {newQuality}");
        }
        
        private void ApplyQualityLevel(QualityLevel quality)
        {
            if (!_qualitySettings.TryGetValue(quality, out var settings)) return;
            
            // Apply Unity quality settings
            QualitySettings.shadowResolution = (ShadowResolution)(int)settings.ShadowQuality;
            QualitySettings.masterTextureLimit = 3 - settings.TextureQuality;
            QualitySettings.anisotropicFiltering = settings.AnisotropicFiltering;
            QualitySettings.antiAliasing = settings.AntiAliasing;
            QualitySettings.vSyncCount = settings.VSyncCount;
            Application.targetFrameRate = settings.TargetFrameRate;
            
            // Apply SpeedTree-specific settings
            _maxVisiblePlants = settings.MaxVisiblePlants;
            _lodManager.SetLODDistanceMultiplier(settings.LODDistanceMultiplier);
            _cullingManager.SetCullingDistance(settings.CullingDistance);
        }
        
        #endregion
        
        #region Performance Monitoring and Analytics
        
        private void UpdatePerformanceTracking()
        {
            // Update basic performance metrics
            _currentMetrics.FrameTime = Time.unscaledDeltaTime * 1000f; // ms
            _currentMetrics.AverageFrameRate = 1f / Time.unscaledDeltaTime;
            _currentMetrics.VisiblePlants = _plantPerformanceData.Values.Count(p => !p.IsCulled);
            
            // Update plant-specific performance data
            foreach (var performanceData in _plantPerformanceData.Values)
            {
                UpdatePlantPerformanceData(performanceData);
            }
        }
        
        private void UpdatePlantPerformanceData(PlantPerformanceData performanceData)
        {
            var instance = performanceData.PlantInstance;
            
            // Update distance to camera
            performanceData.DistanceToCamera = CalculateDistanceToMainCamera(instance.Position);
            
            // Update visibility tracking
            if (!performanceData.IsCulled)
            {
                performanceData.LastVisibleTime = Time.time;
                performanceData.TotalVisibleTime += Time.unscaledDeltaTime;
            }
            
            // Estimate render time (simplified)
            if (!performanceData.IsCulled)
            {
                performanceData.RenderTime = EstimateRenderTime(instance, performanceData.CurrentLODLevel);
            }
        }
        
        private float EstimateRenderTime(SpeedTreePlantInstance instance, int lodLevel)
        {
            var baseRenderTime = 0.1f; // ms base
            
            // LOD affects render time
            var lodMultiplier = lodLevel switch
            {
                0 => 1f,    // Full detail
                1 => 0.7f,  // Reduced detail
                2 => 0.4f,  // Low detail
                3 => 0.2f,  // Billboard
                _ => 0.1f   // Culled
            };
            
            // Plant complexity affects render time
            if (instance.GrowthStage == PlantGrowthStage.Flowering || 
                instance.GrowthStage == PlantGrowthStage.Harvest)
            {
                baseRenderTime *= 1.5f; // More complex plants take longer
            }
            
            return baseRenderTime * lodMultiplier;
        }
        
        private void CollectDetailedPerformanceData()
        {
            var snapshot = new PerformanceSnapshot
            {
                Timestamp = DateTime.Now,
                FrameRate = _currentMetrics.AverageFrameRate,
                MemoryUsage = _currentMetrics.MemoryUsage,
                VisiblePlants = _currentMetrics.VisiblePlants,
                CulledPlants = _currentMetrics.CulledPlants,
                DrawCalls = _currentMetrics.DrawCalls,
                Batches = _currentMetrics.Batches,
                QualityLevel = _currentQualityLevel
            };
            
            _performanceHistory.Add(snapshot);
            
            // Maintain history size
            if (_performanceHistory.Count > 1000)
            {
                _performanceHistory.RemoveAt(0);
            }
        }
        
        private void AnalyzePerformanceTrends()
        {
            if (_performanceHistory.Count < 10) return;
            
            var recent = _performanceHistory.TakeLast(10).ToList();
            var avgFrameRate = recent.Average(s => s.FrameRate);
            var avgMemoryUsage = recent.Average(s => s.MemoryUsage);
            
            // Detect performance trends
            if (avgFrameRate < _targetFrameRate * 0.8f)
            {
                // Performance is degrading
                LogWarning($"Performance degradation detected. Average FPS: {avgFrameRate:F1}");
                TriggerPerformanceOptimization();
            }
            
            if (avgMemoryUsage > _maxMemoryUsageMB * 0.9f)
            {
                // Memory usage is high
                LogWarning($"High memory usage detected: {avgMemoryUsage:F1} MB");
                TriggerMemoryOptimization();
            }
        }
        
        private void TriggerPerformanceOptimization()
        {
            // Reduce quality if not already at minimum
            if (_currentQualityLevel > QualityLevel.Low)
            {
                ChangeQualityLevel(_currentQualityLevel - 1);
            }
            
            // Increase culling aggressiveness
            _cullingManager.IncreaseCullingAggressiveness();
            
            // Reduce update frequencies
            _updateScheduler.ReduceUpdateFrequencies();
            
            RecordOptimizationAction(-1, OptimizationActionType.PerformanceOptimization, 
                "Triggered due to low FPS");
        }
        
        private void GenerateOptimizationRecommendations()
        {
            var recommendations = new List<string>();
            
            // Analyze current state and generate recommendations
            if (_currentMetrics.AverageFrameRate < _targetFrameRate * 0.9f)
            {
                recommendations.Add("Consider reducing plant density or quality level");
            }
            
            if (_currentMetrics.MemoryUsage > _maxMemoryUsageMB * 0.8f)
            {
                recommendations.Add("Enable texture streaming or reduce texture quality");
            }
            
            if (_currentMetrics.DrawCalls > _maxDrawCalls)
            {
                recommendations.Add("Enable batching or GPU instancing for better performance");
            }
            
            var visibilityRatio = (float)_currentMetrics.VisiblePlants / _plantPerformanceData.Count;
            if (visibilityRatio > 0.8f)
            {
                recommendations.Add("Increase culling distance or enable occlusion culling");
            }
            
            // Generate optimization report
            var report = new OptimizationReport
            {
                Timestamp = DateTime.Now,
                CurrentMetrics = _currentMetrics,
                QualityLevel = _currentQualityLevel,
                Recommendations = recommendations,
                PerformanceScore = CalculatePerformanceScore()
            };
            
            OnOptimizationReportGenerated?.Invoke(report);
        }
        
        private float CalculatePerformanceScore()
        {
            var frameRateScore = Mathf.Clamp01(_currentMetrics.AverageFrameRate / _targetFrameRate);
            var memoryScore = Mathf.Clamp01(1f - (_currentMetrics.MemoryUsage / _maxMemoryUsageMB));
            var drawCallScore = Mathf.Clamp01(1f - ((float)_currentMetrics.DrawCalls / _maxDrawCalls));
            
            return (frameRateScore + memoryScore + drawCallScore) / 3f;
        }
        
        #endregion
        
        #region System Updates
        
        private void UpdateOptimizationSystems()
        {
            _lodManager?.Update();
            _batchingSystem?.Update();
            _cullingManager?.Update();
            _memoryManager?.Update();
            _performanceMonitor?.Update();
            _instancePool?.Update();
            _textureManager?.Update();
            _shaderManager?.Update();
            _renderQueueOptimizer?.Update();
            _updateScheduler?.Update();
        }
        
        private void UpdateLODSystems()
        {
            if (_enableDynamicLOD)
            {
                _lodManager.UpdateLODs(_plantPerformanceData.Values.ToList());
            }
        }
        
        private void UpdateCullingSystem()
        {
            _cullingManager.UpdateCulling(_plantPerformanceData.Values.ToList());
        }
        
        private void UpdateBatchingSystem()
        {
            if (_enableBatching)
            {
                _batchingSystem.UpdateBatching();
            }
        }
        
        private void ProcessOptimizationQueue()
        {
            // Process any queued optimization operations
            _updateScheduler.ProcessQueue();
        }
        
        private void UpdatePerformanceMetrics()
        {
            _currentMetrics.LastUpdate = DateTime.Now;
            OnPerformanceMetricsUpdated?.Invoke(_currentMetrics);
        }
        
        #endregion
        
        #region Utility Methods
        
        private void RecordOptimizationAction(int instanceId, OptimizationActionType actionType, string description)
        {
            if (_plantOptimizationStates.TryGetValue(instanceId, out var state))
            {
                var action = new OptimizationAction
                {
                    ActionType = actionType,
                    Description = description,
                    Timestamp = DateTime.Now
                };
                
                state.OptimizationHistory.Add(action);
                
                // Maintain history size
                if (state.OptimizationHistory.Count > 100)
                {
                    state.OptimizationHistory.RemoveAt(0);
                }
            }
        }
        
        #endregion
        
        #region Event Handlers
        
        private void HandlePlantInstanceCreated(SpeedTreePlantInstance instance)
        {
            RegisterPlantForOptimization(instance);
        }
        
        private void HandlePlantInstanceDestroyed(SpeedTreePlantInstance instance)
        {
            UnregisterPlantFromOptimization(instance.InstanceId);
        }
        
        private void HandleSpeedTreePerformanceUpdate(SpeedTreePerformanceMetrics metrics)
        {
            // Integrate SpeedTree manager performance data
            _currentMetrics.FrameRate = metrics.FrameRate;
            _currentMetrics.MemoryUsage += metrics.MemoryUsage;
        }
        
        private void HandleEnvironmentalPerformanceUpdate(EnvironmentalSystemMetrics metrics)
        {
            // Integrate environmental system performance data
            _currentMetrics.ActivePlants = metrics.ActivePlants;
        }
        
        private void HandleGrowthPerformanceUpdate(GrowthPerformanceMetrics metrics)
        {
            // Integrate growth system performance data
            _currentMetrics.AnimationUpdatesPerSecond = metrics.AnimationUpdatesPerSecond;
        }
        
        #endregion
        
        #region Public Interface
        
        public PlantPerformanceData GetPlantPerformanceData(int instanceId)
        {
            return _plantPerformanceData.TryGetValue(instanceId, out var data) ? data : null;
        }
        
        public OptimizationState GetOptimizationState(int instanceId)
        {
            return _plantOptimizationStates.TryGetValue(instanceId, out var state) ? state : null;
        }
        
        public List<PerformanceSnapshot> GetPerformanceHistory(int maxEntries = 100)
        {
            return _performanceHistory.TakeLast(maxEntries).ToList();
        }
        
        public void SetTargetFrameRate(int frameRate)
        {
            _targetFrameRate = frameRate;
            _performanceMonitor.SetTargetFrameRate(frameRate);
        }
        
        public void SetMaxVisiblePlants(int maxPlants)
        {
            _maxVisiblePlants = maxPlants;
            _cullingManager.SetMaxVisiblePlants(maxPlants);
        }
        
        public void SetOptimizationEnabled(bool enabled)
        {
            this.enabled = enabled;
            
            if (!enabled)
            {
                // Disable all optimizations
                _enableDynamicLOD = false;
                _enableOcclusionCulling = false;
                _enableFrustumCulling = false;
                _enableDistanceCulling = false;
                _enableBatching = false;
                _enableDynamicQuality = false;
            }
        }
        
        public void ForceQualityLevel(QualityLevel quality)
        {
            _enableDynamicQuality = false;
            ChangeQualityLevel(quality);
        }
        
        public void EnableDynamicQuality()
        {
            _enableDynamicQuality = true;
        }
        
        public OptimizationSystemReport GetSystemReport()
        {
            return new OptimizationSystemReport
            {
                CurrentMetrics = _currentMetrics,
                QualityLevel = _currentQualityLevel,
                OptimizationSettings = new Dictionary<string, bool>
                {
                    ["DynamicLOD"] = _enableDynamicLOD,
                    ["OcclusionCulling"] = _enableOcclusionCulling,
                    ["FrustumCulling"] = _enableFrustumCulling,
                    ["DistanceCulling"] = _enableDistanceCulling,
                    ["Batching"] = _enableBatching,
                    ["GPUInstancing"] = _enableGPUInstancing,
                    ["DynamicQuality"] = _enableDynamicQuality,
                    ["MemoryPooling"] = _enableMemoryPooling,
                    ["TextureStreaming"] = _enableAssetStreaming
                },
                PlantPerformanceData = _plantPerformanceData.Values.ToList(),
                PerformanceScore = CalculatePerformanceScore(),
                ReportGenerated = DateTime.Now
            };
        }
        
        #endregion
        
        protected override void OnManagerShutdown()
        {
            // Stop coroutines
            if (_optimizationUpdateCoroutine != null)
            {
                StopCoroutine(_optimizationUpdateCoroutine);
            }
            if (_performanceMonitoringCoroutine != null)
            {
                StopCoroutine(_performanceMonitoringCoroutine);
            }
            CancelInvoke();
            
            // Cleanup systems
            _lodManager?.Cleanup();
            _batchingSystem?.Cleanup();
            _cullingManager?.Cleanup();
            _memoryManager?.Cleanup();
            _performanceMonitor?.Cleanup();
            _instancePool?.Cleanup();
            _textureManager?.Cleanup();
            _shaderManager?.Cleanup();
            _renderQueueOptimizer?.Cleanup();
            _updateScheduler?.Cleanup();
            
            // Clear data
            _plantPerformanceData.Clear();
            _plantOptimizationStates.Clear();
            _cameraOptimization.Clear();
            _performanceHistory.Clear();
            
            // Disconnect events
            DisconnectSystemEvents();
            
            LogInfo("SpeedTree Optimization System shutdown complete");
        }
        
        private void DisconnectSystemEvents()
        {
            if (_speedTreeManager != null)
            {
                _speedTreeManager.OnPlantInstanceCreated -= HandlePlantInstanceCreated;
                _speedTreeManager.OnPlantInstanceDestroyed -= HandlePlantInstanceDestroyed;
                _speedTreeManager.OnPerformanceMetricsUpdated -= HandleSpeedTreePerformanceUpdate;
            }
            
            if (_environmentalSystem != null)
            {
                _environmentalSystem.OnMetricsUpdated -= HandleEnvironmentalPerformanceUpdate;
            }
            
            if (_growthSystem != null)
            {
                _growthSystem.OnPerformanceMetricsUpdated -= HandleGrowthPerformanceUpdate;
            }
        }
    }
}