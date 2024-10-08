using System;
using System.Linq;
using System.Threading.Tasks;
using IkeMtz.NRSRx.Core.Models;
using IkeMtz.NRSRx.Core.Unigration;
using NRSRx_ServiceName.Models.V1;
using NRSRx_ServiceName.Data;
using NRSRx_ServiceName.Tests;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using static IkeMtz.NRSRx.Core.Unigration.TestDataFactory;
#if (HasDb)
using Microsoft.EntityFrameworkCore;
#endif
#if (HasEventing)
using IkeMtz.NRSRx.Events;
using IkeMtz.NRSRx.Core.Unigration.Events;
using Moq;
#endif

namespace NRSRx_ServiceName.WebApi.Tests.Integration
{
  [TestClass]
  public partial class ItemModelsTests : BaseUnigrationTests
  {
#if (HasDb)
    [TestMethod]
    [TestCategory("Integration")]
    public async Task GetItemModelsTest()
    {
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, IntegrationWebApiTestStartup>()
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

      var response = await client.GetAsync($"api/v1/{nameof(ItemModel)}s.json?id={itemModel.Id}");
      var result = await DeserializeResponseAsync<ItemModel>(response);
      Assert.IsNotNull(result);
      _ = response.EnsureSuccessStatusCode();
      Assert.AreEqual(itemModel.Name, result?.Name);
    }
#endif

    [TestMethod]
    [TestCategory("Integration")]
    public async Task PostItemModelsTest()
    {
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, IntegrationWebApiTestStartup>());
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.PostAsJsonAsync($"api/v1/{nameof(ItemModel)}s.json", itemModel);
      var httpItemModel = await DeserializeResponseAsync<ItemModel>(response);
      Assert.IsNotNull(httpItemModel);
      _ = response.EnsureSuccessStatusCode();
      Assert.AreEqual("IntegrationTester@email.com", httpItemModel.CreatedBy);
#if (HasDb)
      var dbContext = srv.GetDbContext<DatabaseContext>();
      var dbItemModel = await dbContext.ItemModels.FirstOrDefaultAsync(t => t.Id == httpItemModel.Id);

      Assert.IsNotNull(dbItemModel);
      Assert.AreEqual(httpItemModel.CreatedOnUtc.ToString(), dbItemModel.CreatedOnUtc.ToString());
#endif
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task PutItemModelTest()
    {
      var originalItemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, IntegrationWebApiTestStartup>()
        .ConfigureTestServices(x =>
        {
#if (HasDb)
          ExecuteOnContext<DatabaseContext>(x, db =>
          {
            _ = db.ItemModels.Add(originalItemModel);
          });
#endif
        }));
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var updatedItemModel = JsonClone(originalItemModel);
      updatedItemModel.Name = StringGenerator(100, true, CharacterSets.AlphaNumericChars);

      var response = await client.PutAsJsonAsync($"api/v1/{nameof(ItemModel)}s.json?id={updatedItemModel.Id}", updatedItemModel);
      var httpItemModel = await DeserializeResponseAsync<ItemModel>(response);
      Assert.IsNotNull(httpItemModel);
      _ = response.EnsureSuccessStatusCode();
      Assert.AreEqual("IntegrationTester@email.com", httpItemModel.UpdatedBy);
      Assert.AreEqual(updatedItemModel.Name, httpItemModel.Name);
      Assert.IsNull(updatedItemModel.UpdatedOnUtc);
      Assert.IsNotNull(httpItemModel.UpdatedOnUtc);
#if (HasDb)
      var dbContext = srv.GetDbContext<DatabaseContext>();
      var dbItemModel = await dbContext.ItemModels.FirstOrDefaultAsync(t => t.Id == originalItemModel.Id);

      Assert.IsNotNull(dbItemModel);
      Assert.AreEqual("IntegrationTester@email.com", dbItemModel.UpdatedBy);
      Assert.IsNotNull(dbItemModel.UpdatedOnUtc);
      Assert.AreEqual(httpItemModel.UpdatedOnUtc.ToString(), dbItemModel.UpdatedOnUtc.ToString());
#endif
    }

    [TestMethod]
    [TestCategory("Integration")]
    public async Task DeleteItemModelTest()
    {
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, IntegrationWebApiTestStartup>()
        .ConfigureTestServices(x =>
        {
#if (HasDb)
          ExecuteOnContext<DatabaseContext>(x, db =>
          {
            _ = db.ItemModels.Add(itemModel);
          });
#endif
        }));
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.DeleteAsync($"api/v1/{nameof(ItemModel)}s.json?id={itemModel.Id}");
      var httpItemModel = await DeserializeResponseAsync<ItemModel>(response);
      Assert.IsNull(httpItemModel);
      _ = response.EnsureSuccessStatusCode();
#if (HasDb)
      var dbContext = srv.GetDbContext<DatabaseContext>();
      var dbItemModel = await dbContext.ItemModels.FirstOrDefaultAsync(t => t.Id == itemModel.Id);

      Assert.IsNull(dbItemModel);
#endif
    }
  }
}
