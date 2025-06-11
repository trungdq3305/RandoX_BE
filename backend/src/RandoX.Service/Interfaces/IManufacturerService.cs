using RandoX.Common;
using RandoX.Data.Entities;
using RandoX.Data.Models;
using RandoX.Data.Models.ManufacturerModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Interfaces
{
    public interface IManufacturerService
    {
        Task<ApiResponse<PaginationResult<Manufacturer>>> GetAllManufacturersAsync(int pageNumber, int pageSize);
        Task<ApiResponse<Manufacturer>> GetManufacturerByIdAsync(string id);
        Task<ApiResponse<Manufacturer>> CreateManufacturerAsync(ManufacturerRequest manufacturer);
        Task<ApiResponse<Manufacturer>> UpdateManufacturerAsync(string id, ManufacturerRequest manufacturer);
        Task<ApiResponse<Manufacturer>> DeleteManufacturerAsync(string id);
    }
}
