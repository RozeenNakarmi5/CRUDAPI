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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using OfficeOpenXml;
using CRUDOperationAPI.PaginationClass;


namespace CRUDOperationAPI.Implementation
{
    public class EmployeeImplementation : IConnectionService, IEmployeeService
    {
        private string _connectionString;
        private readonly EmployeeDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;


        //private IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["myconn"].ConnectionString);

        public EmployeeImplementation(IOptions<ConnectionConfig> connectionConfig, IHostingEnvironment hostingEnvironment, EmployeeDbContext db)
        {
            var connection = connectionConfig.Value;
            string connectionString = connection.myconn;
            _connectionString = Connections(connectionString);
            _db = db;
            _hostingEnvironment = hostingEnvironment;


        }
        //public IEnumerable<Employee> GetWorkingEmployee(Pagination pagination)
        //{

        //    //var data = new List<EmployeeContactsRole>();
        //    //using (IDbConnection db = new SqlConnection(_connectionString))
        //    //{
        //    //    data = db.Query<EmployeeContactsRole>("SELECT Employees.EmployeeID, Contacts.ContactID, Contacts.FirstName, Contacts.LastName, Contacts.Address, Contacts.Email, Contacts.ContactNumber, Contacts.EmergencyContactNumber, Employees.Designation, Employees.Salary, Employees.IsFullTimer FROM Employees Join Contacts On (Employees.ContactID = Contacts.ContactID) Where Employees.isWorking = 1").ToList();
        //    //}
        //    //return data;
        //    return _db.Employees.OrderBy(on => on.CreatedTimeStamp)
        //        .Where(x => x.IsWorking == true)
        //        .Skip((pagination.PageNumber - 1) * pagination.PageSize)
        //        .Take(pagination.PageSize)
        //        .ToList();

        //}
        public IEnumerable<EmployeeContactsRole> GetWorkingEmployee(Pagination pagination)
        {
            //var data = new List<EmployeeContactsRole>();
            var data = (from emp in _db.Employees
                        join contact in _db.Contacts on emp.ContactId equals contact.ContactID
                        join users in _db.Users on emp.UserID equals users.UserID
                        join roles in _db.Roles on users.RoleID equals roles.RoleID
                        join departmentEmp in _db.DepartmentEmployee on emp.EmployeeId equals departmentEmp.EmployeeID
                        join department in _db.Departments on departmentEmp.DepartmentID equals department.DepartmentID
                     //join projectemp in _db.EmployeeProject on emp.EmployeeId equals projectemp.EmployeeID
                     //join project in _db.Projects on projectemp.ProjectID equals project.ProjectID
                        orderby contact.FirstName
                        where emp.IsWorking == true
                        select new EmployeeContactsRole
                     {
                         EmployeeID = emp.EmployeeId,
                         ContactID = contact.ContactID,
                         FirstName = contact.FirstName,
                         LastName = contact.LastName,
                         Address = contact.Address,
                         Email = contact.Email,
                         ContactNumber = contact.ContactNumber,
                         ProfilePicture = contact.ProfilePicture,
                         EmergencyContactNumber = contact.EmergencyContactNumber,
                         Designation = emp.Designation,
                         RoleName = roles.RoleName,
                         DepartmentName = department.DepartmentName,
                         Salary = emp.Salary,
                         IsFullTimer = emp.IsFullTimer,
                         //ProjectNames = project.ProjectName
                        })
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
            return data;
        }
        public IEnumerable<EmployeeContactsRole> EmpProject(Pagination pagination)
        {
            //var data = new List<EmployeeContactsRole>();
            var data = (from emp in _db.Employees
                        join contact in _db.Contacts on emp.ContactId equals contact.ContactID
                        join users in _db.Users on emp.UserID equals users.UserID
                        join roles in _db.Roles on users.RoleID equals roles.RoleID
                        join departmentEmp in _db.DepartmentEmployee on emp.EmployeeId equals departmentEmp.EmployeeID
                        join department in _db.Departments on departmentEmp.DepartmentID equals department.DepartmentID
                        join projectemp in _db.EmployeeProject on emp.EmployeeId equals projectemp.EmployeeID
                        join project in _db.Projects on projectemp.ProjectID equals project.ProjectID
                        orderby contact.FirstName
                        where emp.IsWorking == true
                        select new EmployeeContactsRole
                        {
                            EmployeeID = emp.EmployeeId,
                            ContactID = contact.ContactID,
                            FirstName = contact.FirstName,
                            LastName = contact.LastName,
                            Address = contact.Address,
                            Email = contact.Email,
                            ContactNumber = contact.ContactNumber,
                            ProfilePicture = contact.ProfilePicture,
                            EmergencyContactNumber = contact.EmergencyContactNumber,
                            Designation = emp.Designation,
                            RoleName = roles.RoleName,
                            DepartmentName = department.DepartmentName,
                            Salary = emp.Salary,
                            IsFullTimer = emp.IsFullTimer,
                            ProjectNames = project.ProjectName,
                            EmployeeProjectID = projectemp.EmployeeProjectID
                        })
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();
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
                    if (emp.FirstName != null && emp.LastName != null && emp.Address != null && emp.Email != null && emp.ContactNumber != null && emp.EmergencyContactNumber != null &&
                        emp.ProfilePicture != null && emp.Designation != null && emp.Salary != null && emp.UserName != null && emp.IsFullTimer != null && emp.Password != null &&
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
            catch (Exception ex)
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
            if (updateRoles.EmployeeId != 0)
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

        public string ExportEmployeeSchedule()
        {

            string rootFolder = _hostingEnvironment.WebRootPath;
            string fileName = @"ExportEmployeeSchedule.xlsx";
            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));
            using (ExcelPackage package = new ExcelPackage(file))
            {
                IList<EmployeeSchedule> empScheduleList = _db.EmployeeSchedule.ToList();
                string name = "EmployeeSchedule " + DateTime.Now.ToString("dd-MM-yyyy");
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(name);
                int totalRows = empScheduleList.Count();
                worksheet.Cells[1, 1].Value = "Schedule ID";
                worksheet.Cells[1, 2].Value = "Employee ID";
                worksheet.Cells[1, 3].Value = "In Time";
                worksheet.Cells[1, 4].Value = "Out Time";
                worksheet.Cells[1, 5].Value = "Total Hour Work per day";
                int i = 0;
                for (int row = 2; row <= totalRows + 1; row++)
                {
                    worksheet.Cells[row, 1].Value = empScheduleList[i].ScheduleID;
                    worksheet.Cells[row, 2].Value = empScheduleList[i].EmployeeID;
                    worksheet.Cells[row, 3].Value = empScheduleList[i].InTime;
                    worksheet.Cells[row, 4].Value = empScheduleList[i].OutTime;
                    worksheet.Cells[row, 5].Value = empScheduleList[i].TotalHourWorkPerday;
                    i++;
                }
                package.Save();
            }
            return "Employe Schedule List has been exported successfully";
        }

