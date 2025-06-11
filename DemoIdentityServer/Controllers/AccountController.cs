using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DemoIdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly TestUserStore _users;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        public AccountController(TestUserStore users, IIdentityServerInteractionService interaction, IEventService events)
        {
            _users = users;
            _interaction = interaction;
            _events = events;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, string returnUrl)
        {
            if (_users.ValidateCredentials(username, password))
            {
                var user = _users.FindByUsername(username);
                var isUser = new IdentityServerUser(user.SubjectId)
                {
                    DisplayName = user.Username,
                    AdditionalClaims = user.Claims
                };
                await HttpContext.SignInAsync(isUser);

                await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username));

                if (_interaction.IsValidReturnUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return Redirect("~/");
                }
            }

            await _events.RaiseAsync(new UserLoginFailureEvent(username, "invalid credentials"));
            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu!");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await HttpContext.SignOutAsync();
            var logoutRequest = await _interaction.GetLogoutContextAsync(logoutId);
            return Redirect(logoutRequest?.PostLogoutRedirectUri ?? "/");
        }
    }
}
