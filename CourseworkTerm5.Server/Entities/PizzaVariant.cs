namespace CourseworkTerm5.Server.Entities;

public partial class PizzaVariant
{
    public int PizzaVariantId { get; set; }

    public int ProductId { get; set; }

    public int PizzaDiameterId { get; set; }

    public short WeightGram { get; set; }

    public decimal PriceRub { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual PizzaDiameter PizzaDiameter { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
