﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Cw3.Models
{
    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Semester { get; set; }
        public string StudiesName { get; set; }
    }
}