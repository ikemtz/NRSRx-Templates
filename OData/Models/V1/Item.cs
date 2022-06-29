using IkeMtz.NRSRx.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace NRSRx_OData.Models.V1
{

    public partial class Item : IIdentifiable<Guid>, IAuditable
    {
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        public string CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTimeOffset CreatedOnUtc { get; set; }
        public DateTimeOffset? UpdatedOnUtc { get; set; }
    }
}
