using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fertilizesop.DL
{
    public class DashboardDL : IDashboardDL
    {
        public async Task<int> totalstock()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"SELECT SUM(quantity) AS quantity_in_stock FROM products;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }

        public async Task<int> totalsuppliers()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"SELECT COUNT(*) AS Total_Suppliers FROM suppliers;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }

        public async Task<int> totalcustomers()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"SELECT COUNT(*) AS customers FROM customers;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }

        public async Task<int> salestoday()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"SELECT COUNT(*) AS total_sales FROM customerbills WHERE DATE(SaleDate) = CURDATE();";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }

        public async Task<int> totalstockvalue()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"SELECT SUM(sale_price * quantity) AS total FROM products";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }

        public async Task<int> totalproducts()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"select count(*) as total from products";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message, ex);
            }
        }

        public async Task<List<(DateTime Day, decimal TotalSales)>> GetMonthlySalesTrend()
        {
            var result = new List<(DateTime, decimal)>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"
                        SELECT 
                            DATE(Saledate) AS sale_day,
                            SUM(total_price) AS total_sales
                        FROM 
                            customerbills
                        WHERE 
                            MONTH(Saledate) = MONTH(CURDATE())
                            AND YEAR(Saledate) = YEAR(CURDATE())
                        GROUP BY 
                            sale_day
                        ORDER BY 
                            sale_day;";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int dateOrd = reader.GetOrdinal("sale_day");
                            int totalOrd = reader.GetOrdinal("total_sales");

                            DateTime date = reader.IsDBNull(dateOrd) ? DateTime.MinValue : reader.GetDateTime(dateOrd);
                            decimal total = reader.IsDBNull(totalOrd) ? 0m : reader.GetDecimal(totalOrd);

                            result.Add((date, total));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting sales trends: " + ex.Message, ex);
            }

            return result;
        }

        public async Task<List<(string MonthName, decimal TotalSales)>> GetMonthlySalesComparison()
        {
            var result = new List<(string, decimal)>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"
                        SELECT 
                            DATE_FORMAT(Saledate, '%b') AS month_name,
                            MONTH(Saledate) AS month_number,
                            SUM(total_price) AS total_sales
                        FROM 
                            customerbills
                        WHERE 
                            YEAR(Saledate) = YEAR(CURDATE())
                        GROUP BY 
                            month_name, month_number
                        ORDER BY 
                            month_number;";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int monthNameOrd = reader.GetOrdinal("month_name");
                            int totalSalesOrd = reader.GetOrdinal("total_sales");

                            string month = reader.IsDBNull(monthNameOrd) ? string.Empty : reader.GetString(monthNameOrd);
                            decimal total = reader.IsDBNull(totalSalesOrd) ? 0m : reader.GetDecimal(totalSalesOrd);

                            result.Add((month, total));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting monthly sales: " + ex.Message, ex);
            }

            return result;
        }

        public async Task<List<(string SupplierName, int TotalBatches)>> GetTopSupplierContributions()
        {
            var result = new List<(string, int)>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"
                        SELECT 
                            s.name AS supplier_name,
                            COUNT(*) AS total_batches
                        FROM 
                            batches b
                        JOIN 
                            suppliers s ON s.supplier_id = b.supplier_id
                        GROUP BY 
                            s.supplier_id
                        ORDER BY 
                            total_batches DESC
                        LIMIT 5;";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int nameOrd = reader.GetOrdinal("supplier_name");
                            int batchesOrd = reader.GetOrdinal("total_batches");

                            string supplier = reader.IsDBNull(nameOrd) ? string.Empty : reader.GetString(nameOrd);
                            int batches = reader.IsDBNull(batchesOrd) ? 0 : reader.GetInt32(batchesOrd);

                            result.Add((supplier, batches));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting supplier contributions: " + ex.Message, ex);
            }

            return result;
        }

        public async Task<int> getpendingbills()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = "SELECT COUNT(*) AS pending FROM supplierbills WHERE payment_status = 'Due';";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting pending bills: " + ex.Message, ex);
            }
        }

        public async Task<int> outofstocks()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"SELECT count(*) as total from products where quantity=0;";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting supplier contributions: " + ex.Message, ex);
            }
            return 0;
        }

        public async Task<List<Products>> outofstock()
        {
            var result = new List<Products>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"SELECT p.name,p.description from products p where quantity<5;";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int nameOrd = reader.GetOrdinal("name");
                            int descOrd = reader.GetOrdinal("description");

                            string supplier = reader.IsDBNull(nameOrd) ? string.Empty : reader.GetString(nameOrd);
                            string description = reader.IsDBNull(descOrd) ? string.Empty : reader.GetString(descOrd);

                            var stock = new Products(0, supplier, description);
                            result.Add(stock);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting supplier contributions: " + ex.Message, ex);
            }

            return result;
        }

        public async Task<List<inventorylog>> recentlogs()
        {
            var result = new List<inventorylog>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"
    SELECT p.name, i.change_type, i.quantity_change 
    FROM inventory_log i 
    JOIN products p ON p.product_id = i.product_id 
    ORDER BY i.log_date DESC 
    LIMIT 15;";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int nameOrd = reader.GetOrdinal("name");
                            int changeTypeOrd = reader.GetOrdinal("change_type");
                            int qtyOrd = reader.GetOrdinal("quantity_change");

                            string supplier = reader.IsDBNull(nameOrd) ? string.Empty : reader.GetString(nameOrd);
                            string description = reader.IsDBNull(changeTypeOrd) ? string.Empty : reader.GetString(changeTypeOrd);
                            int quantity = reader.IsDBNull(qtyOrd) ? 0 : reader.GetInt32(qtyOrd);

                            var log = new inventorylog(supplier, description, quantity);
                            result.Add(log);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting supplier contributions: " + ex.Message, ex);
            }

            return result;
        }

        public async Task<List<(string ProductName, int QuantitySold)>> GetTopSellingProducts()
        {
            var result = new List<(string, int)>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    await conn.OpenAsync();
                    string query = @"
                SELECT 
                    p.name AS product_name,
                    SUM(cd.quantity) AS total_quantity
                FROM 
                    customer_bill_details cd
                JOIN 
                    products p ON cd.product_id = p.product_id
                JOIN 
                    customerbills cb ON cb.billid = cd.Bill_id
                WHERE 
                    MONTH(cb.SaleDate) = MONTH(CURDATE())
                    AND YEAR(cb.SaleDate) = YEAR(CURDATE())
                GROUP BY 
                    p.product_id
                ORDER BY 
                    total_quantity DESC
                LIMIT 10;";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int nameOrd = reader.GetOrdinal("product_name");
                            int qtyOrd = reader.GetOrdinal("total_quantity");

                            string productName = reader.IsDBNull(nameOrd) ? string.Empty : reader.GetString(nameOrd);
                            int quantitySold = reader.IsDBNull(qtyOrd) ? 0 : reader.GetInt32(qtyOrd);

                            result.Add((productName, quantitySold));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting top selling products: " + ex.Message, ex);
            }

            return result;
        }

    }
}