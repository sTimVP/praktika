using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.IO;

namespace shop
{
    

    public partial class Products : Form
    {
       
        public Products()
        {
            InitializeComponent();
            
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-96VRCH3;Initial Catalog=product_shop;Integrated Security=True");

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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (prodid_tb.Text == "" || categoryid_tb.Text == "" || nameprod_tb.Text == "" || descprod_tb.Text == "" || priceprod_tb.Text == "" || quantityprod_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();

                    // Проверка, существует ли уже такой продукт
                    string checkQuery = "SELECT COUNT(*) FROM Product where product_id=" + prodid_tb.Text + ";";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("product_id", prodid_tb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Продукт с таким ID уже существует!");
                            Con.Close();
                        }
                        else
                        {
                            Con.Close();
                            Con.Open();

                            // Выбор изображения с компьютера
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            if (openFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                // Получение пути к выбранному файлу
                                string imagePath = openFileDialog.FileName;

                                // Преобразование изображения в массив байтов
                                byte[] imageBytes = File.ReadAllBytes(imagePath);

                                string query = "INSERT INTO Product VALUES(@product_id, @category_id, @name, @description, @price, @quantity, @image)";
                                SqlCommand cmd = new SqlCommand(query, Con);

                                // Добавление параметров
                                cmd.Parameters.AddWithValue("@product_id", prodid_tb.Text);
                                cmd.Parameters.AddWithValue("@category_id", categoryid_tb.Text);
                                cmd.Parameters.AddWithValue("@name", nameprod_tb.Text);
                                cmd.Parameters.AddWithValue("@description", descprod_tb.Text);
                                cmd.Parameters.AddWithValue("@price", priceprod_tb.Text);
                                cmd.Parameters.AddWithValue("@quantity", quantityprod_tb.Text);
                                cmd.Parameters.AddWithValue("@image", imageBytes);

                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Продукт добавлен успешно!");
                                Con.Close();
                                populate();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void populate()
        {
            Con.Open();
            string query = "select * from Product";
            SqlDataAdapter sda = new SqlDataAdapter(query,Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProdDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void Products_Load(object sender, EventArgs e)
        {
            populate();
            fillcombo();
        }

        private void ProdDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            prodid_tb.Text = ProdDGV.SelectedRows[0].Cells[0].Value.ToString();
            categoryid_tb.Text= ProdDGV.SelectedRows[0].Cells[1].Value.ToString();
            nameprod_tb.Text= ProdDGV.SelectedRows[0].Cells[2].Value.ToString();
            descprod_tb.Text= ProdDGV.SelectedRows[0].Cells[3].Value.ToString();
            priceprod_tb.Text= ProdDGV.SelectedRows[0].Cells[4].Value.ToString();
            quantityprod_tb.Text= ProdDGV.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (prodid_tb.Text == "")
                {
                    MessageBox.Show("Выберите продукт для удаления");
                }
                else
                {
                    Con.Open();
                    string query = "delete from Product where product_id=" + prodid_tb.Text + "";
                    SqlCommand cmd = new SqlCommand(query,Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Продукт удален Успешно!");
                    Con.Close();
                    populate();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (prodid_tb.Text == "" || categoryid_tb.Text == "" || nameprod_tb.Text == "" || descprod_tb.Text == "" || priceprod_tb.Text == "" || quantityprod_tb.Text == "")
                {
                    MessageBox.Show("Не во всех полях есть данные");
                }
                else
                {
                    Con.Open();

                    // Добавляем выбор изображения
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Изображения (*.jpg; *.jpeg; *.png; *.gif; *.jfif)|*.jpg;*.jpeg;*.png;*.gif;*.jfif";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        byte[] imageData = ConvertImageToByteArray(openFileDialog.FileName);

                        // Обновляем продукт в базе данных с учетом изображения
                        string query = "UPDATE Product SET category_id=@category_id, product_name=@product_name, description=@description, price=@price, stock_quantity=@stock_quantity, image=@image WHERE product_id=@product_id;";

                        SqlCommand cmd = new SqlCommand(query, Con);
                        cmd.Parameters.AddWithValue("@category_id", categoryid_tb.Text);
                        cmd.Parameters.AddWithValue("@product_name", nameprod_tb.Text);
                        cmd.Parameters.AddWithValue("@description", descprod_tb.Text);
                        cmd.Parameters.AddWithValue("@price", priceprod_tb.Text);
                        cmd.Parameters.AddWithValue("@stock_quantity", quantityprod_tb.Text);
                        cmd.Parameters.AddWithValue("@image", imageData);
                        cmd.Parameters.AddWithValue("@product_id", prodid_tb.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Продукт успешно обновлен");
                    }

                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private byte[] ConvertImageToByteArray(string imagePath)
        {
            // Чтение изображения в виде массива байтов
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                return br.ReadBytes((int)fs.Length);
            }
        }

        private void lbLogout_Click_1(object sender, EventArgs e)
        {
            Hide();
            Form Log_in = new Log_in();
            Log_in.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Hide();
            Form Categories = new Categories();
            Categories.Show();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
            Form Mainform = new Mainform();
            Mainform.Show();
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

        private void lbItems_Click(object sender, EventArgs e)
        {
            Hide();
            Form zakazi = new zakazi();
            zakazi.Show();
        }

        private void lbBilling_Click(object sender, EventArgs e)
        {
            Hide();
            Form zakazi = new zakazi();
            zakazi.Show();
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
            ProdDGV.DataSource = ds.Tables[0];

            Con.Close();
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            populate();
        }
    }
}
