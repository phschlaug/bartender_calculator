using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.BartenderApp;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.Report;
using NUnit.Framework.Interfaces;

namespace BartenderCalculator.Appium.Android.UiTests.Tests;

public abstract class BaseUiTests
{
    private AppiumServer _appiumServer;
    protected BartenderCalculatorApp BartenderCalculatorApp;
    protected TestReport TestReport;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _appiumServer = new AppiumServer();
        var isRunning = _appiumServer.StartServer();
        if (!isRunning) throw new ApplicationException("Failed to start server");
        var appiumServerUri = new Uri(_appiumServer.GetServerUrl());
        BartenderCalculatorApp = new BartenderCalculatorApp(appiumServerUri);
        var testCase = TestContext.CurrentContext.Test.ClassName;
        TestReport = new TestReport(testCase!);
    }

    [TearDown]
    public void TearDown()
    {
        TestSpecificTearDown();
        var status = TestContext.CurrentContext.Result.Outcome.Status;
        var message = TestContext.CurrentContext.Result.Message;
        var testCaseName = TestContext.CurrentContext.Test.Name;
        switch (status)
        {
            case TestStatus.Passed:
                TestReport.Pass($"{testCaseName} passed: {message}");
                break;
            case TestStatus.Failed:
                TestReport.Fail($"{testCaseName} failed: {message}");
                AttachScreenshot($"{testCaseName}_failed", $"{testCaseName} Failed");
                break;
            case TestStatus.Skipped:
                TestReport.Skip("Skipped");
                break;
        }
    }

    protected virtual void TestSpecificTearDown()
    {
        
    }

    protected void AttachScreenshot(string screenshotName, string title)
    {
        var testReportPath = TestReport.GetTestReportFilePath();
        var screenshot = BartenderCalculatorApp.TakeScreenshot($"{testReportPath}/{screenshotName}");
        TestReport.AttachScreenshot(screenshot, title); 
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        var deviceDto = BartenderCalculatorApp.GetDeviceInfo();
        TestReport.LogDeviceInfo(deviceDto);
        _appiumServer.StopServer();
        TestReport.FinalizeReport();
    }
}