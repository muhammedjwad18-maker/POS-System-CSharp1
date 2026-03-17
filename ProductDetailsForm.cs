using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class ProductDetailsForm : Form
    {
        private SqlConnection connection;
        private int productId = 0;
        public ProductDetailsForm()
        {
            InitializeComponent();
            InitializeDatabase();
            LoadCategories();
            GenerateBarcode();
        }
        private void InitializeDatabase()
        {
            string connectionString = "Server=.;Database=StoreManagementDB;Integrated Security=True;";
            connection = new SqlConnection(connectionString);
        }

        private void GenerateBarcode()
        {
            txtBarcode.Text = "PRD" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void LoadCategories()
        {
            try
            {
                connection.Open();
                string query = "SELECT CategoryID, CategoryName FROM Categories";
                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                cmbCategory.DataSource = dt;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("هەڵە لە بارکردنی فەیڵەکان: " + ex.Message, "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                SaveProduct();
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("ناوی کاڵا بنووسە", "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                MessageBox.Show("بارکۆد بنووسە", "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SaveProduct()
        {
            try
            {
                connection.Open();
                string query = @"INSERT INTO Products 
                                (Barcode, ProductName, KurdishName, ArabicName, 
                                 UnitType, Quantity, MinQuantity, ExpiryDate, 
                                 CategoryID, Price, Cost)
                                VALUES 
                                (@Barcode, @ProductName, @KurdishName, @ArabicName,
                                 @UnitType, @Quantity, @MinQuantity, @ExpiryDate,
                                 @CategoryID, @Price, @Cost)";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Barcode", txtBarcode.Text);
                cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                cmd.Parameters.AddWithValue("@KurdishName", txtKurdishName.Text);
                cmd.Parameters.AddWithValue("@ArabicName", txtArabicName.Text);
                cmd.Parameters.AddWithValue("@UnitType", cmbUnitType.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text));
                cmd.Parameters.AddWithValue("@MinQuantity", Convert.ToInt32(txtMinQuantity.Text));
                cmd.Parameters.AddWithValue("@ExpiryDate", dtpExpiryDate.Value);
                cmd.Parameters.AddWithValue("@CategoryID", cmbCategory.SelectedValue);
                cmd.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                cmd.Parameters.AddWithValue("@Cost", Convert.ToDecimal(txtCost.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("کاڵاکە تۆمارکرا", "سەرکەوتوویی", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("هەڵە لە تۆمارکردنی کاڵا: " + ex.Message, "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void ClearForm()
        {
            txtProductName.Clear();
            txtKurdishName.Clear();
            txtArabicName.Clear();
            txtQuantity.Text = "0";
            txtMinQuantity.Text = "10";
            txtPrice.Text = "0";
            txtCost.Text = "0";
            GenerateBarcode();
        }

        private void btnGenerateBarcode_Click(object sender, EventArgs e)
        {
            GenerateBarcode();
        }

        private void CheckLowStock()
        {
            try
            {
                connection.Open();
                string query = @"SELECT ProductName, Quantity, MinQuantity 
                               FROM Products 
                               WHERE Quantity <= MinQuantity";

                SqlCommand cmd = new SqlCommand(query, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    string message = "ئاگاداری: کەمبوونەوەی کاڵا\n\n";
                    while (reader.Read())
                    {
                        message += $"{reader["ProductName"]}: {reader["Quantity"]} / {reader["MinQuantity"]}\n";
                    }
                    MessageBox.Show(message, "ئاگاداری", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("هەڵە لە پشکنینی کاڵاکان: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void ProductDetailsForm_Load_1(object sender, EventArgs e)
        {
            // تحميل التصميم الكردي
            ApplyKurdishTheme();
            CheckLowStock();
        }

        private void ApplyKurdishTheme()
        {
            // تخصيص النموذج للغة الكردية
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.Font = new Font("Sakkal Majalla", 12, FontStyle.Regular);

            // تخصيص جميع عناصر التحكم
            foreach (Control control in this.Controls)
            {
                control.Font = new Font("Sakkal Majalla", 12, FontStyle.Regular);
                control.RightToLeft = RightToLeft.Yes;
            }
        }

       
    }

}

