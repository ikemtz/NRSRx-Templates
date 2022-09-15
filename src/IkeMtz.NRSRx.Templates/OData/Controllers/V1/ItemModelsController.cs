using IkeMtz.NRSRx.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using NRSRx_ServiceName.Data;
using NRSRx_ServiceName.Models.V1;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NRSRx_OData.Controllers.V1
{
  [ApiVersion("1.0")]
  [Authorize]
  [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 600)]
  public class ItemModelsController : ODataController
  {
    private readonly DatabaseContext _databaseContext;

    public ItemModelsController(DatabaseContext databaseContext)
    {
      _databaseContext = databaseContext;
    }

    [ProducesResponseType(typeof(ODataEnvelope<ItemModel, Guid>), Status200OK)]
    [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
    [HttpGet]
    public IQueryable<ItemModel> Get()
    {
      return _databaseContext.ItemModels
        .AsNoTracking();
    }
  }
}
