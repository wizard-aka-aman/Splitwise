namespace Splitwise.Dto
{
    public class ExpenseDTO
    {
        public int groupid { get; set; }
        public string paidby { get; set; }
        public List<string> paidto { get; set; }
        public decimal amount { get; set; }
        public string description { get; set; }

        public string image { get; set; }
        public IFormFile imageFile { get; set; }
    }
}
