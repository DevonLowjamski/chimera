using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectChimera.Core;

namespace ProjectChimera.Data.Construction
{
    // Building and Construction System Data Structures
    [System.Serializable]
    public enum BuildingType
    {
        GrowTent,
        GrowRoom,
        Greenhouse,
        ProcessingFacility,
        StorageWarehouse,
        LaboratoryFacility,
        OfficeSpace,
        SecurityFacility,
        UtilityBuilding,
        ResearchFacility
    }

    [System.Serializable]
    public enum ConstructionStage
    {
        Planning,
        Foundation,
        Framing,
        Utilities,
        Electrical,
        Plumbing,
        HVAC,
        Insulation,
        Drywall,
        Flooring,
        Equipment,
        Finishing,
        Inspection,
        Completed
    }

    [System.Serializable]
    public enum BuildingMaterial
    {
        Wood,
        Steel,
        Concrete,
        Glass,
        Aluminum,
        Insulation,
        Electrical,
        Plumbing,
        HVAC,
        Specialized
    }

    [System.Serializable]
    public enum ConstructionPriority
    {
        Low,
        Normal,
        High,
        Emergency
    }

    [System.Serializable]
    public enum PermitType
    {
        Building,
        Electrical,
        Plumbing,
        HVAC,
        Fire_Safety,
        Environmental,
        Zoning,
        Cannabis_License,
        Mechanical,
        Fire,
        Cannabis_Cultivation,
        Cannabis_Processing
    }

    [System.Serializable]
    public enum InspectionType
    {
        Foundation,
        Framing,
        Electrical,
        Plumbing,
        HVAC,
        Insulation,
        Fire_Safety,
        Final,
        Cannabis_Compliance,
        Safety,
        Structural
    }

    [System.Serializable]
    public enum WorkerSpecialty
    {
        GeneralConstruction,
        GeneralLabor,
        Electrician,
        Electrical,
        Plumber,
        Plumbing,
        HVAC_Technician,
        HVAC,
        Carpenter,
        Mason,
        Roofer,
        Painter,
        Flooring_Specialist,
        Equipment_Installer,
        Inspector,
        Project_Manager
    }

    [System.Serializable]
    public enum BuildingQuality
    {
        Basic,
        Standard,
        Premium,
        Luxury,
        Industrial
    }

    // Core Building and Construction Classes
    [System.Serializable]
    public class BuildingProject
    {
        [Header("Project Information")]
        public string ProjectId;
        public string ProjectName;
        public string Description;
        public BuildingType BuildingType;
        public BuildingQuality QualityLevel;
        
        [Header("Planning and Design")]
        public BuildingBlueprint Blueprint;
        public List<BuildingRequirement> Requirements;
        public Vector3 PlannedLocation;
        public Vector3 PlannedDimensions;
        public float PlannedArea;
        
        [Header("Construction Progress")]
        public ConstructionStage CurrentStage;
        public float OverallProgress; // 0-1
        public DateTime ProjectStartDate;
        public DateTime EstimatedCompletionDate;
        public DateTime ActualCompletionDate;
        
        [Header("Resources and Cost")]
        public float TotalBudget;
        public float SpentAmount;
        public float RemainingBudget;
        public int EstimatedDays;
        public List<MaterialRequirement> MaterialRequirements;
        public List<WorkerAssignment> WorkerAssignments;
        
        [Header("Permits and Compliance")]
        public List<ConstructionPermit> RequiredPermits;
        public List<ConstructionInspection> Inspections;
        public bool ComplianceApproved;
        
        [Header("Status and Issues")]
        public ConstructionPriority Priority;
        public List<ConstructionIssue> Issues;
        public List<string> CompletedMilestones;
        public bool IsOnSchedule;
        public bool IsOnBudget;
        
        [Header("Quality Control")]
        public QualityMetrics QualityScores;
        public List<QualityInspection> QualityInspections;
        public float QualityRating; // 0-5 stars
    }

    [System.Serializable]
    public class BuildingBlueprint
    {
        [Header("Blueprint Information")]
        public string BlueprintId;
        public string Name;
        public string Description;
        public BuildingType BuildingType;
        public string ArchitectName;
        public DateTime CreatedDate;
        public string Version;
        
        [Header("Dimensions and Layout")]
        public Vector3 Dimensions;
        public float TotalArea;
        public int FloorCount;
        public List<RoomLayout> Rooms;
        public List<UtilityConnection> UtilityConnections;
        
        [Header("Material Specifications")]
        public List<MaterialSpecification> MaterialSpecs;
        public List<EquipmentSpecification> EquipmentSpecs;
        public EnvironmentalSpecifications EnvironmentalSpecs;
        
        [Header("Compliance and Standards")]
        public List<string> BuildingCodes;
        public List<string> SafetyStandards;
        public List<string> EnvironmentalStandards;
        public bool CannabisSuitability;
        
        [Header("Cost Estimates")]
        public float EstimatedCost;
        public float LaborCostEstimate;
        public float MaterialCostEstimate;
        public float PermitCostEstimate;
        public int EstimatedDays;
    }

    [System.Serializable]
    public class RoomLayout
    {
        public string RoomId;
        public string RoomName;
        public string RoomType; // GrowRoom, ProcessingRoom, Storage, etc.
        public Vector3 Position;
        public Vector3 Dimensions;
        public float Area;
        public List<DoorSpecification> Doors;
        public List<WindowSpecification> Windows;
        public UtilityRequirements Utilities;
        public EnvironmentalRequirements Environmental;
        public List<string> SpecialFeatures;
    }

