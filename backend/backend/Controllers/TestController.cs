using backend.Extensions;
using backend.Interfaces.Services;
using backend.Models;
using backend.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public TestController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("create-chat/{userId1}/{userId2}")]
        public async Task<ActionResult> CreateChat(long userId1, long userId2)
        {
            try
            {
               await _messageService.CreateChat(userId1, userId2);
               return Ok("Chat created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateChat: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPost("send-message/{userId1}/{userId2}")]
        public async Task<ActionResult> SendMessage([FromBody] string content, long userId1, long userId2)
        {
            try
            {
                await _messageService.SendMessage(userId1, userId2, content);
                return Ok("Message sent successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessage: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpGet("get-messages/{userId1}/{userId2}")]
        public async Task<ActionResult<Message>> GetMessages(long userId1, long userId2)
        {
            try
            {
                var messages = await _messageService.GetMessages(userId1, userId2);
                var response = new
                {
                    Message = "Messages fetched successfully",
                    Data = messages
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMessage: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        [HttpPut("read-messages/{userId1}/{userId2}")]
        public async Task<ActionResult> ReadMessages(long userId1, long userId2)
        {
            try
            {
                await _messageService.ReadMessages(userId1, userId2);
                return Ok("Messages read successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ReadMessages: {ex.Message}");
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
        
    }
}
