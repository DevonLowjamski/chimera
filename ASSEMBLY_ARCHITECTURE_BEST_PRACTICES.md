# Project Chimera - Assembly Architecture Best Practices

## 🎯 Purpose
This document establishes proven assembly architecture patterns that prevent circular dependencies, compilation errors, and maintain clean separation of concerns in Project Chimera's complex Unity project structure.

## 🏗️ Project Chimera Assembly Architecture

### Current Assembly Structure (Verified & Working)
```
ProjectChimera.Core/              - Foundation systems, base classes, interfaces
├── ChimeraManager.cs            - Base manager class
├── GameManager.cs               - Central system coordinator  
├── IChimeraManager.cs           - Manager interface contract
└── [Core systems and utilities]

ProjectChimera.Data/              - ScriptableObject definitions and data structures
├── Cultivation/                 - Plant, genetics, cultivation data
├── Economy/                     - Market, trading, economic data
├── Environment/                 - Climate, equipment, facility data
├── Genetics/                    - Breeding, traits, gene data
├── Progression/                 - Skills, research, achievement data
└── [All data-only classes]

ProjectChimera.Systems/           - Game logic and system implementations
├── Cultivation/                 - Cultivation system managers
├── Economy/                     - Economic system managers
├── Environment/                 - Environmental system managers
├── Genetics/                    - Genetics system managers
├── Progression/                 - Progression system managers
├── Events/                      - Event system implementations
└── [All system logic]

ProjectChimera.UI/                - User interface and presentation layer
├── [UI components and controllers]

ProjectChimera.Testing/           - Testing framework and validation
├── Performance/                 - Performance benchmarking
└── [Test implementations]

ProjectChimera.Editor/            - Unity Editor extensions and tools
├── [Editor-only functionality]
```

### Assembly Dependency Flow (MANDATORY)
```
Testing → UI → Systems → Data → Core
   ↑      ↑       ↑        ↑      ↑
Editor ────┴───────┴────────┴──────┘
```

**Critical Rule**: Dependencies MUST flow in one direction only. Higher-level assemblies can reference lower-level assemblies, but NEVER the reverse.

## 📋 Assembly Definition Best Practices

### ✅ MANDATORY Assembly Definition Template
```json
{
    "name": "ProjectChimera.[AssemblyName]",
    "rootNamespace": "ProjectChimera.[AssemblyName]",
    "references": [
        "GUID:valid-guid-reference-here"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": false,
    "precompiledReferences": [],
    "autoReferenced": true,
    "defineConstraints": [],
    "versionDefines": [],
    "noEngineReferences": false
}
```

### ✅ Assembly Reference Verification Protocol
**BEFORE adding any assembly reference:**

1. **Check Dependency Direction**: Ensure reference flows from higher to lower level
2. **Verify Assembly Exists**: Confirm referenced .asmdef file exists
3. **Validate GUID**: Ensure GUID in reference matches actual .asmdef file
4. **Test Compilation**: Build project after adding reference
5. **Check for Circular Dependencies**: Ensure no cycles in dependency graph

### ✅ Assembly Naming Conventions
- **Core Assembly**: `ProjectChimera.Core`
- **Data Assemblies**: `ProjectChimera.Data` (unified data assembly)
- **System Assemblies**: `ProjectChimera.Systems.[SystemName]` OR unified `ProjectChimera.Systems`
- **UI Assembly**: `ProjectChimera.UI`
- **Testing Assembly**: `ProjectChimera.Testing`
- **Editor Assembly**: `ProjectChimera.Editor`

## 🚫 Assembly Anti-Patterns (FORBIDDEN)

### ❌ Circular Dependencies
```
❌ WRONG:
ProjectChimera.Core → ProjectChimera.Systems → ProjectChimera.Core
ProjectChimera.Data → ProjectChimera.Systems → ProjectChimera.Data
```

### ❌ Invalid Reference Directions
```
❌ WRONG:
Core → Systems (Core should never reference Systems)
Data → UI (Data should never reference UI)
Systems → Testing (Systems should never reference Testing)
```

### ❌ Non-Existent Assembly References
```json
❌ WRONG:
{
    "references": [
        "ProjectChimera.Environment",    // Doesn't exist
        "ProjectChimera.Cultivation",    // Doesn't exist  
        "ProjectChimera.NonExistent"     // Obviously doesn't exist
    ]
}
```

### ❌ Overly Granular Assemblies
```
❌ WRONG (too many small assemblies):
ProjectChimera.Systems.Cultivation.PlantGrowth
ProjectChimera.Systems.Cultivation.PlantHealth  
ProjectChimera.Systems.Cultivation.PlantGenetics
ProjectChimera.Systems.Cultivation.PlantVisuals
```

## 🔧 Assembly Management Strategies

### Strategy 1: Unified System Assemblies (RECOMMENDED)
**Pros**: Simpler dependencies, faster compilation, easier maintenance
**Use When**: Systems are tightly coupled and frequently interact

```
ProjectChimera.Systems (single assembly containing):
├── Cultivation/
├── Economy/
├── Environment/
├── Genetics/
└── All other systems
```

### Strategy 2: Granular System Assemblies
**Pros**: Better separation, selective loading, modular development
**Use When**: Systems are independent and rarely interact

```
ProjectChimera.Systems.Cultivation
ProjectChimera.Systems.Economy
ProjectChimera.Systems.Environment
ProjectChimera.Systems.Genetics
```

**IMPORTANT**: Project Chimera currently uses Strategy 1 (unified) successfully.

### Strategy 3: Hybrid Approach
**Pros**: Balance of modularity and simplicity
**Use When**: Some systems need isolation while others can be grouped

```
ProjectChimera.Systems          (core systems)
ProjectChimera.Systems.Advanced (optional/experimental systems)
```

