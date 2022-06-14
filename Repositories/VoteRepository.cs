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
    public class VoteRepository : IVoteRepository<UpVote>
    {
        private readonly AppDbContext _database;

        public VoteRepository(AppDbContext database)
        {
            _database = database;
        }
        public async Task<bool> AddVote(UpVote entity)
        {
            await _database.UpVotes.AddAsync(entity);

            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<List<UpVote>> AllVotes(int id)
        {
            return await _database.UpVotes.Where(vote => vote.ForumPostId == id).ToListAsync();
        }

        public async Task<bool> RemoveVote(UpVote entity)
        {
            _database.UpVotes.Remove(entity);

            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }
    }
}
