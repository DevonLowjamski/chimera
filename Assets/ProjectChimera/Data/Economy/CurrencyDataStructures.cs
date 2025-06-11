using UnityEngine;
using System;
using System.Collections.Generic;

namespace ProjectChimera.Data.Economy
{
    /// <summary>
    /// Comprehensive data structures for Project Chimera's currency and financial system.
    /// Defines all currency types, transaction records, financial tracking, budgeting,
    /// loans, investments, and economic analytics for deep financial gameplay.
    /// </summary>
    
    /// <summary>
    /// Types of currencies available in the game
    /// </summary>
    public enum CurrencyType
    {
        Cash,               // Primary currency for purchases
        Credits,            // Secondary currency for special items
        ResearchPoints,     // For unlocking research and technology
        ReputationPoints,   // Social currency for relationships
        Energy,             // For powering equipment and systems
        Materials           // Crafting and construction resources
    }
    
    /// <summary>
    /// Types of financial transactions
    /// </summary>
    public enum TransactionType
    {
        Income,     // Money coming in
        Expense,    // Money going out
        Transfer,   // Moving between currencies
        Credit,     // Using credit/loans
        Investment, // Long-term financial moves
        Refund,     // Money returned
        Penalty,    // Fines or penalties
        Bonus,      // Special rewards
        System      // Administrative transactions
    }
    
    /// <summary>
    /// Categories for organizing transactions
    /// </summary>
    public enum TransactionCategory
    {
        // Income categories
        Sales,              // Revenue from selling products
        Harvest,            // Income from plant harvests
        Investment,         // Returns from investments
        Loan,               // Money from loans
        Bonus,              // Performance bonuses
        Gift,               // Gifts or grants
        
        // Expense categories
        Equipment,          // Facility and growing equipment
        Seeds,              // Seeds and genetics
        Utilities,          // Power, water, internet
        Supplies,           // Nutrients, tools, consumables
        Maintenance,        // Equipment upkeep
        Marketing,          // Advertising and promotion
        Research,           // R&D expenses
        Wages,              // Employee payments
        Rent,               // Facility rent
        Insurance,          // Business insurance
        Taxes,              // Government taxes
        Legal,              // Legal and compliance costs
        
        // Special categories
        Transfer,           // Between currency types
        Emergency,          // Unexpected costs
        Entertainment,      // Non-essential purchases
        Other,              // Miscellaneous
        System              // Administrative
    }
    
    /// <summary>
    /// Budget tracking periods
    /// </summary>
    public enum BudgetPeriod
    {
        Daily,
        Weekly,
        Monthly,
        Quarterly,
        Annually
    }
    
    /// <summary>
    /// Financial report periods
    /// </summary>
    public enum ReportPeriod
    {
        Current,    // Current state
        Daily,      // Last 24 hours
        Weekly,     // Last 7 days
        Monthly,    // Last 30 days
        Quarterly,  // Last 90 days
        Annual,     // Last 365 days
        Custom      // User-defined period
    }
    
    /// <summary>
    /// Investment risk levels
    /// </summary>
    public enum RiskLevel
    {
        VeryLow,    // Government bonds, savings
        Low,        // Blue chip stocks, CDs
        Medium,     // Balanced funds, real estate
        High,       // Growth stocks, commodities
        VeryHigh    // Crypto, startups, speculation
    }
    
    /// <summary>
    /// Individual financial transaction record
    /// </summary>
    [System.Serializable]
    public class Transaction
    {
        [Header("Transaction Identity")]
        public string TransactionId;
        public DateTime Timestamp;
        
        [Header("Transaction Details")]
        public TransactionType Type;
        public CurrencyType CurrencyType;
        public float Amount;
        public TransactionCategory Category;
        public string Description;
        
        [Header("Context")]
        public string SourceSystem;        // Which system initiated this transaction
        public string RelatedEntityId;     // ID of related entity (plant, equipment, etc.)
        public Dictionary<string, object> Metadata = new Dictionary<string, object>();
        
