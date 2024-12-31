namespace CourseworkTerm5.Server.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerPhone { get; set; } = null!;

    public string CustomerEmail { get; set; } = null!;

    public string? CustomerAddress { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public bool UseShipping { get; set; }

    public int PaymentTypeId { get; set; }

    public decimal TotalPriceRub { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual PaymentType PaymentType { get; set; } = null!;
}
