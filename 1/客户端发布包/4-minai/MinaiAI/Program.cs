using MinaiAI.Services;

var builder = WebApplication.CreateBuilder(args);

// 添加控制器（使用 PascalCase 序列化，匹配前端）
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// 添加 CORS（允许前端跨域调用）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 注册米奈本地AI服务
builder.Services.AddSingleton<DeepSeekService>();

// 配置监听端口
builder.WebHost.UseUrls("http://localhost:5188");

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();

Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
Console.WriteLine("  🐼 米奈 AI 服务已启动");
Console.WriteLine($"  📡 地址: http://localhost:5188");
Console.WriteLine($"  💬 API:  http://localhost:5188/api/Chat/Send");
Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

app.Run();
