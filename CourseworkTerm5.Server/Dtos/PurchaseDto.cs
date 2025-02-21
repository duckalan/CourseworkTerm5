using CourseworkTerm5.Server.Entities;

namespace CourseworkTerm5.Server.Dtos
{
    public record PurchaseDto(CartItemDto CartItem, int Quantity);

    public record CartItemDto(
        int ProductId,
        string Name,
        string? Description,
        Uri ImageUrl,
        decimal PriceRub,
        PizzaDiameter? diameterCm,
        toppings?: ToppingPreview[]);
}
