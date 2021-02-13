using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.DTOs
{
    public class QuestDTO
    {
        public int QuestID { get; set; }
        public string QuestName { get; set; }
        public string Reward { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int StartingLocationID { get; set; }
        public LocationDTO StartingLocation { get; set; }
        public int QuestGiverID { get; set; }
        public NPCDTO QuestGiver { get; set; }
        public int CampaignID { get; set; }
        public CampaignDTO Campaign { get; set; }
    }
}
