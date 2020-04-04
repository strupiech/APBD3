using Cw3.DTOs.Requests;

namespace Cw3.Services
{
    public interface IStudentDbService
    {
        void EnrollStudent(EnrollStudentRequest request);
        void PromoteStudents(PromoteStudentsRequest request);
    }
}