        [Header("State")]
        public float BalanceAfter;         // Account balance after this transaction
        public bool IsRecurring;          // Whether this is a recurring transaction
        public bool IsReversed;           // Whether this transaction was reversed/refunded
        public string ReversalReason;     // Reason if transaction was reversed
        
        [Header("Validation")]
        public bool IsValidated;          // Whether transaction passed validation
        public string ValidationNotes;   // Any validation concerns
        public float TaxAmount;           // Tax portion of transaction
        public float FeeAmount;           // Processing fees
    }
    
    /// <summary>
    /// Currency configuration and display settings
    /// </summary>
    [System.Serializable]
    public class CurrencySettings
    {
        [Header("Display")]
        public string Name;
        public string Symbol;
        public string Icon;
        public Color DisplayColor;
        public int DecimalPlaces;
        
        [Header("Properties")]
        public bool IsExchangeable;       // Can be converted to other currencies
        public bool IsEarnable;           // Can be earned through gameplay
        public bool IsSpendable;          // Can be spent on purchases
        public float ExchangeRate;        // Rate to base currency (Cash)
        
        [Header("Limits")]
        public float MaxAmount;           // Maximum amount that can be held
        public float MinTransfer;         // Minimum transfer amount
        public bool HasStorageCost;       // Whether storage costs money
        public float StorageCostRate;     // Cost per unit per day to store
    }
    
    /// <summary>
    /// Financial statistics and metrics
    /// </summary>
    [System.Serializable]
    public class FinancialStatistics
    {
        [Header("Totals")]
        public float TotalIncome = 0f;
        public float TotalExpenses = 0f;
        public int TransactionCount = 0;
        
        [Header("Categories")]
        public Dictionary<string, float> IncomeByCategory = new Dictionary<string, float>();
        public Dictionary<string, float> ExpensesByCategory = new Dictionary<string, float>();
        
        [Header("Performance")]
        public float HighestBalance = 0f;
        public float LowestBalance = 0f;
        public DateTime HighestBalanceDate;
        public DateTime LowestBalanceDate;
        
        [Header("Milestones")]
        public float LastMilestone = 0f;
        public List<float> AchievedMilestones = new List<float>();
        
        [Header("Averages")]
        public float AverageTransactionAmount = 0f;
        public float AverageIncomePerDay = 0f;
        public float AverageExpensesPerDay = 0f;
        
        [Header("Streaks")]
        public int ProfitableStreak = 0;     // Days with positive cash flow
        public int LossStreak = 0;           // Days with negative cash flow
        public int MaxProfitStreak = 0;      // Best profitable streak
        public int MaxLossStreak = 0;        // Worst loss streak
    }
    
    /// <summary>
    /// Budget for expense tracking and control
    /// </summary>
    [System.Serializable]
    public class Budget
    {
        [Header("Budget Identity")]
        public string BudgetId;
        public string CategoryName;
        
        [Header("Budget Parameters")]
        public float Limit;
        public BudgetPeriod Period;
        public DateTime StartDate;
        public DateTime EndDate;
        
        [Header("Current Status")]
        public float CurrentSpent = 0f;
        public bool IsActive = true;
        public bool IsRecurring = true;
        
        [Header("Alerts")]
        public float WarningThreshold = 0.8f;    // Alert when 80% of budget used
        public float CriticalThreshold = 0.95f;  // Critical alert when 95% used
        public bool AlertsEnabled = true;
        public DateTime LastAlert;
        
        [Header("History")]
        public List<BudgetPeriodRecord> PeriodHistory = new List<BudgetPeriodRecord>();
    }
    
