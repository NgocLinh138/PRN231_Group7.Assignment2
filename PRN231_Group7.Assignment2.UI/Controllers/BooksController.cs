﻿using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.UI.Models.Book;
using System.Text;
using System.Text.Json;
using PublisherResponse = PRN231_Group7.Assignment2.Contract.Service.Publisher.Response;

namespace PRN231_Group7.Assignment2.UI.Controllers
{
    public class BooksController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        public int totalPages;
        public int pageSize = 4;
        public BooksController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchBy,
            string? searchValue,
            double? minPrice,
            double? maxPrice,
            string? orderBy,
            int pageIndex = 1)
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");

            List<BookModel> response = new List<BookModel>();
            try
            {
                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/books?orderByAsc=true&pageIndex={pageIndex}&pageSize={this.pageSize}";

                if(!string.IsNullOrEmpty(searchBy))
                {
                    if(searchBy == "Title")
                    {
                        url += $"&searchByTitle={searchValue}";
                    }
                    else if (searchBy == "Publisher")
                    {
                        url += $"&searchByPublisher={searchValue}";
                    }
                }


                if (minPrice != null)
                {
                    url += $"&minPrice={minPrice}";
                }
                if (maxPrice != null)
                {
                    url += $"&maxPrice={maxPrice}";
                }

              


                var httpResponseMsg = await client.GetAsync(url);
                httpResponseMsg.EnsureSuccessStatusCode();
                response.AddRange(await httpResponseMsg.Content.ReadFromJsonAsync<IEnumerable<BookModel>>());


                //Paging
                var totalItem = await NumberOfItems(searchBy, searchValue);

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



        private async Task<int> NumberOfItems(string searchValue, string publisher)
        {
            List<BookModel> response = new List<BookModel>();
            var client = httpClientFactory.CreateClient();
            var url = $"http://localhost:5010/api/books?orderByAsc=true&pageIndex=1&pageSize=1000";
            if (!string.IsNullOrEmpty(searchValue))

                url += $"&searchValue={searchValue}";

            if (!string.IsNullOrEmpty(publisher))

                url += $"&publisher={publisher}";
            var httpResponseMsg = await client.GetAsync(url);
            httpResponseMsg.EnsureSuccessStatusCode();
            response.AddRange(await httpResponseMsg.Content.ReadFromJsonAsync<IEnumerable<BookModel>>());
            return response.Count();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");


            try
            {
                var publishers = await GetPublishers();
                ViewBag.Publishers = publishers ?? new List<PublisherResponse>();
                return View(new BookRequestModel());
            }
            catch (Exception ex)
            {
                ViewBag.Publishers = new List<PublisherResponse>();
                return View(new BookRequestModel());
            }
        }



        [HttpPost]
        public async Task<IActionResult> Create(BookRequestModel request)
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
                var url = "http://localhost:5010/api/books";

                var httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
                };

                var httpResponseMessage = await client.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Books");
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
            var publishers = await GetPublishers();
            ViewBag.Publishers = publishers ?? new List<PublisherResponse>();

            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");

            var client = httpClientFactory.CreateClient();
            var url = $"http://localhost:5010/api/books/{id}";

            var response = await client.GetFromJsonAsync<UpdateBookRequestModel>(url);
            if (response is not null) return View(response);
            return View(null);
        }



        [HttpPost]
        public async Task<IActionResult> Update(UpdateBookRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                var publishers = await GetPublishers();
                ViewBag.Publishers = publishers ?? new List<PublisherResponse>();
                return View(request);
            }

            var client = httpClientFactory.CreateClient();
            var url = $"http://localhost:5010/api/books/{request.Id}";

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(url),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<BookModel>();
            if (response is not null)
            {
                return RedirectToAction("Index", "Books");
            }

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Delete(BookModel book)
        {

            var roleName = httpContextAccessor.HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(roleName))
                return RedirectToAction("Index", "Books");

            try
            {
                var client = httpClientFactory.CreateClient();
                var url = $"http://localhost:5010/api/books/{book.Id}";

                var httpResponseMessage = await client.DeleteAsync(url);
                httpResponseMessage.EnsureSuccessStatusCode();
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
            }
            return View("Index");
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

    }
}
