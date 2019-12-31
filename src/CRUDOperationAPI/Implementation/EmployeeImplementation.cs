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
using CRUDOperationAPI.Contexts;

namespace CRUDOperationAPI.Implementation
{
    public class EmployeeImplementation : IConnectionService, IEmployeeService
    {
        private string _connectionString;
        private readonly EmployeeDbContext _db;
        

        //private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);

        public EmployeeImplementation(IOptions<ConnectionConfig> connectionConfig, EmployeeDbContext db)
        {
            var connection = connectionConfig.Value;
            string connectionString = connection.myconn;
            _connectionString = Connections(connectionString);
            _db = db;
          
        }
        public List<EmployeeContactsRole> GetWorkingEmployee()
        {
            var data = new List<EmployeeContactsRole>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<EmployeeContactsRole>("SELECT Employees.EmployeeID, Contacts.ContactID, Contacts.FirstName, Contacts.LastName, Contacts.Address, Contacts.Email, Contacts.ContactNumber, Contacts.EmergencyContactNumber, Contacts.ProfilePicture, Employees.Designation, Employees.Salary, Employees.IsFullTimer FROM Employees Join Contacts On (Employees.ContactID = Contacts.ContactID) Where Employees.isWorking = 1").ToList();
            }
            return data;
        }

