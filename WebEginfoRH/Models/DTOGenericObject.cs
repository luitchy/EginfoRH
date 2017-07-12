using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEginfoRH.Models
{
    public class DTOGenericObject
    {
        public long ID { get; set; }
        public string Nome { get; set; }

        public ICollection<Especialidade> Lista { get; set; }
    }
}