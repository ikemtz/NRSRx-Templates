using IkeMtz.NRSRx.Core.Unigration;
using IkeMtz.NRSRx.Core.Unigration.WebApi;
using NRSRx_ServiceName.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;

namespace NRSRx_ServiceName.WebApi.Tests.Integration
{
  public class IntegrationWebApiTestStartup
      : CoreWebApiIntegrationTestStartup<Startup>
  {
    public IntegrationWebApiTestStartup(IConfiguration configuration) : base(new Startup(configuration))
    {
    }
    public override void SetupAuthentication(AuthenticationBuilder builder)
    {
      builder.SetupTestAuthentication(Configuration, TestContext);
    }
    public override void SetupDatabase(IServiceCollection services, string dbConnectionString)
    {
      base.SetupDatabase(services, dbConnectionString);
      Startup.SetupDatabase(services, dbConnectionString);
    }
    public override void SetupPublishers(IServiceCollection services)
    {
      base.SetupPublishers(services);
      Startup.SetupPublishers(services);
    }
  }
}
