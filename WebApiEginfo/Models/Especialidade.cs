using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEginfoRH.Models
{
    [Table("tb_Especialidade")]
    public class Especialidade
    {
        public Especialidade()
        {
            this.Candidatos = new HashSet<Candidato>();
        }
        public int id { get; set; }
        public string nome { get; set; }
        
        public virtual ICollection<Candidato> Candidatos { get; set; }
    }
}