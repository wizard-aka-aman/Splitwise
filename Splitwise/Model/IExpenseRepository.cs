using Splitwise.Dto;

namespace Splitwise.Model
{
    public interface IExpenseRepository
    {
        Task<Expense> CreateExpense(ExpenseDTO expensedto); 
        Task<Expense> CreateExpenseByAdjustment(CreateExpenseByAdjustmentDTO expensedto);
        decimal GetExpenseByUser(string name ,int id);

        List<KeyValuePair<string, decimal>>  GetExpenseForEveryUser(string name, int id);

        List<Expense> GetDescription(int id);
        List<ExpenseWithGroupNameDTO> GetAllActivity(string name ,int start , int end);
        decimal TotalExpense(int id);

        List<KeyValuePair<string, decimal>> TotalExpenseForEveryUser(int id);
        List<KeyValuePair<string, decimal>> TotalExpenseOfLoggedInUser(string name);
    }
}
