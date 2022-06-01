using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Contracts
{
    public interface ICorperRepository
    {
        Task<List<Corper>> All();
        Task<bool> GetById(string id);
        Task<bool> Add(Corper corper);
        Task<bool> Delete(string id);
        Task<bool> Update(Corper corper);
    }
}
