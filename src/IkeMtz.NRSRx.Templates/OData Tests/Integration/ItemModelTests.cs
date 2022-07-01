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
      using var srv = new TestServer(TestHostBuilder<Startup, IntegrationODataTestStartup>()
          .ConfigureTestServices(x =>
          {
            ExecuteOnContext<DatabaseContext>(x, db =>
            {
              _ = db.ItemModels.Add(itemModel);
            });
          })
       );
      var client = srv.CreateClient();
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.GetAsync($"odata/v1/{nameof(ItemModel)}s?$count=true");
      response.EnsureSuccessStatusCode();
      TestContext.WriteLine($"Server Reponse: {response}");
      var body = await response.Content.ReadAsStringAsync();
      var envelope = JsonConvert.DeserializeObject<ODataEnvelope<ItemModel>>(body);
      Assert.AreEqual(envelope?.Count, envelope?.Value.Count());
      envelope?.Value.ToList().ForEach(t =>
      {
        Assert.IsNotNull(t.Name);
        Assert.AreNotEqual(Guid.Empty, t.Id);
      });
      StringAssert.Contains(body, itemModel.Name);
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task GetGroupByItemModelsTest()
    {
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestHostBuilder<Startup, IntegrationODataTestStartup>()
          .ConfigureTestServices(x =>
          {
            ExecuteOnContext<DatabaseContext>(x, db =>
            {
              _ = db.ItemModels.Add(itemModel);
            });
          })
       );
      var client = srv.CreateClient();
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.GetAsync($"odata/v1/{nameof(ItemModel)}s?$apply=groupby(({nameof(itemModel.Name)}))");
      response.EnsureSuccessStatusCode();
      var body = await response.Content.ReadAsStringAsync();
      TestContext.WriteLine($"Server Reponse: {body}");
      Assert.IsFalse(body.ToLower().Contains("updatedby"));
      StringAssert.Contains(body, itemModel.Name);
    }
  }
}
