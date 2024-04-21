using Microsoft.AspNetCore.Mvc;
using PRN231_Group7.Assignment2.Contract.Service.User;
using PRN231_Group7.Assignment2.Repo.Model;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;
using PRN231_Group7.Assignment2.Repo.Services.Interface;
using System.Security.Claims;

namespace PRN231_Group7.Assignment2.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IJWTTokenService jWTTokenService;
        private readonly IUnitOfWork unitOfWork;

        public AuthController(
            IJWTTokenService jWTTokenService,
            IUnitOfWork unitOfWork)
        {
            this.jWTTokenService = jWTTokenService;
            this.unitOfWork = unitOfWork;
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] Request.RegisterUser request)
        {
            var existUser = unitOfWork.UserRepository.FindByEmailAsync(request.EmailAddress);
            if (existUser == null)
                return BadRequest("Email already register!");
            var role = unitOfWork.RoleRepository.GetCustomerRole();
            var user = new User
            {
                EmailAddress = request.EmailAddress,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Phone = request.Phone,
                Password = request.Password,
                HireDate = request.HireDate,
                PubId = request.PublisherId,
                Source = request.Source,
                RoleId = role.Id
            };

            unitOfWork.UserRepository.Insert(user);
            unitOfWork.Save();
            return Ok();

        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] Request.Login request)
        {
            var user = await unitOfWork.UserRepository.FindByEmailAsync(request.EmailAddress);
            if (user != null && user.Password == request.Password)
            {

                var role = unitOfWork.RoleRepository.GetById(user.RoleId);
                //Generate claims
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.EmailAddress),
                        new Claim(ClaimTypes.Role, role.RoleName),
                    };

                // GetToken
                var token = jWTTokenService.GenerateAccessToken(claims);
                return Ok(token);
            }

            return BadRequest("Invalid email or password.");
        }

    }
}




