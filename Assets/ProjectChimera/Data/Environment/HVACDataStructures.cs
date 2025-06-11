using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ProjectChimera.Data.Equipment;
using ProjectChimera.Data.Genetics;

namespace ProjectChimera.Data.Environment
{
    /// <summary>
    /// Comprehensive data structures for HVAC system management in Project Chimera.
    /// Includes zone management, equipment control, automation, and energy optimization.
    /// </summary>

    [System.Serializable]
    public class HVACSystemSettings
    {
        [Range(0.1f, 5f)] public float DefaultTemperatureTolerance = 1f;
        [Range(0.5f, 10f)] public float DefaultHumidityTolerance = 3f;
        [Range(0.01f, 1f)] public float DefaultAirflowTolerance = 0.1f;
        public bool EnableAdvancedDiagnostics = true;
        public bool EnableEnergyOptimization = true;
        public bool EnablePredictiveControl = true;
        [Range(1f, 168f)] public float MaintenanceCheckInterval = 24f; // Hours
        [Range(1f, 72f)] public float EnergyOptimizationInterval = 8f; // Hours
        [Range(60f, 3600f)] public float AlarmResponseTime = 300f; // Seconds
    }

    [System.Serializable]
    public class HVACZoneSettings
    {
        [Range(15f, 35f)] public float DefaultTemperature = 24f;
        [Range(20f, 80f)] public float DefaultHumidity = 55f;
        [Range(0.1f, 2f)] public float DefaultAirflow = 0.3f;
        [Range(300f, 1500f)] public float DefaultCO2Level = 800f;
        [Range(10f, 1000f)] public float ZoneVolume = 100f; // Cubic meters
        public bool EnableZoneIsolation = true;
        public bool EnableVPDControl = true;
        public ZonePriority ZonePriority = ZonePriority.Normal;
    }

    [System.Serializable]
    public class TemperatureControlSettings
    {
        [Range(15f, 35f)] public float MinTemperature = 18f;
        [Range(15f, 35f)] public float MaxTemperature = 30f;
        [Range(0.1f, 5f)] public float TemperatureRampRate = 1f; // Degrees per hour
        public bool EnableNightTimeSetback = true;
        [Range(0f, 10f)] public float NightTimeSetback = 3f;
        public bool EnableSeasonalAdjustment = false;
    }

    [System.Serializable]
    public class HumidityControlSettings
    {
        [Range(20f, 80f)] public float MinHumidity = 40f;
        [Range(20f, 80f)] public float MaxHumidity = 70f;
        [Range(1f, 20f)] public float HumidityRampRate = 5f; // Percent per hour
        public bool EnableDeadband = true;
        [Range(1f, 10f)] public float DeadbandWidth = 5f;
        public bool EnableCondensationPrevention = true;
    }

    [System.Serializable]
    public class AirflowControlSettings
    {
        [Range(0.1f, 2f)] public float MinAirVelocity = 0.1f;
        [Range(0.1f, 2f)] public float MaxAirVelocity = 1.5f;
        [Range(0.5f, 10f)] public float AirChangesPerHour = 4f;
        public bool EnableVariableAirflow = true;
        public bool EnableCO2BasedControl = true;
        [Range(300f, 1500f)] public float CO2Setpoint = 800f;
    }

    [System.Serializable]
    public class PIDControllerSettings
    {
        [Range(0.1f, 2f)] public float TemperatureKp = 0.8f;
        [Range(0.01f, 0.5f)] public float TemperatureKi = 0.1f;
        [Range(0.01f, 0.2f)] public float TemperatureKd = 0.05f;
        
        [Range(0.1f, 2f)] public float HumidityKp = 0.6f;
        [Range(0.01f, 0.5f)] public float HumidityKi = 0.08f;
        [Range(0.01f, 0.2f)] public float HumidityKd = 0.03f;
        
        [Range(0.1f, 2f)] public float AirflowKp = 0.9f;
        [Range(0.01f, 0.5f)] public float AirflowKi = 0.12f;
        [Range(0.01f, 0.2f)] public float AirflowKd = 0.04f;
    }

