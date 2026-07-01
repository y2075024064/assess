using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Assess.WebApi;

/// <summary>
/// 自定义Swagger参数过滤器
/// </summary>
public class CustomParamterFilter : IParameterFilter
{
    private readonly Type enumType = typeof(Enum);

    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        Type type = context.ParameterInfo.ParameterType;
        if (type.IsAssignableTo(enumType))
        {
            string name = type.Name;
            if (type.IsGenericType)
            {
                type = type.GenericTypeArguments[0];
                name = type.Name;
            }

            var schema = context.SchemaRepository.Schemas.GetValueOrDefault(name);
            schema ??= context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);

            parameter.Schema = schema;
        }
    }
}
