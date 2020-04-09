using System;
using System.Threading.Tasks;
using Cw3.DTOs.Requests;
using Cw3.DTOs.Responses;
using Cw3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public EnrollmentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
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
    }
}