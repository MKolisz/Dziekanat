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

    }
}
