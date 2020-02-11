using Dziekanat.Entities;
using Dziekanat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Services
{
    public interface IEmployeeService
    {
        Employee Create(Employee employee, string password);
    }

    public class EmployeeService : IEmployeeService
    {
        private EmployeeContext _context;

        public EmployeeService(EmployeeContext context)
        {
            _context = context;
        }

        public Employee Create(Employee employee, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Employee.Any(x => x.Email == employee.Email))
                throw new AppException("Email " + employee.Email + " is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            employee.PasswordHash = passwordHash;
            employee.PasswordSalt = passwordSalt;

            _context.Employee.Add(employee);
            _context.SaveChanges();

            return employee;
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
