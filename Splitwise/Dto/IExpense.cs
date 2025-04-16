namespace Splitwise.Dto
{
    public interface IExpense
    {
        public int groupid { get; set; }
        public string paidby { get; set; }
        
        public decimal amount { get; set; }
        public string description { get; set; }
    }
}
