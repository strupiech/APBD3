using System.Threading.Tasks;
using Cw3.DTOs.Requests;
using Cw3.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public IConfiguration Configuration { get; set; }

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            return Ok(await _dbService.GetStudents());
        }

        [HttpPost("add")]
        public async Task<IActionResult> ModifyStudent(ModifyStudentRequest request)
        {
            return Ok(await _dbService.ModifyStudent(request));
        }
        
        [HttpPost("remove")]
        public async Task<IActionResult> RemoveStudent(RemoveStudentRequest request)
        {
            return Ok(await _dbService.RemoveStudent(request));
        }
    }
}