internal record ProductSection(
    int SectionId,
    string Name,
    IEnumerable<Product> Products);
