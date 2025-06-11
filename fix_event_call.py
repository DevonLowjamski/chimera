#!/usr/bin/env python3

def fix_event_call():
    file_path = "Assets/ProjectChimera/Systems/Cultivation/PlantManager.cs"
    
    with open(file_path, 'r') as f:
        lines = f.readlines()
    
    print("Fixing event call in PlantManager.cs...")
    
    # Find and fix line 231 (0-based index 230)
    for i, line in enumerate(lines):
        if "_onPlantHarvested?.Raise(plant);" in line:
            lines[i] = line.replace(
                "_onPlantHarvested?.Raise(plant);",
                "_onPlantHarvested?.Raise(); // SimpleGameEventSO - no arguments"
            )
            print(f"Fixed line {i+1}: {lines[i].strip()}")
            break
    
    with open(file_path, 'w') as f:
        f.writelines(lines)
    
    print("✅ Fixed event call successfully!")

if __name__ == "__main__":
    fix_event_call() 