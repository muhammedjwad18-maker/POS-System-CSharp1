using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventorySystem.Forms
{
    public partial class ProductsForm : Form
    {
        private string connectionString =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Hi\Documents\InventorySystem.mdf;Integrated Security=True";

        public ProductsForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        // ===============================
        // Load products into DataGridView
        // ===============================
        private void LoadProducts()
        {
            dgvProducts.Rows.Clear();

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string query = @"SELECT p.Id, p.Name, c.Name AS Category, p.CostPrice, p.SellPrice, p.Quantity
                                     FROM Products p
                                     LEFT JOIN Categories c ON p.CategoryId = c.Id";

                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        dgvProducts.Rows.Add(
                            dr["Id"],
                            dr["Name"],
                            dr["Category"],
                            dr["CostPrice"],
                            dr["SellPrice"],
                            dr["Quantity"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        // ===============================
        // ADD
        // ===============================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductDetailsForm form = new ProductDetailsForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        // ===============================
        // EDIT
        // ===============================
        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product");
                return;
            }

            DataGridViewRow row = dgvProducts.SelectedRows[0];

            ProductDetailsForm form = new ProductDetailsForm();
            form.ProductId = Convert.ToInt32(row.Cells[0].Value);
            form.txtName.Text = row.Cells[1].Value.ToString();
            form.txtCostPrice.Text = row.Cells[3].Value.ToString();
            form.txtSellPrice.Text = row.Cells[4].Value.ToString();
            form.txtQuantity.Text = row.Cells[5].Value.ToString();

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        // ===============================
        // DELETE
        // ===============================
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select product first");
                return;
            }

            int id = Convert.ToInt32(dgvProducts.SelectedRows[0].Cells[0].Value);

            if (MessageBox.Show("Delete this product?", "Confirm",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Products WHERE Id=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }

                LoadProducts();
            }
        }

        // ===============================
        // SEARCH
        // ===============================
        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            dgvProducts.Rows.Clear();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    @"SELECT p.Id, p.Name, c.Name AS Category, p.CostPrice, p.SellPrice, p.Quantity
                      FROM Products p
                      LEFT JOIN Categories c ON p.CategoryId = c.Id
                      WHERE p.Name LIKE @s", con);

                cmd.Parameters.AddWithValue("@s", "%" + txtSearch.Text + "%");

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    dgvProducts.Rows.Add(
                        dr["Id"],
                        dr["Name"],
                        dr["Category"],
                        dr["CostPrice"],
                        dr["SellPrice"],
                        dr["Quantity"]
                    );
                }
            }
        }

        // ===============================
        // REFRESH
        // ===============================
        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadProducts();
        }

        private void grpProductInfo_Enter(object sender, EventArgs e)
        {

        }
    }
}
