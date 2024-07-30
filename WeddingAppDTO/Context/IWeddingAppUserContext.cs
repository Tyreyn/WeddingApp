using Microsoft.EntityFrameworkCore;
using WeddingAppDTO.DataTransferObject;

namespace WeddingAppDTO.Context
{
    public interface IWeddingAppUserContext
    {
        DbSet<PictureDto> Pictures { get; set; }
        DbSet<UserDto> Users { get; set; }
    }
}