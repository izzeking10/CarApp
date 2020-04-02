using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            txtRegNr.Focus();
        }
        #region Events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRegNr.Text) || string.IsNullOrEmpty(txtMake.Text))
                {
                MessageBox.Show("Du måste fylla i  alla rutor", "Felaktig inmatning");
                }
            else
               {

                ListViewItem item = CreateListViewItem(txtRegNr.Text, txtMake.Text, cbxForSale.Checked);
                lsvCars.Items.Add(item);
                ClearTextboxes();
                btnClear.Enabled = true;
                }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lsvCars.SelectedItems.Count > 0)
            {
                var item = lsvCars.SelectedItems[0];
                lsvCars.Items.Remove(item);
                MessageBox.Show("Bilen med registeringsnummer" + item.Text + "är borttagen", "Borttag av bil");
            }
            else
            {
                MessageBox.Show("ingen bil var markerad att tas bort", "borttag av bil");
            }
            btnClear.Enabled = (lsvCars.SelectedItems.Count > 0);
        }
        private void lsvCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (lsvCars.SelectedItems.Count > 0);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lsvCars.Items.Clear();
            txtRegNr.Focus();
        }
        #endregion Events

        #region HELPFUNCTIONS
        private ListViewItem CreateListViewItem(string regNr, string make, bool forSale) 
        {
            ListViewItem item = new ListViewItem(regNr);
            item.SubItems.Add(make);
            item.SubItems.Add(forSale ? "yes" : "no");
            return item;
        }

        private void ClearTextboxes()
        {
            txtRegNr.Clear();
            txtMake.Clear();
            txtYear.Clear();
            cbxForSale.Checked = false;
            txtRegNr.Focus();
        }
        #endregion HELPFUNCTIONS
    }
}
