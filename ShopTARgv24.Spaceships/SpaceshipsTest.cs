using System.Globalization;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;

namespace ShopTARgv24.SpaceShipsTest
{
    public class SpaceShipsTest : TestBase
    {
        private SpaceshipDto SpaceshipDto1()
        {
            return new SpaceshipDto
            {
                Name = "Unit-1",
                TypeName = "R45",
                BuiltDate = DateTime.ParseExact("15.01.2020", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Crew = 5,
                EnginePower = 50,
                Passengers = 25,
                InnerVolume = 450,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

        private SpaceshipDto SpaceshipDto2()
        {
            return new SpaceshipDto
            {
                Name = "Unit-2",
                TypeName = "X99",
                BuiltDate = DateTime.ParseExact("20.02.2021", "dd.MM.yyyy", CultureInfo.InvariantCulture),
                Crew = 10,
                EnginePower = 120,
                Passengers = 40,
                InnerVolume = 900,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };
        }

        // 1) Create

        [Fact]
        public async Task Should_CreateSpaceship_WhenReturnResult()
        {
            // Arrange
            var dto = SpaceshipDto1();

            // Act
            var result = await Svc<ISpaceshipsServices>().Create(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
        }

        // 2) Get by id

        [Fact]
        public async Task Should_GetByIdSpaceship_WhenReturnsEqual()
        {
            // Arrange
            var service = Svc<ISpaceshipsServices>();
            var dto = SpaceshipDto1();

            var created = await service.Create(dto);

            // Act
            var fromDb = await service.DetailAsync((Guid)created.Id);

            // Assert
            Assert.NotNull(fromDb);
            Assert.Equal(created.Id, fromDb.Id);
        }

        // 3) Delete

        [Fact]
        public async Task Should_DeleteByIdSpaceship_WhenDeleteSpaceship()
        {
            // Arrange
            var service = Svc<ISpaceshipsServices>();
            var dto = SpaceshipDto1();

            var created = await service.Create(dto);

            // Act
            await service.Delete((Guid)created.Id);
            var afterDelete = await service.DetailAsync((Guid)created.Id);

            // Assert
            Assert.Null(afterDelete);
        }

        // 4) An attempt to delete using an incorrect ID should not affect the existing ship

        [Fact]
        public async Task ShouldNot_DeleteByIdSpaceship_WhenWrongId()
        {
            // Arrange
            var service = Svc<ISpaceshipsServices>();
            var dto = SpaceshipDto1();

            var created = await service.Create(dto);
            var wrongId = Guid.NewGuid();

            // Act
            try
            {
                await service.Delete(wrongId);
            }
            catch
            {

            }

            var stillExists = await service.DetailAsync((Guid)created.Id);

            // Assert
            Assert.NotNull(stillExists);
            Assert.Equal(created.Id, stillExists.Id);
        }

        // 5) Each Create operation must generate a unique ID

        [Fact]
        public async Task Should_AssignUniqueIds_When_CreateMultipleSpaceships()
        {
            // Arrange
            var service = Svc<ISpaceshipsServices>();
            var dto1 = SpaceshipDto1();
            var dto2 = SpaceshipDto2();

            // Act
            var s1 = await service.Create(dto1);
            var s2 = await service.Create(dto2);

            // Assert
            Assert.NotNull(s1);
            Assert.NotNull(s2);

            Assert.NotEqual(s1.Id, s2.Id);
            Assert.NotEqual(Guid.Empty, s1.Id);
            Assert.NotEqual(Guid.Empty, s2.Id);
        }
    }
}