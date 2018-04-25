using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Multas_tA.Models
{
    public class MultasDb : DbContext
    {

        // construtor da classe
        public MultasDb() : base("MultasDbConnectionString")
        { }

        // identificar as tabelas da base de dados
        public virtual DbSet<Multas> Multas { get; set; }
        public virtual DbSet<Condutores> Condutores { get; set; }
        public virtual DbSet<Viaturas> Viaturas { get; set; }
        public virtual DbSet<Agentes> Agentes { get; set; }

        /// <summary>
        /// Usa a sequência definida em <see cref="Multas_tA.Migrations.SequenciaIdAgentes"/>
        /// para obter, de forma atómica, o ID de um agente.
        /// </summary>
        /// <returns>O próximo ID do agente.</returns>
        public int GetIdAgente()
        {
            // Um objeto que derive da classe "DbContext" (como o MultasDb)
            // permite que seja executado SQL "raw", como no exemplo abaixo.
            return this.Database
                // <int> define o tipo de dados. Pode ser uma classe, os valores dos campos
                // do SELECT serão copiados para o objeto.
                .SqlQuery<int>("Select Next Value For [dbo].[SeqIdAgente]")
                // Single() é um operador do Linq. 
                // Uso este porque só me interessa a primeira (e única) linha.
                // Usaria ToList() se existissem várias, e First()/Last() se só quisesse
                // a primera/última linha de muitas.
                .Single(); 
        }

        /// <summary>
        /// configura a forma como as tabelas são criadas
        /// </summary>
        /// <param name="modelBuilder"> objeto que referencia o gerador de base de dados </param>      
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

    }
}