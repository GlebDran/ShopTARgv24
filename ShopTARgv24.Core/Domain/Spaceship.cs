using ShopTARgv24.Core.Domain; // Убедись, что этот using есть

namespace ShopTARgv24.Core.Domain
{
    public class Spaceship
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? TypeName { get; set; }
        public DateTime? BuiltDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }
        public int? Passengers { get; set; }
        public int? InnerVolume { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // ДОБАВЬ ЭТУ СТРОКУ
        public ICollection<FileToApi> Files { get; set; } = new List<FileToApi>();
    }
}