        public List<EmployeeContactsRole> GetNotWorkingEmployee()
        {
            var data = new List<EmployeeContactsRole>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<EmployeeContactsRole>("SELECT Employees.EmployeeID, Contacts.ContactID, Contacts.FirstName, Contacts.LastName, Contacts.Address, Contacts.Email, Contacts.ContactNumber, Contacts.EmergencyContactNumber, Employees.Designation, Employees.Salary, Employees.IsFullTimer FROM Employees Join Contacts On (Employees.ContactID = Contacts.ContactID) Where Employees.isWorking = 0").ToList();
            }
            return data;
        }


        public EmployeeContactsRole GetEmployeeByID(int id)
        {
            try
             {
                EmployeeContactsRole data;

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    data = db.Query<EmployeeContactsRole>("SELECT Employees.EmployeeID, Contacts.ContactID, Contacts.FirstName, Contacts.LastName, Contacts.Address, Contacts.Email, Contacts.ContactNumber, Contacts.EmergencyContactNumber, Employees.Designation, Employees.Salary, Employees.IsFullTimer, Employees.isWorking FROM Employees Join Contacts On (Employees.ContactID = Contacts.ContactID) where Employees.EmployeeID = @EmployeeID", new { EmployeeID = id }).SingleOrDefault();
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public int RemoveEmployee(int id)
        {
            //try
            //{
            //    int exe;
            //    //var data = new Employee();
            //    using (IDbConnection db = new SqlConnection(_connectionString))
            //    {
            //        string data = "Update Employees SET IsWorking = 0 where EmployeeID = @EmployeeID";
            //        exe = db.Execute(data, new
            //        {
            //            EmployeeID = id
            //        });
            //    }
            //    return exe;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            var employeeDetail = (from emp in _db.Employees
                        where emp.EmployeeId == id
                        select emp).FirstOrDefault();
            //null cha ki chaina check
            if (employeeDetail != null)
            {
                employeeDetail.IsWorking = false;
                _db.Employees.Update(employeeDetail);
            
                var userDetail = (from users in _db.Users
                                      where users.UserID == employeeDetail.UserID
                                      select users).FirstOrDefault();
                if (userDetail != null && employeeDetail.IsWorking == false)
                {
                    _db.Users.Remove(userDetail);
                }
                
            }

            return _db.SaveChanges();
        }

        public void PostEmployee(EmployeeContactsRole emp)
        {
            //var data = new Employee();

            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                try
                {
                    if(emp.FirstName != null && emp.LastName != null && emp.Address != null && emp.Email != null && emp.ContactNumber != null && emp.EmergencyContactNumber != null &&
                        emp.ProfilePicture != null && emp.Designation != null && emp.Salary != null && emp.UserName != null && emp.IsFullTimer !=null && emp.Password != null &&
                         emp.RoleID != null)
                    {
                        var parameter = new DynamicParameters();
                        parameter.Add("@FirstName", emp.FirstName);
                        parameter.Add("@LastName", emp.LastName);
                        parameter.Add("@Address", emp.Address);
                        parameter.Add("@Email", emp.Email);
                        parameter.Add("@ContactNumber", emp.ContactNumber);
                        parameter.Add("@EmergyContactNumber", emp.EmergencyContactNumber);
                        parameter.Add("@ProfilePicture", emp.ProfilePicture);
                        parameter.Add("@Designation", emp.Designation);
                        parameter.Add("@Salary", emp.Salary);
                        parameter.Add("@IsFullTimer", emp.IsFullTimer);
                        parameter.Add("@IsWorking", emp.IsWorking);
                        parameter.Add("@UserName", emp.UserName);
                        parameter.Add("@Password", emp.Password);
                        parameter.Add("@RoleID", emp.RoleID);
                        parameter.Add("@DepartmentID", emp.DepartmentID);
                        parameter.Add("@CreatedTimeStamp", emp.CreatedTimeStamp);

                        db.Execute("InsertIntoContactsAndEmployee", parameter, commandType: CommandType.StoredProcedure);
                    }
                    
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
        public void PutEmployee(EmployeeContactsRole emp)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@Designation", emp.Designation);
                    parameter.Add("@IsFullTimer", emp.IsFullTimer);
                    parameter.Add("@Salary", emp.Salary);
                    parameter.Add("@ModifiedTimeStamp", emp.ModifiedTimeStamp);
                    parameter.Add("@EmployeeID", emp.EmployeeID);
                    db.Execute("UpdateEmployee", parameter, commandType: CommandType.StoredProcedure);
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
                 exe = db.Query<int>("Select Count(Distinct(EmployeeID)) from Employees where IsWorking = 1").FirstOrDefault();
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
            foreach (var x in empPro.ProjectID)
            {

                var getEmployeeProject = (from empPrj in _db.EmployeeProject
                                              //join client in _db.Clients on clientproject.ClientID equals client.ClientID
                                              //join project in _db.Projects on clientproject.ProjectID equals project.ProjectID
                                          where empPrj.EmployeeID == empPro.EmployeeID && empPrj.ProjectID == x
                                          select empPrj).Any();
                var workingEmployee = (from emp in _db.Employees
                                       where emp.EmployeeId == empPro.EmployeeID
                                       where emp.IsWorking == true
                                       select emp).Any();

                if (getEmployeeProject == false)
                {
                    if (workingEmployee == true)
                    {
                        var projectAssign = new EmployeeProject
                        {
                            EmployeeID = empPro.EmployeeID,
                            ProjectID = x
                        };
                        _db.EmployeeProject.Add(projectAssign);
                    }
                    
                }
                _db.SaveChanges();

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
        public void AddUsers(UsersDetail userDetail)
        {

            var selectUser = (from user in _db.Users
                              where user.UserName == userDetail.UserName
                             select user).FirstOrDefault();
            
                if (selectUser == null)
                {
                    var addUsers = new Users
                    {
                        UserName = userDetail.UserName,
                        Password = userDetail.Password,
                        RoleID = userDetail.RoleID
                    };
                    _db.Users.Add(addUsers);
                    _db.SaveChanges();
                    var employeeDetail = (from emp in _db.Employees
                                          where emp.EmployeeId == userDetail.EmployeeId
                                          select emp).FirstOrDefault();
                    if (employeeDetail.EmployeeId != 0 && employeeDetail.IsWorking == false)
                    {
                        employeeDetail.UserID = addUsers.UserID;
                        employeeDetail.IsWorking = true;
                        _db.Employees.Update(employeeDetail);
                        _db.SaveChanges();
                    }
                }
        }

        public void UpdateRoleOfEmployee(UpdateRole updateRoles)
        {
            if(updateRoles.EmployeeId != 0)
            {
                var selectRole = (from emp in _db.Employees
                                  join user in _db.Users on emp.UserID equals user.UserID
                                  where emp.EmployeeId == updateRoles.EmployeeId
                                  select user).FirstOrDefault();
                if (selectRole.UserID != 0)
                {
                    try
                    {
                        selectRole.RoleID = updateRoles.RoleID;
                        _db.Users.Update(selectRole);
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void UpdateEmployeeDepartment(EmployeeDepartmentViewModel empDep)
        {
            var selectEmployee = (from dept in _db.DepartmentEmployee
                                  where dept.EmployeeID == empDep.EmployeeId
                                  select dept).FirstOrDefault();
            
            if (selectEmployee == null)
            {
                var assignDepartmentToEmployee = new DepartmentEmployee
                {
                    EmployeeID = empDep.EmployeeId,
                    DepartmentID = empDep.DepartmentID
                };
                _db.DepartmentEmployee.Add(assignDepartmentToEmployee);
            }
            else
            {
                selectEmployee.DepartmentID = empDep.DepartmentID;
                _db.DepartmentEmployee.Update(selectEmployee);
            }
            _db.SaveChanges();
        }

        public void UpdateContact(EmployeeContactsRole con)
        {
            var selectContact = (from contact in _db.Contacts
                                 where contact.ContactID == con.ContactID
                                 select contact).FirstOrDefault();
            selectContact.FirstName = con.FirstName;
            selectContact.LastName = con.LastName;
            selectContact.Email = con.Email;
            selectContact.Address = con.Address;
            selectContact.ContactNumber = con.ContactNumber;
            selectContact.EmergencyContactNumber = con.EmergencyContactNumber;
            selectContact.ProfilePicture = con.ProfilePicture;
            selectContact.ModifiedTimeStamp = con.ModifiedTimeStamp;
            _db.Contacts.Update(selectContact);
            _db.SaveChanges();
        }
    }
}
