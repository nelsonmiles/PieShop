using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Shop.Auth;


namespace Shop.Controllers
    {
    [Authorize(Roles = "Administrators")]
    public class AdminController : Controller
        {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
            _userManager = userManager;
            _roleManager = roleManager;
            }

        public IActionResult UserManagement()
            {
            var users = _userManager.Users;

            return View(users);
            }

        public IActionResult AddUser()
            {
            return View();
            }


        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel addUserViewModel)
            {
            if (!ModelState.IsValid)

                return View(addUserViewModel);

            var user = new ApplicationUser()
                {
                UserName = addUserViewModel.UserName,
                Email = addUserViewModel.Email,
                Birthdate = addUserViewModel.Birthdate,
                City = addUserViewModel.City,
                Country = addUserViewModel.Country
                };

            IdentityResult result = await _userManager.CreateAsync(user, addUserViewModel.Password);

            if (result.Succeeded)
                {
                return RedirectToAction("UserManagement", _userManager.Users);
                }

            foreach (IdentityError error in result.Errors)
                {
                ModelState.AddModelError("", error.Description);
                }
            return View(addUserViewModel);
            }

        public async Task<IActionResult> EditUser(string id)
            {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return RedirectToAction("UserManagement", _userManager.Users);

            var claims = await _userManager.GetClaimsAsync(user);
            var vm = new EditUserViewModel() { Id = user.Id, Email = user.Email, UserName = user.UserName, UserClaims = claims.Select(c => c.Value).ToList() };

            return View(vm);
            }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserViewModel)
            {
            var user = await _userManager.FindByIdAsync(editUserViewModel.Id);

            if (user != null)
                {
                user.Email = editUserViewModel.Email;
                user.UserName = editUserViewModel.UserName;
                user.Birthdate = editUserViewModel.Birthdate;
                user.City = editUserViewModel.City;
                user.Country = editUserViewModel.Country;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("UserManagement", _userManager.Users);

                ModelState.AddModelError("", "User not updated, something went wrong.");

                return View(editUserViewModel);
                }

            return RedirectToAction("UserManagement", _userManager.Users);
            }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
            {
            var user = await _userManager.FindByIdAsync(Id);

            if (user != null)
                {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("UserManagement");
                else
                    ModelState.AddModelError("", "Something went wrong while deleting this user.");
                }
            else
                {
                ModelState.AddModelError("", "This user can't be found");
                }
            return View("UserManagement", _userManager.Users);
            }

        public IActionResult RoleManagement()
            {
            var roles = _roleManager.Roles;
            return View(roles);
            }


        [HttpGet]
        public IActionResult AddNewRole()
            {

            return View();

            }
        [HttpPost]
        public async Task<IActionResult> AddNewRole(AddRoleViewModel addRoleViewModel)
            {
            if (ModelState.IsValid)
                {
                IdentityRole identityRole = new IdentityRole
                    {
                    Name = addRoleViewModel.RoleName
                    };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                    {
                    return RedirectToAction("Index", "Home");
                    }

                foreach (IdentityError error in result.Errors)
                    {
                    ModelState.AddModelError("", error.Description);
                    }

                }
            return View(addRoleViewModel);

            }





        public async Task<IActionResult> EditRole(string id)
            {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return RedirectToAction("RoleManagement", _roleManager.Roles);

            var editRoleViewModel = new EditRoleViewModel
                {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
                };


            foreach (var user in _userManager.Users)
                {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    editRoleViewModel.Users.Add(user.UserName);
                }

            return View(editRoleViewModel);
            }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
            {
            var role = await _roleManager.FindByIdAsync(editRoleViewModel.Id);

            if (role != null)
                {
                role.Name = editRoleViewModel.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("RoleManagement", _roleManager.Roles);

                ModelState.AddModelError("", "Role not updated, something went wrong.");

                return View(editRoleViewModel);
                }

            return RedirectToAction("RoleManagement", _roleManager.Roles);
            }


        public async Task<IActionResult> AddUserToRole(string roleId)
            {
            
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return RedirectToAction("RoleManagement", _roleManager.Roles);

            var addUserToRoleViewModel = new UserRoleViewModel { RoleId = role.Id };

            foreach (var user in _userManager.Users)
                {
                if (!await _userManager.IsInRoleAsync(user, role.Name))
                    {
                    addUserToRoleViewModel.Users.Add(user);
                    }
                }

            return View(addUserToRoleViewModel);
            }
        [HttpPost]
        public async Task<IActionResult> AddUserToRole(UserRoleViewModel userRoleViewModel)
            {
            var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
                {
                return RedirectToAction("RoleManagement", _roleManager.Roles);
                }

            foreach (IdentityError error in result.Errors)
                {
                ModelState.AddModelError("", error.Description);
                }

            return View(userRoleViewModel);
            }

        }
    }

//Claims





