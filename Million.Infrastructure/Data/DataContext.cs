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
        }
    }
}
