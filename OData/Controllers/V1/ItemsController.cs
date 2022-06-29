using IkeMtz.NRSRx.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using NRSRx_OData.Data;
using NRSRx_OData.Models.V1;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NRSRx_OData.Controllers.V1
{
  [ApiVersion("1.0")]
  [Authorize]
  [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 6000)]
  public class ItemsController : ODataController
  {
    private readonly DatabaseContext _databaseContext;

    public ItemsController(DatabaseContext databaseContext)
    {
      _databaseContext = databaseContext;
    }

    [ProducesResponseType(typeof(ODataEnvelope<Item, Guid>), Status200OK)]
    [EnableQuery(MaxTop = 100, AllowedQueryOptions = AllowedQueryOptions.All)]
    [HttpGet]
    public IQueryable<Item> Get()
    {
      return _databaseContext.Items
        .AsNoTracking();
    }
  }
}
