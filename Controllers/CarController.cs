using CARAPI.Data;
using CARAPI.Services.Car;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CARAPI.Controllers
{
    [Authorize]
    //[Authorize("ClinetCredentialPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {

        private readonly ICarService _carService;

        public CarController(ICarService _carService)
        {
            this._carService = _carService;
        }

        [Route("GetAllCar")]
        [HttpGet]
        public async Task<IEnumerable<Data.Entity.Car>> GetAllCar()
        {
            return await _carService.GetAllCar();
        }

        [Route("GetCarById")]
        [HttpGet]
        public async Task<Data.Entity.Car> GetCarById(int id)
        {
            return await _carService.GetCarById(id);
        }

        [Route("SaveCar")]
        [HttpPost]
        public async Task<bool> SaveCar([FromBody] Data.Entity.Car obj)
        {
           return  await _carService.SaveCar(obj);
        }

        [Route("DeleteCar")]
        [HttpDelete]
        public async Task<bool> DeleteCar(int id)
        {
            return await _carService.DeleteCarById(id);
        }
    }
}
