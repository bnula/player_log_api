using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Data
{
    public class Quest
    {
        public int QuestID { get; set; }
        public string QuestName { get; set; }
        public string Reward { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int StartingLocationID{ get; set; }
        [ForeignKey("StartingLocationID")]
        public Location StartingLocation { get; set; }
        public int QuestGiverID { get; set; }
        [ForeignKey("QuestGiverID")]
        public Npc QuestGiver { get; set; }
        public int CampaignID { get; set; }
        [ForeignKey("CampaignID")]
        public Campaign Campaign { get; set; }
    }
}
