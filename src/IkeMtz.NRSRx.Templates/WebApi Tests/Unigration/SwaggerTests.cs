using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IkeMtz.NRSRx.Core.Unigration;
using IkeMtz.NRSRx.Core.Unigration.Swagger;
using IkeMtz.NRSRx.Core.Web.Swagger;
using NRSRx_ServiceName.Models.V1;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NRSRx_ServiceName.WebApi.Tests.Unigration
{
  [TestClass]
  public class SwaggerTests : BaseUnigrationTests
  {
    [TestMethod]
    [TestCategory("Unigration")]
    public async Task GetSwaggerIndexPageTest()
    {
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>());
      var html = await SwaggerUnitTests.TestHtmlPageAsync(srv);
      Assert.IsNotNull(html);
    }

    [TestMethod]
    [TestCategory("Unigration")]
    public async Task GetSwaggerJsonTest()
    {
      var myConfiguration = new Dictionary<string, string>
      {
        {ReverseProxyDocumentFilter.SwaggerReverseProxyBasePath, "/my-api"},
      };
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>()
        .ConfigureAppConfiguration((builderContext, configurationBuilder) =>
          configurationBuilder.AddInMemoryCollection(myConfiguration)
        ));
      var doc = await SwaggerUnitTests.TestJsonDocAsync(srv);
      _ = await SwaggerUnitTests.TestReverseProxyJsonDocAsync(srv, "/my-api");
      Assert.IsTrue(doc.Components.Schemas.ContainsKey(nameof(ItemModel)));
      Assert.AreEqual($"{nameof(NRSRx_ServiceName)} WebApi Microservice", doc.Info.Title);
    }
  }
}
