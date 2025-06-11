using System;
using NUnit.Framework;

namespace ProjectChimera.Testing.Core
{
    /// <summary>
    /// Performance test attribute for Project Chimera testing framework.
    /// Marks tests as performance benchmarks for automated performance tracking.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class PerformanceAttribute : NUnitAttribute, ITestAction
    {
        public ActionTargets Targets => ActionTargets.Test;
        
        /// <summary>
        /// Performance threshold in milliseconds (optional).
        /// </summary>
        public double ThresholdMs { get; set; } = -1;
        
        /// <summary>
        /// Whether to enforce the performance threshold.
        /// </summary>
        public bool EnforceThreshold { get; set; } = false;
        
        /// <summary>
        /// Category for performance tracking.
        /// </summary>
        public string Category { get; set; } = "General";
        
        public PerformanceAttribute() { }
        
        public PerformanceAttribute(double thresholdMs, string category = "General")
        {
            ThresholdMs = thresholdMs;
            Category = category;
            EnforceThreshold = true;
        }
        
        public void BeforeTest(NUnit.Framework.Interfaces.ITest test)
        {
            // Performance tracking setup would go here
            TestContext.WriteLine($"[PERFORMANCE] Starting performance test: {test.Name}");
            if (ThresholdMs > 0)
            {
                TestContext.WriteLine($"[PERFORMANCE] Threshold: {ThresholdMs}ms");
            }
        }
        
        public void AfterTest(NUnit.Framework.Interfaces.ITest test)
        {
            // Performance tracking cleanup would go here
            TestContext.WriteLine($"[PERFORMANCE] Completed performance test: {test.Name}");
        }
    }
} 