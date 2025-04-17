
using Splitwise.Dto;

namespace Splitwise.Model
{
    public class SettleRepository : ISettleRepository
    {
        private readonly SplitwiseContext _splitwiseContext;

        public SettleRepository(SplitwiseContext splitwiseContext)
        {
            _splitwiseContext = splitwiseContext;
        }
        public async Task<bool> CreateSettle(SettleDTO settle)
        {
            if (settle.GroupId == 0) {
                return false;
            }
            if(settle.Amount <= 0)
            {
                return false;
            }
            if (settle != null)
            {
                var GetGroup = _splitwiseContext.Group.Where(e => e.GroupId == settle.GroupId).ToList();
                Settle s = new Settle()
                {
                    GroupId = settle.GroupId,
                    Group = GetGroup[0],
                    PaidBy = settle.PaidTo,
                    PaidTo = settle.PaidBy, 
                    Amount = settle.Amount

                };
                
                _splitwiseContext.Settle.Add(s);
                await _splitwiseContext.SaveChangesAsync(); 

                return true;
            }
            return false;
        }
    }
}
