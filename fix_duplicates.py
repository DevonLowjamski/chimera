#!/usr/bin/env python3
import re

def fix_duplicate_classes():
    file_path = "Assets/ProjectChimera/Data/Economy/CurrencyDataStructures.cs"
    
    with open(file_path, 'r') as f:
        content = f.read()
    
    # Comment out RiskLevel enum
    content = re.sub(
        r'(\s+public enum RiskLevel\s*\{[^}]+\})',
        r'/* COMMENTED OUT - Duplicate definition exists in FinanceDataStructures.cs\1*/\n',
        content, flags=re.DOTALL
    )
    
    # Comment out LoanPayment class
    content = re.sub(
        r'(\s+\[System\.Serializable\]\s+public class LoanPayment\s*\{[^}]+\})',
        r'/* COMMENTED OUT - Duplicate definition exists in FinanceDataStructures.cs\1*/\n',
        content, flags=re.DOTALL
    )
    
    # Comment out Investment class  
    content = re.sub(
        r'(\s+\[System\.Serializable\]\s+public class Investment\s*\{[^}]+\})',
        r'/* COMMENTED OUT - Duplicate definition exists in FinanceDataStructures.cs\1*/\n',
        content, flags=re.DOTALL
    )
    
    # Comment out CashFlowScenario class
    content = re.sub(
        r'(\s+\[System\.Serializable\]\s+public class CashFlowScenario\s*\{[^}]+\})',
        r'/* COMMENTED OUT - Duplicate definition exists in FinanceDataStructures.cs\1*/\n',
        content, flags=re.DOTALL
    )
    
    with open(file_path, 'w') as f:
        f.write(content)
    
    print("✅ Fixed duplicate class definitions")

if __name__ == "__main__":
    fix_duplicate_classes() 