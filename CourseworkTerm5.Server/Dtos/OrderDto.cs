using CourseworkTerm5.Server.Dtos;

record OrderDto(CustomerDto Customer,
                decimal TotalPriceRub,
                IEnumerable<PurchaseDto> Products);