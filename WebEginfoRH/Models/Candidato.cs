using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEginfoRH.Models
{
    [Table("tb_Candidato")]
    public class Candidato
    {
        public Candidato()
        {
            this.Especialidades = new HashSet<Especialidade>();
        }

        public int id { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public string senha { get; set; }
        public Nullable<int> idPerfil { get; set; }
        public int idEndereco { get; set; }
        public string curriculum { get; set; }

        public virtual ICollection<Especialidade> Especialidades { get; set; }
        public virtual Endereco Endereco { get; set; }
    }
}