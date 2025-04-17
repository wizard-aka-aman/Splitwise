namespace Splitwise.Dto
{
    public class SettleDTO
    {
        public string PaidBy { get; set; }
        public string PaidTo { get; set; }
        public decimal Amount { get; set; }
        public int GroupId { get; set; }
    }
}