## 📊 Assembly Dependency Validation

### Pre-Development Validation Checklist
**Before starting any development session:**

- [ ] **Verify Assembly Structure**: All .asmdef files exist and are properly configured
- [ ] **Check Dependency Flow**: No circular references in dependency graph
- [ ] **Validate GUIDs**: All assembly references use correct GUIDs
- [ ] **Test Compilation**: All assemblies compile without errors
- [ ] **Document Dependencies**: Dependency relationships are clearly documented

### Assembly Health Monitoring Commands
```bash
# Find all assembly definition files
find Assets/ -name "*.asmdef" -exec echo "=== {} ===" \; -exec cat {} \; -exec echo "" \;

# Check for circular dependencies (manual review needed)
grep -r "references" Assets/**/*.asmdef

# Verify assembly compilation
# (Must be done in Unity Editor - Build Settings → Player Settings → Script Compilation)
```

## 🎯 Assembly Design Principles

### Principle 1: Single Responsibility Per Assembly
**Each assembly should have ONE clear purpose:**
- **Core**: Foundation systems and contracts
- **Data**: Data definitions and structures only
- **Systems**: Game logic and system implementations
- **UI**: User interface and presentation
- **Testing**: Testing framework and validation

### Principle 2: Dependency Inversion
**Higher-level assemblies define interfaces, lower-level assemblies implement them:**
```csharp
// In ProjectChimera.Core (low-level)
public interface IPlantManager 
{
    void GrowPlant(int plantId);
}

// In ProjectChimera.Systems (high-level)
public class PlantManager : IPlantManager
{
    public void GrowPlant(int plantId) { /* implementation */ }
}
```

### Principle 3: Stable Dependencies
**Dependencies should point to stable (less likely to change) assemblies:**
- **Most Stable**: Core, Data
- **Moderately Stable**: Systems
- **Least Stable**: UI, Testing, Editor

### Principle 4: Interface Segregation
**Assemblies should not depend on interfaces they don't use:**
```csharp
// ✅ GOOD - Focused interfaces
public interface IPlantGrower 
{
    void GrowPlant(int plantId);
}

public interface IPlantRenderer
{
    void RenderPlant(int plantId);
}

// ❌ BAD - Fat interface
public interface IPlantEverything
{
    void GrowPlant(int plantId);
    void RenderPlant(int plantId);
    void SellPlant(int plantId);
    void AnalyzePlant(int plantId);
    // ... 50 more methods
}
```

## 🔄 Assembly Evolution Guidelines

### Adding New Assemblies
1. **Justify Need**: Why can't existing assemblies handle the new functionality?
2. **Design Dependencies**: Where does the new assembly fit in the dependency graph?
3. **Minimize References**: Add only essential assembly references
4. **Test Integration**: Ensure new assembly integrates cleanly with existing structure
5. **Update Documentation**: Document new assembly purpose and dependencies

### Refactoring Existing Assemblies
1. **Preserve Interfaces**: Don't break existing contracts
2. **Migrate Gradually**: Move code incrementally, test frequently
3. **Update References**: Ensure all dependent assemblies are updated
4. **Test Thoroughly**: Comprehensive testing after refactoring
5. **Document Changes**: Update architecture documentation

### Removing Assemblies
1. **Check Dependencies**: Ensure no other assemblies reference the one being removed
2. **Migrate Code**: Move essential code to appropriate assemblies
3. **Update References**: Remove references from all dependent assemblies
4. **Clean Up**: Remove .asmdef files and update documentation
5. **Test Compilation**: Ensure project still compiles after removal

## 🚨 Assembly Error Prevention

### Common Assembly Errors and Prevention
1. **CS0234 "Type or namespace name does not exist"**
   - **Prevention**: Always verify assembly references before using types
   - **Solution**: Add correct assembly reference or use fully qualified names

2. **Circular Dependency Errors**
   - **Prevention**: Follow strict dependency direction rules
   - **Solution**: Refactor to eliminate cycles, use interfaces for decoupling

3. **Missing Assembly References**
   - **Prevention**: Add references incrementally and test compilation
   - **Solution**: Add missing references or reorganize code

4. **Assembly Compilation Order Issues**
   - **Prevention**: Ensure dependencies are clearly defined
   - **Solution**: Check .asmdef files for correct reference configuration

## 📋 Assembly Maintenance Checklist

### Weekly Assembly Review
- [ ] **Dependency Validation**: Check for new circular dependencies
- [ ] **Reference Cleanup**: Remove unused assembly references  
- [ ] **Performance Check**: Monitor assembly compilation times
- [ ] **Documentation Update**: Keep assembly documentation current

### Monthly Assembly Audit
- [ ] **Architecture Review**: Assess if current structure still serves project needs
- [ ] **Refactoring Opportunities**: Identify assemblies that could be consolidated or split
- [ ] **Dependency Optimization**: Look for ways to reduce coupling between assemblies
- [ ] **Future Planning**: Consider assembly changes needed for upcoming features

## 🎯 Assembly Success Metrics

### Green Flags (Healthy Assembly Architecture)
- ✅ All assemblies compile without errors or warnings
- ✅ No circular dependencies in dependency graph
- ✅ Clear separation of concerns between assemblies
- ✅ Minimal and justified assembly references
- ✅ Fast compilation times (< 10 seconds for full rebuild)

### Red Flags (Assembly Architecture Problems)
- 🚫 Circular dependency errors
- 🚫 Missing assembly reference errors
- 🚫 Overly complex dependency graphs
- 🚫 Slow compilation times (> 30 seconds)
- 🚫 Frequent assembly-related compilation errors

---

**Remember: Good assembly architecture is invisible when it works, but catastrophic when it doesn't. Invest in proper structure early and maintain it religiously.**