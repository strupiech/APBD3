using System;
using System.ComponentModel.DataAnnotations;

namespace Cw3.DTOs.Requests
{
    public class ModifyStudentRequest
    {
        [Required] public string IndexNumber { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public DateTime BirthDate { get; set; }
        [Required]public int IdEnrollment { get; set; }
    }
}