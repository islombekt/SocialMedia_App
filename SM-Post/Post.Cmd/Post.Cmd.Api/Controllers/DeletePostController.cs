using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Exception;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DeletePostController : ControllerBase
    {
          private readonly ILogger<DeletePostController> _logger;
        private readonly ICommandDispetcher _comDisp;
        public DeletePostController( ILogger<DeletePostController> logger,ICommandDispetcher comDisp)
        {
            _logger = logger;
            _comDisp = comDisp;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemovePostAsync(Guid id, DeletePostCommand command)
        {
            command.Id = id;
            try
            {

            
            await _comDisp.SendAsync(command);
            return Ok(new BaseResponse
            {
                Message = "Post Deleted successfully done!"
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
              catch(AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex,"Current retrieve aggregate, client passed incorrect post Id");
                return StatusCode(StatusCodes.Status400BadRequest, new BaseResponse
                {
                    Message = ex.Message
                });
            }
            catch(Exception ex)
            {
                _logger.Log(LogLevel.Error, ex,"System error while creating new request");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = ex.Message
                });
            }
        }
    }
}