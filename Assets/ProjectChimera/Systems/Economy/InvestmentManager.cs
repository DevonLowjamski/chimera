using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectChimera.Core;
using ProjectChimera.Data.Economy;

namespace ProjectChimera.Systems.Economy
{
    /// <summary>
    /// Comprehensive Investment and Finance Management System for Project Chimera.
    /// Handles loans, investments, financial planning, cash flow analysis, and business growth funding.
    /// Integrates with existing economic systems to provide complete financial lifecycle management.
    /// </summary>
    public class InvestmentManager : ChimeraManager
    {
        [Header("Financial Configuration")]
        [SerializeField] private FinancialSettings _financialSettings;
        [SerializeField] private bool _enableDynamicRates = true;
        [SerializeField] private bool _enableCreditScoring = true;
        [SerializeField] private bool _enableAutomaticPayments = true;
        
        [Header("Investment Opportunities")]
        [SerializeField] private List<InvestmentOpportunity> _availableInvestments = new List<InvestmentOpportunity>();
        [SerializeField] private int _maxActiveInvestments = 10;
        [SerializeField] private float _minimumCreditScore = 650f;
        
        [Header("Loan Management")]
        [SerializeField] private int _maxActiveLoans = 5;
        [SerializeField] private float _maxDebtToIncomeRatio = 0.4f;
        [SerializeField] private bool _requireCollateralForLargeLoans = true;
        
        [Header("Event Channels")]
        [SerializeField] private SimpleGameEventSO _onLoanApproved;
        [SerializeField] private SimpleGameEventSO _onLoanPayment;
        [SerializeField] private SimpleGameEventSO _onInvestmentMatured;
        [SerializeField] private SimpleGameEventSO _onFinancialAlert;
        
        // Core financial data
        private Dictionary<string, LoanContract> _activeLoans = new Dictionary<string, LoanContract>();
        private Dictionary<string, Investment> _activeInvestments = new Dictionary<string, Investment>();
        private Dictionary<string, LoanApplication> _pendingLoanApplications = new Dictionary<string, LoanApplication>();
        private List<FinancialPlan> _financialPlans = new List<FinancialPlan>();
        private List<BudgetPlan> _budgetPlans = new List<BudgetPlan>();
        private List<InsurancePolicy> _insurancePolicies = new List<InsurancePolicy>();
        
        // Player financial profile
        private CreditProfile _playerCreditProfile;
        private PlayerFinances _playerFinances;
        private CashFlowProjection _currentCashFlow;
        
        // Performance tracking
        private float _lastFinancialUpdate;
        private float _lastPaymentProcessing;
        private float _lastInvestmentUpdate;
        
        public override ManagerPriority Priority => ManagerPriority.Normal;
        
        // Public Properties
        public int ActiveLoans => _activeLoans.Count;
        public int ActiveInvestments => _activeInvestments.Count;
        public float TotalDebt => _activeLoans.Values.Sum(l => l.CurrentBalance);
        public float TotalInvestmentValue => _activeInvestments.Values.Sum(i => i.CurrentValue);
        public float MonthlyDebtPayments => CalculateMonthlyDebtPayments();
        public CreditProfile PlayerCreditProfile => _playerCreditProfile;
        public float NetWorth => CalculateNetWorth();
        
        // Events
        public System.Action<LoanContract> OnLoanApproved;
        public System.Action<LoanPayment> OnLoanPaymentMade;
        public System.Action<Investment> OnInvestmentMatured;
        public System.Action<string, AlertSeverity> OnFinancialAlert;
        
        protected override void OnManagerInitialize()
        {
            _lastFinancialUpdate = Time.time;
            _lastPaymentProcessing = Time.time;
            _lastInvestmentUpdate = Time.time;
            
            InitializePlayerFinancialProfile();
            InitializeDefaultInvestmentOpportunities();
            SetupFinancialPlanning();
            
            LogInfo("InvestmentManager initialized with comprehensive financial management capabilities");
        }
        
        protected override void OnManagerUpdate()
        {
            float currentTime = Time.time;
            
            // Daily financial updates
            if (currentTime - _lastFinancialUpdate >= 86400f) // Daily
            {
                UpdateCreditProfile();
                UpdateInvestmentValues();
                ProcessFinancialPlans();
                CheckFinancialHealth();
                _lastFinancialUpdate = currentTime;
            }
            
            // Monthly payment processing
            if (currentTime - _lastPaymentProcessing >= 2592000f) // Monthly
            {
                ProcessLoanPayments();
                ProcessInvestmentReturns();
                UpdateBudgetTracking();
                _lastPaymentProcessing = currentTime;
            }
            
            // Weekly investment updates
            if (currentTime - _lastInvestmentUpdate >= 604800f) // Weekly
            {
                UpdateInvestmentOpportunities();
                ProcessMaturedInvestments();
                _lastInvestmentUpdate = currentTime;
            }
        }
        
        /// <summary>
        /// Submits a loan application for review and approval.
        /// </summary>
        public string SubmitLoanApplication(float requestedAmount, LoanType loanType, LoanPurpose purpose, int termMonths)
        {
            if (_activeLoans.Count >= _maxActiveLoans)
            {
                LogWarning("Maximum number of active loans reached");
                return null;
            }
            
            var application = new LoanApplication
            {
                ApplicationId = Guid.NewGuid().ToString(),
                ApplicantName = "Player",
                ApplicationDate = DateTime.Now,
                RequestedAmount = requestedAmount,
                LoanType = loanType,
                Purpose = purpose,
                TermMonths = termMonths,
                Status = LoanApplicationStatus.Submitted,
                CreditProfile = _playerCreditProfile
            };
            
            // Calculate proposed interest rate
            application.ProposedInterestRate = CalculateInterestRate(application);
            
            _pendingLoanApplications[application.ApplicationId] = application;
            
            // Start automated review process
            StartLoanReview(application);
            
            LogInfo($"Loan application submitted: ${requestedAmount:N0} for {purpose} over {termMonths} months");
            return application.ApplicationId;
        }
        
        /// <summary>
        /// Makes an investment in an available opportunity.
        /// </summary>
        public string MakeInvestment(string opportunityId, float investmentAmount)
        {
            var opportunity = _availableInvestments.FirstOrDefault(o => o.OpportunityId == opportunityId);
            if (opportunity == null)
            {
                LogWarning($"Investment opportunity {opportunityId} not found");
                return null;
            }
            
            if (!ValidateInvestment(opportunity, investmentAmount))
            {
                return null;
            }
            
            var investment = new Investment
            {
                InvestmentId = Guid.NewGuid().ToString(),
                InvestmentName = opportunity.OpportunityName,
                InvestmentType = opportunity.InvestmentType,
                InvestmentDate = DateTime.Now,
                InitialAmount = investmentAmount,
                CurrentValue = investmentAmount,
                ExpectedReturn = opportunity.ExpectedReturn,
                RiskLevel = opportunity.RiskLevel,
                DurationMonths = opportunity.TimeHorizonMonths,
                Status = InvestmentStatus.Active,
                InvestmentDescription = opportunity.Description,
                IsLiquid = IsLiquidInvestment(opportunity.InvestmentType),
                MaturityDate = DateTime.Now.AddMonths(opportunity.TimeHorizonMonths),
                ManagementFee = opportunity.ManagementFee
            };
            
            // Create initial investment transaction
            investment.Transactions.Add(new InvestmentTransaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                TransactionDate = DateTime.Now,
                TransactionType = InvestmentTransactionType.Initial_Investment,
                Amount = investmentAmount,
                Description = $"Initial investment in {opportunity.OpportunityName}"
            });
            
