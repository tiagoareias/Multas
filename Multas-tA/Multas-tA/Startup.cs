using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Multas_tA.Models;
using Owin;

namespace Multas_tA {

   public partial class Startup {

      public void Configuration(IAppBuilder app) {
         ConfigureAuth(app);

         // invocar a função que vai criar ROLES + USERS
         iniciaAplicacao();
      }


      /// <summary>
      /// cria, caso não existam, as Roles de suporte à aplicação: Veterinario, Funcionario, Dono
      /// cria, nesse caso, também, um utilizador...
      /// </summary>
      private void iniciaAplicacao() {

         ApplicationDbContext db = new ApplicationDbContext();

         var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
         var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

         // criar a Role 'Agente'
         if(!roleManager.RoleExists("Agentes")) {
            // não existe a 'role'
            // então, criar essa role
            var role = new IdentityRole();
            role.Name = "Agentes";
            roleManager.Create(role);
         }

         // criar a Role 'Condutores'
         if(!roleManager.RoleExists("Condutores")) {
            // não existe a 'role'
            // então, criar essa role
            var role = new IdentityRole();
            role.Name = "Condutores";
            roleManager.Create(role);
         }



         // criar um utilizador 'Agente'
         var user = new ApplicationUser();
         user.UserName = "agente@mail.pt";
         user.Email = "agente@mail.pt";
         //  user.Nome = "Luís Freitas";
         string userPWD = "123_Asd";
         var chkUser = userManager.Create(user, userPWD);

         //Adicionar o Utilizador à respetiva Role-Agentes-
         if(chkUser.Succeeded) {
            var result1 = userManager.AddToRole(user.Id, "Agentes");
         }
      }

      // https://code.msdn.microsoft.com/ASPNET-MVC-5-Security-And-44cbdb97




   }
}
