using RandoX.Common;
using RandoX.Data.Entities;
using RandoX.Data.Interfaces;
using RandoX.Data.Models;
using RandoX.Data.Models.ManufacturerModel;
using RandoX.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Service.Services
{
    public class ManufacturerService : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerService(IManufacturerRepository manufacturerRepository)
        {
            _manufacturerRepository = manufacturerRepository;
        }

        public async Task<ApiResponse<PaginationResult<Manufacturer>>> GetAllManufacturersAsync(int pageNumber, int pageSize)
        {
            try
            {
                var manufacturers = await _manufacturerRepository.GetAllManufacturersAsync();
                var paginatedResult = new PaginationResult<Manufacturer>(manufacturers.ToList(), manufacturers.Count(), pageNumber, pageSize);
                return ApiResponse<PaginationResult<Manufacturer>>.Success(paginatedResult, "success");
            }
            catch (Exception)
            {
                return ApiResponse<PaginationResult<Manufacturer>>.Failure("Fail to get manufacturers");
            }
        }

        public async Task<ApiResponse<Manufacturer>> GetManufacturerByIdAsync(string id)
        {
            try
            {
                var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(id);
                return ApiResponse<Manufacturer>.Success(manufacturer, "success");
            }
            catch (Exception)
            {
                return ApiResponse<Manufacturer>.Failure("Fail to get manufacturer");
            }
        }

        public async Task<ApiResponse<Manufacturer>> CreateManufacturerAsync(ManufacturerRequest manufacturerRequest)
        {
            try
            {
                Manufacturer manufacturer = new Manufacturer
                {
                    Id = Guid.NewGuid().ToString(), 
                    ManufacturerName = manufacturerRequest.ManufacturerName
                };

                await _manufacturerRepository.CreateManufacturerAsync(manufacturer);

                return ApiResponse<Manufacturer>.Success(manufacturer, "Manufacturer created successfully");
            }
            catch (Exception)
            {
                return ApiResponse<Manufacturer>.Failure("Fail to create manufacturer");
            }
        }

        public async Task<ApiResponse<Manufacturer>> UpdateManufacturerAsync(string id, ManufacturerRequest manufacturerRequest)
        {
            try
            {
                var manufacturer = await _manufacturerRepository.GetManufacturerByIdAsync(id);
                if (manufacturer == null)
                {
                    return ApiResponse<Manufacturer>.Failure("Manufacturer not found");
                }

                manufacturer.ManufacturerName = manufacturerRequest.ManufacturerName;

                await _manufacturerRepository.UpdateManufacturerAsync(manufacturer);

                return ApiResponse<Manufacturer>.Success(manufacturer, "Manufacturer updated successfully");
            }
            catch (Exception)
            {
                return ApiResponse<Manufacturer>.Failure("Fail to update manufacturer");
            }
        }
        public async Task<ApiResponse<Manufacturer>> DeleteManufacturerAsync(string id)
        {
            try
            {
                var ma = await _manufacturerRepository.GetManufacturerByIdAsync(id);
                ma.IsDeleted = 1;
                ma.DeletedAt = DateTime.Now;
                await _manufacturerRepository.UpdateManufacturerAsync(ma);
                return ApiResponse<Manufacturer>.Success(ma, "Manufacturer delete successfully");
            }
            catch (Exception)
            {
                return ApiResponse<Manufacturer>.Failure("Fail to delete manufacturer");
            }
        }
    }

}
