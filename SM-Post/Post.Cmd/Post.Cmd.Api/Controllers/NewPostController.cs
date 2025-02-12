using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class NewPostController : ControllerBase
    {
        private readonly ILogger<NewPostController> _logger;
        private readonly ICommandDispetcher _comDisp;
        public NewPostController( ILogger<NewPostController> logger,ICommandDispetcher comDisp)
        {
            _logger = logger;
            _comDisp = comDisp;
        }
        [HttpGet]
          public async Task<ActionResult> GetAsync(){
            return Ok();
          }
        [HttpPost]
        public async Task<ActionResult> NewPostAsync(NewPostCommand command)
        { 
            var id = Guid.NewGuid();
            try
            {
            command.Id = id;
            await _comDisp.SendAsync(command);
            return StatusCode(StatusCodes.Status201Created, new NewPostResponse{
                Message = "New Post creation request completed successfully!",
                Id = id
            });
            }
            catch(InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex,"Client made a bad request");
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex,"System error while creating new request");
                return StatusCode(StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = ex.Message
                });
            }
        }
    }
}