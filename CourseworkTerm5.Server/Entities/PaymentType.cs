namespace CourseworkTerm5.Server.Entities;

public partial class PaymentType
{
    public int PaymentTypeId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
