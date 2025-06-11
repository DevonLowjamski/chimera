using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

namespace ProjectChimera.Data.Tutorial
{
    /// <summary>
    /// Core tutorial step data structure
    /// </summary>
    [Serializable]
    public class TutorialStepData
    {
        public string StepId;
        public string Title;
        public string Description;
        public string DetailedInstructions;
        public TutorialStepType StepType;
        public TutorialTargetType TargetType;
        public string TargetElementId;
        public Vector2 HighlightOffset;
        public Vector2 HighlightSize;
        public TutorialHighlightShape HighlightShape;
        public TutorialValidationType ValidationType;
        public string ValidationTarget;
        public float TimeoutDuration;
        public bool IsOptional;
        public bool CanSkip;
        public List<string> Prerequisites;
        public List<TutorialHint> Hints;
        public AudioClip NarrationClip;
        public Sprite IllustrationImage;
    }
    
    /// <summary>
    /// Tutorial sequence definition
    /// </summary>
    [Serializable]
    public class TutorialSequenceData
    {
        public string SequenceId;
        public string Name;
        public string Description;
        public TutorialCategory Category;
        public int Priority;
        public bool IsRequired;
        public List<string> StepIds;
        public List<string> UnlockRequirements;
        public TutorialReward CompletionReward;
    }
    
    /// <summary>
    /// Tutorial hint system
    /// </summary>
    [Serializable]
    public class TutorialHint
    {
        public string HintText;
        public float DelayBeforeShow;
        public TutorialHintType HintType;
        public string TargetElement;
        public bool IsContextual;
    }
    
    /// <summary>
    /// Tutorial completion reward
    /// </summary>
    [Serializable]
    public class TutorialReward
    {
        public TutorialRewardType RewardType;
        public int Amount;
        public string ItemId;
        public string Description;
    }
    
    /// <summary>
    /// Tutorial progress tracking
    /// </summary>
    [Serializable]
    public class TutorialProgress
    {
        public string TutorialId;
        public string CurrentStepId;
        public TutorialStatus Status;
        public float ProgressPercentage;
        public DateTime StartTime;
        public DateTime LastUpdateTime;
        public int CompletedSteps;
        public int TotalSteps;
        public bool IsCompleted;
        public bool WasSkipped;
        public List<string> CompletedStepIds;
        public Dictionary<string, object> CustomData;
    }
    
    /// <summary>
    /// Tutorial session statistics
    /// </summary>
    [Serializable]
    public class TutorialSessionStats
    {
        public string SessionId;
        public DateTime SessionStart;
        public DateTime SessionEnd;
        public float TotalDuration;
        public int StepsCompleted;
        public int HintsUsed;
        public int StepsSkipped;
        public bool SessionCompleted;
        public TutorialDifficulty PerceivedDifficulty;
        public List<TutorialInteraction> Interactions;
    }
    
    /// <summary>
    /// Tutorial interaction tracking
    /// </summary>
    [Serializable]
    public class TutorialInteraction
    {
        public string StepId;
        public TutorialInteractionType InteractionType;
        public DateTime Timestamp;
        public Vector2 Position;
        public string TargetElement;
        public float Duration;
        public bool WasSuccessful;
    }
    
    /// <summary>
    /// Tutorial validation result
    /// </summary>
    public struct TutorialValidationResult
    {
        public bool IsValid;
        public string ErrorMessage;
        public string NextStepId;
        public TutorialValidationFeedback Feedback;
        public object ValidationData;
    }
    
    /// <summary>
    /// Tutorial event data
    /// </summary>
    [Serializable]
    public struct TutorialEventData
    {
        public string TutorialId;
        public string StepId;
        public TutorialEventType EventType;
        public string Message;
        public DateTime Timestamp;
        public object AdditionalData;
    }
    
    // Enumerations
    
    /// <summary>
    /// Tutorial step types
    /// </summary>
    public enum TutorialStepType
    {
        Introduction,
        Instruction,
        Interaction,
        Validation,
        Information,
        Choice,
        Completion,
        Transition,
        Problem_Solving,
        Assessment,
        Project
    }
    
