using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace InventorySystem
{
    public partial class LoansForm : Form
    {
        private DataTable loansData;
        private bool isEditMode = false;
        private int currentLoanId = 0;

        public LoansForm()
        {
            InitializeComponent();
           
        }

        private void LoansForm_Load(object sender, EventArgs e)
        {
            SetupForm();
            LoadLoans();
            ClearForm();
        }

        private void SetupForm()
        {
            // إعداد ComboBox للأنواع
            cmbPersonType.Items.AddRange(new string[] { "كڕیار", "خزمەتگوزار" });
            cmbStatus.Items.AddRange(new string[] { "چالاک", "تەواو", "دواکەوتوو" });

            // إعداد التواريخ
            dtpLoanDate.Value = DateTime.Now;
            dtpDueDate.Value = DateTime.Now.AddMonths(1);

            // إعداد DataGridView
            dgvLoans.AutoGenerateColumns = false;
            SetupDataGridViewColumns();
        }

        private void SetupDataGridViewColumns()
        {
            // إضافة أعمدة للـ DataGridView
            dgvLoans.Columns.Clear();

            dgvLoans.Columns.Add("Id", "ID");
            dgvLoans.Columns["Id"].Visible = false;

            dgvLoans.Columns.Add("PersonName", "ناو");
            dgvLoans.Columns.Add("PersonType", "جۆر");
            dgvLoans.Columns.Add("Amount", "بڕ");
            dgvLoans.Columns.Add("LoanDate", "بەرواری قەرز");
            dgvLoans.Columns.Add("DueDate", "کۆتایی");
            dgvLoans.Columns.Add("Status", "بار");

            // تنسيق عمود المبلغ
            dgvLoans.Columns["Amount"].DefaultCellStyle.Format = "N0";
            dgvLoans.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void LoadLoans()
        {
            try
            {
                // إنشاء بيانات تجريبية (يمكن استبدالها بقاعدة بيانات)
                loansData = CreateSampleData();
                dgvLoans.DataSource = loansData;

                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"هەڵە لە بارکردنی قەرزەکان: {ex.Message}");
            }
        }

        private DataTable CreateSampleData()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("PersonName", typeof(string));
            table.Columns.Add("PersonType", typeof(string));
            table.Columns.Add("Amount", typeof(decimal));
            table.Columns.Add("LoanDate", typeof(DateTime));
            table.Columns.Add("DueDate", typeof(DateTime));
            table.Columns.Add("Status", typeof(string));

            // بيانات تجريبية
            table.Rows.Add(1, "عەلی کەریم", "كڕیار", 500000, DateTime.Now.AddDays(-30), DateTime.Now.AddDays(30), "چالاک");
            table.Rows.Add(2, "مەحموود عەبدوڵڵا", "خزمەتگوزار", 300000, DateTime.Now.AddDays(-60), DateTime.Now.AddDays(-10), "دواکەوتوو");
            table.Rows.Add(3, "سارا ئەحمەد", "كڕیار", 200000, DateTime.Now.AddDays(-15), DateTime.Now.AddDays(45), "چالاک");

            return table;
        }

        private void UpdateStatistics()
        {
            decimal totalLoans = 0;
            int activeCount = 0;
            int overdueCount = 0;

            if (loansData != null && loansData.Rows.Count > 0)
            {
                foreach (DataRow row in loansData.Rows)
                {
                    totalLoans += Convert.ToDecimal(row["Amount"]);

                    if (row["Status"].ToString() == "چالاک")
                        activeCount++;
                    else if (row["Status"].ToString() == "دواکەوتوو")
                        overdueCount++;
                }
            }

            lblTotalLoans.Text = $"{totalLoans:N0} دینار";
            lblOverdueCount.Text = $"{overdueCount} قەرز";

            // تحديث عنوان النافذة بعدد القروض
            this.Text = $"بەڕێوەبردنی قەرزەکان ({loansData?.Rows.Count ?? 0} قەرز)";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SaveLoan();
            }
        }

        private bool ValidateForm()
        {
            // التحقق من الحقول المطلوبة
            if (string.IsNullOrWhiteSpace(txtPersonName.Text))
            {
                MessageBox.Show("تکایە ناوی کەس بنووسە", "ئاگاداری");
                txtPersonName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("تکایە بڕێکی دروست بنووسە", "ئاگاداری");
                txtAmount.Focus();
                return false;
            }

            if (cmbPersonType.SelectedIndex == -1)
            {
                MessageBox.Show("تکایە جۆری کەس هەڵبژێرە", "ئاگاداری");
                cmbPersonType.Focus();
                return false;
            }

            if (cmbStatus.SelectedIndex == -1)
            {
                MessageBox.Show("تکایە بار هەڵبژێرە", "ئاگاداری");
                cmbStatus.Focus();
                return false;
            }

            return true;
        }

        private void SaveLoan()
        {
            try
            {
                if (isEditMode)
                {
                    // تحديث القرض الموجود
                    UpdateExistingLoan();
                    MessageBox.Show("قەرز نوێکرایەوە بە سەرکەوتوویی", "سەرکەوتوو");
                }
                else
                {
                    // إضافة قرض جديد
                    AddNewLoan();
                    MessageBox.Show("قەرز زیادکرا بە سەرکەوتوویی", "سەرکەوتوو");
                }

                LoadLoans();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"هەڵە لە پاشەکەوتکردن: {ex.Message}", "هەڵە");
            }
        }

        private void AddNewLoan()
        {
            DataRow newRow = loansData.NewRow();
            newRow["Id"] = GetNextId();
            newRow["PersonName"] = txtPersonName.Text;
            newRow["PersonType"] = cmbPersonType.SelectedItem;
            newRow["Amount"] = decimal.Parse(txtAmount.Text);
            newRow["LoanDate"] = dtpLoanDate.Value;
            newRow["DueDate"] = dtpDueDate.Value;
            newRow["Status"] = cmbStatus.SelectedItem;

            loansData.Rows.Add(newRow);
        }

        private void UpdateExistingLoan()
        {
            foreach (DataRow row in loansData.Rows)
            {
                if (Convert.ToInt32(row["Id"]) == currentLoanId)
                {
                    row["PersonName"] = txtPersonName.Text;
                    row["PersonType"] = cmbPersonType.SelectedItem;
                    row["Amount"] = decimal.Parse(txtAmount.Text);
                    row["LoanDate"] = dtpLoanDate.Value;
                    row["DueDate"] = dtpDueDate.Value;
                    row["Status"] = cmbStatus.SelectedItem;
                    break;
                }
            }
        }

        private int GetNextId()
        {
            int maxId = 0;
            foreach (DataRow row in loansData.Rows)
            {
                int id = Convert.ToInt32(row["Id"]);
                if (id > maxId) maxId = id;
            }
            return maxId + 1;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            currentLoanId = 0;
            txtPersonName.Clear();
            txtAmount.Text = "0";
            cmbPersonType.SelectedIndex = -1;
            cmbStatus.SelectedIndex = 0;
            dtpLoanDate.Value = DateTime.Now;
            dtpDueDate.Value = DateTime.Now.AddMonths(1);
            txtDescription.Clear();

            isEditMode = false;
            btnSave.Text = "پاشەکەوت";
            btnDelete.Enabled = false;
        }

        private void dgvLoans_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvLoans.Rows.Count)
            {
                LoadSelectedLoan(e.RowIndex);
            }
        }

        private void LoadSelectedLoan(int rowIndex)
        {
            DataGridViewRow row = dgvLoans.Rows[rowIndex];

            currentLoanId = Convert.ToInt32(row.Cells["Id"].Value);
            txtPersonName.Text = row.Cells["PersonName"].Value?.ToString() ?? "";

            // تعيين نوع الشخص
            string personType = row.Cells["PersonType"].Value?.ToString() ?? "";
            for (int i = 0; i < cmbPersonType.Items.Count; i++)
            {
                if (cmbPersonType.Items[i].ToString() == personType)
                {
                    cmbPersonType.SelectedIndex = i;
                    break;
                }
            }

            txtAmount.Text = Convert.ToDecimal(row.Cells["Amount"].Value).ToString("N0");
            dtpLoanDate.Value = Convert.ToDateTime(row.Cells["LoanDate"].Value);
            dtpDueDate.Value = Convert.ToDateTime(row.Cells["DueDate"].Value);

            // تعيين الحالة
            string status = row.Cells["Status"].Value?.ToString() ?? "";
            for (int i = 0; i < cmbStatus.Items.Count; i++)
            {
                if (cmbStatus.Items[i].ToString() == status)
                {
                    cmbStatus.SelectedIndex = i;
                    break;
                }
            }

            isEditMode = true;
            btnSave.Text = "نوێکردنەوە";
            btnDelete.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (currentLoanId == 0)
            {
                MessageBox.Show("هیچ قەرزێک هەڵنەبژێراوە", "ئاگاداری");
                return;
            }

            if (MessageBox.Show("دڵنیای لە سڕینەوەی ئەم قەرزە؟", "دڵنیابوونەوە",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DeleteLoan();
            }
        }

        private void DeleteLoan()
        {
            for (int i = 0; i < loansData.Rows.Count; i++)
            {
                if (Convert.ToInt32(loansData.Rows[i]["Id"]) == currentLoanId)
                {
                    loansData.Rows[i].Delete();
                    break;
                }
            }

            loansData.AcceptChanges();
            LoadLoans();
            ClearForm();
            MessageBox.Show("قەرز سڕدرایەوە", "سەرکەوتوو");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadLoans();
            ClearForm();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchLoans();
        }

        private void SearchLoans()
        {
            string searchText = txtSearch.Text.ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                dgvLoans.DataSource = loansData;
                return;
            }

            DataTable filteredTable = loansData.Clone();

            foreach (DataRow row in loansData.Rows)
            {
                if (row["PersonName"].ToString().ToLower().Contains(searchText) ||
                    row["PersonType"].ToString().ToLower().Contains(searchText) ||
                    row["Status"].ToString().ToLower().Contains(searchText))
                {
                    filteredTable.ImportRow(row);
                }
            }

            dgvLoans.DataSource = filteredTable;
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // السماح فقط بالأرقام
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtAmount_Leave(object sender, EventArgs e)
        {
            // تنسيق الرقم عند الخروج من الحقل
            if (decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                txtAmount.Text = amount.ToString("N0");
            }
        }
    }
}