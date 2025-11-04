using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ShopTARgv24.Core.ServiceInterface;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class CocktailService : ICocktailService
    {
        private readonly HttpClient _http;

        public CocktailService(HttpClient http)
        {
            _http = http;
            if (_http.BaseAddress == null)
                _http.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");
        }

        public async Task<List<CocktailDto>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<CocktailDto>();

            var url = $"search.php?s={Uri.EscapeDataString(query)}";
            using var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            var root = JsonSerializer.Deserialize<Root>(json, JsonOptions());
            return Map(root);
        }

        public async Task<CocktailDto?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var url = $"lookup.php?i={Uri.EscapeDataString(id)}";
            using var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();
            var root = JsonSerializer.Deserialize<Root>(json, JsonOptions());
            var list = Map(root);
            return list.Count > 0 ? list[0] : null;
        }

        private static List<CocktailDto> Map(Root? root)
        {
            var result = new List<CocktailDto>();
            if (root?.drinks == null) return result;

            foreach (var d in root.drinks)
            {
                var dto = new CocktailDto
                {
                    Id = d.idDrink,
                    Name = d.strDrink,
                    Thumb = d.strDrinkThumb,
                    Category = d.strCategory,
                    Alcoholic = d.strAlcoholic,
                    Glass = d.strGlass,
                    Instructions = d.strInstructions
                };

                void add(string? m, string? i)
                {
                    if (!string.IsNullOrWhiteSpace(i))
                    {
                        dto.Ingredients.Add(string.IsNullOrWhiteSpace(m) ? i : $"{i} — {m}");
                    }
                }

                add(d.strMeasure1, d.strIngredient1);
                add(d.strMeasure2, d.strIngredient2);
                add(d.strMeasure3, d.strIngredient3);
                add(d.strMeasure4, d.strIngredient4);
                add(d.strMeasure5, d.strIngredient5);
                add(d.strMeasure6, d.strIngredient6);
                add(d.strMeasure7, d.strIngredient7);
                add(d.strMeasure8, d.strIngredient8);
                add(d.strMeasure9, d.strIngredient9);
                add(d.strMeasure10, d.strIngredient10);
                add(d.strMeasure11, d.strIngredient11);
                add(d.strMeasure12, d.strIngredient12);
                add(d.strMeasure13, d.strIngredient13);
                add(d.strMeasure14, d.strIngredient14);
                add(d.strMeasure15, d.strIngredient15);

                result.Add(dto);
            }

            return result;
        }

        private static JsonSerializerOptions JsonOptions() => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // ↓ минимальная модель под ответ API
        private class Root { public List<Drink>? drinks { get; set; } }
        private class Drink
        {
            public string idDrink { get; set; }
            public string strDrink { get; set; }
            public string strCategory { get; set; }
            public string strAlcoholic { get; set; }
            public string strGlass { get; set; }
            public string strInstructions { get; set; }
            public string strDrinkThumb { get; set; }

            public string strIngredient1 { get; set; }
            public string strMeasure1 { get; set; }
            public string strIngredient2 { get; set; }
            public string strMeasure2 { get; set; }
            public string strIngredient3 { get; set; }
            public string strMeasure3 { get; set; }
            public string strIngredient4 { get; set; }
            public string strMeasure4 { get; set; }
            public string strIngredient5 { get; set; }
            public string strMeasure5 { get; set; }
            public string strIngredient6 { get; set; }
            public string strMeasure6 { get; set; }
            public string strIngredient7 { get; set; }
            public string strMeasure7 { get; set; }
            public string strIngredient8 { get; set; }
            public string strMeasure8 { get; set; }
            public string strIngredient9 { get; set; }
            public string strMeasure9 { get; set; }
            public string strIngredient10 { get; set; }
            public string strMeasure10 { get; set; }
            public string strIngredient11 { get; set; }
            public string strMeasure11 { get; set; }
            public string strIngredient12 { get; set; }
            public string strMeasure12 { get; set; }
            public string strIngredient13 { get; set; }
            public string strMeasure13 { get; set; }
            public string strIngredient14 { get; set; }
            public string strMeasure14 { get; set; }
            public string strIngredient15 { get; set; }
            public string strMeasure15 { get; set; }
        }
    }
}