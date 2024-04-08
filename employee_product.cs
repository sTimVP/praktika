using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shop
{
    public partial class employee_product : Form
    {
        public employee_product()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-96VRCH3;Initial Catalog=product_shop;Integrated Security=True");

        private void employee_product_Load(object sender, EventArgs e)
        {
            populate();
            fillcombo();
        }
        private void fillcombo()
        {
            //This Method will bind the Combobox with the Database
            Con.Open();
            SqlCommand cmd = new SqlCommand("select category_name from Category", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("category_name", typeof(string));
            dt.Load(rdr);
            prodCB.ValueMember = "category_name";
            prodCB.DataSource = dt;
            Con.Close();
        }
        private void populate()
        {
            Con.Open();
            string query = "select * from Product";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            eProdDGV.DataSource = ds.Tables[0];
            Con.Close();
            Con.Open();
            query = "select * from Category";
            sda = new SqlDataAdapter(query, Con);
            builder = new SqlCommandBuilder(sda);
            ds = new DataSet();
            sda.Fill(ds);
            eCatDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void lbLogout_Click(object sender, EventArgs e)
        {
            Hide();
            Form Log_in = new Log_in();
            Log_in.Show(); 
        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbKlienti_Click(object sender, EventArgs e)
        {
            Hide();
            Form emp_clients = new emp_clients();
            emp_clients.Show();
        }

        private void lbBilling_Click(object sender, EventArgs e)
        {
            Hide();
            Form emp_zakazi = new emp_zakazi();
            emp_zakazi.Show();
        }

        private void prodCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = prodCB.SelectedItem.ToString();
            Con.Open();

            string query = $"SELECT * FROM Category WHERE category_name = '{prodCB.Text}'";

            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {

                catid.Text = reader["category_id"].ToString();





            }
            Con.Close();

            Con.Open();
            // MessageBox.Show(catid.Text);
            int iddd = int.Parse(catid.Text);
            query = $"SELECT * FROM Product WHERE category_id = '{iddd}'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            eProdDGV.DataSource = ds.Tables[0];

            Con.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            populate();
        }
    }
}
