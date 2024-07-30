using Microsoft.EntityFrameworkCore;
using WeddingAppDTO.DataTransferObject;

namespace WeddingAppDTO.Context
{
    public class WeddingAppUserContext : DbContext, IWeddingAppUserContext
    {
        public WeddingAppUserContext(DbContextOptions<WeddingAppUserContext> options) : base(options) { }

        public DbSet<UserDto> Users { get; set; }

        public DbSet<PictureDto> Pictures { get; set; }
    }
}
