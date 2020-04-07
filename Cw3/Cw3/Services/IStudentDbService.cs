using System.Threading.Tasks;
using Cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Services
{
    public interface IStudentDbService
    {
        Task<IActionResult> EnrollStudent(EnrollStudentRequest request);
        Task<IActionResult> PromoteStudents(PromoteStudentsRequest request);
    }
}