using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Enities;

namespace Post.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PostLookupController : ControllerBase
    {
        private readonly ILogger<PostLookupController> _logger;
        private readonly IQueryDispatcher<PostEntity> _queryDis;
        public PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
        {
            _logger=logger;
            _queryDis=queryDispatcher;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {
            var data =await _queryDis.SendAsync(new FindAllPostsQuery());
            if(data == null || !data.Any())
            {
                return NoContent();
            }
            var count = data.Count;
            return Ok(new PostLookupResponse{
                Posts = data,
                Message = $"Successfully returned {count} post{(count > 1 ? "s":string.Empty)}"
            });
            }
            catch(Exception ex)
            {
                string error = "Error while Proccessing the request";
                return ErrorResponse(ex,error);
            }
            
        }
         [HttpGet("byId/{postId}")]
        public async Task<ActionResult> GetPostByIdAsync(Guid postId)
        {
            try
            {
            var data =await _queryDis.SendAsync(new FindPostByIdQuery()
            {
                Id = postId
            });
            if(data == null || !data.Any())
            {
                return NoContent();
            }
           
            return Ok(new PostLookupResponse{
                Posts = data,
                Message = "Successfully returned post"
            });
            }
            catch(Exception ex)
            {
                string error = "Error while Proccessing the request by post Id";
               return ErrorResponse(ex,error);
            }
            
        }
          [HttpGet("byAuthor/{author}")]
        public async Task<ActionResult> GetPostsAsync(string author)
        {
            try
            {
            var data =await _queryDis.SendAsync(new FindPostsByAuthorQuery()
            {
                Author = author
            });
            if(data == null || !data.Any())
            {
                return NoContent();
            }
           
            return Ok(new PostLookupResponse{
                Posts = data,
                Message = "Successfully returned post"
            });
            }
            catch(Exception ex)
            {
                string error = "Error while Proccessing the request by post author";
               return ErrorResponse(ex,error);
            }
            
        }
          [HttpGet("withComments")]
        public async Task<ActionResult> GetPostsWithCommentsAsync()
        {
            try
            {
            var data =await _queryDis.SendAsync(new FindPostsWithCOmmentsQuery()
            {
               
            });
            if(data == null || !data.Any())
            {
                return NoContent();
            }

            return Ok(new PostLookupResponse{
                Posts = data,
                Message = "Successfully returned post"
            });
            }
            catch(Exception ex)
            {
                string error = "Error while Proccessing the request by post author";
               return ErrorResponse(ex,error);
            }
            
        }

           [HttpGet("byLikes/{likes}")]
        public async Task<ActionResult> GetPostsAsync(int likes)
        {
            try
            {
            var data =await _queryDis.SendAsync(new FindPostWithLikesQuery()
            {
                NumberOfLikes = likes
            });
            if(data == null || !data.Any())
            {
                return NoContent();
            }
           
            var count = data.Count;
            return Ok(new PostLookupResponse{
                Posts = data,
                Message = $"Successfully returned {count} post{(count > 1 ? "s":string.Empty)}"
            });
            }
            catch(Exception ex)
            {
                string error = "Error while Proccessing the request by post author";
                return ErrorResponse(ex,error);
            }
            
        }

        private ActionResult ErrorResponse (Exception ex, string safeErrorMessage)
        {
                _logger.LogError(ex,safeErrorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = safeErrorMessage
                });
        }

    }
}