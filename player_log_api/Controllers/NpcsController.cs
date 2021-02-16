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
    /// Endpoint for interacting with Npc table of Player Log API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NpcsController : ControllerBase
    {
        private readonly INpcRepository _npcRepo;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public NpcsController(
            INpcRepository npcRepo,
            ILoggerService logger,
            IMapper mapper)
        {
            _npcRepo = npcRepo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Npc records
        /// </summary>
        /// <returns>List of Npc records</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNpcs()
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                var items = await _npcRepo.FindAll();
                var response = _mapper.Map<List<NpcDTO>>(items);
                _logger.LogInfo($"{controllerName}: Success");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }
        
        /// <summary>
        /// Get an Npc record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Npc record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNpc(int id)
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
                if (!await _npcRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _npcRepo.FindByID(id);
                var response = _mapper.Map<NpcDTO>(item);
                _logger.LogInfo($"{controllerName}: Call Successful - Item ID: {id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Create a new Npc record
        /// </summary>
        /// <param name="npc"></param>
        /// <returns>Npc record</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNpc([FromBody] NpcDTO itemDTO)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                if (itemDTO == null)
                {
                    _logger.LogWarn($"{controllerName}: Empty Request Body");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{controllerName}: Invalid Data");
                    return BadRequest(ModelState);
                }

                var item = _mapper.Map<Npc>(itemDTO);

                _logger.LogInfo($"{controllerName}: Attempted Create");

                var isSuccess = await _npcRepo.Create(item);
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
        /// Update an Npc record
        /// </summary>
        /// <param name="npc"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateNpc([FromBody] NpcDTO itemDTO, int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call - Item ID: {id}");
                if (id < 1 || id != itemDTO.NpcID)
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
                    _logger.LogWarn($"{controllerName}: Invalid Data - Item ID: {id}");
                    return BadRequest(ModelState);
                }
                if (!await _npcRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }

                var item = _mapper.Map<Npc>(itemDTO);
                _logger.LogInfo($"{controllerName}: Attempting Update - Item ID: {id}");
                var isSuccess = await _npcRepo.Update(item);

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
        /// Delete an Npc record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteNpc(int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempting Call");
                if (id < 1)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - Item ID: {id}");
                    return BadRequest();
                }
                if (!await _npcRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _npcRepo.FindByID(id);
                _logger.LogInfo($"{controllerName}: Attempting Delete - Item ID: {id}");
                var isSuccess = await _npcRepo.Delete(item);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Delete Failed - Item ID: {id}");
                }
                _logger.LogInfo($"{controllerName}: Delete Successful - Item ID: {id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong, contact administrator.");
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }
    }
}
