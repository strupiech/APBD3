using System;

namespace Cw3.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Semester { get; set; }
        public string StudiesName { get; set; }
    }
}