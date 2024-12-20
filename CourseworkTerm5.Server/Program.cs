var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

Product[] products = InitProducts();

ProductSection[] productSections = InitProductSections(products);

app.MapGet("/api/product-sections", () => productSections)
.WithName("GetProductSections");

app.MapGet("/api/products", () => products)
.WithName("GetProducts");

app.MapFallbackToFile("/index.html");

app.Run();

static Product[] InitProducts()
{
    UriBuilder uriBuilder = new("http", "localhost", 5105, "images/Products/placeholder.png");
    Uri uri = uriBuilder.Uri;

    return [
    new Product(1, 1, "Продукт1", "Ингридиенты", 9.99m, uri),
    new Product(2, 1, "Продукт2", "Ингридиенты", 99.99m, uri),
    new Product(3, 1, "Продукт3", "Ингридиенты", 999.99m, uri),

    new Product(4, 2, "Продукт4", "Ингридиенты", 9999.99m, uri),
    new Product(5, 2, "Продукт5", "Ингридиенты", 99999.99m, uri),

    new Product(6, 3, "Продукт6", "Ингридиенты", 999999.99m, uri),
];
}

static ProductSection[] InitProductSections(Product[] products)
{
    return [
        new ProductSection(
        1,
        "Пиццы",
        products.Where(product => product.SectionId == 1)),

    new ProductSection(
        2,
        "Соусы",
        products.Where(product => product.SectionId == 2)),

    new ProductSection(
        3,
        "Напитки",
        products.Where(product => product.SectionId == 3)),
];
}