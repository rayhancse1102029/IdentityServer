using CARAPI.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CARAPI.Services.Car
{
    public interface ICarService
    {
        #region Car
        Task<bool> SaveCar(Data.Entity.Car obj);
        Task<IEnumerable<Data.Entity.Car>> GetAllCar();
        Task<Data.Entity.Car> GetCarById(int id);
        Task<bool> DeleteCarById(int id);
        #endregion
    }
}
