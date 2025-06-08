using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Genetics;
using ProjectChimera.Data.Environment;
using System.Collections.Generic;
using System.Linq;

namespace ProjectChimera.Systems.Cultivation
{
    /// <summary>
    /// Central manager for all plant cultivation activities including growth lifecycle,
    /// environmental interactions, and plant health management.
    /// </summary>
    public class PlantManager : ChimeraManager
    {
        [Header("Plant Management")]
        [SerializeField] private float _plantUpdateInterval = 1f; // seconds
        [SerializeField] private int _maxPlantsPerUpdate = 10;
        [SerializeField] private bool _enableDetailedLogging = false;
        
        [Header("Growth Configuration")]
        [SerializeField] private AnimationCurve _defaultGrowthCurve;
        [SerializeField] private float _globalGrowthModifier = 1f;
        [SerializeField] private bool _enableStressSystem = true;
        [SerializeField] private bool _enableGxEInteractions = true;
        
        [Header("Event Channels")]
        [SerializeField] private GameEventSO<PlantInstance> _onPlantCreated;
        [SerializeField] private GameEventSO<PlantInstance> _onPlantGrowthStageChanged;
        [SerializeField] private GameEventSO<PlantInstance> _onPlantHealthChanged;
        [SerializeField] private GameEventSO<PlantInstance> _onPlantHarvested;
        [SerializeField] private GameEventSO<PlantInstance> _onPlantDied;
        
        // Private fields
        private Dictionary<string, PlantInstance> _activePlants = new Dictionary<string, PlantInstance>();
        private List<PlantInstance> _plantsToUpdate = new List<PlantInstance>();
        private int _currentUpdateIndex = 0;
        private float _lastUpdateTime = 0f;
        private PlantUpdateProcessor _updateProcessor;
        
        public override ManagerPriority Priority => ManagerPriority.High;
        
        // Public Properties
        public int ActivePlantCount => _activePlants.Count;
        public float GlobalGrowthModifier 
        { 
            get => _globalGrowthModifier; 
            set => _globalGrowthModifier = Mathf.Clamp(value, 0.1f, 10f); 
        }
        
        protected override void OnManagerInitialize()
        {
            _updateProcessor = new PlantUpdateProcessor(_enableStressSystem, _enableGxEInteractions);
            
            LogInfo($"PlantManager initialized with {_activePlants.Count} plants");
            
            // Initialize growth curve if not set
            if (_defaultGrowthCurve == null || _defaultGrowthCurve.keys.Length == 0)
            {
                InitializeDefaultGrowthCurve();
            }
        }

        protected override void OnManagerShutdown()
        {
            LogInfo("PlantManager shutting down");
            
            // Clean up all plants
            foreach (var plant in _activePlants.Values)
            {
                if (plant != null)
                {
                    Destroy(plant.gameObject);
                }
            }
            
            _activePlants.Clear();
            _plantsToUpdate.Clear();
            _updateProcessor = null;
        }
        
        protected override void OnManagerUpdate()
        {
            if (Time.time - _lastUpdateTime >= _plantUpdateInterval)
            {
                UpdatePlants();
                _lastUpdateTime = Time.time;
            }
        }
        
        /// <summary>
        /// Creates a new plant instance from a strain definition.
        /// </summary>
        public PlantInstance CreatePlant(PlantStrainSO strain, Vector3 position, Transform parent = null)
        {
            if (strain == null)
            {
                LogError("Cannot create plant: strain is null");
                return null;
            }
            
            var plantInstance = PlantInstance.CreateFromStrain(strain, position, parent);
            RegisterPlant(plantInstance);
            
            LogInfo($"Created plant: {plantInstance.PlantID} (Strain: {strain.StrainName})");
            _onPlantCreated?.Raise(plantInstance);
            
            return plantInstance;
        }
        
        /// <summary>
        /// Creates multiple plants from the same strain.
        /// </summary>
        public List<PlantInstance> CreatePlants(PlantStrainSO strain, List<Vector3> positions, Transform parent = null)
        {
            var plants = new List<PlantInstance>();
            
            foreach (var position in positions)
            {
                var plant = CreatePlant(strain, position, parent);
                if (plant != null)
                    plants.Add(plant);
            }
            
            LogInfo($"Created {plants.Count} plants from strain: {strain.StrainName}");
            return plants;
        }
        
