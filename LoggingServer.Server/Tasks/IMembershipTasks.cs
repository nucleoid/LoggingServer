using System.Web.Security;

namespace LoggingServer.Server.Tasks
{
    public interface IMembershipTasks
    {
        MembershipUser GetUser(string username, bool userIsOnline);
        bool ValidateUser(string username, string password);
        MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
                           string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status);
        int MinRequiredPasswordLength { get; }
    }
}
