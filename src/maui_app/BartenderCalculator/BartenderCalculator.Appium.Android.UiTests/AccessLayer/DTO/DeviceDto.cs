namespace BartenderCalculator.Appium.Android.UiTests.AccessLayer.DTO;

public class DeviceDto
{
    public DeviceDto(string name, string platformName, string platformVersion)
    {
        Name = name;
        PlatformName = platformName;
        PlatformVersion = platformVersion;
    }

    public string Name { get; set; }
    public string PlatformName { get; set; }
    public string PlatformVersion { get; set; }
}