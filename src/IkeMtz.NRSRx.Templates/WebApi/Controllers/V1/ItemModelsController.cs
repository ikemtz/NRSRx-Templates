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
  public class ItemModelsController : ControllerBase
  {
#if (HasLogging)
    private readonly ILogger<ItemModelsController> _logger;
#endif
#if (HasDb)
    private readonly DatabaseContext _databaseContext;
#endif
#if (HasDb && HasLogging)
    public ItemModelsController(DatabaseContext databaseContext, ILogger<ItemModelsController> logger)
    {
      _databaseContext = databaseContext;
      _logger = logger;
    }
#elseif (HasDb)
    public ItemModelsController(DatabaseContext databaseContext)
    {
      _databaseContext = databaseContext;
    }
#elseif (HasLogging)
    public ItemModelsController(ILogger<ItemModelsController> logger)
    {
      _logger = logger;
    }
#else
    public ItemModelsController()
    {
    }
#endif

    // Get api/ItemModels
    [HttpGet]
    [ProducesResponseType(Status200OK, Type = typeof(ItemModel))]
#if (HasDb)
    [ProducesResponseType(Status404NotFound)]
#endif
    public async Task<ActionResult> Get([FromQuery] Guid id)
    {
#if (HasDb)
      var dbItemModel = await _databaseContext.ItemModels
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);

      if (dbItemModel == null)
      {
        return NotFound();
      }
      return Ok(dbItemModel);
#else
      return Ok();
#endif
    }

    // Post api/ItemModels
    [HttpPost]
    [ProducesResponseType(Status200OK, Type = typeof(ItemModel))]
    [ValidateModel]
#if (Redis)
    public async Task<ActionResult> Post([FromBody] ItemModelUpsertRequest request, [FromServices] IPublisher<ItemModel, CreatedEvent> publisher)
#else
    public async Task<ActionResult> Post([FromBody] ItemModelUpsertRequest request)
#endif
    {
      var value = SimpleMapper<ItemModelUpsertRequest, ItemModel>.Instance.Convert(request);
#if (HasDb && HasEventing)
      var dbItemModel = _databaseContext.ItemModels.Add(value);
      var recordCount = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      if (recordCount > 0){
        await publisher.PublishAsync(value)
          .ConfigureAwait(false);
      }
      return Ok(dbItemModel.Entity);
#elseif (HasDb)
      var dbItemModel = _databaseContext.ItemModels.Add(value);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      return Ok(dbItemModel.Entity);
#elseif (HasEventing)
      await publisher.PublishAsync(value)
          .ConfigureAwait(false);
      return Ok(value);
#else
      return Ok();
#endif
    }

    // Put api/ItemModels
    [HttpPut]
    [ProducesResponseType(Status200OK, Type = typeof(ItemModel))]
    [ProducesResponseType(Status409Conflict)]
    [ProducesResponseType(Status404NotFound)]
    [ValidateModel]
#if (Redis)
    public async Task<ActionResult> Put([FromQuery] Guid id, [FromBody] ItemModelUpsertRequest request, [FromServices] IPublisher<ItemModel, UpdatedEvent> publisher)
#else
    public async Task<ActionResult> Put([FromQuery] Guid id, [FromBody] ItemModelUpsertRequest request)
#endif
    {
      if (id != request.Id)
      {
#if (HasLogging)
        _logger.LogWarning("Id values in querystring and post data do not match.");
#endif
        return Conflict($"Id values in query string and post data do not match.");
      }
#if (HasDb && HasEventing)
      var dbItemModel = await _databaseContext.ItemModels.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      if (dbItemModel == null)
      {
#if (HasLogging)
        _logger.LogWarning("ItemModel with Id: {id} was not found.", id);
#endif
        return NotFound($"{nameof(ItemModel)} with Id: {id} was not found.");
      }
      SimpleMapper<ItemModelUpsertRequest, ItemModel>.Instance.ApplyChanges(request, dbItemModel);
      var recordCount = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      if (recordCount > 0){
        await publisher.PublishAsync(dbItemModel)
          .ConfigureAwait(false);
      }
      return Ok(dbItemModel);
#elseif (HasDb)
      var dbItemModel = await _databaseContext.ItemModels.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      if (dbItemModel == null)
      {
#if (HasLogging)
        _logger.LogWarning("ItemModel with Id: {id} was not found.", id);
#endif
        return NotFound($"{nameof(ItemModel)} with Id: {id} was not found.");
      }
      SimpleMapper<ItemModelUpsertRequest, ItemModel>.Instance.ApplyChanges(request, dbItemModel);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      return Ok(dbItemModel);
#elseif (HasEventing && HasDb)
      await publisher.PublishAsync(dbItemModel)
          .ConfigureAwait(false);
      return Ok(dbItemModel);
#elseif (HasEventing)
      await publisher.PublishAsync(value)
          .ConfigureAwait(false);
      return Ok(value);
#else
      return Ok();
#endif
    }

    // Put api/ItemModels
    [HttpDelete]
    [ProducesResponseType(Status200OK)]
    [ProducesResponseType(Status404NotFound)]
#if (Redis)
    public async Task<ActionResult> Delete([FromQuery] Guid id, [FromServices] IPublisher<ItemModel, DeletedEvent> publisher)
#else
    public async Task<ActionResult> Delete([FromQuery] Guid id)
#endif
    {
#if (HasDb && HasEventing)
      var dbItemModel = await _databaseContext.ItemModels.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      if (dbItemModel == null)
      {
#if (HasLogging)
        _logger.LogWarning("ItemModel with Id: {id} was not found.", id);
#endif
        return NotFound($"{nameof(ItemModel)} with Id: {id} was not found.");
      }
      _ = _databaseContext.Remove(dbItemModel);
      var recordCount = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      if (recordCount > 0){
        await publisher.PublishAsync(dbItemModel)
          .ConfigureAwait(false);
      }
#elseif (HasDb)
      var dbItemModel = await _databaseContext.ItemModels.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      if (dbItemModel == null)
      {
#if (HasLogging)
        _logger.LogWarning("ItemModel with Id: {id} was not found.", id);
#endif
        return NotFound($"{nameof(ItemModel)} with Id: {id} was not found.");
      }
      _ = _databaseContext.Remove(dbItemModel);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
#endif
      return Ok();
    }
  }
}
