using UnityEngine;
using UnityEngine.UIElements;
using ProjectChimera.Core;
using ProjectChimera.Data.Events;
using ProjectChimera.Systems.Events;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ProjectChimera.UI.Panels
{
    /// <summary>
    /// UI Panel for displaying and managing random events in Project Chimera.
    /// Provides an engaging interface for players to view active events, make decisions,
    /// and track their event history. Creates dramatic moments and story engagement.
    /// </summary>
    public class RandomEventsPanel : ChimeraUIPanel
    {
        [Header("Event Panel Configuration")]
        [SerializeField] private bool _enableEventAnimations = true;
        [SerializeField] private bool _enableEventSounds = true;
        [SerializeField] private float _eventDisplayDuration = 8f;
        [SerializeField] private int _maxVisibleEvents = 5;
        
        [Header("Visual Settings")]
        [SerializeField] private bool _enableSeverityIndicators = true;
        [SerializeField] private bool _enableCategoryIcons = true;
        [SerializeField] private bool _enableUrgencyEffects = true;
        [SerializeField] private float _urgencyPulseSpeed = 2f;
        
        // System References
        private RandomEventManager _eventManager;
        
        // UI Elements
        private VisualElement _rootContainer;
        private VisualElement _eventsContainer;
        private VisualElement _eventHistoryContainer;
        private VisualElement _noEventsPlaceholder;
        private ScrollView _eventsScrollView;
        private ScrollView _historyScrollView;
        private Button _historyToggleButton;
        private Label _eventCountLabel;
        private Label _reputationLabel;
        
        // Tab System
        private VisualElement _tabContainer;
        private Button _activeEventsTab;
        private Button _eventHistoryTab;
        private VisualElement _activeEventsContent;
        private VisualElement _historyContent;
        
        // State Management
        private List<VisualElement> _activeEventElements = new List<VisualElement>();
        private Dictionary<string, VisualElement> _eventElementMap = new Dictionary<string, VisualElement>();
        private bool _isHistoryVisible = false;
        private float _lastUpdateTime = 0f;
        
        // Animation state
        private Dictionary<string, float> _elementAnimationStates = new Dictionary<string, float>();
        
        protected override void OnPanelInitialize()
        {
            base.OnPanelInitialize();
            
            // Find system references
            _eventManager = GameManager.Instance?.GetManager<RandomEventManager>();
            
            if (_eventManager == null)
            {
                LogError("RandomEventManager not found - UI disabled");
                return;
            }
            
            CreateUI();
            SubscribeToEvents();
            
            LogInfo("RandomEventsPanel initialized");
        }
        
        protected override void OnPanelUpdate()
        {
            base.OnPanelUpdate();
            
            if (_eventManager == null || _rootContainer == null) return;
            
            float currentTime = Time.time;
            
            // Update every second
            if (currentTime - _lastUpdateTime >= 1f)
            {
                RefreshEventDisplay();
                UpdateTimerDisplays();
                _lastUpdateTime = currentTime;
            }
            
            // Update animations
            if (_enableEventAnimations)
            {
                UpdateEventAnimations();
            }
        }
        
        private void CreateUI()
        {
            _rootContainer = new VisualElement();
            _rootContainer.name = "random-events-panel";
            _rootContainer.AddToClassList("random-events-panel");
            _rootContainer.style.width = new Length(100, LengthUnit.Percent);
            _rootContainer.style.height = new Length(100, LengthUnit.Percent);
            _rootContainer.style.padding = new StyleLength(20f);
            
            CreateHeader();
            CreateTabSystem();
            CreateActiveEventsView();
            CreateHistoryView();
            CreateNoEventsPlaceholder();
            
            Add(_rootContainer);
            
            // Show active events by default
            ShowActiveEventsTab();
        }
        
        private void CreateHeader()
        {
            var headerContainer = new VisualElement();
            headerContainer.name = "events-header";
            headerContainer.style.flexDirection = FlexDirection.Row;
            headerContainer.style.justifyContent = Justify.SpaceBetween;
            headerContainer.style.alignItems = Align.Center;
            headerContainer.style.marginBottom = 20f;
            headerContainer.style.paddingBottom = 15f;
            headerContainer.style.borderBottomWidth = 2f;
            headerContainer.style.borderBottomColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            
            // Title
            var titleLabel = new Label("Events & Opportunities");
            titleLabel.AddToClassList("panel-title");
            titleLabel.style.fontSize = 24f;
            titleLabel.style.color = new Color(0.9f, 0.7f, 0.2f, 1f);
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            
            // Status container
            var statusContainer = new VisualElement();
            statusContainer.style.flexDirection = FlexDirection.Row;
            statusContainer.style.alignItems = Align.Center;
            
            // Event count
            _eventCountLabel = new Label("0 Active");
            _eventCountLabel.style.fontSize = 14f;
            _eventCountLabel.style.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            _eventCountLabel.style.marginRight = 20f;
            
            // Reputation display
            _reputationLabel = new Label("Reputation: 50");
            _reputationLabel.style.fontSize = 14f;
            _reputationLabel.style.color = new Color(0.6f, 0.8f, 0.6f, 1f);
            
            statusContainer.Add(_eventCountLabel);
            statusContainer.Add(_reputationLabel);
            
            headerContainer.Add(titleLabel);
            headerContainer.Add(statusContainer);
            
            _rootContainer.Add(headerContainer);
        }
        
        private void CreateTabSystem()
        {
            _tabContainer = new VisualElement();
            _tabContainer.name = "tab-container";
            _tabContainer.style.flexDirection = FlexDirection.Row;
            _tabContainer.style.marginBottom = 15f;
            
            // Active Events Tab
            _activeEventsTab = new Button();
            _activeEventsTab.text = "🚨 Active Events";
            _activeEventsTab.name = "active-events-tab";
            _activeEventsTab.AddToClassList("tab-button");
            _activeEventsTab.style.padding = new StyleLength(10f);
            _activeEventsTab.style.marginRight = 5f;
            _activeEventsTab.style.backgroundColor = new Color(0.2f, 0.6f, 0.8f, 1f);
            _activeEventsTab.style.color = Color.white;
            _activeEventsTab.style.borderTopLeftRadius = 8f;
            _activeEventsTab.style.borderTopRightRadius = 8f;
            _activeEventsTab.style.borderBottomWidth = 0f;
            _activeEventsTab.clicked += ShowActiveEventsTab;
            
            // Event History Tab
            _eventHistoryTab = new Button();
            _eventHistoryTab.text = "📚 History";
            _eventHistoryTab.name = "event-history-tab";
            _eventHistoryTab.AddToClassList("tab-button");
            _eventHistoryTab.style.padding = new StyleLength(10f);
            _eventHistoryTab.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            _eventHistoryTab.style.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            _eventHistoryTab.style.borderTopLeftRadius = 8f;
            _eventHistoryTab.style.borderTopRightRadius = 8f;
            _eventHistoryTab.style.borderBottomWidth = 0f;
            _eventHistoryTab.clicked += ShowHistoryTab;
            
            _tabContainer.Add(_activeEventsTab);
            _tabContainer.Add(_eventHistoryTab);
            
            _rootContainer.Add(_tabContainer);
        }
        
        private void CreateActiveEventsView()
        {
            _activeEventsContent = new VisualElement();
            _activeEventsContent.name = "active-events-content";
            _activeEventsContent.style.flexGrow = 1f;
            
            _eventsScrollView = new ScrollView();
            _eventsScrollView.name = "events-scroll-view";
            _eventsScrollView.style.flexGrow = 1f;
            
            _eventsContainer = new VisualElement();
            _eventsContainer.name = "events-container";
            _eventsContainer.style.padding = new StyleLength(10f);
            
            _eventsScrollView.Add(_eventsContainer);
            _activeEventsContent.Add(_eventsScrollView);
            
            _rootContainer.Add(_activeEventsContent);
        }
        
        private void CreateHistoryView()
        {
            _historyContent = new VisualElement();
            _historyContent.name = "history-content";
            _historyContent.style.flexGrow = 1f;
            _historyContent.style.display = DisplayStyle.None;
            
            _historyScrollView = new ScrollView();
            _historyScrollView.name = "history-scroll-view";
            _historyScrollView.style.flexGrow = 1f;
            
            _eventHistoryContainer = new VisualElement();
            _eventHistoryContainer.name = "event-history-container";
            _eventHistoryContainer.style.padding = new StyleLength(10f);
            
            _historyScrollView.Add(_eventHistoryContainer);
            _historyContent.Add(_historyScrollView);
            
            _rootContainer.Add(_historyContent);
        }
        
        private void CreateNoEventsPlaceholder()
        {
            _noEventsPlaceholder = new VisualElement();
            _noEventsPlaceholder.name = "no-events-placeholder";
            _noEventsPlaceholder.style.alignItems = Align.Center;
            _noEventsPlaceholder.style.justifyContent = Justify.Center;
            _noEventsPlaceholder.style.flexGrow = 1f;
            _noEventsPlaceholder.style.display = DisplayStyle.None;
            
            var placeholderContainer = new VisualElement();
            placeholderContainer.style.alignItems = Align.Center;
            placeholderContainer.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            placeholderContainer.style.borderRadius = 12f;
            placeholderContainer.style.padding = new StyleLength(30f);
            placeholderContainer.style.maxWidth = 400f;
            
            var placeholderIcon = new Label("🌟");
            placeholderIcon.style.fontSize = 48f;
            placeholderIcon.style.unityTextAlign = TextAnchor.MiddleCenter;
            placeholderIcon.style.marginBottom = 15f;
            
            var placeholderTitle = new Label("All Quiet on the Farm");
            placeholderTitle.style.fontSize = 18f;
            placeholderTitle.style.color = new Color(0.8f, 0.6f, 0.2f, 1f);
            placeholderTitle.style.unityFontStyleAndWeight = FontStyle.Bold;
            placeholderTitle.style.unityTextAlign = TextAnchor.MiddleCenter;
            placeholderTitle.style.marginBottom = 10f;
            
            var placeholderText = new Label("No active events at the moment. Keep growing, and new opportunities will arise!");
            placeholderText.style.fontSize = 14f;
            placeholderText.style.color = new Color(0.7f, 0.7f, 0.7f, 1f);
            placeholderText.style.unityTextAlign = TextAnchor.MiddleCenter;
            placeholderText.style.whiteSpace = WhiteSpace.Normal;
            placeholderText.style.maxWidth = 300f;
            
            placeholderContainer.Add(placeholderIcon);
            placeholderContainer.Add(placeholderTitle);
            placeholderContainer.Add(placeholderText);
            
            _noEventsPlaceholder.Add(placeholderContainer);
            _activeEventsContent.Add(_noEventsPlaceholder);
        }
        
        private void ShowActiveEventsTab()
        {
            _isHistoryVisible = false;
            
            // Update tab appearances
            _activeEventsTab.style.backgroundColor = new Color(0.2f, 0.6f, 0.8f, 1f);
            _activeEventsTab.style.color = Color.white;
            _eventHistoryTab.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            _eventHistoryTab.style.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            
            // Show/hide content
            _activeEventsContent.style.display = DisplayStyle.Flex;
            _historyContent.style.display = DisplayStyle.None;
            
            RefreshEventDisplay();
        }
        
        private void ShowHistoryTab()
        {
            _isHistoryVisible = true;
            
            // Update tab appearances
            _eventHistoryTab.style.backgroundColor = new Color(0.2f, 0.6f, 0.8f, 1f);
            _eventHistoryTab.style.color = Color.white;
            _activeEventsTab.style.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1f);
            _activeEventsTab.style.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            
            // Show/hide content
            _historyContent.style.display = DisplayStyle.Flex;
            _activeEventsContent.style.display = DisplayStyle.None;
            
            RefreshHistoryDisplay();
        }
        
        private void RefreshEventDisplay()
        {
            if (_eventManager == null) return;
            
            var activeEvents = _eventManager.ActiveEvents;
            var eventDisplayData = _eventManager.GetEventDisplayData();
            
            // Update header labels
            _eventCountLabel.text = $"{activeEvents.Count} Active";
            _reputationLabel.text = $"Reputation: {_eventManager.PlayerReputationScore:F0}";
            
            // Clear existing events
            _eventsContainer.Clear();
            _activeEventElements.Clear();
            _eventElementMap.Clear();
            
            // Show placeholder if no events
            if (eventDisplayData.Count == 0)
            {
                _noEventsPlaceholder.style.display = DisplayStyle.Flex;
                return;
            }
            
            _noEventsPlaceholder.style.display = DisplayStyle.None;
            
            // Sort events by urgency and severity
            var sortedEvents = eventDisplayData
                .OrderByDescending(e => GetEventPriority(e))
                .Take(_maxVisibleEvents)
                .ToList();
            
            // Create event elements
            foreach (var eventData in sortedEvents)
            {
                var eventElement = CreateEventElement(eventData);
                _eventsContainer.Add(eventElement);
                _activeEventElements.Add(eventElement);
                _eventElementMap[eventData.EventId] = eventElement;
            }
        }
        
        private VisualElement CreateEventElement(EventDisplayData eventData)
        {
            var eventContainer = new VisualElement();
            eventContainer.name = $"event-{eventData.EventId}";
            eventContainer.AddToClassList("event-container");
            eventContainer.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            eventContainer.style.borderRadius = 12f;
            eventContainer.style.padding = new StyleLength(20f);
            eventContainer.style.marginBottom = 15f;
            eventContainer.style.borderLeftWidth = 4f;
            eventContainer.style.borderLeftColor = eventData.SeverityColor;
            
            // Add urgency glow for critical events
            if (eventData.Severity == EventSeverity.Critical && _enableUrgencyEffects)
            {
                eventContainer.AddToClassList("critical-event");
                _elementAnimationStates[eventData.EventId] = 0f;
            }
            
            CreateEventHeader(eventContainer, eventData);
            CreateEventDescription(eventContainer, eventData);
            CreateEventChoices(eventContainer, eventData);
            CreateEventTimer(eventContainer, eventData);
            
            return eventContainer;
        }
        
        private void CreateEventHeader(VisualElement container, EventDisplayData eventData)
        {
            var headerContainer = new VisualElement();
            headerContainer.style.flexDirection = FlexDirection.Row;
            headerContainer.style.justifyContent = Justify.SpaceBetween;
            headerContainer.style.alignItems = Align.Center;
            headerContainer.style.marginBottom = 12f;
            
            var titleContainer = new VisualElement();
            titleContainer.style.flexDirection = FlexDirection.Row;
            titleContainer.style.alignItems = Align.Center;
            
            // Category icon
            if (_enableCategoryIcons)
            {
                var categoryIcon = new Label(eventData.CategoryIcon);
                categoryIcon.style.fontSize = 20f;
                categoryIcon.style.marginRight = 8f;
                titleContainer.Add(categoryIcon);
            }
            
            // Event title
            var titleLabel = new Label(eventData.Title);
            titleLabel.style.fontSize = 16f;
            titleLabel.style.color = Color.white;
            titleLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            titleContainer.Add(titleLabel);
            
            // Severity indicator
            if (_enableSeverityIndicators)
            {
                var severityLabel = new Label(GetSeverityText(eventData.Severity));
                severityLabel.style.fontSize = 12f;
                severityLabel.style.color = eventData.SeverityColor;
                severityLabel.style.backgroundColor = new Color(eventData.SeverityColor.r, eventData.SeverityColor.g, eventData.SeverityColor.b, 0.2f);
                severityLabel.style.padding = new StyleLength(4f);
                severityLabel.style.borderRadius = 4f;
                headerContainer.Add(severityLabel);
            }
            
            headerContainer.Add(titleContainer);
            container.Add(headerContainer);
        }
        
        private void CreateEventDescription(VisualElement container, EventDisplayData eventData)
        {
            var descriptionLabel = new Label(eventData.Description);
            descriptionLabel.style.fontSize = 14f;
            descriptionLabel.style.color = new Color(0.9f, 0.9f, 0.9f, 1f);
            descriptionLabel.style.whiteSpace = WhiteSpace.Normal;
            descriptionLabel.style.marginBottom = 15f;
            
            container.Add(descriptionLabel);
            
            // Add story context if available
            if (!string.IsNullOrEmpty(eventData.StoryContext))
            {
                var contextLabel = new Label($"💭 {eventData.StoryContext}");
                contextLabel.style.fontSize = 12f;
                contextLabel.style.color = new Color(0.7f, 0.7f, 0.9f, 1f);
                contextLabel.style.fontStyle = FontStyle.Italic;
                contextLabel.style.marginBottom = 10f;
                container.Add(contextLabel);
            }
        }
        
        private void CreateEventChoices(VisualElement container, EventDisplayData eventData)
        {
            var choicesContainer = new VisualElement();
            choicesContainer.style.marginBottom = 15f;
            
            for (int i = 0; i < eventData.Choices.Count; i++)
            {
                var choiceIndex = i;
                var choiceText = eventData.Choices[i];
                
                var choiceButton = new Button();
                choiceButton.text = choiceText;
                choiceButton.name = $"choice-{choiceIndex}";
                choiceButton.style.padding = new StyleLength(12f);
                choiceButton.style.marginBottom = 8f;
                choiceButton.style.backgroundColor = new Color(0.2f, 0.5f, 0.7f, 0.8f);
                choiceButton.style.color = Color.white;
                choiceButton.style.borderRadius = 6f;
                choiceButton.style.borderTopWidth = 0f;
                choiceButton.style.borderRightWidth = 0f;
                choiceButton.style.borderBottomWidth = 0f;
                choiceButton.style.borderLeftWidth = 0f;
                choiceButton.style.whiteSpace = WhiteSpace.Normal;
                
                // Add hover effect
                choiceButton.RegisterCallback<MouseEnterEvent>(evt =>
                {
                    choiceButton.style.backgroundColor = new Color(0.3f, 0.6f, 0.8f, 1f);
                });
                
                choiceButton.RegisterCallback<MouseLeaveEvent>(evt =>
                {
                    choiceButton.style.backgroundColor = new Color(0.2f, 0.5f, 0.7f, 0.8f);
                });
                
                choiceButton.clicked += () => OnEventChoiceSelected(eventData.EventId, choiceIndex);
                
                choicesContainer.Add(choiceButton);
            }
            
            container.Add(choicesContainer);
        }
        
        private void CreateEventTimer(VisualElement container, EventDisplayData eventData)
        {
            if (!eventData.HasTimeLimit) return;
            
            var timerContainer = new VisualElement();
            timerContainer.name = "timer-container";
            timerContainer.style.flexDirection = FlexDirection.Row;
            timerContainer.style.alignItems = Align.Center;
            timerContainer.style.justifyContent = Justify.Center;
            timerContainer.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
            timerContainer.style.padding = new StyleLength(8f);
            timerContainer.style.borderRadius = 6f;
            
            var timerIcon = new Label("⏰");
            timerIcon.style.fontSize = 14f;
            timerIcon.style.marginRight = 5f;
            
            var timerLabel = new Label(FormatTimeRemaining(eventData.TimeRemaining));
            timerLabel.name = "timer-label";
            timerLabel.style.fontSize = 12f;
            timerLabel.style.color = GetTimerColor(eventData.TimeRemaining);
            timerLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            
            timerContainer.Add(timerIcon);
            timerContainer.Add(timerLabel);
            container.Add(timerContainer);
        }
        
        private void RefreshHistoryDisplay()
        {
            // TODO: Implement event history display
            _eventHistoryContainer.Clear();
            
            var placeholderLabel = new Label("Event history coming soon...");
            placeholderLabel.style.fontSize = 16f;
            placeholderLabel.style.color = new Color(0.7f, 0.7f, 0.7f, 1f);
            placeholderLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            placeholderLabel.style.marginTop = 50f;
            
            _eventHistoryContainer.Add(placeholderLabel);
        }
        
        private void UpdateTimerDisplays()
        {
            foreach (var kvp in _eventElementMap)
            {
                var eventElement = kvp.Value;
                var timerLabel = eventElement.Q<Label>("timer-label");
                
                if (timerLabel != null)
                {
                    var eventData = _eventManager.GetEventDisplayData()
                        .FirstOrDefault(e => e.EventId == kvp.Key);
                    
                    if (eventData != null && eventData.HasTimeLimit)
                    {
                        timerLabel.text = FormatTimeRemaining(eventData.TimeRemaining);
                        timerLabel.style.color = GetTimerColor(eventData.TimeRemaining);
                    }
                }
            }
        }
        
        private void UpdateEventAnimations()
        {
            foreach (var kvp in _elementAnimationStates.ToList())
            {
                var eventId = kvp.Key;
                var animationTime = kvp.Value;
                
                if (_eventElementMap.TryGetValue(eventId, out var element))
                {
                    // Pulse effect for critical events
                    float pulseValue = (Mathf.Sin(animationTime * _urgencyPulseSpeed) + 1f) * 0.5f;
                    float glowIntensity = 0.1f + (pulseValue * 0.3f);
                    
                    element.style.borderLeftColor = new Color(1f, 0.2f, 0.2f, 0.8f + (pulseValue * 0.2f));
                    
                    _elementAnimationStates[eventId] = animationTime + Time.deltaTime;
                }
                else
                {
                    _elementAnimationStates.Remove(eventId);
                }
            }
        }
        
        private void OnEventChoiceSelected(string eventId, int choiceIndex)
        {
            _eventManager?.MakeEventDecision(eventId, choiceIndex);
            
            // Add visual feedback
            if (_eventElementMap.TryGetValue(eventId, out var eventElement))
            {
                // Fade out the event element
                var fadeContainer = new VisualElement();
                fadeContainer.style.position = Position.Absolute;
                fadeContainer.style.left = 0;
                fadeContainer.style.top = 0;
                fadeContainer.style.right = 0;
                fadeContainer.style.bottom = 0;
                fadeContainer.style.backgroundColor = new Color(0f, 0.5f, 0f, 0.3f);
                fadeContainer.style.borderRadius = 12f;
                
                var confirmLabel = new Label("✓ Decision Made");
                confirmLabel.style.color = new Color(0.2f, 0.8f, 0.2f, 1f);
                confirmLabel.style.fontSize = 14f;
                confirmLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                confirmLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                confirmLabel.style.alignSelf = Align.Center;
                confirmLabel.style.marginTop = 50f;
                
                fadeContainer.Add(confirmLabel);
                eventElement.Add(fadeContainer);
                
                // Refresh display after a short delay
                this.schedule.Execute(() => RefreshEventDisplay()).ExecuteLater(1000);
            }
        }
        
        private void SubscribeToEvents()
        {
            if (_eventManager != null)
            {
                _eventManager.OnEventStarted += OnNewEventTriggered;
                _eventManager.OnEventResolved += OnEventResolved;
                _eventManager.OnReputationChanged += OnReputationChanged;
            }
        }
        
        private void OnNewEventTriggered(ActiveRandomEvent newEvent)
        {
            RefreshEventDisplay();
            
            // Show notification or play sound
            if (_enableEventSounds)
            {
                // Would play event notification sound
                LogInfo($"🔔 New Event: {newEvent.Title}");
            }
        }
        
        private void OnEventResolved(ActiveRandomEvent resolvedEvent, EventChoice choice)
        {
            RefreshEventDisplay();
            LogInfo($"✅ Event Resolved: {resolvedEvent.Title}");
        }
        
        private void OnReputationChanged(float newReputation)
        {
            if (_reputationLabel != null)
            {
                _reputationLabel.text = $"Reputation: {newReputation:F0}";
            }
        }
        
        private float GetEventPriority(EventDisplayData eventData)
        {
            float priority = 0f;
            
            // Severity priority
            priority += eventData.Severity switch
            {
                EventSeverity.Critical => 100f,
                EventSeverity.High => 80f,
                EventSeverity.Medium => 60f,
                EventSeverity.Low => 40f,
                EventSeverity.Positive => 50f,
                _ => 30f
            };
            
            // Time urgency
            if (eventData.HasTimeLimit)
            {
                float hoursRemaining = (float)eventData.TimeRemaining.TotalHours;
                if (hoursRemaining < 1f) priority += 50f;
                else if (hoursRemaining < 6f) priority += 30f;
                else if (hoursRemaining < 24f) priority += 10f;
            }
            
            return priority;
        }
        
        private string GetSeverityText(EventSeverity severity)
        {
            return severity switch
            {
                EventSeverity.Critical => "CRITICAL",
                EventSeverity.High => "URGENT",
                EventSeverity.Medium => "IMPORTANT",
                EventSeverity.Low => "MINOR",
                EventSeverity.Positive => "OPPORTUNITY",
                _ => "EVENT"
            };
        }
        
        private string FormatTimeRemaining(TimeSpan timeRemaining)
        {
            if (timeRemaining.TotalDays >= 1)
                return $"{timeRemaining.Days}d {timeRemaining.Hours}h";
            else if (timeRemaining.TotalHours >= 1)
                return $"{timeRemaining.Hours}h {timeRemaining.Minutes}m";
            else
                return $"{timeRemaining.Minutes}m {timeRemaining.Seconds}s";
        }
        
        private Color GetTimerColor(TimeSpan timeRemaining)
        {
            if (timeRemaining.TotalHours < 1)
                return new Color(1f, 0.2f, 0.2f, 1f); // Red - Very urgent
            else if (timeRemaining.TotalHours < 6)
                return new Color(1f, 0.6f, 0.2f, 1f); // Orange - Urgent
            else
                return new Color(0.8f, 0.8f, 0.8f, 1f); // Gray - Normal
        }
        
        protected override void OnPanelDestroy()
        {
            // Unsubscribe from events
            if (_eventManager != null)
            {
                _eventManager.OnEventStarted -= OnNewEventTriggered;
                _eventManager.OnEventResolved -= OnEventResolved;
                _eventManager.OnReputationChanged -= OnReputationChanged;
            }
            
            base.OnPanelDestroy();
        }
    }
}