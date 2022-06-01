using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.Entities;

namespace nyschub.Repositories
{
    public class CorperRepository : ICorperRepository
    {
        private AppDbContext _database;

        public CorperRepository(AppDbContext database)
        {
            _database = database;
        }

        public async Task<bool> Add(Corper corper)
        {
            await _database.Corpers.AddAsync(corper);
            var changes = await  _database.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<List<Corper>> All()
        {
            return await _database.Corpers.Where(corper => corper.Status == 1).ToListAsync();
        }

        public async Task<bool> Delete(string id)
        {
            var corper = await _database.Corpers.FindAsync(id);

            if(corper == null)
            {
                return false;
            }
            _database.Corpers.Remove(corper);
            var changes = await _database.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> GetById(string id)
        {
            var corper = await _database.Corpers.FindAsync(id);
            if (corper.Status == 1) { return true; }
            return false;
            
        }

        public async Task<bool> Update(Corper corper)
        {
            _database.Corpers.Update(corper);
            var changes = await _database.SaveChangesAsync();
            return changes > 0;
        }

        
    }
}
