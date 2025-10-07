using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;

namespace ShopTARgv24.Core.ServiceInterface
{
    public interface IFileToDatabaseServices
    {
        void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain);
    }
}