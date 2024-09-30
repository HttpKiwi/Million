using Microsoft.EntityFrameworkCore;
using Million.Domain.Enitities;

namespace Million.Infrastructure.Data
{
    /// <summary>
    /// Represents the database context for the Million.API application.
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="options">The options for the database context.</param>
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        /// <summary>
        /// Gets or sets the collection of owners in the database.
        /// </summary>
        public DbSet<Owner> Owner { get; set; }

        /// <summary>
        /// Gets or sets the collection of properties in the database.
        /// </summary>
        public DbSet<Property> Property { get; set; }

        /// <summary>
        /// Gets or sets the collection of property images in the database.
        /// </summary>
        public DbSet<PropertyImage> PropertyImage { get; set; }

        /// <summary>
        /// Gets or sets the collection of property traces in the database.
        /// </summary>
        public DbSet<PropertyTrace> PropertyTrace { get; set; }

        /// <summary>
        /// Configures the model relationships and constraints for the database.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure the model.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
                .HasOne<Owner>()
                .WithMany(o => o.Properties)
                .HasForeignKey(p => p.IdOwner)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropertyImage>()
                .HasOne(pi => pi.Property)
                .WithMany(p => p.PropertyImages)
                .HasForeignKey(pi => pi.IdProperty)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PropertyTrace>()
                .HasOne(pt => pt.Property)
                .WithMany(p => p.PropertyTraces)
                .HasForeignKey(pt => pt.IdProperty)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Owner>().HasData(
                    new Owner { IdOwner = 1, Name = "John Doe", Address = "123 Elm Street", Birthday = new DateTime(1975, 8, 15), Photo = null },
                    new Owner { IdOwner = 2, Name = "Jane Smith", Address = "456 Oak Avenue", Birthday = new DateTime(1980, 5, 22), Photo = null }
                );

            modelBuilder.Entity<Property>().HasData(
                new Property
                {
                    IdProperty = 1,
                    Name = "Modern Villa",
                    Address = "789 Pine Road",
                    Price = 500000,
                    CodeInternal = "MODV123",
                    Year = 2015,
                    IdOwner = 1
                },
                new Property
                {
                    IdProperty = 2,
                    Name = "Beachfront Condo",
                    Address = "10 Ocean Drive",
                    Price = 300000,
                    CodeInternal = "BFCD456",
                    Year = 2018,
                    IdOwner = 2
                }
            );

            modelBuilder.Entity<PropertyImage>().HasData(
                new PropertyImage
                {
                    IdPropertyImage = 1,
                    File = null,
                    Enabled = true,
                    IdProperty = 1
                },
                new PropertyImage
                {
                    IdPropertyImage = 2,
                    File = null,
                    Enabled = true,
                    IdProperty = 2
                }
            );

            modelBuilder.Entity<PropertyTrace>().HasData(
                new PropertyTrace
                {
                    IdPropertyTrace = 1,
                    DateSale = new DateTime(2020, 7, 15),
                    Name = "Initial Sale",
                    Value = 450000,
                    Tax = 45000,
                    IdProperty = 1
                },
                new PropertyTrace
                {
                    IdPropertyTrace = 2,
                    DateSale = new DateTime(2021, 3, 10),
                    Name = "Initial Sale",
                    Value = 280000,
                    Tax = 28000,
                    IdProperty = 2
                }
            );
        }
    }
}

