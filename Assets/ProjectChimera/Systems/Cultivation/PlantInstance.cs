using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Genetics;
using ProjectChimera.Data.Environment;
using System;
using System.Collections.Generic;

// Explicit aliases to resolve EnvironmentalConditions namespace conflicts
using EnvironmentalConditions = ProjectChimera.Data.Cultivation.EnvironmentalConditions;
using EnvironmentEnvironmentalConditions = ProjectChimera.Data.Environment.EnvironmentalConditions;

namespace ProjectChimera.Systems.Cultivation
{
    /// <summary>
    /// Represents an individual plant instance with genetic characteristics, growth state,
    /// environmental responses, and health status.
    /// </summary>
    public class PlantInstance : CultivationEntity
    {
        [Header("Plant Identity")]
        [SerializeField] private string _plantID;
        [SerializeField] private PlantStrainSO _strain;
        [SerializeField] private string _plantName;
        [SerializeField] private DateTime _plantedDate;
        [SerializeField] private DateTime _lastWatered;
        [SerializeField] private DateTime _lastFed;
        [SerializeField] private int _generationNumber = 1;
        
        [Header("Growth State")]
        [SerializeField] private PlantGrowthStage _currentGrowthStage = PlantGrowthStage.Seed;
        [SerializeField] private float _growthProgress = 0f; // 0-1 within current stage
        [SerializeField] private float _overallGrowthProgress = 0f; // 0-1 from seed to harvest
        [SerializeField] private int _daysSincePlanted = 0;
        [SerializeField] private Vector3 _plantSize = Vector3.one;
        
        [Header("Health and Condition")]
        [SerializeField] private float _currentHealth = 1f;
        [SerializeField] private float _maxHealth = 1f;
        [SerializeField] private float _stressLevel = 0f;
        [SerializeField] private float _diseaseResistance = 1f;
        [SerializeField] private float _waterLevel = 1f;
        [SerializeField] private float _nutrientLevel = 1f;
        [SerializeField] private bool _isActive = true;
        
        [Header("Environmental Response")]
        [SerializeField] private EnvironmentalConditions _currentEnvironment;
        [SerializeField] private List<ActiveStressor> _activeStressors = new List<ActiveStressor>();
        [SerializeField] private float _environmentalFitness = 1f;
        [SerializeField] private GxEResponseData _gxeResponse;
        
        [Header("Phenotypic Expression")]
        [SerializeField] private PhenotypicTraits _expressedTraits;
        [SerializeField] private float _yieldPotential = 1f;
        [SerializeField] private float _qualityPotential = 1f;
        [SerializeField] private CannabinoidProfile _currentCannabinoids;
        [SerializeField] private TerpeneProfile _currentTerpenes;
        
        // Events
        public event Action<PlantInstance> OnGrowthStageChanged;
        public event Action<PlantInstance> OnHealthChanged;
        public event Action<PlantInstance> OnPlantDied;
        public event Action<PlantInstance> OnEnvironmentChanged;
        
        // Private fields
        private PlantGrowthCalculator _growthCalculator;
        private PlantHealthSystem _healthSystem;
        private EnvironmentalResponseSystem _environmentalSystem;
        private Dictionary<PlantGrowthStage, float> _stageProgressThresholds;
        
        // Public Properties
        public string PlantID => _plantID;
        public PlantStrainSO Strain => _strain;
        public string PlantName => _plantName;
        public DateTime PlantedDate { get => _plantedDate; set => _plantedDate = value; }
        public DateTime LastWatered { get => _lastWatered; set => _lastWatered = value; }
        public DateTime LastFed { get => _lastFed; set => _lastFed = value; }
        public int GenerationNumber => _generationNumber;
        public PlantGrowthStage CurrentGrowthStage => _currentGrowthStage;
        public float GrowthProgress => _growthProgress;
        public float OverallGrowthProgress => _overallGrowthProgress;
        public int DaysSincePlanted => _daysSincePlanted;
        public Vector3 PlantSize => _plantSize;
        public float CurrentHealth => _currentHealth;
        public float Health => _currentHealth; // Alias for compatibility with other systems
        public float MaxHealth => _maxHealth;
        public float StressLevel => _stressLevel;
        public bool IsActive => _isActive;
        public EnvironmentalConditions CurrentEnvironment => _currentEnvironment;
        public float EnvironmentalFitness => _environmentalFitness;
        public PhenotypicTraits ExpressedTraits => _expressedTraits;
        public float YieldPotential => _yieldPotential;
        public float QualityPotential => _qualityPotential;
        
