using Messages.Api.Services;
using Messages.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messages.Api.Controllers
{
    [ApiController]
    [Route("api/messages")]

    public class MessageController : ControllerBase
    {

        private readonly IMessageService _messageService;
        private readonly ILogger _logger;
        public MessageController(IMessageService messageService,
             ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }
      //  [Authorize(Roles = "Api.ReadOnly,Api.ReadWrite")]
        [HttpGet]
        public IActionResult Get()
        {
            var messages = _messageService.GetEntities();
            _logger.LogInformation("list of message are retrived");
            return Ok(new { data = messages });
        }

     //   [Authorize(Roles = "Api.ReadOnly,Api.ReadWrite")]
        [HttpGet("{id}")]
        public IActionResult GetById(string id,[FromQuery] string flag)
        {
            try
            {
                //now its working finally--------------------------
                _logger.LogInformation($"message with id: { id } retrived");
                var message = _messageService.GetEntity(id,(string)flag);
                return Ok(new { data = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex);
            }
        }
      //  [Authorize(Roles = "Api.ReadWrite")]
        [HttpPost]
        public IActionResult Post([FromBody] MessageCheck data)
        {
            var id = _messageService.Add(data.value,data.flag);
            _logger.LogInformation($"message with id: {id} added");
            return Ok(new { data = id});
        }

        [HttpPut("{id}")]
        public IActionResult Put(string id , [FromBody] MessageCheck data)
        {
            try
            {
                _messageService.update(id ,data.value,data.flag);
                _logger.LogInformation($"message with id: { id } is updated");
                return Ok("updated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id,[FromQuery] string flag)
        {
            try
            {
                _messageService.Delete(id,flag);
                _logger.LogInformation($"message with id: { id } is deleted");
                return Ok("deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex);
            }
        }



    }
}
