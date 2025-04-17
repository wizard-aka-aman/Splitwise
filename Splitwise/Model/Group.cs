using System.ComponentModel.DataAnnotations;

namespace Splitwise.Model
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}
