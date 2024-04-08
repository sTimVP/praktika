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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-96VRCH3;Initial Catalog=product_shop;Integrated Security=True");

        

        private void lbItems_Click(object sender, EventArgs e)
        {
            Hide();
            Form Products = new Products();
            Products.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {

            Hide();
            Form Categories = new Categories();
            Categories.Show();
        }

        private void lbBilling_Click(object sender, EventArgs e)
        {
            Hide();
            Form zakazi = new zakazi();
            zakazi.Show();
        }

        private void lbKlienti_Click(object sender, EventArgs e)
        {
            Hide();
            Form Costumer = new Costumer();
            Costumer.Show();
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
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (login_tb.Text == "" || pass_tb.Text == "" || rights_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();

                    // Проверка, существует ли уже такой продукт
                    string checkQuery = "SELECT COUNT(*) FROM Register where login='" + login_tb.Text + "';";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("login", login_tb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Сотрудник с таким ID уже существует!");
                            Con.Close();
                        }

                        else
                        {
                            Con.Close();
                            Con.Open();
                            string query = "insert into Register values('" + login_tb.Text + "', '" + pass_tb.Text + "', '" + rights_tb.Text + "')";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Сотрудник добавлен успешно!");
                            Con.Close();
                            populate();
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
            string query = "select * from Register";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            RegDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (login_tb.Text == "" || pass_tb.Text == "" || rights_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();
                    string query = "update Register set login='" + login_tb.Text + "', password='" + pass_tb.Text + "', admin_or_employee='" + rights_tb.Text + "' WHERE reg_id='" + reg_id.Text + "';";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Сотрудник Успешно обновлен");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RegDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            reg_id.Text = RegDGV.SelectedRows[0].Cells[0].Value.ToString();

            login_tb.Text = RegDGV.SelectedRows[0].Cells[1].Value.ToString();
            pass_tb.Text = RegDGV.SelectedRows[0].Cells[2].Value.ToString();
            rights_tb.Text = RegDGV.SelectedRows[0].Cells[3].Value.ToString();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (login_tb.Text == "")
                {
                    MessageBox.Show("Выберите сотрудника для удаления");
                }
                else
                {
                    Con.Open();
                    string query = "delete from Register where login='" + login_tb.Text + "'";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Сотрудник удален успешно!");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
