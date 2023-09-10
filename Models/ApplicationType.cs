using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class ApplicationType
    {
        [Key]
        public int ApplicationTypeId { get; set; }
        [DisplayName("Applicatin Type")]
        public string? Name { get; set; }
    }
}
