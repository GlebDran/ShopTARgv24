using System;
using System.Linq;
using System.Threading.Tasks;
using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;
using Xunit;

namespace ShopTARgv24.KindergartenTest
{
    public class KindergartenTest : TestBase
    {
        private KindergartenDto MockKindergartenData()
        {
            return new KindergartenDto
            {
                GroupName = "Butterflies",
                ChildrenCount = 20,
                KindergartenName = "Tallinn Kinder",
                TeacherName = "Mrs. Smith",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        private KindergartenDto MockUpdatedKindergartenData(Guid id)
        {
            return new KindergartenDto
            {
                Id = id,
                GroupName = "Ladybugs",
                ChildrenCount = 18,
                KindergartenName = "Tartu Kinder",
                TeacherName = "Mr. John",
                CreatedAt = DateTime.Now.AddDays(-1),
                UpdatedAt = DateTime.Now
            };
        }

        // 1)  Loomine — teenus peab tagastama mitte-null tulemuse

        [Fact]
        public async Task Should_CreateKindergarten_WhenReturnResult()
        {
            // Arrange
            var dto = MockKindergartenData();

            // Act
            var result = await Svc<IKindergartenServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
        }

        // 2) Päring ID järgi — pärast Create-käsku peame saama sama kirje

        [Fact]
        public async Task Should_GetByIdKindergarten_WhenReturnsEqual()
        {
            // Arrange
            var service = Svc<IKindergartenServices>();
            var dto = MockKindergartenData();

            var created = await service.Create(dto);

            // Act
            var fromDb = await service.DetailAsync((Guid)created.Id);

            // Assert
            Assert.NotNull(fromDb);
            Assert.Equal(created.Id, fromDb.Id);
            Assert.Equal(created.GroupName, fromDb.GroupName);
        }

        // 3) Uuendamine — väljad peavad muutuma

        [Fact]
        public async Task Should_UpdateKindergarten_WhenUpdateData()
        {
            // Arrange
            var service = Svc<IKindergartenServices>();

            var created = await service.Create(MockKindergartenData());
            var updateDto = MockUpdatedKindergartenData((Guid)created.Id);

            // Act
            var updated = await service.Update(updateDto);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(created.Id, updated.Id);
            Assert.Equal(updateDto.GroupName, updated.GroupName);
            Assert.Equal(updateDto.ChildrenCount, updated.ChildrenCount);
            Assert.Equal(updateDto.KindergartenName, updated.KindergartenName);
            Assert.Equal(updateDto.TeacherName, updated.TeacherName);
            Assert.Equal(updateDto.UpdatedAt, updated.UpdatedAt);
        }

        // 4) Kustutamine — Delete peab tagastama sama kirje, mis kustutati

        [Fact]
        public async Task Should_DeleteByIdKindergarten_WhenDeleteKindergarten()
        {
            // Arrange
            var service = Svc<IKindergartenServices>();
            var created = await service.Create(MockKindergartenData());
            var id = (Guid)created.Id;

            // Act
            var deleted = await service.Delete(id);

            // Assert
            Assert.NotNull(deleted);
            Assert.Equal(id, deleted.Id);
            Assert.Equal(created.GroupName, deleted.GroupName);
            Assert.Equal(created.ChildrenCount, deleted.ChildrenCount);
        }

        // 5) Lisakontroll: pärast kustutamist ei tohi ID järgi kirjet enam leida

        [Fact]
        public async Task Should_ReturnNull_When_GetDeletedKindergartenById()
        {
            // Arrange
            var service = Svc<IKindergartenServices>();
            var created = await service.Create(MockKindergartenData());
            var id = (Guid)created.Id;

            // Act
            await service.Delete(id);
            var result = await service.DetailAsync(id);

            // Assert
            Assert.Null(result);
        }

        // 6) Kindergarten'i kustutamisel peavad kustuma ka seotud pildid tabelist KindergartenFileToDatabase

        [Fact]
        public async Task Should_DeleteRelatedImages_WhenDeleteKindergarten()
        {
            // Arrange
            var service = Svc<IKindergartenServices>();
            var created = await service.Create(MockKindergartenData());
            var id = (Guid)created.Id;

            var db = Svc<ShopTARgv24Context>();

            db.KindergartenFileToDatabase.Add(new FileToDatabase
            {
                Id = Guid.NewGuid(),
                KindergartenId = id,
                ImageTitle = "group_photo.jpg",
                ImageData = new byte[] { 1, 2, 3 }
            });

            db.KindergartenFileToDatabase.Add(new FileToDatabase
            {
                Id = Guid.NewGuid(),
                KindergartenId = id,
                ImageTitle = "playground.jpg",
                ImageData = new byte[] { 4, 5, 6 }
            });

            await db.SaveChangesAsync();

            // Act
            await service.Delete(id);

            // Assert
            var leftovers = db.KindergartenFileToDatabase
                .Where(x => x.KindergartenId == id)
                .ToList();

            Assert.Empty(leftovers);
        }
    }
}