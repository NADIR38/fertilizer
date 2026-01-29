using fertilizesop.BL.Models;
using fertilizesop.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.BL.Bl
{
    public class dashboardBl : IdashboardBl
    {
        private readonly IDashboardDL idl;
        public dashboardBl(IDashboardDL idl)
        {
            this.idl = idl;
        }
        public async Task<Dashboard> GetDashboardSummary()
        {
            var t1 = idl.totalproducts();
            var t2 = idl.totalcustomers();
            var t3 = idl.totalsuppliers();
            var t4 = idl.totalstockvalue();
            var t5 = idl.salestoday();
            var t6 = idl.getpendingbills();
            var t7 = idl.outofstocks();
            var t8 = idl.totalstock();

            await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8);

            return new Dashboard
            {
                totalproducts = await t1,
                totalcustomers = await t2,
                totalsuppliers = await t3,
                total_stock_value = await t4,
                salestodays = await t5,
                pendingbills = await t6,
                outproduct = await t7,
                total_stock = await t8
            };
        }
        public async Task<List<(string MonthName, decimal TotalSales)>> GetMonthlySalesComparison()
        {
            return await idl.GetMonthlySalesComparison();
        }

        public async Task<List<(DateTime Day, decimal TotalSales)>> GetMonthlySalesTrend()
        {
            return await idl.GetMonthlySalesTrend();
        }

        public async Task<List<(string name, int quantity)>> gettopsellingproducts() => await idl.GetTopSellingProducts();

        public async Task<List<(string SupplierName, int TotalBatches)>> GetTopSupplierContributions()
        {
            return await idl.GetTopSupplierContributions();
        }

        public async Task<List<Products>> outofstock()
        {
            return await idl.outofstock();
        }

        public async Task<List<inventorylog>> recentlogs()
        {
            return await idl.recentlogs();
        }
    }
}
