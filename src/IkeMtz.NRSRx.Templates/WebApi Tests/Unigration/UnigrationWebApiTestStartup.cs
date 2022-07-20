using IkeMtz.NRSRx.Core.Unigration;
using NRSRx_ServiceName.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NRSRx_ServiceName.WebApi.Tests.Unigration
{
  public class UnigrationWebApiTestStartup
      : CoreWebApiUnigrationTestStartup<Startup>
  {
    public UnigrationWebApiTestStartup(IConfiguration configuration) : base(new Startup(configuration))
    {
    }
    public override void SetupAuthentication(AuthenticationBuilder builder)
    {
      builder.SetupTestAuthentication(Configuration, TestContext);
    }
    public override void SetupDatabase(IServiceCollection services, string dbConnectionString)
    {
      services.SetupTestDbContext<DatabaseContext>();
    }
#if (HasEventing)
    public override void SetupPublishers(IServiceCollection services)
    {
    }
#endif
  }
}