    /// <summary>
    /// Tutorial target types for highlighting
    /// </summary>
    public enum TutorialTargetType
    {
        None,
        UIElement,
        GameObject,
        ScreenArea,
        Menu,
        Button,
        Panel,
        Input,
        Custom
    }
    
    /// <summary>
    /// Tutorial highlight shapes
    /// </summary>
    public enum TutorialHighlightShape
    {
        Rectangle,
        Circle,
        Oval,
        RoundedRectangle,
        Polygon,
        Custom
    }
    
    /// <summary>
    /// Tutorial validation types
    /// </summary>
    public enum TutorialValidationType
    {
        None,
        ButtonClick,
        InputValue,
        StateChange,
        Timer,
        Custom,
        SystemEvent,
        ManagerState,
        UIInteraction
    }
    
    /// <summary>
    /// Tutorial categories
    /// </summary>
    public enum TutorialCategory
    {
        Onboarding,
        BasicCultivation,
        AdvancedCultivation,
        Genetics,
        Economics,
        FacilityManagement,
        Research,
        Advanced,
        Tips
    }
    
    /// <summary>
    /// Tutorial status
    /// </summary>
    public enum TutorialStatus
    {
        NotStarted,
        InProgress,
        Paused,
        Completed,
        Skipped,
        Failed,
        Abandoned
    }
    
    /// <summary>
    /// Tutorial hint types
    /// </summary>
    public enum TutorialHintType
    {
        Text,
        Arrow,
        Highlight,
        Animation,
        Audio,
        Popup
    }
    
    /// <summary>
    /// Tutorial reward types
    /// </summary>
    public enum TutorialRewardType
    {
        Experience,
        Currency,
        Item,
        Unlock,
        Achievement,
        Skill
    }
    
    /// <summary>
    /// Tutorial difficulty levels
    /// </summary>
    public enum TutorialDifficulty
    {
        VeryEasy = 1,
        Easy = 2,
        Normal = 3,
        Hard = 4,
        VeryHard = 5
    }
    
    /// <summary>
    /// Tutorial difficulty levels (alternative naming)
    /// </summary>
    public enum TutorialDifficultyLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }
    
    /// <summary>
    /// Tutorial interaction types
    /// </summary>
    public enum TutorialInteractionType
    {
        Click,
        Hover,
        Drag,
        Input,
        KeyPress,
        Scroll,
        Navigation,
        Custom
    }
    
    /// <summary>
    /// Tutorial validation feedback
    /// </summary>
    public enum TutorialValidationFeedback
    {
        Success,
        Failure,
        Partial,
        Retry,
        Hint,
        Skip
    }
    
    /// <summary>
    /// Tutorial event types
    /// </summary>
    public enum TutorialEventType
    {
        Started,
        StepCompleted,
        StepSkipped,
        HintShown,
        ValidationFailed,
        Completed,
        Abandoned,
        Paused,
        Resumed
    }
    
    /// <summary>
    /// Tutorial configuration settings
    /// </summary>
    [Serializable]
    public class TutorialSettings
    {
        public bool EnableTutorials = true;
        public bool AllowSkipping = true;
        public bool ShowHints = true;
        public float HintDelay = 5f;
        public bool EnableNarration = true;
        public float NarrationVolume = 0.8f;
        public bool EnableSoundEffects = true;
        public float SoundEffectVolume = 0.7f;
        public bool HighlightTargets = true;
        public Color HighlightColor = Color.yellow;
        public float HighlightPulseSpeed = 2f;
        public bool DimBackground = true;
        public float BackgroundDimAmount = 0.7f;
        public bool PauseGameDuringTutorial = false;
        public TutorialSpeed DefaultSpeed = TutorialSpeed.Normal;
        public bool SaveProgress = true;
        public bool TrackAnalytics = true;
    }
    
    /// <summary>
    /// Tutorial speed settings
    /// </summary>
    public enum TutorialSpeed
    {
        Slow,
        Normal,
        Fast,
        Instant
    }
}