    [System.Serializable]
    public class VPDOptimizationSettings
    {
        [Range(0.4f, 1.6f)] public float TargetVPD = 1.0f;
        [Range(0.1f, 0.5f)] public float VPDTolerance = 0.2f;
        public bool EnableDynamicVPD = true;
        public bool EnableGrowthStageAdjustment = true;
        public List<VPDSchedulePoint> VPDSchedule = new List<VPDSchedulePoint>();
    }

    [System.Serializable]
    public class VPDSchedulePoint
    {
        [Range(0, 24)] public int Hour;
        [Range(0.4f, 1.6f)] public float TargetVPD;
        public PlantGrowthStage GrowthStage;
    }

    [System.Serializable]
    public class HVACZone
    {
        public string ZoneId;
        public string ZoneName;
        public HVACZoneSettings ZoneSettings;
        public EnvironmentalConditions CurrentConditions;
        public EnvironmentalConditions TargetConditions;
        public List<ActiveHVACEquipment> ZoneEquipment = new List<ActiveHVACEquipment>();
        public HVACControlParameters ControlParameters;
        public HVACZoneStatus ZoneStatus;
        public System.DateTime CreatedAt;
        public System.DateTime LastUpdated;
    }

    [System.Serializable]
    public class ActiveHVACEquipment
    {
        public string EquipmentId;
        public EquipmentDataSO EquipmentData;
        public string ZoneId;
        public Vector3 Position;
        public HVACEquipmentStatus Status;
        [Range(0f, 1f)] public float PowerLevel;
        public float OperatingHours;
        [Range(0.1f, 1.5f)] public float EfficiencyRating = 1f;
        public MaintenanceStatus MaintenanceStatus;
        public System.DateTime InstallationDate;
        public System.DateTime LastMaintenanceDate;
        public List<HVACAlarm> EquipmentAlarms = new List<HVACAlarm>();
    }

    [System.Serializable]
    public class HVACControlParameters
    {
        public bool TemperatureControlEnabled = true;
        public bool HumidityControlEnabled = true;
        public bool AirflowControlEnabled = true;
        public bool CO2ControlEnabled = false;
        public bool VPDOptimizationEnabled = false;
        public VPDOptimizationSettings VPDSettings;
        public ControlStrategy ControlStrategy = ControlStrategy.PID;
        [Range(0.1f, 10f)] public float ControlGain = 1f;
        public bool EnableNightMode = true;
        public NightModeSettings NightModeSettings;
    }

    [System.Serializable]
    public class NightModeSettings
    {
        [Range(0, 23)] public int NightStartHour = 22;
        [Range(1, 23)] public int NightEndHour = 6;
        [Range(-10f, 0f)] public float TemperatureOffset = -2f;
        [Range(-20f, 0f)] public float HumidityOffset = -5f;
        [Range(0f, 1f)] public float AirflowReduction = 0.3f;
    }

    [System.Serializable]
    public class HVACControlLoop
    {
        public string ControlId;
        public string ZoneId;
        public HVACControlType ControlType;
        public PIDController PIDController;
        public bool IsActive;
        public float SetPoint;
        public float CurrentValue;
        public float ControlOutput;
        public float Tolerance;
        public ControlMode ControlMode = ControlMode.Automatic;
        public System.DateTime LastUpdate;
    }

    [System.Serializable]
    public class PIDController
    {
        public float Kp { get; set; }
        public float Ki { get; set; }
        public float Kd { get; set; }
        
        private float _integral;
        private float _previousError;
        private bool _firstRun = true;
        
        public PIDController(float kp, float ki, float kd)
        {
            Kp = kp;
            Ki = ki;
            Kd = kd;
            Reset();
        }
        
        public float Calculate(float setpoint, float processValue, float deltaTime)
        {
            float error = setpoint - processValue;
            
            if (_firstRun)
            {
                _previousError = error;
                _firstRun = false;
            }
            
            // Proportional term
            float proportional = Kp * error;
            
            // Integral term
            _integral += error * deltaTime;
            float integral = Ki * _integral;
            
            // Derivative term
            float derivative = Kd * (error - _previousError) / deltaTime;
            
            _previousError = error;
            
            return proportional + integral + derivative;
        }
        
