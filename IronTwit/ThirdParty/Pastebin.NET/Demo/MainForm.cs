using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenPasteAPI;

namespace DemoPastebin
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load list of available syntax items
            LoadSyntaxList();
        }

        private void LoadSyntaxList()
        {
            cmbSyntax.DataSource = OpenPasteSyntaxItem.GeSupportedSyntaxList();
            cmbSyntax.DisplayMember = "Description";
            cmbSyntax.ValueMember = "Acronym";
        }

        private void chkUsePassword_CheckStateChanged(object sender, EventArgs e)
        {
            // enable/disable password textbox base on checkbox value
            txtPassword.Enabled = chkUsePassword.Checked;
            if (!txtPassword.Enabled) txtPassword.Text = string.Empty;
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                // create new pastebin post
                OpenPaste p = new OpenPaste();
                p.AuthorName = txtAuthor.Text;
                p.Code = rtxtCode.Text;
                p.Description = txtDescription.Text;
                p.IsPrivate = chkUsePassword.Checked;
                p.Password = txtPassword.Text;
                p.SyntaxAcronym = cmbSyntax.SelectedValue.ToString();
                p.SecretKey = txtSecretKey.Text;
                // save post
                p.Save();
                // navigate browser to post URL
                browser.Navigate(p.Url);
                // activate browser tab
                tabs.SelectTab(tabResult);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error when saving Pastebin post.");
            }
        }

        
        
    }
}