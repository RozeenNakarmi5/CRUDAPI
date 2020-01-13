using CRUDOperationAPI.Models;
using CRUDOperationAPI.PaginationClass;
using CRUDOperationAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeContactsRole> GetWorkingEmployee(Pagination pagination);
        List<EmployeeContactsRole> GetNotWorkingEmployee();
        EmployeeContactsRole GetEmployeeByID(int id);
        int RemoveEmployee(int id);
        void PostEmployee(EmployeeContactsRole emp);
        void PutEmployee(EmployeeContactsRole emp);
        int CountEmployee();
        List<EmployeeProjectViewModel> GetEmployeeWithProject();
        void AssignProjectToEmployee(EmployeeProjectViewModel empPro);
        void UpdateProjectToEmployee(EmployeeProjectViewModel empPro);
        int DeleteEmployeeAndProject(int id);
        void AddUsers(UsersDetail users);
        void UpdateRoleOfEmployee(UpdateRole updateRoles);
        void UpdateEmployeeDepartment(EmployeeDepartmentViewModel empDep);
        void UpdateContact(EmployeeContactsRole con);
        string ExportEmployeeSchedule();
        IEnumerable<EmployeeScheduleViewModel> GetEmployeeSchedule(Pagination pagination);
        int CountEmpSchedule();
        List<EmployeeDepartmentWorkTime> GetTotalWorkingHrs();
    }
}
