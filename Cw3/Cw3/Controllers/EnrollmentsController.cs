using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cw3.DTOs.Requests;
using Cw3.DTOs.Responses;
using Cw3.Handlers;
using Cw3.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private readonly EfStudentDbService _efDbService;

        public IConfiguration Configuration { get; set; }

        public EnrollmentsController(IDbService dbService, EfStudentDbService efDbService, IConfiguration configuration)
        {
            Configuration = configuration;
            _dbService = dbService;
            _efDbService = efDbService;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> EnrollStudent(EnrollStudentRequest request)
        {
            await _dbService.EnrollStudent(request);
            var response = new EnrollStudentResponse()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = request.BirthDate,
                IndexNumber = request.IndexNumber,
                Semester = 1,
                StudiesName = request.StudiesName
            };

            return CreatedAtAction("EnrollStudent", response);
        }

        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public async Task<IActionResult> PromoteStudents(PromoteStudentsRequest request)
        {
            await _dbService.PromoteStudents(request);
            var response = new PromoteStudentsResponse()
            {
                Semester = Convert.ToInt32(request.Semester) + 1,
                StudiesName = request.StudiesName
            };

            return Ok(response);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var authHandler = new BearerAuthHandler(_dbService);
            if (await authHandler.HandleAuthenticateAsync(loginRequest) != Accepted())
                return BadRequest("Bledne dane logowania");
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, loginRequest.IndexNumber),
                new Claim(ClaimTypes.Name, "PJATK"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "Me",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid()
            });
        }
    }
}