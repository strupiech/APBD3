using System.ComponentModel.DataAnnotations;

namespace Cw3.DTOs.Requests
{
    public class PromoteStudentsRequest
    {
        [Required] [MaxLength(100)] public string StudiesName { get; set; }
        [Required] public string Semester { get; set; }
    }
}