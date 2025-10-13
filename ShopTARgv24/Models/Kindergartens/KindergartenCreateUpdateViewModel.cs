using Microsoft.AspNetCore.Http;

namespace ShopTARgv24.Models.Kindergartens
{
    public class KindergartenCreateUpdateViewModel
    {
        public Guid? Id { get; set; }
        public string? GroupName { get; set; }
        public int? ChildrenCount { get; set; }
        public string? KindergartenName { get; set; }
        public string? TeacherName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<IFormFile>? Files { get; set; }

        public KindergartenImageViewModel[] Image { get; set; } = Array.Empty<KindergartenImageViewModel>();
    }
}
