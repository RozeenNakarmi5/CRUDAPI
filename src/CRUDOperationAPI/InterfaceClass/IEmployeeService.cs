using CRUDOperationAPI.Models;
using CRUDOperationAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI
{
    public interface IEmployeeService
    {
        List<EmployeeContactsRole> GetAll();
        EmployeeContactsRole GetEmployeeByID(int id);
        int RemoveEmployee(int id);
        void PostEmployee(EmployeeContactsRole emp);
        void PutEmployee(EmployeeContactsRole emp);
        int CountEmployee();
        List<EmployeeProjectViewModel> GetEmployeeWithProject();
        void AssignProjectToEmployee(EmployeeProjectViewModel empPro);
        void UpdateProjectToEmployee(EmployeeProjectViewModel empPro);
        int DeleteEmployeeAndProject(int id);

    }
}
