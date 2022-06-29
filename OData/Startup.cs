using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using IkeMtz.NRSRx.Core.OData;
using IkeMtz.NRSRx.Core.Web;
using NRSRx_OData.Models.V1;
using NRSRx_OData.Configuration;
using NRSRx_OData.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NRSRx_OData
{
  public class Startup : CoreODataStartup
  {
    public override string MicroServiceTitle => $"{nameof(NRSRx_OData)} OData Microservice";
    public override Assembly StartupAssembly => typeof(Startup).Assembly;
    public override bool IncludeXmlCommentsInSwaggerDocs => true;
    public override string[] AdditionalAssemblyXmlDocumentFiles => new[] {
      typeof(Item).Assembly.Location.Replace(".dll", ".xml", StringComparison.InvariantCultureIgnoreCase)
    };

    public override BaseODataModelProvider ODataModelProvider => new ODataModelProvider();

    public Startup(IConfiguration configuration) : base(configuration)
    {
    }

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
  }
}
