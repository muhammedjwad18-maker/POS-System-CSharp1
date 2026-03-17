using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class ReceiptForm : Form
    {
        private DataTable cart;
        private decimal total;

        public ReceiptForm(DataTable cartData, decimal totalAmount)
        {
            InitializeComponent();
            cart = cartData;
            total = totalAmount;
            LoadReceipt();
        }

        private void LoadReceipt()
        {
            lblShop.Text = "My Mini Market";
            lblDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            lblTotal.Text = total.ToString("0.00");

            dgvReceipt.DataSource = cart;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += PrintPage;
            PrintDialog p = new PrintDialog();
            p.Document = pd;

            if (p.ShowDialog() == DialogResult.OK)
                pd.Print();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            float y = 10;
            Font f = new Font("Consolas", 10);
            e.Graphics.DrawString("My Mini Market", new Font("Consolas", 14, FontStyle.Bold), Brushes.Black, 10, y);
            y += 30;
            e.Graphics.DrawString(DateTime.Now.ToString(), f, Brushes.Black, 10, y);
            y += 30;

            foreach (DataRow r in cart.Rows)
            {
                string line = $"{r["Name"]}  x{r["Qty"]}  {r["Price"]}";
                e.Graphics.DrawString(line, f, Brushes.Black, 10, y);
                y += 20;
            }

            y += 20;
            e.Graphics.DrawString("Total: " + total.ToString("0.00"), new Font("Consolas", 12, FontStyle.Bold), Brushes.Black, 10, y);
        }
    }
}
