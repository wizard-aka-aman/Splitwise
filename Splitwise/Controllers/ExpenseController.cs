using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Splitwise.Dto;
using Splitwise.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using Microsoft.AspNetCore.Http; 
using Microsoft.EntityFrameworkCore;
using System.IO;


namespace Splitwise.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly SplitwiseContext _splitwiseContext;

        public ExpenseController(IExpenseRepository expenseRepository, SplitwiseContext splitwiseContext)
        {
            _expenseRepository = expenseRepository;
            _splitwiseContext = splitwiseContext;

        } 


        [HttpPost("createexpense")]
        public async Task<IActionResult> CreateExpense([FromForm] ExpenseDTO expensedto)
        {
            if (expensedto.paidto.Count() <= 0)
            {
                return BadRequest("Please select Atleast one member");
            }
             
            var expense = await _expenseRepository.CreateExpense(expensedto);
            
            //Image(expensedto.Image, expense.Id);
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

        [HttpGet("GetActivity/{name}/{start}/{end}")]
        public List<ExpenseWithGroupNameDTO> GetActivity(string name,int start,int end)
        {

            var items = _expenseRepository.GetAllActivity(name,start,end);
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
        [HttpPut("image/{id}")]
        public async Task<IActionResult> Image(IFormFile filecollection, int id  )
        {
            int passcount = 0;
            int errorcount = 0;

            try
            { 
                 
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await filecollection.CopyToAsync(stream);

                        // Convert to base64 string
                        byte[] imageBytes = stream.ToArray();
                        string base64Image = Convert.ToBase64String(imageBytes);

                        //expensedto.Image = base64Image;

                    // OPTIONAL: You should map DTO to your Expense entity and update DB here
                    var expense = await _splitwiseContext.Expense.FindAsync(id);
                        if (expense != null)
                        {
                            expense.Image = base64Image;
                            await _splitwiseContext.SaveChangesAsync();
                        }

                        passcount++;
                    }
                 

                return Ok("image saved");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(int id)
        {
            List<string> Imageurl = new List<string>();
            //string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            try
            {
                var _productimage = this._splitwiseContext.Expense.Where(item => item.Id == id).ToList();
                if (_productimage != null && _productimage.Count > 0)
                {
                    _productimage.ForEach(item =>
                    {
                        Imageurl.Add(item.Image);
                    });
                }
                else
                {
                    return NotFound("Image Not found ");
                } 

            }
            catch (Exception ex)
            {
            }
            return Ok(Imageurl);

        }

    }

}
