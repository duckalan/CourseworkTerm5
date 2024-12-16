var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

Product[] products =
[
    new Product(1, 1, "Продукт1", "Ингридиенты", false, 9.99m),
    new Product(2, 1, "Продукт2", "Ингридиенты", false, 99.99m),
    new Product(3, 1, "Продукт3", "Ингридиенты", false, 999.99m),

    new Product(4, 2, "Продукт4", "Ингридиенты", false, 9999.99m),
    new Product(5, 2, "Продукт5", "Ингридиенты", false, 99999.99m),

    new Product(6, 3, "Продукт6", "Ингридиенты", false, 999999.99m),
];

ProductSection[] productSections =
[
    new ProductSection(1, "Пиццы", [products[0], products[1], products[2],]),
    new ProductSection(2, "Соусы", [products[3], products[4],]),
    new ProductSection(3, "Напитки", [products[5],]),
];

app.MapGet("/product-sections", () => productSections)
.WithName("GetProductSections");

app.MapGet("/products", () =>
{
    return products;
})
.WithName("GetProducts");

app.MapFallbackToFile("/index.html");

app.Run();

internal record ProductSection(int SectionId,
                               string Name,
                               IList<Product> Products);
internal record Product(int ProductId,
                        int SectionId,
                        string Name,
                        string Ingridients,
                        bool IsFixedPrice,
                        decimal Price);