using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shop
{
    public partial class gost : Form
    {
        public gost()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-96VRCH3;Initial Catalog=product_shop;Integrated Security=True");

        private void gost_Load(object sender, EventArgs e)
        {
            fillcombo();
            populate();



        }
        private void populate() {
            Con.Open();
            string query = "select * from Product";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            Con.Close();

            DataView dv = new DataView(ds.Tables[0]);
            DataTable filteredTable = dv.ToTable(false, "product_name", "description", "price", "image");

            goProdDGV.DataSource = filteredTable;
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

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void lbLogout_Click(object sender, EventArgs e)
        {
            Hide();
            Form Log_in = new Log_in();
            Log_in.Show();
        }

        

        private void prodCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            populate();
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
           
            int iddd = int.Parse(catid.Text);
            query = $"SELECT * FROM Product WHERE category_id = '{iddd}'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);

            DataView dv = new DataView(ds.Tables[0]);
            DataTable filteredTable = dv.ToTable(false, "product_name", "description", "price", "image");

            goProdDGV.DataSource = filteredTable;
            

            Con.Close();
        }

        private void SearchBox2_TextChanged(object sender, EventArgs e)
        {
            Search2(goProdDGV);

        }
        private void Search2(DataGridView dgw2)
        {
            Con.Open();
            string searchString = $"select * from Product where concat(product_id, category_id, product_name, description, price, stock_quantity, image) like '%" + SearchBox2.Text + "%' ";
            SqlCommand com = new SqlCommand(searchString, Con);

            SqlDataReader read = com.ExecuteReader();  
            dgw2.DataSource = null;
            dgw2.Rows.Clear();

            while (read.Read())
            {
                ReadSingleRow2(dgw2, read);
            }

            Con.Close();
        }

        private void ReadSingleRow2(DataGridView dgw2, SqlDataReader reader)
        {
            if (dgw2.Columns.Count == 0)
            {
                // Если столбцы не определены, добавьте их
                dgw2.Columns.Add("product_id", "Product ID");
                dgw2.Columns.Add("category_id", "Category ID");
                dgw2.Columns.Add("product_name", "Product Name");
                dgw2.Columns.Add("description", "Description");
                dgw2.Columns.Add("price", "Price");
                dgw2.Columns.Add("stock_quantity", "Stock Quantity");

                // Добавим столбец для изображений
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                imageColumn.HeaderText = "Image";
                imageColumn.Name = "image";
                dgw2.Columns.Add(imageColumn);
            }

            object[] values = new object[reader.FieldCount];
            reader.GetValues(values);

            // Конвертируем байты в изображение
            byte[] imageBytes = values[6] as byte[];
            Image image = ByteArrayToImage(imageBytes);
            values[6] = image;

            dgw2.Rows.Add(values);
        }

        // Метод для конвертации массива байт в изображение
        private Image ByteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null) return null;
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            populate();
        }
    }
}
