using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Model;
using Splitwise.Model.Chat;

namespace Splitwise.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly SplitwiseContext _context;

        public ChatController(SplitwiseContext context)
        {
            _context = context;
        }

        [HttpGet("{groupName}")]
        public async Task<IActionResult> GetMessages(string groupName)
        {
            var messages = await _context.Messages
                .Where(m => m.GroupName == groupName)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
            return Ok(messages);
        }

        [HttpGet("{groupName}/{reciver}")]
        public async Task<IActionResult> GetMessages(string groupName ,string reciver)
        {
            var messages = await _context.Messages.Where(e => e.GroupName == groupName && e.Sender == reciver ||  e.GroupName == reciver && e.Sender == groupName).ToListAsync(); 
            //var messages1 = await _context.Messages.Where().ToListAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMessage([FromBody] ChatMessage message)
        {
            message.SentAt = DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }

}
