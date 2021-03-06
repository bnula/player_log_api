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
            CreateMap<Campaign, ListCampaignDTO>().ReverseMap();
            CreateMap<Army, ArmyDTO>().ReverseMap();
            CreateMap<Army, UpsertArmyDTO>().ReverseMap();
            CreateMap<Army, ListArmyDTO>().ReverseMap();
            CreateMap<Character, CharacterDTO>().ReverseMap();
            CreateMap<Character, UpsertCharacterDTO>().ReverseMap();
            CreateMap<Character, ListCharacterDTO>().ReverseMap();
            CreateMap<Location, LocationDTO>().ReverseMap();
            CreateMap<Location, UpsertLocationDTO>().ReverseMap();
            CreateMap<Location, ListLocationDTO>().ReverseMap();
            CreateMap<Npc, NpcDTO>().ReverseMap();
            CreateMap<Npc, UpsertNpcDTO>().ReverseMap();
            CreateMap<Npc, ListNpcDTO>().ReverseMap();
            CreateMap<Quest, QuestDTO>().ReverseMap();
            CreateMap<Quest, UpsertQuestDTO>().ReverseMap();
            CreateMap<Quest, ListQuestDTO>().ReverseMap();
        }
    }
}
