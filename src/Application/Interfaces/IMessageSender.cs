// 1 Add contracts infrastructure, presentation, and application.
namespace Application.interfaces
{
    public interface IMessageSender
    {
        bool Send(string message);
    }
}