using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_blazor_ui.Static
{
    public static class Endpoints
    {
        public static string BaseUrl = "https://localhost:44365";
        public static string Login = $"{BaseUrl}/api/Users/Login/";
        public static string Register = $"{BaseUrl}/api/Users/Register/";
        public static string Armies = $"{BaseUrl}/api/Armies/";
        public static string Campaigns = $"{BaseUrl}/api/Campaigns/";
        public static string Characters = $"{BaseUrl}/api/Characters/";
        public static string Locations = $"{BaseUrl}/api/Locations/";
        public static string Npcs = $"{BaseUrl}/api/Npcs/";
        public static string Quests = $"{BaseUrl}/api/Quests/";
    }
}
