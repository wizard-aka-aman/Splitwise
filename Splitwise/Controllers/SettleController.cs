using Microsoft.AspNetCore.Mvc;
using Splitwise.Dto;
using Splitwise.Model;

namespace Splitwise.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SettleController : Controller
    {
        private readonly ISettleRepository _settleRepository;

        public SettleController(ISettleRepository settleRepository)
        {
            _settleRepository = settleRepository;
        }
        [HttpPost("CreateSettle")]
        public async Task<IActionResult> CreateSettle(SettleDTO settle)
        {
            var result = await _settleRepository.CreateSettle(settle);
            return Ok(result);

        }
    }
}
