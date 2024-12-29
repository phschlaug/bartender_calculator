namespace BartenderCalculator.Alert;

public interface IMessenger
{
    Task SendAsync(string title, string message, string cancelButtonText);
}