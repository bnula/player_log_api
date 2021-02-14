using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using player_log_api.Data;
using player_log_api.DTOs;

namespace player_log_api.Mappings
{
    public class Maps : Profile
    {
        public Maps()
        {
            CreateMap<Campaign, CampaignDTO>().ReverseMap();
            CreateMap<Army, ArmyDTO>().ReverseMap();
            CreateMap<Character, CharacterDTO>().ReverseMap();
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Npc, NpcDTO>().ReverseMap();
            CreateMap<Quest, QuestDTO>().ReverseMap();
        }
    }
}
