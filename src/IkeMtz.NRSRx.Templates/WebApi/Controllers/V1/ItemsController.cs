using System;
using System.Threading.Tasks;
using IkeMtz.NRSRx.Core.Models;
using IkeMtz.NRSRx.Core.WebApi;
using NRSRx_WebApi.Models.V1;
using NRSRx_WebApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NRSRx_WebApi.Controllers.V1
{
  [Route("api/v{version:apiVersion}/[controller].{format}"), FormatFilter]
  [ApiVersion(VersionDefinitions.v1_0)]
  [ApiController]
  [Authorize]
  public class ItemsController : ControllerBase
  {
    private readonly DatabaseContext _databaseContext;
    public ItemsController(DatabaseContext databaseContext)
    {
      _databaseContext = databaseContext;
    }

    // Get api/Items
    [HttpGet]
    [ProducesResponseType(Status200OK, Type = typeof(Item))]
    public async Task<ActionResult> Get([FromQuery] Guid id)
    {
      var obj = await _databaseContext.Items
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      return Ok(obj);
    }

    // Post api/Items
    [HttpPost]
    [ProducesResponseType(Status200OK, Type = typeof(Item))]
    [ValidateModel]
    public async Task<ActionResult> Post([FromBody] Item value)
    {
      var dbContextObject = _databaseContext.Items.Add(value);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      return Ok(dbContextObject.Entity);
    }

    // Put api/Items
    [HttpPut]
    [ProducesResponseType(Status200OK, Type = typeof(Item))]
    [ValidateModel]
    public async Task<ActionResult> Put([FromQuery] Guid id, [FromBody] Item value)
    {
      var obj = await _databaseContext.Items.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      SimpleMapper<Item>.Instance.ApplyChanges(value, obj);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      return Ok(obj);
    }

    // Put api/Items
    [HttpDelete]
    [ProducesResponseType(Status200OK, Type = typeof(Item))]
    public async Task<ActionResult> Delete([FromQuery] Guid id)
    {
      var obj = await _databaseContext.Items.FirstOrDefaultAsync(t => t.Id == id)
        .ConfigureAwait(false);
      _ = _databaseContext.Remove(obj);
      _ = await _databaseContext.SaveChangesAsync()
          .ConfigureAwait(false);
      return Ok(obj);
    }
  }
}
