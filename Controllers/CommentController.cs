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
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository<ForumComment> _commentRepositoy;
        

        public CommentController(ICommentRepository<ForumComment> commentRepositoy)
        {
            _commentRepositoy = commentRepositoy;
        }

        [SwaggerOperation(Summary = "endpoint for getting comments under a particular post in the forum")]
        [HttpGet]
        [Route("getallcomments/{postid}")]
        public async Task<IActionResult> GetPostComments(int postid)
        {
            var comments = await _commentRepositoy.GetCommentsUnderPost(postid);

            return Ok(comments);
        }

        [SwaggerOperation(Summary = "endpoint for making a comment under a particular post in the forum")]
        [HttpPost]
        [Route("{username}/{postid}/addcomment")]
        public async Task<IActionResult> AddComment(string username, int postid, ForumCommentDto commentDto)
        {
            var corper = await _commentRepositoy.GetCorperByUsername(username);
            var post = await _commentRepositoy.GetPostById(postid);

            var newComment = new ForumComment
            {
                UserName = corper.UserName,
                PostId = post.Id,
                Comment = commentDto.Comment
            };
            await _commentRepositoy.AddComment(newComment);
            var newCommentDto = new ForumResponseCommentDto
            {
                Comment = commentDto.Comment,
                Username = corper.UserName,
                post = post.Post
            };

            return Ok(newCommentDto);
        }

        [SwaggerOperation(Summary = "endpoint for getting a particular comment under a particular post in the forum")]
        [HttpGet]
        [Route("{commentId}")]
        public async Task<IActionResult> GetCommentById(int commentId)
        {
            var comment = await _commentRepositoy.GetCommentById(commentId);
            return Ok(comment);
        }

        [SwaggerOperation(Summary = "endpoint for deleting a comment under a particular post in the forum")]
        [HttpDelete]
        [Route("deletecomment/{commentId}")]
        public async Task<IActionResult> DeleteCommentById(string username, int commentId)
        {
            try
            {
                var corper = await _commentRepositoy.GetCorperByUsername(username);
                var comment = await _commentRepositoy.GetCommentById(commentId);

                if(corper.UserName == comment.UserName)
                {
                    await _commentRepositoy.DeleteComment(comment);
                    return Ok("your comment has been deleted");
                }
                else
                {
                    return BadRequest("this comment cannot be deleted by you");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
