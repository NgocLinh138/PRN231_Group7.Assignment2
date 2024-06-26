﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.API.CustomActionFilter;
using PRN231_Group7.Assignment2.Repo.Model;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;
using System.Linq.Expressions;
using AddBookRequest = PRN231_Group7.Assignment2.Contract.Service.Book.Request.AddBook;
using BookResponse = PRN231_Group7.Assignment2.Contract.Service.Book.Response;
using UpdateBookRequest = PRN231_Group7.Assignment2.Contract.Service.Book.Request.UpdateBook;

namespace PRN231_Group7.Assignment2.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public BooksController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public static Expression<Func<Book, object>> GetOrderBy(string orderBy)
           => orderBy?.ToLower() switch
           {
               "price" => x => x.Price,
               _ => x => x.Price
           };


        [HttpGet]
        public IActionResult Get(
            string? searchByTitle = null,
            string? searchByPublisher = null,
            string? orderByPrice = null,
            double? minPrice = null,
            double? maxPrice = null,
            bool? orderByAsc = true,
            int? pageIndex = 1,
            int? pageSize = 10)
        {
            Expression<Func<Book, bool>> filter = p =>
                (searchByTitle == null || p.Title.Contains(searchByTitle))
                && (searchByPublisher == null || p.Publisher.PublisherName.Contains(searchByPublisher))
                && (minPrice == null || p.Price >= minPrice)
                && (maxPrice == null || p.Price <= maxPrice);

            var keySelector = GetOrderBy(orderByPrice);

            var result = unitOfWork.BookRepository.Get(
                filter: filter,
                orderBy: keySelector,
                orderByAsc: orderByAsc,
                pageIndex: pageIndex,
                pageSize: pageSize,
                includeProperties: "Publisher");

            return Ok(mapper.Map<IEnumerable<BookResponse>>(result));
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var book = unitOfWork.BookRepository.GetById(id);
            if (book == null)
            {
                return NotFound("No book found");
            }

            return Ok(mapper.Map<BookResponse>(book));
        }


        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddBookRequest request)
        {
            var isExistingPublisher = unitOfWork.PublisherRepository.IsExistPublisher(request.PublisherId);

            if (!isExistingPublisher) return NotFound("No publisher found");

            var book = new Book
            {
                Title = request.Title,
                Type = request.Type,
                Price = request.Price,
                PublishedDate = DateTime.Now,
                PublisherId = request.PublisherId,
                Advance = request.Advance,
                Notes = request.Notes,
                Royalty = request.Royalty,
                YtdSales = request.YtdSales,
            };

            //if (request.BookImage.Length > 0)
            //{
            //    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", request.BookImage.FileName);
            //    using(var stream = System.IO.File.Create(path))
            //    {
            //        await request.BookImage.CopyToAsync(stream);
            //    }
            //    book.BookImage = "/images/" + request.BookImage.FileName;
            //} 
            //else
            //{
            //    book.BookImage = "";
            //}

            unitOfWork.BookRepository.Insert(book);
            unitOfWork.Save();

            var response = mapper.Map<BookResponse>(book);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateBookRequest request)
        {
            var isExistingBook = unitOfWork.BookRepository.IsExistBook(id);
            if (!isExistingBook) return NotFound("No book found");

            //var isExistingPublisher = unitOfWork.PublisherRepository.IsExistPublisher(request.PublisherId);
            //if (!isExistingPublisher) return NotFound("No publisher found");

            var existingBook = unitOfWork.BookRepository.GetById(id);

            existingBook.Title = request.Title;
            existingBook.Type = request.Type;
            existingBook.Price = request.Price;
            existingBook.PublishedDate = request.PublishedDate;
            existingBook.PublisherId = request.PublisherId;
            existingBook.Advance = request.Advance;
            existingBook.Notes = request.Notes;
            existingBook.Royalty = request.Royalty;
            existingBook.YtdSales = request.YtdSales;


            unitOfWork.BookRepository.Update(existingBook);
            unitOfWork.Save();

            return Ok(mapper.Map<BookResponse>(existingBook));
        }



        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var isExistingBook = unitOfWork.BookRepository.IsExistBook(id);
            if (!isExistingBook) return NotFound("No book found");

            unitOfWork.BookRepository.Delete(id);
            unitOfWork.Save();

            return Accepted();
        }



    }
}
