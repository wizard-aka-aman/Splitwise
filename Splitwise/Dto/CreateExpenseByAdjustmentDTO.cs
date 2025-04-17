namespace Splitwise.Dto
{
    public class CreateExpenseByAdjustmentDTO
    {
        public int groupid { get; set; }
        public string paidby { get; set; }
        //public SortedDictionary<string , decimal> paidto { get; set; }
        public List<PaidToItem> paidto { get; set; }

        public decimal amount { get; set; }
        public string description { get; set; }
    }
    public class PaidToItem
    {
        public string name { get; set; }
        public decimal @decimal { get; set; }
    }

}
