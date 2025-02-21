using CourseworkTerm5.Server.Entities;

record OrderDto(CustomerDto Customer,
                PaymentType PaymentType,
                decimal TotalPriceRub,
                bool UseShipping);