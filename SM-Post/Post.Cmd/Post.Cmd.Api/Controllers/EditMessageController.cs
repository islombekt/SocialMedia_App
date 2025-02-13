using CQRS.Core.Exception;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EditMessageController : ControllerBase
    { 
        private readonly ILogger<EditMessageController> _logger;
        private readonly ICommandDispetcher _comDisp;
        public EditMessageController( ILogger<EditMessageController> logger,ICommandDispetcher comDisp)
        {
            _logger = logger;
            _comDisp = comDisp;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditMessageAsync(Guid id, EditMessageCommand command)
        { 
          
            try
            {
            command.Id = id;
            await _comDisp.SendAsync(command);
            return StatusCode(StatusCodes.Status201Created, new BaseResponse{
                Message = "Post edit request completed successfully!",
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