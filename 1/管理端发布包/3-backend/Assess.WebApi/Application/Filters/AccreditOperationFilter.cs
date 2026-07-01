using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Assess.WebApi;

/// <summary>
/// Swagger授权验证过滤器
/// </summary>
public class AccreditOperationFilter : IOperationFilter
{
    private readonly Type type = typeof(ClientType);

    /// <summary>
    /// 应用过滤规则
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.ApiDescription.RelativePath != "Home/GetToken")
        {
            OpenApiParameter parameter1 = new()
            {
                Required = true,
                Name = "X-Token",
                Description = "访问凭证",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" }
            };

            var schema = context.SchemaRepository.Schemas.GetValueOrDefault(type.Name);
            if (schema == null)
            {
                _ = context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
                schema = context.SchemaRepository.Schemas.GetValueOrDefault(type.Name);
            }

            OpenApiParameter parameter2 = new()
            {
                Required = true,
                Schema = schema,
                Name = "X-ClientType",
                Description = "客户端类型",
                In = ParameterLocation.Header,
            };
            //operation.Parameters.Insert(0, parameter1);
            //operation.Parameters.Insert(1, parameter2);
        }
    }
}