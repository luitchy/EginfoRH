namespace WebEginfoRH.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModeloDados : DbContext
    {
        public ModeloDados()
            : base("name=ModeloDados")
        {
        }

        public virtual DbSet<tb_Candidato> tb_Candidato { get; set; }
      //  public virtual DbSet<tb_Detalhe_Candidato> tb_Detalhe_Candidato { get; set; }
        public virtual DbSet<tb_Especialidade> tb_Especialidade { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tb_Especialidade>()
                .Property(e => e.nome)
                .IsUnicode(false);
        }
    }
}
