using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using TODOs.Data.Entities;

namespace TODOs.Data.Repositories
{
    public class TODOsRepository : ITODOsRepository, IDisposable
    {
        TODOsContext _context;
        bool disposed = false;

        //Se podria implementar DI aca tambien para tener una mayor separacion. 
        public TODOsRepository(TODOsContext context)
        {
            _context = context;
        }
        public IEnumerable<TODO> GetAllTODOs()
        {
            //Se podria aplicar paginacion en caso de necesitarse, ya sea con ToPagedList
            //o bien usando Take en el query. En ambos casos recibiendo como parametro, la
            //pagina actual a visualizar.
            return _context.TODOs.ToList();
        }

        public IEnumerable<TODO> FindTODOs(int id, string descr, Estado estado)
        {
            //No tendria sentido filtrar en este caso por descripcion y estado
            //ya que se esta filtrando por Id y el Id es unico, pero asi se pidio en 
            //el requerimiento. 
            return _context.TODOs.Where(t => t.Id == id && t.Descripcion == descr
                                          && t.Estado == estado).ToList();
        }

        public TODO GetTODObyId(int id)
        {
            //Uso el Find en vez del FirstorDefault, para que en caso de que no lo encuentre,
            //me devuelva el valor nulo y pueda detectarlo de forma mas facil en el controller.
            return _context.TODOs.Find(id);
        }

        private string AddAttachment(string path, HttpPostedFile file)
        {
            string FileName = Path.GetFileNameWithoutExtension(file.FileName);
            string FileExtension = Path.GetExtension(file.FileName);

            //Me aseguro que el mismo nombre no se repita para distintos TODOs.
            FileName = FileName.Trim() + "_" + DateTime.Now.ToString("ddMMyyyy") + FileExtension;

            //Obtengo la ruta de AppSettings para alojar el archivo.  
            string UploadPath = ConfigurationManager.AppSettings["AttachPath"].ToString();

            path = UploadPath + FileName;

            try
            {
                //Guarda el archivo.  
                file.SaveAs(path);
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al intentar guardar el archivo.", ex);
            }

            return path;

        }

        public void CreateTODO(string descr, Estado est, string ruta, HttpPostedFile adjunto)
        {
            try
            {
                TODO item = new TODO
                {
                    Descripcion = descr,
                    Estado = est
                };

                if (!String.IsNullOrEmpty(ruta))
                    item.RutaAdjunto = AddAttachment(ruta, adjunto);
                else item.RutaAdjunto = "";

                _context.TODOs.Add(item);
                Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un error al crear el TODO recibido.", ex);
            }
        }

        public bool EditEstado(Estado est, TODO item)
        {
            Estado estado;

            if ((Enum.TryParse(est.ToString(), out estado)) &&
                (item.Estado != estado))
            {
                item.Estado = estado;
                Save();
            }
            else return false;

            return true;
        }

        //El metodo Save tiene sentido si se aplicara Dependency Injection, ya que su contenido dependeria
        //del framework utilizado o la forma de persistir los datos.
        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}