        public void Reset()
        {
            _integral = 0f;
            _previousError = 0f;
            _firstRun = true;
        }
    }

    [System.Serializable]
    public class HVACAlarm
    {
        public string AlarmId;
        public string ZoneId;
        public string EquipmentId;
        public HVACAlarmType AlarmType;
        public HVACAlarmPriority Priority;
        public HVACAlarmStatus AlarmStatus;
        public string AlarmMessage;
        public float AlarmValue;
        public float ThresholdValue;
        public System.DateTime TriggerTime;
        public System.DateTime? AcknowledgeTime;
        public System.DateTime? ClearTime;
        public bool RequiresManualReset;
    }

    [System.Serializable]
    public class HVACMaintenanceSchedule
    {
        public string ScheduleId;
        public string EquipmentId;
        public MaintenanceType MaintenanceType;
        public System.DateTime ScheduledDate;
        public int EstimatedDuration; // Minutes
        public float MaintenanceCost;
        public MaintenancePriority Priority;
        public MaintenanceScheduleStatus Status;
        public string MaintenanceNotes;
        public List<string> RequiredParts = new List<string>();
        public List<string> RequiredSkills = new List<string>();
    }

    [System.Serializable]
    public class HVACSystemStatus
    {
        public int TotalZones;
        public int ActiveZones;
        public int TotalEquipment;
        public int RunningEquipment;
        public float SystemEfficiency;
        public float TotalEnergyConsumption;
        public int ActiveAlarms;
        public System.DateTime LastUpdated;
    }

    [System.Serializable]
    public class EnergyConsumptionTracker
    {
        private List<EnergyDataPoint> _consumptionHistory = new List<EnergyDataPoint>();
        private float _currentConsumption;
        
        public void RecordEnergyConsumption(float consumption, System.DateTime timestamp)
        {
            _currentConsumption = consumption;
            
            _consumptionHistory.Add(new EnergyDataPoint
            {
                Consumption = consumption,
                Timestamp = timestamp
            });
            
            // Keep only last 1000 data points
            if (_consumptionHistory.Count > 1000)
                _consumptionHistory.RemoveAt(0);
        }
        
        public float GetCurrentConsumption() => _currentConsumption;
        
        public float GetAverageConsumption(int hours)
        {
            var cutoffTime = System.DateTime.Now.AddHours(-hours);
            var recentData = _consumptionHistory.Where(dp => dp.Timestamp >= cutoffTime).ToList();
            
            return recentData.Count > 0 ? recentData.Average(dp => dp.Consumption) : 0f;
        }
        
        public List<EnergyDataPoint> GetConsumptionHistory() => _consumptionHistory.ToList();
    }

    [System.Serializable]
    public class EnergyDataPoint
    {
        public float Consumption;
        public System.DateTime Timestamp;
    }

    [System.Serializable]
    public class HVACZoneSnapshot
    {
        public string ZoneId;
        public string ZoneName;
        public System.DateTime Timestamp;
        public EnvironmentalConditions CurrentConditions;
        public EnvironmentalConditions TargetConditions;
        public HVACZoneStatus ZoneStatus;
        public List<HVACEquipmentSnapshot> EquipmentStatus = new List<HVACEquipmentSnapshot>();
        public HVACControlPerformance ControlPerformance;
        public float EnergyEfficiency;
        public VPDOptimizationStatus VPDOptimal;
    }

    [System.Serializable]
    public class HVACEquipmentSnapshot
    {
        public string EquipmentId;
        public string EquipmentName;
        public HVACEquipmentStatus Status;
        public float PowerLevel;
        public float EfficiencyRating;
        public float EnergyConsumption;
        public MaintenanceStatus MaintenanceStatus;
    }

    [System.Serializable]
    public class HVACControlPerformance
    {
        public float TemperatureAccuracy;
        public float HumidityAccuracy;
        public float AirflowAccuracy;
        public float OverallPerformance;
        public int ControlLoopErrors;
        public System.TimeSpan ResponseTime;
    }

