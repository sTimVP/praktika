using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shop
{
    public partial class Mainform : Form
    {
        public Mainform()
        {
            InitializeComponent();
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbItems_Click(object sender, EventArgs e)
        {
            Hide();
            Form Products = new Products();
            Products.Show();
        }

        private void lbLogout_Click(object sender, EventArgs e)
        {
            Hide();
            Form Log_in = new Log_in();
            Log_in.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Hide();
            Form Categories = new Categories();
            Categories.Show();
        }

        private void lbKlienti_Click(object sender, EventArgs e)
        {
            Hide();
            Form Costumer = new Costumer();
            Costumer.Show();
        }

        private void lbCustomer_Click(object sender, EventArgs e)
        {
            Hide();
            Form Register = new Register();
            Register.Show();
        }

        private void lbBilling_Click(object sender, EventArgs e)
        {
            Hide();
            Form zakazi = new zakazi();
            zakazi.Show();
        }
    }
}
