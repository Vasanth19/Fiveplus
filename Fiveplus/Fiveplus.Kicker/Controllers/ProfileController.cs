using System;
using System.Collections.Generic;
using System.Net;
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
    [Authorize]
    [RoutePrefix("manage")]
    public class ProfileController : Controller
    {
         #region  Constructor

        public ProfileController()
        {
        }

        public ProfileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        } 
        #endregion


        #region Account API
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [AngularAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doen't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                //    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [AngularAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Location = model.Location };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //Code added by Vasanth
                    CreateUserdetails(user);

                    var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
                    var resultSignIn = await SignInManager.PasswordSignInAsync(model.Email, model.Password, true, shouldLockout: false);
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
                }

                //   var errors = result.Errors;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.Errors.ConvertToJson("errors"));
            }



            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ModelState.Values.ToString());

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("testregister")]
        public async Task<ActionResult> TestRegister()
        {
            IEnumerable<string> result = new List<string>() { "X", "Y", "Z" };
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.ConvertToJson("errors"));

        }

        private void CreateUserdetails(ApplicationUser user)
        {
            IdentityContext context = HttpContext.GetOwinContext().Get<IdentityContext>();

            context.UserDetails.Add(
                new UserDetail
                {
                    Preference = NotificationPreference.Weekly,
                    ProfileImg = "/assets/img/common/NoImage.png",
                    User = user
                });
            context.SaveChanges();

        }

        #endregion

        [Route("profileconfig")]
        [HttpGet]
        [AllowAnonymous]
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

        [Route("basicprofile")]
        [HttpGet]
        [AllowAnonymous]
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
                ProfileImg = String.IsNullOrEmpty(userDetail.ProfileImg)? "~/assets/img/common/NoImage.png":userDetail.ProfileImg,
                Biography = userDetail.Biography
                
            };
            return new JsonCamelCaseResult(model, JsonRequestBehavior.AllowGet);

           // return Json(model, JsonRequestBehavior.AllowGet);

            //return new HttpStatusCodeResult(HttpStatusCode.OK);  // OK = 200
        }

        [Route("userNameCheck/{userName}")]
        [HttpGet]
        [AllowAnonymous]
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
        [HttpPost]
        [Route("TFA/{status}")]
        public async Task<ActionResult> TFA(String status)
        {
           await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), Convert.ToBoolean(status));
           return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("Test/{status}")]
        public async Task<ActionResult> Test(String status)
        {
            IEnumerable<string> result = new List<string>() { "X", "Y", "Z" };
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.ConvertToJson("errors"));
           // return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("RememberBrowser/{status}")]
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AngularAntiForgeryToken]
        [Route("setPassword")]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
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

        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AngularAntiForgeryToken]
        [Route("changePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.CurrentPassword,model.NewPassword);
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

            return JsonFormResponse();
            // If we got this far, something failed, redisplay form
         //   return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
        }


        public ActionResult JsonFormResponse(JsonRequestBehavior jsonRequestBehaviour = JsonRequestBehavior.DenyGet)
        {
            if (ModelState.IsValid)
            {
                return new HttpStatusCodeResult(200);
            }

            var errorList = new List<JsonValidationError>();
            foreach (var key in ModelState.Keys)
            {
                ModelState modelState = null;
                if (ModelState.TryGetValue(key, out modelState))
                {
                    foreach (var error in modelState.Errors)
                    {
                        errorList.Add(new JsonValidationError()
                        {
                            Key = key,
                            Message = error.ErrorMessage
                        });
                    }
                }
            }

            var response = new JsonResponse()
            {
                Type = "Validation",
                Message = "",
                Errors = errorList
            };

            Response.StatusCode = 400;
            return new JsonCamelCaseResult(response, jsonRequestBehaviour);
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