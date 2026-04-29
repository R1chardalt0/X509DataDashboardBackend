using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using X509Data.ChargePadLine.Api.Infrastructure;
using X509Data.ChargePadLine.Api.Services;

namespace X509Data.ChargePadLine.Api.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

      services.AddScoped<IChargePadLineService, ChargePadLineService>();

      return services;
    }
        public static IServiceCollection AddDBServices(this IServiceCollection services, IConfiguration configuration)
        {
            // 添加日志服务
            services.AddLogging(logging =>
            {
                logging.AddLog4Net()
                             .AddFilter("Microsoft.AspNetCore", LogLevel.Warning)
                             .AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None)
                             .AddFilter("Microsoft", LogLevel.Warning)
                             .AddFilter("System", LogLevel.Warning)
                             // 仅对设备追溯读写锁的日志进行抑制，避免控制台刷屏（只保留错误级别）
                             .AddFilter("Modbus.Service.Cache.DeviceTraceReadWriteLock", LogLevel.Error);
            });

            #region SWagger配置
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Modbus.Api",
                    Version = "v1",
                    Description = "Modbus.Api",
                });

                // 解决重复路由/冲突时抛出的异常（取首个描述），防止 swagger.json 500
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                //添加安全定义
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权",
                    Name = "Authorization", //默认的参数名
                    In = ParameterLocation.Header,//放于请求头中
                    Type = SecuritySchemeType.ApiKey,//类型
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                //添加安全要求
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
          {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
          });
            });
            #endregion


           
            //使用pgSql
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("AppDbContext"));
                options.UseNpgsql(s => s.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));
                //抛出sql文本
                if (configuration["ASPNETCORE_ENVIRONMENT"] == "Development")
                {
                    options.EnableSensitiveDataLogging();
                }
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.LogTo(
                        Console.WriteLine,
                        new[] { DbLoggerCategory.Database.Command.Name },
                        LogLevel.Information
                    )
                    .EnableSensitiveDataLogging()  // 显示 @p0 的实际参数值
                    .EnableDetailedErrors();
                }
            });

            
            return services;
        }
    }
}