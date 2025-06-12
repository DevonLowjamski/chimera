#!/usr/bin/env python3
"""
Fix duplicate class definitions in CurrencyDataStructures.cs
These classes are also defined in FinanceDataStructures.cs causing CS0101 errors
"""

import re

def fix_duplicate_classes():
    file_path = "Assets/ProjectChimera/Data/Economy/CurrencyDataStructures.cs"
    
    # Classes to comment out (they exist in FinanceDataStructures.cs)
    duplicate_classes = [
        "CreditAccount", 
        "LoanPayment", 
        "Investment", 
        "CashFlowScenario",
        "BudgetStatus"
    ]
    
    # Enum to comment out
    duplicate_enums = ["RiskLevel"]
    
    try:
        with open(file_path, 'r') as f:
            content = f.read()
        
        # Comment out duplicate classes
        for class_name in duplicate_classes:
            # Find class definition and its entire block
            pattern = rf'(/// <summary>.*?</summary>\s*\[System\.Serializable\]\s*public class {class_name}.*?^\s*}})'
            content = re.sub(pattern, r'/* COMMENTED OUT - Duplicate definition exists in FinanceDataStructures.cs\n\1\n*/', content, flags=re.MULTILINE | re.DOTALL)
        
        # Comment out duplicate enums  
        for enum_name in duplicate_enums:
            pattern = rf'(public enum {enum_name}.*?^\s*}})'
            content = re.sub(pattern, r'/* COMMENTED OUT - Duplicate definition exists in FinanceDataStructures.cs\n\1\n*/', content, flags=re.MULTILINE | re.DOTALL)
        
        # Write the modified content back
        with open(file_path, 'w') as f:
            f.write(content)
        
        print(f"✅ Fixed duplicate class definitions in {file_path}")
        print("Commented out the following duplicates:")
        for item in duplicate_classes + duplicate_enums:
            print(f"  - {item}")
            
    except Exception as e:
        print(f"❌ Error fixing duplicate classes: {e}")

if __name__ == "__main__":
    fix_duplicate_classes() 