using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Cw3.DTOs.Requests;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s18747;Integrated Security=True";

        public async Task<IActionResult>  EnrollStudent(EnrollStudentRequest request)
        {
            var student = new Student();
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.IndexNumber = request.IndexNumber;
            student.Semester = request.Semester;
            student.BirthDate = request.BirthDate;
            student.StudiesName = request.StudiesName;

            using (var connection = new SqlConnection())
            using (var command = new SqlCommand())
            {
                connection.ConnectionString = ConnectionString;
                command.Connection = connection;
                
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    command.CommandText = "SELECT IdStudy FROM studies WHERE name = @studiesName";
                    command.Parameters.AddWithValue("studiesName", request.StudiesName);
                    
                    command.Transaction = transaction;
                    var dataReader = command.ExecuteReader();
                    if (!dataReader.Read())
                    {
                        dataReader.Close();
                        transaction.Rollback();
                        return new BadRequestResult();
                    }
                    var idStudies = (int) dataReader["IdStudy"];
                    command.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE IdStudy = @idStudy AND Semester = 1";
                    command.Parameters.AddWithValue("idStudy", idStudies);
                    dataReader.Close();
                    dataReader = command.ExecuteReader();
     
                    if (!dataReader.Read())
                    {
                        command.CommandText =
                            "INSERT INTO Enrollments (Semester, IdStudy, StartDate) VALUES (1, @idStudy, @date)";    
                        command.Parameters.AddWithValue("date", DateTime.Now);
                        dataReader.Close();
                        command.ExecuteNonQuery();
                        command.CommandText = "SELECT IdEnrollment FROM Enrollments WHERE IdStudy = @idstudy && Semester = 1";
                        dataReader.Close();
                        dataReader = command.ExecuteReader();
                    }
                    var idEnrollment = (int)dataReader["IdEnrollment"];

                    command.CommandText = "SELECT 1 FROM Student WHERE IndexNumber = @indexNumber";
                    command.Parameters.AddWithValue("indexNumber", request.IndexNumber);
                    dataReader.Close();
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        dataReader.Close();
                        transaction.Rollback();
                        return new BadRequestResult();
                    }
                    
                    command.CommandText =
                        "INSERT INTO Student(FirstName, LastName, IndexNumber, BirthDate, IdEnrollment) VALUES (@firstName, @lastName, @indexNumber, @birthDate, @idEnrollment)";
                    command.Parameters.AddWithValue("idEnrollment", idEnrollment);
                    command.Parameters.AddWithValue("birthDate", request.BirthDate);
                    command.Parameters.AddWithValue("firstName", request.FirstName);
                    command.Parameters.AddWithValue("lastName", request.LastName);
                    dataReader.Close();
                    command.ExecuteNonQuery();

                    transaction.Commit();
                    return new AcceptedResult();
                }
                catch (SqlException ignored)
                {
                    Console.WriteLine(ignored.ToString());
                    transaction.Rollback();
                    return new BadRequestResult();
                }
            }
        }

        public async Task<IActionResult> PromoteStudents(PromoteStudentsRequest studentsRequest)
        {
            using (var connection = new SqlConnection())
            using (var command = new SqlCommand())
            {
                connection.ConnectionString = ConnectionString;
                command.Connection = connection;
                
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    command.CommandText = "SELECT IdEnrollment FROM Enrollment, Studies WHERE Enrollment.IdStudies = Studies.IdStudies AND Studies.Name = @studiesName AND Enrollment.Semester = @semester";
                    command.Parameters.AddWithValue("studiesName", studentsRequest.StudiesName);
                    command.Parameters.AddWithValue("semester", studentsRequest.Semester);

                    var dataReader = command.ExecuteReader(); 
                    if (!dataReader.Read())
                    {
                        dataReader.Close();
                        transaction.Rollback();
                        return new BadRequestResult();
                    }

                }
                catch (SqlException ignored)
                {
                    transaction.Rollback();
                    return new BadRequestResult();
                }
                
                transaction.Commit();
                return new AcceptedResult();
            }
        }
    }
}