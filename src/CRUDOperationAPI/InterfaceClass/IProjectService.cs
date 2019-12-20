using CRUDOperationAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.InterfaceClass
{
    public interface IProjectService
    {
        List<ProjectViewModel> GetNow();
        List<ProjectViewModel> GetScrap();
        ProjectViewModel GetProjectByID(int id);
        int DisableProject(int id);
        int EnableProject(int id);
        void PostProject(ProjectViewModel client);
        void PutProject(ProjectViewModel client);
        int CountProject();
    }
}
