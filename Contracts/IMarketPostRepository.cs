using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Contracts
{
    public interface IMarketPostRepository
    {
        Task<List<MarketPost>> GetPaginated(string username, int page = 1, int pagesize = 10);
        Task<MarketPost> GetById(int id);
        Task<bool> Add(MarketPost entity);
        Task<bool> Delete(int id);
        Task<bool> Update(MarketPost entity);
        Task<List<MarketPost>> AllCorpersPost(string username);
        Task<Corper> GetCorperById(string id);
        
    }
}
