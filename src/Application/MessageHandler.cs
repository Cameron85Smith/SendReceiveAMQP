// 16 Handle the message logic.
public class MessageHandler : IMessageHandler
{
    // Handles the incoming message
    public void Handle(string message)
    {
        var receivedName = GetName(message);
        Console.WriteLine($"Hello {receivedName}, I am your father!");
    }

    private string GetName(string data) => data.Split(' ').Last();
}