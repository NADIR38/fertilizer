using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace fertilizesop.DL
{
    public class Customerbilldl
    {
        private readonly DatabaseHelper _dbHelper;

        public Customerbilldl()
        {
            _dbHelper = DatabaseHelper.Instance;
        }

        public DataTable GetCustomerBillingRecords(string searchTerm = "")
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
            SELECT 
                cb.BillID,
                CONCAT(c.first_name, ' ', c.last_name) AS CustomerName,
                c.phone AS CustomerPhone,
                DATE_FORMAT(cb.SaleDate, '%d-%m-%Y %h:%i %p') AS SaleDate,
                CAST(cb.total_price AS DECIMAL(12,2)) AS TotalAmount,
                CAST(IFNULL(cb.paid_amount, 0) AS DECIMAL(12,2)) AS PaidAmount,
                CAST((cb.total_price - IFNULL(cb.paid_amount, 0)) AS DECIMAL(12,2)) AS DueAmount,
                CASE 
                    WHEN (cb.total_price - IFNULL(cb.paid_amount, 0)) <= 0 THEN 'Paid'
                    ELSE 'Due'
                END AS payment_status
            FROM 
                customerbills cb
            JOIN 
                customers c ON cb.CustomerID = c.customer_id
            WHERE 
                cb.BillID LIKE @searchTerm OR
                CONCAT(c.first_name, ' ', c.last_name) LIKE @searchTerm OR
                c.phone LIKE @searchTerm OR
                cb.SaleDate LIKE @searchTerm
            ORDER BY 
                cb.SaleDate DESC";

                using (var conn = _dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving billing records: " + ex.Message);
            }

            return dt;
        }

        public List<customerbill> getbill()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT    sb.BillID, sb.paid_amount, sb.CustomerID,   sb.total_price,    (sb.total_price-sb.paid_amount) as pending,   sb.SaleDate,    concat(s.first_name,' ',s.last_name) as name, sb.payment_status as Status FROM   customerbills sb      JOIN   customers s ON s.customer_id = sb.CustomerID ";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<customerbill>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billid = reader.GetInt32("BillID");
                                string customer_name = reader.GetString("name");
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_price"));
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("paid_amount"));
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending"));
                                string status = reader.GetString("Status");
                                DateTime date = reader.GetDateTime("SaleDate");
                                int customer_id = reader.GetInt32("CustomerID");
                                var bills = new customerbill(billid, customer_name, date, totalamount, paidamount, pending, customer_id, status);

                                list.Add(bills);



                            }
                        }
                        return list;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error retrieving batches: " + ex.Message, ex);
            }
        }

        public List<customerbill> searchbill(string text)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT    sb.BillID, sb.paid_amount, sb.CustomerID,   sb.total_price,    (sb.total_price-sb.paid_amount) as pending,   sb.SaleDate,    concat(s.first_name,' ',s.last_name) as name, sb.payment_status as Status FROM   customerbills sb      JOIN   customers s ON s.customer_id = sb.CustomerID  where concat(s.first_name,' ',s.last_name) like @text or BillID like @text or payment_status like @text";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@text", "%" + text + "%");
                        var list = new List<customerbill>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billid = reader.GetInt32("BillID");
                                string customer_name = reader.GetString("name");
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_price"));
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("paid_amount"));
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending"));
                                string status = reader.GetString("Status");
                                DateTime date = reader.GetDateTime("SaleDate");
                                int customer_id = reader.GetInt32("CustomerID");
                                var bills = new customerbill(billid, customer_name, date, totalamount, paidamount, pending, customer_id, status);

                                list.Add(bills);



                            }
                        }
                        return list;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error retrieving batches: " + ex.Message, ex);
            }
        }

        public DataTable GetBillDetails(int billId)
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
                    SELECT 
                        p.name AS ProductName,
                        cbd.quantity,
                        p.description AS ProductDescription,
                        (i.sale_price * cbd.quantity) AS TotalPrice,
                        cbd.discount,
                        cbd.status,
                        cbd.warranty,
                        cbd.warranty_from,
                        cbd.warranty_till
                    FROM 
                        customer_bill_details cbd
                    JOIN 
                        products p ON cbd.product_id = p.product_id
                    JOIN 
                        inventory i ON p.product_id = i.product_id
                    WHERE 
                        cbd.bill_id = @billId";

                using (var conn = _dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billId", billId);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving bill details: " + ex.Message);
            }

            return dt;
        }

        public DataTable GetPaymentHistory(int billId)
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"
                    SELECT 
                        date AS PaymentDate,
                        payment AS Amount,
                        BillID
                    FROM 
                        customerpricerecord
                    WHERE 
                        BillID = @billId
                    ORDER BY 
                        date DESC";

                using (var conn = _dbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billId", billId);

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving payment history: " + ex.Message);
            }

            return dt;
        }
        public static bool AddRecord(Customerrecord s)
        {
            int customerId = DatabaseHelper.Instance.getcustid(s.name);

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        // Insert into customerpricerecord
                        string insertQuery = @"
                    INSERT INTO customerpricerecord
                    (customer_id, billid, date, payment, remarks)
                    VALUES (@cust_id, @billid, @date, @payment, @remarks);";

                        using (var insertCmd = new MySqlCommand(insertQuery, conn, transaction))
                        {
                            insertCmd.Parameters.AddWithValue("@cust_id", customerId);
                            insertCmd.Parameters.AddWithValue("@billid", s.bill_id);
                            insertCmd.Parameters.AddWithValue("@date", s.date);
                            insertCmd.Parameters.AddWithValue("@payment", s.payement);
                            insertCmd.Parameters.AddWithValue("@remarks", s.remarks);

                            insertCmd.ExecuteNonQuery();
                        }

                        // Update paid_amount in customerbills
                        string updateQuery = @"
                    UPDATE customerbills
                    SET paid_amount = paid_amount + @payment
                    WHERE billid = @billid;";

                        using (var updateCmd = new MySqlCommand(updateQuery, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@payment", s.payement);
                            updateCmd.Parameters.AddWithValue("@billid", s.bill_id);

                            updateCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding payment record and updating bill: " + ex.Message, ex);
            }
        }


        public static List<Customerrecord> getrecord(int billid)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    var listt = new List<Customerrecord>();
                    conn.Open();
                    string query = "select * from customerpricerecord where BillID=@billid;";
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@billid", billid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32("record_id");
                                int billsid = reader.GetInt32("BillID");
                                int custid = reader.GetInt32("customer_id");
                                DateTime date = reader.GetDateTime("date");
                                decimal payments = reader.GetDecimal("payment");
                                string remarks = reader["remarks"] == DBNull.Value ? "" : reader.GetString("remarks");

                                var record = new Customerrecord(id, custid, payments, date, billsid, remarks);

                                listt.Add(record);
                            }
                        }
                        return listt;
                    }
                }
            }
            catch (Exception ex) { throw new Exception("error" + ex.Message, ex); }
        }

        /// <summary>
        /// Get payment summary for all customers (total pending, total paid, bill count)
        /// </summary>
        public List<CustomerPaymentSummary> GetCustomerPaymentSummary(string searchText = "")
        {
            var summaries = new List<CustomerPaymentSummary>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            c.customer_id,
                            CONCAT(c.first_name, ' ', c.last_name) AS customer_name,
                            c.phone,
                            COALESCE(SUM(cb.total_price - cb.paid_amount), 0) AS total_pending,
                            COALESCE(SUM(cb.paid_amount), 0) AS total_paid,
                            COUNT(CASE WHEN cb.payment_status = 'Due' THEN 1 END) AS pending_bill_count
                        FROM 
                            customers c
                        LEFT JOIN 
                            customerbills cb ON c.customer_id = cb.CustomerID
                        WHERE 
                            CONCAT(c.first_name, ' ', c.last_name) LIKE @searchText 
                            OR c.phone LIKE @searchText 
                            OR @searchText = ''
                        GROUP BY 
                            c.customer_id, c.first_name, c.last_name, c.phone
                        HAVING 
                            pending_bill_count > 0
                        ORDER BY 
                            total_pending DESC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var summary = new CustomerPaymentSummary(
                                    reader.GetInt32("customer_id"),
                                    reader.GetString("customer_name"),
                                    reader.IsDBNull(reader.GetOrdinal("phone")) ? "" : reader.GetString("phone"),
                                    reader.GetDecimal("total_pending"),
                                    reader.GetDecimal("total_paid"),
                                    reader.GetInt32("pending_bill_count")
                                );
                                summaries.Add(summary);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error retrieving customer payment summary: " + ex.Message, ex);
            }

            return summaries;
        }


        /// <summary>
        /// Get all pending bills for a specific customer
        /// </summary>
        public List<customerbill> GetPendingBillsByCustomer(int customerId)
        {
            var bills = new List<customerbill>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            cb.BillID,
                            CONCAT(c.first_name, ' ', c.last_name) AS customer_name,
                            cb.SaleDate,
                            cb.total_price,
                            cb.paid_amount,
                            (cb.total_price - cb.paid_amount) as pending,
                            cb.CustomerID,
                            cb.payment_status as status
                        FROM 
                            customerbills cb
                        JOIN 
                            customers c ON c.customer_id = cb.CustomerID
                        WHERE 
                            cb.CustomerID = @customerId 
                            AND cb.payment_status = 'Due'
                            AND (cb.total_price - cb.paid_amount) > 0
                        ORDER BY 
                            cb.SaleDate ASC";

                    using (var cmd = new MySqlCommand(query,conn))
                    {
                        cmd.Parameters.AddWithValue("@customerId", customerId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var bill = new customerbill(
                                    reader.GetInt32("BillID"),
                                    reader.GetString("customer_name"),
                                    reader.GetDateTime("SaleDate"),
                                    reader.GetDecimal("total_price"),
                                    reader.GetDecimal("paid_amount"),
                                    reader.GetDecimal("pending"),
                                    reader.GetInt32("CustomerID"),
                                    reader.GetString("status")
                                );
                                bills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error retrieving pending bills: " + ex.Message, ex);
            }

            return bills;
        }

        /// <summary>
        /// Process bulk payment across all pending bills for a customer
        /// Payment is distributed proportionally based on each bill's pending amount
        /// </summary>
        public bool ProcessBulkPayment(int customerId, decimal totalPayment, string remarks)
        {
            if (totalPayment <= 0)
                throw new ArgumentException("Payment amount must be greater than zero", nameof(totalPayment));

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            // Get all pending bills
                            var pendingBills = GetPendingBillsByCustomer(customerId);

                            if (pendingBills.Count == 0)
                                throw new Exception("No pending bills found for this customer");

                            // Calculate total pending amount
                            decimal totalPending = pendingBills.Sum(b => b.pending);

                            // Distribute payment across all bills proportionally
                            decimal remainingPayment = totalPayment;

                            for (int i = 0; i < pendingBills.Count; i++)
                            {
                                var bill = pendingBills[i];
                                decimal billPayment;

                                // For the last bill, use remaining payment to avoid rounding issues
                                if (i == pendingBills.Count - 1)
                                {
                                    billPayment = remainingPayment;
                                }
                                else
                                {
                                    // Calculate proportional payment
                                    billPayment = Math.Round((bill.pending / totalPending) * totalPayment, 2);
                                    billPayment = Math.Min(billPayment, bill.pending); // Don't overpay
                                }

                                remainingPayment -= billPayment;

                                // Update the bill
                                string updateQuery = @"
                                    UPDATE customerbills 
                                    SET paid_amount = paid_amount + @payment
                                    WHERE BillID = @billId";

                                using (var cmd = new MySqlCommand(updateQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@payment", billPayment);
                                    cmd.Parameters.AddWithValue("@billId", bill.bill_id);
                                    cmd.ExecuteNonQuery();
                                }

                                // Insert payment record
                                string insertQuery = @"
                                    INSERT INTO customerpricerecord 
                                    (customer_id, BillID, date, payment, remarks)
                                    VALUES (@customerId, @billId, @date, @payment, @remarks)";

                                using (var cmd = new MySqlCommand(insertQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@customerId", customerId);
                                    cmd.Parameters.AddWithValue("@billId", bill.bill_id);
                                    cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@payment", billPayment);
                                    cmd.Parameters.AddWithValue("@remarks", remarks);
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Failed during bulk payment transaction: " + ex.Message, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error processing bulk payment: " + ex.Message, ex);
            }
        }
    }
}