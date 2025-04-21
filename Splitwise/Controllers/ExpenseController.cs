using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Splitwise.Dto;
using Splitwise.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Splitwise.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenseRepository; 

        public ExpenseController(IExpenseRepository expenseRepository )
        {
            _expenseRepository = expenseRepository;
             
        }

        
        [HttpPost("createexpense")]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDTO expensedto)
        {
            if (expensedto.paidto.Count() <= 0)
            {
                return BadRequest("Please select Atleast one member");
            }
            var expense = await _expenseRepository.CreateExpense(expensedto);
            return Ok(new { expense, result = "Expense Created" });
        }
        

        [HttpPost("CreateExpenseByAdjustment")]
        public async Task<IActionResult> CreateExpenseByAdjustment([FromBody] CreateExpenseByAdjustmentDTO expensedto)
        {
            if (expensedto.paidto.Count() <= 0)
            {
                return BadRequest("Please select Atleast one member");
            }
            var expense = await _expenseRepository.CreateExpenseByAdjustment(expensedto);
            return Ok(new { expense, result = "Expense Created" });
        }

        [HttpGet("GetExpenseByUser/{id}/{name}")]
        public  decimal  GetExpenseByUser( string name ,int id)
        { 
            var expense =  _expenseRepository.GetExpenseByUser(name ,id);
            return expense;
        }
        [HttpGet("GetExpenseForEveryUser/{id}/{name}")]
        public IActionResult GetExpenseForEveryUser(string name, int id)
        {
            var expense = _expenseRepository.GetExpenseForEveryUser(name, id);
            return Ok(expense);
        }
        
        
        [HttpGet("GetDescription/{id}")]
        public IActionResult GetDescription(int id)
        {
            var expense = _expenseRepository.GetDescription(id);
            return Ok(expense);
        }

        [HttpGet("GetActivity/{name}")]
        public List<ExpenseWithGroupNameDTO> GetActivity(string name)
        {

            var items = _expenseRepository.GetAllActivity(name);
            return items;
        }
        [HttpGet("TotalExpense/{id}")]
        public decimal TotalExpense(int id)
        {
            return _expenseRepository.TotalExpense(id);
        }

        [HttpGet("TotalExpenseForEveryUser/{id}")]
        public IActionResult TotalExpenseForEveryUser(int id)
        {
           var expensekv = _expenseRepository.TotalExpenseForEveryUser(id);
            return Ok(expensekv);
        }
        [HttpGet("TotalExpenseOfLoggedInUser/{name}")]
        public IActionResult TotalExpenseOfLoggedInUser(string name)
        {
            var loggedIn = _expenseRepository.TotalExpenseOfLoggedInUser(name);
            return Ok(loggedIn);
        }

    }

}
