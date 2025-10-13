using Microsoft.AspNetCore.Http;
using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;

namespace ShopTARgv24.Core.ServiceInterface
{
    public interface IFileServices
    {
        // Сохранение файлов на диск + запись пути в БД (для Spaceship)
        Task FilesToApi(SpaceshipDto dto, Spaceship spaceship);

        // Старый метод (оставляем для совместимости): кладёт файлы в БД из DTO
        void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain);

        // Новый основной метод: кладёт файлы в БД для конкретного детсада
        Task SaveToDatabaseAsync(IEnumerable<IFormFile> files, Guid kindergartenId);

        // Удаление одной картинки по Id
        Task<FileToDatabase?> RemoveImageFromDatabase(FileToDatabaseDto dto);

        // Массовое удаление (перегрузки)
        Task RemoveImagesFromDatabase(IEnumerable<FileToDatabase> images);
        Task<FileToDatabase?> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos);
    }
}