    [System.Serializable]
    public class DoorSpecification
    {
        public string DoorId;
        public string DoorType; // Standard, Security, Fire, etc.
        public Vector3 Position;
        public Vector2 Size;
        public string Material;
        public SecurityLevel SecurityLevel;
        public bool FireRated;
        public float Cost;
    }

    [System.Serializable]
    public class WindowSpecification
    {
        public string WindowId;
        public string WindowType;
        public Vector3 Position;
        public Vector2 Size;
        public string GlassType;
        public bool Operable;
        public float UValue; // Thermal performance
        public float Cost;
    }

    [System.Serializable]
    public class UtilityConnection
    {
        public string UtilityId;
        public UtilityType Type;
        public Vector3 ConnectionPoint;
        public float Capacity;
        public string Specifications;
        public float InstallationCost;
        public bool RequiresPermit;
    }

    [System.Serializable]
    public class UtilityRequirements
    {
        public ElectricalRequirements Electrical;
        public PlumbingRequirements Plumbing;
        public HVACRequirements HVAC;
        public DataRequirements Data;
        public SecurityRequirements Security;
    }

    [System.Serializable]
    public class ElectricalRequirements
    {
        public float VoltageRequired;
        public float AmperageRequired;
        public int OutletCount;
        public bool ThreePhaseRequired;
        public bool EmergencyPowerRequired;
        public List<string> SpecialCircuits;
    }

    [System.Serializable]
    public class PlumbingRequirements
    {
        public bool HotWaterRequired;
        public bool DrainageRequired;
        public float WaterPressureRequired;
        public int FixtureCount;
        public bool BackupWaterRequired;
        public List<string> SpecialSystems;
    }

    [System.Serializable]
    public class HVACRequirements
    {
        public float TemperatureRange;
        public float HumidityRange;
        public float AirChangeRate;
        public bool FiltrationRequired;
        public bool CO2ControlRequired;
        public VentilationType VentilationType;
    }

    [System.Serializable]
    public class DataRequirements
    {
        public bool NetworkRequired;
        public int DataOutletCount;
        public bool FiberRequired;
        public bool WirelessRequired;
        public SecurityLevel DataSecurity;
    }

    [System.Serializable]
    public class SecurityRequirements
    {
        public SecurityLevel Level;
        public bool CameraRequired;
        public bool AccessControlRequired;
        public bool AlarmSystemRequired;
        public bool MotionSensorsRequired;
        public List<string> SpecialSecurity;
    }

    [System.Serializable]
    public class EnvironmentalRequirements
    {
        public float MinTemperature;
        public float MaxTemperature;
        public float MinHumidity;
        public float MaxHumidity;
        public float MinLightLevel;
        public float MaxLightLevel;
        public bool ControlledEnvironment;
        public List<string> SpecialConditions;
    }

    [System.Serializable]
    public class MaterialSpecification
    {
        public string MaterialId;
        public string MaterialName;
        public BuildingMaterial MaterialType;
        public string Grade;
        public string Supplier;
        public float Quantity;
        public string Unit; // sqft, lbs, pieces, etc.
        public float UnitCost;
        public float TotalCost;
        public DateTime DeliveryDate;
        public bool IsSpecialized;
        public List<string> Specifications;
    }

    [System.Serializable]
    public class EquipmentSpecification
    {
        public string EquipmentId;
        public string EquipmentName;
        public string Category;
        public string Model;
        public string Manufacturer;
        public float Cost;
        public DateTime InstallationDate;
        public List<string> InstallationRequirements;
        public WarrantyInfo Warranty;
    }

    [System.Serializable]
    public class EnvironmentalSpecifications
    {
        public InsulationSpecification Insulation;
        public VentilationSpecification Ventilation;
        public LightingSpecification Lighting;
        public SoundproofingSpecification Soundproofing;
        public ContainmentSpecification Containment;
    }

    [System.Serializable]
    public class InsulationSpecification
    {
        public string InsulationType;
        public float RValue;
        public float Thickness;
        public bool VaporBarrier;
        public string FireRating;
    }

    [System.Serializable]
    public class VentilationSpecification
    {
        public VentilationType Type;
        public float AirChangeRate;
        public bool FiltrationSystem;
        public bool HeatRecovery;
        public bool CO2Control;
    }

    [System.Serializable]
    public class LightingSpecification
    {
        public string LightingType;
        public float LightLevel; // Lux
        public string ColorTemperature;
        public bool DimmingControl;
        public bool TimerControl;
        public bool SpectrumControl;
    }

    [System.Serializable]
    public class SoundproofingSpecification
    {
        public float SoundTransmissionClass;
        public string SoundproofingMaterial;
        public float Thickness;
        public bool VibrationControl;
    }

    [System.Serializable]
    public class ContainmentSpecification
    {
        public bool AirSealRequired;
        public bool VaporBarrierRequired;
        public bool SecurityContainmentRequired;
        public float ContainmentLevel;
        public List<string> SpecialFeatures;
    }

    [System.Serializable]
    public class BuildingRequirement
    {
        public string RequirementId;
        public string Description;
        public string Category;
        public RequirementPriority Priority;
        public bool IsMandatory;
        public string Source; // Code, regulation, user preference
        public bool IsSatisfied;
        public string SatisfactionMethod;
    }

