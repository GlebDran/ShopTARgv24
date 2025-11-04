using System.Threading.Tasks;

namespace ShopTARgv24.Core.ServiceInterface
{
    public interface IOpenWeatherService
    {
        Task<OpenWeatherDto?> GetCurrentAsync(string city);
    }

    public class OpenWeatherDto
    {
        public string City { get; set; }
        public string Country { get; set; }
        public double Temp { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}