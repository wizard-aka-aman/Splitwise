using Splitwise.Dto;

namespace Splitwise.Model
{
    public interface IGroupRepository
    {
        Task<bool> CreateGroup(Group group);
        List<Group> GetAll(string name);
        Task<bool> AddMember(int id , List<string> users);
        List<List<string>> GetMemberofGroup(int id);

    }
}
