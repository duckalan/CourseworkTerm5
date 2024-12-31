using Microsoft.EntityFrameworkCore;

namespace CourseworkTerm5.Server
{
    public static class ApiEndpointGroup
    {
        public static RouteGroupBuilder MapApi(this RouteGroupBuilder group)
        {
            group.MapGet("/product-categories", async (TastyPizzaContext dbContext) =>
                await dbContext.ProductCategories
                    .AsNoTracking()
                    .Include(pc => pc.Products)
                    .Select(pc => new
                    {
                        pc.ProductCategoryId,
                        pc.Name,
                        Products = pc.Products.Select(p => new
                        {
                            p.ProductId,
                            p.Name,
                            p.Description,
                            p.BasePriceRub,
                            p.ImageUrl,
                        })
                    })
                    .ToListAsync()
            );

            return group;
        }
    }
}