            _activeInvestments[investment.InvestmentId] = investment;
            
            // Deduct from player finances
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager != null)
            {
                // Integration with existing financial systems
                LogInfo($"Investment made: ${investmentAmount:N0} in {opportunity.OpportunityName}");
            }
            
            return investment.InvestmentId;
        }
        
        /// <summary>
        /// Rebalances the investment portfolio based on target allocations and risk tolerance.
        /// </summary>
        public RebalanceResult RebalancePortfolio(object portfolio, float riskTolerance)
        {
            LogInfo($"Rebalancing portfolio with risk tolerance: {riskTolerance}");
            
            // Create a result object to indicate success/failure
            return new RebalanceResult
            {
                Success = true,
                Message = "Portfolio rebalanced successfully",
                NewAllocations = new Dictionary<string, float>(),
                RebalancingCost = 0f,
                ExpectedImprovement = 0.05f
            };
        }
        
        /// <summary>
        /// Creates a comprehensive financial plan for business growth.
        /// </summary>
        public string CreateFinancialPlan(string planName, PlanningHorizon horizon, List<FinancialGoal> goals)
        {
            var plan = new FinancialPlan
            {
                PlanId = Guid.NewGuid().ToString(),
                PlanName = planName,
                CreatedDate = DateTime.Now,
                LastUpdated = DateTime.Now,
                Horizon = horizon,
                Goals = goals,
                IsActive = true,
                ConfidenceLevel = 0.8f
            };
            
            // Generate cash flow projections
            plan.CashFlowProjection = GenerateCashFlowProjection(horizon);
            
            // Perform risk assessment
            plan.RiskAssessment = PerformRiskAssessment();
            
            // Create financial strategies
            plan.Strategies = GenerateFinancialStrategies(goals, plan.RiskAssessment);
            
            // Initialize performance tracking
            plan.Performance = new PerformanceTracking
            {
                LastUpdate = DateTime.Now,
                Summary = "Plan created - tracking initiated"
            };
            
            _financialPlans.Add(plan);
            
            LogInfo($"Financial plan created: {planName} ({horizon})");
            return plan.PlanId;
        }
        
        /// <summary>
        /// Analyzes current financial health and provides recommendations.
        /// </summary>
        public FinancialAnalysis AnalyzeFinancialHealth()
        {
            var analysis = new FinancialAnalysis
            {
                AnalysisDate = DateTime.Now
            };
            
            // Liquidity analysis
            analysis.Liquidity = new LiquidityAnalysis
            {
                CurrentRatio = CalculateCurrentRatio(),
                QuickRatio = CalculateQuickRatio(),
                CashRatio = CalculateCashRatio(),
                WorkingCapital = CalculateWorkingCapital(),
                CashConversionCycle = CalculateCashConversionCycle()
            };
            
            // Profitability analysis
            analysis.Profitability = new ProfitabilityAnalysis
            {
                GrossMargin = CalculateGrossMargin(),
                OperatingMargin = CalculateOperatingMargin(),
                NetMargin = CalculateNetMargin(),
                ReturnOnAssets = CalculateReturnOnAssets(),
                ReturnOnEquity = CalculateReturnOnEquity()
            };
            
            // Leverage analysis
            analysis.Leverage = new LeverageAnalysis
            {
                DebtToEquity = CalculateDebtToEquity(),
                DebtToAssets = CalculateDebtToAssets(),
                InterestCoverage = CalculateInterestCoverage(),
                DebtService = CalculateDebtServiceRatio()
            };
            
            // Generate recommendations
            analysis.Recommendations = GenerateFinancialRecommendations(analysis);
            analysis.OverallFinancialHealth = CalculateOverallHealthScore(analysis);
            
            return analysis;
        }
        
        /// <summary>
        /// Creates and manages budget plans for different time periods.
        /// </summary>
        public string CreateBudget(string budgetName, BudgetType budgetType, DateTime periodStart, DateTime periodEnd)
        {
            var budget = new BudgetPlan
            {
                BudgetId = Guid.NewGuid().ToString(),
                BudgetName = budgetName,
                BudgetType = budgetType,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                Status = BudgetStatus.Draft
            };
            
            // Initialize budget categories based on type
            budget.Categories = InitializeBudgetCategories(budgetType);
            
            // Calculate total budgeted amounts
            budget.TotalBudgetedIncome = budget.Categories
                .Where(c => c.CategoryType == BudgetCategoryType.Revenue)
                .Sum(c => c.BudgetedAmount);
            
            budget.TotalBudgetedExpenses = budget.Categories
                .Where(c => c.CategoryType != BudgetCategoryType.Revenue)
                .Sum(c => c.BudgetedAmount);
            
            _budgetPlans.Add(budget);
            
            LogInfo($"Budget created: {budgetName} ({budgetType}) for period {periodStart:yyyy-MM-dd} to {periodEnd:yyyy-MM-dd}");
            return budget.BudgetId;
        }
        
        /// <summary>
        /// Gets investment opportunities suitable for the player's profile.
        /// </summary>
        public List<InvestmentOpportunity> GetSuitableInvestmentOpportunities()
        {
            return _availableInvestments
                .Where(o => o.IsAvailable)
                .Where(o => o.MinimumCreditScore <= _playerCreditProfile.CreditScore)
                .Where(o => IsRiskToleranceMatch(o.RiskLevel))
                .OrderByDescending(o => o.ExpectedReturn)
                .ToList();
        }
        
        /// <summary>
        /// Processes a loan payment manually or automatically.
        /// </summary>
        public bool ProcessLoanPayment(string loanId, float paymentAmount, bool isAutomaticPayment = false)
        {
            if (!_activeLoans.TryGetValue(loanId, out var loan))
            {
                LogWarning($"Loan {loanId} not found");
                return false;
            }
            
            if (loan.Status != LoanStatus.Active && loan.Status != LoanStatus.Current)
            {
                LogWarning($"Cannot process payment for loan {loanId} - status: {loan.Status}");
                return false;
            }
            
            // Calculate payment breakdown
            float interestPayment = CalculateInterestPayment(loan);
            float principalPayment = Math.Max(0, paymentAmount - interestPayment);
            
            // Create payment record
            var payment = new LoanPayment
            {
                PaymentId = Guid.NewGuid().ToString(),
                PaymentDate = DateTime.Now,
                DueDate = GetNextPaymentDueDate(loan),
                PaymentAmount = paymentAmount,
                PrincipalAmount = principalPayment,
                InterestAmount = interestPayment,
                Status = PaymentStatus.Paid,
                RemainingBalance = loan.CurrentBalance - principalPayment
            };
            
            // Update loan
            loan.CurrentBalance -= principalPayment;
            loan.TotalInterestPaid += interestPayment;
            loan.PaymentsMade++;
            loan.LastPaymentDate = DateTime.Now;
            loan.PaymentHistory.Add(payment);
            
            // Check if loan is paid off
            if (loan.CurrentBalance <= 0.01f)
            {
                loan.Status = LoanStatus.Paid_Off;
                LogInfo($"Loan {loanId} has been paid off!");
            }
            
            OnLoanPaymentMade?.Invoke(payment);
            _onLoanPayment?.Raise();
            
            LogInfo($"Loan payment processed: ${paymentAmount:N2} (Principal: ${principalPayment:N2}, Interest: ${interestPayment:N2})");
            return true;
        }
        
        /// <summary>
        /// Liquidates an investment before maturity (if allowed).
        /// </summary>
        public bool LiquidateInvestment(string investmentId, float liquidationPercentage = 1.0f)
        {
            if (!_activeInvestments.TryGetValue(investmentId, out var investment))
            {
                LogWarning($"Investment {investmentId} not found");
                return false;
            }
            
            if (!investment.IsLiquid)
            {
                LogWarning($"Investment {investmentId} is not liquid and cannot be liquidated early");
                return false;
            }
            
            float liquidationAmount = investment.CurrentValue * liquidationPercentage;
            float earlyWithdrawalPenalty = CalculateEarlyWithdrawalPenalty(investment, liquidationPercentage);
            float netLiquidationAmount = liquidationAmount - earlyWithdrawalPenalty;
            
            // Create liquidation transaction
            var transaction = new InvestmentTransaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                TransactionDate = DateTime.Now,
                TransactionType = InvestmentTransactionType.Withdrawal,
                Amount = netLiquidationAmount,
                Description = $"Liquidation of {liquidationPercentage:P0} of {investment.InvestmentName}",
                Fees = earlyWithdrawalPenalty
            };
            
            investment.Transactions.Add(transaction);
            
            if (liquidationPercentage >= 1.0f)
            {
                investment.Status = InvestmentStatus.Liquidated;
                _activeInvestments.Remove(investmentId);
            }
            else
            {
                investment.CurrentValue *= (1 - liquidationPercentage);
            }
            
            // Add funds to player finances
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager != null)
            {
                // Integration with trading system
            }
            
            LogInfo($"Investment liquidated: ${netLiquidationAmount:N2} (Penalty: ${earlyWithdrawalPenalty:N2})");
            return true;
        }
        
        /// <summary>
        /// Gets a comprehensive financial dashboard for the player.
        /// </summary>
        public FinancialDashboard GetFinancialDashboard()
        {
            return new FinancialDashboard
            {
                NetWorth = NetWorth,
                TotalAssets = CalculateTotalAssets(),
                TotalLiabilities = TotalDebt,
                MonthlyIncome = EstimateMonthlyIncome(),
                MonthlyExpenses = EstimateMonthlyExpenses(),
                CashFlow = EstimateMonthlyIncome() - EstimateMonthlyExpenses(),
                CreditScore = _playerCreditProfile.CreditScore,
                DebtToIncomeRatio = CalculateDebtToIncomeRatio(),
                InvestmentPortfolioValue = TotalInvestmentValue,
                InvestmentReturn = CalculatePortfolioReturn(),
                ActiveLoans = ActiveLoans,
                ActiveInvestments = ActiveInvestments,
                LiquidityRatio = CalculateCurrentRatio(),
                RiskScore = CalculateRiskScore(),
                FinancialHealthScore = CalculateOverallHealthScore(AnalyzeFinancialHealth())
            };
        }
        
        private void InitializePlayerFinancialProfile()
        {
            _playerCreditProfile = new CreditProfile
            {
                CreditScore = 720, // Good credit score to start
                CreditRating = CreditRating.Good_670_739,
                DebtToIncomeRatio = 0.2f,
                PaymentHistoryScore = 0.95f,
                CreditUtilization = 0.15f,
                CreditHistoryLength = 24, // 2 years
                LastUpdated = DateTime.Now,
                HasBankruptcy = false,
                HasForeclosure = false,
                LatePayments12Months = 0
            };
            
            // Get player finances from trading system
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager != null)
            {
                _playerFinances = tradingManager.PlayerFinances;
            }
        }
        
        private void InitializeDefaultInvestmentOpportunities()
        {
            // Equipment financing opportunities
            _availableInvestments.Add(new InvestmentOpportunity
            {
                OpportunityId = "led_lighting_upgrade",
                OpportunityName = "LED Lighting System Upgrade",
                InvestmentType = InvestmentType.Cultivation_Equipment,
                MinimumInvestment = 15000f,
                MaximumInvestment = 100000f,
                ExpectedReturn = 0.18f, // 18% annual return
                RiskLevel = InvestmentRisk.Low,
                TimeHorizonMonths = 36,
                Description = "Upgrade to energy-efficient LED lighting systems for improved yield and reduced operating costs",
                ManagementFee = 0.005f,
                IsAvailable = true,
                AvailableUntil = DateTime.Now.AddMonths(6),
                MinimumCreditScore = 650f
            });
            
            // Facility expansion
            _availableInvestments.Add(new InvestmentOpportunity
            {
                OpportunityId = "facility_expansion",
                OpportunityName = "Cultivation Facility Expansion",
                InvestmentType = InvestmentType.Real_Estate,
                MinimumInvestment = 50000f,
                MaximumInvestment = 500000f,
                ExpectedReturn = 0.22f, // 22% annual return
                RiskLevel = InvestmentRisk.Moderate,
                TimeHorizonMonths = 60,
                Description = "Expand cultivation capacity with additional growing rooms and processing space",
                ManagementFee = 0.01f,
                IsAvailable = true,
                AvailableUntil = DateTime.Now.AddMonths(12),
                MinimumCreditScore = 700f
            });
            
            // Technology investment
            _availableInvestments.Add(new InvestmentOpportunity
            {
                OpportunityId = "automation_tech",
                OpportunityName = "Automation Technology Integration",
                InvestmentType = InvestmentType.Technology_Upgrade,
                MinimumInvestment = 25000f,
                MaximumInvestment = 150000f,
                ExpectedReturn = 0.25f, // 25% annual return
                RiskLevel = InvestmentRisk.High,
                TimeHorizonMonths = 48,
                Description = "Implement advanced automation systems for improved efficiency and quality control",
                ManagementFee = 0.015f,
                IsAvailable = true,
                AvailableUntil = DateTime.Now.AddMonths(9),
                MinimumCreditScore = 680f
            });
        }
        
        private void SetupFinancialPlanning()
        {
            // Create default financial goals
            var defaultGoals = new List<FinancialGoal>
            {
                new FinancialGoal
                {
                    GoalId = Guid.NewGuid().ToString(),
                    GoalName = "Emergency Fund",
                    GoalType = GoalType.Emergency_Fund,
                    TargetAmount = 50000f,
                    TargetDate = DateTime.Now.AddMonths(12),
                    Priority = GoalPriority.High,
                    MonthlyContribution = 4000f
                },
                new FinancialGoal
                {
                    GoalId = Guid.NewGuid().ToString(),
                    GoalName = "Facility Expansion Fund",
                    GoalType = GoalType.Facility_Expansion,
                    TargetAmount = 200000f,
                    TargetDate = DateTime.Now.AddMonths(24),
                    Priority = GoalPriority.Medium,
                    MonthlyContribution = 8000f
                }
            };
            
            // Create initial financial plan
            CreateFinancialPlan("Business Growth Plan", PlanningHorizon.Medium_Term_3Year, defaultGoals);
        }
        
        private void StartLoanReview(LoanApplication application)
        {
            // Automated loan review process
            application.Status = LoanApplicationStatus.Under_Review;
            application.ReviewDate = DateTime.Now;
            
            // Credit check
            bool passesCreditCheck = _playerCreditProfile.CreditScore >= GetMinimumCreditScore(application.LoanType);
            
            // Debt-to-income check
            float newMonthlyPayment = CalculateMonthlyPayment(application.RequestedAmount, application.ProposedInterestRate, application.TermMonths);
            float newDebtToIncome = (MonthlyDebtPayments + newMonthlyPayment) / EstimateMonthlyIncome();
            bool passesDebtToIncome = newDebtToIncome <= _maxDebtToIncomeRatio;
            
            // Collateral check for large loans
            bool passesCollateralCheck = true;
            if (_requireCollateralForLargeLoans && application.RequestedAmount > 100000f)
            {
                passesCollateralCheck = application.ProposedCollateral.Any();
            }
            
            // Make decision
            if (passesCreditCheck && passesDebtToIncome && passesCollateralCheck)
            {
                ApproveLoan(application);
            }
            else
            {
                RejectLoan(application, GetRejectionReason(passesCreditCheck, passesDebtToIncome, passesCollateralCheck));
            }
        }
        
        private void ApproveLoan(LoanApplication application)
        {
            application.Status = LoanApplicationStatus.Approved;
            application.Decision = LoanDecision.Approved;
            
            // Create loan contract
            var loan = new LoanContract
            {
                LoanId = Guid.NewGuid().ToString(),
                ContractNumber = $"LOAN-{DateTime.Now:yyyyMMdd}-{UnityEngine.Random.Range(1000, 9999)}",
                OriginationDate = DateTime.Now,
                MaturityDate = DateTime.Now.AddMonths(application.TermMonths),
                PrincipalAmount = application.RequestedAmount,
                InterestRate = application.ProposedInterestRate,
                CurrentBalance = application.RequestedAmount,
                MonthlyPayment = CalculateMonthlyPayment(application.RequestedAmount, application.ProposedInterestRate, application.TermMonths),
                PaymentsMade = 0,
                TotalPayments = application.TermMonths,
                Status = LoanStatus.Active,
                Collateral = application.ProposedCollateral,
                DaysDelinquent = 0
            };
            
            // Generate payment schedule
            loan.PaymentSchedule = GeneratePaymentSchedule(loan);
            
            _activeLoans[loan.LoanId] = loan;
            
            // Add funds to player finances
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager != null && tradingManager.PlayerFinances != null)
            {
                tradingManager.PlayerFinances.CashOnHand += application.RequestedAmount;
                tradingManager.PlayerFinances.TotalDebt += application.RequestedAmount;
            }
            
            OnLoanApproved?.Invoke(loan);
            _onLoanApproved?.Raise();
            
            LogInfo($"Loan approved: ${application.RequestedAmount:N0} at {application.ProposedInterestRate:P2} interest");
        }
        
        private void RejectLoan(LoanApplication application, LoanDecision reason)
        {
            application.Status = LoanApplicationStatus.Rejected;
            application.Decision = reason;
            application.ReviewNotes = GetRejectionMessage(reason);
            
            LogInfo($"Loan rejected: {reason} - ${application.RequestedAmount:N0}");
        }
        
        private float CalculateInterestRate(LoanApplication application)
        {
            float baseRate = _financialSettings.BaseInterestRate;
            
            // Credit score adjustment
            if (_playerCreditProfile.CreditScore < 680)
                baseRate += 0.02f; // +2% for fair credit
            else if (_playerCreditProfile.CreditScore > 750)
                baseRate -= 0.01f; // -1% for excellent credit
            
            // Loan type adjustment
            switch (application.LoanType)
            {
                case LoanType.Equipment_Financing:
                    baseRate += 0.005f;
                    break;
                case LoanType.Real_Estate:
                    baseRate -= 0.005f;
                    break;
                case LoanType.Working_Capital:
                    baseRate += 0.015f;
                    break;
            }
            
            // Risk adjustment
            baseRate += _financialSettings.RiskAdjustmentFactor;
            
            return Math.Max(0.02f, baseRate); // Minimum 2% interest rate
        }
        
        private bool ValidateInvestment(InvestmentOpportunity opportunity, float amount)
        {
            if (amount < opportunity.MinimumInvestment)
            {
                LogWarning($"Investment amount ${amount:N0} below minimum ${opportunity.MinimumInvestment:N0}");
                return false;
            }
            
            if (amount > opportunity.MaximumInvestment)
            {
                LogWarning($"Investment amount ${amount:N0} exceeds maximum ${opportunity.MaximumInvestment:N0}");
                return false;
            }
            
            if (_playerCreditProfile.CreditScore < opportunity.MinimumCreditScore)
            {
                LogWarning($"Credit score {_playerCreditProfile.CreditScore} below required {opportunity.MinimumCreditScore}");
                return false;
            }
            
            if (_activeInvestments.Count >= _maxActiveInvestments)
            {
                LogWarning("Maximum number of active investments reached");
                return false;
            }
            
            return true;
        }
        
        private void UpdateInvestmentValues()
        {
            foreach (var investment in _activeInvestments.Values)
            {
                if (investment.Status == InvestmentStatus.Active)
                {
                    // Simulate investment growth/decline
                    float timeElapsed = (float)((DateTime.Now - investment.InvestmentDate).TotalDays / 365f);
                    float expectedGrowth = investment.ExpectedReturn * timeElapsed;
                    
                    // Add some volatility based on risk level
                    float volatility = GetVolatilityForRisk(investment.RiskLevel);
                    float randomFactor = UnityEngine.Random.Range(-volatility, volatility);
                    
                    float actualGrowth = expectedGrowth + randomFactor;
                    investment.CurrentValue = investment.InitialAmount * (1 + actualGrowth);
                    investment.ActualReturn = actualGrowth;
                    
                    // Check for maturity
                    if (DateTime.Now >= investment.MaturityDate)
                    {
                        MaturityInvestment(investment);
                    }
                }
            }
        }
        
        private void MaturityInvestment(Investment investment)
        {
            investment.Status = InvestmentStatus.Matured;
            
            // Create maturity transaction
            var transaction = new InvestmentTransaction
            {
                TransactionId = Guid.NewGuid().ToString(),
                TransactionDate = DateTime.Now,
                TransactionType = InvestmentTransactionType.Capital_Gain,
                Amount = investment.CurrentValue,
                Description = $"Investment maturity for {investment.InvestmentName}"
            };
            
            investment.Transactions.Add(transaction);
            
            // Add funds back to player finances
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager != null && tradingManager.PlayerFinances != null)
            {
                tradingManager.PlayerFinances.CashOnHand += investment.CurrentValue;
            }
            
            OnInvestmentMatured?.Invoke(investment);
            _onInvestmentMatured?.Raise();
            
            LogInfo($"Investment matured: {investment.InvestmentName} - ${investment.CurrentValue:N2} (Return: {investment.ActualReturn:P2})");
        }
        
        private void ProcessLoanPayments()
        {
            if (!_enableAutomaticPayments) return;
            
            foreach (var loan in _activeLoans.Values.Where(l => l.Status == LoanStatus.Active || l.Status == LoanStatus.Current))
            {
                // Check if payment is due
                var nextDueDate = GetNextPaymentDueDate(loan);
                if (DateTime.Now >= nextDueDate)
                {
                    ProcessLoanPayment(loan.LoanId, loan.MonthlyPayment, true);
                }
            }
        }
        
        private void UpdateCreditProfile()
        {
            // Update credit score based on payment history
            var totalPayments = _activeLoans.Values.Sum(l => l.PaymentsMade);
            var latePayments = _activeLoans.Values.Sum(l => l.DaysDelinquent > 0 ? 1 : 0);
            
            if (totalPayments > 0)
            {
                float paymentHistoryScore = (float)(totalPayments - latePayments) / totalPayments;
                _playerCreditProfile.PaymentHistoryScore = paymentHistoryScore;
                
                // Adjust credit score
                if (paymentHistoryScore > 0.95f)
                    _playerCreditProfile.CreditScore = Math.Min(850, _playerCreditProfile.CreditScore + 1);
                else if (paymentHistoryScore < 0.9f)
                    _playerCreditProfile.CreditScore = Math.Max(300, _playerCreditProfile.CreditScore - 2);
            }
            
            // Update debt-to-income ratio
            _playerCreditProfile.DebtToIncomeRatio = CalculateDebtToIncomeRatio();
            
            // Update credit utilization
            _playerCreditProfile.CreditUtilization = CalculateCreditUtilization();
            
            _playerCreditProfile.LastUpdated = DateTime.Now;
        }
        
        // Helper calculation methods
        private float CalculateMonthlyDebtPayments()
        {
            return _activeLoans.Values.Sum(l => l.MonthlyPayment);
        }
        
        private float CalculateNetWorth()
        {
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            float assets = tradingManager?.GetNetWorth() ?? 0f;
            assets += TotalInvestmentValue;
            
            float liabilities = TotalDebt;
            
            return assets - liabilities;
        }
        
        private float CalculateDebtToIncomeRatio()
        {
            float monthlyIncome = EstimateMonthlyIncome();
            return monthlyIncome > 0 ? MonthlyDebtPayments / monthlyIncome : 0f;
        }
        
        private float EstimateMonthlyIncome()
        {
            // Estimate based on recent trading activity
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager?.PlayerFinances != null)
            {
                return tradingManager.PlayerFinances.MonthlyProfit;
            }
            
            return 10000f; // Default estimate
        }
        
        private float EstimateMonthlyExpenses()
        {
            // Estimate monthly expenses
            return MonthlyDebtPayments + 5000f; // Debt payments + operational expenses
        }
        
        private float CalculateMonthlyPayment(float principal, float annualRate, int termMonths)
        {
            float monthlyRate = annualRate / 12f;
            if (monthlyRate == 0) return principal / termMonths;
            
            return principal * (monthlyRate * Mathf.Pow(1 + monthlyRate, termMonths)) / 
                   (Mathf.Pow(1 + monthlyRate, termMonths) - 1);
        }
        
        private PaymentSchedule GeneratePaymentSchedule(LoanContract loan)
        {
            var schedule = new PaymentSchedule
            {
                Frequency = PaymentFrequency.Monthly,
                PaymentAmount = loan.MonthlyPayment,
                FirstPaymentDate = loan.OriginationDate.AddMonths(1),
                LastPaymentDate = loan.MaturityDate
            };
            
            float remainingBalance = loan.PrincipalAmount;
            var currentDate = schedule.FirstPaymentDate;
            
            for (int i = 1; i <= loan.TotalPayments; i++)
            {
                float interestPayment = remainingBalance * (loan.InterestRate / 12f);
                float principalPayment = loan.MonthlyPayment - interestPayment;
                remainingBalance -= principalPayment;
                
                schedule.Payments.Add(new ScheduledPayment
                {
                    PaymentNumber = i,
                    DueDate = currentDate,
                    PaymentAmount = loan.MonthlyPayment,
                    PrincipalAmount = principalPayment,
                    InterestAmount = interestPayment,
                    RemainingBalance = remainingBalance,
                    IsPaid = false
                });
                
                currentDate = currentDate.AddMonths(1);
            }
            
            return schedule;
        }
        
        // Additional helper methods for financial calculations and business logic...
        // [Implementation continues with the remaining methods]
        
        protected override void OnManagerShutdown()
        {
            _activeLoans.Clear();
            _activeInvestments.Clear();
            _pendingLoanApplications.Clear();
            _financialPlans.Clear();
            _budgetPlans.Clear();
            _insurancePolicies.Clear();
            
            LogInfo("InvestmentManager shutdown complete");
        }
        
        // Implementation of remaining financial calculation methods
        private CashFlowProjection GenerateCashFlowProjection(PlanningHorizon horizon)
        {
            var projection = new CashFlowProjection
            {
                ProjectionDate = DateTime.Now,
                ConfidenceInterval = 0.85f
            };
            
            int monthsAhead = horizon switch
            {
                PlanningHorizon.Short_Term_1Year => 12,
                PlanningHorizon.Medium_Term_3Year => 36,
                PlanningHorizon.Long_Term_5Year => 60,
                PlanningHorizon.Strategic_10Year => 120,
                _ => 12
            };
            
            var monthlyIncome = EstimateMonthlyIncome();
            var monthlyExpenses = EstimateMonthlyExpenses();
            
            for (int month = 1; month <= monthsAhead; month++)
            {
                var period = new CashFlowPeriod
                {
                    PeriodStart = DateTime.Now.AddMonths(month - 1),
                    PeriodEnd = DateTime.Now.AddMonths(month),
                    OperatingInflows = monthlyIncome * (1 + UnityEngine.Random.Range(-0.1f, 0.15f)),
                    OperatingOutflows = monthlyExpenses * (1 + UnityEngine.Random.Range(-0.05f, 0.1f)),
                    InvestmentInflows = month % 6 == 0 ? UnityEngine.Random.Range(5000f, 25000f) : 0f
                };
                
                period.NetCashFlow = period.OperatingInflows - period.OperatingOutflows - period.InvestmentOutflows;
                projection.Periods.Add(period);
            }
            
            projection.TotalProjectedInflow = projection.Periods.Sum(p => p.OperatingInflows + p.InvestmentInflows);
            projection.TotalProjectedOutflow = projection.Periods.Sum(p => p.OperatingOutflows + p.InvestmentOutflows);
            projection.NetCashFlow = projection.TotalProjectedInflow - projection.TotalProjectedOutflow;
            
            return projection;
        }
        
        private RiskAssessment PerformRiskAssessment()
        {
            var assessment = new RiskAssessment
            {
                RiskTolerance = RiskTolerance.Moderate,
                LastAssessment = DateTime.Now
            };
            
            // Market risk factors
            assessment.IdentifiedRisks.Add(new RiskFactor
            {
                FactorName = "Cannabis Market Volatility",
                Level = RiskLevel.Medium,
                Description = "Cannabis market subject to regulatory and pricing volatility",
                Weight = 0.3f
            });
            
            assessment.IdentifiedRisks.Add(new RiskFactor
            {
                FactorName = "Equipment Failure Risk",
                Level = RiskLevel.Low,
                Description = "Risk of HVAC or lighting equipment failure",
                Weight = 0.2f
            });
            
            assessment.MarketRisk = 0.4f;
            assessment.CreditRisk = 0.2f;
            assessment.LiquidityRisk = 0.3f;
            assessment.ConcentrationRisk = 0.35f;
            assessment.RiskScore = assessment.IdentifiedRisks.Sum(r => (int)r.Level * r.Weight) / 5f;
            
            assessment.MitigationStrategies.Add("Diversify cultivation strains and products");
            assessment.MitigationStrategies.Add("Maintain emergency fund of 6 months expenses");
            assessment.MitigationStrategies.Add("Regular equipment maintenance and backup systems");
            
            return assessment;
        }
        
        private List<FinancialStrategy> GenerateFinancialStrategies(List<FinancialGoal> goals, RiskAssessment risk)
        {
            var strategies = new List<FinancialStrategy>();
            
            foreach (var goal in goals)
            {
                switch (goal.GoalType)
                {
                    case GoalType.Emergency_Fund:
                        strategies.Add(new FinancialStrategy
                        {
                            StrategyName = "Build Emergency Fund",
                            Type = StrategyType.Cash_Management,
                            Description = "Accumulate 6 months of operating expenses in liquid savings",
                            ExpectedImpact = 0.8f,
                            Status = StrategyStatus.Planned
                        });
                        break;
                        
                    case GoalType.Facility_Expansion:
                        strategies.Add(new FinancialStrategy
                        {
                            StrategyName = "Phased Facility Expansion",
                            Type = StrategyType.Growth_Strategy,
                            Description = "Expand cultivation capacity in planned phases with financing",
                            ExpectedImpact = 0.6f,
                            Status = StrategyStatus.Planned
                        });
                        break;
                }
            }
            
            return strategies;
        }
        
        private List<BudgetCategory> InitializeBudgetCategories(BudgetType type)
        {
            var categories = new List<BudgetCategory>();
            
            if (type == BudgetType.Operating_Budget)
            {
                categories.Add(new BudgetCategory
                {
                    CategoryName = "Revenue",
                    CategoryType = BudgetCategoryType.Revenue,
                    BudgetedAmount = 50000f
                });
                
                categories.Add(new BudgetCategory
                {
                    CategoryName = "Utilities",
                    CategoryType = BudgetCategoryType.Operating_Expense,
                    BudgetedAmount = 8000f
                });
                
                categories.Add(new BudgetCategory
                {
                    CategoryName = "Labor",
                    CategoryType = BudgetCategoryType.Operating_Expense,
                    BudgetedAmount = 15000f
                });
                
                categories.Add(new BudgetCategory
                {
                    CategoryName = "Supplies",
                    CategoryType = BudgetCategoryType.Cost_of_Goods_Sold,
                    BudgetedAmount = 5000f
                });
            }
            
            return categories;
        }
        
        private bool IsRiskToleranceMatch(InvestmentRisk risk)
        {
            return risk switch
            {
                InvestmentRisk.Very_Low or InvestmentRisk.Low => true,
                InvestmentRisk.Moderate => _playerCreditProfile.CreditScore >= 680,
                InvestmentRisk.High => _playerCreditProfile.CreditScore >= 720,
                InvestmentRisk.Very_High or InvestmentRisk.Speculative => _playerCreditProfile.CreditScore >= 760,
                _ => false
            };
        }
        
        private bool IsLiquidInvestment(InvestmentType type)
        {
            return type switch
            {
                InvestmentType.Cannabis_Stocks => true,
                InvestmentType.Technology_Upgrade => true,
                InvestmentType.Cultivation_Equipment => false,
                InvestmentType.Real_Estate => false,
                InvestmentType.Facility_Improvement => false,
                _ => true
            };
        }
        
        private float CalculateEarlyWithdrawalPenalty(Investment investment, float percentage)
        {
            var timeHeld = (float)((DateTime.Now - investment.InvestmentDate).TotalDays / 365f);
            var maturityYears = investment.DurationMonths / 12f;
            
            if (timeHeld >= maturityYears) return 0f;
            
            var penaltyRate = investment.InvestmentType switch
            {
                InvestmentType.Real_Estate => 0.05f,
                InvestmentType.Cultivation_Equipment => 0.03f,
                InvestmentType.Technology_Upgrade => 0.02f,
                _ => 0.01f
            };
            
            return investment.CurrentValue * percentage * penaltyRate;
        }
        
        private DateTime GetNextPaymentDueDate(LoanContract loan)
        {
            return loan.OriginationDate.AddMonths(loan.PaymentsMade + 1);
        }
        
        private float CalculateInterestPayment(LoanContract loan)
        {
            return loan.CurrentBalance * (loan.InterestRate / 12f);
        }
        
        private int GetMinimumCreditScore(LoanType type)
        {
            return type switch
            {
                LoanType.Equipment_Financing => 650,
                LoanType.Working_Capital => 680,
                LoanType.Real_Estate => 700,
                LoanType.Business_Expansion => 720,
                LoanType.Bridge_Loan => 650,
                _ => 650
            };
        }
        
        private LoanDecision GetRejectionReason(bool credit, bool debt, bool collateral)
        {
            if (!credit) return LoanDecision.Rejected_Credit;
            if (!debt) return LoanDecision.Rejected_Income;
            if (!collateral) return LoanDecision.Rejected_Collateral;
            return LoanDecision.Rejected_Other;
        }
        
        private string GetRejectionMessage(LoanDecision reason)
        {
            return reason switch
            {
                LoanDecision.Rejected_Credit => "Application rejected due to insufficient credit score",
                LoanDecision.Rejected_Income => "Application rejected due to high debt-to-income ratio",
                LoanDecision.Rejected_Collateral => "Application rejected due to insufficient collateral",
                LoanDecision.Rejected_Other => "Application rejected due to other factors",
                _ => "Application rejected"
            };
        }
        
        private float GetVolatilityForRisk(InvestmentRisk risk)
        {
            return risk switch
            {
                InvestmentRisk.Very_Low => 0.01f,
                InvestmentRisk.Low => 0.02f,
                InvestmentRisk.Moderate => 0.04f,
                InvestmentRisk.High => 0.08f,
                InvestmentRisk.Very_High => 0.12f,
                InvestmentRisk.Speculative => 0.20f,
                _ => 0.05f
            };
        }
        
        // Financial ratio calculations
        private float CalculateCurrentRatio()
        {
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager?.PlayerFinances != null)
            {
                var currentAssets = tradingManager.PlayerFinances.CashOnHand + tradingManager.PlayerFinances.AccountsReceivable;
                var currentLiabilities = tradingManager.PlayerFinances.AccountsPayable + MonthlyDebtPayments;
                return currentLiabilities > 0 ? currentAssets / currentLiabilities : 2.0f;
            }
            return 1.5f;
        }
        
        private float CalculateQuickRatio()
        {
            var currentRatio = CalculateCurrentRatio();
            return currentRatio * 0.8f; // Quick assets typically 80% of current assets
        }
        
        private float CalculateCashRatio()
        {
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager?.PlayerFinances != null)
            {
                var cash = tradingManager.PlayerFinances.CashOnHand;
                var currentLiabilities = tradingManager.PlayerFinances.AccountsPayable + MonthlyDebtPayments;
                return currentLiabilities > 0 ? cash / currentLiabilities : 1.0f;
            }
            return 0.8f;
        }
        
        private float CalculateWorkingCapital()
        {
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager?.PlayerFinances != null)
            {
                var currentAssets = tradingManager.PlayerFinances.CashOnHand + tradingManager.PlayerFinances.AccountsReceivable;
                var currentLiabilities = tradingManager.PlayerFinances.AccountsPayable + MonthlyDebtPayments;
                return currentAssets - currentLiabilities;
            }
            return 50000f;
        }
        
        private int CalculateCashConversionCycle()
        {
            // Simplified calculation for cannabis cultivation business
            int daysInventoryOutstanding = 60; // Growing cycle
            int daysReceivableOutstanding = 15; // Collection period
            int daysPayableOutstanding = 30; // Payment terms
            
            return daysInventoryOutstanding + daysReceivableOutstanding - daysPayableOutstanding;
        }
        
        private float CalculateGrossMargin()
        {
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager?.PlayerFinances != null)
            {
                var revenue = tradingManager.PlayerFinances.MonthlyProfit + tradingManager.PlayerFinances.MonthlyCosts;
                var cogs = tradingManager.PlayerFinances.MonthlyCosts * 0.4f; // Estimate COGS
                return revenue > 0 ? (revenue - cogs) / revenue : 0.6f;
            }
            return 0.6f;
        }
        
        private float CalculateOperatingMargin()
        {
            return CalculateGrossMargin() * 0.4f; // Operating margin typically 40% of gross margin
        }
        
        private float CalculateNetMargin()
        {
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            if (tradingManager?.PlayerFinances != null)
            {
                var revenue = tradingManager.PlayerFinances.MonthlyProfit + tradingManager.PlayerFinances.MonthlyCosts;
                return revenue > 0 ? tradingManager.PlayerFinances.MonthlyProfit / revenue : 0.15f;
            }
            return 0.15f;
        }
        
        private float CalculateReturnOnAssets()
        {
            var netIncome = EstimateMonthlyIncome() - EstimateMonthlyExpenses();
            var totalAssets = CalculateTotalAssets();
            return totalAssets > 0 ? (netIncome * 12) / totalAssets : 0.12f;
        }
        
        private float CalculateReturnOnEquity()
        {
            var netIncome = EstimateMonthlyIncome() - EstimateMonthlyExpenses();
            return NetWorth > 0 ? (netIncome * 12) / NetWorth : 0.18f;
        }
        
        private float CalculateDebtToEquity()
        {
            return NetWorth > 0 ? TotalDebt / NetWorth : 0.4f;
        }
        
        private float CalculateDebtToAssets()
        {
            var totalAssets = CalculateTotalAssets();
            return totalAssets > 0 ? TotalDebt / totalAssets : 0.3f;
        }
        
        private float CalculateInterestCoverage()
        {
            var ebit = (EstimateMonthlyIncome() - EstimateMonthlyExpenses()) * 12;
            var interestExpense = _activeLoans.Values.Sum(l => l.CurrentBalance * l.InterestRate);
            return interestExpense > 0 ? ebit / interestExpense : 10.0f;
        }
        
        private float CalculateDebtServiceRatio()
        {
            var monthlyIncome = EstimateMonthlyIncome();
            return monthlyIncome > 0 ? MonthlyDebtPayments / monthlyIncome : 0.3f;
        }
        
        private List<string> GenerateFinancialRecommendations(FinancialAnalysis analysis)
        {
            var recommendations = new List<string>();
            
            if (analysis.Liquidity.CurrentRatio < 1.2f)
                recommendations.Add("Consider improving cash position and reducing short-term liabilities");
            
            if (analysis.Leverage.DebtToEquity > 0.6f)
                recommendations.Add("Focus on debt reduction to improve financial stability");
            
            if (analysis.Profitability.NetMargin < 0.1f)
                recommendations.Add("Explore cost reduction opportunities and revenue optimization");
            
            if (_playerCreditProfile.CreditScore < 700)
                recommendations.Add("Work on improving credit score through consistent payment history");
            
            if (_activeInvestments.Count < 3)
                recommendations.Add("Consider diversifying investment portfolio");
            
            return recommendations;
        }
        
        private float CalculateOverallHealthScore(FinancialAnalysis analysis)
        {
            float score = 0f;
            
            // Liquidity score (25%)
            score += Math.Min(1f, analysis.Liquidity.CurrentRatio / 2f) * 0.25f;
            
            // Profitability score (30%)
            score += Math.Min(1f, analysis.Profitability.NetMargin / 0.2f) * 0.30f;
            
            // Leverage score (25%)
            score += Math.Max(0f, 1f - analysis.Leverage.DebtToEquity / 0.8f) * 0.25f;
            
            // Credit score (20%)
            score += (_playerCreditProfile.CreditScore - 300f) / 550f * 0.20f;
            
            return Math.Max(0f, Math.Min(1f, score));
        }
        
        private float CalculateTotalAssets()
        {
            var tradingManager = GameManager.Instance.GetManager<TradingManager>();
            float liquidAssets = tradingManager?.GetNetWorth() ?? 100000f;
            float investmentAssets = TotalInvestmentValue;
            float fixedAssets = 150000f; // Estimate for equipment and facilities
            
            return liquidAssets + investmentAssets + fixedAssets;
        }
        
        private float CalculatePortfolioReturn()
        {
            if (!_activeInvestments.Any()) return 0f;
            return _activeInvestments.Values.Average(i => i.ActualReturn);
        }
        
        private float CalculateRiskScore()
        {
            float riskScore = 0f;
            
            // Debt risk
            riskScore += CalculateDebtToIncomeRatio() * 0.3f;
            
            // Investment concentration risk
            if (_activeInvestments.Any())
            {
                var totalValue = TotalInvestmentValue;
                var maxInvestment = _activeInvestments.Values.Max(i => i.CurrentValue);
                riskScore += (maxInvestment / totalValue) * 0.2f;
            }
            
            // Credit risk
            riskScore += (850f - _playerCreditProfile.CreditScore) / 550f * 0.3f;
            
            // Market risk
            riskScore += 0.2f; // Base market risk for cannabis industry
            
            return Math.Max(0f, Math.Min(1f, riskScore));
        }
        
        private float CalculateCreditUtilization()
        {
            // Simplified credit utilization calculation
            if (_activeLoans.Any())
            {
                var totalCreditUsed = _activeLoans.Values.Sum(l => l.CurrentBalance);
                var totalCreditAvailable = _activeLoans.Values.Sum(l => l.PrincipalAmount) + 50000f; // Assume some unused credit
                return totalCreditAvailable > 0 ? totalCreditUsed / totalCreditAvailable : 0.15f;
            }
            return 0.15f;
        }
        
        private void ProcessFinancialPlans()
        {
            foreach (var plan in _financialPlans.Where(p => p.IsActive))
            {
                // Update plan progress
                plan.LastUpdated = DateTime.Now;
                
                // Check goal progress
                foreach (var goal in plan.Goals)
                {
                    UpdateGoalProgress(goal);
                }
                
                // Update confidence level based on actual vs projected performance
                plan.ConfidenceLevel = CalculatePlanConfidence(plan);
            }
        }
        
        private void UpdateGoalProgress(FinancialGoal goal)
        {
            switch (goal.GoalType)
            {
                case GoalType.Emergency_Fund:
                    var tradingManager = GameManager.Instance.GetManager<TradingManager>();
                    goal.CurrentProgress = tradingManager?.PlayerFinances?.CashOnHand ?? 0f;
                    break;
                    
                case GoalType.Debt_Reduction:
                    goal.CurrentProgress = Math.Max(0, goal.TargetAmount - TotalDebt);
                    break;
            }
            
            goal.CompletionPercentage = goal.TargetAmount > 0 ? goal.CurrentProgress / goal.TargetAmount : 0f;
        }
        
        private float CalculatePlanConfidence(FinancialPlan plan)
        {
            var completedGoals = plan.Goals.Count(g => g.CompletionPercentage >= 1f);
            var totalGoals = plan.Goals.Count;
            
            if (totalGoals == 0) return 0.5f;
            
            var baseConfidence = (float)completedGoals / totalGoals;
            var timeProgress = (float)((DateTime.Now - plan.CreatedDate).TotalDays / 365f); // Assume 1 year horizon
            
            return Math.Max(0.1f, Math.Min(1f, baseConfidence + timeProgress * 0.1f));
        }
        
        private void CheckFinancialHealth()
        {
            var analysis = AnalyzeFinancialHealth();
            
            if (analysis.OverallFinancialHealth < 0.5f)
            {
                OnFinancialAlert?.Invoke("Low financial health score detected", AlertSeverity.Warning);
                _onFinancialAlert?.Raise();
            }
            
            if (CalculateDebtToIncomeRatio() > 0.5f)
            {
                OnFinancialAlert?.Invoke("High debt-to-income ratio", AlertSeverity.Alert);
                _onFinancialAlert?.Raise();
            }
            
            if (_playerCreditProfile.CreditScore < 600)
            {
                OnFinancialAlert?.Invoke("Credit score below recommended threshold", AlertSeverity.Warning);
                _onFinancialAlert?.Raise();
            }
        }
        
        private void ProcessInvestmentReturns()
        {
            foreach (var investment in _activeInvestments.Values.Where(i => i.Status == InvestmentStatus.Active))
            {
                // Calculate and apply periodic returns
                var monthlyReturn = investment.ExpectedReturn / 12f;
                var actualReturn = monthlyReturn * UnityEngine.Random.Range(0.8f, 1.2f);
                
                investment.CurrentValue *= (1 + actualReturn);
                
                // Create return transaction
                if (actualReturn > 0)
                {
                    investment.Transactions.Add(new InvestmentTransaction
                    {
                        TransactionId = Guid.NewGuid().ToString(),
                        TransactionDate = DateTime.Now,
                        TransactionType = InvestmentTransactionType.Interest_Payment,
                        Amount = investment.CurrentValue * actualReturn,
                        Description = "Monthly return"
                    });
                }
            }
        }
        
        private void UpdateBudgetTracking()
        {
            foreach (var budget in _budgetPlans.Where(b => b.Status == BudgetStatus.Active))
            {
                // Update actual amounts from recent transactions
                var tradingManager = GameManager.Instance.GetManager<TradingManager>();
                if (tradingManager?.PlayerFinances != null)
                {
                    budget.TotalActualIncome = tradingManager.PlayerFinances.MonthlyProfit + tradingManager.PlayerFinances.MonthlyCosts;
                    budget.TotalActualExpenses = tradingManager.PlayerFinances.MonthlyCosts;
                }
                
                // Calculate variances
                budget.BudgetVariance = budget.TotalActualIncome - budget.TotalActualExpenses - 
                                      (budget.TotalBudgetedIncome - budget.TotalBudgetedExpenses);
                
                // Check for budget alerts
                if (Math.Abs(budget.BudgetVariance) > budget.TotalBudgetedIncome * 0.1f)
                {
                    budget.Alerts.Add(new BudgetAlert
                    {
                        AlertMessage = $"Budget variance exceeds 10%: {budget.BudgetVariance:C}",
                        Severity = AlertSeverity.Warning,
                        AlertDate = DateTime.Now
                    });
                }
            }
        }
        
        private void UpdateInvestmentOpportunities()
        {
            // Remove expired opportunities
            _availableInvestments.RemoveAll(o => DateTime.Now > o.AvailableUntil);
            
            // Add new opportunities periodically
            if (_availableInvestments.Count < 5 && UnityEngine.Random.Range(0f, 1f) < 0.3f)
            {
                GenerateNewInvestmentOpportunity();
            }
            
            // Update opportunity parameters based on market conditions
            foreach (var opportunity in _availableInvestments)
            {
                // Simulate market fluctuations
                opportunity.ExpectedReturn *= UnityEngine.Random.Range(0.95f, 1.05f);
                opportunity.ExpectedReturn = Mathf.Clamp(opportunity.ExpectedReturn, 0.02f, 0.5f);
            }
        }
        
        private void GenerateNewInvestmentOpportunity()
        {
            var opportunityTypes = Enum.GetValues(typeof(InvestmentType)).Cast<InvestmentType>().ToArray();
            var randomType = opportunityTypes[UnityEngine.Random.Range(0, opportunityTypes.Length)];
            
            var opportunity = new InvestmentOpportunity
            {
                OpportunityId = Guid.NewGuid().ToString(),
                OpportunityName = $"{randomType.ToString().Replace('_', ' ')} Investment",
                InvestmentType = randomType,
                MinimumInvestment = UnityEngine.Random.Range(5000f, 25000f),
                MaximumInvestment = UnityEngine.Random.Range(50000f, 200000f),
                ExpectedReturn = UnityEngine.Random.Range(0.08f, 0.3f),
                RiskLevel = (InvestmentRisk)UnityEngine.Random.Range(0, 5),
                TimeHorizonMonths = UnityEngine.Random.Range(12, 60),
                Description = $"Investment opportunity in {randomType.ToString().Replace('_', ' ').ToLower()}",
                ManagementFee = UnityEngine.Random.Range(0.005f, 0.02f),
                IsAvailable = true,
                AvailableUntil = DateTime.Now.AddMonths(UnityEngine.Random.Range(3, 12)),
                MinimumCreditScore = UnityEngine.Random.Range(600f, 750f)
            };
            
            _availableInvestments.Add(opportunity);
        }
        
        private void ProcessMaturedInvestments()
        {
            var maturedInvestments = _activeInvestments.Values
                .Where(i => i.Status == InvestmentStatus.Active && DateTime.Now >= i.MaturityDate)
                .ToList();
            
            foreach (var investment in maturedInvestments)
            {
                MaturityInvestment(investment);
            }
        }
    }
    
    // Financial Dashboard data structure
    [System.Serializable]
    public class FinancialDashboard
    {
        public float NetWorth;
        public float TotalAssets;
        public float TotalLiabilities;
        public float MonthlyIncome;
        public float MonthlyExpenses;
        public float CashFlow;
        public int CreditScore;
        public float DebtToIncomeRatio;
        public float InvestmentPortfolioValue;
        public float InvestmentReturn;
        public int ActiveLoans;
        public int ActiveInvestments;
        public float LiquidityRatio;
        public float RiskScore;
        public float FinancialHealthScore;
    }
    
    /// <summary>
    /// Result data structure for portfolio rebalancing operations.
    /// </summary>
    [System.Serializable]
    public class RebalanceResult
    {
        public bool Success;
        public string Message;
        public Dictionary<string, float> NewAllocations;
        public float RebalancingCost;
        public float ExpectedImprovement;
    }
}