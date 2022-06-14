using Microsoft.EntityFrameworkCore;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Repositories
{
    public class MarketPostRepository : IMarketPostRepository
    {
        private readonly AppDbContext _database;

        public MarketPostRepository(AppDbContext database)
        {
            _database = database;
        }
        public async Task<bool> Add(MarketPost post)
        {
            await _database.MarketPosts.AddAsync(post);
            var changes = await _database.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<List<MarketPost>> GetPaginated(string state, int page = 1, int pageSize = 10)
        {
            return await _database.MarketPosts.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(post => post.CreatedAt).Where(post => post.StatePost == state).ToListAsync();
        }
        // repository to get a particulars users post
        public async Task<List<MarketPost>> AllCorpersPost(string username)
        {
            return await _database.MarketPosts.Where(post => post.UserName == username).ToListAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var post = await _database.MarketPosts.FirstOrDefaultAsync(post => post.Id == id);

            if (post == null)
            {
                return false;
            }
            _database.MarketPosts.Remove(post);
            var changes = await _database.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<MarketPost> GetById(int id)
        {
            var post = await _database.MarketPosts.FirstOrDefaultAsync(post => post.Id == id);
            if (post == null)
            {
                return null;
            }
            return post;
        }

        public async Task<bool> Update(MarketPost post)
        {
            _database.MarketPosts.Update(post);
            var changes = await _database.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<Corper> GetCorperById(string id)
        {
            var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.Id == id);
            return corper;
        }
    }
}

