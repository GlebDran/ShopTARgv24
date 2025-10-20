namespace ShopTARgv24.Core.Dto
{
    public class AccuLocationWeatherResultDto
    {
        public string CityName { get; set; } = string.Empty;

        public string LocalObservationDateTime { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public double TempMetricValueUnit { get; set; }
    }
}