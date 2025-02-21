using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RestoreReadDbController : ControllerBase
    {
          private readonly ILogger<RestoreReadDbController> _logger;
        private readonly ICommandDispetcher _comDisp;
        public RestoreReadDbController( ILogger<RestoreReadDbController> logger,ICommandDispetcher comDisp)
        {
            _logger = logger;
            _comDisp = comDisp;
        }
       [HttpPost]
        public async Task<ActionResult> RestoreReadDbAsyncAsync()
        { 
           
            try
            {
            
            await _comDisp.SendAsync(new RestoreReadDbCommand());
            return StatusCode(StatusCodes.Status201Created, new BaseResponse{
                Message = "Read database restore request completed successfully!",
               
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
                _logger.Log(LogLevel.Error, ex,"System error while restore read db");
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = ex.Message
                });
            }
        }
    }
}