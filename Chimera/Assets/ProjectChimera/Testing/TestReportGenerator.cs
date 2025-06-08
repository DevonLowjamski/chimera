using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ProjectChimera.Testing.Integration;
using ProjectChimera.Testing.Performance;
using System;

namespace ProjectChimera.Testing
{
    /// <summary>
    /// Comprehensive test report generator for Project Chimera testing systems.
    /// Exports test results to multiple formats for analysis and documentation.
    /// </summary>
    public class TestReportGenerator : MonoBehaviour
    {
        [Header("Report Configuration")]
        [SerializeField] private bool _generateHTMLReports = true;
        [SerializeField] private bool _generateJSONReports = true;
        [SerializeField] private bool _generateCSVReports = true;
        [SerializeField] private string _reportDirectory = "TestReports";
        [SerializeField] private bool _includeDetailedMetrics = true;
        [SerializeField] private bool _includeCharts = true;
        
        [Header("Report Components")]
        [SerializeField] private CultivationTestCoordinator _coordinator;
        [SerializeField] private AdvancedCultivationTestRunner _testRunner;
        [SerializeField] private CultivationIntegrationTests _integrationTests;
        [SerializeField] private CultivationPerformanceTests _performanceTests;
        
        private string _fullReportPath;
        
        private void Start()
        {
            // Create reports directory
            _fullReportPath = Path.Combine(Application.persistentDataPath, _reportDirectory);
            if (!Directory.Exists(_fullReportPath))
            {
                Directory.CreateDirectory(_fullReportPath);
            }
            
            // Auto-find components if not assigned
            if (_coordinator == null) _coordinator = FindAnyObjectByType<CultivationTestCoordinator>();
            if (_testRunner == null) _testRunner = FindAnyObjectByType<AdvancedCultivationTestRunner>();
            if (_integrationTests == null) _integrationTests = FindAnyObjectByType<CultivationIntegrationTests>();
            if (_performanceTests == null) _performanceTests = FindAnyObjectByType<CultivationPerformanceTests>();
        }
        
        public void GenerateComprehensiveReport()
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string reportName = $"ProjectChimera_TestReport_{timestamp}";
            
            Debug.Log($"Generating comprehensive test report: {reportName}");
            
            if (_generateHTMLReports)
            {
                GenerateHTMLReport(reportName);
            }
            
            if (_generateJSONReports)
            {
                GenerateJSONReport(reportName);
            }
            
            if (_generateCSVReports)
            {
                GenerateCSVReport(reportName);
            }
            
            Debug.Log($"Test reports generated in: {_fullReportPath}");
        }
        
