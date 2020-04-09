using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Cw3.Models;

namespace Cw3.Services
{
    public class MockDbService : ICustomDbService
    {
        private static ICollection<Student> _students;
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s18747;Integrated Security=True";
        static MockDbService()
        {
            _students = new List<Student>();
        }

        public IEnumerable<Student> GetStudents()
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT FirstName, LastName, IndexNumber, BirthDate, Student.IdEnrollment, Studies.Name, Enrollment.Semester FROM Student INNER JOIN Enrollment ON Student.IdEnrollment = Enrollment.IdEnrollment INNER JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy";
                
                connection.Open();
                var dr = command.ExecuteReader();
                while (dr.Read())
                {
                    _students.Add(new Student
                    {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        IndexNumber = dr["IndexNumber"].ToString(),
                        BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                        Semester = dr["Semester"].ToString(),
                        StudiesName = dr["Name"].ToString()
                    });
                }
            }
            return _students;
        }

        public Student GetStudent(string id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "SELECT FirstName, LastName, IndexNumber, BirthDate, Student.IdEnrollment, Studies.Name, Enrollment.Semester FROM Student INNER JOIN Enrollment ON Student.IdEnrollment = Enrollment.IdEnrollment INNER JOIN Studies ON Enrollment.IdStudy = Studies.IdStudy WHERE Student.IndexNumber=@id";
                command.Parameters.AddWithValue("id", id);
                
                Student st = null;
                connection.Open();
                var dr = command.ExecuteReader();
                if (dr.Read())
                {
                    st = new Student
                    {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        IndexNumber = dr["IndexNumber"].ToString(),
                        BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                        Semester = dr["Semester"].ToString(),
                        StudiesName = dr["Name"].ToString()
                    };
                }

                return st;
            }
        }
    }
}