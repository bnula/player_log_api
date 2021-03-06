using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.DTOs
{
    public class NpcDTO
    {
        public int NpcID { get; set; }
        public string NpcName { get; set; }
        public string Allegiance { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int HomeLocationID { get; set; }
        public ListLocationDTO HomeLocation { get; set; }
        //public int CurrentLocationID { get; set; }
        //public LocationDTO CurrentLocation { get; set; }
        public int CampaignID { get; set; }
        public ListCampaignDTO Campaign { get; set; }
        public virtual IList<ListQuestDTO> RelatedQuests { get; set; }
    }

    public class UpsertNpcDTO
    {
        public int NpcID { get; set; }
        public string NpcName { get; set; }
        public string Allegiance { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int HomeLocationID { get; set; }
        public int CampaignID { get; set; }
    }

    public class ListNpcDTO
    {
        public int NpcID { get; set; }
        public string NpcName { get; set; }
        public string Allegiance { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
    }
}
