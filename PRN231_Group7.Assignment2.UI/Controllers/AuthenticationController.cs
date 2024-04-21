using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.UI.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using PublisherResponse = PRN231_Group7.Assignment2.Contract.Service.Publisher.Response;

namespace PRN231_Group7.Assignment2.UI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthenticationController(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/auth/Login?EmailAddress={email}&Password={password}";

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                };

                httpRequestMessage.Headers.Add("Accept", "application/json");

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = await httpResponseMessage.Content.ReadAsStringAsync();
                    var token = JsonSerializer.Deserialize<string>(response);
                    httpContextAccessor.HttpContext.Response.Cookies.Append("JwtToken", token);

                    var userRole = GetRoleFromToken(token);
                    httpContextAccessor.HttpContext.Session.SetString("UserRole", userRole);

                    if (userRole == "Admin")
                    {
                        return RedirectToAction("Index", "Books");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                TempData["ErrorMessage"] = "Invalid email or password.";
                return View();

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return View();
            }
        }


        private string GetRoleFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return roleClaim?.Value;
        }



        private async Task<List<PublisherResponse>> GetPublishers()
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync("http://localhost:5010/api/publishers");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var publishers = JsonSerializer.Deserialize<List<PublisherResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return publishers ?? new List<PublisherResponse>();
            }
            catch (Exception ex)
            {
                return new List<PublisherResponse>();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            var publishers = await GetPublishers();
            ViewBag.Publishers = publishers ?? new List<PublisherResponse>();
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var publishers = await GetPublishers();
                    ViewBag.Publishers = publishers ?? new List<PublisherResponse>();
                    return View(request);
                }

                var client = httpClientFactory.CreateClient();
                var url = "http://localhost:5010/api/auth/register";

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login", "Authentication");
                }
                else
                {
                    var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    return BadRequest("Error: " + responseContent);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
