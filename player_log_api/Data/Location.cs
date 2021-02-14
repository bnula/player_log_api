using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Data
{
    public class Location
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string LocationType { get; set; }
        public string LocationInventory { get; set; }
        public int CampaignID { get; set; }
        [ForeignKey("CampaignID")]
        public Campaign Campaign { get; set; }
        public virtual IList<Npc> HomeNpcs { get; set; }
        public virtual IList<Npc> CurrentNpcs { get; set; }
        public virtual IList<Quest> RelatedQuests { get; set; }
    }
}
