using Microsoft.EntityFrameworkCore;
using player_log_api.Contracts;
using player_log_api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Services
{
    public class QuestRepository : IQuestRepository
    {
        private readonly ApplicationDbContext _db;
        public QuestRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Quest entity)
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Quest entity)
        {
            _db.Remove(entity);
            return await Save();
        }

        public async Task<IList<Quest>> FindAll()
        {
            var items = await _db.Quests
                .Include(q => q.Campaign)
                .Include(q => q.QuestGiver)
                .Include(q => q.StartingLocation)
                .ToListAsync();
            return items;
        }

        public async Task<Quest> FindByID(int id)
        {
            var item = await _db.Quests
                .Include(q => q.Campaign)
                .Include(q => q.QuestGiver)
                .Include(q => q.StartingLocation)
                .FirstOrDefaultAsync(q => q.QuestID == id);
            return item;
        }

        public async Task<bool> RecordExistsByID(int id)
        {
            var exists = await _db.Quests.AnyAsync(q => q.QuestID == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Quest entity)
        {
            _db.Update(entity);
            return await Save();
        }
    }
}
