using Microsoft.EntityFrameworkCore;
using player_log_api.Contracts;
using player_log_api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Services
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly ApplicationDbContext _db;
        public CampaignRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Campaign entity)
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Campaign entity)
        {
            _db.Remove(entity);
            return await Save();
        }

        public async Task<IList<Campaign>> FindAll()
        {
            var items = await _db.Campaigns
                .Include(q => q.Characters)
                .Include(q => q.Locations)
                .Include(q => q.Npcs)
                .Include(q => q.Quests)
                .Include(q => q.Armies)
                .ToListAsync();
            return items;
        }

        public async Task<Campaign> FindByID(int id)
        {
            var item = await _db.Campaigns
                .Include(q => q.Characters)
                .Include(q => q.Locations)
                .Include(q => q.Npcs)
                .Include(q => q.Quests)
                .Include(q => q.Armies)
                .FirstOrDefaultAsync(q => q.CampaignID == id);
            return item;
        }

        public Task<bool> RecordExistsByID(int id)
        {
            var exists = _db.Campaigns.AnyAsync(q => q.CampaignID == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Campaign entity)
        {
            _db.Update(entity);
            return await Save();
        }
    }
}
