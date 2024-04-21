using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.API.CustomActionFilter;
using PRN231_Group7.Assignment2.Repo.Model;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;
using System.Linq.Expressions;
using AddAuthorRequest = PRN231_Group7.Assignment2.Contract.Service.Author.Request.AddAuthor;
using AuthorResponse = PRN231_Group7.Assignment2.Contract.Service.Author.Response.AuthorRepsonse;
using UpdateAuthorRequest = PRN231_Group7.Assignment2.Contract.Service.Author.Request.UpdateAuthor;

namespace PRN231_Group7.Assignment2.API.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AuthorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public static Expression<Func<Author, object>> GetOrderBy(string orderBy)
        => orderBy?.ToLower() switch
        {
            "emailaddress" => x => x.EmailAddress,
            "firstname" => x => x.FirstName,
            _ => x => x.FirstName
        };


        [HttpGet]
        public IActionResult Get(
            string? searchValue = null,
            string? orderBy = "",
            bool? orderByAsc = false,
            int? pageIndex = 1,
            int? pageSize = 10)
        {
            Expression<Func<Author, bool>> filter = p
                => (searchValue == null) || p.EmailAddress.Contains(searchValue) || p.FirstName.Contains(searchValue);

            var keySelector = GetOrderBy(orderBy);

            var result = unitOfWork.AuthorRepository.Get(
                filter: filter,
                orderBy: keySelector,
                orderByAsc: orderByAsc,
                pageIndex: pageIndex,
                pageSize: pageSize,
                includeProperties: "");

            return Ok(mapper.Map<IEnumerable<AuthorResponse>>(result));
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var author = unitOfWork.AuthorRepository.GetById(id);
            if (author == null) return NotFound("No author found.");

            return Ok(mapper.Map<AuthorResponse>(author));
        }


        [HttpPost]
        [ValidateModel]
        public IActionResult Create([FromBody] AddAuthorRequest request)
        {
            var author = new Author
            {
                EmailAddress = request.EmailAddress,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
            };

            unitOfWork.AuthorRepository.Insert(author);
            unitOfWork.Save();

            var response = mapper.Map<AuthorResponse>(author);

            return CreatedAtAction(nameof(GetById), new { id = author.Id }, response);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateAuthorRequest request)
        {
            var isExistingAuthor = unitOfWork.AuthorRepository.IsAuthorExist(id);
            if (!isExistingAuthor) return NotFound("No author found.");

            var existingAuthor = unitOfWork.AuthorRepository.GetById(id);
            existingAuthor.EmailAddress = request.EmailAddress;
            existingAuthor.FirstName = request.FirstName;
            existingAuthor.LastName = request.LastName;
            existingAuthor.Phone = request.Phone;
            existingAuthor.Address = request.Address;
            existingAuthor.City = request.City;
            existingAuthor.State = request.State;
            existingAuthor.Zip = request.Zip;

            unitOfWork.AuthorRepository.Update(existingAuthor);
            unitOfWork.Save();

            return Ok(mapper.Map<AuthorResponse>(existingAuthor));
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var author = unitOfWork.AuthorRepository.GetById(id);
            if (author == null) return NotFound("No author found.");

            unitOfWork.AuthorRepository.Delete(id);
            unitOfWork.Save();

            return Accepted();
        }
    }
}
