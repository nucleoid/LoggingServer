using System;
using System.Web.Security;
using LoggingServer.Server.Tasks;

namespace LoggingServer.Interface.Wrappers
{
    public class FormsAuthWrapper : IAuthenticationTasks
    {
        public virtual void SetAuthCookie(String userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public virtual void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}