using Microsoft.EntityFrameworkCore;
using V1 = NRSRx_ServiceName.Models.V1;

namespace NRSRx_ServiceName.Data
{
  public partial class DatabaseContext
  {
    public virtual DbSet<V1.ItemModel> ItemModels { get; set; }
  }
}
