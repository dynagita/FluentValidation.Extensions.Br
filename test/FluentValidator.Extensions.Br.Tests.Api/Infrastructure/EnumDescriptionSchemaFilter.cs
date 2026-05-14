using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace FluentValidator.Extensions.Br.Tests.Api.Infrastructure;

public class EnumDescriptionSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum) return;

        schema.Enum.Clear();
        schema.Type = "string";
        schema.Format = null;

        foreach (var value in Enum.GetValues(context.Type))
        {
            var member = context.Type.GetMember(value.ToString()!)[0];
            var description = member.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
            schema.Enum.Add(new OpenApiString(description!));
        }
    }
}
