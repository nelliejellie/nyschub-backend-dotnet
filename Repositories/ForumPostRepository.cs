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
    public class ForumPostRepository : IPostRepository
    {
        private readonly AppDbContext _database;

        public ForumPostRepository(AppDbContext database)
        {
            _database = database;
        }
        public async Task<bool> Add(ForumPost post)
        {
            await _database.ForumPosts.AddAsync(post);
            var changes = await _database.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<List<ForumPost>> GetPaginated(int page = 1, int pageSize = 10)
        {
            return await _database.ForumPosts.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(post => post.CreatedAt).ToListAsync();
        }
        // repository to get a particulars users post
        public async Task<List<ForumPost>> AllCorpersPost(string username)
        {
            return await _database.ForumPosts.Where(post => post.UserName == username).ToListAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var post = await _database.ForumPosts.FirstOrDefaultAsync(post => post.Id == id);

            if (post == null)
            {
                return false;
            }
            _database.ForumPosts.Remove(post);
            var changes = await _database.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<ForumPost> GetById(int id)
        {
            var post = await _database.ForumPosts.FirstOrDefaultAsync(post => post.Id == id);
            if(post == null)
            {
                return null;
            }
            return post;
        }

        public async Task<bool> Update(ForumPost post)
        {
            _database.ForumPosts.Update(post);
            var changes = await _database.SaveChangesAsync();
            return changes > 0;
        }
    }
}
