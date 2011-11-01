using System.Web.Security;
using LoggingServer.Server.Tasks;

namespace LoggingServer.Mvc.Wrappers
{
    public class MembershipWrapper : IMembershipTasks
    {
        public virtual MembershipUser GetUser(string username, bool userIsOnline)
        {
            return Membership.GetUser(username, userIsOnline);
        }

        public virtual bool ValidateUser(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

        public virtual MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            return Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved,
                                         providerUserKey, out status);
        }

        public virtual int MinRequiredPasswordLength
        {
            get { return Membership.MinRequiredPasswordLength; }
        }
    }
}