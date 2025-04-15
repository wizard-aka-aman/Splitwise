using Microsoft.AspNetCore.Identity;

namespace Splitwise.Model
{
    public class GroupMember
    {
        public int GroupMemberId { get; set; } 

        public Group Group { get; set; }
        public List<string> UserNames { get; set; }
    }
}
