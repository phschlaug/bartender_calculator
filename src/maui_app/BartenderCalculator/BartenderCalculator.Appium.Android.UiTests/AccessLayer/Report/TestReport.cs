using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.Report;

public class TestReport
{
    private readonly ExtentReports _extentReports;
    private ExtentTest? _currentTest;
    
    public TestReport(string testName)
    {
        var testReportFilePath = PrepareTestReportFolder(testName);
        var htmlReporter = new ExtentSparkReporter(testReportFilePath);
        _extentReports = new ExtentReports();
        _extentReports.AttachReporter(htmlReporter); 
    }

    public void CreateNewTest(string testName, string description)
    {
        _currentTest = _extentReports.CreateTest(testName, description);
    }

    public void AttachScreenshot(string screenshotPath)
    {
        _currentTest?.AddScreenCaptureFromPath(screenshotPath);
    }

    public void LogInfo(string message)
    {
        _currentTest?.Log(Status.Info, message);
    }

    public void Pass(string message)
    {
        _currentTest?.Pass(message);
    }
    
    public void Fail(string message)
    {
        _currentTest?.Fail(message);
    }

    public void Skip(string reason)
    {
        _currentTest?.Skip(reason);
    }

    public void LogDeviceInfo(DeviceDto deviceInfo)
    {
        _extentReports.AddSystemInfo("Device Name", deviceInfo.Name);
        _extentReports.AddSystemInfo("Platform Name", deviceInfo.PlatformName);
        _extentReports.AddSystemInfo("Platform Version", deviceInfo.PlatformVersion);
    }
    
    public void FinalizeReport()
    {
        _extentReports.Flush();
    }

    private string PrepareTestReportFolder(string testName)
    {
        string currentFolder = Directory.GetCurrentDirectory();
        var rootFolderName = currentFolder.Substring(0, currentFolder.IndexOf("/maui_app/") + 1);
        var pathToTestReports = Path.Combine(rootFolderName, "TestReports");
        if (!Directory.Exists(pathToTestReports))
        {
            Directory.CreateDirectory(pathToTestReports);
        }
        return Path.Combine(pathToTestReports, $"{testName}.html");
    }
}