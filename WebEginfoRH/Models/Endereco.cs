﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEginfoRH.Models
{
    [Table("tb_Endereco")]
    public class Endereco
    {
        public Endereco()
        {
            this.Candidatos = new HashSet<Candidato>();
        }
        [Key]
        public int idEndereco { get; set; }
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public int numero { get; set; }
        public string cep { get; set; }
        public virtual ICollection<Candidato> Candidatos { get; set; }
    }
}