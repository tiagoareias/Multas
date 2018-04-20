using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas_tA.Models;

namespace Multas_tA.Controllers {
   public class AgentesController : Controller {

      // cria um objeto privado, que representa a base de dados
      private MultasDb db = new MultasDb();

      // GET: Agentes
      public ActionResult Index() {

         // (LINQ)db.Agente.ToList() --> em SQL: SELECT * FROM Agentes ORDER BY 
         // constroi uma lista com os dados de todos os Agentes
         // e envia-a para a View

         var listaAgentes = db.Agentes.ToList().OrderBy(a => a.Nome);

         return View(listaAgentes);
      }

      // GET: Agentes/Details/5
      /// <summary>
      /// Apresenta os detalhes de um Agente
      /// </summary>
      /// <param name="id"> representa a PK que identifica o Agente </param>
      /// <returns></returns>
      public ActionResult Details(int? id) {

         // int? - significa que pode haver valores nulos

         // protege a execução do método contra a Não existencia de dados
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }

         // vai procurar o Agente cujo ID foi fornecido
         Agentes agentes = db.Agentes.Find(id);

         // se o Agente NÃO for encontrado...
         if(agentes == null) {
            return HttpNotFound();
         }

         // envia para a View os dados do Agente
         return View(agentes);
      }





      // GET: Agentes/Create
      public ActionResult Create() {
         return View();
      }



      // POST: Agentes/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agente,
                                 HttpPostedFileBase fileUploadFotografia) {

         // determinar o ID do novo Agente
         int novoID = 0;
         // *****************************************
         // proteger a geração de um novo ID
         // *****************************************
         // determinar o nº de Agentes na tabela
         if(db.Agentes.Count() == 0) {
            novoID = 1;
         }
         else {
            novoID = db.Agentes.Max(a => a.ID) + 1;
         }
         // atribuir o ID ao novo agente
         agente.ID = novoID;
         // ***************************************************
         // outra hipótese possível seria utilizar o
         // try { }
         // catch(Exception) { }
         // ***************************************************


         // var. auxiliar
         string nomeFotografia = "Agente_" + novoID + ".jpg";
         string caminhoParaFotografia = Path.Combine(Server.MapPath("~/imagens/"), nomeFotografia); // indica onde a imagem será guardada

         // verificar se chega efetivamente um ficheiro ao servidor
         if(fileUploadFotografia != null) {
            // guardar o nome da imagem na BD
            agente.Fotografia = nomeFotografia;
         }
         else {
            // não há imagem...
            ModelState.AddModelError("", "Não foi fornecida uma imagem..."); // gera MSG de erro
            return View(agente); // reenvia os dados do 'Agente' para a View
         }

         //    verificar se o ficheiro é realmente uma imagem ---> casa
         //    redimensionar a imagem --> ver em casa

         // ModelState.IsValid --> confronta os dados fornecidos com o modelo
         // se não respeitar as regras do modelo, rejeita os dados
         if(ModelState.IsValid) {
            try {
               // adiciona na estrutura de dados, na memória do servidor,
               // o objeto Agentes
               db.Agentes.Add(agente);
               // faz 'commit' na BD
               db.SaveChanges();

               // guardar a imagem no disco rígido
               fileUploadFotografia.SaveAs(caminhoParaFotografia);

               // redireciona o utilizador para a página de início
               return RedirectToAction("Index");
            }
            catch(Exception) {
               // gerar uma mensagem de erro para o utilizador
               ModelState.AddModelError("", "Ocorreu um erro não determinado na criação do novo Agente...");
            }
         }

         // se se chegar aqui, é pq aconteceu algum problema...
         // devolve os dados do agente à View
         return View(agente);
      }

      // GET: Agentes/Edit/5
      public ActionResult Edit(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Agentes agentes = db.Agentes.Find(id);
         if(agentes == null) {
            return HttpNotFound();
         }
         return View(agentes);
      }

      // POST: Agentes/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "ID,Nome,Fotografia,Esquadra")] Agentes agentes) {
         if(ModelState.IsValid) {
            // atualiza os dados do Agente, na estrutura de dados em memória
            db.Entry(agentes).State = EntityState.Modified;
            // Commit
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         return View(agentes);
      }

      // GET: Agentes/Delete/5
      public ActionResult Delete(int? id) {
         if(id == null) {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         Agentes agentes = db.Agentes.Find(id);
         if(agentes == null) {
            return HttpNotFound();
         }
         return View(agentes);
      }

      // POST: Agentes/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteConfirmed(int id) {
         // procurar o Agente
         Agentes agentes = db.Agentes.Find(id);
         // remover da memória
         db.Agentes.Remove(agentes);
         // commit na BD
         db.SaveChanges();
         return RedirectToAction("Index");
      }

      protected override void Dispose(bool disposing) {
         if(disposing) {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
