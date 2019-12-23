using CRUDOperationAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.InterfaceClass
{
    public interface IDepartmentService
    {
        List<Departments> GetAll();
        Departments GetDepartmentByID(int id);
        void PostDepartment(Departments dept);
        void PutDepartment(Departments dept);
        int CountDepartment();
        int DeleteDepartment(int id);


    }
}
