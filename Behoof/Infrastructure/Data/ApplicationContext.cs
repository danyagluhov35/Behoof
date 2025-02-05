using Microsoft.EntityFrameworkCore;

namespace Behoof.Infrastructure.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Country> Country { get; set; } = null!;
        public DbSet<City> City { get; set; } = null!;
        public DbSet<Color> Color { get; set; } = null!;
        public DbSet<Camera> Camera { get; set; } = null!;
        public DbSet<Diagonal> Diagonal { get; set; } = null!;
        public DbSet<Memory> Memory { get; set; } = null!;
        public DbSet<Network> Network { get; set; } = null!;
        public DbSet<Power> Power { get; set; } = null!;
        public DbSet<Product> Product { get; set; } = null!;
        public DbSet<Category> Category { get; set; } = null!;
        public DbSet<System> System { get; set; } = null!;
        public DbSet<YearOfRealise> YearOfRealise { get; set; } = null!;
        public DbSet<Favorite> Favorite { get; set; } = null!;
        public DbSet<FavoriteItem> FavoriteItem { get; set; } = null!;
        public DbSet<Supplier> Supplier { get; set; } = null!;
        public DbSet<HistoryProduct> HistoryProduct { get; set; } = null!;
        public DbSet<SupplierProduct> SupplierProduct { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-82QJ6HP;Database=Behoof;Trusted_Connection=True;Encrypt=False");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupplierProduct>()
                .HasKey(sp => new { sp.SupplierId, sp.ProductId }); // Композитный ключ

            modelBuilder.Entity<SupplierProduct>()
                .HasOne(sp => sp.Supplier)
                .WithMany(s => s.SupplierProducts)
                .HasForeignKey(sp => sp.SupplierId);

            modelBuilder.Entity<SupplierProduct>()
                .HasOne(sp => sp.Product)
                .WithMany(p => p.SupplierProducts)
                .HasForeignKey(sp => sp.ProductId);
        }
    }
}
