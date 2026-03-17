using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventorySystem
{
    public partial class ActivationForm : Form
    {
        private string hwid;
        private string activationFile = "activation.key";
        public ActivationForm()
        {
            InitializeComponent();
        }

        private void ActivationForm_Load(object sender, EventArgs e)
        {
            hwid = GetHWID();
            lblHWID.Text = $"HWID: {hwid}";
        }

        private string GetHWID()
        {
            string cpuID = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                cpuID = obj["ProcessorId"].ToString();
                break;
            }
            return cpuID;
        }

        private string GenerateActivationCode(string hwid)
        {
            string secretKey = "YourSecretKey123";
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(hwid + secretKey));
                return BitConverter.ToString(hash).Replace("-", "").Substring(0, 16).ToUpper();
            }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            string userCode = txtActivationCode.Text.Trim().ToUpper();
            string correctCode = GenerateActivationCode(hwid);

            if (userCode == correctCode)
            {
                File.WriteAllText(activationFile, correctCode);
                MessageBox.Show("Done!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("the Code is worn.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public static bool IsActivated()
        {
            if (File.Exists("activation.key"))
            {
                string hwid = new ActivationForm().GetHWID();
                string correctCode = new ActivationForm().GenerateActivationCode(hwid);
                string storedCode = File.ReadAllText("activation.key").Trim();

                return storedCode == correctCode;
            }
            return false;
        }

       

        private void lblHWID_Click_1(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void lblCode_Click(object sender, EventArgs e)
        {

        }

        private void txtActivationCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
