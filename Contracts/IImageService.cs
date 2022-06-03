using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Contracts
{
    public interface IImageService
    {
        Task<string> AddImage(string path);
    }
}
