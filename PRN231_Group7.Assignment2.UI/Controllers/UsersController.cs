using Microsoft.AspNetCore.Mvc;
using UpdateUser = PRN231_Group7.Assignment2.UI.Models.User.UserRequestModel.UpdateUser;
using System.Text;
using System.Text.Json;
using PRN231_Group7.Assignment2.UI.Models.User;
using RoleResponse = PRN231_Group7.Assignment2.Contract.Service.Role.Response;

namespace PRN231_Group7.Assignment2.UI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public UsersController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? searchValue)
        {
            List<UserModel> response = new List<UserModel>();

            try
            {
                var client = httpClientFactory.CreateClient();
                var url = "http://localhost:5010/api/users";

                if (!string.IsNullOrEmpty(searchValue))
                {
                    url += $"?searchValue={searchValue}";
                }

                var httpResponseMsg = await client.GetAsync(url);
                httpResponseMsg.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMsg.Content.ReadFromJsonAsync<IEnumerable<UserModel>>());
            }
            catch (Exception ex)
            {
            }
            return View(response);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var roles = await GetRoles();
                ViewBag.Roles = roles ?? new List<RoleResponse>();
                return View(new UserRequestModel());
            }
            catch (Exception ex)
            {
                ViewBag.Roles = new List<RoleResponse>();
                return View(new UserRequestModel());
            }
        }


        [HttpPost]
        public async Task<IActionResult> Create(UpdateUser request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var client = httpClientFactory.CreateClient();
                var url = "http://localhost:5010/api/users";

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Users");
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


        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var client = httpClientFactory.CreateClient();
            var url = $"http://localhost:5010/api/users/{id}";

            var roles = await GetRoles();
            ViewBag.Roles = roles ?? new List<RoleResponse>();

            var response = await client.GetFromJsonAsync<UpdateUser>(url);
            if (response is not null) return View(response);
            return View(null);
        }



        [HttpPost]
        public async Task<IActionResult> Update(UpdateUser request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/users/{request.Id}";

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpRequestMessage.Content.ReadFromJsonAsync<UserModel>();
                if (response is not null)
                {
                    return RedirectToAction("Index", "Users");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }

            return View();
        }



        private async Task<List<RoleResponse>> GetRoles()
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync("http://localhost:5010/api/roles");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var roles = JsonSerializer.Deserialize<List<RoleResponse>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return roles ?? new List<RoleResponse>();
            }
            catch (Exception ex)
            {
                return new List<RoleResponse>();
            }
        }
    }
}
