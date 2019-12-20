using CRUDOperationAPI.Connections;
using CRUDOperationAPI.InterfaceClass;
using CRUDOperationAPI.ViewModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRUDOperationAPI.Models;
using Dapper;
using CRUDOperationAPI.Contexts;

namespace CRUDOperationAPI.Implementation
{
    public class ClientImplementation : IConnectionService, IClientService
    {
        private string _connectionString;
        private EmployeeDbContext _db;
        string message = "";
        public ClientImplementation(IOptions<ConnectionConfig> connectionConfig, EmployeeDbContext db)
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

        public int CountClient()
        {
            int exe;
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // string sqlQuery = "Select Count(Distinct(EmployeeID)) from Employees";
                // exe= db.Execute(sqlQuery);
                exe = db.Query<int>("Select Count(Distinct(ClientID)) from Clients").FirstOrDefault();
            }
            return exe;
        }

        public int DeleteClient(int id)
        {
            try
            {
                int exe;
                //var data = new Employee();
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string data = "Delete from Clients where ClientID = @ClientID";
                    exe = db.Execute(data, new
                    {
                        ClientID = id
                    });
                }
                return exe;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ClientProjectViewModel> GetClientProject()
        {
            var data = new List<ClientProjectViewModel>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<ClientProjectViewModel>("Select ClientProject.ClientProjectID,Clients.ClientID, Clients.ClientFirstName, Clients.ClientLastName, Clients.ClientOffice, Clients.OfficeAddress, Clients.ClientContactNumber, Projects.ProjectID, Projects.ProjectName  from ClientProject Join Clients ON (Clients.ClientID = ClientProject.ClientID) JOIN Projects ON (Projects.ProjectID = ClientProject.ProjectID)").ToList();
            }
            return data;
        }

        public ClientProjectViewModel GetClientByID(int id)
        {
            try
            {
                ClientProjectViewModel data;

                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    data = db.Query<ClientProjectViewModel>("SELECT * from Clients where ClientID = @ClientID", new { ClientID = id }).SingleOrDefault();
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PostClient(ClientProjectViewModel client)
        {
            try
            {
                //using (IDbConnection db = new SqlConnection(_connectionString))
                //{
                //    var parameter = new DynamicParameters();

                //    foreach (var items in client.ProjectID)
                //    {
                //        parameter.Add("@ProjectID", items);
                //    }
                //    parameter.Add("@ClientFirstName", client.ClientFirstName);
                //    parameter.Add("@ClientLastName", client.ClientLastName);
                //    parameter.Add("@ClientOffice", client.ClientOffice);
                //    parameter.Add("@OfficeAddress", client.OfficeAddress);
                //    parameter.Add("@ClientContactNumber", client.ClientContactNumber);
                //    db.Execute("InsertIntoClientProjects", parameter, commandType: CommandType.StoredProcedure);
                //_db.Clients.Add(client.ClientID, client.ClientFirstName, client.ClientLastName, client.ClientOffice, client.ClientContactNumber);
                var getContact = _db.Clients.Where(x => x.ClientContactNumber == client.ClientContactNumber && x.ClientFirstName == client.ClientFirstName && x.ClientLastName == client.ClientLastName);
                if (getContact.Count() == 0)
                {
                    var clientDetail = new Clients
                    {

                        ClientFirstName = client.ClientFirstName,
                        ClientLastName = client.ClientLastName,
                        ClientContactNumber = client.ClientContactNumber,
                        ClientOffice = client.ClientOffice,
                        OfficeAddress = client.OfficeAddress
                    };
                    _db.Clients.Add(clientDetail);
                    //_db.SaveChanges();
                    foreach (var x in client.ProjectID)
                    {
                        var clientProject = new ClientProject
                        {
                            //ClientID = clientDetail.ClientID,
                            Clients=clientDetail,
                            ProjectID = x
                        };
                        _db.ClientProject.Add(clientProject);
                    }
                    _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AssignProjectToClient(ClientProjectViewModel assignProject)
        {
            //var getClient = (from user in _db.Clients
            //                 join clientproject in _db.ClientProject on user.ClientID equals clientproject.ClientID

            //                 where user.ClientID == assignProject.ClientID
            //                 select user.ClientID).FirstOrDefault();
           

           
                foreach (var x in assignProject.ProjectID)
                {
                //var getProjectID = (from project in _db.Projects
                //                    join clientproject in _db.ClientProject on project.ProjectID equals clientproject.ProjectID
                //                    where project.ProjectID == x
                //                    select project.ProjectID).FirstOrDefault();
               var getClientProject = (from clientproject in _db.ClientProject
                                       //join client in _db.Clients on clientproject.ClientID equals client.ClientID
                                       //join project in _db.Projects on clientproject.ProjectID equals project.ProjectID
                                       where clientproject.ClientID == assignProject.ClientID && clientproject.ProjectID == x
                                       select clientproject).Any();
                    if (getClientProject == false)
                    {
                        var projectAssign = new ClientProject
                        {
                            ClientID = assignProject.ClientID,
                            ProjectID = x
                        };
                        _db.ClientProject.Add(projectAssign);
                    }
                _db.SaveChanges();
            }


        }
        public void PutClient(ClientProjectViewModel client)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    var parameter = new DynamicParameters();
                    parameter.Add("@ClientFirstName", client.ClientFirstName);
                    parameter.Add("@ClientLastName", client.ClientLastName);
                    parameter.Add("@ClientOffice", client.ClientOffice);
                    parameter.Add("@OfficeAddress", client.OfficeAddress);
                    parameter.Add("@ClientContactNumber", client.ClientContactNumber);
                    parameter.Add("@ClientID", client.ClientID);
                    db.Execute("UpdateClient", parameter, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateClientProject(ClientProjectViewModel client)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                var parameter = new DynamicParameters();
                parameter.Add("@ClientID", client.ClientID);
                parameter.Add("@ClientProjectID", client.ClientProjectID);
                parameter.Add("@ProjectID", client.ProjectID);
                db.Execute("UpdateClientProject", parameter, commandType: CommandType.StoredProcedure);
            }
        }
        public List<ClientProjectViewModel> GetALL()
        {
            var data = new List<ClientProjectViewModel>();
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                data = db.Query<ClientProjectViewModel>("Select * from clients").ToList();
            }
            return data;
        }

        
    }
}
