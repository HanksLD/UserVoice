using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using UserVoice.Service;
using System.Text;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserVoice.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private IAppUserService service;

        public AccountController(IAppUserService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            password = MD5Encrypted(password);
            var serviceResponse = service.Login(email, password);
            if (!serviceResponse.IsSuccess)
            {
                return Content("<script>alert('" + serviceResponse.ErrorMessage + "')</script>", "text/javascript", Encoding.UTF8);
            }
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, serviceResponse.Result.Email),
                    new Claim("FullName", serviceResponse.Result.FullName),
                    new Claim(ClaimTypes.Role, "Administrator"),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                 new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect(Request.Query["ReturnUrl"].ToString() ?? "/");
        }

        [Models.UserAuthorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/Account/Login");
        }

        private string MD5Encrypted(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;
            string pwd = "";
            System.Security.Cryptography.MD5 mD5 = System.Security.Cryptography.MD5.Create();
            byte[] result = mD5.ComputeHash(Encoding.UTF8.GetBytes(input));
            for (int i = 0; i < result.Length; i++)
            {
                pwd += result[i].ToString("X");
            }
            return pwd;
        }
    }
}
