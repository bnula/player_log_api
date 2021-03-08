using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using player_log_blazor_ui.Characters;

namespace player_log_blazor_ui.Campaigns
{
    public class CampaignModel
    {
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        public IList<CharacterModel> Characters { get; set; }

    }
}
