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
    /// Endpoint to interact with Armies table of Player Log API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ArmiesController : ControllerBase
    {
        private readonly IArmyRepository _armyRepo;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public ArmiesController(
            IArmyRepository armyRepo,
            ILoggerService logger,
            IMapper mapper)
        {
            _armyRepo = armyRepo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Army records
        /// </summary>
        /// <returns>List of Army records</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArmies()
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                var items = await _armyRepo.FindAll();
                var response = _mapper.Map<List<ArmyDTO>>(items);
                _logger.LogInfo($"{controllerName}: Success");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get an Army record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Army record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArmy(int id)
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
                if (!await _armyRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _armyRepo.FindByID(id);
                var response = _mapper.Map<ArmyDTO>(item);
                _logger.LogInfo($"{controllerName}: Success - Item ID: {id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Create a new Army record
        /// </summary>
        /// <param name="army"></param>
        /// <returns>Army record</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateArmy([FromBody] ArmyDTO itemDTO)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                if (itemDTO == null)
                {
                    _logger.LogWarn($"{controllerName}: Empty Request Body");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{controllerName}: Invalid Data");
                    return BadRequest(ModelState);
                }
                var item = _mapper.Map<Army>(itemDTO);
                _logger.LogInfo($"{controllerName}: Attempted Create");
                var isSuccess = await _armyRepo.Create(item);
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
        /// Update an Army record
        /// </summary>
        /// <param name="army"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateArmy([FromBody] ArmyDTO itemDTO, int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call - Item ID: {id}");
                if (id < 1 || id != itemDTO.ArmyID)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - Item ID: {id}");
                    return BadRequest();
                }
                if (itemDTO == null)
                {
                    _logger.LogWarn($"{controllerName}: Empty Request Body");
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{controllerName}: Invalid Data");
                    return BadRequest(ModelState);
                }
                if (!await _armyRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = _mapper.Map<Army>(itemDTO);
                _logger.LogInfo($"{controllerName}: Attempted Update - Item ID: {id}");
                var isSuccess = await _armyRepo.Update(item);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Update Failed - Item ID: {id}");
                }
                _logger.LogInfo($"{controllerName}: Update Successful - Item ID: {id}");
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Delete an Aremy record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteArmy(int id)
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
                if (!await _armyRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _armyRepo.FindByID(id);
                _logger.LogInfo($"{controllerName}: Attempted Delete - Item ID: {id}");
                var isSuccess = await _armyRepo.Delete(item);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Delete Failed - Item ID: {id}");
                }
                _logger.LogInfo($"{controllerName}: Delete successful - Item ID: {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
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
            return StatusCode(500, "Something went wrong, please contact the administrator.");
        }
    }
}
