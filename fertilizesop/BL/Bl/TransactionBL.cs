using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.DL;
using fertilizesop.BL.Models;
using fertilizesop.Interfaces.BLInterfaces;

namespace fertilizesop.BL.Bl
{
    public class TransactionBL: ItransactionBL
    {
        private TransactionDAL dal = new TransactionDAL();

        public bool AddTransaction(Transaction t)
        {
            decimal currentBalance = dal.GetLatestBalance();

            if (t.TransactionType == "Deposit")
                t.RemainingBalance = currentBalance + t.Amount;
            else if (t.TransactionType == "Withdraw")
                t.RemainingBalance = currentBalance - t.Amount;

            return dal.InsertTransaction(t);
        }

        public DataTable ViewAllTransactions()
        {
            return dal.GetAllTransactions();
        }

        public DataTable Search(string keyword)
        {
            return dal.SearchTransactions(keyword);
        }
    }

}
