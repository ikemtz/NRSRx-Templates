using IkeMtz.NRSRx.Core.Unigration;
using IkeMtz.NRSRx.Core.Unigration.WebApi;
using NRSRx_ServiceName.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NRSRx_ServiceName.WebApi.Tests.Integration
{
  public class IntegrationWebApiTestStartup
      : CoreWebApiIntegrationTestStartup<Startup>
  {
    public IntegrationWebApiTestStartup(IConfiguration configuration) : base(new Startup(configuration))
    {
    }
  }
}