    [System.Serializable]
    public class MaterialRequirement
    {
        public string MaterialId;
        public string MaterialName;
        public BuildingMaterial Type;
        public float RequiredQuantity;
        public float AvailableQuantity;
        public float OrderedQuantity;
        public string Unit;
        public float UnitCost;
        public string Supplier;
        public DateTime RequiredDate;
        public DateTime OrderDate;
        public DateTime ExpectedDelivery;
        public MaterialStatus Status;
    }

    [System.Serializable]
    public class WorkerAssignment
    {
        public string AssignmentId;
        public string WorkerId;
        public string WorkerName;
        public WorkerSpecialty Specialty;
        public DateTime StartDate;
        public DateTime EndDate;
        public float HoursPerDay;
        public float HourlyRate;
        public string TaskDescription;
        public float CompletionPercentage;
        public WorkerStatus Status;
    }

    [System.Serializable]
    public class ConstructionPermit
    {
        public string PermitId;
        public PermitType Type;
        public string Description;
        public string IssuingAuthority;
        public DateTime ApplicationDate;
        public DateTime IssuedDate;
        public DateTime ExpirationDate;
        public float Cost;
        public PermitStatus Status;
        public List<string> Conditions;
        public List<string> RequiredDocuments;
    }

    [System.Serializable]
    public class ConstructionInspection
    {
        public string InspectionId;
        public InspectionType Type;
        public string Description;
        public DateTime ScheduledDate;
        public DateTime CompletedDate;
        public string InspectorName;
        public InspectionResult Result;
        public List<string> PassedItems;
        public List<string> FailedItems;
        public List<string> Notes;
        public bool ReinspectionRequired;
    }

    [System.Serializable]
    public class ConstructionIssue
    {
        public string IssueId;
        public string Title;
        public string Description;
        public IssueType IssueType;
        public IssueSeverity Severity;
        public IssueCategory Category;
        public DateTime ReportedDate;
        public string ReportedBy;
        public DateTime ResolvedDate;
        public string ResolvedBy;
        public IssueStatus Status;
        public string ResolutionDescription;
        public float CostImpact;
        public int DelayDays;
    }

    [System.Serializable]
    public class QualityMetrics
    {
        public float StructuralQuality; // 0-100
        public float FinishQuality; // 0-100
        public float SystemsQuality; // 0-100
        public float SafetyCompliance; // 0-100
        public float EnvironmentalCompliance; // 0-100
        public float OverallQuality; // 0-100
        public int DefectCount;
        public int ReworkCount;
        public float CustomerSatisfaction; // 0-100
        
        // Additional properties used by ConstructionManager
        public int TotalInspections;
        public int PassedInspections;
    }

    [System.Serializable]
    public class QualityInspection
    {
        public string InspectionId;
        public string Area;
        public DateTime InspectionDate;
        public string InspectorName;
        public float QualityScore; // 0-100
        public List<QualityDefect> Defects;
        public List<string> PassedChecks;
        public string OverallNotes;
    }

    [System.Serializable]
    public class QualityDefect
    {
        public string DefectId;
        public string Description;
        public DefectSeverity Severity;
        public string Location;
        public bool RequiresRework;
        public float EstimatedCostToFix;
        public int EstimatedDaysToFix;
        public DefectStatus Status;
        
        // Additional properties used by ConstructionManager
        public string DefectType;
        public DateTime DetectedDate;
        public string Area;
    }

    [System.Serializable]
    public class WarrantyInfo
    {
        public string WarrantyProvider;
        public int WarrantyPeriodMonths;
        public DateTime WarrantyStartDate;
        public DateTime WarrantyEndDate;
        public string WarrantyTerms;
        public List<string> CoveredItems;
        public List<string> ExcludedItems;
    }

    [System.Serializable]
    public class ConstructionSchedule
    {
        public string ScheduleId;
        public DateTime ProjectStartDate;
        public DateTime ProjectEndDate;
        public List<ConstructionTask> Tasks;
        public List<TaskDependency> Dependencies;
        public List<ScheduleMilestone> Milestones;
        public float OverallProgress;
        public bool IsOnSchedule;
        public int DelayDays;
    }

    [System.Serializable]
    public class ConstructionTask
    {
        [Header("Task Information")]
        public string TaskId;
        public string ProjectId;
        public string TaskName;
        public string Description;
        public ConstructionStage Stage;
        public ConstructionPhase ConstructionPhase;
        public DateTime StartDate;
        public DateTime EndDate;
        public int DurationDays;
        public float EstimatedHours;
        public int RequiredWorkerCount;
        public float Progress; // 0-1
        public TaskStatus Status;
        public List<string> Prerequisites;
        public List<string> RequiredWorkers;
        public List<MaterialRequirement> RequiredMaterials;
        public WorkerSpecialty RequiredSpecialty;
        public float Cost;
        public TaskPriority Priority;
        
        // Add missing property for InProgress status
        public bool InProgress => Status == TaskStatus.In_Progress;
    }

    [System.Serializable]
    public class TaskDependency
    {
        public string DependencyId;
        public string PredecessorTaskId;
        public string SuccessorTaskId;
        public DependencyType Type; // Finish-to-Start, Start-to-Start, etc.
        public int LagDays;
    }

    [System.Serializable]
    public class ScheduleMilestone
    {
        public string MilestoneId;
        public string Name;
        public string Description;
        public DateTime PlannedDate;
        public DateTime ActualDate;
        public bool IsCompleted;
        public float CompletionPercentage;
        public List<string> RequiredTasks;
    }

