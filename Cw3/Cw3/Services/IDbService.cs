using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Services
{
    public interface IDbService
    {
        Task<IActionResult> EnrollStudent(EnrollStudentRequest request);
        Task<IActionResult> PromoteStudents(PromoteStudentsRequest request);
        bool CheckIndexNumber(string index);
    }
}