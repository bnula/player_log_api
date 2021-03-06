using Microsoft.EntityFrameworkCore;
using player_log_api.Contracts;
using player_log_api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Services
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ApplicationDbContext _db;
        public LocationRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Location entity)
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Location entity)
        {
            _db.Remove(entity);
            return await Save();
        }

        public async Task<IList<Location>> FindAll()
        {
            var items = await _db.Locations
                .Include(q => q.Campaign)
                .Include(q => q.HomeNpcs)
                .Include(q => q.RelatedQuests)
                //.Include(q => q.CurrentNpcs)
                .ToListAsync();
            return items;
        }

        public async Task<Location> FindByID(int id)
        {
            var item = await _db.Locations
                .Include(q => q.Campaign)
                .Include(q => q.HomeNpcs)
                .Include(q => q.RelatedQuests)
                //.Include(q => q.CurrentNpcs)
                .FirstOrDefaultAsync(q => q.LocationID == id);
            return item;
        }

        public async Task<bool> RecordExistsByID(int id)
        {
            var exists = await _db.Locations.AnyAsync(q => q.LocationID == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Location entity)
        {
            _db.Update(entity);
            return await Save();
        }
    }
}
