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
using System.Net;
using Microsoft.EntityFrameworkCore;
#endif
#if (HasEventing)
using IkeMtz.NRSRx.Events;
using IkeMtz.NRSRx.Core.Unigration.Events;
using Moq;
#endif

namespace NRSRx_ServiceName.WebApi.Tests.Unigration
{
  [TestClass]
  public partial class ItemModelsTests : BaseUnigrationTests
  {
#if (HasDb)
    [TestMethod]
    [TestCategory("Unigration")]
    public async Task GetItemModelsTest()
    {
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>()
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
#if (HasDb)
    [TestMethod]
    [TestCategory("Unigration")]
    public async Task GetItemModelsNotFoundTest()
    {
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>());
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.GetAsync($"api/v1/{nameof(ItemModel)}s.json?id={itemModel.Id}");
      Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
#endif

    [TestMethod]
    [TestCategory("Unigration")]
    public async Task PostItemModelsTest()
    {
#if (Redis)
      var mockPublisher = MockRedisStreamFactory<ItemModel, CreatedEvent>.CreatePublisher();
#endif
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>()
        .ConfigureTestServices(x =>
        {
#if (Redis)
          _ = x.AddSingleton(mockPublisher.Object);
#endif
        }));
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
      Assert.AreEqual(httpItemModel.CreatedOnUtc, dbItemModel.CreatedOnUtc);
#endif
#if (Redis)
      mockPublisher.Verify(t => t.PublishAsync(It.Is<ItemModel>(t => t.Id == httpItemModel.Id)), Times.Once);
#endif
    }

    [TestMethod]
    [TestCategory("Unigration")]
    public async Task PutItemModelTest()
    {
#if (Redis)
      var mockPublisher = MockRedisStreamFactory<ItemModel, UpdatedEvent>.CreatePublisher();
#endif
      var originalItemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>()
        .ConfigureTestServices(x =>
        {
#if (HasDb)
          ExecuteOnContext<DatabaseContext>(x, db =>
          {
            _ = db.ItemModels.Add(originalItemModel);
          });
#endif
#if (Redis)
          _ = x.AddSingleton(mockPublisher.Object);
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
      var dbUpdatedItemModel = await dbContext.ItemModels.FirstOrDefaultAsync(t => t.Id == originalItemModel.Id);

      Assert.IsNotNull(dbUpdatedItemModel);
      Assert.AreEqual("IntegrationTester@email.com", dbUpdatedItemModel.UpdatedBy);
      Assert.IsNotNull(dbUpdatedItemModel.UpdatedOnUtc);
      Assert.AreEqual(httpItemModel.UpdatedOnUtc, dbUpdatedItemModel.UpdatedOnUtc);
#endif
#if (Redis)
      mockPublisher.Verify(t => t.PublishAsync(It.Is<ItemModel>(t => t.Id == originalItemModel.Id)), Times.Once);
#endif
    }

    [TestMethod]
    [TestCategory("Unigration")]
    [ExpectedException(typeof(HttpRequestException))]
    public async Task PutItemModelIdConflictTest()
    {
#if (Redis)
      var mockPublisher = MockRedisStreamFactory<ItemModel, UpdatedEvent>.CreatePublisher();
#endif
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>()
        .ConfigureTestServices(x =>
        {
#if (Redis)
          _ = x.AddSingleton(mockPublisher.Object);
#endif
        }));
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.PutAsJsonAsync($"api/v1/{nameof(ItemModel)}s.json?id={Guid.NewGuid()}", itemModel);
      _ = response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    [TestCategory("Unigration")]
    [ExpectedException(typeof(HttpRequestException))]
    public async Task PutItemModelNotFoundTest()
    {
#if (Redis)
      var mockPublisher = MockRedisStreamFactory<ItemModel, UpdatedEvent>.CreatePublisher();
#endif
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>()
        .ConfigureTestServices(x =>
        {
#if (Redis)
          _ = x.AddSingleton(mockPublisher.Object);
#endif
        }));
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.PutAsJsonAsync($"api/v1/{nameof(ItemModel)}s.json?id={itemModel.Id}", itemModel);
      _ = response.EnsureSuccessStatusCode();
    }

    [TestMethod]
    [TestCategory("Unigration")]
    public async Task DeleteItemModelTest()
    {
#if (Redis)
      var mockPublisher = MockRedisStreamFactory<ItemModel, DeletedEvent>.CreatePublisher();
#endif
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>()
        .ConfigureTestServices(x =>
        {
#if (HasDb)
          ExecuteOnContext<DatabaseContext>(x, db =>
          {
            _ = db.ItemModels.Add(itemModel);
          });
#endif
#if (Redis)
          _ = x.AddSingleton(mockPublisher.Object);
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
#if (Redis)
      mockPublisher.Verify(t => t.PublishAsync(It.Is<ItemModel>(t => t.Id == itemModel.Id)), Times.Once);
#endif
    }

#if (HasDb)
    [TestMethod]
    [TestCategory("Unigration")]
    public async Task DeleteItemModelsNotFoundTest()
    {
#if (Redis)
      var mockPublisher = MockRedisStreamFactory<ItemModel, DeletedEvent>.CreatePublisher();
#endif
      var itemModel = Factories.ItemModelFactory();
      using var srv = new TestServer(TestWebHostBuilder<Startup, UnigrationWebApiTestStartup>()
        .ConfigureTestServices(x => {
#if (Redis)
          _ = x.AddSingleton(mockPublisher.Object);
#endif
        }));
      var client = srv.CreateClient(TestContext);
      GenerateAuthHeader(client, GenerateTestToken());

      var response = await client.DeleteAsync($"api/v1/{nameof(ItemModel)}s.json?id={itemModel.Id}");
      Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }
#endif
  }
}
