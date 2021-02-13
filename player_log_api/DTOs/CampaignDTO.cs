using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.DTOs
{
    public class CampaignDTO
    {
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public virtual IList<CharacterDTO> Characters { get; set; }
        public virtual IList<LocationDTO> Locations { get; set; }
        public virtual IList<NPCDTO> NPCs { get; set; }
        public virtual IList<QuestDTO> Quests { get; set; }
        public virtual IList<ArmyDTO> Armies { get; set; }
    }
}