        private void GenerateHTMLReport(string reportName)
        {
            var html = new StringBuilder();
            
            // HTML Header
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html lang=\"en\">");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset=\"UTF-8\">");
            html.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
            html.AppendLine($"    <title>Project Chimera Test Report - {DateTime.Now:yyyy-MM-dd}</title>");
            html.AppendLine("    <style>");
            html.AppendLine(GetHTMLStyles());
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            
            // Report Header
            html.AppendLine("    <div class=\"header\">");
            html.AppendLine("        <h1>Project Chimera - Comprehensive Test Report</h1>");
            html.AppendLine($"        <p class=\"timestamp\">Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");
            html.AppendLine($"        <p class=\"environment\">Unity {Application.unityVersion} | {Application.platform}</p>");
            html.AppendLine("    </div>");
            
            // Executive Summary
            html.AppendLine("    <div class=\"section\">");
            html.AppendLine("        <h2>Executive Summary</h2>");
            html.AppendLine(GenerateExecutiveSummaryHTML());
            html.AppendLine("    </div>");
            
            // Coordination Results
            if (_coordinator != null && _coordinator.CoordinationResult != null)
            {
                html.AppendLine("    <div class=\"section\">");
                html.AppendLine("        <h2>Test Coordination Results</h2>");
                html.AppendLine(GenerateCoordinationResultsHTML(_coordinator.CoordinationResult));
                html.AppendLine("    </div>");
            }
            
            // Core System Tests
            if (_testRunner != null && _testRunner.TestSuite != null)
            {
                html.AppendLine("    <div class=\"section\">");
                html.AppendLine("        <h2>Core System Test Results</h2>");
                html.AppendLine(GenerateTestSuiteHTML(_testRunner.TestSuite, _testRunner.TestResults));
                html.AppendLine("    </div>");
            }
            
            // Integration Tests
            if (_integrationTests != null && _integrationTests.TestSuite != null)
            {
                html.AppendLine("    <div class=\"section\">");
                html.AppendLine("        <h2>Integration Test Results</h2>");
                html.AppendLine(GenerateIntegrationTestsHTML(_integrationTests.TestSuite));
                html.AppendLine("    </div>");
            }
            
            // Performance Tests
            if (_performanceTests != null && _performanceTests.TestSuite != null)
            {
                html.AppendLine("    <div class=\"section\">");
                html.AppendLine("        <h2>Performance Test Results</h2>");
                html.AppendLine(GeneratePerformanceTestsHTML(_performanceTests.TestSuite));
                html.AppendLine("    </div>");
            }
            
            // Detailed Metrics
            if (_includeDetailedMetrics)
            {
                html.AppendLine("    <div class=\"section\">");
                html.AppendLine("        <h2>Detailed Metrics</h2>");
                html.AppendLine(GenerateDetailedMetricsHTML());
                html.AppendLine("    </div>");
            }
            
            // Recommendations
            html.AppendLine("    <div class=\"section\">");
            html.AppendLine("        <h2>Recommendations</h2>");
            html.AppendLine(GenerateRecommendationsHTML());
            html.AppendLine("    </div>");
            
            // HTML Footer
            html.AppendLine("    <div class=\"footer\">");
            html.AppendLine("        <p>Generated by Project Chimera Testing System</p>");
            html.AppendLine("    </div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");
            
            // Write to file
            string filePath = Path.Combine(_fullReportPath, $"{reportName}.html");
            File.WriteAllText(filePath, html.ToString());
        }
        
        private void GenerateJSONReport(string reportName)
        {
            var report = new
            {
                metadata = new
                {
                    reportName = reportName,
                    generatedAt = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    unityVersion = Application.unityVersion,
                    platform = Application.platform.ToString(),
                    testingSystemVersion = "1.0.0"
                },
                coordination = _coordinator?.CoordinationResult,
                coreTests = _testRunner?.TestSuite,
                integrationTests = _integrationTests?.TestSuite,
                performanceTests = _performanceTests?.TestSuite,
                summary = GenerateTestSummary()
            };
            
            string json = JsonUtility.ToJson(report, true);
            string filePath = Path.Combine(_fullReportPath, $"{reportName}.json");
            File.WriteAllText(filePath, json);
        }
        
        private void GenerateCSVReport(string reportName)
        {
            var csv = new StringBuilder();
            
            // CSV Header
            csv.AppendLine("TestCategory,TestName,Status,Details,Timestamp,Duration,MemoryUsage,FrameTime");
            
            // Add coordination results
            if (_coordinator?.CoordinationResult != null)
            {
                var result = _coordinator.CoordinationResult;
                csv.AppendLine($"Coordination,Overall,{result.OverallStatus},{result.ErrorMessage ?? "Success"},{result.StartTime:yyyy-MM-dd HH:mm:ss},{result.TotalDuration:F2},,");
            }
            
            // Add core test results
            if (_testRunner?.TestResults != null)
            {
                foreach (var test in _testRunner.TestResults)
                {
                    var status = test.Passed ? "PASS" : "FAIL";
                    csv.AppendLine($"Core,{test.Name},{status},{test.Details},{test.Timestamp:yyyy-MM-dd HH:mm:ss},,,");
                }
            }
            
            // Add integration test results
            if (_integrationTests?.TestResults != null)
            {
                foreach (var test in _integrationTests.TestResults)
                {
                    var status = test.Passed ? "PASS" : "FAIL";
                    csv.AppendLine($"Integration,{test.Name},{status},{test.Details},{test.Timestamp:yyyy-MM-dd HH:mm:ss},,,");
                }
            }
            
            // Add performance test results
            if (_performanceTests?.TestResults != null)
            {
                foreach (var test in _performanceTests.TestResults)
                {
                    string memoryUsage = test.Metrics?.AverageMemoryUsage.ToString("F1") ?? "";
                    string frameTime = test.Metrics?.AverageFrameTime.ToString("F2") ?? "";
                    var status = test.Passed ? "PASS" : "FAIL";
                    csv.AppendLine($"Performance,{test.Name},{status},{test.Details},{test.Timestamp:yyyy-MM-dd HH:mm:ss},,{memoryUsage},{frameTime}");
                }
            }
            
            string filePath = Path.Combine(_fullReportPath, $"{reportName}.csv");
            File.WriteAllText(filePath, csv.ToString());
        }
        
        private string GetHTMLStyles()
        {
            return @"
                body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0; padding: 20px; background-color: #f5f5f5; }
                .header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; border-radius: 10px; margin-bottom: 20px; }
                .header h1 { margin: 0; font-size: 2.5em; }
                .timestamp, .environment { margin: 5px 0; opacity: 0.9; }
                .section { background: white; margin: 20px 0; padding: 25px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }
                .section h2 { color: #333; border-bottom: 2px solid #667eea; padding-bottom: 10px; }
                .summary-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(250px, 1fr)); gap: 20px; margin: 20px 0; }
                .summary-card { background: #f8f9fa; padding: 20px; border-radius: 8px; border-left: 4px solid #667eea; }
                .summary-card h3 { margin: 0 0 10px 0; color: #333; }
                .summary-card .value { font-size: 2em; font-weight: bold; color: #667eea; }
                .test-table { width: 100%; border-collapse: collapse; margin: 20px 0; }
                .test-table th, .test-table td { padding: 12px; text-align: left; border-bottom: 1px solid #ddd; }
                .test-table th { background-color: #667eea; color: white; }
                .test-table tr:hover { background-color: #f5f5f5; }
                .status-pass { color: #28a745; font-weight: bold; }
                .status-fail { color: #dc3545; font-weight: bold; }
                .status-partial { color: #ffc107; font-weight: bold; }
                .metric-bar { background: #e9ecef; height: 20px; border-radius: 10px; overflow: hidden; margin: 5px 0; }
                .metric-fill { height: 100%; background: linear-gradient(90deg, #28a745, #ffc107, #dc3545); transition: width 0.3s; }
                .footer { text-align: center; margin-top: 40px; padding: 20px; color: #666; }
                .recommendation { background: #e7f3ff; border-left: 4px solid #0066cc; padding: 15px; margin: 10px 0; border-radius: 5px; }
                .warning { background: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 10px 0; border-radius: 5px; }
                .error { background: #f8d7da; border-left: 4px solid #dc3545; padding: 15px; margin: 10px 0; border-radius: 5px; }
            ";
        }
        
        private string GenerateExecutiveSummaryHTML()
        {
            var summary = GenerateTestSummary();
            var html = new StringBuilder();
            
            html.AppendLine("        <div class=\"summary-grid\">");
            html.AppendLine($"            <div class=\"summary-card\">");
            html.AppendLine($"                <h3>Total Tests</h3>");
            html.AppendLine($"                <div class=\"value\">{summary.TotalTests}</div>");
            html.AppendLine($"            </div>");
            html.AppendLine($"            <div class=\"summary-card\">");
            html.AppendLine($"                <h3>Success Rate</h3>");
            html.AppendLine($"                <div class=\"value\">{summary.OverallSuccessRate:F1}%</div>");
            html.AppendLine($"            </div>");
            html.AppendLine($"            <div class=\"summary-card\">");
            html.AppendLine($"                <h3>Failed Tests</h3>");
            html.AppendLine($"                <div class=\"value\">{summary.TotalFailedTests}</div>");
            html.AppendLine($"            </div>");
            html.AppendLine($"            <div class=\"summary-card\">");
            html.AppendLine($"                <h3>Test Duration</h3>");
            html.AppendLine($"                <div class=\"value\">{summary.TotalDuration:F1}s</div>");
            html.AppendLine($"            </div>");
            html.AppendLine("        </div>");
            
            return html.ToString();
        }
        
        private string GenerateCoordinationResultsHTML(TestCoordinationResult result)
        {
            var html = new StringBuilder();
            
            html.AppendLine("        <table class=\"test-table\">");
            html.AppendLine("            <tr><th>Phase</th><th>Status</th><th>Details</th></tr>");
            html.AppendLine($"            <tr><td>Validation</td><td class=\"{GetStatusClass(result.ValidationResult?.OverallStatus.ToString())}\">{result.ValidationResult?.OverallStatus}</td><td>Success Rate: {result.ValidationResult?.SuccessRate:F1}%</td></tr>");
            html.AppendLine($"            <tr><td>Core Tests</td><td class=\"{GetStatusClass(result.CoreTestsStatus.ToString())}\">{result.CoreTestsStatus}</td><td>Passed: {result.CoreTestsPassed}, Failed: {result.CoreTestsFailed}</td></tr>");
            html.AppendLine($"            <tr><td>Cultivation Tests</td><td class=\"{GetStatusClass(result.CultivationTestsStatus.ToString())}\">{result.CultivationTestsStatus}</td><td>Passed: {result.CultivationTestsPassed}, Failed: {result.CultivationTestsFailed}</td></tr>");
            html.AppendLine($"            <tr><td>Integration Tests</td><td class=\"{GetStatusClass(result.IntegrationTestsStatus.ToString())}\">{result.IntegrationTestsStatus}</td><td>Passed: {result.IntegrationTestsPassed}, Failed: {result.IntegrationTestsFailed}</td></tr>");
            html.AppendLine($"            <tr><td>Overall</td><td class=\"{GetStatusClass(result.OverallStatus.ToString())}\">{result.OverallStatus}</td><td>Success Rate: {result.OverallSuccessRate:F1}%</td></tr>");
            html.AppendLine("        </table>");
            
            return html.ToString();
        }
        
        private string GenerateTestSuiteHTML(TestSuite testSuite, List<TestResult> testResults)
        {
            var html = new StringBuilder();
            
            html.AppendLine("        <table class=\"test-table\">");
            html.AppendLine("            <tr><th>Test Name</th><th>Status</th><th>Details</th><th>Timestamp</th></tr>");
            
            foreach (var test in testResults)
            {
                var statusClass = test.Passed ? "status-pass" : "status-fail";
                var statusText = test.Passed ? "PASS" : "FAIL";
                html.AppendLine($"            <tr>");
                html.AppendLine($"                <td>{test.Name}</td>");
                html.AppendLine($"                <td class=\"{statusClass}\">{statusText}</td>");
                html.AppendLine($"                <td>{test.Details}</td>");
                html.AppendLine($"                <td>{test.Timestamp:HH:mm:ss}</td>");
                html.AppendLine($"            </tr>");
            }
            
            html.AppendLine("        </table>");
            return html.ToString();
        }
        
        private string GenerateIntegrationTestsHTML(IntegrationTestSuite testSuite)
        {
            var html = new StringBuilder();
            
            html.AppendLine($"        <p><strong>Success Rate:</strong> {testSuite.SuccessRate:F1}% ({testSuite.PassedTests}/{testSuite.TotalTests})</p>");
            html.AppendLine($"        <p><strong>Duration:</strong> {testSuite.Duration:F1} seconds</p>");
            
            html.AppendLine("        <table class=\"test-table\">");
            html.AppendLine("            <tr><th>Test Name</th><th>Status</th><th>Details</th></tr>");
            
            foreach (var test in testSuite.TestResults)
            {
                var statusClass = test.Passed ? "status-pass" : "status-fail";
                var statusText = test.Passed ? "PASS" : "FAIL";
                html.AppendLine($"            <tr>");
                html.AppendLine($"                <td>{test.Name}</td>");
                html.AppendLine($"                <td class=\"{statusClass}\">{statusText}</td>");
                html.AppendLine($"                <td>{test.Details}</td>");
                html.AppendLine($"            </tr>");
            }
            
            html.AppendLine("        </table>");
            return html.ToString();
        }
        
        private string GeneratePerformanceTestsHTML(PerformanceTestSuite testSuite)
        {
            var html = new StringBuilder();
            
            html.AppendLine($"        <p><strong>Success Rate:</strong> {testSuite.SuccessRate:F1}% ({testSuite.PassedTests}/{testSuite.TotalTests})</p>");
            html.AppendLine($"        <p><strong>Target Frame Rate:</strong> {testSuite.TargetFrameRate} FPS</p>");
            
            if (testSuite.OverallMetrics != null)
            {
                var metrics = testSuite.OverallMetrics;
                html.AppendLine("        <div class=\"summary-grid\">");
                html.AppendLine($"            <div class=\"summary-card\">");
                html.AppendLine($"                <h3>Average Frame Time</h3>");
                html.AppendLine($"                <div class=\"value\">{metrics.AverageFrameTime:F2}ms</div>");
                html.AppendLine($"            </div>");
                html.AppendLine($"            <div class=\"summary-card\">");
                html.AppendLine($"                <h3>Max Frame Time</h3>");
                html.AppendLine($"                <div class=\"value\">{metrics.MaxFrameTime:F2}ms</div>");
                html.AppendLine($"            </div>");
                html.AppendLine($"            <div class=\"summary-card\">");
                html.AppendLine($"                <h3>Average Memory</h3>");
                html.AppendLine($"                <div class=\"value\">{metrics.AverageMemoryUsage:F1}MB</div>");
                html.AppendLine($"            </div>");
                html.AppendLine($"            <div class=\"summary-card\">");
                html.AppendLine($"                <h3>Max Memory</h3>");
                html.AppendLine($"                <div class=\"value\">{metrics.MaxMemoryUsage:F1}MB</div>");
                html.AppendLine($"            </div>");
                html.AppendLine("        </div>");
            }
            
            html.AppendLine("        <table class=\"test-table\">");
            html.AppendLine("            <tr><th>Test Name</th><th>Status</th><th>Details</th></tr>");
            
            foreach (var test in testSuite.TestResults)
            {
                var statusClass = test.Passed ? "status-pass" : "status-fail";
                var statusText = test.Passed ? "PASS" : "FAIL";
                html.AppendLine($"            <tr>");
                html.AppendLine($"                <td>{test.Name}</td>");
                html.AppendLine($"                <td class=\"{statusClass}\">{statusText}</td>");
                html.AppendLine($"                <td>{test.Details}</td>");
                html.AppendLine($"            </tr>");
            }
            
            html.AppendLine("        </table>");
            return html.ToString();
        }
        
        private string GenerateDetailedMetricsHTML()
        {
            var html = new StringBuilder();
            
            html.AppendLine("        <h3>System Information</h3>");
            html.AppendLine("        <table class=\"test-table\">");
            html.AppendLine("            <tr><th>Property</th><th>Value</th></tr>");
            html.AppendLine($"            <tr><td>Unity Version</td><td>{Application.unityVersion}</td></tr>");
            html.AppendLine($"            <tr><td>Platform</td><td>{Application.platform}</td></tr>");
            html.AppendLine($"            <tr><td>System Memory</td><td>{SystemInfo.systemMemorySize}MB</td></tr>");
            html.AppendLine($"            <tr><td>Graphics Memory</td><td>{SystemInfo.graphicsMemorySize}MB</td></tr>");
            html.AppendLine($"            <tr><td>Processor</td><td>{SystemInfo.processorType}</td></tr>");
            html.AppendLine($"            <tr><td>Graphics Device</td><td>{SystemInfo.graphicsDeviceName}</td></tr>");
            html.AppendLine("        </table>");
            
            return html.ToString();
        }
        
        private string GenerateRecommendationsHTML()
        {
            var html = new StringBuilder();
            var summary = GenerateTestSummary();
            
            if (summary.OverallSuccessRate < 90f)
            {
                html.AppendLine("        <div class=\"warning\">");
                html.AppendLine("            <strong>Warning:</strong> Overall success rate is below 90%. Review failed tests and address issues before production deployment.");
                html.AppendLine("        </div>");
            }
            
            if (summary.TotalFailedTests > 0)
            {
                html.AppendLine("        <div class=\"error\">");
                html.AppendLine($"            <strong>Action Required:</strong> {summary.TotalFailedTests} tests failed. Investigate and resolve these issues:");
                html.AppendLine("            <ul>");
                
                // Add specific failed test recommendations
                if (_testRunner?.TestResults != null)
                {
                    foreach (var test in _testRunner.TestResults)
                    {
                        if (!test.Passed)
                        {
                            html.AppendLine($"                <li><strong>{test.Name}:</strong> {test.Details}</li>");
                        }
                    }
                }
                
                html.AppendLine("            </ul>");
                html.AppendLine("        </div>");
            }
            
            html.AppendLine("        <div class=\"recommendation\">");
            html.AppendLine("            <strong>Performance Optimization:</strong> Monitor frame times and memory usage during extended gameplay sessions.");
            html.AppendLine("        </div>");
            
            html.AppendLine("        <div class=\"recommendation\">");
            html.AppendLine("            <strong>Integration Testing:</strong> Run integration tests after any major system changes to ensure compatibility.");
            html.AppendLine("        </div>");
            
            return html.ToString();
        }
        
        private string GetStatusClass(string status)
        {
            switch (status?.ToLower())
            {
                case "passed":
                case "pass":
                case "success":
                    return "status-pass";
                case "failed":
                case "fail":
                case "error":
                    return "status-fail";
                case "partialpass":
                case "warning":
                    return "status-partial";
                default:
                    return "";
            }
        }
        
        private TestSummary GenerateTestSummary()
        {
            var summary = new TestSummary();
            
            // Aggregate from all test sources
            if (_coordinator?.CoordinationResult != null)
            {
                var result = _coordinator.CoordinationResult;
                summary.TotalTests += result.TotalTestsPassed + result.TotalTestsFailed;
                summary.TotalPassedTests += result.TotalTestsPassed;
                summary.TotalFailedTests += result.TotalTestsFailed;
                summary.TotalDuration += result.TotalDuration;
            }
            
            if (_testRunner?.TestResults != null)
            {
                foreach (var test in _testRunner.TestResults)
                {
                    summary.TotalTests++;
                    if (test.Passed) summary.TotalPassedTests++;
                    else summary.TotalFailedTests++;
                }
            }
            
            if (_integrationTests?.TestSuite != null)
            {
                var suite = _integrationTests.TestSuite;
                summary.TotalTests += suite.TotalTests;
                summary.TotalPassedTests += suite.PassedTests;
                summary.TotalFailedTests += suite.FailedTests;
                summary.TotalDuration += suite.Duration;
            }
            
            if (_performanceTests?.TestSuite != null)
            {
                var suite = _performanceTests.TestSuite;
                summary.TotalTests += suite.TotalTests;
                summary.TotalPassedTests += suite.PassedTests;
                summary.TotalFailedTests += suite.FailedTests;
            }
            
            // Calculate success rate
            if (summary.TotalTests > 0)
            {
                summary.OverallSuccessRate = (float)summary.TotalPassedTests / summary.TotalTests * 100f;
            }
            
            return summary;
        }
        
        // Public methods
        [ContextMenu("Generate Comprehensive Report")]
        public void GenerateComprehensiveReportManual()
        {
            GenerateComprehensiveReport();
        }
        
        [ContextMenu("Open Reports Directory")]
        public void OpenReportsDirectory()
        {
            Application.OpenURL(_fullReportPath);
        }
        
        public string GetReportsDirectory()
        {
            return _fullReportPath;
        }
    }
    
    [System.Serializable]
    public class TestSummary
    {
        public int TotalTests;
        public int TotalPassedTests;
        public int TotalFailedTests;
        public float OverallSuccessRate;
        public float TotalDuration;
    }
} 