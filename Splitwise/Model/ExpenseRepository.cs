
using Microsoft.AspNetCore.Mvc;
using Splitwise.Dto;

namespace Splitwise.Model
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly SplitwiseContext _splitwiseContext;
        private readonly IGroupRepository _groupRepository;
         
        public ExpenseRepository(SplitwiseContext splitwiseContext , IGroupRepository groupRepository)
        {
            _splitwiseContext = splitwiseContext;
            _groupRepository = groupRepository;
        }
        public async Task<Expense> CreateExpense(ExpenseDTO expensedto)
        {
            Expense expense = new Expense();
            for (int i = 0; i < expensedto.paidto.Count(); i++)
            {
                expense = new Expense
                {
                    AddedWhen = DateTime.Now,
                    GroupId = expensedto.groupid,
                    PaidBy = expensedto.paidby,
                    Amount = expensedto.amount / expensedto.paidto.Count(),
                    PaidTo = expensedto.paidto[i],
                    Description = expensedto.description

                };
                _splitwiseContext.Expense.Add(expense);
                await _splitwiseContext.SaveChangesAsync();
            }
            return expense;
        }
        
        public decimal GetExpenseByUser(string name , int id)
        {
            decimal RecieveAmount = _splitwiseContext.Expense.
                Where(e => e.GroupId == id && e.PaidBy == name && e.PaidTo != name).
                Sum(e => e.Amount); 

            decimal GivenAmount = _splitwiseContext.Expense.
                Where(e => e.GroupId == id && e.PaidBy != name && e.PaidTo == name).
                Sum(e => e.Amount);

            decimal UserAmount = RecieveAmount - GivenAmount;
            return UserAmount;
            
        }

        public List<KeyValuePair<string, decimal>> GetExpenseForEveryUser(string name, int id)
        {
            List<KeyValuePair<string, decimal>> ls = new List<KeyValuePair<string, decimal>>();
            var ListOfUsersOtherThenLoggedIn = _groupRepository.GetMemberofGroup(id);
            var listt = ListOfUsersOtherThenLoggedIn[0].ToList();
            foreach (var item in listt)
            {

                decimal RecieveAmount = _splitwiseContext.Expense.
                Where(e => e.GroupId == id && e.PaidBy == name && e.PaidTo == item).
                Sum(e => e.Amount);

                decimal GivenAmount = _splitwiseContext.Expense.
                    Where(e => e.GroupId == id && e.PaidBy == item && e.PaidTo == name).
                    Sum(e => e.Amount);

                decimal UserAmount = RecieveAmount - GivenAmount;
                KeyValuePair<string, decimal> kp = new KeyValuePair<string, decimal>(item, UserAmount);
                ls.Add(kp);

            }


           
            return ls;
        }
    }
}