    // Supporting Enums
    [System.Serializable]
    public enum UtilityType
    {
        Electrical,
        Water,
        Sewer,
        Gas,
        Internet,
        Cable,
        Telephone
    }

    [System.Serializable]
    public enum VentilationType
    {
        Natural,
        Mechanical,
        Hybrid,
        None
    }

    [System.Serializable]
    public enum SecurityLevel
    {
        Basic,
        Standard,
        High,
        Maximum
    }

    [System.Serializable]
    public enum RequirementPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    [System.Serializable]
    public enum MaterialStatus
    {
        Planning,
        Ordered,
        In_Transit,
        Delivered,
        Installed,
        Rejected
    }

    [System.Serializable]
    public enum WorkerStatus
    {
        Assigned,
        Active,
        On_Break,
        Completed,
        Reassigned
    }

    [System.Serializable]
    public enum PermitStatus
    {
        Not_Applied,
        Application_Submitted,
        Under_Review,
        Approved,
        Issued,
        Rejected,
        Expired,
        Submitted
    }

    [System.Serializable]
    public enum InspectionResult
    {
        Pending,
        Passed,
        Failed,
        Conditional_Pass,
        Cancelled
    }

    [System.Serializable]
    public enum IssueSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    [System.Serializable]
    public enum IssueCategory
    {
        Safety,
        Quality,
        Schedule,
        Budget,
        Materials,
        Labor,
        Permits,
        Weather,
        Design_Change
    }

    [System.Serializable]
    public enum IssueStatus
    {
        Open,
        In_Progress,
        Resolved,
        Closed,
        Escalated
    }

    [System.Serializable]
    public enum DefectSeverity
    {
        Minor,
        Moderate,
        Major,
        Critical
    }

    [System.Serializable]
    public enum DefectStatus
    {
        Open,
        In_Repair,
        Fixed,
        Verified,
        Closed
    }

    [System.Serializable]
    public enum TaskStatus
    {
        Not_Started,
        In_Progress,
        Completed,
        On_Hold,
        Cancelled
    }

    [System.Serializable]
    public enum TaskPriority
    {
        Low,
        Normal,
        High,
        Critical
    }

    [System.Serializable]
    public enum DependencyType
    {
        Finish_to_Start,
        Start_to_Start,
        Finish_to_Finish,
        Start_to_Finish
    }

    // Additional Construction Types for InteractiveFacilityConstructor
    [System.Serializable]
    public class FacilityTemplate
    {
        [Header("Template Information")]
        public string TemplateId;
        public string TemplateName;
        public string Description;
        public BuildingType FacilityType;
        public BuildingQuality QualityLevel;
        
        [Header("Dimensions and Layout")]
        public Vector3 Dimensions;
        public float TotalArea;
        public List<ConstructionRoomTemplate> RoomTemplates;
        public List<UtilityConnection> UtilityConnections;
        
        [Header("System Requirements")]
        public float BaseConstructionCost;
        public float RequiredHVACCapacity; // in tons
        public float RequiredPowerCapacity; // in watts
        
        [Header("Construction Requirements")]
        public List<MaterialRequirement> MaterialRequirements;
        public List<PermitType> RequiredPermits;
        public List<WorkerSpecialty> RequiredSpecialties;
        public int EstimatedConstructionDays;
        
        [Header("Cost Estimates")]
        public float EstimatedMaterialCost;
        public float EstimatedLaborCost;
        public float EstimatedPermitCost;
        public float EstimatedTotalCost;
        
        [Header("Compliance and Standards")]
        public List<string> BuildingCodes;
        public List<string> SafetyStandards;
        public bool CannabisSuitability;
        public EnvironmentalSpecifications EnvironmentalSpecs;
        
        // Add missing property for Rooms access
        public List<ConstructionRoomTemplate> Rooms => RoomTemplates;
    }

    [System.Serializable]
    public class ConstructionRoomTemplate
    {
        [Header("Room Information")]
        public string RoomId;
        public string RoomName;
        public string RoomType;
        public string Description;

        [Header("Dimensions")]
        public Vector3 Dimensions;
        public float Area;
        public float Height;
        public float Length;
        public float Width;

        [Header("Requirements")]
        public UtilityRequirements UtilityRequirements;
        public EnvironmentalRequirements EnvironmentalRequirements;
        public SecurityRequirements SecurityRequirements;

        [Header("Features")]
        public List<DoorSpecification> Doors;
        public List<WindowSpecification> Windows;
        public List<string> SpecialFeatures;

        [Header("Cost")]
        public float EstimatedCost;
        public List<MaterialRequirement> MaterialRequirements;
        
        // Add missing property for ProcessingRoom type check
        public bool IsProcessingRoom => RoomType == "ProcessingRoom";
    }

    [System.Serializable]
    public class PlannedRoom
    {
        public string PlannedRoomId;
        public ConstructionRoomTemplate RoomTemplate;
        public Vector3 Position;
        public Quaternion Rotation;
        public RoomStatus Status;
        public float ActualCost;
        public DateTime PlannedDate;
        public DateTime CompletedDate;
        public List<string> ModificationNotes;
    }

    [System.Serializable]
    public class ConstructionProject
    {
        [Header("Project Information")]
        public string ProjectId;
        public string ProjectName;
        public string Description;
        public Vector3 BuildingSite;
        public FacilityTemplate FacilityTemplate;
        
