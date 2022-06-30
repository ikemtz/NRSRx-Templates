using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using IkeMtz.NRSRx.Core.OData;
using IkeMtz.NRSRx.Core.Web;
using NRSRx_ServiceName.Models.V1;
using NRSRx_ServiceName.Data;
using NRSRx_OData.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NRSRx_OData
{
  public class Startup : CoreODataStartup
  {
    public override string MicroServiceTitle => $"{nameof(NRSRx_ServiceName)} OData Microservice";
    public override Assembly StartupAssembly => typeof(Startup).Assembly;
    public override bool IncludeXmlCommentsInSwaggerDocs => true;
    public override string[] AdditionalAssemblyXmlDocumentFiles => new[] {
      typeof(Item).Assembly.Location.Replace(".dll", ".xml", StringComparison.InvariantCultureIgnoreCase)
    };

    public override BaseODataModelProvider ODataModelProvider => new ODataModelProvider();

    public Startup(IConfiguration configuration) : base(configuration)
    {
    }
#if (HasLogging)
    public override void SetupLogging(IServiceCollection? services = null, IApplicationBuilder? app = null)
    {
#if (ApplicationInsights)
      this.SetupApplicationInsights(services);
#endif
#if (Elasticsearch)
      this.SetupElasticsearch(app);
#endif
#if (Splunk)
      this.SetupSplunk(app);
#endif
    }
#endif

    [ExcludeFromCodeCoverage]
    public override void SetupDatabase(IServiceCollection services, string dbConnectionString)
    {
#if (MsSql)
      _ = services
       .AddDbContextPool<DatabaseContext>(x => x.UseSqlServer(dbConnectionString));
#endif
#if (MySql)
      _ = services
        .AddDbContext<DatabaseContext>(x => x.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString)));
#endif
    }

    public override void SetupHealthChecks(IServiceCollection services, IHealthChecksBuilder healthChecks)
    {
      _ = healthChecks.AddDbContextCheck<DatabaseContext>();
    }
  }
}
