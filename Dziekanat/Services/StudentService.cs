using Dziekanat.Entities;
using Dziekanat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Services
{
    public interface IStudentService
    {
        Student Create(Student student, string password);
        void Update(Student studentParam, int studentId);
        void UploadImage(int studentId, byte[] image);
        Student GetById(int studentId);
        IEnumerable<Student> GetAll();
        public void Delete(int studentId);
    }

    public class StudentService : IStudentService
    {
        private StudentContext _context;
        public StudentService(StudentContext context)
        {
            _context = context;
        }

        public Student Create(Student student, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Student.Any(x => x.Email == student.Email))
                throw new AppException("Email " + student.Email + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            student.PasswordHash = passwordHash;
            student.PasswordSalt = passwordSalt;

            _context.Student.Add(student);
            _context.SaveChanges();

            return student;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public void Update(Student studentParam, int studentId)
        {
            var student = _context.Student.Find(studentId);

            if (student == null)
                throw new AppException("User not found");

            if (studentParam.First_Name != null && studentParam.First_Name.Length > 30)
                throw new AppException("First Name can contain max 30 characters");
            if (studentParam.Last_Name != null && studentParam.Last_Name.Length > 30)
                throw new AppException("Last Name can contain max 30 characters");



            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(studentParam.First_Name))
                student.First_Name = studentParam.First_Name;
            if (!string.IsNullOrWhiteSpace(studentParam.Last_Name))
                student.Last_Name = studentParam.Last_Name;

            if (studentParam.Department_Id>0)
                student.Department_Id = studentParam.Department_Id;
            if (!string.IsNullOrWhiteSpace(studentParam.Field_Of_Study))
                student.Field_Of_Study = studentParam.Field_Of_Study;
            if (studentParam.Semester>0)
                student.Semester = studentParam.Semester;


            _context.Student.Update(student);
            _context.SaveChanges();
        }

        public void UploadImage(int studentId, byte[] image)
        {
            var student = _context.Student.Find(studentId);

            if (image.Length > 0)
                student.Image = image;
            _context.Student.Update(student);
            _context.SaveChanges();
        }

        public Student GetById(int studentId)
        {
            return _context.Student.Find(studentId);
        }

        public IEnumerable<Student> GetAll()
        {
            return _context.Student;
        }


        public void Delete(int studentId)
        {
            var student = _context.Student.Find(studentId);
            if (student != null)
            {
                _context.Student.Remove(student);
                _context.SaveChanges();
            }
            else throw new AppException("Student not found");
        }
    }
}
