namespace CourseworkTerm5.Server.Entities;

public partial class Topping
{
    public int ToppingId { get; set; }

    public string Name { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public virtual ICollection<ToppingVariant> ToppingVariants { get; set; } = new List<ToppingVariant>();
}
