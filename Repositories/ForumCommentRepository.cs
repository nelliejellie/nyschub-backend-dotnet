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
    public class ForumCommentRepository : ICommentRepository<ForumComment>
    {
        private readonly AppDbContext _database;

        public ForumCommentRepository(AppDbContext database)
        {
            _database = database;
        }
        public async Task<bool> AddComment(ForumComment comment)
        {
            await _database.ForumComments.AddAsync(comment);
            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<bool> DeleteComment(ForumComment comment)
        {
            await Task.Run(() => _database.ForumComments.Remove(comment));

            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }

        public async Task<ForumComment> GetCommentById(int id)
        {
            var comment = await _database.ForumComments.FirstOrDefaultAsync(comment => comment.Id == id);

            return comment;

        }

        public async Task<List<ForumComment>> GetCommentsUnderPost(int postId)
        {
            return await _database.ForumComments.Where(comment => comment.PostId == postId).OrderByDescending(comment => comment.CreatedAt).ToListAsync();
        }

        public async Task<Corper> GetCorperByUsername(string username)
        {
            var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.UserName == username);

            return corper;
        }

        public Task<MarketPost> GetMarketPostById(int postid)
        {
            throw new NotImplementedException();
        }

        public async Task<ForumPost> GetPostById(int postid)
        {
            var post = await _database.ForumPosts.FirstOrDefaultAsync(post => post.Id == postid);

            return post;
        }
    }
}
