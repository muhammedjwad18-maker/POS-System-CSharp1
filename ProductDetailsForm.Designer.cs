using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InventorySystem
{
    partial class ProductDetailsForm
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtProductName;
        private TextBox txtKurdishName;
        private TextBox txtArabicName;
        private TextBox txtBarcode;
        private TextBox txtQuantity;
        private TextBox txtMinQuantity;
        private TextBox txtPrice;
        private TextBox txtCost;
        private ComboBox cmbUnitType;
        private ComboBox cmbCategory;
        private DateTimePicker dtpExpiryDate;
        private Button btnSave;
        private Button btnGenerateBarcode;
        private Label lblProductName;
        private Label lblKurdishName;
        private Label lblArabicName;
        private Label lblBarcode;
        private Label lblUnitType;
        private Label lblQuantity;
        private Label lblMinQuantity;
        private Label lblPrice;
        private Label lblCost;
        private Label lblExpiryDate;
        private Label lblCategory;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.txtKurdishName = new System.Windows.Forms.TextBox();
            this.txtArabicName = new System.Windows.Forms.TextBox();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtMinQuantity = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.cmbUnitType = new System.Windows.Forms.ComboBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.dtpExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGenerateBarcode = new System.Windows.Forms.Button();
            this.lblProductName = new System.Windows.Forms.Label();
            this.lblKurdishName = new System.Windows.Forms.Label();
            this.lblArabicName = new System.Windows.Forms.Label();
            this.lblBarcode = new System.Windows.Forms.Label();
            this.lblUnitType = new System.Windows.Forms.Label();
            this.lblQuantity = new System.Windows.Forms.Label();
            this.lblMinQuantity = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblCost = new System.Windows.Forms.Label();
            this.lblExpiryDate = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(180, 20);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(200, 20);
            this.txtProductName.TabIndex = 1;
            // 
            // txtKurdishName
            // 
            this.txtKurdishName.Location = new System.Drawing.Point(180, 60);
            this.txtKurdishName.Name = "txtKurdishName";
            this.txtKurdishName.Size = new System.Drawing.Size(200, 20);
            this.txtKurdishName.TabIndex = 3;
            // 
            // txtArabicName
            // 
            this.txtArabicName.Location = new System.Drawing.Point(180, 100);
            this.txtArabicName.Name = "txtArabicName";
            this.txtArabicName.Size = new System.Drawing.Size(200, 20);
            this.txtArabicName.TabIndex = 5;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(180, 140);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(150, 20);
            this.txtBarcode.TabIndex = 7;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(180, 220);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(100, 20);
            this.txtQuantity.TabIndex = 12;
            this.txtQuantity.Text = "0";
            // 
            // txtMinQuantity
            // 
            this.txtMinQuantity.Location = new System.Drawing.Point(180, 260);
            this.txtMinQuantity.Name = "txtMinQuantity";
            this.txtMinQuantity.Size = new System.Drawing.Size(100, 20);
            this.txtMinQuantity.TabIndex = 14;
            this.txtMinQuantity.Text = "10";
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(180, 300);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 20);
            this.txtPrice.TabIndex = 16;
            this.txtPrice.Text = "0";
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(180, 340);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(100, 20);
            this.txtCost.TabIndex = 18;
            this.txtCost.Text = "0";
            // 
            // cmbUnitType
            // 
            this.cmbUnitType.Items.AddRange(new object[] {
            "كارتن",
            "وحدة"});
            this.cmbUnitType.Location = new System.Drawing.Point(180, 180);
            this.cmbUnitType.Name = "cmbUnitType";
            this.cmbUnitType.Size = new System.Drawing.Size(150, 21);
            this.cmbUnitType.TabIndex = 10;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Location = new System.Drawing.Point(180, 420);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(200, 21);
            this.cmbCategory.TabIndex = 22;
            // 
            // dtpExpiryDate
            // 
            this.dtpExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpExpiryDate.Location = new System.Drawing.Point(180, 380);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.Size = new System.Drawing.Size(150, 20);
            this.dtpExpiryDate.TabIndex = 20;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(180, 460);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "پاشەکەوت";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGenerateBarcode
            // 
            this.btnGenerateBarcode.Location = new System.Drawing.Point(340, 140);
            this.btnGenerateBarcode.Name = "btnGenerateBarcode";
            this.btnGenerateBarcode.Size = new System.Drawing.Size(40, 25);
            this.btnGenerateBarcode.TabIndex = 8;
            this.btnGenerateBarcode.Text = "...";
            // 
            // lblProductName
            // 
            this.lblProductName.Location = new System.Drawing.Point(20, 20);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(150, 25);
            this.lblProductName.TabIndex = 0;
            this.lblProductName.Text = "ناوی کاڵا (ئینگلیزی):";
            // 
            // lblKurdishName
            // 
            this.lblKurdishName.Location = new System.Drawing.Point(20, 60);
            this.lblKurdishName.Name = "lblKurdishName";
            this.lblKurdishName.Size = new System.Drawing.Size(100, 23);
            this.lblKurdishName.TabIndex = 2;
            this.lblKurdishName.Text = "ناوی کوردی:";
            // 
            // lblArabicName
            // 
            this.lblArabicName.Location = new System.Drawing.Point(20, 100);
            this.lblArabicName.Name = "lblArabicName";
            this.lblArabicName.Size = new System.Drawing.Size(100, 23);
            this.lblArabicName.TabIndex = 4;
            this.lblArabicName.Text = "ناوی عەرەبی:";
            // 
            // lblBarcode
            // 
            this.lblBarcode.Location = new System.Drawing.Point(20, 140);
            this.lblBarcode.Name = "lblBarcode";
            this.lblBarcode.Size = new System.Drawing.Size(100, 23);
            this.lblBarcode.TabIndex = 6;
            this.lblBarcode.Text = "بارکۆد:";
            // 
            // lblUnitType
            // 
            this.lblUnitType.Location = new System.Drawing.Point(20, 180);
            this.lblUnitType.Name = "lblUnitType";
            this.lblUnitType.Size = new System.Drawing.Size(100, 23);
            this.lblUnitType.TabIndex = 9;
            this.lblUnitType.Text = "جۆری یەکە:";
            // 
            // lblQuantity
            // 
            this.lblQuantity.Location = new System.Drawing.Point(20, 220);
            this.lblQuantity.Name = "lblQuantity";
            this.lblQuantity.Size = new System.Drawing.Size(100, 23);
            this.lblQuantity.TabIndex = 11;
            this.lblQuantity.Text = "بڕ:";
            // 
            // lblMinQuantity
            // 
            this.lblMinQuantity.Location = new System.Drawing.Point(20, 260);
            this.lblMinQuantity.Name = "lblMinQuantity";
            this.lblMinQuantity.Size = new System.Drawing.Size(100, 23);
            this.lblMinQuantity.TabIndex = 13;
            this.lblMinQuantity.Text = "کەمترین بڕ:";
            // 
            // lblPrice
            // 
            this.lblPrice.Location = new System.Drawing.Point(20, 300);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(100, 23);
            this.lblPrice.TabIndex = 15;
            this.lblPrice.Text = "نرخ:";
            // 
            // lblCost
            // 
            this.lblCost.Location = new System.Drawing.Point(20, 340);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(100, 23);
            this.lblCost.TabIndex = 17;
            this.lblCost.Text = "تێچوو:";
            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.Location = new System.Drawing.Point(20, 380);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new System.Drawing.Size(100, 23);
            this.lblExpiryDate.TabIndex = 19;
            this.lblExpiryDate.Text = "بەرواری کۆتایی:";
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(20, 420);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(100, 23);
            this.lblCategory.TabIndex = 21;
            this.lblCategory.Text = "فەیڵ:";
            // 
            // ProductDetailsForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 520);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.txtProductName);
            this.Controls.Add(this.lblKurdishName);
            this.Controls.Add(this.txtKurdishName);
            this.Controls.Add(this.lblArabicName);
            this.Controls.Add(this.txtArabicName);
            this.Controls.Add(this.lblBarcode);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.btnGenerateBarcode);
            this.Controls.Add(this.lblUnitType);
            this.Controls.Add(this.cmbUnitType);
            this.Controls.Add(this.lblQuantity);
            this.Controls.Add(this.txtQuantity);
            this.Controls.Add(this.lblMinQuantity);
            this.Controls.Add(this.txtMinQuantity);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.lblCost);
            this.Controls.Add(this.txtCost);
            this.Controls.Add(this.lblExpiryDate);
            this.Controls.Add(this.dtpExpiryDate);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProductDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تۆمارکردنی کاڵای نوێ";
            this.Load += new System.EventHandler(this.ProductDetailsForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }


}