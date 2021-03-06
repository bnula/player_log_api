using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using player_log_api.Contracts;
using player_log_api.Data;
using player_log_api.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Controllers
{
    /// <summary>
    /// Endpoint for interacting with Location Table in PLayer Log API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationRepository _locRepo;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public LocationsController(
            ILocationRepository locRepo,
            ILoggerService logger,
            IMapper mapper)
        {
            _locRepo = locRepo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Location records
        /// </summary>
        /// <returns>List of Location records</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLocations()
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attemted Call");
                var items = await _locRepo.FindAll();
                var response = _mapper.Map<List<LocationDTO>>(items);
                _logger.LogInfo($"{controllerName}: Success");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get a single Location record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a Location record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLocation(int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                if (id < 1)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - Item ID: {id}");
                    return BadRequest();
                }
                var item = await _locRepo.FindByID(id);
                if (item == null)
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }

                var response = _mapper.Map<LocationDTO>(item);
                _logger.LogInfo($"{controllerName}: Success - Item ID: {id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Create a new Location record
        /// </summary>
        /// <param name="location"></param>
        /// <returns>Location record</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateLocation([FromBody] UpsertLocationDTO itemDTO)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                if (itemDTO == null)
                {
                    _logger.LogWarn($"{controllerName}: Empty Request");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{controllerName}: Invalid Data");
                    return BadRequest(ModelState);
                }
                _logger.LogInfo($"{controllerName}: Attempted Create");
                var item = _mapper.Map<Location>(itemDTO);
                var isSuccess = await _locRepo.Create(item);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Create Failed");
                }
                _logger.LogInfo($"{controllerName}: Create Successful");
                return Created("Create", new { item });
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update an existing Location record
        /// </summary>
        /// <param name="location"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateLocation([FromBody] UpsertLocationDTO itemDTO, int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call - Item ID: {id}");
                if (id < 1 || itemDTO == null || id != itemDTO.LocationID)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - Item ID: {id}");
                    return BadRequest();
                }
                if (!await _locRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{controllerName}: Invalid Data - Item ID: {id}");
                    return BadRequest(ModelState);
                }
                var item = _mapper.Map<Location>(itemDTO);
                _logger.LogInfo($"{controllerName}: Attempted Update - Item ID: {id}");
                var isSuccess = await _locRepo.Update(item);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Update Failed - Item ID: {id}");
                }
                _logger.LogInfo($"{controllerName}: Update Successful - Item ID: {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Delete a Location record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call - Item ID: {id}");
                if (id < 1)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - Item ID: {id}");
                    return BadRequest();
                }
                if (!await _locRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _locRepo.FindByID(id);
                _logger.LogInfo($"{controllerName}: Attempted Delete - Item ID: {id}");
                var isSuccess = await _locRepo.Delete(item);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Delete Failed - Item ID: {id}");
                }
                _logger.LogInfo($"{controllerName}: Delete Successfull - Item ID: {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{ex.Message} - {ex.InnerException}");
            }
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong, contact the administrator");
        }
    }
}
