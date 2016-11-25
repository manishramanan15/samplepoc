using System.Linq;
using System.Web.Http;

using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;

//using System.Web.OData;
//using System.Web.OData.Builder;
//using System.Web.OData.Extensions;

using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using Microsoft.OData.Edm;

using HLF.ERP.Service.Database;
using HLF.ERP.Services.Models;


namespace HLF.ERP.Service
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            //register odata routes
            RegisterOdata(config);

            //config.EnableUnqualifiedNameCall(true);
            

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void RegisterOdata(HttpConfiguration config)
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Product>("Products");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<Order_Detail>("Order_Details");
            builder.EntitySet<Supplier>("Suppliers");
            builder.EntitySet<Region>("Regions");
            builder.EntitySet<Territory>("Territories");

            //builder.Namespace = "ProductService";

            //builder.EntityType<Product>().Collection
            //    .Function("MostExpensive")
            //    .Returns<ProductDetail>();

            //builder.ComplexType<ProductDetail>();


            config.Routes.MapODataServiceRoute(routeName:"odata", routePrefix:"odata", model: builder.GetEdmModel());
        }
    }
}
