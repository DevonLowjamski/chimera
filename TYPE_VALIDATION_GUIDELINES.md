# Project Chimera - Type Validation Guidelines

## 🎯 Purpose
This document provides specific protocols for validating types, enums, and class definitions before creating code references. These guidelines prevent the type assumption errors that caused Project Chimera's 300+ compilation error crisis.

## 🔍 Pre-Code-Generation Type Validation Protocol

### Step 1: Type Existence Verification
**BEFORE referencing ANY type, perform this validation:**

```bash
# Search for class definition
find Assets/ -name "*.cs" -exec grep -l "public class TypeName\|class TypeName" {} \;

# Search for enum definition  
find Assets/ -name "*.cs" -exec grep -l "public enum TypeName\|enum TypeName" {} \;

# Search for struct definition
find Assets/ -name "*.cs" -exec grep -l "public struct TypeName\|struct TypeName" {} \;

# Search for interface definition
find Assets/ -name "*.cs" -exec grep -l "public interface TypeName\|interface TypeName" {} \;
```

### Step 2: Namespace Verification
**For each located type, verify its namespace:**

```bash
# Extract namespace from file
grep -n "namespace.*" path/to/file.cs

# Verify assembly location matches namespace expectation
# ProjectChimera.Data.* should be in ProjectChimera/Data/
# ProjectChimera.Systems.* should be in ProjectChimera/Systems/
```

### Step 3: Member Validation
**For each type, document all accessible members:**

```bash
# For enums - extract all member names
grep -A 20 "enum TypeName" path/to/file.cs | grep -E "^\s*[A-Za-z]" | sed 's/,$//'

# For classes - extract public properties and methods
grep -A 50 "class TypeName" path/to/file.cs | grep -E "public.*{|public.*;"
```

### Step 4: Abstract Method Verification (CRITICAL)
**BEFORE implementing any derived class, verify ALL abstract methods:**

```bash
# Find abstract methods in base class
grep -n "abstract.*void\|abstract.*string\|abstract.*bool" path/to/BaseClass.cs

# Find protected abstract methods  
grep -n "protected abstract" path/to/BaseClass.cs

# Example for ChimeraManager:
grep -n "protected abstract" Assets/ProjectChimera/Core/ChimeraManager.cs
```

**MANDATORY CHECKLIST for Derived Classes:**
- [ ] **OnManagerInitialize()**: Must be implemented in all ChimeraManager derivatives
- [ ] **OnManagerShutdown()**: Must be implemented in all ChimeraManager derivatives  
- [ ] All other abstract methods from base classes
- [ ] Interface methods if implementing interfaces

**CS0534 Error Prevention**: Always check base class abstract requirements BEFORE writing derived class.

## 📋 Type Category Validation Checklists

### ✅ Enum Validation Checklist
**Before using any enum value:**

- [ ] **Locate Definition**: Found actual `public enum TypeName` in source file
- [ ] **Extract Members**: Listed all exact member names (case-sensitive)
- [ ] **Check Multiple Definitions**: Searched for same enum name in different namespaces
- [ ] **Verify Assembly Access**: Confirmed enum is accessible from target assembly
- [ ] **Document Safe Values**: Listed confirmed values for future reference

**Example Safe Enum Usage Documentation:**
```csharp
// ProjectChimera.Data.Cultivation.CareQuality (verified enum members):
// - Poor
// - Good  
// - Excellent
// NEVER use: OptimalCare, AutomationLevel, Adequate (do not exist)

var careQuality = ProjectChimera.Data.Cultivation.CareQuality.Good; // ✅ Verified
```

### ✅ Class Validation Checklist  
**Before using any class:**

- [ ] **Confirm Type Category**: Verified it's actually a class, not enum/struct
- [ ] **Check Constructor**: Documented available constructors
- [ ] **List Public Properties**: Documented exact property names and types
- [ ] **List Public Methods**: Documented method signatures
- [ ] **Verify Inheritance**: Checked base classes and implemented interfaces

**Example Class Usage Documentation:**
```csharp
// ProjectChimera.Events.PlayerChoice (verified class properties):
// - ChoiceId (string)
// - Description (string) 
// - Impact (PlayerChoiceImpact enum)
// NEVER use: PlayerChoice.SomeEnumValue (it's a class, not enum)

var choice = new ProjectChimera.Events.PlayerChoice 
{ 
    ChoiceId = "cultivation-method",
    Description = "Choose cultivation approach" 
}; // ✅ Verified class instantiation
```

### ✅ Property/Field Validation Checklist
**Before accessing any property or field:**

- [ ] **Verify Property Name**: Confirmed exact spelling and case
- [ ] **Check Property Type**: Documented expected type for assignments
- [ ] **Confirm Access Level**: Verified property is public and accessible
- [ ] **Test Getter/Setter**: Checked if property is read-only or read-write

**Example Property Validation:**
```csharp
// ProjectChimera.Data.Construction.ParticipantInfo (verified properties):
// - PlayerId (string) - read/write
// - PlayerName (string) - read/write  
// - Role (ParticipantRole enum) - read/write
// NEVER use: ParticipantRole (property name is "Role")

var participant = new ProjectChimera.Data.Construction.ParticipantInfo
{
    PlayerId = "player1",           // ✅ Verified property
    PlayerName = "Test Player",     // ✅ Verified property
    Role = ParticipantRole.Architect // ✅ Verified property and enum value
};
```

