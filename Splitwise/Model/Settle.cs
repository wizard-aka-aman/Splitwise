namespace Splitwise.Model
{
    public class Settle
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }

        public string PaidBy { get; set; }
        public string PaidTo { get; set; }
        public decimal Amount { get; set; }
    }
}
