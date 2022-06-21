using nyschub.DTO;
using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Contracts
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginDto userDto);
        Task<string> CreateToken(Corper corper);
    }
}
