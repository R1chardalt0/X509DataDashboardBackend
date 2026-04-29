using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using X509Data.ChargePadLine.Api.Entities;

namespace X509Data.ChargePadLine.Api.Infrastructure
{
  public class AppDbContext : DbContext
  {
    private readonly IHostEnvironment _env;
    public AppDbContext(DbContextOptions<AppDbContext> options, IHostEnvironment env) : base(options)
    {
      this._env = env;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }

    public DbSet<DataList> DataLists { get; set; }
    public DbSet<StationList> StationLists { get; set; }
  }
}