        /// <summary>
        /// Registers an existing plant instance with the manager.
        /// </summary>
        public void RegisterPlant(PlantInstance plant)
        {
            if (plant == null)
            {
                LogError("Cannot register null plant");
                return;
            }
            
            if (_activePlants.ContainsKey(plant.PlantID))
            {
                LogWarning($"Plant {plant.PlantID} already registered, skipping");
                return;
            }
            
            _activePlants[plant.PlantID] = plant;
            _plantsToUpdate.Add(plant);
            
            // Subscribe to plant events
            plant.OnGrowthStageChanged += OnPlantGrowthStageChanged;
            plant.OnHealthChanged += OnPlantHealthChanged;
            plant.OnPlantDied += OnPlantDied;
            
            if (_enableDetailedLogging)
                LogInfo($"Registered plant: {plant.PlantID}");
        }
        
        /// <summary>
        /// Removes a plant from management (for harvesting, death, etc.).
        /// </summary>
        public void UnregisterPlant(string plantID, PlantRemovalReason reason = PlantRemovalReason.Other)
        {
            if (!_activePlants.TryGetValue(plantID, out var plant))
            {
                LogWarning($"Attempted to unregister unknown plant: {plantID}");
                return;
            }
            
            // Unsubscribe from events
            plant.OnGrowthStageChanged -= OnPlantGrowthStageChanged;
            plant.OnHealthChanged -= OnPlantHealthChanged;
            plant.OnPlantDied -= OnPlantDied;
            
            _activePlants.Remove(plantID);
            _plantsToUpdate.Remove(plant);
            
            // Raise appropriate event
            switch (reason)
            {
                case PlantRemovalReason.Harvested:
                    _onPlantHarvested?.Raise(plant);
                    break;
                case PlantRemovalReason.Died:
                    _onPlantDied?.Raise(plant);
                    break;
            }
            
            LogInfo($"Unregistered plant: {plantID} (Reason: {reason})");
        }
        
        /// <summary>
        /// Gets a plant instance by ID.
        /// </summary>
        public PlantInstance GetPlant(string plantID)
        {
            _activePlants.TryGetValue(plantID, out var plant);
            return plant;
        }
        
        /// <summary>
        /// Gets all plants in a specific growth stage.
        /// </summary>
        public List<PlantInstance> GetPlantsInStage(PlantGrowthStage stage)
        {
            return _activePlants.Values.Where(p => p.CurrentGrowthStage == stage).ToList();
        }
        
        /// <summary>
        /// Gets all plants ready for harvest.
        /// </summary>
        public List<PlantInstance> GetHarvestablePlants()
        {
            return GetPlantsInStage(PlantGrowthStage.Harvest);
        }
        
        /// <summary>
        /// Gets all plants that need attention (low health, stress, etc.).
        /// </summary>
        public List<PlantInstance> GetPlantsNeedingAttention()
        {
            return _activePlants.Values.Where(p => 
                p.CurrentHealth < 0.7f || 
                p.StressLevel > 0.3f || 
                p.HasActiveStressors()
            ).ToList();
        }
        
        /// <summary>
        /// Updates the environmental conditions for all plants.
        /// </summary>
        public void UpdateEnvironmentalConditions(EnvironmentalConditions newConditions)
        {
            foreach (var plant in _activePlants.Values)
            {
                plant.UpdateEnvironmentalConditions(newConditions);
            }
            
            LogInfo($"Updated environmental conditions for {_activePlants.Count} plants");
        }
        
        /// <summary>
        /// Applies environmental stress to plants based on conditions.
        /// </summary>
        public void ApplyEnvironmentalStress(EnvironmentalStressSO stressSource, float intensity)
        {
            if (!_enableStressSystem || stressSource == null)
                return;
            
            int affectedPlants = 0;
            foreach (var plant in _activePlants.Values)
            {
                if (plant.ApplyStress(stressSource, intensity))
                    affectedPlants++;
            }
            
            LogInfo($"Applied stress '{stressSource.StressName}' to {affectedPlants} plants");
        }
        
        /// <summary>
        /// Harvests a plant and returns the harvest results.
        /// </summary>
        public HarvestResults HarvestPlant(string plantID)
        {
            var plant = GetPlant(plantID);
            if (plant == null)
            {
                LogError($"Cannot harvest unknown plant: {plantID}");
                return null;
            }
            
            if (plant.CurrentGrowthStage != PlantGrowthStage.Harvest)
            {
                LogWarning($"Plant {plantID} is not ready for harvest (Stage: {plant.CurrentGrowthStage})");
                return null;
            }
            
            var harvestResults = plant.Harvest();
            UnregisterPlant(plantID, PlantRemovalReason.Harvested);
            
            LogInfo($"Harvested plant {plantID}: {harvestResults.TotalYieldGrams}g yield, {harvestResults.QualityScore:F2} quality");
            
            return harvestResults;
        }
        
