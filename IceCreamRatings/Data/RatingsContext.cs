using IceCreamRatings.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Humanizer;
using System.Linq.Expressions;
using System.Reflection;

namespace IceCreamRatings.Data;

public class RatingsContext : DbContext
{
    public DbSet<Ratings> Ratings => Set<Ratings>();
    private readonly string connectionString;

    private static EntityTypeBuilder<T> Camelize<T>(EntityTypeBuilder<T> model) where T : class
    {
        PropertyInfo[] properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            string name = property.Name.Camelize();
            PropertyBuilder builder = model.Property(property.Name).ToJsonProperty(name);

            if (property.Name == "Id")
            {
                builder.ValueGeneratedOnAdd();
            }
        }

        return model;
    }
    public RatingsContext(string connectionString)
    {
        this.connectionString = connectionString;
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseCosmos(connectionString, "ICR", options => options.ConnectionMode(ConnectionMode.Direct));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ratings>(model => Camelize(model).HasNoDiscriminator().ToContainer(nameof(Ratings)).HasKey(e => e.Id));
    }
}
