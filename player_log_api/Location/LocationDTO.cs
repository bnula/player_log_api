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
        public ListCampaignDTO Campaign { get; set; }
        public virtual IList<ListNpcDTO> HomeNpcs { get; set; }
        //public virtual IList<NpcDTO> CurrentNpcs { get; set; }
        public virtual IList<ListQuestDTO> RelatedQuests { get; set; }
    }

    public class UpsertLocationDTO
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string LocationType { get; set; }
        public string LocationInventory { get; set; }
        public int CampaignID { get; set; }
    }

    public class ListLocationDTO
    {
        public int LocationID { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string LocationType { get; set; }
        public string LocationInventory { get; set; }
    }
}
