using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandoX.Data.Entities;
using RandoX.Data.Models.ManufacturerModel;
using RandoX.Service.Interfaces;

namespace RandoX.API.Controllers
{
    [Authorize]
    public class ManufacturerController : BaseAPIController
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllManufacturers(int pageNumber, int pageSize)
        {
            var response = await _manufacturerService.GetAllManufacturersAsync(pageNumber, pageSize);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetManufacturerById(string id)
        {
            var response = await _manufacturerService.GetManufacturerByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateManufacturer([FromBody] ManufacturerRequest manufacturerRequest)
        {
            var response = await _manufacturerService.CreateManufacturerAsync(manufacturerRequest);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateManufacturer(string id, [FromBody] ManufacturerRequest manufacturerRequest)
        {
            var response = await _manufacturerService.UpdateManufacturerAsync(id, manufacturerRequest);
            return Ok(response);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteManufacturer(string id)
        {
            var response = await _manufacturerService.DeleteManufacturerAsync(id);
            return Ok(response);
        }
    }

}
