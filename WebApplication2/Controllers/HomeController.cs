using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Http.Headers;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> CallApi([FromServices] IHttpClientFactory httpClientFactory)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            using var client = httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://localhost:44324/WeatherForecast/secure");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["ApiResult"] = "API call failed: " + response.StatusCode;
                return View();
            }

            var result = await response.Content.ReadAsStringAsync();
            ViewData["ApiResult"] = result;

            return View();
        }
    }
}
