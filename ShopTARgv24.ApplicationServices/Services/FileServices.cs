using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARgv24Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices(ShopTARgv24Context context, IHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public async Task FilesToApi(SpaceshipDto dto, Spaceship spaceship)
        {
            if (dto?.Files == null || dto.Files.Count == 0)
                return;

            var uploadRoot = Path.Combine(_webHost.ContentRootPath, "multipleFileUpload");
            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            foreach (var file in dto.Files.Where(f => f != null && f.Length > 0))
            {
                var safeName = Path.GetFileName(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}_{safeName}";
                var filePath = Path.Combine(uploadRoot, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var path = new FileToApi
                {
                    Id = Guid.NewGuid(),
                    ExistingFilePath = uniqueFileName,
                    SpaceshipId = spaceship.Id
                };

                await _context.FileToApis.AddAsync(path);
            }

            await _context.SaveChangesAsync();
        }

        public async Task SaveToDatabaseAsync(IEnumerable<IFormFile> files, Guid kindergartenId)
        {
            if (files == null) return;

            foreach (var file in files.Where(f => f is { Length: > 0 }))
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);

                var entity = new FileToDatabase
                {
                    Id = Guid.NewGuid(),
                    KindergartenId = kindergartenId,
                    ImageTitle = Path.GetFileName(file.FileName),
                    ImageData = ms.ToArray()
                };

                await _context.KindergartenFileToDatabase.AddAsync(entity);
            }

            await _context.SaveChangesAsync();
        }

        public void UploadFilesToDatabase(KindergartenDto dto, Kindergarten domain)
        {
            if (dto?.Files == null || dto.Files.Count == 0) return;

            if (domain.Id is Guid kgId)
                SaveToDatabaseAsync(dto.Files, kgId).GetAwaiter().GetResult();
        }

        public async Task<FileToDatabase?> RemoveImageFromDatabase(FileToDatabaseDto dto)
        {
            var image = await _context.KindergartenFileToDatabase
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (image == null) return null;

            _context.KindergartenFileToDatabase.Remove(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task RemoveImagesFromDatabase(IEnumerable<FileToDatabase> images)
        {
            if (images == null) return;
            _context.KindergartenFileToDatabase.RemoveRange(images);
            await _context.SaveChangesAsync();
        }

        public async Task<FileToDatabase?> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos)
        {
            if (dtos == null || dtos.Length == 0) return null;

            foreach (var dto in dtos)
            {
                var image = await _context.KindergartenFileToDatabase
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                if (image != null)
                    _context.KindergartenFileToDatabase.Remove(image);
            }

            await _context.SaveChangesAsync();
            return null;
        }

        public async Task RemoveAllForKindergartenAsync(Guid kindergartenId)
        {
            var images = await _context.KindergartenFileToDatabase
                .Where(x => x.KindergartenId == kindergartenId)
                .ToListAsync();

            if (images.Count == 0) return;

            _context.KindergartenFileToDatabase.RemoveRange(images);
            await _context.SaveChangesAsync();
        }
    }
}