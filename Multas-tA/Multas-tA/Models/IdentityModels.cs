﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Multas_tA.Models {
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.

    /// <summary>
    /// classe responsável por gerar 'UTILIZADORES'
    /// </summary>
    public class ApplicationUser : IdentityUser {
        //criar tributos que serao explicitos na minha aplicação
        //vao ser criados na tabela aspnetusers
        public string NomeProprio { get; set; }

        public string Apelido { get; set; }

        public  DateTime? DataNascimento {get;set;}

      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {
         // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
         var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
         // Add custom user claims here
         return userIdentity;
      }
   }


   /// <summary>
   /// classe responsável por fazer a 'gestão' da base de dados da Autenticação
   /// mais,
   /// vamos fazer a migração da nossa BD para esta classe
   /// </summary>
   public class ApplicationDbContext : IdentityDbContext<ApplicationUser> {
      public ApplicationDbContext()
          : base("MultasDbConnectionString", throwIfV1Schema: false) {
      }

      static ApplicationDbContext() {
         // Set the database intializer which is run once during application start
         // This seeds the database with admin user credentials and admin role
         Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
      }

      public static ApplicationDbContext Create() {
         return new ApplicationDbContext();
      }

      // juntar o código da nossa anterior base de dados
      // identificar as tabelas da base de dados
      public virtual DbSet<Multas> Multas { get; set; }
      public virtual DbSet<Condutores> Condutores { get; set; }
      public virtual DbSet<Viaturas> Viaturas { get; set; }
      public virtual DbSet<Agentes> Agentes { get; set; }

      /// <summary>
      /// configura a forma como as tabelas são criadas
      /// </summary>
      /// <param name="modelBuilder"> objeto que referencia o gerador de base de dados </param>      
      protected override void OnModelCreating(DbModelBuilder modelBuilder) {

         modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
         modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

         base.OnModelCreating(modelBuilder);
      }






   }
}