using Cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.DAL
{
    public interface IStudentDbService
    {
        void EnrollStudent(EnrollStudentRequest request);
        void PromoteStudents(int semester, string studies);
    }
}