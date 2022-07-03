using System;
using System.ComponentModel.DataAnnotations;
using IkeMtz.NRSRx.Core.Models;

namespace NRSRx_ServiceName.Models.V1
{

  public partial class ItemModel : ItemModelUpsertRequest, IAuditable
  {
   
    [Required]
    public string CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? UpdatedOnUtc { get; set; }
  }
  public partial class ItemModelUpsertRequest: IIdentifiable {
    public Guid Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }
  }
}