    [System.Serializable]
    public class VPDOptimizationStatus
    {
        public float CurrentVPD;
        public float TargetVPD;
        public float VPDDeviation;
        public bool IsOptimal;
        public List<string> OptimizationActions = new List<string>();
    }

    [System.Serializable]
    public class EnergyOptimizationResult
    {
        public float PotentialSavings;
        public List<EquipmentOptimization> EquipmentOptimizations = new List<EquipmentOptimization>();
        public List<string> RecommendedActions = new List<string>();
        public float ImplementationCost;
        public float PaybackPeriodMonths;
    }

    [System.Serializable]
    public class EquipmentOptimization
    {
        public string EquipmentId;
        public float CurrentPowerLevel;
        public float OptimalPowerLevel;
        public float EnergySavings;
        public string OptimizationReason;
    }

    [System.Serializable]
    public class EnergyConsumptionReport
    {
        public System.DateTime ReportDate;
        public System.TimeSpan ReportingPeriod;
        public List<ZoneEnergyReport> ZoneReports = new List<ZoneEnergyReport>();
        public float TotalEnergyConsumption;
        public float TotalEnergyCost;
        public float AverageEfficiency;
    }

    [System.Serializable]
    public class ZoneEnergyReport
    {
        public string ZoneId;
        public string ZoneName;
        public float TotalEnergyConsumption;
        public Dictionary<string, float> EquipmentConsumption = new Dictionary<string, float>();
        public float EfficiencyRating;
        public List<string> OptimizationOpportunities = new List<string>();
    }

    [System.Serializable]
    public class EnvironmentalPrediction
    {
        public float PredictionTime; // Minutes ahead
        public bool RequiresAction;
        public List<string> RecommendedActions = new List<string>();
        public float ConfidenceLevel;
        public EnvironmentalConditions PredictedConditions;
    }

    // Equipment-specific ScriptableObject classes
    [CreateAssetMenu(fileName = "New Heating Equipment", menuName = "Project Chimera/Equipment/HVAC/Heating Equipment")]
    public class HeatingEquipmentSO : EquipmentDataSO
    {
        [Header("Heating Specifications")]
        [Range(1000f, 50000f)] public float HeatingCapacity = 5000f; // Watts
        [Range(0.8f, 1.2f)] public float EfficiencyRating = 0.95f;
        [Range(15f, 35f)] public float MinOperatingTemperature = 15f;
        [Range(15f, 35f)] public float MaxOperatingTemperature = 35f;
        public HeatingMethod HeatingMethod = HeatingMethod.Electric;
        public bool SupportsModulation = true;
    }

    [CreateAssetMenu(fileName = "New Cooling Equipment", menuName = "Project Chimera/Equipment/HVAC/Cooling Equipment")]
    public class CoolingEquipmentSO : EquipmentDataSO
    {
        [Header("Cooling Specifications")]
        [Range(1000f, 50000f)] public float CoolingCapacity = 5000f; // Watts
        [Range(2f, 6f)] public float COP = 3.5f; // Coefficient of Performance
        [Range(15f, 35f)] public float MinOperatingTemperature = 18f;
        [Range(15f, 35f)] public float MaxOperatingTemperature = 30f;
        public CoolingMethod CoolingMethod = CoolingMethod.AirConditioner;
        public bool SupportsVariableSpeed = true;
    }

    [CreateAssetMenu(fileName = "New Humidification Equipment", menuName = "Project Chimera/Equipment/HVAC/Humidification Equipment")]
    public class HumidificationEquipmentSO : EquipmentDataSO
    {
        [Header("Humidification Specifications")]
        [Range(1f, 50f)] public float HumidificationRate = 5f; // Liters per hour
        [Range(20f, 80f)] public float MinHumidity = 30f;
        [Range(20f, 80f)] public float MaxHumidity = 70f;
        public HumidificationMethod HumidificationMethod = HumidificationMethod.Ultrasonic;
        public bool RequiresWaterConnection = true;
    }

