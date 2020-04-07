using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Cw3.DAL;

namespace Cw3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [RegularExpression("^s[0-9]+$")] public string IndexNumber { get; set; }

        [Required] [MaxLength(100)] public string FirstName { get; set; }

        [Required] [MaxLength(100)] public string LastName { get; set; }

        [Required] [JsonConverter(typeof(JsonDateTimeConverter))] public DateTime BirthDate { get; set; }

        [Required] public string Semester { get; set; }

        [Required] public string StudiesName { get; set; }
    }
}