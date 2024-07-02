using Microsoft.EntityFrameworkCore;
using WeddingApp.Data.Entities;

namespace WeddingApp.Data.Context
{
    public interface IWeddingAppUserContext
    {
        DbSet<PictureEntity> Pictures { get; set; }
        DbSet<UserEntity> Users { get; set; }
    }
}