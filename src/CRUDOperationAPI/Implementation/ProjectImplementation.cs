using CRUDOperationAPI.InterfaceClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDOperationAPI.ViewModels;
using CRUDOperationAPI.Connections;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using CRUDOperationAPI.Contexts;

namespace CRUDOperationAPI.Implementation
{
    public class ProjectImplementation : IConnectionService, IProjectService
    {
        private string _connectionString;
        private readonly EmployeeDbContext _db;

        public ProjectImplementation(IOptions<ConnectionConfig> connectionConfig, EmployeeDbContext db)
        {
            var connection = connectionConfig.Value;
            string connectionString = connection.myconn;
            _connectionString = Connections(connectionString);
            _db = db;
        }

       

        public string Connections(string ConnectionString)
        {
            return ConnectionString;
        }
        public int CountProject()
        {
            int exe;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                exe = db.Query<int>("Select Count(Distinct(ProjectID)) from Projects where Projects.IsActive = 1").FirstOrDefault();
            }
            return exe;
        }

        public int EnableProject(int id)
        {
            var projectDetail = (from project in _db.Projects
                                 where project.ProjectID == id
                                 select project).FirstOrDefault();
            projectDetail.IsActive = true;
            _db.Projects.Update(projectDetail);
            
            return _db.SaveChanges();
        }
        public int DisableProject(int id)
        {
            //try
            //{
            //    int exe;
            //    //var data = new Employee();
            //    using (IDbConnection db = new SqlConnection(_connectionString))
            //    {
            //        string data = "Delete from Projects where ProjectID = @ProjectID";
            //        exe = db.Execute(data, new
            //        {
            //            ProjectID = id
            //        });
            //    }
            //    return exe;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            var projectDetail = (from project in _db.Projects
                                 where project.ProjectID == id
                                 select project).FirstOrDefault();
            projectDetail.IsActive = false;
            _db.Projects.Update(projectDetail);
            var unassignEmployee = (from emp in _db.EmployeeProject
                                    where emp.ProjectID == id
                                    select emp).ToList();
            foreach(var x in unassignEmployee)
            {
                _db.EmployeeProject.Remove(x);
            }
            var unassignClients = (from clients in _db.ClientProject
                                   where clients.ProjectID == id
                                   select clients).ToList();
            foreach(var y in unassignClients)
            {
                _db.ClientProject.Remove(y);
            }
            return _db.SaveChanges();
        }
        

        public List<ProjectViewModel> GetNow()
        {
            var data = new List<ProjectViewModel>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<ProjectViewModel>("Select * from Projects where Projects.IsActive = 1").ToList();
            }
            return data;
        }
        public List<ProjectViewModel> GetScrap()
        {
            var data = new List<ProjectViewModel>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<ProjectViewModel>("Select * from Projects where Projects.IsActive = 0").ToList();
            }
            return data;
        }

        public ProjectViewModel GetProjectByID(int id)
        {
            try
            {
                ProjectViewModel data;

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    data = db.Query<ProjectViewModel>("SELECT * from Projects where ProjectID = @ProjectID", new { ProjectID = id }).SingleOrDefault();
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        public void  PostProject(ProjectViewModel project)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@ProjectName", project.ProjectName);
                    parameter.Add("@ProjectDescription", project.ProjectDescription); 
                    parameter.Add("@ProjectStartDate", project.ProjectStartDate);
                    parameter.Add("@ProjectEndDate", project.ProjectEndDate);
                    parameter.Add("True", project.IsActive);
                    parameter.Add("@CreatedTimeStamp", project.CreatedTimeStamp);
                    db.Execute("InsertIntoProject", parameter, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void PutProject(ProjectViewModel project)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@ProjectID", project.ProjectID);
                    parameter.Add("@ProjectName", project.ProjectName);
                    parameter.Add("@projectDescription", project.ProjectDescription);
                    parameter.Add("@ProjectStartDate", project.ProjectStartDate);
                    parameter.Add("@ProjectEndDate", project.ProjectEndDate);
                    parameter.Add("@ModifiedTimeStamp", project.ModifiedTimeStamp);
                    db.Execute("UpdateProject", parameter, commandType: CommandType.StoredProcedure);
                      }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        
    }
}
