using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BreakingNewsWeb.Models.TestQueryToDb;

public class EducationContext : DbContext
{
    public DbSet<Customer> customers { get; set; }
    public DbSet<Order> orders { get; set; }
    public DbSet<Product> products { get; set; }
    public EducationContext(DbContextOptions<EducationContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Order>()
        //      .HasOne(Customer)
        //      .WithMany(Order)
        //      .
    }
}


[Table("customer", Schema = "public")]
public class Customer
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("age")]
    public int Age { get; set; }
}

[Table("orders", Schema = "public")]
public class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_id")]
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product? Product { get; set; }

    [Column("customer_id")]
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("product_count")]
    public int ProductCount { get; set; }

    [Column("price")]
    public decimal Price { get; set; }
}

[Table("products", Schema = "public")]
public class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("product_name")]
    public string ProductName { get; set; }

    [Column("company")]
    public string Company { get; set; }

    [Column("product_count")]
    public int ProductCount { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

}
