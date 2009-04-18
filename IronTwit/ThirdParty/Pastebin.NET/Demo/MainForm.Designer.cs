namespace DemoPastebin
{
    partial class MainForm
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
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabDef = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblAuthor = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.chkUsePassword = new System.Windows.Forms.CheckBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cmbSyntax = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rtxtCode = new System.Windows.Forms.RichTextBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.tabResult = new System.Windows.Forms.TabPage();
            this.browser = new System.Windows.Forms.WebBrowser();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.tabs.SuspendLayout();
            this.tabDef.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabDef);
            this.tabs.Controls.Add(this.tabResult);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(934, 707);
            this.tabs.TabIndex = 0;
            // 
            // tabDef
            // 
            this.tabDef.Controls.Add(this.tableLayoutPanel1);
            this.tabDef.Location = new System.Drawing.Point(4, 22);
            this.tabDef.Name = "tabDef";
            this.tabDef.Padding = new System.Windows.Forms.Padding(3);
            this.tabDef.Size = new System.Drawing.Size(926, 681);
            this.tabDef.TabIndex = 0;
            this.tabDef.Text = "Post Definition";
            this.tabDef.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lblAuthor, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtAuthor, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkUsePassword, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPassword, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtDescription, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbSyntax, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.rtxtCode, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnProcess, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.txtSecretKey, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 416F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(920, 675);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblAuthor
            // 
            this.lblAuthor.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblAuthor.AutoSize = true;
            this.lblAuthor.Location = new System.Drawing.Point(159, 8);
            this.lblAuthor.Name = "lblAuthor";
            this.lblAuthor.Size = new System.Drawing.Size(38, 13);
            this.lblAuthor.TabIndex = 0;
            this.lblAuthor.Text = "Author";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Password protection";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(137, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Description";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(158, 128);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Syntax";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(138, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Secret Key";
            // 
            // txtAuthor
            // 
            this.txtAuthor.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtAuthor.Location = new System.Drawing.Point(203, 5);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new System.Drawing.Size(164, 20);
            this.txtAuthor.TabIndex = 7;
            // 
            // chkUsePassword
            // 
            this.chkUsePassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkUsePassword.AutoSize = true;
            this.chkUsePassword.Location = new System.Drawing.Point(203, 38);
            this.chkUsePassword.Name = "chkUsePassword";
            this.chkUsePassword.Size = new System.Drawing.Size(15, 14);
            this.chkUsePassword.TabIndex = 9;
            this.chkUsePassword.UseVisualStyleBackColor = true;
            this.chkUsePassword.CheckStateChanged += new System.EventHandler(this.chkUsePassword_CheckStateChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(203, 65);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(164, 20);
            this.txtPassword.TabIndex = 10;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDescription.Location = new System.Drawing.Point(203, 95);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(353, 20);
            this.txtDescription.TabIndex = 11;
            // 
            // cmbSyntax
            // 
            this.cmbSyntax.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbSyntax.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSyntax.FormattingEnabled = true;
            this.cmbSyntax.Location = new System.Drawing.Point(203, 124);
            this.cmbSyntax.Name = "cmbSyntax";
            this.cmbSyntax.Size = new System.Drawing.Size(164, 21);
            this.cmbSyntax.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(165, 180);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Code";
            // 
            // rtxtCode
            // 
            this.rtxtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtxtCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtCode.Location = new System.Drawing.Point(203, 183);
            this.rtxtCode.Name = "rtxtCode";
            this.rtxtCode.Size = new System.Drawing.Size(714, 410);
            this.rtxtCode.TabIndex = 15;
            this.rtxtCode.Text = "";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(203, 599);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 16;
            this.btnProcess.Text = "OK";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // tabResult
            // 
            this.tabResult.Controls.Add(this.browser);
            this.tabResult.Location = new System.Drawing.Point(4, 22);
            this.tabResult.Name = "tabResult";
            this.tabResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabResult.Size = new System.Drawing.Size(926, 681);
            this.tabResult.TabIndex = 1;
            this.tabResult.Text = "Post Result";
            this.tabResult.UseVisualStyleBackColor = true;
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(3, 3);
            this.browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(920, 675);
            this.browser.TabIndex = 0;
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtSecretKey.Location = new System.Drawing.Point(203, 155);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(164, 20);
            this.txtSecretKey.TabIndex = 17;
            this.txtSecretKey.UseSystemPasswordChar = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 707);
            this.Controls.Add(this.tabs);
            this.Name = "MainForm";
            this.Text = "Pastebin API Demo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabs.ResumeLayout(false);
            this.tabDef.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabResult.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabDef;
        private System.Windows.Forms.TabPage tabResult;
        private System.Windows.Forms.WebBrowser browser;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblAuthor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.CheckBox chkUsePassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbSyntax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox rtxtCode;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox txtSecretKey;

    }
}