        [Header("Status and Progress")]
        public ProjectStatus Status;
        public ConstructionPhase CurrentPhase;
        public List<ConstructionPhase> CompletedPhases;
        public List<ConstructionPhaseData> PhaseDetails;
        public float OverallProgress; // 0-1
        public List<string> CompletedTasks;
        
        [Header("Dates")]
        public DateTime CreatedDate;
        public DateTime StartDate;
        public DateTime EstimatedCompletionDate;
        public DateTime ActualCompletionDate;
        public DateTime CompletionDate;
        
        [Header("Cost and Budget")]
        public float EstimatedCost;
        public float ActualCost;
        public float RemainingBudget;
        
        [Header("Duration")]
        public float EstimatedDuration; // hours
        public int ActualDuration; // days
        
        [Header("Permits and Validation")]
        public List<PermitType> RequiredPermits;
        public bool PermitsApproved;
        public ValidationResult ValidationResults;
        
        [Header("Planning")]
        public List<PlannedRoom> PlannedRooms;
        public List<ConstructionTask> Tasks;
        
        [Header("Issues and Quality")]
        public List<ConstructionIssue> Issues;
        public QualityMetrics QualityMetrics;
        
        // Extension methods for CS0117 fixes
        public List<PermitApplication> ApprovedPermits => new List<PermitApplication>();
        public List<PermitApplication> RejectedPermits => new List<PermitApplication>();
        public List<PermitType> ApprovedPermitTypes = new List<PermitType>();
    }

    [System.Serializable]
    public enum ConstructionPhase
    {
        Planning,
        Permitting,
        SitePreparation,
        Foundation,
        Structure,
        Systems,
        Finishing,
        Final,
        Completed
    }

    [System.Serializable]
    public class ConstructionPhaseData
    {
        public string PhaseId;
        public string PhaseName;
        public string Description;
        public ConstructionStage Stage;
        public DateTime StartDate;
        public DateTime EndDate;
        public float Progress; // 0-1
        public PhaseStatus Status;
        public List<ConstructionTask> Tasks;
        public List<string> Prerequisites;
        public float EstimatedCost;
        public float ActualCost;
    }

    [System.Serializable]
    public class ConstructionSettings
    {
        [Header("Construction Configuration")]
        public bool EnableRealTimeConstruction;
        public bool EnforceZoningLaws;
        public bool RequirePermits;
        public float ConstructionSpeedMultiplier;
        
        [Header("Design Tools")]
        public bool EnableGridSnapping;
        public bool ShowConstructionGuides;
        public bool ValidateRealTime;
        public GridSnapSettings GridSnapSettings;
        
        [Header("Building Constraints")]
        public Vector3 MinRoomSize;
        public Vector3 MaxRoomSize;
        public float WallThickness;
        public bool RequireFoundation;
        public bool EnforceFireSafety;
        public bool RequireVentilation;
        public float MaxBuildingHeight;
        public float MinSetbackDistance;
        public int RequiredParkingSpaces;
        public float MaxLotCoverage;
        
        [Header("Economic Settings")]
        public bool EnableFinancing;
        public float LaborCostPerHour;
        public float MaterialMarkup;
        
        [Header("Quality Control")]
        public bool EnableQualityInspections;
        public float QualityThreshold;
        public bool AutoFailOnCriticalIssues;
    }

    [System.Serializable]
    public class GridSnapSettings
    {
        public float GridSize;
        public bool SnapToGrid;
        public bool ShowGrid;
        public Color GridColor;
        public Color MajorGridColor;
        public int MajorGridInterval;
        public Vector3 MinRoomSize;
        public Vector3 MaxRoomSize;
        public float WallThickness;
        public bool RequireFoundation;
        public bool EnforceFireSafety;
        public bool RequireVentilation;
        public float MaxBuildingHeight;
        public float MinSetbackDistance;
        public int RequiredParkingSpaces;
        public float MaxLotCoverage;
    }

    [System.Serializable]
    public class ValidationResult
    {
        public bool IsValid;
        public List<string> Errors;
        public List<string> Warnings;
        public List<string> Recommendations;
        public float ValidationScore; // 0-1
    }

    [System.Serializable]
    public class ConstructionProgress
    {
        public string ProgressId;
        public string ProjectId;
        public string TaskId;
        public ConstructionTask Task;
        public DateTime StartTime;
        public float Progress; // 0-1 (alias for CompletionPercentage)
        public float CompletionPercentage; // 0-1
        public TaskStatus Status;
        public DateTime CompletionTime;
        public List<ConstructionWorker> AssignedWorkers;
        public float ActualCost;
        public DateTime LastUpdated;
        public List<string> CompletedMilestones;
        public List<ConstructionIssue> Issues;
        public WorkerEfficiency WorkerEfficiency;
    }

    [System.Serializable]
    public class WorkerEfficiency
    {
        public float EfficiencyRating; // 0-1
        public List<string> EfficiencyFactors;
        public float WeatherImpact;
        public float EquipmentImpact;
        public float SkillImpact;
    }

    [System.Serializable]
    public class PermitApplication
    {
        [Header("Application Information")]
        public string ApplicationId;
        public string ProjectId;
        public PermitType PermitType;
        public DateTime ApplicationDate;
        public DateTime ExpectedApprovalDate;
        public PermitStatus Status;
        public string IssuingAuthority;
        public float Fee;
        public List<string> RequiredDocuments;
        public List<string> SubmittedDocuments;
        public string Notes;
        
