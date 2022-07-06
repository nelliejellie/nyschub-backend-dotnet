using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.DTO;
using nyschub.Entities;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DownVoteController : ControllerBase
    {
        private readonly IVoteRepository<DownVote> _voteReopsitory;
        private readonly AppDbContext _database;
        private readonly IVoteRepository<UpVote> _upVoteReopsitory;

        public DownVoteController(IVoteRepository<DownVote> voteReopsitory, AppDbContext database, IVoteRepository<UpVote> upVoteReopsitory)
        {
            _voteReopsitory = voteReopsitory;
            _database = database;
            _upVoteReopsitory = upVoteReopsitory;
        }

        [SwaggerOperation(Summary = "downvote a users post")]
        [HttpPost]
        [Route("addvote")]
        public async Task<IActionResult> AddVote([FromQuery] DownVoteDto downVoteDto)
        {
            var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.UserName == downVoteDto.UserName);
            var votes = await _database.DownVotes.Where(vote => vote.ForumPostId == downVoteDto.ForumPostId).ToListAsync();
            var upvotes = await _database.UpVotes.Where(vote => vote.ForumPostId == downVoteDto.ForumPostId).ToListAsync();

            // if user has voted in upvote already
            var existsInUpvote = await Task.Run(() => upvotes.Any(x => x.Username == downVoteDto.UserName));

            if (existsInUpvote)
            {
                var vote = await _database.UpVotes.FirstOrDefaultAsync(vote => vote.ForumPostId == downVoteDto.ForumPostId);
                await _upVoteReopsitory.RemoveVote(vote);
            }
            // if user has already downvoted
            var hasAlreadyPosted = await Task.Run(() => votes.Any(x => x.Username == downVoteDto.UserName));

            if (hasAlreadyPosted)
            {
                return BadRequest(new { Success = "false", Response = "corp member has voted already" });
            }
            else
            {
                var newVote = new DownVote
                {
                    Username = downVoteDto.UserName,
                    ForumPostId = downVoteDto.ForumPostId,
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

        [SwaggerOperation(Summary = "unvote a previous vote")]
        [HttpDelete]
        [Route("removevote/{username}/{postid}/{voteid}")]
        public async Task<IActionResult> DeleteVote(int voteid, string username, int postid)
        {
            try
            {
                var vote = await _database.DownVotes.FirstOrDefaultAsync(vote => vote.Id == voteid);
                var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.UserName == username);
                var post = await _database.ForumPosts.FirstOrDefaultAsync(post => post.Id == postid);

                if (post.UserName == corper.UserName)
                {
                    if (corper.UserName == vote.Username)
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

        [SwaggerOperation(Summary = "get all the downvotes under a particular comment")]
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
