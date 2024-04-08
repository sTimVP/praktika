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

namespace shop
{
    public partial class Categories : Form
    {
        public Categories()
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

        private void lbItems_Click(object sender, EventArgs e)
        {
            Hide();
            Form Products = new Products();
            Products.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (categoryid_tb.Text == "" || namecat_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else {
                    Con.Open();

                    // Проверка, существует ли уже такая категория
                    string checkQuery = "SELECT COUNT(*) FROM Category WHERE category_id=" + categoryid_tb.Text + ";";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("category_id", categoryid_tb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Такая категория уже существует!");
                            Con.Close();
                        }

                else
                        {
                            Con.Close();
                            Con.Open();
                            string query = "insert into Category values(" + categoryid_tb.Text + ", '" + namecat_tb.Text + "')";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Категория добавлена успешно!");
                            Con.Close();
                            populate();
                        }
                    }
                } }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void populate()
        {
            Con.Open();
            string query = "select * from Category";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CatDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
       

        private void Categories_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (categoryid_tb.Text == "" || namecat_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();
                    string query = "update Category set category_name='" + namecat_tb.Text + "' WHERE category_id=" + categoryid_tb.Text + ";";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Категория Успешно обновлена");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (categoryid_tb.Text == "")
                {
                    MessageBox.Show("Выберите категорию для удаления");
                }
                else
                {
                    Con.Open();
                    string query = "delete from Category where category_id=" + categoryid_tb.Text + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Категория удалена успешно!");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CatDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            categoryid_tb.Text = CatDGV.SelectedRows[0].Cells[0].Value.ToString();
            namecat_tb.Text = CatDGV.SelectedRows[0].Cells[1].Value.ToString();
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

        private void lbBilling_Click(object sender, EventArgs e)
        {
            Hide();
            Form zakazi = new zakazi();
            zakazi.Show();
        }
    }
    
}