        public IEnumerable<EmployeeScheduleViewModel> GetEmployeeSchedule(Pagination pagination)
        {
            var data = (from empsch in _db.EmployeeSchedule
                        join emp in _db.Employees on empsch.EmployeeID equals emp.EmployeeId
                        join cont in _db.Contacts on emp.ContactId equals cont.ContactID
                        orderby empsch.CreatedTimeStamp
                        select new EmployeeScheduleViewModel
                        {
                            EmployeeID = emp.EmployeeId,
                            EmployeeFirstName = cont.FirstName,
                            EmployeeLastName = cont.LastName,
                            InTime = empsch.InTime,
                            OutTime = empsch.OutTime,
                            TotalHourWorkPerday = empsch.TotalHourWorkPerday
                        })
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToList();


            return data;
        }
        public int CountEmpSchedule()
        {
            var countEmpSche = (from empsche in _db.EmployeeSchedule
                                select empsche).Count();
            return countEmpSche;
        }
        
        public List<EmployeeDepartmentWorkTime> GetTotalWorkingHrs()
        {

            var data = new List<EmployeeDepartmentWorkTime>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {

                data = db.Query<EmployeeDepartmentWorkTime>("select EmployeeSchedule.employeeID, Contacts.FirstName, Departments.DepartmentName, sum(DATEDIFF(HOUR, '0:00:00', EmployeeSchedule.TotalHourWorkPerday)) as totalhrs, " +
                                        "avg(DATEDIFF(HOUR, '0:00:00', EmployeeSchedule.TotalHourWorkPerday)) as avghrs from EmployeeSchedule " +
                                        "join DepartmentEmployee on EmployeeSchedule.employeeID = DepartmentEmployee.employeeID " +
                                        "join Departments on Departments.DepartmentID = DepartmentEmployee.DepartmentID " +
                                        "join Employees on Employees.employeeID = DepartmentEmployee.employeeID " +
                                        "join Contacts on Contacts.ContactID = Employees.ContactId " +
                                        "group by EmployeeSchedule.EmployeeID, Contacts.FirstName, Departments.DepartmentName;").ToList();
            }
            //select EmployeeSchedule.employeeID, Contacts.FirstName, Departments.DepartmentName, sum(DATEDIFF(HOUR, '0:00:00', [EmployeeSchedule].TotalHourWorkPerday)) as totalhrs
            //from[EmployeeDb].[dbo].[EmployeeSchedule]
            //join DepartmentEmployee on EmployeeSchedule.employeeID = DepartmentEmployee.employeeID
            //join Departments on Departments.DepartmentID = DepartmentEmployee.DepartmentID
            //join Employees on Employees.employeeID = DepartmentEmployee.employeeID


            //join Contacts on Contacts.ContactID = Employees.ContactId
            //group by EmployeeSchedule.EmployeeID, Contacts.FirstName, Departments.DepartmentName;
            return data;
        }
		 public int CountEmpProjects()
		        {
		            var countEmpPrj = (from empprj in _db.EmployeeProject
		                                select empprj).Count();
		            return countEmpPrj;
		        }
		
    }
}
