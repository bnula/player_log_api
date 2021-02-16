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
    /// Endpoint to interact with Characters table of Player Log API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterRepository _charRepo;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public CharactersController(
            ICharacterRepository charRepo,
            ILoggerService logger,
            IMapper mapper)
        {
            _charRepo = charRepo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Character records
        /// </summary>
        /// <returns>List of Character records</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCharacters()
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                var items = await _charRepo.FindAll();
                var response = _mapper.Map<List<CharacterDTO>>(items);
                _logger.LogInfo($"{controllerName}: Success");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get a single Character record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Character record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCharacter(int id)
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
                if (!await _charRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _charRepo.FindByID(id);
                var response = _mapper.Map<CharacterDTO>(item);
                _logger.LogInfo($"{controllerName}: Success - Item ID: {id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Create a new Character record
        /// </summary>
        /// <param name="character"></param>
        /// <returns>Character record</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCharacter([FromBody] CharacterDTO itemDTO)
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
                var item = _mapper.Map<Character>(itemDTO);
                _logger.LogInfo($"{controllerName}: Attempting Create");
                var isSuccess = await _charRepo.Create(item);
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
        /// Update a Character record
        /// </summary>
        /// <param name="character"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCharacter([FromBody] CharacterDTO itemDTO, int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempting Call - Item ID: {id}");
                if (id < 1 || id != itemDTO.CharacterID)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - Item ID: {id}");
                    return BadRequest();
                }
                if (itemDTO == null)
                {
                    _logger.LogWarn($"{controllerName}: Empty Request Body");
                    return BadRequest();
                }
                if (!await _charRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = _mapper.Map<Character>(itemDTO);
                _logger.LogInfo($"{controllerName}: Attempting Update - Item ID: {id}");
                var isSuccess = await _charRepo.Update(item);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Update Failed - Item ID:{id}");
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
        /// Delete a Character Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCharacter(int id)
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
                if (!await _charRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _charRepo.FindByID(id);
                _logger.LogInfo($"{controllerName}: Attempting Delete - Item ID: {id}");
                var isSuccess = await _charRepo.Delete(item);
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

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;

            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError(message);
            return StatusCode(500, "Something went wrong, please contact the administrator");
        }
    }
}
