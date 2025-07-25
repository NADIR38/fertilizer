using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fertilizesop.BL.Models;


namespace fertilizesop.Interfaces.BLInterfaces
{
    internal interface ItransactionBL
    {
        bool AddTransaction(Transaction t);
        DataTable ViewAllTransactions();
        DataTable Search(string keyword);
    }
}