## 🚫 Forbidden Type Assumption Patterns

### ❌ NEVER Assume Enum Values Exist
```csharp
// ❌ WRONG - Assuming enum values without verification
var care = CareQuality.OptimalCare;        // OptimalCare doesn't exist
var automation = AutomationLevel.Full;     // AutomationLevel doesn't exist
var quality = QualityRating.Adequate;      // Adequate doesn't exist

// ✅ CORRECT - Using verified enum values
var care = ProjectChimera.Data.Cultivation.CareQuality.Good;     // Verified to exist
var task = ProjectChimera.Data.Cultivation.CultivationTaskType.Watering; // Verified
```

### ❌ NEVER Mix Class and Enum Syntax
```csharp
// ❌ WRONG - Treating class as enum
var choice = PlayerChoice.CultivationMethod; // PlayerChoice is a class, not enum

// ✅ CORRECT - Using proper class instantiation
var choice = new ProjectChimera.Events.PlayerChoice 
{ 
    ChoiceId = "cultivation-method",
    Description = "Select cultivation approach"
};
```

### ❌ NEVER Use Unqualified Ambiguous Types
```csharp
// ❌ WRONG - Ambiguous reference (exists in multiple namespaces)
var state = NarrativeState.Active;          // Ambiguous - multiple definitions
var experience = ExperienceSource.Research; // Ambiguous - multiple definitions

// ✅ CORRECT - Fully qualified names
var dataState = ProjectChimera.Data.Narrative.NarrativeState.Active;
var systemState = ProjectChimera.Systems.Narrative.NarrativeState.Active;
var progressionExp = ProjectChimera.Data.Progression.ExperienceSource.Research;
```

## 🔧 Type Validation Tools and Commands

### Comprehensive Type Search Commands
```bash
# Find all occurrences of a type name
find Assets/ -name "*.cs" -exec grep -n "TypeName" {} +

# Find class definitions specifically
find Assets/ -name "*.cs" -exec grep -n "public class\|class.*:" {} + | grep "TypeName"

# Find enum definitions specifically  
find Assets/ -name "*.cs" -exec grep -n "public enum\|enum.*:" {} + | grep "TypeName"

# Find all members of an enum
grep -A 30 "enum TypeName" path/to/file.cs | grep -E "^\s*[A-Za-z]"

# Find all properties of a class
grep -A 50 "class TypeName" path/to/file.cs | grep -E "public.*\{.*get|public.*;"
```

### Assembly Validation Commands
```bash
# Check assembly references
find Assets/ -name "*.asmdef" -exec cat {} \; | grep -A 5 -B 5 "references"

# Verify namespace structure matches assembly organization
find Assets/ProjectChimera/Data/ -name "*.cs" -exec grep -l "namespace ProjectChimera\.Data" {} \;
find Assets/ProjectChimera/Systems/ -name "*.cs" -exec grep -l "namespace ProjectChimera\.Systems" {} \;
```

## 📊 Type Validation Documentation Template

### For Each New Type Reference, Document:
```markdown
## Type: [TypeName]
- **Category**: Class/Enum/Struct/Interface
- **Namespace**: ProjectChimera.[Namespace].[SubNamespace]
- **Assembly**: ProjectChimera.[AssemblyName]
- **File Location**: Assets/ProjectChimera/[Path]/[File].cs
- **Definition Line**: Line [Number]

### Verified Members:
- **Properties**: [List with types]
- **Methods**: [List with signatures]  
- **Enum Values**: [List exact names] (if applicable)

### Usage Examples:
```csharp
// Safe usage patterns
[Provide working code examples]
```

### Common Mistakes to Avoid:
- [List specific anti-patterns for this type]
```

## 🎯 Validation Success Criteria

### ✅ Type Reference is Valid When:
- [ ] Type definition located in actual source file
- [ ] Namespace and assembly verified
- [ ] All referenced members confirmed to exist
- [ ] Usage syntax matches type category (class/enum/struct)
- [ ] Fully qualified names used for ambiguous types
- [ ] Assembly dependencies are satisfied

### 🚫 Type Reference is Invalid When:
- [ ] Type definition not found in any source file
- [ ] Referenced members do not exist in type definition
- [ ] Namespace doesn't match actual location
- [ ] Assembly references are missing or circular
- [ ] Ambiguous references without qualification
- [ ] Class/enum syntax mismatch

## 🔄 Continuous Validation Process

### During Development:
1. **Before each coding session**: Validate all types to be referenced
2. **After each type creation**: Update validation documentation
3. **Before each build**: Run comprehensive type existence verification
4. **After each error**: Add new validation rules to prevent recurrence

### Weekly Validation Maintenance:
1. **Review type usage patterns**: Identify frequently referenced types
2. **Update validation documentation**: Add new types and usage examples
3. **Clean up obsolete validations**: Remove validations for deleted types
4. **Test validation commands**: Ensure search commands still work correctly

---

**Remember: 5 minutes of type validation prevents 5 hours of compilation error debugging.**