
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public async Task<Expense> CreateExpenseByAdjustment(CreateExpenseByAdjustmentDTO expensedto)
        {
            Expense expense = new Expense();
            int times = expensedto.paidto.Count();

            // Convert to SortedDictionary
            var paidToDict = new Dictionary<string, decimal>(
                expensedto.paidto.ToDictionary(x => x.name, x => x.@decimal)
            );
            var KeysList = paidToDict.Keys.ToList();

            for (int i = 0; i < times; i++)
            {
                if (paidToDict[KeysList[i]] > 0) { 
                expense = new Expense
                {
                    AddedWhen = DateTime.Now,
                    GroupId = expensedto.groupid,
                    PaidBy = expensedto.paidby,
                    Amount = paidToDict[KeysList[i]],
                    PaidTo = KeysList[i],
                    Description = expensedto.description

                };
                 
                _splitwiseContext.Expense.Add(expense);
                await _splitwiseContext.SaveChangesAsync();
                }
            }
            return expense;
        }

        public List<ExpenseWithGroupNameDTO> GetAllActivity(string name)
        {
            var userGroups = _groupRepository.GetAll(name); // Assume this gives List<Group> with GroupId and GroupName

            // Create a dictionary for fast groupId -> groupName lookup
            var groupDict = userGroups.ToDictionary(g => g.GroupId, g => g.Name);

            // Get all expenses in a single DB call where group is in user's groups
            var groupIds = groupDict.Keys.ToList();

            var expenses = _splitwiseContext.Expense
                .Where(e => groupIds.Contains(e.GroupId))
                .ToList();

            // Map to custom object with group name and sort
            var result = expenses
                .Select(e => new ExpenseWithGroupNameDTO
                {
                    Id = e.Id,
                    GroupId = e.GroupId,
                    PaidBy = e.PaidBy,
                    PaidTo = e.PaidTo,
                    Amount = e.Amount,
                    AddedWhen = e.AddedWhen,
                    Description = e.Description,
                    GroupName = groupDict[e.GroupId] // quick lookup
                })
                .OrderByDescending(e => e.AddedWhen)
                .ToList();

            return result;

        }

        public List<Expense> GetDescription(int id)
        {

            var getfullexpense = _splitwiseContext.Expense.Where(e => e.GroupId == id).OrderByDescending(e => e.AddedWhen).ToList();
            return getfullexpense;

        }

        public decimal GetExpenseByUser(string name , int id)
        {
            decimal RecieveAmount = _splitwiseContext.Expense.
                Where(e => e.GroupId == id && e.PaidBy == name && e.PaidTo != name).
                Sum(e => e.Amount); 

            decimal GivenAmount = _splitwiseContext.Expense.
                Where(e => e.GroupId == id && e.PaidBy != name && e.PaidTo == name).
                Sum(e => e.Amount);

            decimal RecieveAmountSettle = _splitwiseContext.Settle.
               Where(e => e.GroupId == id && e.PaidBy == name && e.PaidTo != name).
               Sum(e => e.Amount);

            decimal GivenAmountSettle = _splitwiseContext.Settle.
                Where(e => e.GroupId == id && e.PaidBy != name && e.PaidTo == name).
                Sum(e => e.Amount);


            //decimal UserAmount = RecieveAmount - GivenAmount;
            decimal UserAmount = RecieveAmount + RecieveAmountSettle - GivenAmount - GivenAmountSettle;
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

                decimal RecieveAmountSettle = _splitwiseContext.Settle.
               Where(e => e.GroupId == id && e.PaidBy == name && e.PaidTo == item).
               Sum(e => e.Amount);

                decimal GivenAmount = _splitwiseContext.Expense.
                Where(e => e.GroupId == id && e.PaidBy == item && e.PaidTo == name).
                Sum(e => e.Amount); 
                
                decimal GivenAmountSettle = _splitwiseContext.Settle.
                Where(e => e.GroupId == id && e.PaidBy == item && e.PaidTo == name).
                Sum(e => e.Amount);

                //decimal UserAmount = RecieveAmount - GivenAmount;
                decimal UserAmount = RecieveAmount + RecieveAmountSettle - GivenAmount - GivenAmountSettle;
                KeyValuePair<string, decimal> kp = new KeyValuePair<string, decimal>(item, UserAmount);
                ls.Add(kp);

            }
            return ls;
        }

        public decimal TotalExpense(int id)
        {
            decimal total = 0;
          total =  _splitwiseContext.Expense.Where(e => e.GroupId == id).Sum(e => e.Amount);
            return total;
        }

        public List<KeyValuePair<string, decimal>> TotalExpenseForEveryUser(int id)
        {
            var ExpenseOfThatGroup = _splitwiseContext.Expense.Where(e => e.GroupId == id).ToList();

            List<KeyValuePair<string, decimal>> kv = new List<KeyValuePair<string, decimal>>();

            var forfindGroupMemberOfThatGroup1 = _groupRepository.GetMemberofGroup(id);

            var GroupMemberOfThatGroup = forfindGroupMemberOfThatGroup1[0].ToList();

            foreach(var  group in GroupMemberOfThatGroup)
            {
                decimal UserAmount = 0;
               UserAmount = ExpenseOfThatGroup.Where(e => e.PaidBy == group).Sum(e => e.Amount);
                KeyValuePair<string, decimal> kp = new KeyValuePair<string, decimal>(group, UserAmount);
                kv.Add(kp);
            }
            return kv;

        }

        public List<KeyValuePair<string, decimal>> TotalExpenseOfLoggedInUser(string name)
        {
            var GroupsOfLoggedInUser = _groupRepository.GetAll(name);

            List<KeyValuePair<string, decimal>> kv = new List<KeyValuePair<string, decimal>>();

            foreach (var group in GroupsOfLoggedInUser)
            {
                decimal UserAmount = 0;

                UserAmount = _splitwiseContext.Expense.Where(e => e.GroupId == group.GroupId && e.PaidBy == name).Sum(e => e.Amount); 

                KeyValuePair<string, decimal> kp = new KeyValuePair<string, decimal>(group.Name, UserAmount);
                kv.Add(kp);
            }
            return kv;
        }
    }
}
