using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Data
{
    public class Campaign
    {
        public int CampaignID { get; set; }
        public string CampaignName { get; set; }
        public virtual IList<Character> Characters { get; set; }
        public virtual IList<Location> Locations { get; set; }
        public virtual IList<NPC> NPCs { get; set; }
        public virtual IList<Quest> Quests { get; set; }
        public virtual IList<Army> Armies { get; set; }
    }
}
