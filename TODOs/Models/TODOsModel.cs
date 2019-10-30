using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TODOs.Data.Entities;

namespace TODOs.Models
{
    public class TODOsModel
    {
        public string Descripcion { get; set; }
        public Estado Estado { get; set; }
        public string RutaAdjunto { get; set; }
        public HttpPostedFile Adjunto { get; set; }
    }
}