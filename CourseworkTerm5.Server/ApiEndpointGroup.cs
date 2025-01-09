using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;

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
                })
                .ToListAsync()
            );

            group.MapPost("/sendemail", async
                ([FromBody] Temporary t, IConfiguration config) =>
            {
                using var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("ВкусноПицца", config["EmailSettings:auth:user"]));
                emailMessage.To.Add(new MailboxAddress("", t.UserEmail));
                emailMessage.Subject = "Ваш чек";
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = string.Join(", ", t.Products)
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(config["EmailSettings:host"], config.GetValue<int>("EmailSettings:port"), false);
                    await client.AuthenticateAsync(config["EmailSettings:auth:user"], config["EmailSettings:auth:pass"]);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }

                Results.Ok(t.UserEmail);
            });

            return group;
        }
    }
}
record Temporary(string UserEmail, string[] Products);