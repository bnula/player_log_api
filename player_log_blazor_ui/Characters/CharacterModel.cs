using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using player_log_blazor_ui.Campaigns;

namespace player_log_blazor_ui.Characters
{
    public class CharacterModel
    {
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public CampaignModel Campaign { get; set; }
    }
}
