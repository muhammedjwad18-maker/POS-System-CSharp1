using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class CategoriesForm : Form
    { private SqlConnection connection;
        public CategoriesForm()
        {
            InitializeComponent();       
                InitializeDatabase();
                ApplyKurdishTheme();
                LoadCategories();
            }

            private void InitializeDatabase()
            {
                string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Hi\\Documents\\InventorySystem.mdf;Integrated Security=True;Connect Timeout=30;";
                connection = new SqlConnection(connectionString);
            }

            private void ApplyKurdishTheme()
            {
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = true;
                this.Font = new Font("NRT Reg", 10.5f);
            }

            private void LoadCategories()
            {
                try
                {
                    connection.Open();
                    string query = "SELECT CategoryID, CategoryName, Description FROM Categories ORDER BY CategoryName";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvCategories.DataSource = dt;
                    dgvCategories.Columns["CategoryID"].HeaderText = "ژمارە";
                    dgvCategories.Columns["CategoryName"].HeaderText = "ناوی فەیڵ";
                    dgvCategories.Columns["Description"].HeaderText = "شرح";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("هەڵە لە بارکردنی فەیڵەکان: " + ex.Message,
                        "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }

            private void btnSave_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    MessageBox.Show("ناوی فەیڵ بنووسە", "هەڵە",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SaveCategory();
            }

            private void SaveCategory()
            {
                try
                {
                    connection.Open();
                    string query = @"IF EXISTS(SELECT 1 FROM Categories WHERE CategoryID = @CategoryID)
                    UPDATE Categories SET CategoryName = @CategoryName, Description = @Description 
                    WHERE CategoryID = @CategoryID
                ELSE
                    INSERT INTO Categories (CategoryName, Description) 
                    VALUES (@CategoryName, @Description)";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@CategoryID",
                        lblCategoryId.Text == "0" ? 0 : Convert.ToInt32(lblCategoryId.Text));
                    cmd.Parameters.AddWithValue("@CategoryName", txtCategoryName.Text);
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("فەیڵەکە تۆمارکرا", "سەرکەوتوویی",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();
                    LoadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("هەڵە لە تۆمارکردنی فەیڵ: " + ex.Message,
                        "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }

            private void ClearForm()
            {
                lblCategoryId.Text = "0";
                txtCategoryName.Clear();
                txtDescription.Clear();
                btnSave.Text = "پاشەکەوت";
                btnDelete.Enabled = false;
            }

            private void dgvCategories_CellClick(object sender, DataGridViewCellEventArgs e)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dgvCategories.Rows[e.RowIndex];
                    lblCategoryId.Text = row.Cells["CategoryID"].Value.ToString();
                    txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();
                    txtDescription.Text = row.Cells["Description"].Value?.ToString() ?? "";
                    btnSave.Text = "نوێکردنەوە";
                    btnDelete.Enabled = true;
                }
            }

            private void btnDelete_Click(object sender, EventArgs e)
            {
                if (lblCategoryId.Text != "0")
                {
                    DialogResult result = MessageBox.Show("دڵنیایت دەتەوێت ئەم فەیڵە بسڕیتەوە؟",
                        "سڕینەوە", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        DeleteCategory();
                    }
                }
            }

            private void DeleteCategory()
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@CategoryID", Convert.ToInt32(lblCategoryId.Text));
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("فەیڵەکە سڕدرایەوە", "سەرکەوتوویی",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ClearForm();
                    LoadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("هەڵە لە سڕینەوەی فەیڵ: " + ex.Message,
                        "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }

            private void btnNew_Click(object sender, EventArgs e)
            {
                ClearForm();
            }

            private void txtSearch_TextChanged(object sender, EventArgs e)
            {
                if (dgvCategories.DataSource is DataTable dt)
                {
                    string searchText = txtSearch.Text.Trim();
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        dt.DefaultView.RowFilter = $"CategoryName LIKE '%{searchText}%' OR Description LIKE '%{searchText}%'";
                    }
                    else
                    {
                        dt.DefaultView.RowFilter = "";
                    }
                }
            }
        }
    }

