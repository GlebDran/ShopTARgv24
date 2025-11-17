using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;


namespace ShopTARgv24.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly ShopTARgv24Context _context;
        private readonly IFileServices _fileServices;

        public RealEstateServices
            (
                ShopTARgv24Context context,
                IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate domain = new RealEstate();

            domain.Id = Guid.NewGuid();
            domain.Area = dto.Area;
            domain.Location = dto.Location;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, domain);
            }

            await _context.RealEstate.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<RealEstate?> Update(RealEstateDto dto)
        {

            if (dto.Id == null || dto.Id == Guid.Empty)
                return null;

            var entity = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (entity == null)
                return null;

            entity.Area = dto.Area;
            entity.Location = dto.Location;
            entity.RoomNumber = dto.RoomNumber;
            entity.BuildingType = dto.BuildingType;
            entity.ModifiedAt = dto.ModifiedAt ?? DateTime.Now;

            await _context.SaveChangesAsync();

            return entity;
        }
        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var result = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<RealEstate?> Delete(Guid id)
        {
            var entity = await _context.RealEstate
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return null;

            var images = await _context.FileToDatabases
                 .Where(f => f.RealEstateId == id)
                 .ToListAsync();

            if (images.Count > 0)
                _context.FileToDatabases.RemoveRange(images);

            _context.RealEstate.Remove(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}