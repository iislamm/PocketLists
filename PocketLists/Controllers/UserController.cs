using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using PocketLists.Data;
using PocketLists.Models;

namespace PocketLists.Controllers
{
    public class UserController : Controller
    {
        private readonly UsersContext _context;

        public UserController(UsersContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        public IActionResult Signin(string error = null)
        {
            SigninViewModel signinvm = new SigninViewModel
            {
                ErrorMessage = error
            };
            return View(signinvm);
        }

        public async System.Threading.Tasks.Task Login(User user)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                        new Claim("ID", user.ID.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("DisplayName", user.DisplayName)
                    };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "ID", null);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(principal);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("DisplayName,Email,Password")] SignupViewModel args)
        {
            args.PasswordHash = BCrypt.Net.BCrypt.HashPassword(args.Password);
            User user = new User
            {
                DisplayName = args.DisplayName,
                Email = args.Email,
                Password = args.PasswordHash
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            user = _context.Users.Single(u => u.Email.Equals(user.Email));

            await this.Login(user);

            return RedirectToAction("Index", "");
        }

        [HttpPost]
        public async Task<IActionResult> Signin([Bind("Email,Password")] SigninViewModel args)
        {
            try
            {
                User user = _context.Users.Where(u => u.Email == args.Email).Single();
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(args.Password, user.Password);
                if (isValidPassword)
                {
                     await this.Login(user);
                }
                else
                {
                    InvalidOperationException ex = new InvalidOperationException();
                    ex.Data["field"] = "password";
                    throw ex;
                }
            }
            catch(InvalidOperationException e)
            {
                if (e.Data["field"] != null && e.Data["field"].ToString() == "password")
                {
                    return RedirectToAction("Signin", new RouteValueDictionary(new { error = "Invalid Password" }));
                }
                return RedirectToAction("Signin", new RouteValueDictionary(new { error = "Email Not found" }));
            }
            return RedirectToAction("Index", "");
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", ""); 
        }
    }
}