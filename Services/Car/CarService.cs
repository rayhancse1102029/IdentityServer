using CARAPI.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CARAPI.Services.Car
{
    public class CarService : ICarService
    {
        private readonly CarApiDbContext _context;

        public CarService(CarApiDbContext _context)
        {
            this._context = _context;
        }

        #region Car
        public async Task<bool> SaveCar(Data.Entity.Car obj)
        {
            if (obj.Id != 0)
            {
                _context.Cars.Update(obj);
                await _context.SaveChangesAsync();
                return true;
            }
            await _context.Cars.AddAsync(obj);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<Data.Entity.Car>> GetAllCar()
        {
            return await _context.Cars.AsNoTracking().ToListAsync();
        }
        public async Task<Data.Entity.Car> GetCarById(int id)
        {
            return await _context.Cars.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteCarById(int id)
        {
            _context.Cars.Remove(_context.Cars.Find(id));
            await _context.SaveChangesAsync();
            return true;
        }
        #endregion
    }
}
