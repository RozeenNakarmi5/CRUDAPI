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

namespace CRUDOperationAPI.Implementation
{
    public class ProjectImplementation : IConnectionService, IProjectService
    {
        private string _connectionString;
        public ProjectImplementation(IOptions<ConnectionConfig> connectionConfig)
        {
            var connection = connectionConfig.Value;
            string connectionString = connection.myconn;
            _connectionString = Connections(connectionString);
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
                exe = db.Query<int>("Select Count(Distinct(ProjectID)) from Projects").FirstOrDefault();
            }
            return exe;
        }

        public int DeleteProject(int id)
        {
            try
            {
                int exe;
                //var data = new Employee();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string data = "Delete from Projects where ProjectID = @ProjectID";
                    exe = db.Execute(data, new
                    {
                        ProjectID = id
                    });
                }
                return exe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientProjectViewModel> GetAll()
        {
            var data = new List<ClientProjectViewModel>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<ClientProjectViewModel>("Select * from Projects").ToList();
            }
            return data;
        }

        public ClientProjectViewModel GetProjectByID(int id)
        {
            try
            {
                ClientProjectViewModel data;

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    data = db.Query<ClientProjectViewModel>("SELECT * from Projects where ProjectID = @ProjectID", new { ProjectID = id }).SingleOrDefault();
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void  PostProject(ClientProjectViewModel client)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameter = new DynamicParameters();
                     parameter.Add("@ProjectName", client.ProjectName);
                     parameter.Add("@ProjectStartDate", Convert.ToDateTime(client.ProjectStartDate));
                     parameter.Add("@ProjectEndDate", Convert.ToDateTime(client.ProjectEndDate));
                     db.Execute("InsertIntoProject", parameter, commandType: CommandType.StoredProcedure);


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void PutProject(ClientProjectViewModel client)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                try
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@ProjectID", client.ProjectID);
                    parameter.Add("@ProjectName", client.ProjectName);
                    parameter.Add("@ProjectStartDate", client.ProjectStartDate);
                    parameter.Add("@ProjectEndDate", client.ProjectEndDate);
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
