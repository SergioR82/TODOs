using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TODOs.Data.Entities;
using TODOs.Data.Repositories;
using TODOs.Models;

namespace TODOs.Controllers
{
    public class TODOsController : ApiController
    {
        readonly ITODOsRepository _repo;

        public TODOsController(ITODOsRepository repository)
        {
            _repo = repository;
        }

        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _repo.GetAllTODOs());
        }

        public HttpResponseMessage Get(int id, string descr = "", Estado estado = 0) 
        {
            return Request.CreateResponse(HttpStatusCode.OK, _repo.FindTODOs(id,descr,estado));
        }

        public HttpResponseMessage Post([FromBody]TODOsModel item)
        {
            try
            {
                _repo.CreateTODO(item.Descripcion, item.Estado, item.RutaAdjunto, item.Adjunto);
                return Request.CreateResponse(HttpStatusCode.Created, "El TODO fue ingresado correctamente.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Patch(int id, [FromBody] TODO item)
        {
            try
            {
                var todo = _repo.GetTODObyId(id);
                if (todo == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                if (item == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "El contenido del item TODO recibido es invalido.");

                if (!_repo.EditEstado(item.Estado, todo))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"No se pudo actualizar el estado del TODO {id}");

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
