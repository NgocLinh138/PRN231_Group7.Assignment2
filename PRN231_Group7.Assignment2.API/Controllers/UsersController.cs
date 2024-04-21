using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.API.CustomActionFilter;
using PRN231_Group7.Assignment2.Repo.Model;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;
using System.Linq.Expressions;
using AddUserRequest = PRN231_Group7.Assignment2.Contract.Service.User.Request.AddUser;
using UpdateUserRequest = PRN231_Group7.Assignment2.Contract.Service.User.Request.UpdateUser;
using UserResponse = PRN231_Group7.Assignment2.Contract.Service.User.Response;


namespace PRN231_Group7.Assignment2.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public static Expression<Func<User, object>> GetOrderBy(string orderBy)
            => orderBy?.ToLower() switch
            {
                "emailaddress" => x => x.EmailAddress,
                "firstname" => x => x.FirstName,
                _ => x => x.FirstName
            };


        [HttpGet]
        public IActionResult Get(
            string? searchValue = null,
            string? orderBy = null,
            bool? orderByAsc = true,
            int? pageIndex = 1,
            int? pageSize = 10)
        {
            Expression<Func<User, bool>> filter = p
                => (searchValue == null) || p.EmailAddress.Contains(searchValue) || p.FirstName.Contains(searchValue);

            var keySelector = GetOrderBy(orderBy);
            var result = unitOfWork.UserRepository.Get(
                filter: filter,
                orderBy: keySelector,
                orderByAsc: orderByAsc,
                pageIndex: pageIndex,
                pageSize: pageSize,
                includeProperties: "Publisher,Role");

            return Ok(mapper.Map<IEnumerable<UserResponse>>(result));
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var user = unitOfWork.UserRepository.GetById(id);
            if (user == null) return NotFound("No user found.");

            return Ok(mapper.Map<UserResponse>(user));
        }


        [HttpPost]
        [ValidateModel]
        public IActionResult Create([FromBody] AddUserRequest request)
        {
            var user = new User
            {
                EmailAddress = request.EmailAddress,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Password = request.Password,
                HireDate = request.HireDate,
                PubId = request.PubId,
                RoleId = request.RoleId
            };

            unitOfWork.UserRepository.Insert(user);
            unitOfWork.Save();

            var response = mapper.Map<UserResponse>(user);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, response);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateUserRequest request)
        {
            var isExistingUser = unitOfWork.UserRepository.IsExistUser(id);
            if (!isExistingUser) return NotFound("No user found.");

            var existingUser = unitOfWork.UserRepository.Get(filter: u => u.Id == id, includeProperties: "Publisher,Role").FirstOrDefault();

            if (existingUser == null) return NotFound("No user found.");

            existingUser.EmailAddress = request.EmailAddress;
            existingUser.FirstName = request.FirstName;
            existingUser.LastName = request.LastName;
            existingUser.Phone = request.Phone;
            existingUser.Password = request.Password;
            existingUser.Source = request.Source;
            existingUser.HireDate = request.HireDate;
            //existingUser.PubId = request.PubId;
            //existingUser.RoleId = request.RoleId;

            unitOfWork.UserRepository.Update(existingUser);
            unitOfWork.Save();

            return Ok(mapper.Map<UserResponse>(existingUser));
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var existingUser = unitOfWork.UserRepository.GetById(id);
            if (existingUser == null) return NotFound("No user found.");

            unitOfWork.UserRepository.Delete(existingUser);
            unitOfWork.Save();

            return Accepted();
        }
    }
}