    /// <summary>
    /// Budget performance for a specific period
    /// </summary>
    [System.Serializable]
    public class BudgetPeriodRecord
    {
        public DateTime PeriodStart;
        public DateTime PeriodEnd;
        public float BudgetAmount;
        public float ActualSpent;
        public float Variance;              // Budget - Actual (positive = under budget)
        public float VariancePercentage;    // Variance as percentage of budget
        public bool WasOverBudget;
    }
    
    /// <summary>
    /// Current budget status for display
    /// </summary>
    [System.Serializable]
    public class BudgetStatus
    {
        public float BudgetAmount;
        public float SpentAmount;
        public float RemainingAmount;
        public float PercentageUsed;
        public bool IsOverBudget;
        public bool IsNearLimit;            // Close to budget limit
        public int DaysRemaining;           // Days left in budget period
        public float DailyBurnRate;         // Current spending rate per day
        public float ProjectedOverage;      // Projected amount over budget
    }
    
    /// <summary>
    /// Credit account for loans and credit purchases
    /// </summary>
    [System.Serializable]
    public class CreditAccount
    {
        [Header("Credit Terms")]
        public float CreditLimit;
        public float UsedCredit = 0f;
        public float InterestRate;
        public float MinimumPayment;
        
        [Header("Payment Info")]
        public float PaymentDue = 0f;
        public DateTime LastPaymentDate;
        public DateTime NextPaymentDue;
        public bool IsCurrentOnPayments = true;
        
        [Header("Credit Score")]
        public int CreditScore = 600;       // 300-850 scale
        public DateTime LastScoreUpdate;
        public List<CreditScoreChange> ScoreHistory = new List<CreditScoreChange>();
        
        [Header("Terms")]
        public DateTime AccountOpenDate;
        public bool IsActive = true;
        public string AccountType = "Business Line of Credit";
    }
    
    /// <summary>
    /// Credit score change record
    /// </summary>
    [System.Serializable]
    public class CreditScoreChange
    {
        public DateTime Date;
        public int OldScore;
        public int NewScore;
        public string Reason;
        public int Impact;                  // Point change (positive or negative)
    }
    
    /// <summary>
    /// Loan information
    /// </summary>
    [System.Serializable]
    public class Loan
    {
        [Header("Loan Identity")]
        public string LoanId;
        public string Purpose;
        public string LoanType;             // Equipment, Working Capital, etc.
        
        [Header("Loan Terms")]
        public float PrincipalAmount;
        public float InterestRate;
        public int TermDays;
        public DateTime StartDate;
        public DateTime MaturityDate;
        
        [Header("Current Status")]
        public float RemainingBalance;
        public DateTime LastPaymentDate;
        public float MonthlyPayment;
        public bool IsActive = true;
        
        [Header("Payment History")]
        public List<LoanPayment> PaymentHistory = new List<LoanPayment>();
        public int PaymentsMade = 0;
        public int PaymentsMissed = 0;
        public bool IsCurrentOnPayments = true;
        
        [Header("Performance")]
        public float TotalInterestPaid = 0f;
        public float TotalPrincipalPaid = 0f;
        public RiskLevel RiskRating = RiskLevel.Medium;
    }
    
    /// <summary>
    /// Individual loan payment record
    /// </summary>
    [System.Serializable]
    public class LoanPayment
    {
        public DateTime PaymentDate;
        public float Amount;
        public float PrincipalPortion;
        public float InterestPortion;
        public float BalanceAfter;
        public bool WasOnTime;
        public string PaymentMethod;
    }
    
    /// <summary>
    /// Investment information
    /// </summary>
    [System.Serializable]
    public class Investment
    {
        [Header("Investment Identity")]
        public string InvestmentId;
        public string Type;                 // Stocks, Bonds, Real Estate, etc.
        public string Description;
        
        [Header("Investment Details")]
        public float Amount;                // Initial investment amount
        public float CurrentValue;          // Current market value
        public float ExpectedReturn;        // Expected annual return rate
        public RiskLevel Risk;
        
