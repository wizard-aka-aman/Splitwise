using Microsoft.AspNetCore.Mvc;
using Splitwise.Dto;
using Splitwise.Model;

namespace Splitwise.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GroupController : Controller
    {

        private readonly IGroupRepository _groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpPost("creategroup")]
        public async Task<IActionResult> Creategroup([FromBody] Group group)
        {
            var items = await _groupRepository.CreateGroup(group);
            if (items == false)
            {
                return NotFound("Unable to Create");
            }
            return Ok(group);

        }
        [HttpGet("getallgroup/{name}")]
        public  List<Group> GetAllGroup(string name)
        {

            var items =   _groupRepository.GetAll(name);
            return items;
        }
        //addmember/3
        [HttpPost("addmember/{id}")]
        public async Task<bool> Addmember([FromBody] List<string> users, int id)
        {

            var items = await _groupRepository.AddMember(id, users);
            return true;
        }
        [HttpGet("GetMemberofGroup/{id}")]
        public List<List<string>> GetMemberofGroup( int id)
        {

            var items = _groupRepository.GetMemberofGroup(id);
            return items;
        }

       

    }
}
