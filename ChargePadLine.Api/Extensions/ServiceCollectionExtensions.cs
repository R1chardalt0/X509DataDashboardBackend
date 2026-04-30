using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using X509Data.ChargePadLine.Api.Infrastructure;

using X509Data.ChargePadLine.Api.Services;

namespace X509Data.ChargePadLine.Api.Extensions
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddDBServices(this IServiceCollection services, IConfiguration configuration)
    {
      var connectionString = configuration.GetConnectionString("AppDbContext");

      Console.WriteLine($"连接字符串: {connectionString}");

      services.AddDbContext<AppDbContext>(options =>
          options.UseNpgsql(connectionString));

      return services;
    }

    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
      services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

      services.AddScoped<IChargePadLineService, ChargePadLineService>();
      services.AddScoped<ILatestDataService, LatestDataService>();

      return services;
    }
  }
}