﻿using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.UI.Models.Author;
using PRN231_Group7.Assignment2.UI.Models.Book;
using System.Text;
using System.Text.Json;
using CreateAuthor = PRN231_Group7.Assignment2.UI.Models.Author.AuthorRequestModel.CreateAuthor;
using UpdateAuthor = PRN231_Group7.Assignment2.UI.Models.Author.AuthorRequestModel.UpdateAuthor;


namespace PRN231_Group7.Assignment2.UI.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        public int totalPages;
        public int pageSize = 4;
        public AuthorsController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchValue,
            int pageIndex = 1)
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");

            List<AuthorModel> response = new List<AuthorModel>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/authors?orderByAsc=true&pageIndex={pageIndex}&pageSize={this.pageSize}";

                if (!string.IsNullOrEmpty(searchValue))
                {
                    url += $"searchValue={searchValue}";
                }

                var httpResponseMessage = await client.GetAsync(url);
                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<AuthorModel>>());

                //Paging
                var totalItem = await NumberOfItems(searchValue);

                this.totalPages = totalItem / pageSize;
                if (totalItem % pageSize != 0)
                {
                    totalPages++;
                }

                ViewBag.TotalPages = this.totalPages;
                ViewBag.PageIndex = pageIndex;
            }
            catch (Exception ex)
            {
            }
            return View(response);
        }

        private async Task<int> NumberOfItems(string searchValue)
        {
            List<BookModel> response = new List<BookModel>();
            var client = httpClientFactory.CreateClient();
            var url = $"http://localhost:5010/api/authors?orderByAsc=true&pageIndex=1&pageSize=1000";
            if (!string.IsNullOrEmpty(searchValue))
                url += $"&searchValue={searchValue}";

            var httpResponseMsg = await client.GetAsync(url);
            httpResponseMsg.EnsureSuccessStatusCode();
            response.AddRange(await httpResponseMsg.Content.ReadFromJsonAsync<IEnumerable<BookModel>>());
            return response.Count();
        }


        [HttpGet]
        public IActionResult Create()
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthor request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var client = httpClientFactory.CreateClient();
                var url = "http://localhost:5010/api/authors";

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Authors");
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
            var url = $"http://localhost:5010/api/authors/{id}";

            var response = await client.GetFromJsonAsync<UpdateAuthor>(url);
            if (response is not null) return View(response);

            return View(null);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateAuthor request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(request);
                }

                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/authors/{request.Id}";

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
                    return RedirectToAction("Index", "Authors");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Delete(AuthorModel request)
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");

            try
            {
                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/authors/{request.Id}";

                var httpResponseMessage = await client.DeleteAsync(url);
                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Authors");
            }
            catch (Exception ex)
            {
            }
            return View("Index");
        }




    }
}
