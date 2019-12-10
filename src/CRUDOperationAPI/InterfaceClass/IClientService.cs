using CRUDOperationAPI.Models;
using CRUDOperationAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.InterfaceClass
{
    public interface IClientService
    {
        List<ClientProjectViewModel> GetClientProject();
        ClientProjectViewModel GetClientByID(int id);
        int DeleteClient(int id);
        void PostClient(ClientProjectViewModel client);
        void PutClient(ClientProjectViewModel client);
        int CountClient();
        List<ClientProjectViewModel> GetALL();
        void UpdateClientProject(ClientProjectViewModel client);
    }
}
