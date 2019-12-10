using CRUDOperationAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.InterfaceClass
{
    public interface IProjectService
    {
        List<ClientProjectViewModel> GetAll();
        ClientProjectViewModel GetProjectByID(int id);
        int DeleteProject(int id);
        void PostProject(ClientProjectViewModel client);
        void PutProject(ClientProjectViewModel client);
        int CountProject();
    }
}
