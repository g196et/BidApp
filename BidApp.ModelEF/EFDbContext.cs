using System.Data.Entity;

namespace BidApp.ModelEF
{
    /// <summary>
    /// Контекст для таблиц Bids и Comments
    /// </summary>
    class EFDbContext : DbContext
    {
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Конфигурация для связи one-to-many (1 заявка ко многим комментариям)
            modelBuilder.Entity<Comment>()
                .HasRequired<Bid>(s => s.Bid)
                .WithMany(g => g.Comments)
                .HasForeignKey<int>(s => s.BidId)
                .WillCascadeOnDelete(true);
        }
    }
}
