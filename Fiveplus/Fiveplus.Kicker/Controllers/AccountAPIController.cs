using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Fiveplus.Data.DbContexts;
using Fiveplus.Data.Models;
using Fiveplus.Kicker.Helpers;
using Fiveplus.Kicker.Models;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;

namespace Fiveplus.Kicker.Controllers
{
    
    [RoutePrefix("accountapi")]
    public class AccountApiController:Controller
    {

        #region  Constructor

        public AccountApiController()
        {
        }

        public AccountApiController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        //
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
            ViewBag.Errors = true;
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
          [Route("testregister")]
        public async Task<ActionResult> TestRegister()
        {
            IEnumerable<string> result = new List<string>() { "X", "Y", "Z" };
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, result.ConvertToJson("errors"));
       
        }


        #region Helper Methods


        //Added by Vasanth

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

    }
}