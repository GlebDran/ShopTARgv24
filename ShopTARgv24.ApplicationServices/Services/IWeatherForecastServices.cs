using System;
using ShopTARgv24.Core.Dto;

namespace ShopTARgv24.ApplicationServices.Services
{
    internal interface IWeatherForecastServices
    {
        Task<AccuLocationWeatherResultDto> AccuWeatherResult(AccuLocationWeatherResultDto dto);
    }
}