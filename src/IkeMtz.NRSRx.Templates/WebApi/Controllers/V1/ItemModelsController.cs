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

#if (HasDb)
    private async Task<ItemModel?> GetItemModel(Guid id)
    {
      var dbItemModel = await _databaseContext.ItemModels
        .AsSplitQuery()
        .FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);

#if (HasLogging)
    if (dbItemModel == null) {
      _logger.LogWarning("ItemModel with Id: {id} was not found.", id);
    }
#endif
      return dbItemModel;
    }
#endif

    // Get api/ItemModels
    [HttpGet]
    [ProducesResponseType(Status200OK, Type = typeof(ItemModel))]
#if (HasDb)
    [ProducesResponseType(Status404NotFound, Type = typeof(ProblemDetails))]
#endif
    public async Task<ActionResult<ItemModel>> Get([FromQuery] Guid id)
    {
#if (HasDb)
      var dbItemModel = await GetItemModel(id);

      return (dbItemModel == null) ?
        NotFound(new ProblemDetails { Title = $"{nameof(ItemModel)} with Id: {id} was not found."}):
        Ok(dbItemModel);
#else
      return Ok();
#endif
    }

    // Post api/ItemModels
    [HttpPost]
    [ProducesResponseType(Status200OK, Type = typeof(ItemModel))]
    [ValidateModel]
#if (Redis)
    public async Task<ActionResult<ItemModel>> Post([FromBody] ItemModelUpsertRequest request, [FromServices] IPublisher<ItemModel, CreatedEvent> publisher)
#else
    public async Task<ActionResult<ItemModel>> Post([FromBody] ItemModelUpsertRequest request)
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
    [ProducesResponseType(Status409Conflict, Type = typeof(ProblemDetails))]
    [ProducesResponseType(Status404NotFound, Type = typeof(ProblemDetails))]
    [ValidateModel]
#if (Redis)
    public async Task<ActionResult<ItemModel>> Put([FromQuery] Guid id, [FromBody] ItemModelUpsertRequest request, [FromServices] IPublisher<ItemModel, UpdatedEvent> publisher)
#else
    public async Task<ActionResult<ItemModel>> Put([FromQuery] Guid id, [FromBody] ItemModelUpsertRequest request)
#endif
    {
      if (id != request.Id)
      {
#if (HasLogging)
        _logger.LogWarning("Id values in querystring and post data do not match.");
#endif
        return Conflict(new ProblemDetails { Title = $"Id values in query string and post data do not match." });
      }
#if (HasDb && HasEventing)
      var dbItemModel = await GetItemModel(id);
      if (dbItemModel == null)
      {
        return NotFound(new ProblemDetails { Title = $"{nameof(ItemModel)} with Id: {id} was not found."});
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
        return NotFound(new ProblemDetails { Title = $"{nameof(ItemModel)} with Id: {id} was not found." });
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
    [ProducesResponseType(Status404NotFound, Type = typeof(ProblemDetails))]
#if (Redis)
    public async Task<ActionResult<ItemModel>> Delete([FromQuery] Guid id, [FromServices] IPublisher<ItemModel, DeletedEvent> publisher)
#else
    public async Task<ActionResult<ItemModel>> Delete([FromQuery] Guid id)
#endif
    {
#if (HasDb && HasEventing)
      var dbItemModel = await GetItemModel(id);
      if (dbItemModel == null)
      {
        return NotFound(new ProblemDetails { Title = $"{nameof(ItemModel)} with Id: {id} was not found." });
      }
      _ = _databaseContext.Remove(dbItemModel);
      var recordCount = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      if (recordCount > 0){
        await publisher.PublishAsync(dbItemModel)
          .ConfigureAwait(false);
      }
#elseif (HasDb)
      var dbItemModel = await GetItemModel(id);
      if (dbItemModel == null)
      {
        return NotFound(new ProblemDetails { Title = $"{nameof(ItemModel)} with Id: {id} was not found." });
      }
      _ = _databaseContext.Remove(dbItemModel);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
#endif
      return Ok();
    }
  }
}
