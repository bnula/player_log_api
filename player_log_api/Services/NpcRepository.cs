using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using player_log_api.Contracts;
using player_log_api.Data;

namespace player_log_api.Services
{
    public class NpcRepository : INpcRepository
    {
        private readonly ApplicationDbContext _db;
        public NpcRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Npc entity)
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Npc entity)
        {
            _db.Remove(entity);
            return await Save();
        }

        public async Task<IList<Npc>> FindAll()
        {
            var items = await _db.Npcs
                .Include(q => q.HomeLocation)
                .Include(q => q.CurrentLocation)
                .Include(q => q.Campaign)
                .Include(q => q.RelatedQuests)
                .ToListAsync();
            return items;
        }

        public async Task<Npc> FindByID(int id)
        {
            var item = await _db.Npcs
                .Include(q => q.HomeLocation)
                .Include(q => q.CurrentLocation)
                .Include(q => q.Campaign)
                .Include(q => q.RelatedQuests)
                .FirstOrDefaultAsync(q => q.NpcID == id);
            return item;
        }

        public async Task<bool> RecordExistsByID(int id)
        {
            var exists = await _db.Npcs.AnyAsync(q => q.NpcID == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Npc entity)
        {
            _db.Update(entity);
            return await Save();
        }
    }
}