        [Header("Timeline")]
        public DateTime StartDate;
        public DateTime MaturityDate;
        public bool HasMaturityDate;
        public bool IsActive = true;
        
        [Header("Performance")]
        public float TotalReturn;           // Total return to date
        public float AnnualizedReturn;      // Annualized return rate
        public List<InvestmentUpdate> ValueHistory = new List<InvestmentUpdate>();
        
        [Header("Dividends")]
        public float TotalDividends = 0f;
        public List<DividendPayment> DividendHistory = new List<DividendPayment>();
    }
    
    /// <summary>
    /// Investment value update record
    /// </summary>
    [System.Serializable]
    public class InvestmentUpdate
    {
        public DateTime Date;
        public float Value;
        public float ChangeAmount;
        public float ChangePercentage;
        public string Reason;               // Market news, earnings, etc.
    }
    
    /// <summary>
    /// Dividend payment record
    /// </summary>
    [System.Serializable]
    public class DividendPayment
    {
        public DateTime PaymentDate;
        public float Amount;
        public float Yield;                 // Dividend yield percentage
        public string InvestmentType;
    }
    
    /// <summary>
    /// Cash flow prediction data
    /// </summary>
    [System.Serializable]
    public class CashFlowData
    {
        [Header("Current Period")]
        public float ProjectedIncome;
        public float ProjectedExpenses;
        public float NetCashFlow;
        public float ProjectedBalance;
        
        [Header("Confidence")]
        public float ConfidenceLevel;       // 0-1, how confident we are in predictions
        public string ConfidenceReason;     // Why confidence is high/low
        
        [Header("Breakdown")]
        public Dictionary<string, float> IncomeBySource = new Dictionary<string, float>();
        public Dictionary<string, float> ExpensesByCategory = new Dictionary<string, float>();
        
        [Header("Scenarios")]
        public CashFlowScenario OptimisticScenario;
        public CashFlowScenario PessimisticScenario;
        public CashFlowScenario RealisticScenario;
    }
    
    /// <summary>
    /// Cash flow scenario (optimistic, realistic, pessimistic)
    /// </summary>
    [System.Serializable]
    public class CashFlowScenario
    {
        public string ScenarioName;
        public float ProjectedIncome;
        public float ProjectedExpenses;
        public float NetCashFlow;
        public float EndingBalance;
        public float Probability;           // Likelihood of this scenario
        public string KeyAssumptions;       // What assumptions drive this scenario
    }
    
    /// <summary>
    /// Comprehensive financial report
    /// </summary>
    [System.Serializable]
    public class FinancialReport
    {
        [Header("Report Identity")]
        public string ReportId;
        public DateTime ReportDate;
        public ReportPeriod Period;
        public DateTime PeriodStart;
        public DateTime PeriodEnd;
        
        [Header("Balance Sheet")]
        public float CashBalance;
        public float TotalAssets;
        public float TotalLiabilities;
        public float NetWorth;
        
        [Header("Income Statement")]
        public float TotalIncome;
        public float TotalExpenses;
        public float NetIncome;
        public float GrossMargin;
        public float OperatingMargin;
        public float NetMargin;
        
        [Header("Cash Flow")]
        public float OperatingCashFlow;
        public float InvestingCashFlow;
        public float FinancingCashFlow;
        public float NetCashFlow;
        
        [Header("Performance Metrics")]
        public float BurnRate;              // Daily cash consumption
        public float RunwayDays;            // Days until cash runs out
        public float ProfitMargin;          // Net income / revenue
        public float ROI;                   // Return on investment
        
        [Header("Detailed Breakdowns")]
        public Dictionary<string, float> IncomeByCategory;
        public Dictionary<string, float> ExpensesByCategory;
        public Dictionary<string, BudgetStatus> BudgetStatus;
        
        [Header("Predictions")]
        public CashFlowData CashFlowPrediction;
        
        [Header("Alerts")]
        public List<FinancialAlert> Alerts = new List<FinancialAlert>();
        public List<string> Recommendations = new List<string>();
    }
    
