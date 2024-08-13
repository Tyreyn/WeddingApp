using Microsoft.EntityFrameworkCore;
using WeddingAppDTO.DataTransferObject;

namespace WeddingAppDTO.Context
{
    public class WeddingAppUserContext : DbContext, IWeddingAppUserContext
    {
        public WeddingAppUserContext(DbContextOptions<WeddingAppUserContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        public DbSet<PlannerComment> PlannerComments { get; set; }

        public DbSet<Food> Foods { get; set; }
    }
}
