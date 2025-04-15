namespace Splitwise.Model
{
    public class Expense
    {
        public int Id { get; set; } 
        public int GroupId { get; set; } 
        public string PaidBy { get; set; }
        public string PaidTo { get; set; }
        public decimal Amount { get; set; }
        public DateTime AddedWhen { get; set; } = DateTime.Now;
        public string Description { get; set; }
    }
}
