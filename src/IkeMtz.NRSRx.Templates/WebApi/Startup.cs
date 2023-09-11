using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using IkeMtz.NRSRx.Core.Web;
using IkeMtz.NRSRx.Core.WebApi;
using NRSRx_ServiceName.Models.V1;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if (HasDb)
using Microsoft.EntityFrameworkCore;
using NRSRx_ServiceName.Data;
#endif
#if (HasEventing)
using IkeMtz.NRSRx.Events;
#endif
#if (Redis)
using StackExchange.Redis;
using IkeMtz.NRSRx.Events.Publishers.Redis;
#endif

namespace NRSRx_WebApi
{
  public class Startup : CoreWebApiStartup
  {
    public override string MicroServiceTitle => $"{nameof(NRSRx_ServiceName)} WebApi Microservice";
    public override Assembly StartupAssembly => typeof(Startup).Assembly;
    public override bool IncludeXmlCommentsInSwaggerDocs => true;
    public override string[] AdditionalAssemblyXmlDocumentFiles => new[] {
      typeof(ItemModel).Assembly.Location.Replace(".dll", ".xml", StringComparison.InvariantCultureIgnoreCase)
    };

    public Startup(IConfiguration configuration) : base(configuration) { }

#if (HasDb)
    [ExcludeFromCodeCoverage]
    public override void SetupDatabase(IServiceCollection services, string dbConnectionString)
    {
#if (MsSql)
      _ = services
       .AddDbContext<DatabaseContext>(x => x.UseSqlServer(dbConnectionString, options => options.EnableRetryOnFailure()));
#endif
#if (MySql)
      _ = services
        .AddDbContext<DatabaseContext>(x => x.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString), options => options.EnableRetryOnFailure()));
#endif
    }
#endif

#if (HasLogging)
    [ExcludeFromCodeCoverage]
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
        .AddRedis(Configuration.GetValue<string>("REDIS_CONNECTION_STRING"));
#elseif (HasDb)
        .AddDbContextCheck<DatabaseContext>();
#elseif (Redis)
        .AddRedis(Configuration.GetValue<string>("REDIS_CONNECTION_STRING"));
#endif
    }
#endif

#if (Redis)
    [ExcludeFromCodeCoverage]
    public override void SetupPublishers(IServiceCollection services)
    {
      var redisConnectionString = Configuration.GetValue<string>("REDIS_CONNECTION_STRING");
      if (!string.IsNullOrWhiteSpace(redisConnectionString))
      {
        var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
        _ = services.AddSingleton<IPublisher<ItemModel, CreatedEvent>>((x) => new RedisStreamPublisher<ItemModel, CreatedEvent>(connectionMultiplexer));
        _ = services.AddSingleton<IPublisher<ItemModel, UpdatedEvent>>((x) => new RedisStreamPublisher<ItemModel, UpdatedEvent>(connectionMultiplexer));
        _ = services.AddSingleton<IPublisher<ItemModel, DeletedEvent>>((x) => new RedisStreamPublisher<ItemModel, DeletedEvent>(connectionMultiplexer));
      }
    }
#endif
  }
}
