using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TODOs.Data.Entities
{
    public class TODO
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public Estado Estado { get; set; }
        public string RutaAdjunto { get; set; } 
    }
}
