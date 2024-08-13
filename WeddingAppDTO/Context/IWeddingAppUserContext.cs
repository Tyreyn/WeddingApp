using Microsoft.EntityFrameworkCore;
using WeddingAppDTO.DataTransferObject;

namespace WeddingAppDTO.Context
{
    public interface IWeddingAppUserContext
    {
        DbSet<Picture> Pictures { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<PlannerComment> PlannerComments { get; set; }
        DbSet<Food> Foods { get; set; }
    }
}