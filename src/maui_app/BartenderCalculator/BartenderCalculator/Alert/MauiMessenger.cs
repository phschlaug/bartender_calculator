namespace BartenderCalculator.Alert;

public class MauiMessenger: IMessenger
{
    public async Task SendAsync(string title, string message, string cancelButtonText)
    {
        var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
        if (mainPage != null)
        {
            await mainPage.DisplayAlert(title, message, cancelButtonText);
        }
    }
}