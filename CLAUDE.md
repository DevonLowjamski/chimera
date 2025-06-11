# Project Chimera - Claude Development Context

## Project Overview
Project Chimera is a comprehensive cannabis cultivation simulation game built in Unity 6000.2.0b2. The project features a sophisticated ScriptableObject-driven architecture with manager-based systems for handling cultivation, genetics, economics, and facility management.

## Current Status: Unity 6 Migration Complete ✅
- **Unity Version**: 6000.2.0b2 (Unity 6 Beta)
- **Render Pipeline**: Universal Render Pipeline (URP) 17.2.0  
- **All Core Systems**: Fully operational and tested
- **Test Success Rate**: 100% (5/5 core systems passing)
- **Genetics Engine**: Fully implemented with research-based cannabis genetics

## Architecture Overview

### Core Manager System
- **GameManager**: Central singleton coordinating all systems
- **TimeManager**: Game time acceleration and offline progression
- **DataManager**: ScriptableObject asset management
- **EventManager**: Global event system coordination  
- **SaveManager**: Save/load with Data Transfer Objects (DTOs)
- **SettingsManager**: Player preferences and configuration

### Assembly Structure
```
ProjectChimera.Core/          - Core managers and base classes (runtime)
ProjectChimera.Data/          - ScriptableObject data definitions
ProjectChimera.Testing/       - Editor-only testing tools
ProjectChimera.Systems/       - Gameplay systems (Cultivation, Genetics, etc.)
ProjectChimera.UI/            - User interface components
```

### Key Design Patterns
- **ScriptableObject-Driven**: All data and events use SOs for modularity
- **Event-Driven Architecture**: Loose coupling via SO-based event channels
- **Manager Pattern**: Centralized system coordination through GameManager
- **DTO Pattern**: Clean data serialization for save/load systems

## Critical Development Lessons

### Unity 6 API Changes
- `FindObjectsOfType<T>()` → `UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None)`
- Yield statements cannot be inside try-catch blocks (C# restriction)
- Some packages deprecated (VSCode Editor, UI Toolkit previews)

### Manager Setup Requirements
1. Add manager components to GameManager GameObject
2. **CRITICAL**: Assign component references in GameManager Inspector
3. Managers initialize in specific dependency order
4. All managers must be registered for `GetManager<T>()` access

### Testing Infrastructure
- **RuntimeSystemTester**: Runtime validation of core systems (in Core assembly)
- **CoreSystemTester**: Advanced editor testing (in Testing assembly)  
- Run tests after any major architectural changes

### ScriptableObject Best Practices
- Always create assets through Unity's Create menu (never manually)
- Inheritance: `ChimeraDataSO` has `ValidateDataSpecific()`, `ChimeraConfigSO` has `ValidateData()`
- Use `[CreateAssetMenu]` attributes for proper asset creation

## Current File Structure

### Core Systems (Complete)
- **Base Classes**: ChimeraScriptableObject, ChimeraMonoBehaviour, ChimeraManager
- **Event System**: GameEventSO hierarchy with typed event channels
- **Manager System**: All 6 core managers implemented and tested
- **Testing**: RuntimeSystemTester validates all core functionality

### Testing Assets Created
- `TestSimpleEvent.asset` - Simple event channel for testing
- `TestStringEvent.asset` - String event channel for testing
- Scene: `Assets/ProjectChimera/Testing/Scenes/CoreTestScene.unity`

## Development Commands

### Testing
- **Run Core Tests**: Press Play in CoreTestScene.unity
- **Manual Test Trigger**: Right-click RuntimeSystemTester → "Run Tests"

### Build Verification
After making changes, always:
1. Check console for compilation errors (should be 0)
2. Run RuntimeSystemTester (should show 100% success rate)
3. Verify manager references are assigned in GameManager Inspector

## Current Development Phase: Advanced Systems Complete ✅
Comprehensive facility management simulation complete. Current status:
- ✅ Cannabis cultivation mechanics (PlantManager, lifecycle, health)
- ✅ Genetics and breeding systems (inheritance, trait expression, algorithms)
- ✅ Environmental control systems (HVAC, Lighting with full automation)
- ✅ Player progression and skill trees (research unlocks, technology advancement)
- ✅ Economic simulation systems (markets, trading, investments, finance)
- ✅ Advanced facility management (automation, IoT, AI advisor)

### Recently Completed: Comprehensive System Integration
All major systems implemented and integrated:
- **Environmental Systems**: HVAC and Lighting with intelligent automation
- **Economic Systems**: Market dynamics, trading, investments, and financial planning
- **Progression Systems**: Skill trees and research with technology unlocks
- **Automation Systems**: IoT sensor networks, predictive control, monitoring dashboards
- **AI Advisor**: Intelligent recommendations, predictive analytics, optimization insights
- **Integration Testing**: Comprehensive testing framework for system interoperability

## Technical Specifications
- **Unity Version**: 6000.2.0b2
- **Rendering**: URP 17.2.0
- **Scripting Backend**: Mono
- **Target Platform**: PC (Windows/Mac/Linux)
- **Architecture**: Component-based with ScriptableObject data layer

All systems tested and operational as of December 2024.