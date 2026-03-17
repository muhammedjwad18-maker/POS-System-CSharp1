namespace InventorySystem
{
    partial class ActivationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblHWID = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.txtActivationCode = new System.Windows.Forms.TextBox();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(38, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(340, 30);
            this.lblTitle.TabIndex = 6;
            this.lblTitle.Text = "Activation";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // lblHWID
            // 
            this.lblHWID.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblHWID.Location = new System.Drawing.Point(78, 50);
            this.lblHWID.Name = "lblHWID";
            this.lblHWID.Size = new System.Drawing.Size(255, 40);
            this.lblHWID.TabIndex = 7;
            this.lblHWID.Click += new System.EventHandler(this.lblHWID_Click_1);
            // 
            // lblCode
            // 
            this.lblCode.Location = new System.Drawing.Point(39, 104);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(100, 23);
            this.lblCode.TabIndex = 8;
            this.lblCode.Text = "Activation code :";
            this.lblCode.Click += new System.EventHandler(this.lblCode_Click);
            // 
            // txtActivationCode
            // 
            this.txtActivationCode.Location = new System.Drawing.Point(59, 130);
            this.txtActivationCode.Name = "txtActivationCode";
            this.txtActivationCode.Size = new System.Drawing.Size(340, 20);
            this.txtActivationCode.TabIndex = 9;
            this.txtActivationCode.TextChanged += new System.EventHandler(this.txtActivationCode_TextChanged);
            // 
            // btnActivate
            // 
            this.btnActivate.Location = new System.Drawing.Point(113, 175);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(100, 30);
            this.btnActivate.TabIndex = 10;
            this.btnActivate.Text = "OK";
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(233, 175);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 30);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // ActivationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 241);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblHWID);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.txtActivationCode);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.btnExit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ActivationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ActivationForm";
            this.Load += new System.EventHandler(this.ActivationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblHWID;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtActivationCode;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Button btnExit;
    }
}