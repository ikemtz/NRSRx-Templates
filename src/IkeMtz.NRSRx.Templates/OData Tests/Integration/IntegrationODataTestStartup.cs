using IkeMtz.NRSRx.Core.Unigration;
using IkeMtz.NRSRx.Core.Unigration.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NRSRx_ServiceName.Data;

namespace NRSRx_ServiceName.OData.Tests.Integration
{
  public class IntegrationODataTestStartup
      : CoreODataIntegrationTestStartup<Startup>
  {
    public IntegrationODataTestStartup(IConfiguration configuration)
        : base(new Startup(configuration))
    {
    }
    public override void SetupAuthentication(AuthenticationBuilder builder)
    {
      builder.SetupTestAuthentication(Configuration, TestContext);
    }
    public override void SetupDatabase(IServiceCollection services, string dbConnectionString)
    {
      var serviceProvider = services.BuildServiceProvider();
#if (MsSql)
       _ = services
        .AddDbContextPool<DatabaseContext>(x =>
        {
          x.UseSqlServer(dbConnectionString);
          x.AddInterceptors(new AuditableTestInterceptor(serviceProvider.GetService<IHttpContextAccessor>()));
        });
#endif
#if (MySql)
       _ = services
        .AddDbContext<DatabaseContext>(x =>
        {
          x.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString));
          x.AddInterceptors(new AuditableTestInterceptor(serviceProvider.GetService<IHttpContextAccessor>()));
        });
#endif

    }
  }
}
