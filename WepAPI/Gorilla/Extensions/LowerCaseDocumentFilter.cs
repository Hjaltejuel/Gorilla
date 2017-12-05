using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WepAPI.Models
{
  
     public class LowerCaseDocumentFilter : IDocumentFilter
     {
         public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
         {
             swaggerDoc.Paths = swaggerDoc.Paths.ToDictionary(d => d.Key.ToLower(), d => d.Value);
         }
      }
    
}
