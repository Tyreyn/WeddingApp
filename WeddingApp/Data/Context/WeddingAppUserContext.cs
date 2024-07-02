using Microsoft.EntityFrameworkCore;
using WeddingApp.Data.Entities;

namespace WeddingApp.Data.Context
{
    public class WeddingAppUserContext : DbContext, IWeddingAppUserContext
    {
        public WeddingAppUserContext(DbContextOptions<WeddingAppUserContext> options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<PictureEntity> Pictures { get; set; }
    }
}