        // Additional properties for compatibility
        public string PlantId => _plantID;
        public PlantStrainSO PlantStrain { get => _strain; set => _strain = value; }
        public PlantGrowthStage CurrentStage { get => _currentGrowthStage; set => _currentGrowthStage = value; }
        public bool IsHarvestable => _currentGrowthStage == PlantGrowthStage.Harvest || _currentGrowthStage == PlantGrowthStage.Harvested;
        public float WaterLevel { get => _waterLevel; set => _waterLevel = value; }
        public float NutrientLevel { get => _nutrientLevel; set => _nutrientLevel = value; }
        public PlantStrainSO GeneticProfile { get => _strain; set => _strain = value; }
        public string StrainName => _strain != null ? _strain.StrainName : "Unknown";

        /// <summary>
        /// Make the plant sprout (transition from seed to germination)
        /// </summary>
        public void Sprout()
        {
            if (_currentGrowthStage == PlantGrowthStage.Seed)
            {
                _currentGrowthStage = PlantGrowthStage.Germination;
                _growthProgress = 0f;
                LogInfo($"Plant {PlantID} has sprouted!");
            }
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            if (string.IsNullOrEmpty(_plantID))
                _plantID = GenerateUniqueID();
            
            InitializeStageThresholds();
            InitializeSystems();
        }
        
        protected override void Start()
        {
            base.Start();
            
            if (_strain != null)
            {
                InitializeFromStrain();
            }
        }
        
        /// <summary>
        /// Creates a new plant instance from a strain definition.
        /// </summary>
        public static PlantInstance CreateFromStrain(PlantStrainSO strain, Vector3 position, Transform parent = null)
        {
            var plantObject = new GameObject($"Plant_{strain.StrainName}_{GenerateShortID()}");
            plantObject.transform.position = position;
            
            if (parent != null)
                plantObject.transform.SetParent(parent);
            
            var plantInstance = plantObject.AddComponent<PlantInstance>();
            plantInstance.InitializeFromStrain(strain);
            
            return plantInstance;
        }

        /// <summary>
        /// Creates a wrapper PlantInstance from a SpeedTree plant instance.
        /// NOTE: SpeedTree integration commented out until proper assembly references are configured
        /// </summary>
        /*
        public static PlantInstance CreateFromSpeedTree(ProjectChimera.Systems.SpeedTree.SpeedTreePlantInstance speedTreeInstance)
        {
            if (speedTreeInstance == null)
            {
                Debug.LogError("Cannot create PlantInstance from null SpeedTree instance");
                return null;
            }

            var plantObject = new GameObject($"PlantWrapper_{speedTreeInstance.PlantId}");
            plantObject.transform.position = speedTreeInstance.transform.position;
            plantObject.transform.rotation = speedTreeInstance.transform.rotation;
            plantObject.transform.SetParent(speedTreeInstance.transform.parent);
            
            var plantInstance = plantObject.AddComponent<PlantInstance>();
            plantInstance.InitializeFromSpeedTree(speedTreeInstance);
            
            return plantInstance;
        }
        */
        
        /// <summary>
        /// Initializes the plant from a strain definition.
        /// </summary>
        public void InitializeFromStrain(PlantStrainSO strain = null)
        {
            if (strain != null)
                _strain = strain;
            
            if (_strain == null)
            {
                Debug.LogError($"Cannot initialize plant {_plantID}: no strain assigned");
                return;
            }
            
            _plantName = $"{_strain.StrainName}_{GenerateShortID()}";
            _plantedDate = DateTime.Now;
            _currentGrowthStage = PlantGrowthStage.Seed;
            _growthProgress = 0f;
            _overallGrowthProgress = 0f;
            _daysSincePlanted = 0;
            
            // Initialize health based on strain genetics
            _maxHealth = 1.0f; // Base health - could be modified by strain genetics in future
            _currentHealth = _maxHealth;
            _diseaseResistance = 1.0f + _strain.DiseaseResistanceModifier;
            
            // Initialize phenotypic traits from genetics
            InitializePhenotypicTraits();
            
            // Initialize growth systems
            _growthCalculator?.Initialize(_strain, _expressedTraits);
            _healthSystem?.Initialize(_strain, _diseaseResistance);
            _environmentalSystem?.Initialize(_strain);
            
            LogInfo($"Initialized plant {_plantID} from strain {_strain.StrainName}");
        }

