using Microsoft.EntityFrameworkCore;

namespace NRSRx_ServiceName.Data
{
  public partial class DatabaseContext : DbContext
  {
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }
  }
}
