using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.DTOs
{
    public class ArmyDTO
    {
        public int ArmyID { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Composition { get; set; }
        public int HomeLocationID { get; set; }
        public LocationDTO HomeLocation { get; set; }
        public int CurrentLocationID { get; set; }
        public LocationDTO CurrentLocation { get; set; }
        public int LeaderID { get; set; }
        public NPCDTO Leader { get; set; }
        public int CampaignID { get; set; }
        public CampaignDTO Campaign { get; set; }
    }
}
