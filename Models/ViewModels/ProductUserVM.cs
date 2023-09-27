namespace WebApp.Models.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            Products = new List<Product>();
        }
        public ApplicationUser? ApplicationUser { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
