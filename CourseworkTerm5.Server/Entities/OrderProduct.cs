namespace CourseworkTerm5.Server.Entities;

public partial class OrderProduct
{
    public int OrderProductId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? PizzaVariantId { get; set; }

    public byte ProductCount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual PizzaVariant? PizzaVariant { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual ICollection<ToppingVariant> ToppingVariants { get; set; } = new List<ToppingVariant>();
}
