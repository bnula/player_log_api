using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.DTOs
{
    public class LocationDTO
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string LocationType { get; set; }
        public string LocationInventory { get; set; }
        public int CampaignID { get; set; }
        public CampaignDTO Campaign { get; set; }
        public virtual IList<NpcDTO> BaseNpcs { get; set; }
        public virtual IList<NpcDTO> CurrentNpcs { get; set; }
        public virtual IList<QuestDTO> RelatedQuests { get; set; }
    }
}
