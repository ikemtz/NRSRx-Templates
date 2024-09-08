using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IkeMtz.NRSRx.Core.Models;
using IkeMtz.NRSRx.Core.Unigration;
using NRSRx_ServiceName.Models.V1;
using NRSRx_ServiceName.Data;
using NRSRx_ServiceName.Tests;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace NRSRx_ServiceName.OData.Tests.Integration
{
  [TestClass]
  public partial class ItemModelsTests : BaseUnigrationTests
  {
    [TestMethod]
    [TestCategory("Integration")]
    public async Task GetItemModelsTest()
    {
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, IntegrationODataTestStartup>()
          .ConfigureTestServices(x =>
          {
            ExecuteOnContext<DatabaseContext>(x, db =>
            {
              _ = db.ItemModels.Add(itemModel);
            });
          })
       );
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.GetAsync($"odata/v1/{nameof(ItemModel)}s?$count=true");
      var envelope = await DeserializeResponseAsync<ODataEnvelope<ItemModel>>(response);
      Assert.IsNotNull(envelope);
      response.EnsureSuccessStatusCode();
      Assert.AreEqual(envelope?.Count, envelope?.Value.Count());
      envelope?.Value.ToList().ForEach(t =>
      {
        Assert.IsNotNull(t.Name);
        Assert.AreNotEqual(Guid.Empty, t.Id);
      });
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task GetGroupByItemModelsTest()
    {
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, IntegrationODataTestStartup>()
          .ConfigureTestServices(x =>
          {
            ExecuteOnContext<DatabaseContext>(x, db =>
            {
              _ = db.ItemModels.Add(itemModel);
            });
          })
       );
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.GetAsync($"odata/v1/{nameof(ItemModel)}s?$apply=groupby(({nameof(itemModel.Name)}))");
      var envelope = await DeserializeResponseAsync<ODataEnvelope<ItemModel>>(response);
      Assert.IsNotNull(envelope);
      response.EnsureSuccessStatusCode();
    }
  }
}
