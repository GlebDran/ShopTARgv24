namespace ShopTARgv24.Core.Dto
{
    public class AccuLocationWeatherResultDto
    {
        public string CityName { get; set; } = string.Empty;

        public DateTime EndDate { get; set; }
        public string Text { get; set; } = string.Empty;
        public double TempMetricValueUnit { get; set; }
    }
}