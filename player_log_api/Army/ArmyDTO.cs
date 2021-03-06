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
        public string ArmyComposition { get; set; }
        public int HomeLocationID { get; set; }
        public ListLocationDTO HomeLocation { get; set; }
        public int CurrentLocationID { get; set; }
        public ListLocationDTO CurrentLocation { get; set; }
        public int LeaderID { get; set; }
        public ListNpcDTO Leader { get; set; }
        public int CampaignID { get; set; }
        public ListCampaignDTO Campaign { get; set; }
    }

    public class UpsertArmyDTO
    {
        public int ArmyID { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string ArmyComposition { get; set; }
        public int HomeLocationID { get; set; }
        public int CurrentLocationID { get; set; }
        public int LeaderID { get; set; }
        public int CampaignID { get; set; }
    }

    public class ListArmyDTO
    {
        public int ArmyID { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string ArmyComposition { get; set; }
    }
}