        // Add missing properties
        public DateTime SubmissionDate;
        public int EstimatedProcessingDays;
        public float ApplicationFee;
        public bool Submitted => Status != PermitStatus.Not_Applied;
        
        // Add missing properties for permit processing
        public DateTime EstimatedProcessingDate => ApplicationDate.AddDays(7); // Default 7 days
        public DateTime SubmittedDate => SubmissionDate;
        public DateTime ApprovalDate;
        public string RejectionReason;
    }

    [System.Serializable]
    public class ConstructionMetrics
    {
        [Header("Overall Metrics")]
        public int TotalProjects;
        public int ActiveProjects;
        public int CompletedProjects;
        public float TotalValue;
        public int ActiveWorkers;
        public float ConstructionEfficiency;
        public DateTime LastUpdated;
        
        [Header("Quality Metrics")]
        public float AverageCompletionTime;
        public float AverageCostOverrun;
        public int TotalDefects;
        public int TotalRework;
        public float CustomerSatisfactionRating;
        
        [Header("Efficiency Metrics")]
        public float WorkerProductivity;
        public float MaterialWastePercentage;
        public float ScheduleAdherence;
        
        public List<ConstructionWorker> GetActiveWorkers()
        {
            // This would be populated by the construction system
            return new List<ConstructionWorker>();
        }
    }

    [System.Serializable]
    public class ConstructionEvent
    {
        public string EventId;
        public string ProjectId;
        public string TaskId;
        public DateTime EventDate;
        public ConstructionEventType EventType;
        public string Description;
        public string Details;
        public float ActualCost;
        public IssueSeverity Severity;
        public string ReportedBy;
    }

    [System.Serializable]
    public class ConstructionCostUpdate
    {
        public string ProjectId;
        public float PreviousCost;
        public float NewCost;
        public float EstimatedCost;
        public float ActualCostToDate;
        public float CostDifference;
        public float CostVariance;
        public string Reason;
        public DateTime UpdateDate;
        public string UpdatedBy;
    }

    [System.Serializable]
    public class ConstructionReport
    {
        [Header("Report Information")]
        public string ReportId;
        public DateTime ReportDate;
        public DateTime GeneratedDate;
        public string GeneratedBy;
        public string ProjectId; // null for overall report
        
        [Header("Summary")]
        public List<ProjectSummary> ProjectSummaries;
        public ConstructionMetrics TotalMetrics;
        public List<string> KeyFindings;
        public List<string> Recommendations;
        
        [Header("Issues and Quality")]
        public QualityMetrics OverallQuality;
    }

    // Additional Enums for Construction System
    [System.Serializable]
    public enum ProjectStatus
    {
        Planning,
        RequiresRevision,
        PermitPending,
        ReadyToStart,
        InProgress,
        OnHold,
        Completed,
        Cancelled,
        Paused
    }

    [System.Serializable]
    public enum PhaseStatus
    {
        NotStarted,
        InProgress,
        Completed,
        OnHold,
        Failed
    }

    [System.Serializable]
    public enum RoomStatus
    {
        Planned,
        InProgress,
        Completed,
        OnHold,
        Cancelled
    }

    [System.Serializable]
    public enum ConstructionEventType
    {
        ProjectStarted,
        PhaseCompleted,
        IssueReported,
        IssueResolved,
        InspectionPassed,
        InspectionFailed,
        MaterialDelivered,
        WorkerAssigned,
        CostUpdated,
        ScheduleChanged
    }

    [System.Serializable]
    public enum IssueType
    {
        ValidationFailed,
        InvalidPlacement,
        MaterialShortage,
        WorkerUnavailable,
        WeatherDelay,
        PermitIssue,
        QualityFailure,
        SafetyViolation,
        DesignChange,
        BudgetOverrun
    }

    // Additional Missing Types for InteractiveFacilityConstructor
    [System.Serializable]
    public enum SkillLevel
    {
        Novice = 0,
        Apprentice = 1,
        Skilled = 2,
        Expert = 3,
        Master = 4
    }

    [System.Serializable]
    public class ConstructionWorker
    {
        [Header("Worker Information")]
        public string WorkerId;
        public string Name;
        public WorkerSpecialty Specialty;
        public SkillLevel SkillLevel;
        
        [Header("Employment")]
        public float HourlyRate;
        public bool IsAvailable;
        public bool IsContractor; // vs employee
        public DateTime HireDate;
        
        [Header("Performance")]
        public float ProductivityModifier = 1.0f; // 0.5 - 2.0
        public float QualityRating; // 0-5
        public int ProjectsCompleted;
        public float ExperienceYears;
        
        [Header("Current Assignment")]
        public string CurrentProjectId;
        public string CurrentTaskId;
        public DateTime AssignmentStartDate;
        public float HoursWorkedToday;
        
        [Header("Certifications")]
        public List<string> Certifications;
        public List<string> Licenses;
        public DateTime LastSafetyTraining;
    }

    [System.Serializable]
    public class FacilityDesignTool
    {
        public GridSnapSettings GridSettings;
        public bool IsDesigning;
        public FacilityTemplate CurrentTemplate;
        public List<GameObject> PreviewObjects;
        
        public FacilityDesignTool(GridSnapSettings gridSettings)
        {
            GridSettings = gridSettings;
            IsDesigning = false;
            PreviewObjects = new List<GameObject>();
        }
        
        public void StartDesign(FacilityTemplate template)
        {
            CurrentTemplate = template;
            IsDesigning = true;
        }
        
