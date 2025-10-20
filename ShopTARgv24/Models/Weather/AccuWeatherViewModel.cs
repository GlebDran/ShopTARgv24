namespace ShopTARgv24.Models.Weather
{
    public class AccuWeatherViewModel
    {

        public string LocalObservationDateTime { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public double TempMetricValueUnit { get; set; }
    }
}