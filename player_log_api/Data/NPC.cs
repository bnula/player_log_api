﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Data
{
    public class NPC
    {
        public int NPCID { get; set; }
        public string NPCName { get; set; }
        public string Allegiance { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int HomeLocationID { get; set; }
        [ForeignKey("HomeLocationID")]
        public Location HomeLocation { get; set; }
        public int CurrentLocationID { get; set; }
        [ForeignKey("CurrentLocation")]
        public Location CurrentLocation { get; set; }
        public int CampaignID { get; set; }
        [ForeignKey("CampaignID")]
        public Campaign Campaign { get; set; }
    }
}
