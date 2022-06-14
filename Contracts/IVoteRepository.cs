using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Contracts
{
    public interface IVoteRepository<T>
    {
        Task<bool> AddVote(T entity);

        Task<bool> RemoveVote(T entity);
        Task<List<T>> AllVotes(int id);

    }
}
