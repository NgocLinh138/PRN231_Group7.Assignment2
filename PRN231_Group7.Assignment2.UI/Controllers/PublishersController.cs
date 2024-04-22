using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.UI.Models.Publisher;
using System.Text;
using System.Text.Json;
using CreatePublisher = PRN231_Group7.Assignment2.UI.Models.Publisher.PublisherRequestModel.CreatePublisher;
using UpdatePublisher = PRN231_Group7.Assignment2.UI.Models.Publisher.PublisherRequestModel.UpdatePublisher;

namespace PRN231_Group7.Assignment2.UI.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        public PublishersController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string? searchValue)
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");

            List<PublisherModel> response = new List<PublisherModel>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var url = "http://localhost:5010/api/publishers";

                if (!string.IsNullOrEmpty(searchValue))
                {
                    url += $"?searchValue={searchValue}";
                }

                var httpResponseMessage = await client.GetAsync(url);
                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<PublisherModel>>());
            }
            catch (Exception ex)
            {
            }
            return View(response);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePublisher request)
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var client = httpClientFactory.CreateClient();
                var url = "http://localhost:5010/api/publishers";

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Publishers");
                }
                else
                {
                    var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    return BadRequest("Error:" + responseContent);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error:" + ex.Message);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");

            var client = httpClientFactory.CreateClient();
            var url = $"http://localhost:5010/api/publishers/{id}";

            var response = await client.GetFromJsonAsync<UpdatePublisher>(url);
            if (response is not null) return View(response);

            return View(null);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdatePublisher request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/publishers/{request.Id}";

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                var response = await httpResponseMessage.Content.ReadAsStringAsync();
                if (response is not null)
                {
                    return RedirectToAction("Index", "Publishers");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Delete(PublisherModel request)
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");

            try
            {
                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/publishers/{request.Id}";

                var httpResponseMessage = await client.DeleteAsync(url);
                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Publishers");
            }
            catch (Exception ex)
            {
            }
            return View("Index");
        }



    }
}
