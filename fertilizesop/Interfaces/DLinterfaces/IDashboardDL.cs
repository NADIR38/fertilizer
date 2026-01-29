using fertilizesop.BL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fertilizesop.DL
{
    public interface IDashboardDL
    {
        Task<List<(string MonthName, decimal TotalSales)>> GetMonthlySalesComparison();
        Task<List<(DateTime Day, decimal TotalSales)>> GetMonthlySalesTrend();
        Task<int> getpendingbills();
        Task<List<(string ProductName, int QuantitySold)>> GetTopSellingProducts();
        Task<List<(string SupplierName, int TotalBatches)>> GetTopSupplierContributions();
        Task<List<Products>> outofstock();
        Task<int> outofstocks();
        Task<List<inventorylog>> recentlogs();
        Task<int> salestoday();
        Task<int> totalcustomers();
        Task<int> totalproducts();
        Task<int> totalstock();
        Task<int> totalstockvalue();
        Task<int> totalsuppliers();
    }
}