        /// <summary>
        /// Gets comprehensive statistics about all managed plants.
        /// </summary>
        public PlantManagerStatistics GetStatistics()
        {
            var stats = new PlantManagerStatistics();
            
            foreach (var plant in _activePlants.Values)
            {
                stats.TotalPlants++;
                stats.PlantsByStage[(int)plant.CurrentGrowthStage]++;
                stats.AverageHealth += plant.CurrentHealth;
                stats.AverageStress += plant.StressLevel;
                
                if (plant.CurrentHealth < 0.5f)
                    stats.UnhealthyPlants++;
                
                if (plant.StressLevel > 0.7f)
                    stats.HighStressPlants++;
            }
            
            if (stats.TotalPlants > 0)
            {
                stats.AverageHealth /= stats.TotalPlants;
                stats.AverageStress /= stats.TotalPlants;
            }
            
            return stats;
        }
        
        private void UpdatePlants()
        {
            if (_plantsToUpdate.Count == 0)
                return;
            
            var timeManager = GameManager.Instance.GetManager<TimeManager>();
            float deltaTime = timeManager.GetScaledDeltaTime();
            
            int plantsToProcess = Mathf.Min(_maxPlantsPerUpdate, _plantsToUpdate.Count);
            int endIndex = Mathf.Min(_currentUpdateIndex + plantsToProcess, _plantsToUpdate.Count);
            
            for (int i = _currentUpdateIndex; i < endIndex; i++)
            {
                var plant = _plantsToUpdate[i];
                if (plant != null && plant.IsActive)
                {
                    _updateProcessor.UpdatePlant(plant, deltaTime, _globalGrowthModifier);
                }
            }
            
            _currentUpdateIndex = endIndex;
            
            // Reset index when we've processed all plants
            if (_currentUpdateIndex >= _plantsToUpdate.Count)
            {
                _currentUpdateIndex = 0;
                
                // Remove any inactive plants from the update list
                _plantsToUpdate.RemoveAll(p => p == null || !p.IsActive);
            }
        }
        
        private void OnPlantGrowthStageChanged(PlantInstance plant)
        {
            LogInfo($"Plant {plant.PlantID} advanced to {plant.CurrentGrowthStage}");
            _onPlantGrowthStageChanged?.Raise(plant);
        }
        
        private void OnPlantHealthChanged(PlantInstance plant)
        {
            if (_enableDetailedLogging)
                LogInfo($"Plant {plant.PlantID} health changed to {plant.CurrentHealth:F2}");
            
            _onPlantHealthChanged?.Raise(plant);
        }
        
        private void OnPlantDied(PlantInstance plant)
        {
            LogInfo($"Plant {plant.PlantID} died (Health: {plant.CurrentHealth:F2})");
            UnregisterPlant(plant.PlantID, PlantRemovalReason.Died);
        }
        
        private void InitializeDefaultGrowthCurve()
        {
            _defaultGrowthCurve = new AnimationCurve();
            _defaultGrowthCurve.AddKey(0f, 0f);      // Start
            _defaultGrowthCurve.AddKey(0.25f, 0.1f); // Slow initial growth
            _defaultGrowthCurve.AddKey(0.5f, 0.4f);  // Accelerating growth
            _defaultGrowthCurve.AddKey(0.75f, 0.8f); // Peak growth
            _defaultGrowthCurve.AddKey(1f, 1f);      // Mature
            
            LogInfo("Initialized default growth curve");
        }
        

    }
    
    /// <summary>
    /// Statistics about all plants managed by the PlantManager.
    /// </summary>
    [System.Serializable]
    public class PlantManagerStatistics
    {
        public int TotalPlants;
        public int[] PlantsByStage = new int[System.Enum.GetValues(typeof(PlantGrowthStage)).Length];
        public float AverageHealth;
        public float AverageStress;
        public int UnhealthyPlants;
        public int HighStressPlants;
    }
    
    public enum PlantRemovalReason
    {
        Harvested,
        Died,
        Removed,
        Other
    }
}