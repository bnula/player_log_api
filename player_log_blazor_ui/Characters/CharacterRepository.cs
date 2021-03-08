using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using player_log_blazor_ui.Base;

namespace player_log_blazor_ui.Characters
{
    public class CharacterRepository : BaseRepository<CharacterModel>, ICharacterRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILocalStorageService _storage;
        public CharacterRepository(
            IHttpClientFactory clientFactory,
            ILocalStorageService storage) : base (clientFactory, storage)
        {
            _clientFactory = clientFactory;
            _storage = storage;
        }
    }
}
