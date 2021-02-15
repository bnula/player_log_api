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
    /// Endpoint to interact with the Campaigns table in Player Log API
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignRepository _campRepo;
        private readonly IArmyRepository _armyRepo;
        private readonly ICharacterRepository _charRepo;
        private readonly ILocationRepository _locRepo;
        private readonly INpcRepository _npcRepo;
        private readonly IQuestRepository _questRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;
        public CampaignsController(
            ICampaignRepository campRepo,
            IArmyRepository armyrepo,
            ICharacterRepository charRepo,
            ILocationRepository locRepo,
            INpcRepository npcRepo,
            IQuestRepository questRepo,
            IMapper mapper,
            ILoggerService logger
            )
        {
            _campRepo = campRepo;
            _armyRepo = armyrepo;
            _charRepo = charRepo;
            _locRepo = locRepo;
            _npcRepo = npcRepo;
            _questRepo = questRepo;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all Campaign records
        /// </summary>
        /// <returns>List of Campaign records</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCampaigns()
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call");
                var items = await _campRepo.FindAll();
                var response = _mapper.Map<List<CampaignDTO>>(items);
                _logger.LogInfo($"{controllerName}: Success");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }   
        }

        /// <summary>
        /// Get a single Campaign record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Campaign record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCampaign(int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call - Item ID: {id}");
                var item = await _campRepo.FindByID(id);
                if (item == null)
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Campaign ID: {id}");
                    return NotFound();
                }
                var response = _mapper.Map<CampaignDTO>(item);
                _logger.LogInfo($"{controllerName}: Success - Campaign ID: {id}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }

        }

        /// <summary>
        /// Creates a new Campaign item
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns>Campaign Item</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCampaign([FromBody] CampaignDTO itemDTO)
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
                var item = _mapper.Map<Campaign>(itemDTO);
                var isSuccess = await _campRepo.Create(item);
                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Item Creation Failed");
                }
                _logger.LogInfo($"{controllerName}: Item Created");
                return Created("Create", new { item });
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Update a Campaign Item
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCampaign([FromBody] CampaignDTO itemDTO, int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call - Item ID: {id}");
                if (itemDTO == null || id < 1 || id != itemDTO.CampaignID)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - ItemID: {id}");
                    return BadRequest();
                }

                if (!await _campRepo.RecordExistsByID(id))
                {
                    _logger.LogWarn($"{controllerName}: Not Found - ItemID:{id}");
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{controllerName}: Invalid Data - ItemID: {id}");
                    return BadRequest(ModelState);
                }

                _logger.LogInfo($"{controllerName}: Attempted Update - ItemID:{id}");
                var item = _mapper.Map<Campaign>(itemDTO);
                var isSuccess = await _campRepo.Update(item);

                if (!isSuccess)
                {
                    return InternalError($"{controllerName}: Update Failed - ItemID: {id}");
                }

                _logger.LogInfo($"{controllerName}: Update Successful - ItemID:{id}");
                return NoContent();
            }
            catch (Exception ex)
            {
                return InternalError($"{controllerName}: {ex.Message} - {ex.InnerException}");
            }
        }

        /// <summary>
        /// Delete a Campaign Item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var controllerName = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{controllerName}: Attempted Call - Item ID: {id}");
                if (id < 0)
                {
                    _logger.LogWarn($"{controllerName}: Invalid ID - Item ID: {id}");
                    return BadRequest();
                }
                var item = await _campRepo.FindByID(id);
                if (item == null)
                {
                    _logger.LogWarn($"{controllerName}: Not Found - Item ID: {id}");
                    return NotFound();
                }

                _logger.LogInfo($"{controllerName}: Attempting Delete - Item ID: {id}");
                var isSuccess = await _campRepo.Delete(item);

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
            return StatusCode(500, "Something went wrong, please contact the administrator.");
        }
    }
}
