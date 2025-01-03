using Microsoft.EntityFrameworkCore;

namespace CourseworkTerm5.Server
{
    public static class ApiEndpointGroup
    {
        public static RouteGroupBuilder MapApi(this RouteGroupBuilder group)
        {
            group.MapGet("/product-categories", async (TastyPizzaContext dbContext) =>
                await dbContext
                    .ProductCategories
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

            // TODO: Add check for empty Variants field if passed non pizza productId
            group.MapGet("/pizzas/{productId}", async (TastyPizzaContext dbContext, int productId) =>
                await dbContext
                    .Products
                    .AsNoTracking()
                    .Where(p => p.ProductId == productId)
                    .Include(p => p.PizzaVariants)
                    .ThenInclude(pv => pv.PizzaDiameter)
                    .Select(p => new
                    {
                        p.Name,
                        p.Description,
                        p.ImageUrl,
                        Variants = p.PizzaVariants.Select(pv => new
                        {
                            pv.PizzaDiameter.DiameterCm,
                            pv.WeightGram,
                            pv.PriceRub
                        })
                    })
                    .FirstAsync()
            );

            group.MapGet("/toppings", async (TastyPizzaContext dbContext) =>
                await dbContext
                .Toppings
                .AsNoTracking()
                .Include(t => t.ToppingVariants)
                .ThenInclude(tv => tv.PizzaDiameter)
                .Select(t => new
                {
                    t.ToppingId,
                    t.Name,
                    t.ImageUrl,
                    Variants = t.ToppingVariants.Select(tv => new
                    {
                        tv.PizzaDiameter.DiameterCm,
                        tv.WeightGram,
                        tv.PriceRub,
                    })
                }).ToListAsync()
            );

            return group;
        }
    }
}
