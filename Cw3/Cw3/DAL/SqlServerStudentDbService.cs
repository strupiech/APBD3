using System;
using System.Data.SqlClient;
using Cw3.DTOs.Requests;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3.DAL
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        public void EnrollStudent(EnrollStudentRequest request)
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
                command.Connection = connection;
                
                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    command.CommandText = "SELECT IdStudy FROM studies WHERE name = @studiesName";
                    command.Parameters.AddWithValue("studiesName", request.StudiesName);

                    var dataReader = command.ExecuteReader();
                    if (!dataReader.Read())
                    {
                        transaction.Rollback();
                    }
                    var idstudies = (int) dataReader["IdStudies"];

                    command.CommandText = "SELECT IdEnrollment FROM Enrollments WHERE IdStudy = @idstudy && Semester = 1";
                    command.Parameters.AddWithValue("idstudy", idstudies);
                    dataReader = command.ExecuteReader();
                    
                    if (!dataReader.Read())
                    {
                        command.CommandText =
                            "INSERT INTO Enrollments (Semester, IdStudy, StartDate) VALUES (1, @idstudy, @date)";    
                        command.Parameters.AddWithValue("date", DateTime.Now);
                        command.ExecuteNonQuery();
                        command.CommandText = "SELECT IdEnrollment FROM Enrollments WHERE IdStudy = @idstudy && Semester = 1";
                        dataReader = command.ExecuteReader();
                    }
                    var idEnrollment = (int)dataReader["IdEnrollment"];

                    command.CommandText = "SELECT 1 FROM Student WHERE IndexNumber = @indexNumber";
                    command.Parameters.AddWithValue("indexNumber", request.IndexNumber);
                    dataReader = command.ExecuteReader();
                    if (dataReader.Read())
                    {
                        transaction.Rollback();
                    }
                    
                    command.CommandText =
                        "INSERT INTO Student(FirstName, LastName, IndexNumber, BirthDate, IdEnrollment) VALUES (@firstName, @lastName, @indexNumber, @birthDate, @idEnrollment)";
                    command.Parameters.AddWithValue("idEnrollment", idEnrollment);
                    command.Parameters.AddWithValue("birthDate", request.BirthDate);
                    command.Parameters.AddWithValue("firstName", request.FirstName);
                    command.Parameters.AddWithValue("lastName", request.LastName);
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ignored)
                {
                    transaction.Rollback();
                }
            }
        }

        public void PromoteStudents(int semester, string studies)
        {
            throw new System.NotImplementedException();
        }
    }
}