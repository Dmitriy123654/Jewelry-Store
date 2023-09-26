using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApp.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApp.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string? ShortDesc { get; set; } = "";
        [Required]
        [Range(1, 15)]
        public string? Description { get; set; } = " ";
        [Required]
        [Range(1,int.MaxValue)]
        public int Price { get; set; }
        public string? Image { get; set; }


        [Display(Name ="Category Type")]
        [Required]
        public int CategoryId { get;set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public virtual Category Category { get; set; }


        [Display(Name = "Application Type")]
        [Required]
        public int ApplicationTypeId { get; set; }
        [ForeignKey("ApplicationTypeId")]
        [ValidateNever]
        public virtual ApplicationType ApplicationType { get; set; }

    }
}
