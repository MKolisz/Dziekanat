using Dziekanat.Entities;
using Dziekanat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Services
{
    public interface IGradeService
    {
        void Create(Grade grade, int studentId);
        void Update(Grade grade, int gradeId);
        IEnumerable<Grade> GetGrades(int myId, int subjectId);
        void Delete(int gradeId);
    }

    public class GradeService:IGradeService
    {

        private GradeContext _gradeContext;
        private SubjectContext _subjectContext;
        private StudentContext _studentContext;
        public GradeService(GradeContext gradeContext, SubjectContext subjectContext, StudentContext studentContext)
        {
            _gradeContext = gradeContext;
            _subjectContext = subjectContext;
            _studentContext = studentContext;
        }

        public void Create(Grade grade, int studentId)
        {
            grade.Student_Id = studentId;

            if(!_subjectContext.Subject.Any(x=>x.Subject_Id==grade.Subject_Id))
                throw new AppException("Subject does not exist");

            if (!_studentContext.Student.Any(x => x.Student_Id == grade.Student_Id))
                throw new AppException("Student does not exist");

            _gradeContext.Grade.Add(grade);
            _gradeContext.SaveChanges();
        }

        public void Update(Grade gradeParam, int gradeId)
        {

            var grade = _gradeContext.Grade.Find(gradeId);

            if (grade == null)
                throw new AppException("Grade does not exist");

            if (!_studentContext.Student.Any(x => x.Student_Id == gradeParam.Student_Id))
                throw new AppException("Student does not exist");

            grade.Grade_Value = gradeParam.Grade_Value;

            _gradeContext.Grade.Update(grade);
            _gradeContext.SaveChanges();
        }


        public IEnumerable<Grade> GetGrades(int myId, int subjectId)
        {
            if(_studentContext.Student.Find(myId)==null)
                throw new AppException("Student does not exist");

            if (_subjectContext.Subject.Find(subjectId) == null)
                throw new AppException("Subject does not exist");

            return _gradeContext.Grade.Where(x => x.Student_Id == myId && x.Subject_Id==subjectId);
        }

        public void Delete(int gradeId)
        {
            var grade = _gradeContext.Grade.Find(gradeId);
            if (grade != null)
            {
                _gradeContext.Grade.Remove(grade);
                _gradeContext.SaveChanges();
            }
            else throw new AppException("Grade not found");
        }
    }
}
