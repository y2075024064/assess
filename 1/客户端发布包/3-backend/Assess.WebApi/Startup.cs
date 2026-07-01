using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Infrastructure.Converters;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Assess.Model.Result;
using System.Reflection;
using Newtonsoft.Json;
using Assess.Service;
using Assess.Entity;
using Serilog;

namespace Assess.WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        AppSettings.LoadConfig(e => configuration.GetValue<string>(e));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        //添加 端点API资源管理器
        services.AddEndpointsApiExplorer();

        services.AddOptions();

        services.Configure<AppSettings>(Configuration);

        services.AddControllers(options =>
        {
            //添加自定义异常过滤器
            options.Filters.Add<CustomErrorFilterAttribute>();

        }).AddNewtonsoftJson(options => //Json 配置
        {
            options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.Converters.Add(new DateTimeConverter());
            options.SerializerSettings.Converters.Add(new DateOnlyConverter());
            options.SerializerSettings.Converters.Add(new TimeOnlyConverter());
            options.SerializerSettings.Converters.Add(new EnumConverter());
            options.SerializerSettings.Formatting = Formatting.None;
        }).ConfigureApiBehaviorOptions(options =>
        {
            //自定义数据验证返回结果
            options.InvalidModelStateResponseFactory = context =>
            {
                string msg = "请求模型验证失败";
                var errors = context.ModelState
                   .Where(s => s.Value != null && s.Value.ValidationState == ModelValidationState.Invalid)
                   .SelectMany(s => s.Value!.Errors.ToList())
                   .Select(e => e.ErrorMessage)
                   .ToList();
                if (errors.Count > 0)
                {
                    msg = string.Join(" ", errors);
                }

                return new JsonResult(new RequestResult() { Code = (uint)ResultCode.BadRequest, Msg = msg });
            };
        });

        //添加分布式内存缓存
        services.AddDistributedMemoryCache();
        MySession.SetHeaderName("X-Token");

        //添加 MySql 数据库上下文服务
        var connection = Configuration.GetConnectionString("MySqlConnection");
        if (connection == null) { throw new Exception("数据库连接字符串为空！"); }
        services.AddDbContext<AssessDBContext>(options => options.UseMySQL(connection));

        //获取 哪些域名可以访问
        var allowedOrigins = Configuration.GetValue<string>("AllowedOrigins");
        string[] allowOrigins = allowedOrigins?.Split(',') ?? Array.Empty<string>();
        //添加 跨域访问 服务
        services.AddCors(options => options.AddPolicy("AllowCors", builder =>
            builder.WithOrigins(allowOrigins).AllowAnyMethod().AllowAnyHeader())
        );

        //添加 Swagger 服务
        services.AddSwaggerGen(opti =>
        {
            List<string> codes = new();
            var codeArray = Enum.GetValues(typeof(ResultCode));
            foreach (var item in codeArray) { codes.Add($"{(ushort)item}：{item}"); }

            string codeStr = string.Join(", ", codes);
            string description = "<strong>网页及请求统一编码格式：utf-8<br /><br />全局 code 说明：[ " + codeStr + " ]</strong>";
            OpenApiContact contact = new() { Email = "1440643037@qq.com", Name = "伟宇凌风", Url = new Uri("https://www.cnblogs.com/sgwy/") };

            opti.SwaggerDoc("api_v1", new OpenApiInfo
            {
                Version = "v1",
                Contact = contact,
                Description = description,
                Title = "青少年在线评测系统 API在线文档",
            });
            opti.SwaggerDoc("admin_v1", new OpenApiInfo
            {
                Version = "v1",
                Contact = contact,
                Description = description,
                Title = "青少年在线评测系统后台 API在线文档",
            });

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            opti.IncludeXmlComments(baseDirectory + "Assess.WebApi.xml", true);
            opti.IncludeXmlComments(baseDirectory + "Assess.Model.xml");

            opti.OperationFilter<AccreditOperationFilter>();
            opti.ParameterFilter<CustomParamterFilter>();

            //将 DateOnly 类型映射为 string 类型
            opti.MapType<DateOnly>(() => new OpenApiSchema() { Type = "string", Example = new Microsoft.OpenApi.Any.OpenApiString("2023-07-16") });
            //将 TimeOnly 类型映射为 string 类型
            opti.MapType<TimeOnly>(() => new OpenApiSchema() { Type = "string", Example = new Microsoft.OpenApi.Any.OpenApiString("14:15:15") });

            //将所有参数使用驼峰命名法
            opti.DescribeAllParametersInCamelCase();
            //启用注释
            opti.EnableAnnotations();
        });

        //配置使用 Newtonsoft 支持
        services.AddSwaggerGenNewtonsoftSupport();

        //添加 业务服务
        Assembly assembly = typeof(BusinessServiceBase<>).Assembly;
        var types = assembly.GetTypes().Where(e => !e.IsInterface && e.IsPublic && e.IsVisible && e.Namespace != null);
        foreach (Type type in types)
        {
            if (!type.Namespace!.EndsWith(".Services")) { continue; }
            var interfase = type.GetInterfaces().LastOrDefault();
            if (interfase != null) { _ = services.AddTransient(interfase, type); }
        }

        // If using Kestrel:
        services.Configure<KestrelServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
            options.Limits.MaxRequestBodySize = 20 * 1024 * 1024;
            options.Limits.MaxRequestBufferSize = 2 * 1024 * 1024;
        });

        // If using IIS:
        services.Configure<IISServerOptions>(options =>
        {
            options.AllowSynchronousIO = true;
            options.MaxRequestBodySize = 20 * 1024 * 1024;
            options.MaxRequestBodyBufferSize = 2 * 1024 * 1024;
        });
    }

    /// <summary>
    /// 配置HTTP请求管道
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        app.UseSerilogRequestLogging();

        bool useSwaggerUI = Configuration.GetValue<bool>("UseSwaggerUI");
        if (useSwaggerUI)
        {
            //添加 Swagger中间件
            app.UseSwagger();

            //使用 SwaggerUI 显示Swagger接口
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";

                options.SwaggerEndpoint("/swagger/api_v1/swagger.json", "Assess API v1.0");
                options.SwaggerEndpoint("/swagger/admin_v1/swagger.json", "Assess Admin API v1.0");

                options.DefaultModelsExpandDepth(-1);
            });
        }

        if (env.IsDevelopment())
        {
            //添加 开发模式异常页面显示
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHttpsRedirection();
        }

        //添加 跨域访问中间件
        app.UseCors("AllowCors");

        //静态文件查看中间件
        app.UseStaticFiles();

        //添加 反向代理中间件
        app.UseForwardedHeaders(new ForwardedHeadersOptions()
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        //添加 端点路由中间件
        app.UseRouting();

        //添加 授权中间件
        app.UseAuthorization();

        app.MapControllers();

        app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);

        WebFileTool.Init(env.WebRootPath, AppSettings.Settings.UploadAddress, AppSettings.Settings.UploadDirectory);
    }
}