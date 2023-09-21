using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;

namespace WebApp.Models
{
    public class Product
    {
        /*private readonly ApplicationDbContext db;
        public Product(ApplicationDbContext _db)
        {
            db = _db;
        }*/

        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(1,int.MaxValue)]
        public int Price { get; set; }
        public string? Image { get; set; }
        [Display(Name ="Category Type")]
        [Required]
        public int CategoryId { get;set; }
        [ForeignKey("CategoryId")]
        [Required]
        public virtual Category Category { get; set; }
        //public Category Category { get; set; }

        [Display(Name = "Application Type")]
        [Required]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }


    }
}
