using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using player_log_api.Contracts;
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
                return Ok(response);
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
            return StatusCode(500, "Something went wrong, please contact the administrator.");
        }
    }
}
