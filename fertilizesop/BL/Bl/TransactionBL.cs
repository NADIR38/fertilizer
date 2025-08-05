using fertilizesop.BL.Models;
using fertilizesop.DL;
using fertilizesop.Interfaces.BLInterfaces;
using fertilizesop.Interfaces.DLinterfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class TransactionBL:ItransactionBL
    {
private readonly ITransactionDAL dal;
        private readonly IBankDAL bankDal;
        public TransactionBL(ITransactionDAL transactionDAL, IBankDAL bankDAL)
        {
            dal = transactionDAL ?? throw new ArgumentNullException(nameof(transactionDAL));
            bankDal = bankDAL ?? throw new ArgumentNullException(nameof(bankDAL));
        }
        public bool AddTransaction(string bankName, string type, decimal amount, DateTime date)
        {
            int bankId = bankDal.GetBankIdByName(bankName);
            if (bankId == -1) return false;

            decimal currentBalance = bankDal.GetRemainingBalance(bankName);
            decimal newBalance = currentBalance;

            if (type == "deposit")
                newBalance += amount;
            else if (type == "withdraw")
            {
                if (currentBalance < amount)
                    throw new Exception("Insufficient Balance");
                newBalance -= amount;
            }

            Transaction t = new Transaction
            {
                BankId = bankId,
                BankName = bankName,
                TransactionType = type,
                Amount = amount,
                TransactionDate = date
            };

            bool transactionInserted = dal.InsertTransaction(t);
            if (!transactionInserted) return false;

            return bankDal.UpdateRemainingBalance(bankId, newBalance);
        }

        public List<Transaction> GetBankTransactionHistory(int bankId)
        {
            return dal.GetTransactionsByBankId(bankId);
        }
    }

}
