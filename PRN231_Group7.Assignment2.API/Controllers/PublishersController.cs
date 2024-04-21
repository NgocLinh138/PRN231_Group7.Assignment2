using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.API.CustomActionFilter;
using PRN231_Group7.Assignment2.Repo.Model;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;
using System.Linq.Expressions;
using AddPublisherRequest = PRN231_Group7.Assignment2.Contract.Service.Publisher.Request.AddPublisher;
using PublisherResponse = PRN231_Group7.Assignment2.Contract.Service.Publisher.Response;
using UpdatePublisherRequest = PRN231_Group7.Assignment2.Contract.Service.Publisher.Request.UpdatePublisher;

namespace PRN231_Group7.Assignment2.API.Controllers
{
    [Route("api/publishers")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PublishersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public static Expression<Func<Publisher, object>> GetOrderBy(string orderBy)
            => orderBy?.ToLower() switch
            {
                "publisherName" => x => x.PublisherName,
                _ => x => x.PublisherName
            };


        [HttpGet]
        public IActionResult Get(
            string? searchValue = null,
            string? orderBy = "",
            bool? orderByAsc = false,
            int? pageIndex = 1,
            int? pageSize = 10
            )
        {
            Expression<Func<Publisher, bool>> filter = p => (searchValue == null || p.PublisherName.Contains(searchValue));

            var keySelector = GetOrderBy(orderBy);

            var result = unitOfWork.PublisherRepository.Get(
                filter: filter,
                orderBy: keySelector,
                orderByAsc: orderByAsc,
                pageIndex: pageIndex,
                pageSize: pageSize,
                includeProperties: "");

            return Ok(mapper.Map<IEnumerable<PublisherResponse>>(result));
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var publisher = unitOfWork.PublisherRepository.GetById(id);
            if (publisher == null) return NotFound("No publisher found.");

            return Ok(mapper.Map<PublisherResponse>(publisher));
        }


        [HttpPost]
        [ValidateModel]
        public IActionResult Create([FromBody] AddPublisherRequest request)
        {
            var publisher = new Publisher
            {
                PublisherName = request.PublisherName,
                City = request.City,
                State = request.State,
                Country = request.Country,
            };

            unitOfWork.PublisherRepository.Insert(publisher);
            unitOfWork.Save();

            var response = mapper.Map<PublisherResponse>(publisher);

            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdatePublisherRequest request)
        {
            var isExistingPublisher = unitOfWork.PublisherRepository.IsExistPublisher(id);
            if (!isExistingPublisher) return NotFound("No publisher found.");

            var existingPublisher = unitOfWork.PublisherRepository.GetById(id);
            existingPublisher.PublisherName = request.PublisherName;
            existingPublisher.City = request.City;
            existingPublisher.Country = request.Country;
            existingPublisher.State = request.State;

            unitOfWork.PublisherRepository.Update(existingPublisher);
            unitOfWork.Save();

            return Ok(mapper.Map<PublisherResponse>(existingPublisher));
        }



        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var publisher = unitOfWork.PublisherRepository.GetById(id);
            if (publisher == null) return NotFound("No publisher found.");

            unitOfWork.PublisherRepository.Delete(id);
            unitOfWork.Save();

            return Accepted();
        }

    }
}
