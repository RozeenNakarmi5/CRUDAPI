using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDOperationAPI.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using CRUDOperationAPI.ViewModels;
using CRUDOperationAPI.InterfaceClass;
using Microsoft.Extensions.Options;
using CRUDOperationAPI.Connections;

namespace CRUDOperationAPI.Implementation
{
    public class EmployeeImplementation : IConnection, IEmployee
    {
        private string _connectionString;

        //private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);

        public EmployeeImplementation(IOptions<ConnectionConfig> connectionConfig)
        {
            var connection = connectionConfig.Value;
            string connectionString = connection.myconn;
            _connectionString = Connections(connectionString);
          
        }
        public List<EmployeeContacts> GetAll()
        {
            var data = new List<EmployeeContacts>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<EmployeeContacts>("SELECT Employees.EmployeeID, Contacts.ContactID, Contacts.FirstName, Contacts.LastName, Contacts.Address, Contacts.Email, Contacts.ContactNumber, Contacts.EmergencyContactNumber, Employees.Designation, Employees.Salary, Employees.WorkingHrPerday, Employees.IsFullTimer FROM Employees Join Contacts On (Employees.ContactID = Contacts.ContactID)").ToList();
            }
            return data;
        }

        public EmployeeContacts GetEmployeeByID(int id)
        {
            try
             {
                EmployeeContacts data;

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    data = db.Query<EmployeeContacts>("SELECT Employees.EmployeeID, Contacts.ContactID, Contacts.FirstName, Contacts.LastName, Contacts.Address, Contacts.Email, Contacts.ContactNumber, Contacts.EmergencyContactNumber, Employees.Designation, Employees.Department, Employees.Salary, Employees.WorkingHrPerday, Employees.IsFullTimer FROM Employees Join Contacts On (Employees.ContactID = Contacts.ContactID) where Contacts.ContactID = @ContactID", new { ContactID = id }).SingleOrDefault();
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public int DeleteEmployee(int id)
        {
            try
            {
                int exe;
                //var data = new Employee();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string data = "Delete from Contacts where ContactID = @ContactID";
                    exe = db.Execute(data, new
                    {
                        ContactID = id
                    });
                }
                return exe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void PostEmployee(EmployeeContacts emp)
        {
            //var data = new Employee();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@FirstName", emp.FirstName);
                    parameter.Add("@LastName", emp.LastName);
                    parameter.Add("@Address", emp.Address);
                    parameter.Add("@Email", emp.Email);
                    parameter.Add("@ContactNumber", emp.ContactNumber);
                    parameter.Add("@EmergyContactNumber", emp.EmergencyContactNumber);
                    parameter.Add("@Designation", emp.Designation);
                    parameter.Add("@Salary", emp.Salary);
                    parameter.Add("@WorkingHrPerDa", emp.WorkingHrPerDay);
                    parameter.Add("@IsFullTimer", emp.IsFullTimer);
                    parameter.Add("@Department", emp.Department);


                    db.Execute("InsertIntoContactsAndEmployee", parameter, commandType: CommandType.StoredProcedure);
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public void PutEmployee(EmployeeContacts emp)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@FirstName", emp.FirstName);
                    parameter.Add("@LastName", emp.LastName);
                    parameter.Add("@Address", emp.Address);
                    parameter.Add("@Email", emp.Email);
                    parameter.Add("@ContactNumber", emp.ContactNumber);
                    parameter.Add("@EmergencyContactNumber", emp.EmergencyContactNumber);
                    parameter.Add("@Designation", emp.Designation);
                    parameter.Add("@Salary", emp.Salary);
                    parameter.Add("@WorkingHrPerDa", emp.WorkingHrPerDay);
                    parameter.Add("@IsFullTimer", emp.IsFullTimer);
                    parameter.Add("@Department", emp.Department);
                    parameter.Add("@ContactID", emp.ContactID);
                    db.Execute("UpdateContactsAndEmployee", parameter, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public int CountEmployee()
        {
            int exe;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // string sqlQuery = "Select Count(Distinct(EmployeeID)) from Employees";
                // exe= db.Execute(sqlQuery);
                 exe = db.Query<int>("Select Count(Distinct(EmployeeID)) from Employees").FirstOrDefault();
            }
            return exe;
        }

        public string Connections(string ConnectionString)
        {
            return ConnectionString;
        }

        public List<EmployeeProjectViewModel> GetEmployeeWithProject()
        {
            var data = new List<EmployeeProjectViewModel>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<EmployeeProjectViewModel>("SELECT Employees.EmployeeID, Contacts.ContactID, Contacts.FirstName, Contacts.LastName, Contacts.Address, Contacts.Email, Contacts.ContactNumber, Contacts.EmergencyContactNumber, Employees.Designation, Employees.Department, Projects.ProjectID, Projects.ProjectName,EmployeeProject.EmployeeProjectID FROM EmployeeProject Join Employees On (Employees.EmployeeID = EmployeeProject.EmployeeID) Join Contacts ON (Contacts.ContactID = Employees.ContactID) JOIN Projects ON (Projects.ProjectID = EmployeeProject.ProjectID)").ToList();
            }
            return data;
        }

        public void AssignProjectToEmployee(EmployeeProjectViewModel empPro)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@EmployeeID", empPro.EmployeeID);
                    parameter.Add("@ProjectID", empPro.ProjectID);
                    db.Execute("AssignProjectToEmployee", parameter, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void UpdateProjectToEmployee(EmployeeProjectViewModel empPro)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@EmployeeID", empPro.EmployeeID);
                    parameter.Add("@ProjectID", empPro.ProjectID);
                    parameter.Add("@EmployeeProjectID", empPro.EmployeeProjectID);
                    db.Execute("UpdateProjectEmployee", parameter, commandType: CommandType.StoredProcedure);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteEmployeeAndProject(int id)
        {
            try
            {
                int exe;
                //var data = new Employee();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string data = "Delete from EmployeeProject where EmployeeProjectID = @EmployeeProjectID";
                    exe = db.Execute(data, new
                    {
                        EmployeeProjectID = id
                    });
                }
                return exe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
