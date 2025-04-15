using Splitwise.Dto;

namespace Splitwise.Model
{
    public interface IExpenseRepository
    {
        Task<Expense> CreateExpense(ExpenseDTO expensedto); 
        decimal GetExpenseByUser(string name ,int id);

        List<KeyValuePair<string, decimal>>  GetExpenseForEveryUser(string name, int id);

    }
}
