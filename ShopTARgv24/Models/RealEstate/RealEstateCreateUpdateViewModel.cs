using System.ComponentModel.DataAnnotations;

namespace ShopTARgv24.Models.RealEstate
{
    public class RealEstateCreateUpdateViewModel
    {
        public Guid? Id { get; set; }

        [Range(1, Double.MaxValue, ErrorMessage = "Value cannot be negative")]
        public double? Area { get; set; }
        public string? Location { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Value cannot be negative")]
        public int? RoomNumber { get; set; }
        public string? BuildingType { get; set; }

        public List<IFormFile> Files { get; set; }
        public List<RealEstateImageViewModel> Image { get; set; }
            = new List<RealEstateImageViewModel>();

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}