        public void EndDesign()
        {
            IsDesigning = false;
            CurrentTemplate = null;
            ClearPreviews();
        }
        
        public GameObject CreateRoomPreview(ConstructionRoomTemplate roomTemplate, Vector3 position, Quaternion rotation)
        {
            // This would create a preview GameObject in a real implementation
            return null;
        }
        
        private void ClearPreviews()
        {
            PreviewObjects.Clear();
        }
    }

    [System.Serializable]
    public class BuildingValidator
    {
        public ConstructionSettings Settings;
        
        public BuildingValidator(ConstructionSettings settings)
        {
            Settings = settings;
        }
        
        public ValidationResult ValidateBuildingSite(Vector3 buildingSite, FacilityTemplate template)
        {
            var result = new ValidationResult
            {
                IsValid = true,
                Errors = new List<string>(),
                Warnings = new List<string>(),
                Recommendations = new List<string>(),
                ValidationScore = 1.0f
            };
            
            // Add validation logic here
            return result;
        }
        
        public bool ValidateRoomPlacement(ConstructionRoomTemplate roomTemplate, Vector3 position, Quaternion rotation)
        {
            // Add room placement validation logic
            return true;
        }
    }

    [System.Serializable]
    public class ConstructionPlanner
    {
        public List<ConstructionTask> CreateTasksForPhase(ConstructionProject project, ConstructionPhase phase)
        {
            var tasks = new List<ConstructionTask>();
            
            // Create tasks based on phase and project requirements
            // This would contain the actual planning logic
            
            return tasks;
        }
        
        public List<ConstructionTask> GetTasksForPhase(ConstructionProject project, ConstructionPhase phase)
        {
            // Return tasks for the specified phase from the project
            return project.Tasks?.Where(t => t.Stage.ToString() == phase.ToString()).ToList() ?? new List<ConstructionTask>();
        }
        
        public ConstructionSchedule CreateProjectSchedule(ConstructionProject project)
        {
            var schedule = new ConstructionSchedule
            {
                ScheduleId = System.Guid.NewGuid().ToString(),
                ProjectStartDate = project.StartDate,
                ProjectEndDate = project.EstimatedCompletionDate,
                Tasks = new List<ConstructionTask>(),
                Dependencies = new List<TaskDependency>(),
                Milestones = new List<ScheduleMilestone>(),
                OverallProgress = 0f,
                IsOnSchedule = true,
                DelayDays = 0
            };
            
            return schedule;
        }
    }

    [System.Serializable]
    public class ConstructionWorkforce
    {
        public List<ConstructionWorker> Workers;
        public Dictionary<string, ConstructionWorker> WorkerLookup;
        
        public ConstructionWorkforce()
        {
            Workers = new List<ConstructionWorker>();
            WorkerLookup = new Dictionary<string, ConstructionWorker>();
        }
        
        public void AddWorker(ConstructionWorker worker)
        {
            Workers.Add(worker);
            WorkerLookup[worker.WorkerId] = worker;
        }
        
        public List<ConstructionWorker> GetAvailableWorkers(WorkerSpecialty specialty = WorkerSpecialty.GeneralConstruction)
        {
            return Workers.Where(w => w.IsAvailable && (specialty == WorkerSpecialty.GeneralConstruction || w.Specialty == specialty)).ToList();
        }
        
        public List<ConstructionWorker> GetActiveWorkers()
        {
            return Workers.Where(w => !w.IsAvailable && !string.IsNullOrEmpty(w.CurrentTaskId)).ToList();
        }
        
        public ConstructionWorker GetWorker(string workerId)
        {
            return WorkerLookup.TryGetValue(workerId, out var worker) ? worker : null;
        }
        
        public List<ConstructionWorker> AssignWorkers(ConstructionTask task, int workerCount)
        {
            var availableWorkers = GetAvailableWorkers(task.RequiredSpecialty);
            var assignedWorkers = availableWorkers.Take(workerCount).ToList();
            
            foreach (var worker in assignedWorkers)
            {
                worker.IsAvailable = false;
                worker.CurrentTaskId = task.TaskId;
                worker.AssignmentStartDate = DateTime.Now;
            }
            
            return assignedWorkers;
        }
        
        public List<ConstructionWorker> AssignWorkers(WorkerSpecialty specialty, int workerCount)
        {
            var availableWorkers = GetAvailableWorkers(specialty);
            var assignedWorkers = availableWorkers.Take(workerCount).ToList();
            
            foreach (var worker in assignedWorkers)
            {
                worker.IsAvailable = false;
                worker.AssignmentStartDate = DateTime.Now;
            }
            
            return assignedWorkers;
        }
        
        public void ReleaseWorkers(List<ConstructionWorker> workers)
        {
            foreach (var worker in workers)
            {
                worker.IsAvailable = true;
                worker.CurrentTaskId = null;
            }
        }
    }

    [System.Serializable]
    public class MaterialInventory
    {
        public List<MaterialStock> Materials;
        public Dictionary<string, MaterialStock> MaterialLookup;
        
        public MaterialInventory()
        {
            Materials = new List<MaterialStock>();
            MaterialLookup = new Dictionary<string, MaterialStock>();
        }
        
        public bool HasMaterial(string materialType, float quantity)
        {
            return MaterialLookup.ContainsKey(materialType) && 
                   MaterialLookup[materialType].AvailableQuantity >= quantity;
        }
        
