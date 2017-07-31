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
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Curriculo { get; set; }
        public string CaminhoCurriculo { get; set; }
        public int? IdPerfil { get; set; }
        public string NomePerfil { get; set; }
        public string EnderecoCidade { get; set; }
        public ICollection<Especialidade> Lista { get; set; }
    }
}