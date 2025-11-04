using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopTARgv24.Core.ServiceInterface
{
    public interface ICocktailService
    {
        Task<List<CocktailDto>> SearchAsync(string query);
        Task<CocktailDto?> GetByIdAsync(string id);
    }

    public class CocktailDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Thumb { get; set; }
        public string Category { get; set; }
        public string Alcoholic { get; set; }
        public string Glass { get; set; }
        public string Instructions { get; set; }
        public List<string> Ingredients { get; set; } = new();
    }
}