    /// <summary>
    /// Financial alert or warning
    /// </summary>
    [System.Serializable]
    public class FinancialAlert
    {
        public string AlertId;
        public DateTime AlertTime;
        public FinancialAlertType Type;
        public string Message;
        public string Severity;             // Low, Medium, High, Critical
        public bool IsResolved = false;
        public DateTime ResolvedTime;
        public string ResolutionNotes;
    }
    
    /// <summary>
    /// Types of financial alerts
    /// </summary>
    public enum FinancialAlertType
    {
        LowCash,                // Cash balance is low
        BudgetOverrun,          // Over budget in category
        UnusualSpending,        // Spending pattern anomaly
        PaymentDue,             // Loan payment due soon
        InvestmentLoss,         // Investment losing value
        CreditLimitNear,        // Near credit limit
        CashFlowNegative,       // Negative cash flow projected
        TaxDeadline,            // Tax payment due
        MaintenanceDue,         // Equipment maintenance needed
        ContractExpiring        // Important contract expiring
    }
    
    /// <summary>
    /// Currency display data for UI
    /// </summary>
    [System.Serializable]
    public class CurrencyDisplayData
    {
        public CurrencyType CurrencyType;
        public float Amount;
        public string FormattedAmount;
        public string Icon;
        public Color Color;
        public bool IsPositive;
        public float ChangeAmount;          // Recent change in amount
        public float ChangePercentage;      // Recent change as percentage
        public string ChangeDirection;      // "up", "down", "stable"
        public bool IsFlashing;             // Whether to flash for recent change
    }
    
    /// <summary>
    /// Transaction validation helper
    /// </summary>
    [System.Serializable]
    public class TransactionValidator
    {
        [Header("Validation Rules")]
        public float MaxSingleTransaction = 100000f;
        public float MaxDailySpending = 50000f;
        public List<string> RestrictedCategories = new List<string>();
        
        [Header("Fraud Detection")]
        public bool EnableFraudDetection = true;
        public float SuspiciousAmountThreshold = 10000f;
        public int MaxTransactionsPerMinute = 10;
        
        /// <summary>
        /// Validate a transaction before processing
        /// </summary>
        public bool ValidateTransaction(float amount, TransactionCategory category, string description)
        {
            // Check amount limits
            if (amount > MaxSingleTransaction)
            {
                Debug.LogWarning($"Transaction amount {amount} exceeds single transaction limit");
                return false;
            }
            
            // Check restricted categories
            if (RestrictedCategories.Contains(category.ToString()))
            {
                Debug.LogWarning($"Category {category} is restricted");
                return false;
            }
            
            // Check for suspicious patterns
            if (EnableFraudDetection && amount > SuspiciousAmountThreshold)
            {
                Debug.LogWarning($"Large transaction requires additional verification: {amount}");
                // Would implement additional checks here
            }
            
            return true;
        }
    }
    
    /// <summary>
    /// Economic difficulty settings
    /// </summary>
    [System.Serializable]
    public class EconomicDifficulty
    {
        [Header("Difficulty Settings")]
        public string DifficultyName;
        public float IncomeMultiplier = 1f;     // Multiplier for all income
        public float ExpenseMultiplier = 1f;    // Multiplier for all expenses
        public float LoanInterestModifier = 0f; // Added to loan interest rates
        public float CreditLimitModifier = 1f;  // Multiplier for credit limits
        
        [Header("Market Conditions")]
        public float MarketVolatility = 1f;     // How much prices fluctuate
        public float CompetitionLevel = 1f;     // How aggressive AI competitors are
        public float RegulationLevel = 1f;      // How strict regulations are
        
        [Header("Random Events")]
        public float EventFrequency = 1f;       // Multiplier for economic event frequency
        public float EventSeverity = 1f;        // Multiplier for economic event impact
    }
}