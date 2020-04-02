using System;
using System.ComponentModel.DataAnnotations;

namespace Cw3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [RegularExpression("^s[0-9]+$")] public string IndexNumber { get; set; }

        [Required] [MaxLength(100)] public string FirstName { get; set; }

        [Required] [MaxLength(100)] public string LastName { get; set; }

        [Required] public DateTime BirthDate { get; set; }

        [Required] public string Semester { get; set; }

        [Required] public string StudiesName { get; set; }
    }
}