﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nyschub.Contracts;
using nyschub.DataAccess;
using nyschub.DTO;
using nyschub.Entities;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace nyschub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        public readonly IPostRepository _postRepository;
        private readonly AppDbContext _database;
        private readonly ICorperRepository _corperRepository;
        private readonly IImageService _imageService;

        public ForumController(IPostRepository postRepository, AppDbContext database, ICorperRepository corperRepository, IImageService imageService)
        {
            _postRepository = postRepository;
            _database = database;
            _corperRepository = corperRepository;
            _imageService = imageService;
        }

        // get all posts
        [HttpGet]
        [Route("AllPosts/{page}", Name = "AllPost")]
        public async Task<IActionResult> GetPosts(int page)
        {
            var posts = await _postRepository.GetPaginated(page,10);
            return Ok(posts);
        }

        // gets a particular post 
        [HttpGet]
        [Route("Post/{id}", Name = "Post")]
        public async Task<IActionResult> GetPost(int id)
        {
            try
            {
                var post = await _postRepository.GetById(id);
                if (post == null)
                {
                    return BadRequest("this user does not exist");
                }
                return Ok(post);
            }
            catch (Exception)
            {
                return BadRequest("user not found");
            }
            
        }

        // gets all the post made by a corper
        [HttpGet]
        [Route("MyPosts/{username}", Name = "MyPosts")]
        public async Task<IActionResult> MyPosts(string username)
        {
            try
            {
                var posts = await _postRepository.AllCorpersPost(username);
                if (posts.Count == 0)
                {
                    return Ok("user has no posts");
                }
                return Ok(posts);
            }
            catch (Exception)
            {

                return BadRequest("somethin went wrong with the server");
            }
            
        }


        // corper creates a post
        [HttpPost]
        [Route("AddPost", Name = "AddPost")]
        public async Task<IActionResult> AddPost([FromForm] ForumRequestPostDto forumPostDto, string id)
        {
            var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.Id == id);
            var userName = corper.UserName;

            string imgUrl = "";
            var filePath = Path.GetTempFileName();
            using (var stream = System.IO.File.Create(filePath))
            {
                await forumPostDto.PhotoPath.CopyToAsync(stream);
            }
            var uploadResult = await _imageService.AddImage(filePath);
            imgUrl = uploadResult;


            var newPost = new ForumPost()
            {
                Post = forumPostDto.Post,
                PhotoPath = imgUrl,
                Caption = forumPostDto.Caption,
                UserName = userName,
            };

            var hasPosted = await _postRepository.Add(newPost);

            if (hasPosted)
            {
                return Ok(newPost);
            }
            return BadRequest("something went wrong with the server");
        }

        // corper wants to delete post
        [HttpDelete]
        [Route("DeletePost/{id}", Name = "DeletePost")]
        public async Task<IActionResult> DeletePost(int id, string userId)
        {
            try
            {
                var corper = await _database.Corpers.FirstOrDefaultAsync(corper => corper.Id == userId);
                var post = await _database.ForumPosts.FirstOrDefaultAsync(post => post.Id == id);

                if (post.UserName == corper.UserName)
                {
                    await _postRepository.Delete(post.Id);
                }
                return Ok($"{post.Post.Substring(0, 5)} has been deleted");
            }
            catch (Exception)
            {

                return BadRequest("you cant delete this post");
            }
        }
    }
}