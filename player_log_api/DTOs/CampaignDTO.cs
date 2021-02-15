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
        public virtual IList<ListCharacterDTO> Characters { get; set; }
        public virtual IList<ListLocationDTO> Locations { get; set; }
        public virtual IList<ListNpcDTO> Npcs { get; set; }
        public virtual IList<ListQuestDTO> Quests { get; set; }
        public virtual IList<ListArmyDTO> Armies { get; set; }
    }
    
    public class ListCampaignDTO
    {
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
    }
}
