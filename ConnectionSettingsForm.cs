using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class ConnectionSettingsForm : Form
    {

        public ConnectionSettingsForm()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }
        private void LoadCurrentSettings()
        {
            // تحميل الإعدادات الحالية
            txtServer.Text = ".";
            txtDatabase.Text = "InventorySystemDB";
            cmbAuth.SelectedIndex = 0; // Windows Authentication
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            string connectionString = BuildConnectionString();

            try
            {
                using (System.Data.SqlClient.SqlConnection conn =
                       new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    conn.Open();
                    MessageBox.Show("✅ پەیوەندی سەرکەوتووبوو!", "سەرکەوتوویی");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ هەڵە: {ex.Message}", "هەڵە");
            }
        }

        private string BuildConnectionString()
        {
            string server = txtServer.Text;
            string database = txtDatabase.Text;

            if (cmbAuth.SelectedIndex == 0) // Windows Auth
            {
                return $"Server={server};Database={database};Integrated Security=True;";
            }
            else // SQL Auth
            {
                return $"Server={server};Database={database};User Id={txtUsername.Text};Password={txtPassword.Text};";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string connectionString = BuildConnectionString();

            // حفظ في ConfigHelper
            ConfigHelper.ConnectionString = connectionString;

            // حفظ في ملف
            ConfigHelper.SaveConnectionString(connectionString);

            MessageBox.Show("ڕێکخستنەکان پاشەکەوتکران", "سەرکەوتوویی");
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}