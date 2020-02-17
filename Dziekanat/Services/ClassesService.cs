using Dziekanat.Entities;
using Dziekanat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Services
{
    public interface IClassesService
    {
        void Create(Classes classes);
        void Update(Classes classes, int classesId);
        public IEnumerable<Classes> GetAll(int groupId);
        public void Delete(int classesId);
    }

    public class ClassesService:IClassesService
    {
        private ClassesContext _classesContext;
        private GroupContext _groupContext;
        private SubjectContext _subjectContext;
        private EmployeeContext _employeeContext;
        public ClassesService(ClassesContext classesContext, GroupContext groupContext,
            SubjectContext subjectContext, EmployeeContext employeeContext)
        {
            _classesContext = classesContext;
            _groupContext = groupContext;
            _subjectContext = subjectContext;
            _employeeContext = employeeContext;
        }

        public void Create(Classes classes)
        {
            if (classes.Group_Id < 1 || classes.Subject_Id < 1 || classes.Employee_Id < 1)
                throw new AppException("Group ID, Subject ID and Employee ID needed");
            if(_groupContext.Group.Find(classes.Group_Id)==null)
                throw new AppException("Group does not exist");
            if (_subjectContext.Subject.Find(classes.Subject_Id) == null)
                throw new AppException("Subject does not exist");
            if (_employeeContext.Employee.Find(classes.Employee_Id) == null)
                throw new AppException("Employee does not exist");

            _classesContext.Classes.Add(classes);
            _classesContext.SaveChanges();
        }

        public void Update(Classes classesParam, int classesId)
        {
            var classes = _classesContext.Classes.Find(classesId);
            if (classes == null)
                throw new AppException("Classes not found");

            if (classesParam.Employee_Id > 0 && _employeeContext.Employee.Find(classesParam.Employee_Id) != null)
                classes.Employee_Id = classesParam.Employee_Id;
            else throw new AppException("Employee does no exist");

            if (classesParam.Group_Id>0 && _groupContext.Group.Find(classesParam.Group_Id) != null)
                classes.Group_Id = classesParam.Group_Id;
            else throw new AppException("Group does no exist");

            if (classesParam.Subject_Id>0 && _subjectContext.Subject.Find(classesParam.Subject_Id) != null)
                classes.Subject_Id = classesParam.Subject_Id;
            else throw new AppException("Subject does no exist");

            classes.Time = classesParam.Time;

            _classesContext.Classes.Update(classes);
            _classesContext.SaveChanges();
        }

        public IEnumerable<Classes> GetAll(int groupId)
        {
            if(_groupContext.Group.Find(groupId)==null)
                throw new AppException("Group does not exist");
            return _classesContext.Classes.Where(x => x.Group_Id == groupId && x.Time > DateTime.Now && x.Time < DateTime.Now.AddDays(7));
        }

        public void Delete(int classesId)
        {
            var classes = _classesContext.Classes.Find(classesId);
            if (classes != null)
            {
                _classesContext.Classes.Remove(classes);
                _classesContext.SaveChanges();
            }
            else throw new AppException("Classes not found");
        }
    }
}
