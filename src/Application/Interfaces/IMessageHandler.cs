// Create an abstration to handle the message
    public interface IMessageHandler
    {
        void Handle(string message);
    }