namespace CourseworkTerm5.Server.Entities;

public partial class Product
{
    public int ProductId { get; set; }

    public int ProductCategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? BasePriceRub { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ICollection<PizzaVariant> PizzaVariants { get; set; } = new List<PizzaVariant>();

    public virtual ProductCategory ProductCategory { get; set; } = null!;
}
