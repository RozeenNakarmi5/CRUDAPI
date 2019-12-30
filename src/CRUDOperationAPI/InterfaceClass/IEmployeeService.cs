﻿using CRUDOperationAPI.Models;
using CRUDOperationAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI
{
    public interface IEmployeeService
    {
        List<EmployeeContactsRole> GetWorkingEmployee();
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
       


    }
}
