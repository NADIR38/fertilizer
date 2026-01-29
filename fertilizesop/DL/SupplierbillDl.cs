using fertilizesop.BL.Models;
using KIMS;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace fertilizesop.DL
{
    public class SupplierbillDl : ISupplierbillDl
    {
        /// <summary>
        /// Creates a new supplier bill
        /// </summary>
        public bool CreateSupplierBill(int batchId, int supplierId, decimal totalAmount, decimal paidAmount)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();

                    // Determine payment status
                    string paymentStatus = paidAmount >= totalAmount ? "Paid" : "Due";

                    string query = @"
                        INSERT INTO supplierbills 
                        (batch_id, supplier_id, total_price, paid_amount, date, payment_status)
                        VALUES (@batch_id, @supplier_id, @total_price, @paid_amount, @date, @payment_status)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@batch_id", batchId);
                        cmd.Parameters.AddWithValue("@supplier_id", supplierId);
                        cmd.Parameters.AddWithValue("@total_price", totalAmount);
                        cmd.Parameters.AddWithValue("@paid_amount", paidAmount);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@payment_status", paymentStatus);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create supplier bill: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets supplier bill ID by batch and supplier
        /// </summary>
        public int GetSupplierBillId(int batchId, int supplierId)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT supplier_bill_id 
                        FROM supplierbills 
                        WHERE batch_id = @batch_id AND supplier_id = @supplier_id 
                        LIMIT 1";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@batch_id", batchId);
                        cmd.Parameters.AddWithValue("@supplier_id", supplierId);

                        object result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting supplier bill ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds payment record to supplierpricerecord
        /// </summary>
        public bool AddPaymentRecord(int supplierId, int billId, decimal paymentAmount, string remarks)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        INSERT INTO supplierpricerecord 
                        (supplier_id, supplier_bill_id, date, payment, remarks)
                        VALUES (@supplier_id, @bill_id, @date, @payment, @remarks)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@supplier_id", supplierId);
                        cmd.Parameters.AddWithValue("@bill_id", billId);
                        cmd.Parameters.AddWithValue("@date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@payment", paymentAmount);
                        cmd.Parameters.AddWithValue("@remarks", string.IsNullOrWhiteSpace(remarks) ? "Initial Payment" : remarks);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding payment record: {ex.Message}", ex);
            }
        }

        public Supplierbill GetSupplierBillByBatchName(string batchName)
        {
            if (string.IsNullOrWhiteSpace(batchName))
                throw new ArgumentException("Batch name is required", nameof(batchName));

            int batchId = DatabaseHelper.Instance.getbatchid(batchName);

            string query = @"
                SELECT 
                    sb.supplier_bill_id,
                    s.name AS supplier_name,
                    b.batch_name,
                    sb.total_price,
                    sb.date,
                    sb.paid_amount
                FROM 
                    supplierbills sb
                JOIN batches b ON sb.batch_id = b.batch_id
                JOIN suppliers s ON b.supplier_id = s.supplier_id
                WHERE sb.batch_id = @batch_id;";

            using (var conn = DatabaseHelper.Instance.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@batch_id", batchId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Supplierbill(
                                Convert.ToInt32(reader["supplier_bill_id"]),
                                reader["supplier_name"].ToString(),
                                reader["batch_name"].ToString(),
                                reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : Convert.ToDecimal(reader["total_price"]),
                                reader.IsDBNull(reader.GetOrdinal("date")) ? DateTime.MinValue : Convert.ToDateTime(reader["date"]),
                                reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : Convert.ToDecimal(reader["paid_amount"])
                            );
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public bool UpdateBill(Supplierbill s)
        {
            try
            {
                int batch_id = DatabaseHelper.Instance.getbatchid(s.batch_name);
                int supplier_id = DatabaseHelper.Instance.getsuppierid(s.supplier_name);

                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();

                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // 1. Update the supplier bill
                            string updateQuery = @"
                                UPDATE supplierbills 
                                SET total_price = @total,
                                    paid_amount = paid_amount + @paid,
                                    payment_status = CASE 
                                        WHEN (paid_amount + @paid) >= total_price THEN 'Paid' 
                                        ELSE 'Due' 
                                    END
                                WHERE batch_id = @batch_id AND supplier_id = @supplier_id";

                            using (var cmd = new MySqlCommand(updateQuery, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                                cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                                cmd.Parameters.AddWithValue("@total", s.total_price);
                                cmd.Parameters.AddWithValue("@paid", s.paid_price);
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Get the supplier_bill_id
                            string selectBillId = @"
                                SELECT supplier_bill_id 
                                FROM supplierbills 
                                WHERE batch_id = @batch_id AND supplier_id = @supplier_id 
                                LIMIT 1";

                            int billId;
                            using (var cmd = new MySqlCommand(selectBillId, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@batch_id", batch_id);
                                cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                                var result = cmd.ExecuteScalar();
                                if (result == null)
                                    throw new Exception("No matching bill found.");
                                billId = Convert.ToInt32(result);
                            }

                            // 3. Insert into supplierpricerecord
                            string insertRecord = @"
                                INSERT INTO supplierpricerecord 
                                (supplier_id, supplier_bill_id, date, payment, remarks)
                                VALUES (@supplier_id, @bill_id, @date, @payment, @remarks)";

                            using (var cmd = new MySqlCommand(insertRecord, conn, tran))
                            {
                                cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                                cmd.Parameters.AddWithValue("@bill_id", billId);
                                cmd.Parameters.AddWithValue("@date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@payment", s.paid_price);
                                cmd.Parameters.AddWithValue("@remarks", "Payment Update");
                                cmd.ExecuteNonQuery();
                            }

                            tran.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            throw new Exception("Failed during transaction: " + ex.Message, ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update bill: " + ex.Message, ex);
            }
        }

        public List<Supplierbill> getbills(string text)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            sb.supplier_Bill_ID, 
                            sb.batch_id,
                            sb.supplier_id,  
                            sb.paid_amount,   
                            sb.total_price,    
                            (sb.total_price - sb.paid_amount) as pending,   
                            sb.Date,    
                            s.name,    
                            b.batch_name,
                            sb.payment_status as status 
                        FROM supplierbills sb      
                        JOIN suppliers s ON s.supplier_id = sb.supplier_id      
                        JOIN batches b ON b.batch_id = sb.batch_id    
                        WHERE s.name LIKE @text OR b.batch_name LIKE @text OR sb.payment_status LIKE @text";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<Supplierbill>();
                        cmd.Parameters.AddWithValue("@text", "%" + text + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billid = reader.GetInt32("supplier_Bill_ID");
                                string suppname = reader.GetString("name");
                                string Bname = reader.GetString("batch_name");
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal("paid_amount");
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal("total_price");
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal("pending");
                                DateTime date = reader.GetDateTime("date");
                                int batch_id = reader.GetInt32("batch_id");
                                int suppid = reader.GetInt32("supplier_id");
                                string status = reader.GetString("status");

                                var bills = new Supplierbill(billid, suppname, date, totalamount, paidamount, Bname, pending, batch_id, suppid, status);
                                list.Add(bills);
                            }
                        }
                        return list;
                    }
                }
            }
            catch (MySqlException ex)
            {
                throw new Exception("Error retrieving bills: " + ex.Message, ex);
            }
        }

        public List<Supplierbill> getbill()
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            sb.supplier_Bill_ID,   
                            sb.paid_amount, 
                            sb.batch_id,
                            sb.supplier_id,   
                            sb.total_price,    
                            (sb.total_price - sb.paid_amount) as pending,   
                            sb.Date,    
                            s.name,    
                            b.batch_name,
                            sb.payment_status as Status 
                        FROM supplierbills sb      
                        JOIN suppliers s ON s.supplier_id = sb.supplier_id      
                        JOIN batches b ON b.batch_id = sb.batch_id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<Supplierbill>();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billid = reader.GetInt32("supplier_Bill_ID");
                                string suppname = reader.GetString("name");
                                string Bname = reader.GetString("batch_name");
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal("paid_amount");
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal("total_price");
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal("pending");
                                string status = reader.GetString("Status");
                                DateTime date = reader.GetDateTime("date");
                                int batch_id = reader.GetInt32("batch_id");
                                int suppid = reader.GetInt32("supplier_id");

                                var bills = new Supplierbill(billid, suppname, date, totalamount, paidamount, Bname, pending, batch_id, suppid, status);
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

        public List<Supplierbill> getbills(int billid)
        {
            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            sb.supplier_Bill_ID,   
                            sb.paid_amount, 
                            sb.batch_id,
                            sb.supplier_id,  
                            sb.total_price,    
                            (sb.total_price - sb.paid_amount) as pending,   
                            sb.Date,    
                            s.name,    
                            b.batch_name,
                            sb.payment_status as status 
                        FROM supplierbills sb      
                        JOIN suppliers s ON s.supplier_id = sb.supplier_id      
                        JOIN batches b ON b.batch_id = sb.batch_id    
                        WHERE sb.supplier_Bill_ID = @billid";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        var list = new List<Supplierbill>();
                        cmd.Parameters.AddWithValue("@billid", billid);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int billsid = reader.GetInt32("supplier_Bill_ID");
                                string suppname = reader.GetString("name");
                                string Bname = reader.GetString("batch_name");
                                decimal paidamount = reader.IsDBNull(reader.GetOrdinal("paid_amount")) ? 0 : reader.GetDecimal("paid_amount");
                                decimal totalamount = reader.IsDBNull(reader.GetOrdinal("total_price")) ? 0 : reader.GetDecimal("total_price");
                                decimal pending = reader.IsDBNull(reader.GetOrdinal("pending")) ? 0 : reader.GetDecimal("pending");
                                string status = reader.GetString("status");
                                DateTime date = reader.GetDateTime("date");
                                int batch_id = reader.GetInt32("batch_id");
                                int suppid = reader.GetInt32("supplier_id");

                                var bills = new Supplierbill(billsid, suppname, date, totalamount, paidamount, Bname, pending, batch_id, suppid, status);
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

        public List<SupplierPaymentSummary> GetSupplierPaymentSummary(string searchText = "")
        {
            var summaries = new List<SupplierPaymentSummary>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            s.supplier_id,
                            s.name,
                            s.phone,
                            COALESCE(SUM(sb.total_price - sb.paid_amount), 0) AS total_pending,
                            COALESCE(SUM(sb.paid_amount), 0) AS total_paid,
                            COUNT(CASE WHEN sb.payment_status = 'Due' THEN 1 END) AS pending_bill_count
                        FROM suppliers s
                        LEFT JOIN supplierbills sb ON s.supplier_id = sb.supplier_id
                        WHERE s.name LIKE @searchText OR s.phone LIKE @searchText OR @searchText = ''
                        GROUP BY s.supplier_id, s.name, s.phone
                        HAVING pending_bill_count > 0
                        ORDER BY total_pending DESC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var summary = new SupplierPaymentSummary(
                                    reader.GetInt32("supplier_id"),
                                    reader.GetString("name"),
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
                throw new Exception("Error retrieving supplier payment summary: " + ex.Message, ex);
            }

            return summaries;
        }

        public List<Supplierbill> GetPendingBillsBySupplier(int supplierId)
        {
            var bills = new List<Supplierbill>();

            try
            {
                using (var conn = DatabaseHelper.Instance.GetConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            sb.supplier_Bill_ID, 
                            s.name,
                            sb.Date,
                            sb.total_price,
                            sb.paid_amount,
                            b.batch_name,
                            (sb.total_price - sb.paid_amount) as pending,
                            sb.batch_id,
                            sb.supplier_id,
                            sb.payment_status as status
                        FROM supplierbills sb
                        JOIN suppliers s ON s.supplier_id = sb.supplier_id
                        LEFT JOIN batches b ON b.batch_id = sb.batch_id
                        WHERE sb.supplier_id = @supplierId 
                          AND sb.payment_status = 'Due'
                          AND (sb.total_price - sb.paid_amount) > 0
                        ORDER BY sb.Date ASC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@supplierId", supplierId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var bill = new Supplierbill(
                                    reader.GetInt32("supplier_Bill_ID"),
                                    reader.GetString("name"),
                                    reader.GetDateTime("Date"),
                                    reader.GetDecimal("total_price"),
                                    reader.GetDecimal("paid_amount"),
                                    reader.IsDBNull(reader.GetOrdinal("batch_name")) ? "" : reader.GetString("batch_name"),
                                    reader.GetDecimal("pending"),
                                    reader.IsDBNull(reader.GetOrdinal("batch_id")) ? 0 : reader.GetInt32("batch_id"),
                                    reader.GetInt32("supplier_id"),
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

        public bool ProcessBulkPayment(int supplierId, decimal totalPayment, string remarks)
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
                            var pendingBills = GetPendingBillsBySupplier(supplierId);

                            if (pendingBills.Count == 0)
                                throw new Exception("No pending bills found for this supplier");

                            decimal totalPending = pendingBills.Sum(b => b.pending);
                            decimal remainingPayment = totalPayment;

                            for (int i = 0; i < pendingBills.Count; i++)
                            {
                                var bill = pendingBills[i];
                                decimal billPayment;

                                if (i == pendingBills.Count - 1)
                                {
                                    billPayment = remainingPayment;
                                }
                                else
                                {
                                    billPayment = Math.Round((bill.pending / totalPending) * totalPayment, 2);
                                    billPayment = Math.Min(billPayment, bill.pending);
                                }

                                remainingPayment -= billPayment;

                                string updateQuery = @"
                                    UPDATE supplierbills 
                                    SET paid_amount = paid_amount + @payment,
                                        payment_status = CASE 
                                            WHEN (paid_amount + @payment) >= total_price THEN 'Paid' 
                                            ELSE 'Due' 
                                        END
                                    WHERE supplier_Bill_ID = @billId";

                                using (var cmd = new MySqlCommand(updateQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@payment", billPayment);
                                    cmd.Parameters.AddWithValue("@billId", bill.bill_id);
                                    cmd.ExecuteNonQuery();
                                }

                                string insertQuery = @"
                                    INSERT INTO supplierpricerecord 
                                    (supplier_id, supplier_bill_id, date, payment, remarks)
                                    VALUES (@supplierId, @billId, @date, @payment, @remarks)";

                                using (var cmd = new MySqlCommand(insertQuery, conn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@supplierId", supplierId);
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