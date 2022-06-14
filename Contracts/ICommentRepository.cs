using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Contracts
{
    public interface ICommentRepository<T>
    {
        Task<List<T>> GetCommentsUnderPost(int postid);
        Task<T> GetCommentById(int id);
        Task<bool> AddComment(T comment);
        Task<bool> DeleteComment(T comment);
        Task<Corper> GetCorperByUsername(string username);
        Task<ForumPost> GetPostById(int postid);
        Task<MarketPost> GetMarketPostById(int postid);
    }
}
