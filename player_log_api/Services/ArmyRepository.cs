using Microsoft.EntityFrameworkCore;
using player_log_api.Contracts;
using player_log_api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Services
{
    public class ArmyRepository : IArmyRepository
    {
        private readonly ApplicationDbContext _db;
        public ArmyRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Army entity)
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Army entity)
        {
            _db.Remove(entity);
            return await Save();
        }

        public async Task<IList<Army>> FindAll()
        {
            var items = await _db.Armies
                .Include(q => q.Leader)
                .Include(q => q.HomeLocation)
                .Include(q => q.CurrentLocation)
                .Include(q => q.Campaign)
                .ToListAsync();
            return items;
        }

        public async Task<Army> FindByID(int id)
        {
            var item = await _db.Armies
                .Include(q => q.Leader)
                .Include(q => q.HomeLocation)
                .Include(q => q.CurrentLocation)
                .Include(q => q.Campaign)
                .FirstOrDefaultAsync(q => q.ArmyID == id);
            return item;
        }

        public async Task<bool> RecordExistsByID(int id)
        {
            var exists = await _db.Armies.AnyAsync(q => q.ArmyID == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Army entity)
        {
            _db.Update(entity);
            return await Save();
        }
    }
}
