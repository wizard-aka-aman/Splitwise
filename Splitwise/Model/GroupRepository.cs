
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Splitwise.Dto; 
namespace Splitwise.Model
{
    public class GroupRepository : IGroupRepository
    {
        private readonly SplitwiseContext _splitwiseContext;


        private readonly Lazy<IExpenseRepository> _expenseRepository;

        public GroupRepository(SplitwiseContext splitwiseContext, Lazy<IExpenseRepository> expenseRepository)
        {
            _splitwiseContext = splitwiseContext;
            _expenseRepository = expenseRepository;
        }

        //public GroupRepository(SplitwiseContext splitwiseContext)
        //{
        //    _splitwiseContext = splitwiseContext; 
        //}
         

        public async Task<bool> CreateGroup(Group group)
        {
            if (group != null) {
                _splitwiseContext.Group.Add(group);
                await _splitwiseContext.SaveChangesAsync();
                await AddMember(group.GroupId, new List<string> { group.CreatedBy});
                await _splitwiseContext.SaveChangesAsync();

                return true;
            }
            return false;
        }
       
        public List<Group> GetAll(string name)
        {
            var AllItemGroupMember =   _splitwiseContext.GroupMember.Include(e => e.Group).ToList();
            List<Group> gp = new List<Group>();
            //SortedSet<Group> gp = new SortedSet<Group>();


            for (int i = 0; i < AllItemGroupMember.Count; i++)
            {
                if (AllItemGroupMember[i].UserNames.Contains(name))
                {
                    if(AllItemGroupMember[i].Group.IsDeleted==false)
                    gp.Add(AllItemGroupMember[i].Group);
                }
            }
             
            //var AllItem = await _splitwiseContext.Group.Where(e => e.CreatedBy == name).ToListAsync();
            //foreach (var group in AllItem) {
            //    gp.Add(group);
            //}
            return gp;
        }

        public async Task<bool> AddMember(int id, List<string> users)
        {
            // Find the group by id
            var group = await _splitwiseContext.Group.FirstOrDefaultAsync(g => g.GroupId == id);
            if (group == null)
                return false; // Group doesn't exist

            // Try to find an existing GroupMember record for this group
            var groupMember = await _splitwiseContext.GroupMember
                .Include(gm => gm.Group)
                .FirstOrDefaultAsync(gm => gm.Group.GroupId == id);

            if (groupMember != null)
            {
                // Update the existing member list
                groupMember.UserNames = users;
                _splitwiseContext.GroupMember.Update(groupMember);
            }
            else
            {
                // Create a new GroupMember record
                var newMember = new GroupMember
                {
                    Group = group,
                    UserNames = users
                };
                await _splitwiseContext.GroupMember.AddAsync(newMember);
            }

            await _splitwiseContext.SaveChangesAsync();
            return true;
        }

        public    List<List<string>> GetMemberofGroup(int id) {
            var GetMemberofGroup = _splitwiseContext.GroupMember.Include(gm => gm.Group).Where(e => e.Group.GroupId == id).Select(e => e.UserNames).ToList();

            return GetMemberofGroup;
        }

        public async Task<bool> EditGroup(int id ,Group group)
        {
            

            var getGroup = await _splitwiseContext.Group.FindAsync(id);
            if (getGroup == null)
            {
                return false;
            }
            getGroup.Name = group.Name;
            await _splitwiseContext.SaveChangesAsync(); 
            return true;

            
        }

        public async Task<bool> DeleteGroup(string name, int id)
        {
            if(id == 0)
            {
                return false;
            }
            var GetGroup = await _splitwiseContext.Group.FindAsync(id);
            var expenseRepo = _expenseRepository.Value;
            List<KeyValuePair<string, decimal>> GetExpenseForEveryUser = expenseRepo.GetExpenseForEveryUser(name , id);

            var KeysOfUsers = GetExpenseForEveryUser;

            decimal sum = 0;

            for (int i = 0; i < GetExpenseForEveryUser.Count(); i++)
            {
                sum += Math.Abs(GetExpenseForEveryUser[i].Value);
            }

            if (GetGroup != null && sum == 0)
            {
                GetGroup.IsDeleted = true;
            await _splitwiseContext.SaveChangesAsync(); 
            return true;
            }
            return false;
        }
    }
}
