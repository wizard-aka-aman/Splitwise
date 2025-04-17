using Splitwise.Dto;

namespace Splitwise.Model
{
    public interface ISettleRepository 
    {
        Task<bool> CreateSettle(SettleDTO settle);
    }
}
