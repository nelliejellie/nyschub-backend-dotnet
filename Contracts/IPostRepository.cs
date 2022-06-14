using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Contracts
{
    public interface IPostRepository
    {
        Task<List<ForumPost>> GetPaginated(int page = 1, int pagesize = 10);
        Task<ForumPost> GetById(int id);
        Task<bool> Add(ForumPost entity);
        Task<bool> Delete(int id);
        Task<bool> Update(ForumPost entity);
        Task<List<ForumPost>> AllCorpersPost(string username);
        Task<Corper> GetCorperById(string id);
    }
}
