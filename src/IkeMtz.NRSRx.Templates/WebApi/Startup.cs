using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using IkeMtz.NRSRx.Core.Web;
using IkeMtz.NRSRx.Core.WebApi;
using NRSRx_WebApi.Models.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if (HasDb)
using Microsoft.EntityFrameworkCore;
using NRSRx_WebApi.Data;
#endif
#if (HasEventing)
using IkeMtz.NRSRx.Events;
using NRSRx_WebApi.Publishers;
#endif
#if (Redis)
using StackExchange.Redis;
#endif

namespace NRSRx_WebApi
{
  public class Startup : CoreWebApiStartup
  {
    public override string MicroServiceTitle => $"{nameof(NRSRx_WebApi)} WebApi Microservice";
    public override Assembly StartupAssembly => typeof(Startup).Assembly;
    public override bool IncludeXmlCommentsInSwaggerDocs => true;
    public override string[] AdditionalAssemblyXmlDocumentFiles => new[] {
      typeof(Item).Assembly.Location.Replace(".dll", ".xml", StringComparison.InvariantCultureIgnoreCase)
    };

    public Startup(IConfiguration configuration) : base(configuration) { }

#if (HasDb)
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
#endif

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

#if (HasDb || HasEventing)
    public override void SetupHealthChecks(IServiceCollection services, IHealthChecksBuilder healthChecks)
    {
      _ = healthChecks
#if (HasDb && Redis)
        .AddDbContextCheck<DatabaseContext>()
        .AddRedis<DatabaseContext>(Configuration.GetValue<string>("REDIS_CONNECTION_STRING"));
#elseif (HasDb)
        .AddDbContextCheck<DatabaseContext>();
#elseif (Redis)
        .AddRedis<DatabaseContext>(Configuration.GetValue<string>("REDIS_CONNECTION_STRING"));
#endif
    }
#endif

#if (Redis)
    [ExcludeFromCodeCoverage]
    public override void SetupPublishers(IServiceCollection services)
    {
      var redisConnectionString = Configuration.GetValue<string>("REDIS_CONNECTION_STRING");
      var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
      _ = services.AddSingleton<ISimplePublisher<Item, CreatedEvent, RedisValue>>((x) => new ItemCreatedPublisher(connectionMultiplexer));
      _ = services.AddSingleton<ISimplePublisher<Item, UpdatedEvent, RedisValue>>((x) => new ItemUpdatedPublisher(connectionMultiplexer));
      _ = services.AddSingleton<ISimplePublisher<Item, DeletedEvent, RedisValue>>((x) => new ItemDeletedPublisher(connectionMultiplexer));
    }
#endif
  }
}
