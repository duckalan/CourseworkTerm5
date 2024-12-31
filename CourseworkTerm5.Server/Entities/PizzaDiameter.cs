namespace CourseworkTerm5.Server.Entities;

public partial class PizzaDiameter
{
    public int PizzaDiameterId { get; set; }

    public byte DiameterCm { get; set; }

    public virtual ICollection<PizzaVariant> PizzaVariants { get; set; } = new List<PizzaVariant>();

    public virtual ICollection<ToppingVariant> ToppingVariants { get; set; } = new List<ToppingVariant>();
}
