using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using nyschub.Contracts;
using nyschub.DTO;
using nyschub.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

    namespace nyschub.Controllers
    {
    
        [Route("api/[controller]")]
        [ApiController]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public class MarketController : ControllerBase
        {
            public readonly IMarketPostRepository _marketpostRepository;
            private readonly ICorperRepository _corperRepository;
            private readonly IImageService _imageService;

            public MarketController(IMarketPostRepository marketpostRepository, ICorperRepository corperRepository, IImageService imageService)
            {
                _marketpostRepository = marketpostRepository;
                _corperRepository = corperRepository;
                _imageService = imageService;
            }

        // get all posts
        [HttpGet]
        [Route("{state}/marketplace/{page}")]
        public async Task<IActionResult> GetPosts([FromRoute]string state, int page)
            {
                var posts = await _marketpostRepository.GetPaginated(state,page, 10);
                return Ok(posts);
            }

            // gets a particular post 
            [HttpGet]
            [Route("Post/{id}")]
            public async Task<IActionResult> GetPost(int id)
            {
                try
                {
                    var post = await _marketpostRepository.GetById(id);
                    if (post == null)
                    {
                        return BadRequest("this post has been deleted or doesnt exist");
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
            [Route("MyPosts/{username}")]
            public async Task<IActionResult> MyPosts(string username)
            {
                try
                {
                    var posts = await _marketpostRepository.AllCorpersPost(username);
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
            [Route("AddPost")]
            public async Task<IActionResult> AddPost([FromForm] MarketPostDto marketPostDto, string userId)
            {
                var corper = await _marketpostRepository.GetCorperById(userId);
       
                var urlStrings = new List<string>();
                
            

                foreach (var files in marketPostDto.PhotoPaths)
                {
                    if(files.Length > 0)
                    {
                        var filePath = Path.GetTempFileName();
                        using(var stream = new FileStream(filePath, FileMode.Create))
                        {
                            files.CopyTo(stream);
                        }
                        var uploadResult = await _imageService.AddImage(filePath);
                        urlStrings.Add(uploadResult);
                    }
                    
                }

                

               
                var newPost = new MarketPost()
                {
                    IsSold = false,
                    PhotoPath = urlStrings[0],
                    PhotoPathTwo = urlStrings.Count > 1 ? urlStrings[1] : "empty",
                    PhotoPathThree = urlStrings.Count > 2 ? urlStrings[2] : "empty",
                    Description = marketPostDto.Description,
                    Title = marketPostDto.Title,
                    Price = marketPostDto.Price,
                    UserName = corper.UserName,
                    StatePost = corper.StateOfDeployment,
                    CorperName = $"{corper.FirstName} {corper.LastName}"
                };

                

                var hasPosted = await _marketpostRepository.Add(newPost);

                if (hasPosted)
                {
                    return Ok(newPost);
                }
                return BadRequest("something went wrong while trying to create this post");
            }

            // update the market post
            [HttpPut]
            [Route("marketpost/{postId}")]
            public async Task<IActionResult> UpdatePost(int postId, UpdateMarketPostDto updateMarketPostDto)
            {
            try
            {
                var marketPost = await _marketpostRepository.GetById(postId);
                marketPost.Price = updateMarketPostDto.Price;
                marketPost.StatePost = updateMarketPostDto.State;

                var isSuccessful = await _marketpostRepository.Update(marketPost);

                if (isSuccessful)
                {
                    return Ok(marketPost);
                }
                else
                {
                    return BadRequest(new { Success = false, Error = "the details were not filled in properly" });
                }
            }
            catch (Exception)
            {

                throw;
            }
            }
            // corper wants to delete post
            [HttpDelete]
            [Route("DeletePost/{id}")]
            public async Task<IActionResult> DeletePost(int id, string userId)
            {
                try
                {
                    var corper = await _marketpostRepository.GetCorperById(userId);
                    var post = await _marketpostRepository.GetById(id);

                    if (post.UserName == corper.UserName)
                    {
                        await _marketpostRepository.Delete(post.Id);
                        return Ok("your post has been deleted");
                    }
                    else
                    {
                        return BadRequest("this comment cannot be deleted by you");
                    };
                }
                catch (Exception)
                {

                    throw;
                }
            }

        }
    
    }
