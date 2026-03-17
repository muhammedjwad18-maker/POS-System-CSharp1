using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class SalesForm : Form
    {
        private DataTable cartTable;
        private decimal totalAmount = 0;

        public SalesForm()
        {
            InitializeComponent();
            InitializeCart();
            LoadCustomers();
            LoadProducts();
        }
        private string DatabaseConnection = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Hi\Documents\InventorySystem.mdf;Integrated Security=True;Connect Timeout=30;";

        private void InitializeCart()
        {
            cartTable = new DataTable();
            cartTable.Columns.Add("بارکۆد", typeof(string));
            cartTable.Columns.Add("ناوی کاڵا", typeof(string));
            cartTable.Columns.Add("نرخ", typeof(decimal));
            cartTable.Columns.Add("بڕ", typeof(int));
            cartTable.Columns.Add("کۆ", typeof(decimal));

            dgvCart.DataSource = cartTable;

            // تنسيق الأعمدة
            if (dgvCart.Columns.Count > 0)
            {
                dgvCart.Columns["بارکۆد"].Width = 150;
                dgvCart.Columns["ناوی کاڵا"].Width = 250;
                dgvCart.Columns["نرخ"].Width = 120;
                dgvCart.Columns["بڕ"].Width = 100;
                dgvCart.Columns["کۆ"].Width = 150;
            }
        }

        private void LoadCustomers()
        {
            try
            {
                string query = "SELECT CustomerID, CustomerName FROM Customers ORDER BY CustomerName";
                DataTable dt = DatabaseConnection.ExecuteReader(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    cmbCustomers.DisplayMember = "CustomerName";
                    cmbCustomers.ValueMember = "CustomerID";
                    cmbCustomers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"هەڵە لە بارکردنی کڕیاران: {ex.Message}", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                string query = @"SELECT ProductID, ProductName, Barcode, SellPrice 
                                FROM Products 
                                WHERE Quantity > 0 
                                ORDER BY ProductName";
                DataTable dt = DatabaseConnection.ExecuteReader(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    cmbProducts.DisplayMember = "ProductName";
                    cmbProducts.ValueMember = "ProductID";
                    cmbProducts.DataSource = dt;

                    // حفظ البيانات الكاملة
                    cmbProducts.Tag = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"هەڵە لە بارکردنی کاڵاکان: {ex.Message}", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnProductSearch_Click(object sender, EventArgs e)
        {
            SearchProductByBarcode();
        }

        private void txtBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchProductByBarcode();
                e.Handled = true;
            }
        }

        private void SearchProductByBarcode()
        {
            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                MessageBox.Show("تکایە بارکۆد بنووسە!", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = @"SELECT ProductID, ProductName, Barcode, SellPrice, Quantity 
                                FROM Products 
                                WHERE Barcode = @barcode";
                SqlParameter[] parameters = { new SqlParameter("@barcode", txtBarcode.Text.Trim()) };

                DataTable dt = DatabaseConnection.ExecuteReader(query, parameters);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int stockQty = Convert.ToInt32(dt.Rows[0]["Quantity"]);

                    if (stockQty <= 0)
                    {
                        MessageBox.Show("ئەم کاڵایە لە کۆگا نییە!", "هەڵە",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // تحديد المنتج في ComboBox
                    int productId = Convert.ToInt32(dt.Rows[0]["ProductID"]);
                    cmbProducts.SelectedValue = productId;

                    txtPrice.Text = dt.Rows[0]["SellPrice"].ToString();
                    txtQuantity.Focus();
                    txtQuantity.SelectAll();
                }
                else
                {
                    MessageBox.Show("کاڵا نەدۆزرایەوە!", "هەڵە",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtBarcode.Clear();
                    txtBarcode.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"هەڵە: {ex.Message}", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedValue == null)
                return;

            try
            {
                DataTable dt = cmbProducts.Tag as DataTable;
                if (dt != null)
                {
                    DataRow[] rows = dt.Select($"ProductID = {cmbProducts.SelectedValue}");
                    if (rows.Length > 0)
                    {
                        txtBarcode.Text = rows[0]["Barcode"].ToString();
                        txtPrice.Text = rows[0]["SellPrice"].ToString();
                    }
                }
            }
            catch { }
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedValue == null)
            {
                MessageBox.Show("تکایە کاڵایەک هەڵبژێرە!", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int quantity;
            if (!int.TryParse(txtQuantity.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("بڕی دروست بنووسە!", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return;
            }

            // التحقق من الكمية المتاحة
            try
            {
                string checkQuery = "SELECT Quantity FROM Products WHERE Barcode = @barcode";
                SqlParameter[] checkParams = { new SqlParameter("@barcode", txtBarcode.Text) };
                DataTable checkDt = DatabaseConnection.ExecuteReader(checkQuery, checkParams);

                if (checkDt != null && checkDt.Rows.Count > 0)
                {
                    int stockQty = Convert.ToInt32(checkDt.Rows[0]["Quantity"]);

                    // حساب الكمية الموجودة في السلة
                    int cartQty = 0;
                    foreach (DataRow row in cartTable.Rows)
                    {
                        if (row["بارکۆد"].ToString() == txtBarcode.Text)
                        {
                            cartQty = Convert.ToInt32(row["بڕ"]);
                            break;
                        }
                    }

                    if (cartQty + quantity > stockQty)
                    {
                        MessageBox.Show($"دانەی کاڵا لە کۆگا بەسە! (دانەی مەوجود: {stockQty})", "هەڵە",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"هەڵە: {ex.Message}", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal price = Convert.ToDecimal(txtPrice.Text);
            decimal total = price * quantity;

            // التحقق من وجود المنتج في السلة
            bool found = false;
            foreach (DataRow row in cartTable.Rows)
            {
                if (row["بارکۆد"].ToString() == txtBarcode.Text)
                {
                    int oldQty = Convert.ToInt32(row["بڕ"]);
                    row["بڕ"] = oldQty + quantity;
                    row["کۆ"] = (oldQty + quantity) * price;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                cartTable.Rows.Add(
                    txtBarcode.Text,
                    cmbProducts.Text,
                    price,
                    quantity,
                    total
                );
            }

            CalculateTotal();
            ClearProductFields();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCart.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("دڵنیای لە لابردنی ئەم کاڵایە؟", "دڵنیابوونەوە",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    dgvCart.Rows.Remove(dgvCart.SelectedRows[0]);
                    CalculateTotal();
                }
            }
            else
            {
                MessageBox.Show("تکایە ڕیزێک هەڵبژێرە!", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClearCart_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("سەبەتە بەتاڵە!", "زانیاری",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("دڵنیای لە پاککردنەوەی سەبەتە؟", "دڵنیابوونەوە",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                cartTable.Clear();
                CalculateTotal();
            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            totalAmount = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                totalAmount += Convert.ToDecimal(row["کۆ"]);
            }

            lblTotalAmount.Text = totalAmount.ToString("N0") + " دینار";

            decimal discount = 0;
            decimal.TryParse(txtDiscount.Text, out discount);

            if (discount > totalAmount)
            {
                MessageBox.Show("داشکاندن لە کۆی گشتی زیاتر نابێت!", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiscount.Text = "0";
                discount = 0;
            }

            decimal netAmount = totalAmount - discount;
            lblNetAmount.Text = netAmount.ToString("N0") + " دینار";
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0)
            {
                MessageBox.Show("سەبەتە بەتاڵە!", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbCustomers.SelectedValue == null)
            {
                MessageBox.Show("تکایە کڕیارێک هەڵبژێرە!", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCustomers.Focus();
                return;
            }

            try
            {
                decimal discount = 0;
                decimal.TryParse(txtDiscount.Text, out discount);
                decimal netAmount = totalAmount - discount;

                string invoiceNumber = "INV" + DateTime.Now.ToString("yyyyMMddHHmmss");

                // حفظ الفاتورة
                string saleQuery = @"INSERT INTO Sales (InvoiceNumber, CustomerID, CustomerName, TotalAmount, PaidAmount, RemainingAmount, UserID) 
                                    VALUES (@invoice, @customerId, @customerName, @total, @paid, @remaining, @userId); 
                                    SELECT SCOPE_IDENTITY();";

                SqlParameter[] saleParams = {
                    new SqlParameter("@invoice", invoiceNumber),
                    new SqlParameter("@customerId", cmbCustomers.SelectedValue),
                    new SqlParameter("@customerName", cmbCustomers.Text),
                    new SqlParameter("@total", netAmount),
                    new SqlParameter("@paid", netAmount),
                    new SqlParameter("@remaining", 0),
                    new SqlParameter("@userId", 1) // استبدل بـ LoginForm.LoggedInUserID
                };

                object result = DatabaseConnection.ExecuteScalar(saleQuery, saleParams);

                if (result != null)
                {
                    int saleID = Convert.ToInt32(result);

                    // حفظ تفاصيل الفاتورة وتحديث المخزون
                    foreach (DataRow row in cartTable.Rows)
                    {
                        string detailQuery = @"INSERT INTO SaleDetails (SaleID, ProductID, Barcode, ProductName, Quantity, UnitPrice, TotalPrice) 
                                              SELECT @saleID, ProductID, @barcode, @productName, @qty, @price, @total 
                                              FROM Products WHERE Barcode = @barcode;
                                              
                                              UPDATE Products SET Quantity = Quantity - @qty WHERE Barcode = @barcode;";

                        SqlParameter[] detailParams = {
                            new SqlParameter("@saleID", saleID),
                            new SqlParameter("@barcode", row["بارکۆد"]),
                            new SqlParameter("@productName", row["ناوی کاڵا"]),
                            new SqlParameter("@qty", row["بڕ"]),
                            new SqlParameter("@price", row["نرخ"]),
                            new SqlParameter("@total", row["کۆ"])
                        };

                        DatabaseConnection.ExecuteQuery(detailQuery, detailParams);
                    }

                    MessageBox.Show($"فرۆشتن سەرکەوتوو بوو!\n\nژمارەی وەسڵ: {invoiceNumber}\nکۆی گشتی: {totalAmount:N0} دینار\nداشکاندن: {discount:N0} دینار\nخاوی: {netAmount:N0} دینار",
                        "سەرکەوتوو", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // تحديث قائمة المنتجات
                    LoadProducts();

                    // تنظيف النموذج
                    cartTable.Clear();
                    txtDiscount.Text = "0";
                    CalculateTotal();
                    ClearProductFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"هەڵە لە تەواوکردنی فرۆشتن:\n{ex.Message}", "هەڵە",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearProductFields()
        {
            txtBarcode.Clear();
            txtPrice.Text = "0";
            txtQuantity.Text = "1";
            txtBarcode.Focus();

            // تحديد أول عنصر في ComboBox
            if (cmbProducts.Items.Count > 0)
                cmbProducts.SelectedIndex = 0;
        }
    }
}