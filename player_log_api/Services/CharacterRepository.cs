using Microsoft.EntityFrameworkCore;
using player_log_api.Contracts;
using player_log_api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace player_log_api.Services
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly ApplicationDbContext _db;
        public CharacterRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> Create(Character entity)
        {
            await _db.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Character entity)
        {
            _db.Characters.Remove(entity);
            return await Save();
        }

        public async Task<IList<Character>> FindAll()
        {
            var items = await _db.Characters
                .Include(q => q.Campaign)
                .ToListAsync();
            return items;
        }

        public async Task<Character> FindByID(int id)
        {
            var item = await _db.Characters
                .Include(q => q.Campaign)
                .FirstOrDefaultAsync(q => q.CharacterID == id);
            return item;
        }

        public async Task<bool> RecordExistsByID(int id)
        {
            var exists = await _db.Characters.AnyAsync(q => q.CharacterID == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Character entity)
        {
            _db.Update(entity);
            return await Save();
        }
    }
}
