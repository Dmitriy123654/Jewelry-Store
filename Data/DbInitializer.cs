using WebApp.Models;
namespace WebApp.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Categories.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new Category[]
            {
                new Category{Name="test1",DisplayOrder=1},
                new Category{Name="test2",DisplayOrder=20}
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

           
        }
    }
}

