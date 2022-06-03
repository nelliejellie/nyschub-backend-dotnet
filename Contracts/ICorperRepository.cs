using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Contracts
{
    public interface ICorperRepository
    {
        Task<List<Corper>> GetPaginated(int page = 1, int pageSize = 10);
        Task<bool> GetById(string id);
        Task<bool> Update(Corper corper);
    }
}
