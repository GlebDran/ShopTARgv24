using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;
using ShopTARgv24.Core.Domain;
using Microsoft.AspNetCore.Http; // Для IFormFile

namespace ShopTARgv24.RealEstateTest
{
    public class SpaceshipsTest : TestBase
    {
        // --- Вспомогательные методы для Mock данных ---

        /// <summary>
        /// Создает DTO с базовыми данными для нового корабля.
        /// </summary>
        private SpaceshipDto MockSpaceshipData()
        {
            return new SpaceshipDto
            {
                Name = "Millennium Falcon",
                TypeName = "Light Freighter",
                BuiltDate = new DateTime(1977, 5, 25),
                Crew = 4,
                EnginePower = 10000,
                Passengers = 6,
                InnerVolume = 200,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Files = new List<IFormFile>(), // Важно инициализировать списки
                FileToApiDtos = new List<FileToApiDto>()
            };
        }

        /// <summary>
        /// Создает DTO с данными для обновления существующего корабля.
        /// </summary>
        private SpaceshipDto MockUpdateSpaceshipData(Guid id)
        {
            return new SpaceshipDto
            {
                Id = id,
                Name = "X-Wing",
                TypeName = "Starfighter",
                BuiltDate = new DateTime(1980, 5, 21),
                Crew = 1,
                EnginePower = 12000,
                Passengers = 0,
                InnerVolume = 50,
                ModifiedAt = DateTime.Now.AddDays(1) // Обновляем дату
            };
        }


        // --- 5 ТЕСТОВ ДЛЯ SPACESHIPS ---

        /// <summary>
        /// Тест 1: Проверяет, что корабль успешно создается с валидными данными.
        /// </summary>
        [Fact]
        public async Task Should_CreateSpaceship_WhenValidData()
        {
            // Arrange (Подготовка)
            var dto = MockSpaceshipData();

            // Act (Действие)
            // Вызываем сервис <ISpaceshipsServices> и его метод Create
            var result = await Svc<ISpaceshipsServices>().Create(dto);

            // Assert (Проверка)
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Crew, result.Crew);
            Assert.NotEqual(Guid.Empty, result.Id); // Убедимся, что ID был присвоен
        }

        /// <summary>
        /// Тест 2: Проверяет, что мы можем получить созданный корабль по его ID.
        /// </summary>
        [Fact]
        public async Task Should_GetSpaceshipById_WhenReturnsEqual()
        {
            // Arrange
            var dto = MockSpaceshipData();
            var created = await Svc<ISpaceshipsServices>().Create(dto);
            var id = (Guid)created.Id;

            // Act
            var result = await Svc<ISpaceshipsServices>().DetailAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal(dto.Name, result.Name);
        }

        /// <summary>
        /// Тест 3: Проверяет, что корабль удаляется и его нельзя получить снова.
        /// </summary>
        [Fact]
        public async Task Should_DeleteSpaceshipById_WhenDelete()
        {
            // Arrange
            var dto = MockSpaceshipData();
            var created = await Svc<ISpaceshipsServices>().Create(dto);
            var id = (Guid)created.Id;

            // Act
            // Удаляем корабль
            var deleted = await Svc<ISpaceshipsServices>().Delete(id);
            // Пытаемся получить его снова из базы данных
            var result = await Svc<ISpaceshipsServices>().DetailAsync(id);

            // Assert
            Assert.Equal(created.Id, deleted.Id); // Проверяем, что метод Delete вернул удаленный объект
            Assert.Null(result); // Проверяем, что DetailAsync не нашел объект (он удален)
        }

        /// <summary>
        /// Тест 4: Проверяет, что данные корабля успешно обновляются.
        /// </summary>
        [Fact]
        public async Task Should_UpdateSpaceship_WhenUpdateData()
        {
            // Arrange
            var dto = MockSpaceshipData();
            var created = await Svc<ISpaceshipsServices>().Create(dto);
            var id = (Guid)created.Id;
            var updateDto = MockUpdateSpaceshipData(id); // Используем mock для обновления

            // Act
            var updated = await Svc<ISpaceshipsServices>().Update(updateDto);
            // Получаем обновленные данные из базы
            var result = await Svc<ISpaceshipsServices>().DetailAsync(id);

            // Assert
            Assert.NotNull(updated);
            Assert.NotNull(result);
            Assert.Equal(id, updated.Id);
            Assert.Equal(updateDto.Name, result.Name); // Имя должно обновиться
            Assert.Equal("Starfighter", result.TypeName); // Тип должен обновиться
            Assert.NotEqual(dto.Name, result.Name); // Старого имени быть не должно
        }

        /// <summary>
        /// Тест 5: Проверяет, что при создании нескольких кораблей им присваиваются уникальные ID.
        /// (Скопировано из RealEstateTest)
        /// </summary>
        [Fact]
        public async Task Should_AssignUniqueIds_When_CreateMultiple()
        {
            // Arrange
            var dto1 = MockSpaceshipData();
            dto1.Name = "Discovery One"; // Даем уникальные имена для ясности
            var dto2 = MockSpaceshipData();
            dto2.Name = "Nostromo";

            // Act
            var s1 = await Svc<ISpaceshipsServices>().Create(dto1);
            var s2 = await Svc<ISpaceshipsServices>().Create(dto2);

            // Assert
            Assert.NotNull(s1);
            Assert.NotNull(s2);
            Assert.NotEqual(s1.Id, s2.Id); // Самая главная проверка - ID не равны
            Assert.NotEqual(Guid.Empty, s1.Id);
            Assert.NotEqual(Guid.Empty, s2.Id);
        }
    }
}