using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.UI.Models;
using System.Text;
using System.Text.Json;

namespace PRN231_Group7.Assignment2.UI.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public AuthenticationController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
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
                var url = "http://localhost:5010/api/auth/Login";

                var request = new LoginModel
                {
                    EmailAddress = email,
                    Password = password,
                };

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if(httpResponseMessage.IsSuccessStatusCode)
                {
                    var response = await httpResponseMessage.Content.ReadAsStringAsync();
                    var tokenObj = JsonSerializer.Deserialize<dynamic>(response);
                    var token = tokenObj.Token;
                    var role = tokenObj.Role;

                    if (role == "Admin")
                    {
                        return RedirectToAction("Index", "Books");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("Index");
            }
        }
    }
}
