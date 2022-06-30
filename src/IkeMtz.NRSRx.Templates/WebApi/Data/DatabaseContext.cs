using IkeMtz.NRSRx.Core.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using V1 = NRSRx_ServiceName.Models.V1;

namespace NRSRx_ServiceName.Data
{
  public partial class DatabaseContext : AuditableDbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options, httpContextAccessor)
    {
    }

    public virtual DbSet<V1.Item> Items { get; set; }
  }
}
