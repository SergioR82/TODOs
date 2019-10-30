using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TODOs.Data.Entities;

namespace TODOs.Data.Repositories
{
    public interface ITODOsRepository: IDisposable
    {
        IEnumerable<TODO> GetAllTODOs();
        TODO GetTODObyId(int id);
        IEnumerable<TODO> FindTODOs(int id, string descr, Estado estado);
        void CreateTODO(string descr, Estado est, string ruta, HttpPostedFile adjunto);
        bool EditEstado(Estado est, TODO item);
        void Save();
    }
}