        /// <summary>
        /// Initializes the plant from a SpeedTree plant instance.
        /// NOTE: SpeedTree integration commented out until proper assembly references are configured
        /// </summary>
        /*
        public void InitializeFromSpeedTree(ProjectChimera.Systems.SpeedTree.SpeedTreePlantInstance speedTreeInstance)
        {
            if (speedTreeInstance == null)
            {
                Debug.LogError($"Cannot initialize plant {_plantID}: null SpeedTree instance");
                return;
            }

            _plantID = speedTreeInstance.PlantId;
            _strain = speedTreeInstance.PlantStrain;
            _plantName = speedTreeInstance.PlantId;
            _plantedDate = speedTreeInstance.PlantedDate;
            _currentGrowthStage = (PlantGrowthStage)speedTreeInstance.CurrentGrowthStage;
            _growthProgress = speedTreeInstance.MaturityLevel;
            _overallGrowthProgress = speedTreeInstance.MaturityLevel;
            _daysSincePlanted = (int)(DateTime.Now - speedTreeInstance.PlantedDate).TotalDays;

            // Map SpeedTree health values
            _maxHealth = 1.0f;
            _currentHealth = speedTreeInstance.Health / 100f; // Convert from 0-100 to 0-1
            _stressLevel = speedTreeInstance.StressLevel / 100f; // Convert from 0-100 to 0-1
            _diseaseResistance = speedTreeInstance.DiseaseResistance / 100f; // Convert from 0-100 to 0-1

            // Initialize systems if strain is available
            if (_strain != null)
            {
                InitializePhenotypicTraits();
                _growthCalculator?.Initialize(_strain, _expressedTraits);
                _healthSystem?.Initialize(_strain, _diseaseResistance);
                _environmentalSystem?.Initialize(_strain);
            }

            LogInfo($"Initialized wrapper plant {_plantID} from SpeedTree instance");
        }
        */
        
        /// <summary>
        /// Updates the plant's growth, health, and environmental responses.
        /// </summary>
        public void UpdatePlant(float deltaTime, float globalGrowthModifier = 1f)
        {
            if (!_isActive || _currentHealth <= 0f)
                return;
            
            // Update environmental responses
            _environmentalSystem?.UpdateEnvironmentalResponse(_currentEnvironment, deltaTime);
            _environmentalFitness = _environmentalSystem?.GetEnvironmentalFitness() ?? 1f;
            
            // Update health system
            _healthSystem?.UpdateHealth(deltaTime, _activeStressors, _environmentalFitness);
            UpdateHealthValues();
            
            // Update growth if plant is healthy enough
            if (_currentHealth > 0.1f)
            {
                UpdateGrowth(deltaTime, globalGrowthModifier);
            }
            
            // Update phenotypic expression based on environment
            UpdatePhenotypicExpression();
            
            // Update visual representation
            UpdateVisualRepresentation();
            
            // Check for death
            if (_currentHealth <= 0f && _isActive)
            {
                HandlePlantDeath();
            }
        }
        
        /// <summary>
        /// Updates environmental conditions affecting this plant.
        /// </summary>
        public void UpdateEnvironmentalConditions(EnvironmentalConditions newConditions)
        {
            var previousConditions = _currentEnvironment;
            _currentEnvironment = newConditions;
            
            // Process environmental change through the environmental system
            _environmentalSystem?.ProcessEnvironmentalChange(previousConditions, newConditions);
            
            OnEnvironmentChanged?.Invoke(this);
        }
        
        /// <summary>
        /// Updates environmental adaptation for this plant based on current conditions.
        /// </summary>
        public void UpdateEnvironmentalAdaptation(EnvironmentalConditions conditions)
        {
            // Update environmental conditions first
            UpdateEnvironmentalConditions(conditions);
            
            // Calculate adaptation progress based on environmental fitness
            var adaptationRate = 0.01f; // Base adaptation rate
            var environmentalStress = 1f - _environmentalFitness;
            
            // Increase adaptation rate under stress
            if (environmentalStress > 0.3f)
            {
                adaptationRate *= (1f + environmentalStress);
            }
            
            // Apply adaptation to environmental system
            _environmentalSystem?.ProcessAdaptation(conditions, adaptationRate);
            
            LogInfo($"Updated environmental adaptation for plant {_plantID} (fitness: {_environmentalFitness:F2})");
        }
        
        /// <summary>
        /// Applies stress to the plant.
        /// </summary>
        public bool ApplyStress(EnvironmentalStressSO stressSource, float intensity)
        {
            if (stressSource == null || intensity <= 0f)
                return false;
            
            var stressor = new ActiveStressor
            {
                StressSource = stressSource,
                Intensity = intensity,
                StartTime = Time.time,
                IsActive = true
            };
            
            _activeStressors.Add(stressor);
            UpdateStressLevel();
            
            LogInfo($"Applied stress '{stressSource.StressName}' to plant {_plantID} (Intensity: {intensity:F2})");
            return true;
        }
        
