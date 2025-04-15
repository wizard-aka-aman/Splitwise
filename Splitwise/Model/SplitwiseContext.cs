using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Splitwise.Model
{
    public class SplitwiseContext : IdentityDbContext<IdentityUser>
    {

        public SplitwiseContext(DbContextOptions options) : base(options)
        {
 
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupMember> GroupMember { get; set; }
        public DbSet<Expense> Expense { get; set; }


    }
}
