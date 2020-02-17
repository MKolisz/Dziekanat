using Dziekanat.Entities;
using Dziekanat.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dziekanat.Services
{
    public interface IGroupService
    {
        void Create(Group group);
        void Update(Group group, int groupId);
        public IEnumerable<Group> GetAll();
        public void Delete(int groupId);
    }

    public class GroupService:IGroupService
    {
        private GroupContext _context;

        public GroupService(GroupContext context)
        {
            _context = context;
        }

        public void Create(Group group)
        {
            if (string.IsNullOrWhiteSpace(group.Group_Name))
                throw new AppException("Group Name is required");

            _context.Group.Add(group);
            _context.SaveChanges();
        }

        public void Update(Group groupParam, int groupId)
        {
            var group = _context.Group.Find(groupId);

            if (group == null)
                throw new AppException("Group not found");

            if (groupParam.Group_Name != null && groupParam.Group_Name.Length > 10)
                throw new AppException("Group Name can contain max 10 characters");

            if (!string.IsNullOrWhiteSpace(groupParam.Group_Name))
                group.Group_Name = groupParam.Group_Name;

            _context.Group.Update(group);
            _context.SaveChanges();
        }

        public IEnumerable<Group> GetAll()
        {
            return _context.Group;
        }

        public void Delete(int groupId)
        {
            var group = _context.Group.Find(groupId);
            if (group != null)
            {
                _context.Group.Remove(group);
                _context.SaveChanges();
            }
            else throw new AppException("Group not found");
        }
    }
}
