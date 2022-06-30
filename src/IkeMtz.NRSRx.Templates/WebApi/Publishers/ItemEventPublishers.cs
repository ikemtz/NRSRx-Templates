using IkeMtz.NRSRx.Events;
#if (Redis)
using IkeMtz.NRSRx.Events.Publishers.Redis;
using StackExchange.Redis;
#endif
using NRSRx_WebApi.Models.V1;

namespace NRSRx_WebApi.Publishers
{
  public class ItemCreatedPublisher : RedisStreamPublisher<Item, CreatedEvent>,
    ISimplePublisher<Item, CreatedEvent, RedisValue>
  {
    public ItemCreatedPublisher(IConnectionMultiplexer connection) : base(connection)
    {
    }
  }
  public class ItemDeletedPublisher : RedisStreamPublisher<Item, DeletedEvent>,
    ISimplePublisher<Item, DeletedEvent, RedisValue>
  {
    public ItemDeletedPublisher(IConnectionMultiplexer connection) : base(connection)
    {
    }
  }
  public class ItemUpdatedPublisher : RedisStreamPublisher<Item, UpdatedEvent>,
    ISimplePublisher<Item, UpdatedEvent, RedisValue>
  {
    public ItemUpdatedPublisher(IConnectionMultiplexer connection) : base(connection)
    {
    }
  }
}
