using System;
using System.Threading.Tasks;
using IkeMtz.NRSRx.Core.Models;
using IkeMtz.NRSRx.Core.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
#if (HasDb)
using Microsoft.EntityFrameworkCore;
using NRSRx_ServiceName.Data;
#endif
#if (HasEventing)
using IkeMtz.NRSRx.Events;
#endif
#if (Redis)
using IkeMtz.NRSRx.Events.Publishers.Redis;
#endif
#if (HasLogging)
using Microsoft.Extensions.Logging;
#endif
using NRSRx_ServiceName.Models.V1;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NRSRx_WebApi.Controllers.V1
{
  [Route("api/v{version:apiVersion}/[controller].{format}"), FormatFilter]
  [ApiVersion(VersionDefinitions.v1_0)]
  [ApiController]
  [Authorize]
  public class ItemsController : ControllerBase
  {
#if (HasLogging)
    private readonly ILogger<ItemsController> _logger;
#endif
#if (HasDb)
    private readonly DatabaseContext _databaseContext;
#endif
#if (HasDb && HasLogging)
    public ItemsController(DatabaseContext databaseContext, ILogger<ItemsController> logger)
    {
      _databaseContext = databaseContext;
      _logger = logger;
    }
#elseif (HasDb)
    public ItemsController(DatabaseContext databaseContext)
    {
      _databaseContext = databaseContext;
    }
#elseif (HasLogging)
    public ItemsController(ILogger<ItemsController> logger)
    {
      _logger = logger;
    }
#else
    public ItemsController()
    {
    }
#endif

    // Get api/Items
    [HttpGet]
    [ProducesResponseType(Status200OK, Type = typeof(Item))]
    public async Task<ActionResult> Get([FromQuery] Guid id)
    {
#if (HasDb)
      var obj = await _databaseContext.Items
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      return Ok(obj);
#else
      return Ok();
#endif
    }

    // Post api/Items
    [HttpPost]
    [ProducesResponseType(Status200OK, Type = typeof(Item))]
    [ValidateModel]
#if (Redis)
    public async Task<ActionResult> Post([FromBody] Item value, [FromServices] RedisStreamPublisher<Item, CreatedEvent> publisher)
#else
    public async Task<ActionResult> Post([FromBody] Item value)
#endif
    {
#if (HasDb && HasEventing)
      var dbContextObject = _databaseContext.Items.Add(value);
      var recordCount = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      if (recordCount == 1){
        await publisher.PublishAsync(value)
          .ConfigureAwait(false);
      }
      return Ok(dbContextObject.Entity);
#elseif (HasDb)
      var dbContextObject = _databaseContext.Items.Add(value);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      return Ok(dbContextObject.Entity);
#elseif (HasEventing)
      await publisher.PublishAsync(value)
          .ConfigureAwait(false);
      return Ok(value);
#else
      return Ok();
#endif
    }

    // Put api/Items
    [HttpPut]
    [ProducesResponseType(Status200OK, Type = typeof(Item))]
    [ProducesResponseType(Status409Conflict)]
    [ProducesResponseType(Status404NotFound)]
    [ValidateModel]
#if (Redis)
    public async Task<ActionResult> Put([FromQuery] Guid id, [FromBody] Item value, [FromServices] RedisStreamPublisher<Item, UpdatedEvent> publisher)
#else
    public async Task<ActionResult> Put([FromQuery] Guid id, [FromBody] Item value)
#endif
    {
      if (id != value.Id)
      {
#if (HasLogging)
        _logger.LogWarning("Id values in querystring and post data do not match.");
#endif
        return Conflict($"Id values in query string and post data do not match.");
      }
#if (HasDb && HasEventing)
      var dbContextObject = await _databaseContext.Items.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      if (dbContextObject == null)
      {
#if (HasLogging)
        _logger.LogWarning("Item with Id: {id} was not found.", id);
#endif
        return NotFound($"{nameof(Item)} with Id: {id} was not found.");
      }
      SimpleMapper<Item>.Instance.ApplyChanges(value, dbContextObject);
      var recordCount = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      if (recordCount == 1){
        await publisher.PublishAsync(value)
          .ConfigureAwait(false);
      }
      return Ok(dbContextObject);
#elseif (HasDb)
      var dbContextObject = await _databaseContext.Items.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      if (dbContextObject == null)
      {
#if (HasLogging)
        _logger.LogWarning("Item with Id: {id} was not found.", id);
#endif
        return NotFound($"{nameof(Item)} with Id: {id} was not found.");
      }
      SimpleMapper<Item>.Instance.ApplyChanges(value, dbContextObject);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      return Ok(dbContextObject);
#elseif (HasEventing)
      await publisher.PublishAsync(value)
          .ConfigureAwait(false);
      return Ok(value);
#else
      return Ok();
#endif
    }

    // Put api/Items
    [HttpDelete]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status404NotFound)]
#if (Redis)
    public async Task<ActionResult> Delete([FromQuery] Guid id, [FromServices] RedisStreamPublisher<Item, DeletedEvent> publisher)
#else
    public async Task<ActionResult> Delete([FromQuery] Guid id)
#endif
    {
#if (HasDb && HasEventing)
      var dbContextObject = await _databaseContext.Items.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      if (dbContextObject == null)
      {
#if (HasLogging)
        _logger.LogWarning("Item with Id: {id} was not found.", id);
#endif
        return NotFound($"{nameof(Item)} with Id: {id} was not found.");
      }
      _ = _databaseContext.Remove(dbContextObject);
      var recordCount = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      if (recordCount == 1){
        await publisher.PublishAsync(dbContextObject)
          .ConfigureAwait(false);
      }
#elseif (HasDb)
      var dbContextObject = await _databaseContext.Items.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      if (dbContextObject == null)
      {
#if (HasLogging)
        _logger.LogWarning("Item with Id: {id} was not found.", id);
#endif
        return NotFound($"{nameof(Item)} with Id: {id} was not found.");
      }
      _ = _databaseContext.Remove(dbContextObject);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
#endif
      return Ok();
    }
  }
}
