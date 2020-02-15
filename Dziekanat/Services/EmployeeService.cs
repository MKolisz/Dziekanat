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
        public void Update(Employee employeeParam, int employeeId);
        public Employee GetById(int myId);
        public IEnumerable<Employee> GetAll();
        public void Delete(int employeeId);
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

        public void Update(Employee employeeParam, int employeeId)
        {
            var employee = _context.Employee.Find(employeeId);

            if (employee == null)
                throw new AppException("Employee not found");

            if (employeeParam.First_Name != null && employeeParam.First_Name.Length > 30)
                throw new AppException("First Name can contain max 30 characters");
            if (employeeParam.Last_Name != null && employeeParam.Last_Name.Length > 30)
                throw new AppException("Last Name can contain max 30 characters");



            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(employeeParam.First_Name))
                employee.First_Name = employeeParam.First_Name;
            if (!string.IsNullOrWhiteSpace(employeeParam.Last_Name))
                employee.Last_Name = employeeParam.Last_Name;

            if (employeeParam.Department_Id > 0)
                employee.Department_Id = employeeParam.Department_Id;



            _context.Employee.Update(employee);
            _context.SaveChanges();
        }

        public Employee GetById(int employeeId)
        {
            return _context.Employee.Find(employeeId);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employee;
        }

        public void Delete(int employeeId)
        {
            var employee = _context.Employee.Find(employeeId);
            if(employee!=null)
            {
                _context.Employee.Remove(employee);
                _context.SaveChanges();
            }
            else throw new AppException("Employee not found");
        }
    }
}
