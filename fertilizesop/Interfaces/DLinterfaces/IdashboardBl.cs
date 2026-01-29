using fertilizesop.BL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public interface IdashboardBl
    {
        Task<Dashboard> GetDashboardSummary();
        Task<List<(string MonthName, decimal TotalSales)>> GetMonthlySalesComparison();
        Task<List<(DateTime Day, decimal TotalSales)>> GetMonthlySalesTrend();
        Task<List<(string name, int quantity)>> gettopsellingproducts();
        Task<List<(string SupplierName, int TotalBatches)>> GetTopSupplierContributions();
        Task<List<Products>> outofstock();
        Task<List<inventorylog>> recentlogs();
    }
}