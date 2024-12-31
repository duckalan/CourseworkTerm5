using CourseworkTerm5.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseworkTerm5.Server;

public partial class TastyPizzaContext : DbContext
{
    public TastyPizzaContext()
    {
    }

    public TastyPizzaContext(DbContextOptions<TastyPizzaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderProduct> OrderProducts { get; set; }

    public virtual DbSet<PaymentType> PaymentTypes { get; set; }

    public virtual DbSet<PizzaDiameter> PizzaDiameters { get; set; }

    public virtual DbSet<PizzaVariant> PizzaVariants { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Topping> Toppings { get; set; }

    public virtual DbSet<ToppingVariant> ToppingVariants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__46596229D81B62A2");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ClosedAt)
                .HasColumnType("datetime")
                .HasColumnName("closed_at");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CustomerAddress)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_address");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("customer_email");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("customer_phone");
            entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");
            entity.Property(e => e.TotalPriceRub)
                .HasColumnType("money")
                .HasColumnName("total_price_rub");
            entity.Property(e => e.UseShipping).HasColumnName("use_shipping");

            entity.HasOne(d => d.PaymentType).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__payment___6B24EA82");
        });

        modelBuilder.Entity<OrderProduct>(entity =>
        {
            entity.HasKey(e => e.OrderProductId).HasName("PK__Order_Pr__6530D82B4044D8A8");

            entity.ToTable("Order_Product");

            entity.HasIndex(e => new { e.OrderId, e.ProductId, e.PizzaVariantId }, "UQ_Order_Product_order_id_product_id_pizza_variant_id").IsUnique();

            entity.Property(e => e.OrderProductId).HasColumnName("order_product_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PizzaVariantId).HasColumnName("pizza_variant_id");
            entity.Property(e => e.ProductCount).HasColumnName("product_count");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_Pro__order__6FE99F9F");

            entity.HasOne(d => d.PizzaVariant).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.PizzaVariantId)
                .HasConstraintName("FK__Order_Pro__pizza__71D1E811");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order_Pro__produ__70DDC3D8");

            entity.HasMany(d => d.ToppingVariants).WithMany(p => p.OrderProducts)
                .UsingEntity<Dictionary<string, object>>(
                    "OrderProductToppingVariant",
                    r => r.HasOne<ToppingVariant>().WithMany()
                        .HasForeignKey("ToppingVariantId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Order_Pro__toppi__76969D2E"),
                    l => l.HasOne<OrderProduct>().WithMany()
                        .HasForeignKey("OrderProductId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Order_Pro__order__75A278F5"),
                    j =>
                    {
                        j.HasKey("OrderProductId", "ToppingVariantId").HasName("PK__Order_Pr__D8976DC9F3C287A7");
                        j.ToTable("Order_Product_Topping_variant");
                        j.IndexerProperty<int>("OrderProductId").HasColumnName("order_product_id");
                        j.IndexerProperty<int>("ToppingVariantId").HasColumnName("topping_variant_id");
                    });
        });

        modelBuilder.Entity<PaymentType>(entity =>
        {
            entity.HasKey(e => e.PaymentTypeId).HasName("PK__Payment___8C1ABD6FB6E75682");

            entity.ToTable("Payment_types");

            entity.HasIndex(e => e.Name, "UQ_Payment_types_name").IsUnique();

            entity.Property(e => e.PaymentTypeId).HasColumnName("payment_type_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<PizzaDiameter>(entity =>
        {
            entity.HasKey(e => e.PizzaDiameterId).HasName("PK__Pizza_di__A4323142E15C6575");

            entity.ToTable("Pizza_diameters");

            entity.HasIndex(e => e.DiameterCm, "UQ_Pizza_diameters_diameter_cm").IsUnique();

            entity.Property(e => e.PizzaDiameterId).HasColumnName("pizza_diameter_id");
            entity.Property(e => e.DiameterCm).HasColumnName("diameter_cm");
        });

        modelBuilder.Entity<PizzaVariant>(entity =>
        {
            entity.HasKey(e => e.PizzaVariantId).HasName("PK__Pizza_va__DABEA0052C8144CB");

            entity.ToTable("Pizza_variants");

            entity.HasIndex(e => new { e.ProductId, e.PizzaDiameterId }, "UQ_Pizza_variants_product_id_pizza_diameter_id").IsUnique();

            entity.Property(e => e.PizzaVariantId).HasColumnName("pizza_variant_id");
            entity.Property(e => e.PizzaDiameterId).HasColumnName("pizza_diameter_id");
            entity.Property(e => e.PriceRub)
                .HasColumnType("smallmoney")
                .HasColumnName("price_rub");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.WeightGram).HasColumnName("weight_gram");

            entity.HasOne(d => d.PizzaDiameter).WithMany(p => p.PizzaVariants)
                .HasForeignKey(d => d.PizzaDiameterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pizza_var__pizza__59063A47");

            entity.HasOne(d => d.Product).WithMany(p => p.PizzaVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pizza_var__produ__5812160E");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__47027DF5F3EC13EA");

            entity.HasIndex(e => e.Description, "UQ_Products_description").IsUnique();

            entity.HasIndex(e => e.ImageUrl, "UQ_Products_image_url").IsUnique();

            entity.HasIndex(e => e.Name, "UQ_Products_name").IsUnique();

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.BasePriceRub)
                .HasColumnType("smallmoney")
                .HasColumnName("base_price_rub");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(1000)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__produc__4F7CD00D");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.ProductCategoryId).HasName("PK__Product___1F8847F97123253A");

            entity.ToTable("Product_categories");

            entity.HasIndex(e => e.Name, "UQ_Product_categories_name").IsUnique();

            entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Topping>(entity =>
        {
            entity.HasKey(e => e.ToppingId).HasName("PK__Toppings__141E1E067039B065");

            entity.HasIndex(e => e.ImageUrl, "UQ_Toppings_image_url").IsUnique();

            entity.HasIndex(e => e.Name, "UQ_Toppings_name").IsUnique();

            entity.Property(e => e.ToppingId).HasColumnName("topping_id");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(1000)
                .HasColumnName("image_url");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ToppingVariant>(entity =>
        {
            entity.HasKey(e => e.ToppingVariantId).HasName("PK__Topping___DA7B5E2AAA1CDCCE");

            entity.ToTable("Topping_variants");

            entity.HasIndex(e => new { e.ToppingId, e.PizzaDiameterId }, "UQ_Topping_variants_topping_id_pizza_diameter_id").IsUnique();

            entity.Property(e => e.ToppingVariantId).HasColumnName("topping_variant_id");
            entity.Property(e => e.PizzaDiameterId).HasColumnName("pizza_diameter_id");
            entity.Property(e => e.PriceRub)
                .HasColumnType("smallmoney")
                .HasColumnName("price_rub");
            entity.Property(e => e.ToppingId).HasColumnName("topping_id");
            entity.Property(e => e.WeightGram).HasColumnName("weight_gram");

            entity.HasOne(d => d.PizzaDiameter).WithMany(p => p.ToppingVariants)
                .HasForeignKey(d => d.PizzaDiameterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Topping_v__pizza__6383C8BA");

            entity.HasOne(d => d.Topping).WithMany(p => p.ToppingVariants)
                .HasForeignKey(d => d.ToppingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Topping_v__toppi__628FA481");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
