using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Shop.Auth;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shop.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
      
        public AccountController(SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
           
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
            {
            if (!ModelState.IsValid)
                return View(loginViewModel);
            var user = await
               _userManager.FindByNameAsync(loginViewModel.UserName);
            if (user != null)
                {
                var result = await
                    _signInManager.PasswordSignInAsync
                    (user, loginViewModel.Password,loginViewModel.Rememberme, false);


                if (result.Succeeded)
                    {

                    return RedirectToAction("Index", "Home");
                    }

                }

            ModelState.AddModelError("", "User Name/Password Not found");
            return View(loginViewModel);
            }


        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }

       
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = loginViewModel.UserName
                };

                var result = await _userManager.CreateAsync(user, loginViewModel.Password);

                if (result.Succeeded)
                {
                 

                    return RedirectToAction("Index", "Home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
        }

            return View(loginViewModel);

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        }
}
