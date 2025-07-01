using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectChimera.Core;
using ProjectChimera.Core.Events;
using ProjectChimera.Core.Interfaces;

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

    // Missing types that are causing CS0234 errors - these are construction-specific versions
    [System.Serializable]
    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Medium,
        Hard,
        Expert
    }

    [System.Serializable]
    public enum ProjectType
    {
        Residential,
        Commercial,
        Industrial,
        Agricultural,
        Research,
        Mixed,
        GrowRoom,
        ProcessingFacility,
        Greenhouse,
        Laboratory
    }

    [System.Serializable]
    public enum ChallengeType
    {
        TimeTrial,
        Budget,
        Quality,
        Efficiency,
        Innovation,
        Safety,
        SpaceOptimization
    }

    // Additional missing types for collaborative construction
    [System.Serializable]
    public enum CollaborativeSessionStatus
    {
        Active,
        Paused,
        Completed,
        Cancelled
    }

    [System.Serializable]
    public enum ShareDataBetweenSessions
    {
        None,
        BasicData,
        FullData
    }

    [System.Serializable]
    public enum ConflictResolutionStrategy
    {
        Vote,
        Authority,
        Consensus,
        Automatic
    }

    [System.Serializable]
    public enum ParticipantRole
    {
        Lead,
        Architect,
        Engineer,
        Designer,
        Reviewer,
        Observer
    }

    [System.Serializable]
    public enum InnovationType
    {
        Design,
        Material,
        Process,
        Technology,
        Sustainability,
        Safety,
        Efficiency
    }

    // Core Building and Construction Classes
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
        public string Name; // Changed from RoomName to Name for consistency
        public string RoomName; // Keep old property for backward compatibility
        public RoomType RoomType; // Changed from string to enum
        public Vector3 Position;
        public Vector3 Dimensions;
        public float Area;
        public List<DoorSpecification> Doors;
        public List<WindowSpecification> Windows;
        public UtilityRequirements Utilities;
        public EnvironmentalRequirements Environmental;
        public List<string> SpecialFeatures;
        
        // Property to maintain backward compatibility
        public string Name_Compat 
        { 
            get => Name ?? RoomName; 
            set { Name = value; RoomName = value; } 
        }
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
        // Added for compatibility with older references if needed
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
        // Added for compatibility
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
        public string TaskId; // Ensure TaskId is present
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
    
    // Helper Enums
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
        Design_Change,
        Equipment,
        Other
    }

    [System.Serializable]
    public enum SafetyComplianceLevel
    {
        Unknown,
        NonCompliant,
        PartiallyCompliant,
        Compliant,
        ExceedsStandards
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
    public enum ProjectPriority
    {
        Low,
        Medium,
        High,
        Critical,
        Emergency
    }

    [System.Serializable]
    public enum DependencyType
    {
        Finish_to_Start,
        Start_to_Start,
        Finish_to_Finish,
        Start_to_Finish
    }
    
    // Consolidated from ConstructionGamingDataStructures
    // DifficultyLevel and ChallengeType are now defined in ProjectChimera.Core
    // These enums are imported via the Core assembly reference

    [System.Serializable]
    public enum ChallengeStatus
    {
        NotStarted,
        InProgress,
        Active, // Added missing value
        Completed,
        Failed,
        Archived,
        Expired, // Added missing value for environmental challenges
        TimedOut // Added missing value
    }

    // New Data Structures for Advanced Systems
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
        public Vector2 Dimensions;
        public float TotalArea;
        public List<ConstructionRoomTemplate> RoomTemplates;
        public List<UtilityConnection> UtilityConnections;

        [Header("System Requirements")]
        public float RequiredHVACCapacity; // in tons
        public float RequiredPowerCapacity; // in watts

        [Header("Construction Requirements")]
        public List<MaterialSpecification> RequiredMaterials;
        public List<PermitType> RequiredPermits;
        public List<WorkerSpecialty> RequiredSpecialties;
        public int EstimatedConstructionDays;

        [Header("Cost Estimates")]
        public float BaseConstructionCost;
        public float EstimatedMaterialCost;
        public float EstimatedLaborCost;
        public float EstimatedPermitCost;
        public float EstimatedTotalCost;

        [Header("Compliance and Standards")]
        public List<string> BuildingCodes;
        public List<string> SafetyStandards;
        public bool CannabisSuitability;
        public EnvironmentalSpecifications EnvironmentalSpecs;

        public List<ConstructionRoomTemplate> Rooms => RoomTemplates;
    }

    [System.Serializable]
    public class ConstructionRoomTemplate
    {
        [Header("Room Information")]
        public string TemplateRoomId;
        public string RoomName;
        public string RoomType;
        public string Description;

        [Header("Dimensions")]
        public Vector2 Dimensions;
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

        public bool IsGrowRoom => RoomType == "GrowRoom";
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
        public float EstimatedCost;
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
        public ProjectType ProjectType; // Added missing property
        public Vector3 BuildingSite;
        public Vector3 Position;
        public FacilityTemplate FacilityTemplate;
        public Blueprint3D Blueprint; // Added missing property

        [Header("Status and Progress")]
        public ProjectStatus Status;
        public ConstructionPhase CurrentPhase;
        public List<ConstructionPhase> CompletedPhases;
        public List<ConstructionPhaseData> PhaseDetails;
        public float OverallProgress; // 0-1
        public List<string> CompletedTasks;

        [Header("Workforce")]
        public List<WorkerAssignment> AssignedWorkers;

        [Header("Dates")]
        public DateTime CreatedDate; // Added missing property
        public DateTime StartDate;
        public DateTime EstimatedCompletionDate;
        public DateTime ActualCompletionDate;
        public DateTime CompletionDate;

        [Header("Cost and Budget")]
        public float TotalBudget;
        public float EstimatedCost;
        public float ActualCost;
        public float RemainingBudget;

        [Header("Duration")]
        public int EstimatedDuration; // days
        public int ActualDuration; // days

        [Header("Permits and Validation")]
        public List<string> RequiredPermits = new List<string>(); // Added missing property
        public List<PermitApplication> Permits;
        public bool PermitsApproved;
        public ValidationResult ValidationResults;

        [Header("Planning")]
        public ConstructionSchedule Schedule;
        public List<ConstructionTask> Tasks;
        public List<PlannedRoom> PlannedRooms = new List<PlannedRoom>(); // Added missing property for room planning

        [Header("Issues and Quality")]
        public List<ConstructionIssue> Issues;
        public QualityMetrics QualityMetrics;

        [Header("Gaming Features")]
        public ConstructionGamingFeatures GamingFeatures; // Added missing property

        [Header("Collaboration")]
        public DateTime CreationTime; // Added missing property for collaborative projects
        public List<string> Participants = new List<string>(); // Added missing property for collaborative projects

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
        public bool EnableConstruction;
        public bool EnforceZoningLaws;
        public bool RequirePermits;
        public float ConstructionSpeedMultiplier;

        [Header("Design Tools")]
        public bool UseDesignTool;
        public bool ShowConstructionGuides;
        public bool ValidateRealTime;
        public GridSnapSettings GridSnapSettings;

        [Header("Building Constraints")]
        public Vector2 MaxBuildingSize;
        public Vector3 MaxRoomSize;
        public Vector3 MinRoomSize;  // Added missing property
        public float WallThickness;
        public bool RequireFoundation;
        public bool EnforceFireSafety;
        public bool RequireVentilation;
        public float MaxBuildingHeight;
        public float MinSetbackDistance;
        public int RequiredParkingSpaces;
        public float MaxLotCoverage;

        [Header("Economic Settings")]
        public float DemolitionCostPerSqFt;
        public float LaborCostPerHour;
        public float MaterialMarkup;

        [Header("Quality Control")]
        public bool EnableQualitySystem;
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
        public ConstructionProject Project;
        public DateTime StartTime;
        public float Progress; // 0-1 (alias for CompletionPercentage)
        public float CompletionPercentage; // 0-1
        public TaskStatus Status;
        public DateTime CompletionTime;
        public List<ConstructionWorker> AssignedWorkers;
        public float EstimatedCost;
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
        public float MoraleImpact;
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

        public DateTime SubmissionDate;
        public int EstimatedProcessingDays;
        public float ApplicationFee;
        public bool Submitted => Status != PermitStatus.Not_Applied;

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

        [Header("Financial Metrics")]
        public float TotalSpent;
        public float AverageCostPerSqFt;
        public float AverageCostOverrun;

        [Header("Quality Metrics")]
        public float AverageQualityScore;
        public int TotalDefects;
        public int TotalRework;
        public float CustomerSatisfactionRating;

        [Header("Efficiency Metrics")]
        public float AverageProjectDuration;
        public float MaterialWastePercentage;
        public float ScheduleAdherence;
        
        // Additional properties referenced in error messages
        public float AverageCompletionTime;
        public float WorkerProductivity;

        public List<ConstructionWorker> GetActiveWorkers()
        {
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
        public float FinancialImpact;
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
        public string ReportTitle;
        public ConstructionMetrics TotalMetrics;
        public List<string> KeyFindings;
        public List<string> Recommendations;

        [Header("Details")]
        public List<ConstructionProject> Projects;
        public List<ConstructionIssue> OpenIssues;
        public List<ConstructionIssue> ResolvedIssues;
        public List<ConstructionWorker> Workforce;
        public List<ProjectSummary> ProjectSummaries = new List<ProjectSummary>();
    }

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
        Idle,
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
    
    // Core Data Structures
    [System.Serializable]
    public enum SkillLevel
    {
        Beginner = 0,
        Novice = 1,
        Apprentice = 2,
        Intermediate = 3,
        Skilled = 4,
        Advanced = 5,
        Expert = 6,
        Master = 7
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
        public float EfficiencyMultiplier; // 1.0 is normal
        public float ProductivityModifier; // Added missing property
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

    // Design & Planning
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
            PreviewObjects = new List<GameObject>();
        }

        public void StartDesign(FacilityTemplate template)
        {
            IsDesigning = true;
            CurrentTemplate = template;
            // Logic to enter design mode
        }

        public void EndDesign()
        {
            IsDesigning = false;
            ClearPreviews();
            // Logic to exit design mode
        }

        public GameObject CreateRoomPreview(ConstructionRoomTemplate roomTemplate, Vector3 position, Quaternion rotation)
        {
            // Logic to create a visual preview of a room
            return new GameObject("RoomPreview");
        }

        private void ClearPreviews()
        {
            foreach (var preview in PreviewObjects)
            {
                GameObject.Destroy(preview);
            }
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
            var result = new ValidationResult { IsValid = true, Errors = new List<string>() };
            // Example validation logic
            if (buildingSite.y < 0)
            {
                result.IsValid = false;
                result.Errors.Add("Building site cannot be underground.");
            }
            return result;
        }

        public bool ValidateRoomPlacement(ConstructionRoomTemplate roomTemplate, Vector3 position, Quaternion rotation)
        {
            // Placeholder for room placement validation
            return true;
        }
    }
    
    // Workforce & Materials
    [System.Serializable]
    public class ConstructionPlanner
    {
        public List<ConstructionTask> CreateTasksForPhase(ConstructionProject project, ConstructionPhase phase)
        {
            // Logic to generate tasks based on phase and template
            var tasks = new List<ConstructionTask>();
            // Example:
            tasks.Add(new ConstructionTask { TaskId = "TASK-001", TaskName = "Clear Site" });
            return tasks;
        }

        public List<ConstructionTask> GetTasksForPhase(ConstructionProject project, ConstructionPhase phase)
        {
            return project.Tasks.Where(t => t.ConstructionPhase == phase).ToList();
        }

        public ConstructionSchedule CreateProjectSchedule(ConstructionProject project)
        {
            var schedule = new ConstructionSchedule
            {
                ScheduleId = $"SCH-{project.ProjectId}",
                ProjectStartDate = project.StartDate,
                ProjectEndDate = project.EstimatedCompletionDate,
                Tasks = new List<ConstructionTask>(),
                Dependencies = new List<TaskDependency>(),
                Milestones = new List<ScheduleMilestone>()
            };
            // Logic to populate schedule
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
            return Workers.Where(w => w.IsAvailable && w.Specialty == specialty).ToList();
        }

        public List<ConstructionWorker> GetActiveWorkers()
        {
            return Workers.Where(w => !w.IsAvailable).ToList();
        }
        
        public ConstructionWorker GetWorker(string workerId)
        {
            return WorkerLookup.ContainsKey(workerId) ? WorkerLookup[workerId] : null;
        }

        public List<ConstructionWorker> AssignWorkers(ConstructionTask task, int workerCount)
        {
            var availableWorkers = GetAvailableWorkers(task.RequiredSpecialty).Take(workerCount).ToList();
            foreach (var worker in availableWorkers)
            {
                worker.IsAvailable = false;
                worker.CurrentProjectId = task.ProjectId;
                worker.CurrentTaskId = task.TaskId;
            }
            return availableWorkers;
        }

        public List<ConstructionWorker> AssignWorkers(WorkerSpecialty specialty, int workerCount)
        {
            var availableWorkers = GetAvailableWorkers(specialty).Take(workerCount).ToList();
            foreach (var worker in availableWorkers)
            {
                worker.IsAvailable = false;
            }
            return availableWorkers;
        }

        public void ReleaseWorkers(List<ConstructionWorker> workers)
        {
            foreach (var worker in workers)
            {
                var w = GetWorker(worker.WorkerId);
                if (w != null)
                {
                    w.IsAvailable = true;
                    w.CurrentProjectId = null;
                    w.CurrentTaskId = null;
                }
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
            return MaterialLookup.ContainsKey(materialType) && MaterialLookup[materialType].AvailableQuantity >= quantity;
        }
        
        public bool HasMaterials(List<string> requiredMaterials)
        {
            foreach (var mat in requiredMaterials)
            {
                if (!MaterialLookup.ContainsKey(mat)) return false;
            }
            return true;
        }
        
        public bool HasMaterials(List<MaterialRequirement> requiredMaterials)
        {
            foreach (var req in requiredMaterials)
            {
                if (!HasMaterial(req.MaterialName, req.RequiredQuantity)) return false;
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
                var newStock = new MaterialStock { MaterialType = materialType, AvailableQuantity = quantity, CostPerUnit = costPerUnit };
                Materials.Add(newStock);
                MaterialLookup[materialType] = newStock;
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
            return Equipment.Where(e => e.IsAvailable && e.EquipmentType == equipmentType).ToList();
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
            return Contractors.Where(c => c.IsAvailable && c.Specialties.Contains(specialty)).ToList();
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
    
    // UI-Specific Data Structures
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
        public float QualityScore;
        public int IssueCount;
        public List<string> CompletedMilestones;
    }
    
    // ConstraintType enum is defined earlier in this file around line 1710 to avoid duplicates
    
    [System.Serializable]
    public enum ConstraintType
    {
        Budget,
        Time,
        Space,
        Material,
        Labor,
        Permit,
        Safety,
        Environmental,
        Quality,
        Zoning,
        Area,        // Added for ConstructionChallengeEngine
        Code,        // Added for ConstructionChallengeEngine
        Accessibility // Added for ConstructionChallengeEngine
    }
    
    [System.Serializable]
    public enum ObjectiveType
    {
        Efficiency,
        Quality,
        Safety,
        Cost,
        Innovation,
        Compliance,
        Performance,
        Sustainability,
        Minimize,    // Added for ConstructionChallengeEngine
        Maximize,    // Added for ConstructionChallengeEngine
        Achieve,     // Added for ConstructionChallengeEngine
        Optimize,    // Added for ConstructionChallengeEngine
        Include,     // Added for ConstructionChallengeEngine
        Balance,     // Added missing value
        Exclude      // Added missing value
    }
    
    [System.Serializable]
    public enum ObjectiveCategory
    {
        Efficiency,
        Cost,
        Safety,
        Sustainability,
        Quality,
        Innovation,
        Compliance
    }
    
    [System.Serializable]
    public enum HintType
    {
        Technical,
        Creative,
        Regulatory,
        Economic,
        Practical
    }
    
    // ProjectType is now defined in ProjectChimera.Core
    // This enum is imported via the Core assembly reference
    
    [System.Serializable]
    public class ArchitecturalChallenge
    {
        public string ChallengeId;
        public string Title;
        public string Description;
        public ChallengeType Type;
        public DifficultyLevel Difficulty;
        public float ComplexityModifier;
        public float RewardMultiplier;
        public ChallengeStatus Status;
        public string CreatedBy;
        public List<string> Tags = new List<string>();
        public List<ChallengeConstraint> Constraints = new List<ChallengeConstraint>();
        public List<ChallengeObjective> Objectives = new List<ChallengeObjective>();
        public Blueprint3D BaseDesign;
        public ChallengeMetrics TargetMetrics;
        public System.TimeSpan TimeLimit;
        public bool HasTimeLimit;
        public DateTime StartTime;
        public float PassingScore;
        public List<ChallengeHint> Hints = new List<ChallengeHint>();
        public ChallengeRewards Rewards;
        public DateTime CompletionTime;
        public ChallengeResult Result;
    }
    
    [System.Serializable]
    public class ChallengeResult
    {
        public string ResultId;
        public string ChallengeId;
        public DateTime SubmissionTime;
        public ArchitecturalChallenge Challenge;
        public PlayerProgressProfile PlayerProfile; // Added missing property
        public Dictionary<string, float> ObjectiveScores = new Dictionary<string, float>();
        public List<string> AchievedObjectives = new List<string>();
        public List<string> FailedObjectives = new List<string>();
        public float OverallScore;
        public bool IsSuccessful;
        public bool HasBreakthrough;
        public int ExperienceGained;
        public List<string> UnlockedFeatures = new List<string>();
        public System.TimeSpan CompletionTime;
    }
    
    // DesignSolution class is defined earlier in this file around line 1696 to avoid duplicates
    
    [System.Serializable]
    public class Blueprint3D
    {
        public string BlueprintId;
        public string Name;
        public string Description;
        public Vector3 Dimensions;
        public float TotalArea; // Added missing property
        public List<BuildingComponent> Components = new List<BuildingComponent>();
        public List<Room> Rooms = new List<Room>();
        public List<SystemLayout> Systems = new List<SystemLayout>();
        
        // Conversion methods to resolve type conflicts
        public static Blueprint3D FromBuildingBlueprint(BuildingBlueprint buildingBlueprint)
        {
            if (buildingBlueprint == null) return null;
            
            return new Blueprint3D
            {
                BlueprintId = buildingBlueprint.BlueprintId,
                Name = buildingBlueprint.Name,
                Description = buildingBlueprint.Description,
                Dimensions = buildingBlueprint.Dimensions,
                TotalArea = buildingBlueprint.TotalArea,
                Components = new List<BuildingComponent>(),
                Rooms = buildingBlueprint.Rooms?.Select(r => new Room 
                { 
                    RoomId = r.RoomId, 
                    Name = r.Name ?? r.RoomName, 
                    RoomType = r.RoomType 
                }).ToList() ?? new List<Room>(),
                Systems = new List<SystemLayout>()
            };
        }
        
        public BuildingBlueprint ToBuildingBlueprint()
        {
            return new BuildingBlueprint
            {
                BlueprintId = this.BlueprintId,
                Name = this.Name,
                Description = this.Description,
                Dimensions = this.Dimensions,
                TotalArea = this.TotalArea,
                Rooms = this.Rooms?.Select(r => new RoomLayout 
                { 
                    RoomId = r.RoomId, 
                    Name = r.Name,
                    RoomName = r.Name, // Set both for compatibility
                    RoomType = r.RoomType 
                }).ToList() ?? new List<RoomLayout>(),
                MaterialSpecs = new List<MaterialSpecification>(),
                EquipmentSpecs = new List<EquipmentSpecification>()
            };
        }
    }
    
    [System.Serializable]
    public class BuildingComponent
    {
        public string ComponentId;
        public string Name;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public BuildingMaterial Material;
        public float Cost;
    }
    
    [System.Serializable]
    public class Room
    {
        public string RoomId;
        public string Name;
        public RoomType RoomType;
        public Vector3 Position;
        public Vector3 Size;
        public List<string> ComponentIds = new List<string>();
    }
    
    [System.Serializable]
    public class SystemLayout
    {
        public string SystemId;
        public string Name;
        public string Type;
        public List<Vector3> Connections = new List<Vector3>();
        public Dictionary<string, object> Properties = new Dictionary<string, object>();
    }
    
    [System.Serializable]
    public class ChallengeConstraint
    {
        public string ConstraintId;
        public string Name;
        public ConstraintType Type;
        public float Value;
        public string Description;
        public bool IsHardConstraint;
    }
    
    [System.Serializable]
    public class ChallengeObjective
    {
        public string ObjectiveId;
        public string Name;
        public ObjectiveType Type;
        public ObjectiveCategory Category;
        public float TargetValue;
        public float Weight;
        public string Description;
    }
    
    [System.Serializable]
    public class ChallengeMetrics
    {
        public float SpaceEfficiency;
        public float EnergyEfficiency;
        public float CostEffectiveness;
        public float SafetyScore;
        public float SustainabilityScore;
    }
    
    [System.Serializable]
    public class ChallengeHint
    {
        public string HintId;
        public string Text;
        public ChallengeObjective RelatedObjective;
        public int CostToUnlock;
        public bool IsUnlocked;
        public HintType Type;
    }
    
    [System.Serializable]
    public class ChallengeRewards
    {
        public int ExperiencePoints;
        public int CurrencyReward;
        public List<string> UnlockedFeatures = new List<string>();
        public List<string> UnlockedBlueprints = new List<string>();
        public List<Achievement> Achievements = new List<Achievement>();
        public List<CertificationCredit> CertificationCredits = new List<CertificationCredit>();
    }
    
    [System.Serializable]
    public class Achievement
    {
        public string AchievementId;
        public string Name;
        public string Title;
        public string Description;
        public string IconPath;
        public DateTime UnlockedDate;
        public string Category; // Added missing property
    }
    
    [System.Serializable]
    public class ChallengeObjectives
    {
        public List<ChallengeObjective> Primary = new List<ChallengeObjective>();
        public List<ChallengeObjective> Secondary = new List<ChallengeObjective>();
        public List<ChallengeObjective> Bonus = new List<ChallengeObjective>();
    }

    [System.Serializable]
    public class CertificationCredit
    {
        public string CreditId;
        public string Name;
        public string CertificationBody;
        public float CreditValue;
        public DateTime ExpirationDate;
    }
    
    #region Missing Gaming System Data Structures
    
    [System.Serializable]
    public class CompetitionEntry
    {
        public string EntryId;
        public string CompetitionId;
        public string PlayerId;
        public string PlayerName;
        public Blueprint3D SubmittedDesign;
        public DateTime SubmissionTime;
        public float Score;
        public int Rank;
        public List<string> JudgeComments = new List<string>();
        public bool IsWinner;
        public ChallengeRewards Rewards;
        
        // Additional properties referenced in error messages
        public DateTime SubmissionDate;
        public object Status;
    }
    
    [System.Serializable]
    public class CertificationPath
    {
        public string PathId;
        public string CertificationId; // Added missing property
        public string PathName;
        public string Title;
        public string Description;
        public List<string> RequiredChallenges = new List<string>();
        public List<string> RequiredProjects = new List<string>();
        public int MinExperiencePoints;
        public float MinOverallScore;
        public List<CertificationCredit> RequiredCredits = new List<CertificationCredit>();
        public TimeSpan Duration;
        public List<CertificationModule> Modules = new List<CertificationModule>();
        public List<string> Prerequisites = new List<string>();
        public string Industry;
        public CertificationLevel Level;
        public bool IsCompleted;
        public DateTime CompletionDate;
        
        // Additional properties and methods referenced in error messages
        public string Progress;
        public EnrollmentStatus Status;
        
        // Method that accepts 2 parameters as shown in error
        public float CalculateActivityProgress(object param1, object param2)
        {
            return 0.0f; // Placeholder implementation
        }
    }
    
    [System.Serializable]
    public class ConstructionGamingMetrics
    {
        public int TotalChallengesAttempted;
        public int TotalChallengesCompleted;
        public float AverageScore;
        public int TotalExperiencePoints;
        public int ProjectsCompleted;
        public float AverageProjectRating;
        public List<string> UnlockedFeatures = new List<string>();
        public List<string> EarnedCertifications = new List<string>();
        public Dictionary<string, float> SkillRatings = new Dictionary<string, float>();
        public DateTime LastActivity;
        public int ActiveChallenges;
        public int ActiveCollaborations;
        public int TotalPlayers;
        public DateTime LastUpdated;
    }
    
    [System.Serializable]
    public class PlayerProgressProfile
    {
        public string PlayerId;
        public string PlayerName;
        public int Level;
        public int ExperiencePoints;
        public ConstructionGamingMetrics GamingMetrics;
        public List<string> CompletedChallenges = new List<string>();
        public List<string> CompletedProjects = new List<string>();
        public List<string> UnlockedBlueprints = new List<string>();
        public Dictionary<string, float> SkillLevels = new Dictionary<string, float>();
        public List<CertificationEnrollment> ActiveCertifications = new List<CertificationEnrollment>(); // Added missing property
        public DateTime LastLoginDate;
        public float TotalPlayTimeHours;
        
        // Additional properties referenced in error messages
        public SkillLevel SkillLevel;
        public List<string> UnlockedFeatures = new List<string>();
        public List<Achievement> Achievements = new List<Achievement>();
    }
    
    [System.Serializable]
    public class ArchitecturalInnovation
    {
        public string InnovationId;
        public string Name;
        public string Description;
        public string DiscoveredBy;
        public DateTime DiscoveryDate;
        public float EfficiencyGain;
        public float CostSavings;
        public List<string> RequiredTechnologies = new List<string>();
        public bool IsPatented;
        public float MarketValue;
        public string Title;
        public InnovationType Type;
        public float ImpactScore; // Added missing property
    }
    
    #endregion

    // Collaborative Construction Classes
    [System.Serializable]
    public class CollaborativeSession
    {
        public string SessionId;
        public string SessionName;
        public string Description;
        public SessionStatus Status;
        public List<SessionParticipant> Participants = new List<SessionParticipant>();
        public List<CollaborationEvent> EventHistory = new List<CollaborationEvent>();
        public ConstructionProject Project;
        public DateTime StartTime;
        public DateTime? EndTime;
        public CollaborationSettings Settings;
        public SharedResources SharedResources;
        public Dictionary<string, object> SessionData = new Dictionary<string, object>();
        public CommunicationChannels Communication;
        public ConflictResolutionSystem ConflictResolution;
    }

    [System.Serializable]
    public class SessionParticipant
    {
        public string ParticipantId;
        public string PlayerId;
        public string PlayerName;
        public ParticipantRole Role;
        public List<Permission> Permissions = new List<Permission>();
        public DateTime JoinTime;
        public DateTime? LeaveTime;
        public ParticipantStatus Status;
        public CollaborationMetrics Metrics = new CollaborationMetrics();
        public List<string> Contributions = new List<string>();
    }

    [System.Serializable]
    public class CollaborationEvent
    {
        public string EventId;
        public string ParticipantId;
        public EventType EventType;
        public DateTime Timestamp;
        public string Description;
        public List<string> AffectedParticipants = new List<string>();
    }

    [System.Serializable]
    public class CollaborationSettings
    {
        public int MaxParticipants = 10;
        public bool AllowSpectators = true;
        public bool EnableVoiceChat = true;
        public bool EnableTextChat = true;
        public bool EnableScreenSharing = false;
        public bool RequirePermissionForChanges = true;
        public bool EnableRealTimeSync = true;
        public float SyncInterval = 0.1f;
        public ConflictResolutionMode ConflictResolution = ConflictResolutionMode.Voting;
    }

    [System.Serializable]
    public class SharedResources
    {
        public SharedBudget Budget;
        public SharedMaterialPool Materials;
        public SharedEquipmentPool Equipment;
        public SharedKnowledgeBase Knowledge;
        public SharedBlueprintLibrary Blueprints;
    }

    [System.Serializable]
    public class CommunicationChannels
    {
        public bool VoiceChatEnabled = true;
        public bool TextChatEnabled = true;
        public bool VideoEnabled = false;
        public List<ChatMessage> ChatHistory = new List<ChatMessage>();
        public List<VoiceSession> VoiceSessions = new List<VoiceSession>();
        public object ScreenSharing;
    }

    [System.Serializable]
    public class ConflictResolutionSystem
    {
        public ConflictResolutionMode Mode = ConflictResolutionMode.Voting;
        public List<ActiveConflict> ActiveConflicts = new List<ActiveConflict>();
        public List<ResolvedConflict> ResolvedConflicts = new List<ResolvedConflict>();
        public ConflictResolutionRules Rules = new ConflictResolutionRules();
    }

    [System.Serializable]
    public class CollaborationMetrics
    {
        public int TotalActions;
        public int AcceptedActions;
        public int RejectedActions;
        public float ContributionScore;
        public DateTime LastActivity;
    }

    [System.Serializable]
    public class Permission
    {
        public string Name;
        public bool Allowed;
        public string Description;
    }

    // Supporting Enums
    [System.Serializable]
    public enum SessionStatus
    {
        Waiting,
        Active,
        Paused,
        Completed,
        Cancelled
    }

    [System.Serializable]
    public enum ParticipantStatus
    {
        Online,
        Away,
        Offline
    }

    [System.Serializable]
    public enum EventType
    {
        Join,
        Leave,
        Action,
        Message,
        Conflict,
        Resolution
    }

    [System.Serializable]
    public enum ConflictResolutionMode
    {
        Voting,
        Authority,
        Consensus,
        Automatic
    }

    // Placeholder classes for referenced types
    [System.Serializable] public class SharedBudget { }
    [System.Serializable] public class SharedMaterialPool { }
    [System.Serializable] public class SharedEquipmentPool { }
    [System.Serializable] public class SharedKnowledgeBase { }
    [System.Serializable] 
    public class SharedBlueprintLibrary { }
    
    [System.Serializable]
    public class SharedBlueprint
    {
        public string SharedId;
        public string OriginalBlueprintId;
        public SharingParameters SharingParameters;
        public DateTime ShareDate;
        public List<string> Downloads = new List<string>();
        public List<BlueprintRating> Ratings = new List<BlueprintRating>();
        public List<string> Tags = new List<string>();
        
        // Additional properties mentioned in error messages
        public string SharedBlueprintId;
        public SharingParameters SharingParametersValues;
        public DateTime SharedDate;
        public int DownloadCount;
        public List<BlueprintRating> BlueprintRatings = new List<BlueprintRating>();
        
    }
    
    [System.Serializable]
    public class BlueprintRating
    {
        public string RatingId;
        public string UserId;
        public float Rating; // 1-5 stars
        public string Review;
        public DateTime RatingDate;
    }
    [System.Serializable] public class ChatMessage { }
    [System.Serializable] public class VoiceSession { }
    [System.Serializable] public class ActiveConflict { }
    [System.Serializable] public class ResolvedConflict { }
    [System.Serializable] public class ConflictResolutionRules { }

    // Building Project Management
    [System.Serializable]
    public class BuildingProject
    {
        public string ProjectId;
        public string Name;
        public string ProjectName; // Added missing property (settable)
        public string Description;
        public ProjectType Type;
        public BuildingType BuildingType; // Added missing property
        public ProjectStatus Status;
        public Blueprint3D Blueprint;
        public Vector3 Location;
        public Vector3 PlannedLocation; // Added missing property
        public Vector3 PlannedDimensions; // Added missing property
        public float PlannedArea; // Added missing property
        public BuildingQuality QualityLevel; // Added missing property
        public float TotalBudget;
        public float RemainingBudget; // Added missing property
        public float SpentBudget;
        public float SpentAmount; // Added missing property (settable)
        public DateTime StartDate;
        public DateTime ProjectStartDate; // Added missing property (settable)
        public DateTime EstimatedCompletionDate;
        public DateTime ActualCompletionDate;
        public List<ConstructionStage> CompletedStages = new List<ConstructionStage>();
        public ConstructionStage CurrentStage;
        public List<string> Requirements = new List<string>(); // Added missing property
        public List<MaterialRequirement> MaterialRequirements = new List<MaterialRequirement>(); // Changed to MaterialRequirement objects
        public List<string> RequiredPermits = new List<string>();
        public List<string> ObtainedPermits = new List<string>();
        public List<ConstructionPermit> Permits = new List<ConstructionPermit>(); // List of actual permit objects
        public List<WorkerAssignment> AssignedWorkers = new List<WorkerAssignment>();
        public List<MaterialOrder> MaterialOrders = new List<MaterialOrder>();
        public List<ProjectMilestone> Milestones = new List<ProjectMilestone>();
        public List<ConstructionIssue> Issues = new List<ConstructionIssue>();
        public float Progress; // 0.0 to 1.0
        public float OverallProgress; // Added missing property (settable)
        public string ContractorId;
        public string ClientId;
        
        // Added missing properties for Error Wave 67
        public ProjectPriority Priority = ProjectPriority.Medium; // Added missing property
        public List<string> CompletedMilestones = new List<string>(); // Added missing property
        public bool IsOnSchedule = true; // Added missing property
        public bool IsOnBudget = true; // Added missing property
        public Dictionary<string, float> QualityScores = new Dictionary<string, float>(); // Added missing property
        public List<QualityInspection> QualityInspections = new List<QualityInspection>(); // Added missing property
        public float QualityRating = 0.0f; // Added missing property
        
        // Additional missing properties from error messages  
        public List<WorkerAssignment> WorkerAssignments = new List<WorkerAssignment>(); // Added missing property
        public List<ConstructionInspection> Inspections = new List<ConstructionInspection>(); // Added missing property
        public int EstimatedDays; // Added missing property
        
        public Dictionary<string, object> CustomProperties = new Dictionary<string, object>();
    }

    [System.Serializable]
    public class MaterialOrder
    {
        public string OrderId;
        public string MaterialName;
        public float Quantity;
        public string Unit;
        public float UnitCost;
        public float TotalCost;
        public DateTime OrderDate;
        public DateTime DeliveryDate;
        public OrderStatus Status;
    }

    [System.Serializable]
    public enum OrderStatus
    {
        Pending,
        Ordered,
        InTransit,
        Delivered,
        Cancelled
    }

    [System.Serializable]
    public class ProjectMilestone
    {
        [Header("Milestone Information")]
        public string MilestoneId;
        public string Name;
        public string Description;
        public DateTime TargetDate;
        public DateTime ActualDate;
        public bool IsCompleted;
        public List<string> Requirements = new List<string>();
    }

    // Missing types causing CS0246 errors
    [System.Serializable]
    public class ArchitecturalBreakthrough
    {
        public string BreakthroughId;
        public string Name;
        public string Title;
        public string Description;
        public string Type;
        public string DiscoveredBy;
        public DateTime DiscoveryDate;
        public float ImpactScore;
        public List<string> AffectedSystems = new List<string>();
        public bool IsPatentable;
        public float CommercialValue;
    }

    [System.Serializable]
    public class CompetitionResult
    {
        public string ResultId;
        public string CompetitionId;
        public string CompetitionTitle;
        public string PlayerId;
        public string PlayerName;
        public float Score;
        public int Rank;
        public DateTime CompletionTime;
        public List<string> Achievements = new List<string>();
        public bool IsWinner;
        public ChallengeRewards Rewards;
        public List<CompetitionWinner> Winners = new List<CompetitionWinner>();
    }

    [System.Serializable]
    public class CompetitionWinner
    {
        public string PlayerId;
        public string PlayerName;
        public string CompetitionId;
        public int Rank;
        public float Score;
        public string Category;
        public string Placement;
        public int ExperienceReward;
        public List<string> Achievements = new List<string>();
        public Dictionary<string, object> Rewards = new Dictionary<string, object>();
        public System.DateTime WinDate;
    }

    [System.Serializable]
    public class CertificationAchievement
    {
        public string AchievementId;
        public string CertificationId;
        public string PlayerId;
        public string CertificationName;
        public string IssuingBody;
        public DateTime EarnedDate;
        public DateTime CompletionDate;
        public DateTime ExpirationDate;
        public List<string> RequiredSkills = new List<string>();
        public float SkillLevel;
        public CertificationGrade Grade;
        public bool IsValid;
    }
    
    // Missing types from CS0246 and CS0117 errors
    [System.Serializable]
    public class ConstructionEducationSystem
    {
        public string SystemId;
        public string Name;
        public List<CertificationPath> CertificationPaths = new List<CertificationPath>();
        
        public void Initialize() { }
        public void LoadCertificationPrograms() { }
        public void LoadEducationalContent() { }
        public void StartCertificationTracking(CertificationEnrollment enrollment) { }
        public float CalculateActivityProgress(CertificationEnrollment enrollment, ConstructionActivity activity) => 0.0f;
    }
    
    // Missing classes for competition and analytics
    [System.Serializable]
    public class ArchitecturalCompetitionManager
    {
        public string ManagerId;
        public List<object> Competitions = new List<object>();
        
        public void Initialize() { }
        public void LoadCompetitionTemplates() { }
        public void SetupRewardSystem() { }
        public void UpdateActiveCompetitions() { }
        public object GetCompetition(string competitionId) => null;
        public void AddCompetitionEntry(object competition, CompetitionEntry entry) { }
        public void EvaluateEntry(object competition, CompetitionEntry entry) { }
        public CompetitionResult EvaluateCompetition(object competition) => new CompetitionResult();
    }
    
    
    [System.Serializable]
    public class OptimizationEngine
    {
        public string EngineId;
        
        public OptimizationSuggestions GenerateOptimizationSuggestions(Blueprint3D blueprint, OptimizationGoals goals) => new OptimizationSuggestions();
        public DesignRecommendations GenerateDesignRecommendations(DesignContext context) => new DesignRecommendations();
        public object ValidateOptimization(object input) => null;
    }
    
    [System.Serializable]
    public class StructuralEngineeringSystem
    {
        public string SystemId;
        
        public OptimizationSuggestions ValidateOptimizations(OptimizationSuggestions suggestions) => suggestions;
    }
    
    [System.Serializable]
    public class PerformanceAnalyticsEngine
    {
        public string EngineId;
        
        public void Initialize() { }
        public void StartDataCollection() { }
        public void UpdateAnalytics() { }
        public void Shutdown() { }
        public PerformanceAnalytics GeneratePerformanceReport(string timeframe) => new PerformanceAnalytics();
        public PlayerPerformanceMetrics GeneratePlayerMetrics(PlayerProgressProfile profile) => new PlayerPerformanceMetrics();
        public void StartChallengeTracking(ArchitecturalChallenge challenge) { }
        public void RecordChallengeSubmission(ArchitecturalChallenge challenge, DesignSolution solution, ChallengeResult result) { }
        public void RecordAIAssistantUsage(DesignContext context, DesignRecommendations recommendations) { }
        public void RecordCollaborativeAction(CollaborativeSession session, string playerId, CollaborativeAction action) { }
    }
    
    [System.Serializable]
    public class PlayerPerformanceMetrics
    {
        public string PlayerId;
        public Dictionary<string, float> Metrics = new Dictionary<string, float>();
        
        public object SkillLevel => 0.5f;
    }
    
    // Additional missing classes for analytics
    [System.Serializable]
    public class PerformanceReport
    {
        public string ReportId;
        public DateTime GeneratedDate;
        public Dictionary<string, float> Metrics = new Dictionary<string, float>();
    }
    
    [System.Serializable]
    public class PlayerMetrics  
    {
        public string PlayerId;
        public Dictionary<string, float> Skills = new Dictionary<string, float>();
        public Dictionary<string, int> Achievements = new Dictionary<string, int>();
    }
    
    // Data structures for construction gaming and optimization systems
    // These are defined here in Data assembly to avoid circular dependencies
    
    /// <summary>
    /// Optimization suggestions for construction projects
    /// </summary>
    [System.Serializable]
    public class OptimizationSuggestions
    {
        public string SuggestionId;
        public List<string> Recommendations = new List<string>();
        public float PotentialSavings;
        public float EfficiencyGain;
        public string Category;
        public int Priority;
        public System.DateTime Generated;
    }

    /// <summary>
    /// Design recommendations data structure for AI-assisted construction
    /// </summary>
    [System.Serializable]
    public class DesignRecommendations
    {
        public string RecommendationId;
        public List<string> StructuralSuggestions = new List<string>();
        public List<string> EfficiencyImprovements = new List<string>();
        public List<string> CostOptimizations = new List<string>();
        public List<string> SafetyEnhancements = new List<string>();
        public float ConfidenceScore;
        public System.DateTime GeneratedTime;
        public Dictionary<string, object> AdditionalData = new Dictionary<string, object>();
    }

    /// <summary>
    /// Performance analytics for construction systems
    /// </summary>
    [System.Serializable]
    public class PerformanceAnalytics
    {
        public string AnalyticsId;
        public float ConstructionSpeed;
        public float QualityScore;
        public float EfficiencyRating;
        public Dictionary<string, float> Metrics = new Dictionary<string, float>();
        public System.DateTime LastUpdate;
    }
    
    
    
    public enum EnrollmentStatus
    {
        Active,
        Completed, 
        Failed,
        Suspended,
        Withdrawn
    }

    [System.Serializable]
    public enum CertificationLevel
    {
        Beginner,
        Basic,
        Intermediate,
        Advanced,
        Expert,
        Master
    }

    [System.Serializable]
    public enum CertificationModuleType
    {
        Theory,
        Practical,
        Assessment,
        Project,
        Workshop
    }
    
    [System.Serializable]
    public class CertificationEnrollment
    {
        public string EnrollmentId;
        public string UserId;
        public string CertificationId;
        public DateTime EnrollmentDate;
        public EnrollmentStatus Status;
        public float Progress;
        public float CurrentScore;
        
        // Additional properties referenced in error messages
        public string PlayerId;
        public List<string> CompletedModules = new List<string>();
        public CertificationPath CertificationPath;
        public string Title;
        public List<string> UnlockedFeatures = new List<string>();
        public List<Achievement> Achievements = new List<Achievement>();
        public DateTime CompletionDate;
    }

    [System.Serializable]
    public class CertificationModule
    {
        public string ModuleId;
        public string Title;
        public string Description;
        public List<string> LearningObjectives = new List<string>();
        public int EstimatedHours;
        public bool IsRequired;
        public List<string> Prerequisites = new List<string>();
        public CertificationModuleType Type;
    }

    [System.Serializable]
    public class ProjectCreationRequest
    {
        public string RequestId;
        public string ProjectName;
        public string Description;
        public ProjectType Type;
        public ProjectType ProjectType => Type; // Added missing property alias
        public Blueprint3D Blueprint; // Added missing property
        public Vector3 Location;
        public float Budget;
        public DateTime RequestedStartDate;
        public string RequestedBy;
        public List<string> Requirements = new List<string>();
        public bool EnableChallenges = true; // Added missing property
        public bool EnableCollaboration = false; // Added missing property
        public bool EnableCompetitions = false; // Added missing property
        public bool EnableEducation = false; // Added missing property
        public DifficultyLevel DifficultyLevel = DifficultyLevel.Medium; // Added missing property
    }

    [System.Serializable]
    public class SharingParameters
    {
        public bool ShareProgress = true;
        public bool ShareResources = false;
        public bool ShareBlueprints = false;
        public bool ShareMetrics = true;
        public ShareDataBetweenSessions DataSharing = ShareDataBetweenSessions.BasicData;
        public List<string> Tags = new List<string>(); // Added missing property
    }

    [System.Serializable]
    public class ConstructionActivity
    {
        public string ActivityId;
        public string Name;
        public string Description;
        public DateTime StartTime;
        public DateTime EndTime;
        public string PerformedBy;
        public ConstructionStage Stage;
        public float Progress;
        public List<string> ResourcesUsed = new List<string>();
    }

    [System.Serializable]
    public class DesignContext
    {
        public string ContextId;
        public string Name;
        public BuildingType BuildingType;
        public Vector3 SiteConstraints;
        public List<string> Requirements = new List<string>();
        public EnvironmentalSpecifications Environmental;
        public float BudgetConstraint;
        public DateTime DeadlineConstraint;
    }

    // Additional missing enums
    [System.Serializable]
    public enum ActionType
    {
        PlaceComponent,
        ModifyComponent,
        DeleteComponent,
        AddComment,
        ShareResource,
        UpdateProgress,
        RequestHelp
    }

    // Collaborative action data structure
    [System.Serializable]
    public class CollaborativeAction
    {
        public string ActionId;
        public ActionType ActionType;
        public string PerformedBy;
        public DateTime Timestamp;
        public Dictionary<string, object> Parameters = new Dictionary<string, object>();
        public string Description;
        public bool RequiresApproval;
    }

    // Collaborative project configuration
    [System.Serializable]
    public class ParticipantInfo
    {
        public string PlayerId;
        public string PlayerName;
        public ParticipantRole? Role;
        public SkillLevel SkillLevel;
        public List<string> Specializations = new List<string>();
    }

    [System.Serializable]
    public class CollaborativeProjectConfig
    {
        public string ProjectName;
        public string Description;
        public List<ParticipantInfo> Participants = new List<ParticipantInfo>();
        public CollaborationSettings Settings;
        public ResourcePool ResourcePool;
        public ProjectType Type;
        public float Budget;
        public DateTime StartDate;
    }

    [System.Serializable]
    public class ResourcePool
    {
        public float Budget;
        public Dictionary<string, float> Materials = new Dictionary<string, float>();
        public List<string> AvailableEquipment = new List<string>();
        public int MaxWorkers;
    }

    [System.Serializable]
    public class ConstructionGamingFeatures
    {
        public bool EnableChallenges = true;
        public bool EnableCollaboration = false;
        public bool EnableCompetitions = false;
        public bool EnableLeaderboards = false;
        public bool EnableAchievements = true;
        public bool EnableProgressTracking = true;
        public bool EnableSkillProgression = true;
        public bool EnableRewards = true;
        public bool EnableEducation = true; // Added missing property
        public DifficultyLevel DifficultyLevel = DifficultyLevel.Medium; // Added missing property
    }

    // Additional missing system classes
    [System.Serializable]
    public class ArchitecturalDesignEngine
    {
        public string EngineId;
        public void Initialize() { }
    }
    
    [System.Serializable]
    public class ConstructionSimulationEngine
    {
        public string EngineId;
        public void Initialize() { }
    }
    
    [System.Serializable]
    public class RealTimeConstructionEngine
    {
        public string EngineId;
        public void Initialize() { }
        public void UpdateConstruction(float deltaTime) { }
    }
    
    [System.Serializable]
    public class ConstructionChallengeEngine
    {
        public string EngineId;
        
        public void Initialize() { }
        public void LoadChallengeTemplates() { }
        public void SetComplexityScale(float scale) { }
        public void SetRewardMultiplier(float multiplier) { }
        public void UpdateChallenge(ArchitecturalChallenge challenge) { }
        public ArchitecturalChallenge GenerateChallenge(ChallengeType type, DifficultyLevel difficulty, ChallengeParameters parameters) => new ArchitecturalChallenge();
        public ChallengeResult EvaluateChallengeSolution(ArchitecturalChallenge challenge, DesignSolution solution) => new ChallengeResult();
    }
    
    [System.Serializable]
    public class CollaborativeConstructionSystem
    {
        public string SystemId;
        
        public void Initialize() { }
        public void SetMaxCollaborators(int max) { }
        public void SetSyncInterval(float interval) { }
        public void UpdateSession(CollaborativeSession session) { }
        public void ProcessCollaborativeAction(string playerId, CollaborativeAction action) { }
        public void Shutdown() { }
        public CollaborativeSession StartCollaborativeProject(CollaborativeProjectConfig config) => new CollaborativeSession();
    }
    
    [System.Serializable]
    public class ConstructionMiniGameSystem
    {
        public string SystemId;
        public void Initialize() { }
    }
    
    [System.Serializable]
    public class AIDesignAssistant
    {
        public string AssistantId;
        public void Initialize() { }
        public void Update() { }
        public DesignRecommendations GenerateDesignRecommendations(DesignContext context) => new DesignRecommendations();
    }
    
    [System.Serializable]
    public class InnovationDetectionSystem
    {
        public string SystemId;
        public void Initialize() { }
        public void UpdateInnovationDetection() { }
        public ArchitecturalBreakthrough AnalyzeForBreakthrough(DesignSolution solution, ArchitecturalChallenge challenge) => new ArchitecturalBreakthrough();
    }
    
    [System.Serializable]
    public class ChallengeParameters
    {
        public string ParameterId;
        public Dictionary<string, object> Parameters = new Dictionary<string, object>();
        public List<ChallengeConstraint> Constraints = new List<ChallengeConstraint>();
        public List<ChallengeObjective> Objectives = new List<ChallengeObjective>();
        
        // Additional properties expected by tests
        public ProjectType ProjectType = ProjectType.GrowRoom;
        public Vector3 SiteSize = new Vector3(10f, 3f, 8f);
        public float BudgetLimit = 50000f;
        public List<string> RequiredFeatures = new List<string>();
        public DifficultyLevel Difficulty = DifficultyLevel.Normal;
        public float TimeLimit = 24f; // hours
        public bool AllowCollaboration = false;
        public string Description = "";
    }
    
} // End namespace ProjectChimera.Data.Construction