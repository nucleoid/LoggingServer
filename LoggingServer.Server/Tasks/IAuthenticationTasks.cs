
namespace LoggingServer.Server.Tasks
{
    public interface IAuthenticationTasks
    {
        void SetAuthCookie(string userName, bool createPersistentCookie);
        void SignOut();
    }
}
