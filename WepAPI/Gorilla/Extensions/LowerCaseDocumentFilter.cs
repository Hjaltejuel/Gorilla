using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Gorilla.Extensions
{
  
     public class LowerCaseDocumentFilter : IDocumentFilter
     {
         public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
         {
             swaggerDoc.Paths = swaggerDoc.Paths.ToDictionary(d => d.Key.ToLower(), d => d.Value);
         }
      }
    
}
