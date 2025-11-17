namespace ShopTARgv24.Core.Domain
{
    public class RealEstate
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Location { get; set; }
        public int? RoomNumber { get; set; }
        public string? BuildingType { get; set; }

        public ICollection<FileToDatabase> Files { get; set; } = new List<FileToDatabase>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}