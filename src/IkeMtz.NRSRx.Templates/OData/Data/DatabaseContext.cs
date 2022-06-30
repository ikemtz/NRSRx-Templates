using Microsoft.EntityFrameworkCore;
using V1 = NRSRx_ServiceName.Models.V1;

namespace NRSRx_ServiceName.Data
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
