using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.DTO;
using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UpvoteController : ControllerBase
    {
        private readonly IVoteRepository<UpVote> _voteReopsitory;
        private readonly AppDbContext _database;
        private readonly IVoteRepository<DownVote> _downVoteReopsitory;

        public UpvoteController(IVoteRepository<UpVote> voteReopsitory, AppDbContext database, IVoteRepository<DownVote> downVoteReopsitory)
        {
            _voteReopsitory = voteReopsitory;
            _database = database;
            _downVoteReopsitory = downVoteReopsitory;
        }

        [HttpPost]
        [Route("addvote")]
        public async Task<IActionResult> AddVote([FromQuery]UpVoteDto upVoteDto)
        {
            var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.UserName == upVoteDto.UserName);
            var votes = await _database.UpVotes.Where(vote => vote.ForumPostId == upVoteDto.ForumPostId).ToListAsync();
            var downvotes = await _database.DownVotes.Where(vote => vote.ForumPostId == upVoteDto.ForumPostId).ToListAsync();

            var hasAlreadyPosted = await Task.Run(() => votes.Any(x => x.Username == upVoteDto.UserName));

            // if user has voted in upvote already
            var existsInDownvote = await Task.Run(() => downvotes.Any(x => x.Username == upVoteDto.UserName));

            if (existsInDownvote)
            {
                var vote = await _database.DownVotes.FirstOrDefaultAsync(vote => vote.ForumPostId == upVoteDto.ForumPostId);
                await _downVoteReopsitory.RemoveVote(vote);
            }
            // if user has already downvoted

            if (hasAlreadyPosted)
            {
                return BadRequest(new { Success = "false", Response = "corp member has voted already" });
            }
            else
            {
                var newVote = new UpVote
                {
                    Username = upVoteDto.UserName,
                    ForumPostId = upVoteDto.ForumPostId,
                };

                var hasVoted = await _voteReopsitory.AddVote(newVote);

                if (hasVoted)
                {
                    return Ok(newVote);
                }
                else
                {
                    return BadRequest(new { Success = "false" });
                }
            }
            

            
        }

        [HttpDelete]
        [Route("removevote/{username}/{postid}/{voteid}")]
        public async Task<IActionResult> DeleteVote(int voteid, string username, int postid)
        {
            try
            {
                var vote = await _database.UpVotes.FirstOrDefaultAsync(vote => vote.Id == voteid);
                var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.UserName == username);
                var post = await _database.ForumPosts.FirstOrDefaultAsync(post => post.Id == postid);

                if (post.UserName == corper.UserName)
                {
                    if(corper.UserName == vote.Username)
                    {
                        await _voteReopsitory.RemoveVote(vote);
                        return Ok("your vote has been deleted");
                    }
                    return BadRequest(new { Success = "false", Response = " vote could not be deleted" });
                }
                else
                {
                    return BadRequest(new { Success = "false", Response = " vote could not be deleted" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [Route("votes/{id}")]
        public async Task<IActionResult> AllVotes(int id)
        {
            var post = await _database.ForumPosts.FirstOrDefaultAsync(post => post.Id == id);
            var votes = await _voteReopsitory.AllVotes(post.Id);

            return Ok(votes);
        }
    }
}
