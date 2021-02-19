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
    /// Endpoint used to interact with Quests table of Player Log API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuestsController : ControllerBase
    {
        private readonly IQuestRepository _questRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        public QuestsController(
            IQuestRepository questRepo,
            IMapper mapper,
            ILoggerService logger)
        {
            _questRepo = questRepo;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get a list of all Quest records
        /// </summary>
        /// <returns>a list of Quest records</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuests()
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                var items = await _questRepo.FindAll();
                var response = _mapper.Map<List<QuestDTO>>(items);
                _logger.LogInfo($"{controllerName}: Success!");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Get a Quest record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Quest record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetQuest(int id)
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
                if (!await _questRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _questRepo.FindByID(id);
                _logger.LogInfo($"{controllerName}: Success - Item ID: {id}");
                var response = _mapper.Map<Quest>(item);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Create a new Quest record
        /// </summary>
        /// <param name="quest"></param>
        /// <returns>Quest record</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateQuest([FromBody] QuestDTO itemDTO)
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
                    return BadRequest();
                }
                var item = _mapper.Map<Quest>(itemDTO);
                _logger.LogInfo($"{controllerName}: Attempted Create");
                var isSuccess = await _questRepo.Create(item);
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
        /// Update a Quest record
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateQuest([FromBody] QuestDTO itemDTO, int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call - Item ID: {id}");
                if (id < 1 || id != itemDTO.QuestID)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - Item ID: {id}");
                    return BadRequest();
                }
                if (itemDTO == null)
                {
                    _logger.LogWarn($"{controllerName}: Empty Request Body");
                    return BadRequest();
                }
                if (!await _questRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{controllerName}: Invalid Request Data");
                    return BadRequest();
                }
                var item = _mapper.Map<Quest>(itemDTO);
                _logger.LogInfo($"{controllerName}: Attempted Update - Item ID: {id}");
                var isSuccess = await _questRepo.Update(item);
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
        /// Delete a Quest record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteQuest(int id)
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
                if (!await _questRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }
                var item = await _questRepo.FindByID(id);
                _logger.LogInfo($"{controllerName}: Attempted Delte - Item ID: {id}");
                var isSuccess = await _questRepo.Delete(item);
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
            return StatusCode(500, "Something went wrong, please contact administrator.");
        }
    }
}
