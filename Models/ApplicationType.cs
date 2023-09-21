using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ApplicationType
    {
        [Key]
        public int ApplicationTypeId { get; set; }
        [Required]
        [DisplayName("Applicatin Type")]
        public string? Name { get; set; }
        //public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