        public bool HasMaterials(List<string> requiredMaterials)
        {
            foreach (var material in requiredMaterials)
            {
                if (!MaterialLookup.ContainsKey(material) || MaterialLookup[material].AvailableQuantity <= 0)
                {
                    return false;
                }
            }
            return true;
        }
        
        public bool HasMaterials(List<MaterialRequirement> requiredMaterials)
        {
            foreach (var material in requiredMaterials)
            {
                if (!HasMaterial(material.MaterialName, material.RequiredQuantity))
                {
                    return false;
                }
            }
            return true;
        }
        
        public void AddMaterial(string materialType, float quantity, float costPerUnit)
        {
            if (MaterialLookup.ContainsKey(materialType))
            {
                MaterialLookup[materialType].AvailableQuantity += quantity;
            }
            else
            {
                var stock = new MaterialStock
                {
                    MaterialType = materialType,
                    AvailableQuantity = quantity,
                    CostPerUnit = costPerUnit,
                    LastUpdated = System.DateTime.Now
                };
                Materials.Add(stock);
                MaterialLookup[materialType] = stock;
            }
        }
        
        public bool ConsumeMaterial(string materialType, float quantity)
        {
            if (HasMaterial(materialType, quantity))
            {
                MaterialLookup[materialType].AvailableQuantity -= quantity;
                return true;
            }
            return false;
        }
        
        public MaterialStock GetMaterialData(string materialType)
        {
            return MaterialLookup.ContainsKey(materialType) ? MaterialLookup[materialType] : null;
        }
    }

    [System.Serializable]
    public class MaterialStock
    {
        public string MaterialType;
        public float AvailableQuantity;
        public float CostPerUnit;
        public DateTime LastUpdated;
        public string Supplier;
        public DateTime ExpirationDate;
        public string StorageLocation;
    }

    [System.Serializable]
    public class EquipmentPool
    {
        public List<ConstructionEquipment> Equipment;
        public Dictionary<string, ConstructionEquipment> EquipmentLookup;
        
        public EquipmentPool()
        {
            Equipment = new List<ConstructionEquipment>();
            EquipmentLookup = new Dictionary<string, ConstructionEquipment>();
        }
        
        public List<ConstructionEquipment> GetAvailableEquipment(string equipmentType)
        {
            return Equipment.Where(e => e.EquipmentType == equipmentType && e.IsAvailable).ToList();
        }
        
        public bool ReserveEquipment(string equipmentId, string projectId)
        {
            if (EquipmentLookup.ContainsKey(equipmentId) && EquipmentLookup[equipmentId].IsAvailable)
            {
                EquipmentLookup[equipmentId].IsAvailable = false;
                EquipmentLookup[equipmentId].CurrentProjectId = projectId;
                return true;
            }
            return false;
        }
    }

    [System.Serializable]
    public class ConstructionEquipment
    {
        public string EquipmentId;
        public string EquipmentName;
        public string EquipmentType;
        public bool IsAvailable;
        public string CurrentProjectId;
        public float HourlyRate;
        public DateTime LastMaintenance;
        public DateTime NextMaintenance;
        public EquipmentCondition Condition;
    }

    [System.Serializable]
    public enum EquipmentCondition
    {
        Excellent,
        Good,
        Fair,
        Poor,
        NeedsRepair
    }

    [System.Serializable]
    public class ContractorManager
    {
        public List<Contractor> Contractors;
        public Dictionary<string, Contractor> ContractorLookup;
        
        public ContractorManager()
        {
            Contractors = new List<Contractor>();
            ContractorLookup = new Dictionary<string, Contractor>();
        }
        
        public List<Contractor> GetAvailableContractors(WorkerSpecialty specialty)
        {
            return Contractors.Where(c => c.Specialties.Contains(specialty) && c.IsAvailable).ToList();
        }
        
        public bool HireContractor(string contractorId, string projectId)
        {
            if (ContractorLookup.ContainsKey(contractorId) && ContractorLookup[contractorId].IsAvailable)
            {
                ContractorLookup[contractorId].IsAvailable = false;
                ContractorLookup[contractorId].CurrentProjectId = projectId;
                return true;
            }
            return false;
        }
    }

    [System.Serializable]
    public class Contractor
    {
        public string ContractorId;
        public string CompanyName;
        public string ContactName;
        public List<WorkerSpecialty> Specialties;
        public bool IsAvailable;
        public string CurrentProjectId;
        public float Rating; // 0-5
        public float HourlyRate;
        public List<string> Certifications;
        public bool IsLicensed;
        public bool IsInsured;
    }

    [System.Serializable]
    public enum RoomType
    {
        GrowRoom,
        ProcessingRoom,
        StorageRoom,
        OfficeSpace,
        LaboratoryRoom,
        SecurityRoom,
        UtilityRoom,
        MaintenanceRoom,
        ReceptionArea,
        ConferenceRoom
    }

    [System.Serializable]
    public class ProjectSummary
    {
        [Header("Project Information")]
        public string ProjectId;
        public string ProjectName;
        public ProjectStatus Status;
        public float Progress;
        
        [Header("Cost Information")]
        public float EstimatedCost;
        public float ActualCostToDate;
        public float CostVariance;
        
        [Header("Timeline Information")]
        public DateTime EstimatedCompletion;
        public DateTime ActualCompletion;
        public bool IsOnSchedule;
        
        [Header("Quality Metrics")]
        public float QualityRating;
        public int IssueCount;
        public List<string> CompletedMilestones;
    }
}