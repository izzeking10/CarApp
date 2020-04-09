using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

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

                ListViewItem item = CreateListViewItem(txtRegNr.Text, txtMake.Text, txtModel.Text, txtYear.Text, cbxForSale.Checked);
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
        /// <summary>
        /// Säkerställer att knappen "ta bort markerad bil" bara är tillgänglig när det finns en bil markerad
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsvCars_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = (lsvCars.SelectedItems.Count > 0);
        }
        /// <summary>
        /// Rensar bort alla bilar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            lsvCars.Items.Clear();
            txtRegNr.Focus();
        }
        #endregion Events

        #region HELPFUNCTIONS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="regNr"></param>
        private void PrintData(string regNr)
        {
            string token = "DtIAxcVeOZhJzLnC6LYN3BjwasJw2FIA5hdvgP00lNKw1cM53ddy1iWpll54";
            string call = String.Format($"https://api.biluppgifter.se/api/v1/vehicle/regno/XNF905?api_token=DtIAxcVeOZhJzLnC6LYN3BjwasJw2FIA5hdvgP00lNKw1cM53ddy1iWpll54");
            try
            {
                WebRequest request = HttpWebRequest.Create(call);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string carJson = reader.ReadToEnd();
                JObject jsonCar = JObject.Parse(carJson);
                txtMake.Text = jsonCar["data"]["basic"]["data"]["make"].ToString();
                txtModel.Text = jsonCar["data"]["basic"]["data"]["model"].ToString();
                txtYear.Text = jsonCar["data"]["basic"]["data"]["model_year"].ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show($"Bil med registreringsnummer {regNr} kunde inte hittas\n\nMeddelande: {e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtRegNr.Text))
            {
                string regNr = txtRegNr.Text.ToUpper();
                PrintData(regNr);
            }
            else
            {
                MessageBox.Show("Du måste ange ett registreringsnummer", "Inmatning Saknas", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        ///  Hjälpfunktion till btnAdd_Click
        /// </summary>
        /// <param name="regNr"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="year"></param>
        /// <param name="forSale"></param>
        /// <returns></returns>
        private ListViewItem CreateListViewItem(string regNr, string make, string model, string year , bool forSale) 
        {
            ListViewItem item = new ListViewItem(regNr);
            item.SubItems.Add(make);
            item.SubItems.Add(model);
            item.SubItems.Add(year);
            item.SubItems.Add(forSale ? "yes" : "no");
            return item;
        }
        /// <summary>
        /// Rensar alla textfält och sätter regnummer till att vara i fokus
        /// </summary>
        private void ClearTextboxes()
        {
            txtRegNr.Clear();
            txtMake.Clear();
            txtModel.Clear();
            txtYear.Clear();
            cbxForSale.Checked = false;
            txtRegNr.Focus();
        }

        #endregion HELPFUNCTIONS
    }
}
