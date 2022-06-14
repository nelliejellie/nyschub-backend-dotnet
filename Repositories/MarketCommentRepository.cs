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
    public class MarketCommentRepository : ICommentRepository<MarketComment>
    {
        private readonly AppDbContext _database;

        public MarketCommentRepository(AppDbContext database)
        {
            _database = database;
        }

        // add a comment
        public async Task<bool> AddComment(MarketComment comment)
        {
            await _database.MarketComments.AddAsync(comment);
            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }

        // delete a comment
        public async Task<bool> DeleteComment(MarketComment comment)
        {
            await Task.Run(() => _database.MarketComments.Remove(comment));

            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }

        // get a comment by id
        public async Task<MarketComment> GetCommentById(int id)
        {
            var comment = await _database.MarketComments.FirstOrDefaultAsync(comment => comment.Id == id);

            return comment;
        }

        // get all comments under the marketpost
        public async Task<List<MarketComment>> GetCommentsUnderPost(int postid)
        {
            return await _database.MarketComments.Where(comment => comment.MarketPostId == postid).OrderByDescending(comment => comment.CreatedAt).ToListAsync();
        }

        public async Task<Corper> GetCorperByUsername(string username)
        {
            var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.UserName == username);

            return corper;
        }

        public async Task<MarketPost> GetMarketPostById(int postid)
        {
            var post = await _database.MarketPosts.FirstOrDefaultAsync(post => post.Id == postid);

            return post;
        }

        Task<ForumPost> ICommentRepository<MarketComment>.GetPostById(int postid)
        {
            throw new NotImplementedException();
        }
    }
}
