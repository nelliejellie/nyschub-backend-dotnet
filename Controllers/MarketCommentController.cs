using Microsoft.AspNetCore.Mvc;
using nyschub.Contracts;
using nyschub.DTO;
using nyschub.Entities;
using nyschub.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketCommentController : ControllerBase
    {
        private readonly ICommentRepository<MarketComment> _commentRepository;
        private readonly EmailService _email;

        public MarketCommentController(ICommentRepository<MarketComment> commentRepository, EmailService email)
        {
            _commentRepository = commentRepository;
            _email = email;
        }

        // route to get all comments under post
        [HttpGet]
        [Route("getallcomments/{postid}")]
        public async Task<IActionResult> GetPostComments(int postid)
        {
            var comments = await _commentRepository.GetCommentsUnderPost(postid);

            return Ok(comments);
        }


        // route to post a comment
        [HttpPost]
        [Route("{username}/{postid}/addcomment")]
        public async Task<IActionResult> AddComment(string username, int postid, MarketCommentDto commentDto)
        {
            var corper = await _commentRepository.GetCorperByUsername(username);
            var post = await _commentRepository.GetMarketPostById(postid);
            var OwnerOfPost = await _commentRepository.GetCorperByUsername(post.UserName);

            var newComment = new MarketComment
            {
                UserName = corper.UserName,
                MarketPostId = post.Id,
                Comment = commentDto.Comment
            };
            var isSuccessful = await _commentRepository.AddComment(newComment);
            var emailModel = new EmailModel()
            {
                Receipient = OwnerOfPost.Email,
                Title = "UPDATE FROM NYSCHUB MARKETPLACE!!!",
                Body = $"Dear {OwnerOfPost.FirstName}, {corper.UserName} just made a comment on your post"
            };

            await _email.SendMail(emailModel);
            var newCommentDto = new MarketResponseCommentDto
            {
                Comment = commentDto.Comment,
                Username = corper.UserName,
                post = post.Title
            };

            if (isSuccessful)
            {
                return Ok(newCommentDto);
            }
            else
            {
                return BadRequest(new { Success = false, Error = "the request is incomplete" });
            }
            
        }

        // route to get comment by id
        [HttpGet]
        [Route("{commentId}")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            
            try
            {
                var comment = await _commentRepository.GetCommentById(commentId);
                return Ok(comment);

            }
            catch (Exception)
            {

                return NotFound(new { Success = false, Error = "Content doesnt exist" });
            }
        }


        // route to delete comment
        [HttpDelete]
        [Route("deletecomment/{commentId}")]
        public async Task<IActionResult> DeleteCommentById(string username, int commentId)
        {
            try
            {
                var corper = await _commentRepository.GetCorperByUsername(username);
                var comment = await _commentRepository.GetCommentById(commentId);

                if (corper.UserName == comment.UserName)
                {
                    await _commentRepository.DeleteComment(comment);
                    return Ok("your comment has been deleted");
                }
                else
                {
                    return BadRequest(new { Success=false, Error = "this comment cannot be deleted by you" });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
