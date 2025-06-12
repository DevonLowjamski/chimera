using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectChimera.Core;

namespace ProjectChimera.Data.Economy
{
    [System.Serializable]
    public enum CurrencyType
    {
        Cash,
        ResearchPoints,
        ReputationPoints
    }

    [System.Serializable]
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer,
        Credit,
        Investment,
        Loan
    }

    [System.Serializable]
    public enum TransactionCategory
    {
        Operations,
        Maintenance,
        Research,
        Marketing,
        Equipment,
        Facilities,
        Salary,
        Utilities,
        Investment,
        Loan,
        Tax,
        Other
    }

    [System.Serializable]
    public enum BudgetPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly
    }

    [System.Serializable]
    public enum ReportPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Yearly,
        Custom
    }

    [System.Serializable]
    public class Transaction
    {
        [Header("Transaction Information")]
        public string TransactionId;
        public DateTime Timestamp;
        public CurrencyType CurrencyType;
        public TransactionType TransactionType;
        public float Amount;
        public float BalanceAfter;
        public string Description;
        public TransactionCategory Category;
        public string Reference;
        public bool IsSuccessful = true;
        public string ErrorMessage;
        
        public Transaction()
        {
            TransactionId = System.Guid.NewGuid().ToString();
            Timestamp = DateTime.UtcNow;
        }
    }

    [System.Serializable]
    public class FinancialStatistics
    {
        [Header("Financial Overview")]
        public float TotalIncome;
        public float TotalExpenses;
        public float NetProfit;
        public float MonthlyRevenue;
        public float MonthlyExpenses;
        public float CashFlow;
        public DateTime LastUpdated;
        
        [Header("Category Breakdowns")]
        public Dictionary<TransactionCategory, float> IncomeByCategory = new Dictionary<TransactionCategory, float>();
        public Dictionary<TransactionCategory, float> ExpensesByCategory = new Dictionary<TransactionCategory, float>();
        public Dictionary<CurrencyType, float> BalancesByCurrency = new Dictionary<CurrencyType, float>();
    }

    [System.Serializable]
    public class CashFlowData
    {
        [Header("Cash Flow Analysis")]
        public float TotalInflow;
        public float TotalOutflow;
        public float NetCashFlow;
        public DateTime PeriodStart;
        public DateTime PeriodEnd;
        public BudgetPeriod Period;
        public List<Transaction> Transactions = new List<Transaction>();
    }

    [System.Serializable]
    public class TransactionValidator
    {
        [Header("Validation Rules")]
        public float MinimumBalance = 0f;
        public float MaxTransactionAmount = 1000000f;
        public bool RequireDescription = true;
        public bool AllowNegativeBalance = false;
        public List<TransactionCategory> RestrictedCategories = new List<TransactionCategory>();
    }

    [System.Serializable]
    public class CurrencySettings
    {
        [Header("Currency Configuration")]
        public string Name;
        public string Symbol;
        public Sprite Icon;
        public Color DisplayColor = Color.white;
        public int DecimalPlaces = 2;
        public bool IsExchangeable = true;
    }

    [System.Serializable]
    public class CurrencyDisplayData
    {
        [Header("Display Information")]
        public string FormattedAmount;
        public string CurrencySymbol;
        public Color DisplayColor;
        public Sprite CurrencyIcon;
    }

    [System.Serializable]
    public class FinancialReport
    {
        [Header("Report Information")]
        public string ReportId;
        public DateTime GeneratedDate;
        public ReportPeriod Period;
        public DateTime PeriodStart;
        public DateTime PeriodEnd;
        
        [Header("Financial Summary")]
        public float TotalRevenue;
        public float TotalExpenses;
        public float NetIncome;
        public float TotalAssets;
        public float TotalLiabilities;
        public float NetWorth;
        
        [Header("Detailed Breakdowns")]
        public List<Transaction> Transactions = new List<Transaction>();
        public FinancialStatistics Statistics;
        public Dictionary<string, BudgetStatusReport> BudgetStatus = new Dictionary<string, BudgetStatusReport>();
    }

    [System.Serializable]
    public class BudgetStatusReport
    {
        [Header("Budget Information")]
        public string CategoryName;
        public float Spent;
        public float Limit;
        public float Remaining;
        public float Percentage;
        public bool IsOverBudget;
        public DateTime LastUpdated;
    }

    // Simple Budget class for CurrencyManager
    [System.Serializable]
    public class Budget
    {
        [Header("Budget Information")]
        public string BudgetId;
        public string CategoryName;
        public float Limit;
        public float CurrentSpent;
        public BudgetPeriod Period;
        public DateTime StartDate;
        public DateTime EndDate;
        public bool IsActive = true;
        
        public float Remaining => Limit - CurrentSpent;
        public float PercentageUsed => Limit > 0 ? (CurrentSpent / Limit) * 100f : 0f;
        public bool IsOverBudget => CurrentSpent > Limit;
    }

    // Simple Loan class for CurrencyManager
    [System.Serializable]
    public class Loan
    {
        [Header("Loan Information")]
        public string LoanId;
        public float PrincipalAmount;
        public float InterestRate;
        public int TermDays;
        public DateTime StartDate;
        public DateTime LastPaymentDate;
        public float RemainingBalance;
        public string Purpose;
        public bool IsActive = true;
        
        public DateTime EndDate => StartDate.AddDays(TermDays);
        public bool IsOverdue => DateTime.Now > EndDate && RemainingBalance > 0;
        public float TotalInterest => PrincipalAmount * InterestRate * (TermDays / 365f);
    }

    // BudgetStatus enum for CurrencyManager
    [System.Serializable]
    public enum BudgetStatus
    {
        OnTrack,
        Warning,
        OverBudget,
        Inactive
    }
} 