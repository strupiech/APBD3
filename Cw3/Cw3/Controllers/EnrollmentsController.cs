using Cw3.DTOs.Requests;
using Cw3.DTOs.Responses;
using Cw3.Models;
using Cw3.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentDbService _dbService;

        public EnrollmentsController(IStudentDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest studentRequest)
        {
            _dbService.EnrollStudent(studentRequest);
            var response = new EnrollStudentResponse();

            return CreatedAtAction("EnrollStudent", response);
        }
        
        [HttpPost("promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest studentRequest)
        {
            _dbService.PromoteStudents(studentRequest);
            var response = new PromoteStudentsResponse();

            return Ok(response);
        }
    }
}