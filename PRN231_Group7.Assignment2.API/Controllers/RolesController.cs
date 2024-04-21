using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.API.CustomActionFilter;
using PRN231_Group7.Assignment2.Repo.Model;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;
using System.Linq.Expressions;
using AddRoleRequest = PRN231_Group7.Assignment2.Contract.Service.Role.Request.AddRole;
using RoleResponse = PRN231_Group7.Assignment2.Contract.Service.Role.Response;
using UpdateRoleRequest = PRN231_Group7.Assignment2.Contract.Service.Role.Request.UpdateRole;

namespace PRN231_Group7.Assignment2.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RolesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }


        public static Expression<Func<Role, object>> GetOrderBy(string orderBy)
            => orderBy?.ToLower() switch
            {
                "rolename" => x => x.RoleName,
                _ => x => x.RoleName
            };


        [HttpGet]
        public IActionResult Get(
            string? searchValue = null,
            string? orderBy = "")
        {
            Expression<Func<Role, bool>> filter = p
                => (searchValue == null) || p.RoleName.Contains(searchValue);

            var keySelector = GetOrderBy(orderBy);

            var result = unitOfWork.RoleRepository.Get(
                filter: filter,
                orderBy: keySelector,
                includeProperties: "");

            return Ok(mapper.Map<IEnumerable<RoleResponse>>(result));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var role = unitOfWork.RoleRepository.GetById(id);
            if (role == null) return NotFound("No role found.");

            return Ok(mapper.Map<RoleResponse>(role));
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult Create([FromBody] AddRoleRequest request)
        {
            var role = new Role
            {
                RoleName = request.RoleName,
            };

            unitOfWork.RoleRepository.Insert(role);
            unitOfWork.Save();

            var response = mapper.Map<RoleResponse>(role);

            return CreatedAtAction(nameof(GetById), new { id = role.Id }, response);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRoleRequest request)
        {
            var isExistingRole = unitOfWork.RoleRepository.IsRoleExist(id);
            if (!isExistingRole) return NotFound("No role found");

            var existingRole = unitOfWork.RoleRepository.GetById(id);
            existingRole.RoleName = request.RoleName;

            unitOfWork.RoleRepository.Update(existingRole);
            unitOfWork.Save();

            return Ok(mapper.Map<RoleResponse>(existingRole));
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var existingRole = unitOfWork.RoleRepository.GetById(id);
            if (existingRole == null) return NotFound("No role found");

            unitOfWork.RoleRepository.Delete(existingRole);
            unitOfWork.Save();

            return Accepted();
        }

    }
}
