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
    public class DownVoteRepository : IVoteRepository<DownVote>
    {
        private readonly AppDbContext _database;

        public DownVoteRepository(AppDbContext database)
        {
            _database = database;
        }
        public async Task<bool> AddVote(DownVote entity)
        {
            await _database.DownVotes.AddAsync(entity);

            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<List<DownVote>> AllVotes(int id)
        {
            return await _database.DownVotes.Where(vote => vote.ForumPostId == id).ToListAsync();
        }

        public async Task<bool> RemoveVote(DownVote entity)
        {
            _database.DownVotes.Remove(entity);

            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }
    }
}
