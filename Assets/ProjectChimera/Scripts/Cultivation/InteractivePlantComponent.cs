using UnityEngine;
using ProjectChimera.Core;
using ProjectChimera.Data.Genetics;
using ProjectChimera.Data.Environment;
using ProjectChimera.Systems.Cultivation;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ProjectChimera.Cultivation
{
    /// <summary>
    /// Advanced interactive plant component with detailed growth mechanics,
    /// environmental interactions, and visual feedback systems.
    /// </summary>
    public class InteractivePlantComponent : MonoBehaviour
    {
        [Header("Plant Configuration")]
        [SerializeField] private PlantStrainSO _strainData;
        [SerializeField] private bool _enableVisualGrowth = true;
        [SerializeField] private bool _enableEnvironmentalEffects = true;
        [SerializeField] private bool _enableInteractionFeedback = true;
        
        [Header("Growth Stages")]
        [SerializeField] private Transform _seedStage;
        [SerializeField] private Transform _sproutStage;
        [SerializeField] private Transform _vegetativeStage;
        [SerializeField] private Transform _floweringStage;
        [SerializeField] private Transform _harvestStage;
        
        [Header("Visual Effects")]
        [SerializeField] private ParticleSystem _growthParticles;
        [SerializeField] private ParticleSystem _healthEffects;
        [SerializeField] private ParticleSystem _wateringEffects;
        [SerializeField] private ParticleSystem _nutrientEffects;
        [SerializeField] private Light _plantGlow;
        
        [Header("Interactive Elements")]
        [SerializeField] private Collider _interactionCollider;
        [SerializeField] private Renderer _plantRenderer;
        [SerializeField] private AudioSource _plantAudioSource;
        [SerializeField] private Canvas _infoCanvas;
        [SerializeField] private TMPro.TextMeshProUGUI _statusText;
        
        [Header("Environmental Sensors")]
        [SerializeField] private float _temperatureSensorRange = 5f;
        [SerializeField] private float _humiditySensorRange = 5f;
        [SerializeField] private float _lightSensorRange = 10f;
        [SerializeField] private LayerMask _environmentalLayers = -1;
        
        // Plant State
        private PlantInstance _plantInstance;
        private PlantGrowthStage _currentStage = PlantGrowthStage.Seed;
        private PlantGrowthStage _previousStage = PlantGrowthStage.Seed;
        private float _growthProgress = 0f;
        private float _health = 100f;
        private float _waterLevel = 100f;
        private float _nutrientLevel = 100f;
        private bool _isHarvestable = false;
        private bool _isSelected = false;
        private bool _isHovered = false;
        
        // Environmental Data
        private EnvironmentalConditions _currentConditions;
        private float _lastEnvironmentalUpdate = 0f;
        private float _environmentalUpdateInterval = 1f;
        
        // Growth Mechanics
        private float _lastGrowthUpdate = 0f;
        private float _growthUpdateInterval = 0.1f;
        private AnimationCurve _growthCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        private Vector3 _originalScale;
        private Vector3 _targetScale;
        
        // Resource Consumption
        private float _waterConsumptionRate = 1f;
        private float _nutrientConsumptionRate = 0.8f;
        private float _lastResourceUpdate = 0f;
        private float _resourceUpdateInterval = 5f;
        
        // Visual Feedback
        private Material _originalMaterial;
        private Color _originalColor;
        private Color _healthyColor = Color.green;
        private Color _unhealthyColor = Color.red;
        private Color _selectedColor = Color.yellow;
        private Color _hoveredColor = Color.cyan;
        
        // Audio Clips
        [Header("Audio")]
        [SerializeField] private AudioClip _growthSFX;
        [SerializeField] private AudioClip _wateringSFX;
        [SerializeField] private AudioClip _harvestSFX;
        [SerializeField] private AudioClip _healthDeclineSFX;
        
        // Events
        public System.Action<InteractivePlantComponent> OnPlantClicked;
        public System.Action<InteractivePlantComponent> OnPlantHovered;
        public System.Action<InteractivePlantComponent> OnPlantUnhovered;
        public System.Action<InteractivePlantComponent, PlantGrowthStage> OnStageChanged;
        public System.Action<InteractivePlantComponent, float> OnHealthChanged;
        public System.Action<InteractivePlantComponent> OnPlantHarvestReady;
        public System.Action<InteractivePlantComponent> OnPlantDied;
        
        // Properties
        public PlantInstance PlantData => _plantInstance;
        public PlantGrowthStage CurrentStage => _currentStage;
        public float GrowthProgress => _growthProgress;
        public float Health => _health;
        public float WaterLevel => _waterLevel;
        public float NutrientLevel => _nutrientLevel;
        public bool IsHarvestable => _isHarvestable;
        public bool IsSelected => _isSelected;
        public EnvironmentalConditions CurrentConditions => _currentConditions;
        
        private void Awake()
        {
            InitializePlant();
        }
        
        private void Start()
        {
            SetupVisualComponents();
            SetupInteractionComponents();
            StartGrowthCycle();
        }
        
        private void Update()
        {
            float currentTime = Time.time;
            
            // Update environmental conditions
            if (currentTime - _lastEnvironmentalUpdate >= _environmentalUpdateInterval)
            {
                UpdateEnvironmentalConditions();
                _lastEnvironmentalUpdate = currentTime;
            }
            
            // Update growth progress
            if (currentTime - _lastGrowthUpdate >= _growthUpdateInterval)
            {
                UpdateGrowthProgress();
                _lastGrowthUpdate = currentTime;
            }
            
            // Update resource consumption
            if (currentTime - _lastResourceUpdate >= _resourceUpdateInterval)
            {
                UpdateResourceConsumption();
                _lastResourceUpdate = currentTime;
            }
            
            // Update visual feedback
            UpdateVisualFeedback();
            
            // Update interaction feedback
            HandleInteractionInput();
        }
        
        #region Initialization
        
        private void InitializePlant()
        {
            // Create plant instance data
            _plantInstance = new PlantInstance();
            
            if (_strainData != null)
            {
                _plantInstance.StrainName = _strainData.StrainName;
                _plantInstance.GeneticProfile = _strainData.GeneticProfile;
            }
            else
            {
                _plantInstance.StrainName = "Unknown Strain";
            }
            
            _plantInstance.PlantId = Guid.NewGuid().ToString();
            _plantInstance.PlantedDate = DateTime.Now;
            _plantInstance.CurrentStage = PlantGrowthStage.Seed;
            _plantInstance.Health = 100f;
            _plantInstance.WaterLevel = 100f;
            _plantInstance.NutrientLevel = 100f;
            
            // Store original scale
            _originalScale = transform.localScale;
            _targetScale = _originalScale;
            
            // Initialize environmental conditions
            _currentConditions = new EnvironmentalConditions
            {
                Temperature = 24f,
                Humidity = 60f,
                CO2Level = 400f,
                LightIntensity = 200f,
                AirFlow = 0.5f
            };
        }
        
        private void SetupVisualComponents()
        {
            // Setup renderer and materials
            if (_plantRenderer == null)
                _plantRenderer = GetComponentInChildren<Renderer>();
            
            if (_plantRenderer != null)
            {
                _originalMaterial = _plantRenderer.material;
                _originalColor = _originalMaterial.color;
            }
            
            // Setup growth stage objects
            SetupGrowthStages();
            
            // Setup particle systems
            SetupParticleSystems();
            
            // Setup plant glow
            if (_plantGlow != null)
            {
                _plantGlow.enabled = false;
                _plantGlow.intensity = 0.5f;
                _plantGlow.range = 2f;
                _plantGlow.color = _healthyColor;
            }
            
            // Setup info canvas
            if (_infoCanvas != null)
            {
                _infoCanvas.worldCamera = Camera.main;
                _infoCanvas.gameObject.SetActive(false);
            }
        }
        
        private void SetupGrowthStages()
        {
            // Hide all growth stages initially
            if (_seedStage != null) _seedStage.gameObject.SetActive(_currentStage == PlantGrowthStage.Seed);
            if (_sproutStage != null) _sproutStage.gameObject.SetActive(_currentStage == PlantGrowthStage.Sprout);
            if (_vegetativeStage != null) _vegetativeStage.gameObject.SetActive(_currentStage == PlantGrowthStage.Vegetative);
            if (_floweringStage != null) _floweringStage.gameObject.SetActive(_currentStage == PlantGrowthStage.Flowering);
            if (_harvestStage != null) _harvestStage.gameObject.SetActive(_currentStage == PlantGrowthStage.Harvest);
        }
        
        private void SetupParticleSystems()
        {
            if (_growthParticles != null)
            {
                var main = _growthParticles.main;
                main.startColor = Color.green;
                main.startLifetime = 2f;
                main.startSpeed = 1f;
                _growthParticles.Stop();
            }
            
            if (_healthEffects != null)
            {
                var main = _healthEffects.main;
                main.startColor = _healthyColor;
                _healthEffects.Stop();
            }
            
            if (_wateringEffects != null)
            {
                var main = _wateringEffects.main;
                main.startColor = Color.blue;
                _wateringEffects.Stop();
            }
            
            if (_nutrientEffects != null)
            {
                var main = _nutrientEffects.main;
                main.startColor = Color.yellow;
                _nutrientEffects.Stop();
            }
        }
        
        private void SetupInteractionComponents()
        {
            // Setup interaction collider
            if (_interactionCollider == null)
                _interactionCollider = GetComponent<Collider>();
            
            if (_interactionCollider == null)
            {
                _interactionCollider = gameObject.AddComponent<SphereCollider>();
                ((SphereCollider)_interactionCollider).radius = 1.5f;
            }
            
            // Setup audio source
            if (_plantAudioSource == null)
                _plantAudioSource = GetComponent<AudioSource>();
            
            if (_plantAudioSource == null)
                _plantAudioSource = gameObject.AddComponent<AudioSource>();
            
            _plantAudioSource.spatialBlend = 1f; // 3D sound
            _plantAudioSource.volume = 0.5f;
            _plantAudioSource.playOnAwake = false;
        }
        
        #endregion
        
        #region Growth Mechanics
        
        private void StartGrowthCycle()
        {
            StartCoroutine(GrowthCycleCoroutine());
        }
        
        private IEnumerator GrowthCycleCoroutine()
        {
            while (_currentStage != PlantGrowthStage.Harvest && _health > 0f)
            {
                yield return new WaitForSeconds(1f);
                
                if (_health > 50f && _waterLevel > 20f && _nutrientLevel > 20f)
                {
                    float growthRate = CalculateGrowthRate();
                    _growthProgress += growthRate * Time.deltaTime;
                    
                    if (_growthProgress >= 1f)
                    {
                        AdvanceGrowthStage();
                    }
                }
            }
        }
        
        private float CalculateGrowthRate()
        {
            float baseRate = 0.01f; // 1% per second under perfect conditions
            
            // Environmental modifiers
            float tempModifier = CalculateTemperatureModifier();
            float humidityModifier = CalculateHumidityModifier();
            float lightModifier = CalculateLightModifier();
            float healthModifier = _health / 100f;
            float waterModifier = Mathf.Clamp01(_waterLevel / 100f);
            float nutrientModifier = Mathf.Clamp01(_nutrientLevel / 100f);
            
            return baseRate * tempModifier * humidityModifier * lightModifier * 
                   healthModifier * waterModifier * nutrientModifier;
        }
        
        private float CalculateTemperatureModifier()
        {
            float optimalTemp = _strainData?.OptimalTemperature ?? 24f;
            float tempDifference = Mathf.Abs(_currentConditions.Temperature - optimalTemp);
            return Mathf.Clamp01(1f - (tempDifference / 10f));
        }
        
        private float CalculateHumidityModifier()
        {
            float optimalHumidity = _strainData?.OptimalHumidity ?? 60f;
            float humidityDifference = Mathf.Abs(_currentConditions.Humidity - optimalHumidity);
            return Mathf.Clamp01(1f - (humidityDifference / 30f));
        }
        
        private float CalculateLightModifier()
        {
            float minLight = 100f;
            float maxLight = 800f;
            float normalizedLight = Mathf.Clamp01(_currentConditions.LightIntensity / maxLight);
            return normalizedLight;
        }
        
        private void AdvanceGrowthStage()
        {
            _previousStage = _currentStage;
            
            switch (_currentStage)
            {
                case PlantGrowthStage.Seed:
                    _currentStage = PlantGrowthStage.Sprout;
                    break;
                case PlantGrowthStage.Sprout:
                    _currentStage = PlantGrowthStage.Vegetative;
                    break;
                case PlantGrowthStage.Vegetative:
                    _currentStage = PlantGrowthStage.Flowering;
                    break;
                case PlantGrowthStage.Flowering:
                    _currentStage = PlantGrowthStage.Harvest;
                    _isHarvestable = true;
                    OnPlantHarvestReady?.Invoke(this);
                    break;
            }
            
            _growthProgress = 0f;
            _plantInstance.CurrentStage = _currentStage;
            
            // Visual stage transition
            TransitionToStage(_currentStage);
            
            // Play growth effects
            PlayGrowthEffects();
            
            OnStageChanged?.Invoke(this, _currentStage);
            
            Debug.Log($"Plant {_plantInstance.StrainName} advanced to {_currentStage} stage");
        }
        
        private void TransitionToStage(PlantGrowthStage newStage)
        {
            // Hide all stages
            if (_seedStage != null) _seedStage.gameObject.SetActive(false);
            if (_sproutStage != null) _sproutStage.gameObject.SetActive(false);
            if (_vegetativeStage != null) _vegetativeStage.gameObject.SetActive(false);
            if (_floweringStage != null) _floweringStage.gameObject.SetActive(false);
            if (_harvestStage != null) _harvestStage.gameObject.SetActive(false);
            
            // Show current stage
            switch (newStage)
            {
                case PlantGrowthStage.Seed:
                    if (_seedStage != null) _seedStage.gameObject.SetActive(true);
                    _targetScale = _originalScale * 0.3f;
                    break;
                case PlantGrowthStage.Sprout:
                    if (_sproutStage != null) _sproutStage.gameObject.SetActive(true);
                    _targetScale = _originalScale * 0.5f;
                    break;
                case PlantGrowthStage.Vegetative:
                    if (_vegetativeStage != null) _vegetativeStage.gameObject.SetActive(true);
                    _targetScale = _originalScale * 0.8f;
                    break;
                case PlantGrowthStage.Flowering:
                    if (_floweringStage != null) _floweringStage.gameObject.SetActive(true);
                    _targetScale = _originalScale;
                    break;
                case PlantGrowthStage.Harvest:
                    if (_harvestStage != null) _harvestStage.gameObject.SetActive(true);
                    _targetScale = _originalScale * 1.2f;
                    break;
            }
            
            // Animate scale transition
            StartCoroutine(AnimateScaleTransition());
        }
        
        private IEnumerator AnimateScaleTransition()
        {
            Vector3 startScale = transform.localScale;
            float duration = 1f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t = _growthCurve.Evaluate(t);
                
                transform.localScale = Vector3.Lerp(startScale, _targetScale, t);
                yield return null;
            }
            
            transform.localScale = _targetScale;
        }
        
        #endregion
        
        #region Environmental Systems
        
        private void UpdateEnvironmentalConditions()
        {
            // Sample environmental conditions from nearby systems
            SampleTemperature();
            SampleHumidity();
            SampleLighting();
            SampleAirFlow();
            
            // Update plant health based on conditions
            UpdateHealthFromEnvironment();
        }
        
        private void SampleTemperature()
        {
            var hvacSystems = FindObjectsOfType<HVACController>();
            float nearestTemp = 24f; // Default room temperature
            float nearestDistance = float.MaxValue;
            
            foreach (var hvac in hvacSystems)
            {
                float distance = Vector3.Distance(transform.position, hvac.transform.position);
                if (distance < _temperatureSensorRange && distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTemp = hvac.CurrentTemperature;
                }
            }
            
            _currentConditions.Temperature = nearestTemp;
        }
        
        private void SampleHumidity()
        {
            var hvacSystems = FindObjectsOfType<HVACController>();
            float nearestHumidity = 60f; // Default humidity
            float nearestDistance = float.MaxValue;
            
            foreach (var hvac in hvacSystems)
            {
                float distance = Vector3.Distance(transform.position, hvac.transform.position);
                if (distance < _humiditySensorRange && distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestHumidity = hvac.CurrentHumidity;
                }
            }
            
            _currentConditions.Humidity = nearestHumidity;
        }
        
        private void SampleLighting()
        {
            var lightControllers = FindObjectsOfType<LightingController>();
            float totalIntensity = 0f;
            int lightCount = 0;
            
            foreach (var lightController in lightControllers)
            {
                float distance = Vector3.Distance(transform.position, lightController.transform.position);
                if (distance < _lightSensorRange)
                {
                    float falloff = 1f - (distance / _lightSensorRange);
                    totalIntensity += lightController.CurrentIntensity * falloff;
                    lightCount++;
                }
            }
            
            if (lightCount > 0)
            {
                _currentConditions.LightIntensity = totalIntensity;
            }
            else
            {
                // Ambient lighting
                _currentConditions.LightIntensity = 50f;
            }
        }
        
        private void SampleAirFlow()
        {
            var ventilationSystems = FindObjectsOfType<VentilationController>();
            float airFlow = 0.3f; // Default air flow
            
            foreach (var vent in ventilationSystems)
            {
                float distance = Vector3.Distance(transform.position, vent.transform.position);
                if (distance < 5f)
                {
                    airFlow = Mathf.Max(airFlow, vent.AirFlowRate);
                }
            }
            
            _currentConditions.AirFlow = airFlow;
        }
        
        private void UpdateHealthFromEnvironment()
        {
            float healthChange = 0f;
            
            // Temperature stress
            float tempStress = Mathf.Abs(_currentConditions.Temperature - 24f);
            if (tempStress > 5f)
            {
                healthChange -= tempStress * 0.1f * Time.deltaTime;
            }
            
            // Humidity stress
            float humidityStress = Mathf.Abs(_currentConditions.Humidity - 60f);
            if (humidityStress > 20f)
            {
                healthChange -= humidityStress * 0.05f * Time.deltaTime;
            }
            
            // Light stress
            if (_currentConditions.LightIntensity < 100f)
            {
                healthChange -= 0.5f * Time.deltaTime;
            }
            
            // Update health
            if (healthChange != 0f)
            {
                ModifyHealth(healthChange);
            }
        }
        
        #endregion
        
        #region Resource Management
        
        private void UpdateResourceConsumption()
        {
            // Water consumption based on stage and environmental conditions
            float waterConsumption = _waterConsumptionRate * GetStageConsumptionMultiplier();
            
            // Temperature affects water consumption
            if (_currentConditions.Temperature > 26f)
            {
                waterConsumption *= 1.5f;
            }
            
            // Humidity affects water consumption
            if (_currentConditions.Humidity < 50f)
            {
                waterConsumption *= 1.3f;
            }
            
            ModifyWaterLevel(-waterConsumption);
            
            // Nutrient consumption
            float nutrientConsumption = _nutrientConsumptionRate * GetStageConsumptionMultiplier();
            ModifyNutrientLevel(-nutrientConsumption);
            
            // Health effects from resource deficiency
            if (_waterLevel < 20f)
            {
                ModifyHealth(-2f * Time.deltaTime);
            }
            
            if (_nutrientLevel < 20f)
            {
                ModifyHealth(-1f * Time.deltaTime);
            }
        }
        
        private float GetStageConsumptionMultiplier()
        {
            return _currentStage switch
            {
                PlantGrowthStage.Seed => 0.2f,
                PlantGrowthStage.Sprout => 0.5f,
                PlantGrowthStage.Vegetative => 1.0f,
                PlantGrowthStage.Flowering => 1.5f,
                PlantGrowthStage.Harvest => 0.3f,
                _ => 1.0f
            };
        }
        
        public void ModifyHealth(float amount)
        {
            float oldHealth = _health;
            _health = Mathf.Clamp(_health + amount, 0f, 100f);
            _plantInstance.Health = _health;
            
            if (_health != oldHealth)
            {
                OnHealthChanged?.Invoke(this, _health);
                
                if (_health <= 0f)
                {
                    OnPlantDied?.Invoke(this);
                }
            }
        }
        
        public void ModifyWaterLevel(float amount)
        {
            _waterLevel = Mathf.Clamp(_waterLevel + amount, 0f, 100f);
            _plantInstance.WaterLevel = _waterLevel;
            
            if (amount > 0f && _wateringEffects != null)
            {
                _wateringEffects.Play();
                PlayAudioClip(_wateringSFX);
            }
        }
        
        public void ModifyNutrientLevel(float amount)
        {
            _nutrientLevel = Mathf.Clamp(_nutrientLevel + amount, 0f, 100f);
            _plantInstance.NutrientLevel = _nutrientLevel;
            
            if (amount > 0f && _nutrientEffects != null)
            {
                _nutrientEffects.Play();
            }
        }
        
        #endregion
        
        #region Visual Feedback
        
        private void UpdateVisualFeedback()
        {
            UpdateMaterialColor();
            UpdateParticleEffects();
            UpdateGlowEffect();
            UpdateInfoDisplay();
        }
        
        private void UpdateMaterialColor()
        {
            if (_plantRenderer == null || _originalMaterial == null) return;
            
            Color targetColor = _originalColor;
            
            if (_isSelected)
            {
                targetColor = Color.Lerp(targetColor, _selectedColor, 0.5f);
            }
            else if (_isHovered)
            {
                targetColor = Color.Lerp(targetColor, _hoveredColor, 0.3f);
            }
            
            // Health-based color modification
            if (_health < 50f)
            {
                float healthRatio = _health / 50f;
                targetColor = Color.Lerp(_unhealthyColor, targetColor, healthRatio);
            }
            else
            {
                targetColor = Color.Lerp(targetColor, _healthyColor, (_health - 50f) / 50f * 0.2f);
            }
            
            _plantRenderer.material.color = targetColor;
        }
        
        private void UpdateParticleEffects()
        {
            // Health effects
            if (_healthEffects != null)
            {
                if (_health < 30f && !_healthEffects.isPlaying)
                {
                    var main = _healthEffects.main;
                    main.startColor = _unhealthyColor;
                    _healthEffects.Play();
                }
                else if (_health >= 30f && _healthEffects.isPlaying)
                {
                    _healthEffects.Stop();
                }
            }
        }
        
        private void UpdateGlowEffect()
        {
            if (_plantGlow == null) return;
            
            bool shouldGlow = _isSelected || _isHovered || _isHarvestable;
            _plantGlow.enabled = shouldGlow;
            
            if (shouldGlow)
            {
                if (_isHarvestable)
                {
                    _plantGlow.color = Color.gold;
                    _plantGlow.intensity = 1f + Mathf.Sin(Time.time * 2f) * 0.3f;
                }
                else if (_isSelected)
                {
                    _plantGlow.color = _selectedColor;
                    _plantGlow.intensity = 0.8f;
                }
                else if (_isHovered)
                {
                    _plantGlow.color = _hoveredColor;
                    _plantGlow.intensity = 0.6f;
                }
            }
        }
        
        private void UpdateInfoDisplay()
        {
            if (_statusText != null && (_isSelected || _isHovered))
            {
                string statusInfo = $"{_plantInstance.StrainName}\n" +
                                   $"Stage: {_currentStage}\n" +
                                   $"Health: {_health:F0}%\n" +
                                   $"Growth: {_growthProgress:P0}\n" +
                                   $"Water: {_waterLevel:F0}%\n" +
                                   $"Nutrients: {_nutrientLevel:F0}%";
                
                if (_isHarvestable)
                {
                    statusInfo += "\n<color=gold>Ready to Harvest!</color>";
                }
                
                _statusText.text = statusInfo;
                
                if (_infoCanvas != null)
                {
                    _infoCanvas.gameObject.SetActive(true);
                }
            }
            else if (_infoCanvas != null)
            {
                _infoCanvas.gameObject.SetActive(false);
            }
        }
        
        #endregion
        
        #region Growth Effects
        
        private void UpdateGrowthProgress()
        {
            if (_enableVisualGrowth && _growthProgress > 0f)
            {
                // Visual growth scaling within stage
                float stageProgress = _growthProgress;
                Vector3 currentStageScale = GetCurrentStageBaseScale();
                Vector3 nextStageScale = GetNextStageBaseScale();
                Vector3 targetScale = Vector3.Lerp(currentStageScale, nextStageScale, stageProgress);
                
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * 2f);
            }
        }
        
        private Vector3 GetCurrentStageBaseScale()
        {
            return _currentStage switch
            {
                PlantGrowthStage.Seed => _originalScale * 0.3f,
                PlantGrowthStage.Sprout => _originalScale * 0.5f,
                PlantGrowthStage.Vegetative => _originalScale * 0.8f,
                PlantGrowthStage.Flowering => _originalScale,
                PlantGrowthStage.Harvest => _originalScale * 1.2f,
                _ => _originalScale
            };
        }
        
        private Vector3 GetNextStageBaseScale()
        {
            PlantGrowthStage nextStage = _currentStage switch
            {
                PlantGrowthStage.Seed => PlantGrowthStage.Sprout,
                PlantGrowthStage.Sprout => PlantGrowthStage.Vegetative,
                PlantGrowthStage.Vegetative => PlantGrowthStage.Flowering,
                PlantGrowthStage.Flowering => PlantGrowthStage.Harvest,
                _ => _currentStage
            };
            
            return nextStage switch
            {
                PlantGrowthStage.Sprout => _originalScale * 0.5f,
                PlantGrowthStage.Vegetative => _originalScale * 0.8f,
                PlantGrowthStage.Flowering => _originalScale,
                PlantGrowthStage.Harvest => _originalScale * 1.2f,
                _ => _originalScale
            };
        }
        
        private void PlayGrowthEffects()
        {
            if (_growthParticles != null)
            {
                _growthParticles.Play();
            }
            
            PlayAudioClip(_growthSFX);
        }
        
        #endregion
        
        #region Interaction System
        
        private void HandleInteractionInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CheckForClick();
            }
        }
        
        private void CheckForClick()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == _interactionCollider)
                {
                    OnPlantClicked?.Invoke(this);
                    SetSelected(true);
                }
            }
        }
        
        private void OnMouseEnter()
        {
            SetHovered(true);
            OnPlantHovered?.Invoke(this);
        }
        
        private void OnMouseExit()
        {
            SetHovered(false);
            OnPlantUnhovered?.Invoke(this);
        }
        
        public void SetSelected(bool selected)
        {
            _isSelected = selected;
        }
        
        public void SetHovered(bool hovered)
        {
            _isHovered = hovered;
        }
        
        #endregion
        
        #region Public Interface
        
        /// <summary>
        /// Water the plant manually
        /// </summary>
        public void WaterPlant(float amount = 25f)
        {
            ModifyWaterLevel(amount);
        }
        
        /// <summary>
        /// Add nutrients to the plant
        /// </summary>
        public void AddNutrients(float amount = 20f)
        {
            ModifyNutrientLevel(amount);
        }
        
        /// <summary>
        /// Harvest the plant if ready
        /// </summary>
        public HarvestResult HarvestPlant()
        {
            if (!_isHarvestable)
                return null;
            
            var result = new HarvestResult
            {
                PlantId = _plantInstance.PlantId,
                StrainName = _plantInstance.StrainName,
                HarvestDate = DateTime.Now,
                TotalYield = CalculateYield(),
                Quality = CalculateQuality()
            };
            
            PlayAudioClip(_harvestSFX);
            
            // Destroy plant object after harvest
            StartCoroutine(DestroyAfterHarvest());
            
            return result;
        }
        
        private float CalculateYield()
        {
            float baseYield = _strainData?.ExpectedYield ?? 50f;
            float healthModifier = _health / 100f;
            float environmentalModifier = CalculateEnvironmentalQuality();
            
            return baseYield * healthModifier * environmentalModifier;
        }
        
        private float CalculateQuality()
        {
            float healthFactor = _health / 100f;
            float environmentalFactor = CalculateEnvironmentalQuality();
            
            return (healthFactor + environmentalFactor) / 2f * 100f;
        }
        
        private float CalculateEnvironmentalQuality()
        {
            float tempQuality = CalculateTemperatureModifier();
            float humidityQuality = CalculateHumidityModifier();
            float lightQuality = CalculateLightModifier();
            
            return (tempQuality + humidityQuality + lightQuality) / 3f;
        }
        
        private IEnumerator DestroyAfterHarvest()
        {
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Get detailed plant status information
        /// </summary>
        public PlantStatusInfo GetStatusInfo()
        {
            return new PlantStatusInfo
            {
                PlantId = _plantInstance.PlantId,
                StrainName = _plantInstance.StrainName,
                CurrentStage = _currentStage,
                Health = _health,
                GrowthProgress = _growthProgress,
                WaterLevel = _waterLevel,
                NutrientLevel = _nutrientLevel,
                DaysOld = (DateTime.Now - _plantInstance.PlantedDate).Days,
                IsHarvestable = _isHarvestable,
                EstimatedYield = CalculateYield(),
                EnvironmentalConditions = _currentConditions
            };
        }
        
        #endregion
        
        #region Audio
        
        private void PlayAudioClip(AudioClip clip)
        {
            if (clip != null && _plantAudioSource != null)
            {
                _plantAudioSource.PlayOneShot(clip);
            }
        }
        
        #endregion
        
        private void OnDestroy()
        {
            // Clean up any ongoing coroutines
            StopAllCoroutines();
        }
        
        #region Debug
        
        private void OnDrawGizmosSelected()
        {
            // Draw sensor ranges
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _temperatureSensorRange);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _humiditySensorRange);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _lightSensorRange);
        }
        
        #endregion
    }
    
    [System.Serializable]
    public class PlantStatusInfo
    {
        public string PlantId;
        public string StrainName;
        public PlantGrowthStage CurrentStage;
        public float Health;
        public float GrowthProgress;
        public float WaterLevel;
        public float NutrientLevel;
        public int DaysOld;
        public bool IsHarvestable;
        public float EstimatedYield;
        public EnvironmentalConditions EnvironmentalConditions;
    }
    
    [System.Serializable]
    public class HarvestResult
    {
        public string PlantId;
        public string StrainName;
        public DateTime HarvestDate;
        public float TotalYield;
        public float Quality;
    }
}