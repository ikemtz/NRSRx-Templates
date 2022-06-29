using System.Diagnostics.CodeAnalysis;
using IkeMtz.NRSRx.Core.Web;
using Microsoft.Extensions.Hosting;

namespace NRSRx_OData
{
  [ExcludeFromCodeCoverage]
  public static class Program
  {
    public static void Main()
    {
      CoreWebStartup.CreateDefaultHostBuilder<Startup>().UseLogging().Build().Run();
    }
  }
}
