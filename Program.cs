using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using X509Data.ChargePadLine.Api.Extensions;
using X509Data.ChargePadLine.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var config = builder.Configuration;
// 读取 appsettings.json 中的 Urls 配置
var urls = builder.Configuration["Urls"] ?? "http://0.0.0.0:5000";
// 设置应用监听的 URL（覆盖默认配置）
builder.WebHost.UseUrls(urls);


ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

InitializeDatabaseAndStartServices(app.Services);

// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowClient"); // 新增：启用 CORS


var logger = app.Services.GetRequiredService<ILogger<Program>>();

// 先映射控制器（包括 StaticFilesController），确保控制器路由优先
app.MapControllers();

app.UseHttpsRedirection();
app.UseAuthentication(); // 在UseAuthorization前添加
app.UseAuthorization();
app.Run();

#region 配置服务
void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{

    // 添加控制器
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    // 豆包AI代理调用需要 HttpClient 工厂
    services.AddHttpClient();
    // 注册数据库和业务服务
    services.AddDBServices(configuration);
    services.AddBusinessServices(configuration);

    // 添加 CORS 策略（开发环境放开，生产环境建议限定具体域名）
    services.AddCors(options =>
    {
        options.AddPolicy("AllowClient", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });
}
#endregion

#region 数据库服务
void InitializeDatabaseAndStartServices(IServiceProvider services)
{
    using (var scope = services.CreateScope())
    {
        var scopedServices = scope.ServiceProvider;
        try
        {
            // 初始化AppDbContext数据库
            var dbContext = scopedServices.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();

            var logger = scopedServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("数据库创建成功");
        }
        catch (Exception ex)
        {
            var logger = scopedServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "初始化过程中发生错误");
            throw;
        }
    }
}
#endregion