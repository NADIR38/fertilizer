using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Models
{
    public class Transaction
    {
       
        public string TransactionType { get; set; } // "Deposit" or "Withdraw"
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
        public decimal RemainingBalance { get; set; }

        public Transaction ()
        {

        }
        public Transaction(string type,decimal Amount,DateTime date,string desc,decimal balance)
        {
            TransactionType = type;
            this.Amount = Amount;
            TransactionDate = date;
            Description = desc;
            RemainingBalance = balance;
        }
    }
}
