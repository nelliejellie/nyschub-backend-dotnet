using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using nyschub.Entities;

namespace nyschub.DataAccess
{
    public class AppDbContext : IdentityDbContext<Corper>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Corper> Corpers { get; set; }
        public DbSet<DownVote> DownVotes { get; set; }
        public DbSet<UpVote> UpVotes { get; set; }
        public DbSet<ForumComment> ForumComments { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<MarketPost> MarketPosts { get; set; }
        public DbSet<MarketComment> MarketComments { get; set; }
        public DbSet<PasswordResetModel> TokenTable { get; set; }
    }
}
