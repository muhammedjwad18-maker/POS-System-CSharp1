using InventorySystem.Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class Dashboard : Form
    {
        private string connectionString =
           @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Hi\Documents\InventorySystem.mdf;Integrated Security=True";

        private SqlConnection connection;
        private Timer refreshTimer;
        private int userId;
        private string userName;

        public Dashboard(string role)
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;
            InitializeDatabase();
            ApplyKurdishTheme();
            LoadStatistics();
            SetupTimer();
            lblWelcome.Text = $"بەخێربێیت {userName}!";
        }

        public Dashboard(int userID, string fullName, string userType)
        {
            userId = userID;
        }

        private void InitializeDatabase()
        {
            string connectionString = Configuration.ConnectionString;
            connection = new SqlConnection(connectionString);
        }

        private void ApplyKurdishTheme()
        {
            // تطبيق التصميم الكردي
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.BackColor = Color.FromArgb(240, 242, 245);

            // تعيين الخط الكردي
            Font kurdishFont = new Font("NRT Reg", 10.5f, FontStyle.Regular);
            this.Font = kurdishFont;

            // تخصيص ألوان الأزرار
            foreach (Control control in pnlMenu.Controls)
            {
                if (control is Button btn)
                {
                    btn.Font = kurdishFont;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Cursor = Cursors.Hand;
                    btn.Padding = new Padding(10, 5, 10, 5);

                    // تأثير عند المرور
                    btn.MouseEnter += (s, e) =>
                    {
                        btn.BackColor = Color.FromArgb(52, 152, 219);
                        btn.ForeColor = Color.White;
                    };

                    btn.MouseLeave += (s, e) =>
                    {
                        if (btn.Tag?.ToString() != "active")
                        {
                            btn.BackColor = Color.Transparent;
                            btn.ForeColor = Color.FromArgb(52, 73, 94);
                        }
                    };
                }
            }
        }

        private void LoadStatistics()
        {
            try
            {
                connection.Open();

                // إحصائيات المنتجات
                string productsQuery = @"SELECT 
                    COUNT(*) as TotalProducts,
                    SUM(CASE WHEN Quantity <= MinQuantity THEN 1 ELSE 0 END) as LowStock,
                    SUM(CASE WHEN ExpiryDate <= DATEADD(DAY, 7, GETDATE()) THEN 1 ELSE 0 END) as ExpiringSoon
                    FROM Products";

                SqlCommand cmd = new SqlCommand(productsQuery, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblTotalProducts.Text = reader["TotalProducts"].ToString();
                    lblLowStock.Text = reader["LowStock"].ToString();
                    lblExpiringSoon.Text = reader["ExpiringSoon"].ToString();
                }
                reader.Close();

                // إحصائيات المبيعات اليومية
                string salesQuery = @"SELECT 
                    ISNULL(SUM(TotalAmount), 0) as TodaySales,
                    COUNT(*) as TodayOrders
                    FROM Sales 
                    WHERE CONVERT(DATE, SaleDate) = CONVERT(DATE, GETDATE())";

                cmd = new SqlCommand(salesQuery, connection);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblTodaySales.Text = string.Format("{0:N0} دینار", reader["TodaySales"]);
                    lblTodayOrders.Text = reader["TodayOrders"].ToString();
                }
                reader.Close();

                // إحصائيات العملاء والموردين
                string customerQuery = @"SELECT 
                    COUNT(*) as TotalCustomers,
                    SUM(Balance) as TotalCustomerBalance
                    FROM Customers";

                cmd = new SqlCommand(customerQuery, connection);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblTotalCustomers.Text = reader["TotalCustomers"].ToString();
                    lblCustomerBalance.Text = string.Format("{0:N0} دینار", reader["TotalCustomerBalance"]);
                }
                reader.Close();

                // إحصائيات المشتريات
                string purchaseQuery = @"SELECT 
                    ISNULL(SUM(TotalAmount), 0) as TodayPurchases
                    FROM Purchases 
                    WHERE CONVERT(DATE, PurchaseDate) = CONVERT(DATE, GETDATE())";

                cmd = new SqlCommand(purchaseQuery, connection);
                object result = cmd.ExecuteScalar();
                lblTodayPurchases.Text = string.Format("{0:N0} دینار", result);

                // تنبيهات المخزون المنخفض
                LoadLowStockAlerts();

                // تحميل أحدث المبيعات
                LoadRecentSales();

                // تحميل المنتجات الأكثر مبيعاً
                LoadTopProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("هەڵە لە بارکردنی ئامارەکان: " + ex.Message,
                    "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void LoadLowStockAlerts()
        {
            try
            {
                string query = @"SELECT TOP 5 ProductName, Quantity, MinQuantity 
                               FROM Products 
                               WHERE Quantity <= MinQuantity 
                               ORDER BY Quantity ASC";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvAlerts.DataSource = dt;
                dgvAlerts.Columns["ProductName"].HeaderText = "ناوی کاڵا";
                dgvAlerts.Columns["Quantity"].HeaderText = "بڕ";
                dgvAlerts.Columns["MinQuantity"].HeaderText = "کەمترین بڕ";

                // تلوين الصفوف
                foreach (DataGridViewRow row in dgvAlerts.Rows)
                {
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    int minQuantity = Convert.ToInt32(row.Cells["MinQuantity"].Value);

                    if (quantity < minQuantity * 0.5)
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                    else if (quantity <= minQuantity)
                        row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 200);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading alerts: " + ex.Message);
            }
        }

        private void LoadRecentSales()
        {
            try
            {
                string query = @"SELECT TOP 5 
                    s.SaleID,
                    s.SaleDate,
                    c.FullName as CustomerName,
                    s.TotalAmount,
                    s.NetAmount
                    FROM Sales s
                    LEFT JOIN Customers c ON s.CustomerID = c.CustomerID
                    ORDER BY s.SaleDate DESC";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvRecentSales.DataSource = dt;
                dgvRecentSales.Columns["SaleID"].HeaderText = "ژمارە";
                dgvRecentSales.Columns["SaleDate"].HeaderText = "بەروار";
                dgvRecentSales.Columns["CustomerName"].HeaderText = "کڕیار";
                dgvRecentSales.Columns["TotalAmount"].HeaderText = "کۆی گشتی";
                dgvRecentSales.Columns["NetAmount"].HeaderText = "خاو";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading recent sales: " + ex.Message);
            }
        }

        private void LoadTopProducts()
        {
            try
            {
                string query = @"SELECT TOP 5 
                    p.ProductName,
                    SUM(sd.Quantity) as TotalSold,
                    SUM(sd.Quantity * sd.UnitPrice) as TotalRevenue
                    FROM SaleDetails sd
                    INNER JOIN Products p ON sd.ProductID = p.ProductID
                    GROUP BY p.ProductName
                    ORDER BY TotalSold DESC";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvTopProducts.DataSource = dt;
                dgvTopProducts.Columns["ProductName"].HeaderText = "ناوی کاڵا";
                dgvTopProducts.Columns["TotalSold"].HeaderText = "فرۆشراو";
                dgvTopProducts.Columns["TotalRevenue"].HeaderText = "داھات";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading top products: " + ex.Message);
            }
        }

        private void SetupTimer()
        {
            refreshTimer = new Timer();
            refreshTimer.Interval = 300000; // 5 دقائق
            refreshTimer.Tick += (s, e) => LoadStatistics();
            refreshTimer.Start();
        }

        private void ResetButtonColors()
        {
            foreach (Control control in pnlMenu.Controls)
            {
                if (control is Button btn)
                {
                    btn.BackColor = Color.Transparent;
                    btn.ForeColor = Color.FromArgb(52, 73, 94);
                    btn.Tag = null;
                }
            }
        }

        private void ShowForm(Form form)
        {
            // إغلاق النموذج الحالي إذا كان مفتوحاً
            foreach (Form openForm in Application.OpenForms)
            {
                if (openForm.GetType() == form.GetType() && openForm != this)
                {
                    openForm.BringToFront();
                    return;
                }
            }

            form.MdiParent = this;
            form.WindowState = FormWindowState.Maximized;
            form.Show();
        }

        // أحداث النقر على الأزرار
        private void btnProducts_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            btnProducts.BackColor = Color.FromArgb(52, 152, 219);
            btnProducts.ForeColor = Color.White;
            btnProducts.Tag = "active";

            ProductsForm productsForm = new ProductsForm();
            ShowForm(productsForm);
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            btnSales.BackColor = Color.FromArgb(52, 152, 219);
            btnSales.ForeColor = Color.White;
            btnSales.Tag = "active";

            SalesForm salesForm = new SalesForm();
            ShowForm(salesForm);
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            btnCustomers.BackColor = Color.FromArgb(52, 152, 219);
            btnCustomers.ForeColor = Color.White;
            btnCustomers.Tag = "active";

            CustomersForm customersForm = new CustomersForm(); 
            ShowForm(customersForm);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            btnReports.BackColor = Color.FromArgb(52, 152, 219);
            btnReports.ForeColor = Color.White;
            btnReports.Tag = "active";

            ReportsForm reportsForm = new ReportsForm();
            ShowForm(reportsForm);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ResetButtonColors();
            btnSettings.BackColor = Color.FromArgb(52, 152, 219);
            btnSettings.ForeColor = Color.White;
            btnSettings.Tag = "active";

            SettingsForm settingsForm = new SettingsForm();
            ShowForm(settingsForm);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("دڵنیایت دەتەوێت دەرچیت؟",
                "دەرچوون", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("دڵنیایت دەتەوێت سیستەمەکە دابخەیت؟",
                "داخستن", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                refreshTimer.Stop();
                Application.Exit();
            }
        }

        private void lblRefresh_MouseClick(object sender, MouseEventArgs e)
        {
            LoadStatistics();
            MessageBox.Show("ئامارەکان نوێکرانەوە", "سەرکەوتوویی",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            // تحميل الصورة الشخصية
            LoadUserProfile();

            // تحديث الوقت
            UpdateDateTime();

            // بدء مؤقت تحديث الوقت
            Timer timeTimer = new Timer();
            timeTimer.Interval = 1000;
            timeTimer.Tick += (s, ev) => UpdateDateTime();
            timeTimer.Start();
        }

        private void LoadUserProfile()
        {
            try
            {
                // تحميل بيانات المستخدم
                string query = @"SELECT FullName, ProfileImage FROM Users WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserID", userId);

                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    lblFullName.Text = reader["FullName"].ToString();

                    // تحميل الصورة إذا وجدت
                    if (reader["ProfileImage"] != DBNull.Value)
                    {
                        byte[] imageData = (byte[])reader["ProfileImage"];
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(imageData))
                        {
                            pbProfile.Image = Image.FromStream(ms);
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading profile: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateDateTime()
        {
            lblDateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        // إظهار النموذج في منطقة المحتوى
        private void ShowFormInPanel(Form form)
        {
            // تنظيف منطقة المحتوى
            pnlContent.Controls.Clear();

            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(form);
            form.Show();
        }

        // أحداث القائمة الجانبية الإضافية
        private void btnCategories_Click(object sender, EventArgs e)
        {
            CategoriesForm categoriesForm = new CategoriesForm();
            ShowFormInPanel(categoriesForm);
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            SuppliersForm suppliersForm = new SuppliersForm();
            ShowFormInPanel(suppliersForm);
        }

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            PurchaseForm purchaseForm = new PurchaseForm();
            ShowFormInPanel(purchaseForm);
        }

        private void btnLoans_Click(object sender, EventArgs e)
        {
            LoansForm loansForm = new LoansForm();
            ShowFormInPanel(loansForm);
        }

        private void btnProductDetails_Click(object sender, EventArgs e)
        {
            ProductDetailsForm productDetailsForm = new ProductDetailsForm();
            ShowFormInPanel(productDetailsForm);
        }

        // زر المساعدة
        private void btnHelp_Click(object sender, EventArgs e)
        {
            string helpText = @"ڕێنمایی بەکارهێنان:

1. کاڵاکان: بۆ بەڕێوەبردنی هەموو کاڵاکان
2. فرۆشتن: بۆ تۆمارکردنی فرۆشتنە نوێیەکان
3. کڕیاران: بەڕێوەبردنی کڕیارەکان
4. راپۆرتەکان: بینینی راپۆرت و ئامار
5. ڕێکخستنەکان: گۆڕینەکانی سیستەم

ئامانج: بەڕێوەبردنی کۆگا و فرۆشتن";

            MessageBox.Show(helpText, "یارمەتی", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // زر النسخ الاحتياطي
        private void btnBackup_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.Description = "فۆڵدەرێک هەڵبژێرە بۆ هەڵگرتنی کۆپیەکی پاڵپشتی";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string backupPath = System.IO.Path.Combine(dialog.SelectedPath,
                            $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

                        string backupQuery = $"BACKUP DATABASE StoreManagementDB TO DISK = '{backupPath}'";

                        connection.Open();
                        SqlCommand cmd = new SqlCommand(backupQuery, connection);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("کۆپیەکی پاڵپشتی سازکرا بە سەرکەوتوویی!",
                            "سەرکەوتوویی", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("هەڵە لە دروستکردنی کۆپیەکی پاڵپشتی: " + ex.Message,
                            "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }

    internal class pnlMenu
    {
        public static IEnumerable<Control> Controls { get; internal set; }
    }
}
