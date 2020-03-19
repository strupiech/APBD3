using System;
using Cw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        
        [HttpGet]
        public IActionResult GetStudent(string orderBy)
        {
            return Ok(_dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
            {
                return Ok("Strupiechowski");
            }else if (id == 2)
            {
                return Ok("Kowalski");
            }

            return NotFound("Nie znaleziono studenta");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]

        public IActionResult ModifyStudent(Student student, int id)
        {
            return Ok($"Aktualizacja zakonczona id: {id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(Student student, int id)
        {
            return Ok($"Usuwanie ukonczone id: {id}");
        }
    }
}