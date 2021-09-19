using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using FoodMvc.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace FoodMvc.Controllers
{
    public class APIProjectCallFromOtherMVCProjectCarController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public APIProjectCallFromOtherMVCProjectCarController(IHttpClientFactory _httpClientFactory)
        {
            this._httpClientFactory = _httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllCarFromAPIProject()
        {
            var httpClient = _httpClientFactory.CreateClient("carOfDotNetApi");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Car/GetAllCar");

            var response = await httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            //var carList = JsonConverter.DeserializeObject<List<Car>>(content);
            var carList = JsonSerializer.Deserialize<List<Car>>(content);

            return (IActionResult)carList;
        
        }

    }
}
