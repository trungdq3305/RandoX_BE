using RandoX.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Interfaces
{
    public interface IManufacturerRepository
    {
        Task<Manufacturer> CreateManufacturerAsync(Manufacturer manufacturer);
        Task<IEnumerable<Manufacturer>> GetAllManufacturersAsync();
        Task<Manufacturer> GetManufacturerByIdAsync(string id);
        Task<Manufacturer> UpdateManufacturerAsync(Manufacturer manufacturer);
    }

}
