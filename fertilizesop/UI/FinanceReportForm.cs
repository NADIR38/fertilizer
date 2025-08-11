using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using fertilizesop.BL.Bl;

namespace fertilizesop.UI
{
    public partial class FinanceReportForm : Form
    {
        private readonly FinanceReportBL reportBLL;
        public FinanceReportForm()
        {
            InitializeComponent();
            reportBLL = new FinanceReportBL();
            InitControls();
        }

        private void InitControls()
        {
            // Fill months with names
            comboMonth.Items.Clear();
            for (int m = 1; m <= 12; m++)
                comboMonth.Items.Add(new { MonthNumber = m, Name = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m) });

            comboMonth.DisplayMember = "Name";
            comboMonth.ValueMember = "MonthNumber";
            comboMonth.SelectedIndex = DateTime.Today.Month - 1;

            // Fill years (example: 2020..next year)
            comboYear.Items.Clear();
            int startYear = 2020;
            int endYear = DateTime.Today.Year + 1;
            for (int y = startYear; y <= endYear; y++) comboYear.Items.Add(y);
            comboYear.SelectedItem = DateTime.Today.Year;

            radioMonthly.Checked = true;
            comboMonth.Enabled = true;
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            int month = int.Parse(comboMonth.SelectedItem.ToString());
            int year = int.Parse(comboYear.SelectedItem.ToString());

            DataTable dt = reportBLL.GetMonthlyReport(month, year);
            dgvReport.DataSource = dt;
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            try
            {
                int year = Convert.ToInt32(comboYear.SelectedItem);
                DataTable dt;

                if (radioMonthly.Checked)
                {
                    if (comboMonth.SelectedItem == null) { MessageBox.Show("Select month."); return; }
                    dynamic msel = comboMonth.SelectedItem;
                    int month = (int)msel.MonthNumber;
                    dt = reportBLL.GetMonthlyReport(year, month);

                    //var totals = _service.GetTotalsForMonth(year, month);
                    //ShowTotals(totals.totalSales, totals.totalExpenses, totals.profit);

                    // Format datagrid: period is date
                    dgvReport.DataSource = dt;
                    if (dgvReport.Columns.Contains("period"))
                        dgvReport.Columns["period"].HeaderText = "Date";
                }
                else
                {
                    dt = reportBLL.GetYearlyReport(year);
                    //var totals = _service.GetTotalsForYear(year);
                    //ShowTotals(totals.totalSales, totals.totalExpenses, totals.profit);

                    dgvReport.DataSource = dt;
                    if (dgvReport.Columns.Contains("period"))
                        dgvReport.Columns["period"].HeaderText = "Month";
                    // replace month numbers with names for readability
                    foreach (DataGridViewRow r in dgvReport.Rows)
                    {
                        if (r.Cells["period"].Value != null && int.TryParse(r.Cells["period"].Value.ToString(), out int mnum))
                        {
                            r.Cells["period"].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(mnum);
                        }
                    }
                }

                // Format numeric columns
                foreach (DataGridViewColumn col in dgvReport.Columns)
                {
                    if (col.Name == "TotalSales" || col.Name == "TotalExpense" || col.Name == "Profit")
                    {
                        col.DefaultCellStyle.Format = "N2";
                        col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message);
            }
        }
    }
}
