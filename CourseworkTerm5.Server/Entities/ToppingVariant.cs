namespace CourseworkTerm5.Server.Entities;

public partial class ToppingVariant
{
    public int ToppingVariantId { get; set; }

    public int ToppingId { get; set; }

    public int PizzaDiameterId { get; set; }

    public short WeightGram { get; set; }

    public decimal PriceRub { get; set; }

    public virtual PizzaDiameter PizzaDiameter { get; set; } = null!;

    public virtual Topping Topping { get; set; } = null!;

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
