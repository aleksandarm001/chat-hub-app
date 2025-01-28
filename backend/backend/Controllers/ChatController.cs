using backend.Interfaces.Services;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
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
    }
}
