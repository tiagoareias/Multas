using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Multas_tA
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region Formatação JSON e XML

            // Desligar o formatador do XML.
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Impedir referências circulares no JSON.
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = 
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Pretty-print do JSON
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = 
                Newtonsoft.Json.Formatting.Indented;

            // Opcional: Converter os nomes de propriedades PascalCase (a la .net)
            // para camelCase (a la Java/JavaScript).
            // Descomentar as duas seguintes linhas mudará o JSON devolvido.

            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
            //    new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            #endregion
            
            #region Routing

            // Configuração do Attribute Routing para Web API.
            // isto não fará nada se não tivermos [Route] nos nossos métodos.
            // Será explicado no futuro...
            config.MapHttpAttributeRoutes();

            // O equivalente do "MapRoute" do MVC (ver RouteConfig.cs nesta pasta).
            // Atenção que aqui é diferente, não se mete o "{action}",
            // o nome do método ajuda a determinar a operação.
            // Ou seja, no AgentesController:
            // - GetAgentes() -> GET /api/agentes -> Listar agentes.
            // - GetAgentes(int id) -> GET /api/agentes/{id} -> Obter um agente, pelo seu ID.
            // - PostAgentes(Agentes agente) -> POST /api/agentes -> Criar um agente. Os dados do agente vão no BODY.
            // - PutAgentes(int id, Agentes agente) -> PUT /api/agentes/{id} -> Update de um agente, pelo seu ID. Os dados do agente vão no BODY.
            // - DeleteAgentes(int id) -> DELETE /api/agentes/{id} -> Apaga um agente.
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            #endregion


        }
    }
}
