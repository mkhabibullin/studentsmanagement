using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace SM.API.Filters
{
    public class SwaggerSieveOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (IsMethodWithHttpGetAttribute(context))
            {
                var pageParam = operation.Parameters.FirstOrDefault(p => p.Name == "Page" && p.In == ParameterLocation.Query && string.IsNullOrEmpty(p.Description));
                if (pageParam != null)
                {
                    pageParam.Description = "is the number of page to return";
                }
                var pageSize = operation.Parameters.FirstOrDefault(p => p.Name == "PageSize" && p.In == ParameterLocation.Query && string.IsNullOrEmpty(p.Description));
                if (pageSize != null)
                {
                    pageSize.Description = "is the number of items returned per page";
                }

                var filters = operation.Parameters.FirstOrDefault(p => p.Name == "Filters" && p.In == ParameterLocation.Query && string.IsNullOrEmpty(p.Description));
                if (filters != null)
                {
                    filters.Description = "is a comma-delimited list of {Name}{Operator}{Value}";
                    filters.Examples = new Dictionary<string, OpenApiExample>
                    {
                        { "Equals", new OpenApiExample { Description = "field_name==filter_text" } },
                        { "Contains", new OpenApiExample { Description = "field_name@=filter_text" } },
                        { "Greater than", new OpenApiExample { Description = "Age>30" } },
                        { "More examples", new OpenApiExample { Description = "https://github.com/Biarity/Sieve#operators" } }
                    };
                }

                var sorts = operation.Parameters.FirstOrDefault(p => p.Name == "Sorts" && p.In == ParameterLocation.Query && string.IsNullOrEmpty(p.Description));
                if (sorts != null)
                {
                    sorts.Description = "is a comma-delimited ordered list of property names to sort by. Adding a - before the name switches to sorting descendingly.";
                    sorts.Examples = new Dictionary<string, OpenApiExample>
                    {
                        { "Sort 'created' field by descending", new OpenApiExample { Description = "-created" } },
                        { "Sort 'name' field by ascending", new OpenApiExample { Description = "name" } },
                        { "Complex sorting", new OpenApiExample { Description = "\"LikeCount,CommentCount,-created\" - sort by likes, then comments, then descendingly by date created " } }
                    };
                }
            }
        }

        private bool IsMethodWithHttpGetAttribute(OperationFilterContext context)
        {
            return context.MethodInfo.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(HttpGetAttribute));
        }
    }
}
