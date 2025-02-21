namespace CourseworkTerm5.Server.Dtos
{
    public record CartItemDto(
        int ProductId,
        string Name,
        string? Description,
        Uri ImageUrl,
        decimal PriceRub);

    // !!!!!!!
    public record PurchaseDto(string ProductName, int Quantity);
}