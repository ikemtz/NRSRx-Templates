using System.Diagnostics.CodeAnalysis;
using IkeMtz.NRSRx.Core.Web;
using Microsoft.Extensions.Hosting;

namespace NRSRx_WebApi
{
  [ExcludeFromCodeCoverage]
  public static class Program
  {
    public static void Main()
    {
#if (HasLogging)
      CoreWebStartup.CreateDefaultHostBuilder<Startup>().UseLogging().Build().Run();
#else
      CoreWebStartup.CreateDefaultHostBuilder<Startup>().Build().Run();
#endif
    }
  }
}
