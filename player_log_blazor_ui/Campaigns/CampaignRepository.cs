using Blazored.LocalStorage;
using player_log_blazor_ui.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace player_log_blazor_ui.Campaigns
{
    public class CampaignRepository : BaseRepository<CampaignModel>, ICampaignRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _storage;

        public CampaignRepository(
            IHttpClientFactory clientFactory,
            ILocalStorageService storage) : base(clientFactory, storage)
        {
            _clientFactory = clientFactory;
            _storage = storage;
        }
    }
}
