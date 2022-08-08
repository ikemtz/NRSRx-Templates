using IkeMtz.NRSRx.Events;
using System.Diagnostics.CodeAnalysis;
#if (Redis)
using IkeMtz.NRSRx.Events.Publishers.Redis;
using StackExchange.Redis;
#endif
using NRSRx_ServiceName.Models.V1;

namespace NRSRx_WebApi.Publishers
{
#if (Redis)
  [ExcludeFromCodeCoverage]
  public class ItemModelCreatedPublisher : RedisStreamPublisher<ItemModel, CreatedEvent>,
    ISimplePublisher<ItemModel, CreatedEvent, RedisValue>
  {
    public ItemModelCreatedPublisher(IConnectionMultiplexer connection) : base(connection)
    {
    }
  }
  [ExcludeFromCodeCoverage]
  public class ItemModelDeletedPublisher : RedisStreamPublisher<ItemModel, DeletedEvent>,
    ISimplePublisher<ItemModel, DeletedEvent, RedisValue>
  {
    public ItemModelDeletedPublisher(IConnectionMultiplexer connection) : base(connection)
    {
    }
  }
  [ExcludeFromCodeCoverage]
  public class ItemModelUpdatedPublisher : RedisStreamPublisher<ItemModel, UpdatedEvent>,
    ISimplePublisher<ItemModel, UpdatedEvent, RedisValue>
  {
    public ItemModelUpdatedPublisher(IConnectionMultiplexer connection) : base(connection)
    {
    }
  }
#endif
}