    [CreateAssetMenu(fileName = "New Dehumidification Equipment", menuName = "Project Chimera/Equipment/HVAC/Dehumidification Equipment")]
    public class DehumidificationEquipmentSO : EquipmentDataSO
    {
        [Header("Dehumidification Specifications")]
        [Range(1f, 50f)] public float DehumidificationRate = 3f; // Liters per hour
        [Range(20f, 80f)] public float OperatingHumidityRange = 60f;
        public DehumidificationMethod DehumidificationMethod = DehumidificationMethod.Refrigeration;
        public bool RequiresDrainConnection = true;
    }

    [CreateAssetMenu(fileName = "New Fan Equipment", menuName = "Project Chimera/Equipment/HVAC/Fan Equipment")]
    public class FanEquipmentSO : EquipmentDataSO
    {
        [Header("Fan Specifications")]
        [Range(100f, 10000f)] public float AirflowRate = 1000f; // CFM
        [Range(0.1f, 2f)] public float MaxAirVelocity = 1.5f;
        [Range(20f, 80f)] public float NoiseLevel = 40f; // dBA
        public FanType FanType = FanType.Axial;
        public bool SupportsVariableSpeed = true;
    }

    [CreateAssetMenu(fileName = "New Ventilation Equipment", menuName = "Project Chimera/Equipment/HVAC/Ventilation Equipment")]
    public class VentilationEquipmentSO : EquipmentDataSO
    {
        [Header("Ventilation Specifications")]
        [Range(100f, 10000f)] public float VentilationRate = 500f; // CFM
        [Range(0.5f, 10f)] public float AirChangesPerHour = 4f;
        public VentilationType VentilationType = VentilationType.Exhaust;
        public bool SupportsHeatRecovery = false;
    }

    // Comprehensive enum definitions
    public enum HVACZoneStatus
    {
        Active,
        Standby,
        Maintenance,
        Alarm,
        Offline
    }

    public enum HVACEquipmentStatus
    {
        Off,
        Standby,
        Running,
        Fault,
        Maintenance
    }

    public enum HVACControlType
    {
        Temperature,
        Humidity,
        Airflow,
        CO2,
        VPD
    }

    public enum ControlStrategy
    {
        OnOff,
        PID,
        Fuzzy,
        Model_Predictive,
        Adaptive
    }

    public enum ControlMode
    {
        Automatic,
        Manual,
        Override,
        Emergency
    }

    public enum HVACAlarmType
    {
        Temperature_High,
        Temperature_Low,
        Humidity_High,
        Humidity_Low,
        Equipment_Fault,
        Communication_Loss,
        Energy_Efficiency_Low,
        Maintenance_Required,
        Filter_Replacement
    }

    public enum HVACAlarmPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum HVACAlarmStatus
    {
        Active,
        Acknowledged,
        Cleared,
        Disabled
    }

    public enum MaintenanceType
    {
        Routine,
        Preventive,
        Corrective,
        Emergency,
        Filter_Change,
        Calibration,
        Cleaning
    }

    public enum MaintenancePriority
    {
        Low,
        Normal,
        High,
        Critical
    }

    public enum MaintenanceScheduleStatus
    {
        Scheduled,
        In_Progress,
        Completed,
        Overdue,
        Cancelled
    }

    public enum MaintenanceStatus
    {
        Excellent,
        Good,
        Fair,
        Poor,
        Critical
    }

    public enum ZonePriority
    {
        Low,
        Normal,
        High,
        Critical
    }

    public enum HeatingMethod
    {
        Electric,
        Gas,
        Heat_Pump,
        Radiant,
        Steam,
        Hot_Water
    }

    public enum CoolingMethod
    {
        AirConditioner,
        Evaporative,
        Chilled_Water,
        Heat_Pump,
        Refrigeration
    }

    public enum HumidificationMethod
    {
        Steam,
        Ultrasonic,
        Evaporative,
        Atomizing,
        Wetted_Media
    }

    public enum DehumidificationMethod
    {
        Refrigeration,
        Desiccant,
        Membrane,
        Condensation
    }

    public enum FanType
    {
        Axial,
        Centrifugal,
        Mixed_Flow,
        Cross_Flow,
        Inline
    }

    public enum VentilationType
    {
        Supply,
        Exhaust,
        Mixed,
        Heat_Recovery
    }


}