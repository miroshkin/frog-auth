using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Treehouse.FitnessFrog.Shared.Models;
using Treehouse.FitnessFrog.Shared.Security;
using Treehouse.FitnessFrog.ViewModels;

namespace Treehouse.FitnessFrog.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(AccountRegisterViewModel viewModel)
        {
            // If the ModelState is valid...
            if (ModelState.IsValid)
            {
                // Instantiate a User object
                var user = new User { UserName = viewModel.Email, Email = viewModel.Email };

                // Create the user
                var result = await _userManager.CreateAsync(user, viewModel.Password);

                // If the user was successfully created...
                if (result.Succeeded)
                {
                    // Sign-in the user and redirect them to the web app's "Home" page
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index", "Entries");

                }

                // If there were errors...
                // Add model errors

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }



            return View(viewModel);
        }
    }
}