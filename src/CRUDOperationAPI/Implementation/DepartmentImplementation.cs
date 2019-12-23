using CRUDOperationAPI.InterfaceClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDOperationAPI.Models;
using CRUDOperationAPI.Contexts;

namespace CRUDOperationAPI.Implementation
{
    public class DepartmentImplementation : IDepartmentService
    {
        private readonly EmployeeDbContext _database;
        public DepartmentImplementation(EmployeeDbContext database)
        {
            _database = database;
        }

        public int CountDepartment()
        {
            return (_database.Departments.ToList()).Count();
        }

        public int DeleteDepartment(int id)
        {
            var selectDepartment = (from dep in _database.Departments
                                    where dep.DepartmentID == id
                                    select dep).FirstOrDefault();
            _database.Departments.Remove(selectDepartment);
            return _database.SaveChanges();
        }

        public List<Departments> GetAll()
        {
           return _database.Departments.ToList();
        }

        public Departments GetDepartmentByID(int id)
        {
            var department = (from dept in _database.Departments
                             where dept.DepartmentID == id
                             select dept).FirstOrDefault();
            return department;
        }

        public void PostDepartment(Departments dept)
        {
            var selectDepartmentName = (from dep in _database.Departments
                                       where dep.DepartmentName == dept.DepartmentName
                                       select dept).Any();
            if (selectDepartmentName == false)
            {
                var department = new Departments
                {
                    DepartmentName = dept.DepartmentName
                };
                _database.Departments.Add(department);
                _database.SaveChanges();
            }
            else
            {
                throw new Exception();
            }
            
        }

        public void PutDepartment(Departments dept)
        {
            var selectDepartmentName = (from dep in _database.Departments
                                        where dep.DepartmentName == dept.DepartmentName
                                        select dept).Any();
            if (selectDepartmentName == false)
            {
                var getDeptId = (from dep in _database.Departments
                                 where dep.DepartmentID == dept.DepartmentID
                                 select dep).SingleOrDefault();
                getDeptId.DepartmentName = dept.DepartmentName;
                _database.Departments.Update(getDeptId);
                _database.SaveChanges();
            }
            else
            {
                throw new Exception();
            }

        }
    }
}
