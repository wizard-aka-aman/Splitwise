using Splitwise.Dto;

namespace Splitwise.Model
{
    public interface IGroupRepository
    {
        Task<bool> CreateGroup(Group group);
        List<Group> GetAll(string name);
        Task<bool> AddMember(int id , List<string> users);
        List<List<string>> GetMemberofGroup(int id);
        Task<bool> EditGroup(int id, Group group);
        Task<bool> DeleteGroup(string name ,int id);
        Group GetGroupById(int id);

    }
}
