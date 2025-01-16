﻿using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Zepto_API_Backup.Filter
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                schema.Type = "string";
                schema.Enum.Clear();
                foreach (var enumValue in Enum.GetValues(context.Type))
                {
                    schema.Enum.Add(new OpenApiString(enumValue.ToString()));
                }
            }
        }
    }
}
