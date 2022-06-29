using Microsoft.EntityFrameworkCore;
using V1 = NRSRx_OData.Models.V1;

namespace NRSRx_OData.Data
{
  public partial class DatabaseContext : DbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<V1.Item> Items { get; set; } 
  }
}
