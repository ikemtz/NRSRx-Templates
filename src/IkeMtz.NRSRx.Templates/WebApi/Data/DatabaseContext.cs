using IkeMtz.NRSRx.Core;
using IkeMtz.NRSRx.Core.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace NRSRx_ServiceName.Data
{
  public partial class DatabaseContext : AuditableDbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> options, ICurrentUserProvider currentUserProvider)
        : base(options, currentUserProvider)
    {
    }
  }
}
