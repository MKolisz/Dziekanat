using Dziekanat.Entities;
using Dziekanat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Services
{
    public interface ISubjectService
    {
        void Create(Subject subject);
        void Update(Subject subject, int subjectId);
        public Subject GetById(int subjectId);
        public IEnumerable<Subject> GetAll();
        public void Delete(int subjectId);
    }
    public class SubjectService:ISubjectService
    {

        private SubjectContext _context;
        public SubjectService(SubjectContext context)
        {
            _context = context;
        }

        public void Create(Subject subject)
        {
            if (string.IsNullOrWhiteSpace(subject.Subject_Name))
                throw new AppException("Subject Name is required");

            if (_context.Subject.Any(x => x.Subject_Name == subject.Subject_Name))
                throw new AppException("Subject " + subject.Subject_Name + " already exists");

            _context.Subject.Add(subject);
            _context.SaveChanges();
        }

        public void Update(Subject subjectParam, int subjectId)
        {
            var subject = _context.Subject.Find(subjectId);

            if (subject == null)
                throw new AppException("Subject not found");

            if (subjectParam.Subject_Name != null && subjectParam.Subject_Name.Length > 30)
                throw new AppException("Subject Name can contain max 30 characters");

            if (!string.IsNullOrWhiteSpace(subjectParam.Subject_Name))
                subject.Subject_Name = subjectParam.Subject_Name;

            _context.Subject.Update(subject);
            _context.SaveChanges();
        }

        public Subject GetById(int subjectId)
        {
            return _context.Subject.Find(subjectId);
        }

        public IEnumerable<Subject> GetAll()
        {
            return _context.Subject;
        }

        public void Delete(int subjectId)
        {
            var subject = _context.Subject.Find(subjectId);
            if (subject != null)
            {
                _context.Subject.Remove(subject);
                _context.SaveChanges();
            }
            else throw new AppException("Subject not found");
        }
    }
}