        /// <summary>
        /// Removes a specific stress source.
        /// </summary>
        public void RemoveStress(EnvironmentalStressSO stressSource)
        {
            _activeStressors.RemoveAll(s => s.StressSource == stressSource);
            UpdateStressLevel();
        }
        
        /// <summary>
        /// Checks if the plant has any active stressors.
        /// </summary>
        public bool HasActiveStressors()
        {
            return _activeStressors.Count > 0;
        }
        
        /// <summary>
        /// Harvests the plant and returns harvest results.
        /// </summary>
        public HarvestResults Harvest()
        {
            if (_currentGrowthStage != PlantGrowthStage.Harvest)
            {
                LogWarning($"Plant {_plantID} is not ready for harvest");
                return null;
            }
            
            var results = _growthCalculator?.CalculateHarvestResults(_currentHealth, _qualityPotential, _expressedTraits);
            _isActive = false;
            
            LogInfo($"Harvested plant {_plantID}: {results?.TotalYieldGrams ?? 0}g");
            return results;
        }
        
        /// <summary>
        /// Advances the plant to the next growth stage if conditions are met.
        /// </summary>
        public bool AdvanceGrowthStage()
        {
            if (_currentGrowthStage == PlantGrowthStage.Harvest)
                return false;
            
            var nextStage = (PlantGrowthStage)((int)_currentGrowthStage + 1);
            
            if (CanAdvanceToStage(nextStage))
            {
                _currentGrowthStage = nextStage;
                _growthProgress = 0f;
                
                OnGrowthStageChanged?.Invoke(this);
                LogInfo($"Plant {_plantID} advanced to {_currentGrowthStage}");
                
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Calculates breeding value for genetic algorithms.
        /// </summary>
        public float CalculateBreedingValue()
        {
            float value = 0f;
            
            // Base on expressed traits
            value += _expressedTraits.YieldMultiplier * 0.3f;
            value += _expressedTraits.QualityMultiplier * 0.3f;
            value += _expressedTraits.PotencyMultiplier * 0.2f;
            value += _maxHealth * 0.1f;
            value += _diseaseResistance * 0.1f;
            
            return Mathf.Clamp01(value);
        }
        
        private void InitializeStageThresholds()
        {
            _stageProgressThresholds = new Dictionary<PlantGrowthStage, float>
            {
                { PlantGrowthStage.Seed, 0.05f },
                { PlantGrowthStage.Germination, 0.1f },
                { PlantGrowthStage.Seedling, 0.2f },
                { PlantGrowthStage.Vegetative, 0.6f },
                { PlantGrowthStage.Flowering, 0.9f },
                { PlantGrowthStage.Harvest, 1.0f }
            };
        }
        
        private void InitializeSystems()
        {
            _growthCalculator = new PlantGrowthCalculator();
            _healthSystem = new PlantHealthSystem();
            _environmentalSystem = new EnvironmentalResponseSystem();
        }
        
        private void InitializePhenotypicTraits()
        {
            if (_strain == null)
                return;
            
            _expressedTraits = new PhenotypicTraits
            {
                YieldMultiplier = (_strain.BaseYieldGrams / 100f) + UnityEngine.Random.Range(-0.1f, 0.1f), // Convert grams to multiplier
                QualityMultiplier = _strain.BaseQualityModifier + UnityEngine.Random.Range(-0.1f, 0.1f),
                PotencyMultiplier = _strain.BasePotencyModifier + UnityEngine.Random.Range(-0.1f, 0.1f),
                FloweringTime = _strain.BaseFloweringTime + UnityEngine.Random.Range(-5, 5),
                PlantHeight = _strain.BaseHeight + UnityEngine.Random.Range(-0.2f, 0.2f)
            };
            
            _yieldPotential = _expressedTraits.YieldMultiplier;
            _qualityPotential = _expressedTraits.QualityMultiplier;
        }
        
        private void UpdateGrowth(float deltaTime, float globalGrowthModifier)
        {
            var growthRate = _growthCalculator?.CalculateGrowthRate(
                _currentGrowthStage, 
                _environmentalFitness, 
                _currentHealth, 
                globalGrowthModifier
            ) ?? 0f;
            
            var timeManager = GameManager.Instance?.GetManager<TimeManager>();
            var gameTime = timeManager?.GetScaledDeltaTime() ?? deltaTime;
            
            _growthProgress += growthRate * gameTime;
            _overallGrowthProgress = CalculateOverallProgress();
            
            // Update plant size based on growth
            UpdatePlantSize();
            
            // Check for stage advancement
            if (_growthProgress >= 1f)
            {
                AdvanceGrowthStage();
            }
        }
        
        private void UpdateHealthValues()
        {
            var previousHealth = _currentHealth;
            
            _currentHealth = _healthSystem?.GetCurrentHealth() ?? _currentHealth;
            _stressLevel = _healthSystem?.GetStressLevel() ?? _stressLevel;
            
            if (Mathf.Abs(_currentHealth - previousHealth) > 0.01f)
            {
                OnHealthChanged?.Invoke(this);
            }
        }
        
        private void UpdateStressLevel()
        {
            _stressLevel = 0f;
            foreach (var stressor in _activeStressors)
            {
                if (stressor.IsActive)
                    _stressLevel += stressor.Intensity * stressor.StressSource.StressMultiplier;
            }
            
            _stressLevel = Mathf.Clamp01(_stressLevel);
        }
        
        private void UpdatePhenotypicExpression()
        {
            // Environmental factors affect trait expression
            if (_environmentalFitness < 0.7f)
            {
                _yieldPotential = _expressedTraits.YieldMultiplier * _environmentalFitness;
                _qualityPotential = _expressedTraits.QualityMultiplier * Mathf.Sqrt(_environmentalFitness);
            }
            else
            {
                _yieldPotential = _expressedTraits.YieldMultiplier;
                _qualityPotential = _expressedTraits.QualityMultiplier;
            }
        }
        
        private void UpdatePlantSize()
        {
            var baseSize = _strain?.BaseHeight ?? 1f;
            var sizeMultiplier = _expressedTraits.PlantHeight * _overallGrowthProgress;
            
            _plantSize = Vector3.one * (baseSize * sizeMultiplier);
            transform.localScale = _plantSize;
        }
        
        private void UpdateVisualRepresentation()
        {
            // Update visual elements based on growth stage and health
            // This would interface with rendering components
        }
        
        private float CalculateOverallProgress()
        {
            float stageWeight = _stageProgressThresholds[_currentGrowthStage];
            float previousStagesWeight = 0f;
            
            foreach (var stage in _stageProgressThresholds)
            {
                if ((int)stage.Key < (int)_currentGrowthStage)
                    previousStagesWeight += stage.Value;
            }
            
            return previousStagesWeight + (stageWeight * _growthProgress);
        }
        
        private bool CanAdvanceToStage(PlantGrowthStage targetStage)
        {
            // Add stage-specific advancement requirements here
            return _currentHealth > 0.3f && _growthProgress >= 1f;
        }
        
        private void HandlePlantDeath()
        {
            _isActive = false;
            LogInfo($"Plant {_plantID} died (Health: {_currentHealth:F2})");
            OnPlantDied?.Invoke(this);
        }
        
        private static string GenerateUniqueID()
        {
            return $"PLANT_{DateTime.Now.Ticks:X}_{UnityEngine.Random.Range(1000, 9999)}";
        }
        
        private static string GenerateShortID()
        {
            return UnityEngine.Random.Range(1000, 9999).ToString();
        }
    }
    
    /// <summary>
    /// Represents an active stressor affecting the plant.
    /// </summary>
    [System.Serializable]
    public class ActiveStressor
    {
        public EnvironmentalStressSO StressSource;
        public float Intensity;
        public float StartTime;
        public bool IsActive;
        public float Duration => Time.time - StartTime;
    }
    
    /// <summary>
    /// Phenotypic traits expressed by the plant.
    /// </summary>
    [System.Serializable]
    public class PhenotypicTraits
    {
        public float YieldMultiplier = 1f;
        public float QualityMultiplier = 1f;
        public float PotencyMultiplier = 1f;
        public int FloweringTime = 60; // days
        public float PlantHeight = 1f; // meters
        public float DiseaseResistance = 1f;
        public float HeatTolerance = 1f;
        public float ColdTolerance = 1f;
        public float DroughtTolerance = 1f;
    }
    
    /// <summary>
    /// GxE response data for environmental interactions.
    /// </summary>
    [System.Serializable]
    public class GxEResponseData
    {
        public float TemperatureResponse = 1f;
        public float HumidityResponse = 1f;
        public float LightResponse = 1f;
        public float NutrientResponse = 1f;
        public float CO2Response = 1f;
    }
    
    /// <summary>
    /// Results from harvesting a plant.
    /// </summary>
    [System.Serializable]
    public class HarvestResults
    {
        public string PlantID;
        public float TotalYieldGrams;
        public float QualityScore;
        public CannabinoidProfile Cannabinoids;
        public TerpeneProfile Terpenes;
        public int FloweringDays;
        public float FinalHealth;
        public DateTime HarvestDate;
    }
}