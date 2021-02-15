using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Data
{
    public class Army
    {
        public int ArmyID { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string ArmyComposition { get; set; }
        public int HomeLocationID { get; set; }
        [ForeignKey("HomeLocationID")]
        public Location HomeLocation { get; set; }
        public int CurrentLocationID { get; set; }
        [ForeignKey("CurrentLocationID")]
        public Location CurrentLocation { get; set; }
        public int LeaderID { get; set; }
        [ForeignKey("LeaderID")]
        public Npc Leader { get; set; }
        [ForeignKey("CampaignID")]
        public int CampaignID { get; set; }
        public Campaign Campaign { get; set; }
    }
}
