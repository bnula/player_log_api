using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.DTOs
{
    public class CharacterDTO
    {
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int CampaignID { get; set; }
        public CampaignDTO Campaign { get; set; }
    }
}
