using Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using BlogManagement.Helper;
using Entities.Models;
using Entities;

namespace BlogManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService userService;
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;

        public UserController(ILogger<UserController> logger, UserService userService, CookieUserDetailsHandler cookieUserDetailsHandler)
        {
            _logger = logger;
            this.userService = userService;
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(CreateNewUserRequest model)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                var (newUser, operation_message) = await userService.Add(model);
                message = operation_message;

                if(newUser is not null)
                {
                    ViewBag.message = message;
                    return View("Login");
                }
            }
            ViewBag.message = message;
            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginRequest dto)
        {
            var (user, message) = await userService.GetUser(dto);
            
            if(user is not null)
            {
                var Claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName + "" + user.LastName),
                    new Claim(ClaimTypes.Email , user.EmailId),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                    new Claim(ClaimTypes.UserData, user.Id.ToString())
                };

                var Identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(Identity));
                    
                return RedirectToAction("Explore", "Blog");
            }
            ViewBag.message = message;
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Home()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);

            if(user is null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public async Task<ActionResult> UserList(GetAllUsersRequest dto)
        {        
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);

            if(user is null)
            {
                return RedirectToAction("Login");
            }

            if(user.Role.Name != Constants.Keys.ADMIN)
            {
                return RedirectToAction("Explore", "Blog");
            }

            var (users, totalItems) = await userService.GetAllUsers(dto);

            var modelForAdmin = new PagedList<User>
            {
                Items = users,
                PageNumber = dto.PageNo,
                PageSize = dto.PageSize,
                TotalItems = totalItems,
                Requestor = user
            };
           
            return View(modelForAdmin);
        }

        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
            
                var user = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
                if(user is null)
                {
                    return RedirectToAction("Registration", "User");
                }

                return View(user);
            }
            catch
            {
                return RedirectToAction("Error", "Blog");
            }
        }
    }
}