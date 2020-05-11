using System;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw3.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cw3.Services
{
    public class SqlServerStudentDbService : IDbService
    {
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s18747;Integrated Security=True";

        private readonly StrupiechContext _context;

        public SqlServerStudentDbService(StrupiechContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> EnrollStudent(EnrollStudentRequest request)
        {
            try
            {
                var studiesId = Convert.ToInt32(_context.Studies.Where(st => st.Name == request.StudiesName).Select(
                    st =>
                        new
                        {
                            st.IdStudy
                        }).FirstOrDefault());

                if (studiesId == 0) return new BadRequestResult();

                var enrollment = _context.Enrollment.FirstOrDefault(en => en.IdStudy == studiesId && en.Semester == 1);
                int enrollmentId;
                if (enrollment == null)
                {
                    enrollmentId = _context.Enrollment.Max(enr => enr.IdEnrollment) + 1;
                    _context.Enrollment.Add(new Enrollment()
                    {
                        IdEnrollment = enrollmentId,
                        Semester = 1,
                        IdStudy = studiesId,
                        StartDate = DateTime.Now
                    });
                }
                else
                    enrollmentId = enrollment.IdEnrollment;

                var newStudent = _context.Student.First(st => st.IndexNumber == request.IndexNumber);

                if (newStudent != null) return new BadRequestResult();

                _context.Student.Add(new Student()
                {
                    IdEnrollment = enrollmentId,
                    BirthDate = request.BirthDate,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                });

                await _context.SaveChangesAsync();
                return new AcceptedResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> PromoteStudents(PromoteStudentsRequest request)
        {
            try
            {
                var enrollment = _context.Enrollment.Join(_context.Studies,
                    en => en.IdStudy,
                    st => st.IdStudy,
                    (en, st) => new
                    {
                        enrolIdStud = en.IdStudy,
                        studIdStud = st.IdStudy,
                        st.Name,
                        en.Semester
                    }).FirstOrDefault(arg => arg.enrolIdStud == arg.studIdStud &&
                                             arg.Name == request.StudiesName &&
                                             arg.Semester == request.Semester);

                if (enrollment == null) return new BadRequestResult();

                _context.Database.ExecuteSqlInterpolated($"PromoteStudents {request.Semester},{request.StudiesName}");
                return new AcceptedResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestResult();
            }
        }

        public async Task<IActionResult> GetStudents()
        {
            return new OkObjectResult(_context.Student.ToList());
        }

        public async Task<IActionResult> ModifyStudent(ModifyStudentRequest request)
        {
            var student = _context.Student.First(st => st.IndexNumber == request.IndexNumber);
            if (student == null) return new BadRequestResult();
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.BirthDate = request.BirthDate;
            if (request.IdEnrollment != 0)
            {
                student.IdEnrollment = request.IdEnrollment;
            }

            await _context.SaveChangesAsync();
            return new OkObjectResult(student);
        }

        public async Task<IActionResult> RemoveStudent(RemoveStudentRequest request)
        {
            var student = _context.Student.First(s => s.IndexNumber == request.IndexNumber);
            if (student == null) return new BadRequestResult();
            _context.Remove(student);
            _context.SaveChanges();
            return new OkObjectResult(student);
        }

        public bool CheckIndexNumber(string index)
        {
            using (var connection = new SqlConnection())
            using (var command = new SqlCommand())
            {
                connection.ConnectionString = ConnectionString;
                command.Connection = connection;

                connection.Open();
                command.CommandText = "SELECT 1 FROM Student WHERE Student.IndexNumber = @index";
                command.Parameters.AddWithValue("index", index);

                var dataReader = command.ExecuteReader();
                return dataReader.Read();
            }
        }

        public bool CheckUserCredentials(string index, string password)
        {
            using (var connection = new SqlConnection())
            using (var command = new SqlCommand())
            {
                connection.ConnectionString = ConnectionString;
                command.Connection = connection;

                connection.Open();
                command.CommandText =
                    "SELECT 1 FROM Student WHERE Student.IndexNumber = @index AND Student.Password = @password";
                command.Parameters.AddWithValue("index", index);
                command.Parameters.AddWithValue("password", password);

                var dataReader = command.ExecuteReader();
                return dataReader.Read();
            }
        }
    }
}