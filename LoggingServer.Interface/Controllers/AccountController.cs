using System;
using System.Web.Mvc;
using System.Web.Security;
using LoggingServer.Interface.Attributes;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Tasks;

namespace LoggingServer.Interface.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationTasks _authTasks;
        private readonly IMembershipTasks _membershipTasks;

        public AccountController(IAuthenticationTasks authTasks, IMembershipTasks membershipTasks)
        {
            _authTasks = authTasks;
            _membershipTasks = membershipTasks;
        }

        [AllowAnonymous]
        public ActionResult LogOn()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_membershipTasks.ValidateUser(model.UserName, model.Password))
                {
                    _authTasks.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            _authTasks.SignOut();

            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View(new RegisterModel { MinRequiredPasswordLength = _membershipTasks.MinRequiredPasswordLength });
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                _membershipTasks.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    _authTasks.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, ErrorCodeToString(createStatus));
            }
            model.MinRequiredPasswordLength = _membershipTasks.MinRequiredPasswordLength;
            return View(model);
        }

        public ActionResult ChangePassword()
        {
            return View(new ChangePasswordModel { MinRequiredPasswordLength = _membershipTasks.MinRequiredPasswordLength });
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = _membershipTasks.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                    return RedirectToAction("ChangePasswordSuccess");

                ModelState.AddModelError(string.Empty, "The current password is incorrect or the new password is invalid.");
            }

            model.MinRequiredPasswordLength = _membershipTasks.MinRequiredPasswordLength;
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        internal static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }
}
