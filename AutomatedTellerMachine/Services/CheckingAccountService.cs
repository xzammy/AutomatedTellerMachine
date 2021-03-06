﻿using AutomatedTellerMachine.Models;
using System.Linq;

namespace AutomatedTellerMachine.Services
{
    public class CheckingAccountService
    {
        private IApplicationDbContext _db;

        public CheckingAccountService(IApplicationDbContext dbContext)
        {
            _db = dbContext;
        }

        public void CreateCheckingAccount(string firstName, string lastName, 
            string userId, decimal initialBalance)
        {
            

            //ONLY FOR DEV PURPOSES
            //using this arbitrary value instead of hardcoded account numbers but this solution 
            //could lead to errors becasue when you delete an account you can end up having duplicate acc.num.
            // should be storing it in a confing file...
            var accNumbber = (123456 + _db.CheckingAccounts.Count())
                .ToString()
                .PadLeft(10, '0');

            var checkingAccount = new CheckingAccount()
            {
                FirstName = firstName,
                LastName = lastName,
                AccountNumber = accNumbber,
                Balance = 0,
                ApplicationUserId = userId

            };
            //add newly created CheckingAccount object to dbcontext
            _db.CheckingAccounts.Add(checkingAccount);
            //save changes to db
            _db.SaveChanges();
        }

        public void UpdateBalance(int checkingAccountId)
        {
            var checkingAccount = 
                _db.CheckingAccounts
                .Where(c => c.Id == checkingAccountId)
                .First();
            
           checkingAccount.Balance = 
                _db.Transactions
                .Where(c => c.CheckingAccountId == checkingAccountId)
                .Sum(c => c.AmountDecimal);
      
            _db.SaveChanges();

        }
    }
}