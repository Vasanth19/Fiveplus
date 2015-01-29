using System;
using System.Web.Http;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;
using Fiveplus.Kicker.Helpers;
using Fiveplus.Kicker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Fiveplus.Kicker.Controllers
{
    [System.Web.Mvc.Authorize]
    [System.Web.Mvc.RoutePrefix("manage")]
    public class ProfileController : Controller
    {
        public ProfileController()
        {
        }

        public ProfileController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        [System.Web.Mvc.Route("profileconfig")]
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult ProfileConfig()
        {
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = UserManager.GetPhoneNumber(User.Identity.GetUserId()),
                TwoFactor = UserManager.GetTwoFactorEnabled(User.Identity.GetUserId()),
                Logins = UserManager.GetLogins(User.Identity.GetUserId()),
                BrowserRemembered = AuthenticationManager.TwoFactorBrowserRemembered(User.Identity.GetUserId())
            };
            return  new JsonCamelCaseResult(model, JsonRequestBehavior.AllowGet);

            //return new HttpStatusCodeResult(HttpStatusCode.OK);  // OK = 200
        }

        [System.Web.Mvc.Route("basicprofile")]
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult BasicProfile()
        {

            IdentityContext context = HttpContext.GetOwinContext().Get<IdentityContext>();

            var userDetail = context.UserDetails.Find(User.Identity.GetUserId());
            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            var model = new 
            {
                UserName = User.Identity.GetUserName(),
                Email = UserManager.GetEmail(User.Identity.GetUserId()),
                Location = currentUser.Location,
                TimeZone = userDetail.Timezone,
                ProfileImg = String.IsNullOrEmpty(userDetail.ProfileImg)? "~/assets/img/common/NoImage.png":userDetail.ProfileImg
                
            };
            return new JsonCamelCaseResult(model, JsonRequestBehavior.AllowGet);

           // return Json(model, JsonRequestBehavior.AllowGet);

            //return new HttpStatusCodeResult(HttpStatusCode.OK);  // OK = 200
        }

        [System.Web.Mvc.Route("userNameCheck/{userName}")]
        [System.Web.Mvc.HttpGet]
        [System.Web.Mvc.AllowAnonymous]
        public ActionResult UserNameCheck(string userName)
        {
            userName = userName.Trim();
            var user = UserManager.FindByName(userName);
            if (user != null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Conflict);
               // return new JsonCamelCaseResult(true, JsonRequestBehavior.AllowGet);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        // POST: /Manage/DisableTFA
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.Route("TFA/{status}")]
        public async Task<ActionResult> TFA(String status)
        {
           await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), Convert.ToBoolean(status));
           return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.Route("RememberBrowser/{status}")]
        public async Task<ActionResult> RememberBrowser(String status)
        {
            
            if (!String.IsNullOrEmpty(status) && status.ToUpper()=="TRUE")
            {
                var rememberBrowserIdentity =
                AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(User.Identity.GetUserId());
                AuthenticationManager.SignIn(new AuthenticationProperties {IsPersistent = true}, rememberBrowserIdentity);
            }
            else
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            }
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        public async Task<ActionResult> ManageLogins()
        {
          
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound);
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();


            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            var model = new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            };

            return new JsonCamelCaseResult(model, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpPost]
        //[ValidateAntiForgeryToken]
        [AngularAntiForgeryToken]
        [System.Web.Mvc.Route("setPwd")]
        public async Task<ActionResult> SetPassword([FromBody] SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}