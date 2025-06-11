using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login(string? returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "oidc");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync("oidc", new AuthenticationProperties
            {
                RedirectUri = Url.Action("CallbackLogout", "Auth", null, Request.Scheme)
            });

            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult CallbackLogout()
        {
            return RedirectToAction("Login", "Auth"); ;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Tokens()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");
            return View(new TokenViewModel
            {
                AccessToken = accessToken,
                IdToken = idToken,
                RefreshToken = refreshToken
            });
        }
    }
}

public class TokenViewModel
{
    public string? AccessToken { get; set; }
    public string? IdToken { get; set; }
    public string? RefreshToken { get; set; }
}