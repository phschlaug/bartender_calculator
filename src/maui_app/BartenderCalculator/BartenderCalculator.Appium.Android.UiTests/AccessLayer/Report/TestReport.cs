using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;

namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.Report;

public class TestReport
{
    private readonly ExtentReports _extentReports;
    private ExtentTest? _currentTest;
    private string _testReportFolderPath;
    
    public TestReport(string testName)
    {
        var htmlReportFilePath = PrepareTestReportFolder(testName);
        var htmlReporter = new ExtentSparkReporter(htmlReportFilePath);
        _extentReports = new ExtentReports();
        _extentReports.AttachReporter(htmlReporter); 
    }

    public void CreateNewTest(string testName, string description)
    {
        _currentTest = _extentReports.CreateTest(testName, description);
    }

    public void AttachScreenshot(string screenshotPath, string title)
    {
        var media = MediaEntityBuilder.CreateScreenCaptureFromPath(screenshotPath, title).Build();
        _currentTest?.Info(title, media);
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

    public string GetTestReportFilePath()
    {
        return _testReportFolderPath;
    }

    private string PrepareTestReportFolder(string testName)
    {
        string currentFolder = Directory.GetCurrentDirectory();
        var mauiAppFolder = currentFolder.Substring(0, currentFolder.IndexOf("/maui_app/") + 1);
        _testReportFolderPath = Path.Combine(mauiAppFolder, "TestReports");
        
        if (!Directory.Exists(_testReportFolderPath))
        {
            Directory.CreateDirectory(_testReportFolderPath);
        }
        return Path.Combine(_testReportFolderPath, $"{testName}.html");
    }
}