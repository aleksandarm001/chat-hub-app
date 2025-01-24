using backend.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public ChatController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("messages")]
        public async Task<IActionResult> GetMessages()
        {
            var messages = await _messageService.GetMessages();
            return Ok(messages);
        }
        
        
    }
}
