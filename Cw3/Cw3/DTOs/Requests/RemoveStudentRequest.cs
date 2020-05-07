using System;
using System.ComponentModel.DataAnnotations;

namespace Cw3.DTOs.Requests
{
    public class RemoveStudentRequest
    {
        [Required] public string IndexNumber { get; set; }
    }
}