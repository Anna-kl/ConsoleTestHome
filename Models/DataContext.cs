using Microsoft.EntityFrameworkCore;



namespace TestHomeWork.Models
{
    public class EFDataContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public EFDataContext(DbContextOptions<EFDataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("homework");
            modelBuilder.Entity<Currency>()
               .Property(p => p.Id)
               .ValueGeneratedOnAdd();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("Host = postgres78.1gb.ru; Username = xgb_ocpio; Password = ZPyDk7j-cfHA; Database = xgb_ocpio; Port = 5432");
        //}
    }
}
