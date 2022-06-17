using Microsoft.EntityFrameworkCore;
using nyschub.DataAccess;
using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Repositories
{
    public class TokenRepository
    {
        private readonly AppDbContext _database;

        public TokenRepository(AppDbContext database)
        {
            _database = database;
        }

        // post token to the database 
        public async Task<bool> Add(PasswordResetModel pass)
        {
            await _database.TokenTable.AddAsync(pass);
            var changes = await _database.SaveChangesAsync();

            return changes > 0;
        }

        // get token by random token name
        public async Task<PasswordResetModel> GetToken(string token)
        {
            var myAppToken = await _database.TokenTable.FirstOrDefaultAsync(t => t.EmailToken == token);

            return myAppToken;
        }
    }
}
