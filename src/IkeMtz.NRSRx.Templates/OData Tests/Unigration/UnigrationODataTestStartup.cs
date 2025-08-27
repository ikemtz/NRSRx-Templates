using IkeMtz.NRSRx.Core.Unigration;
using NRSRx_ServiceName.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NRSRx_ServiceName.OData.Tests.Unigration
{
  public class UnigrationODataTestStartup(IConfiguration configuration)
            : CoreODataUnigrationTestStartup<Startup>(new Startup(configuration))
  {
    public override void SetupDatabase(IServiceCollection services, string dbConnectionString)
    {
      services.SetupTestDbContext<DatabaseContext>();
    }
  }
}
