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

        // return paginated data of all corpers
        public async Task<List<Corper>> GetPaginated(int page = 1, int pageSize = 10)
        {
            return await _database.Corpers.Skip((page - 1) * pageSize).Take(pageSize).Where(corper => corper.Status == 1).ToListAsync();
        }

        // get user by id
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
