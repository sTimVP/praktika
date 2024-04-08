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
    public partial class Costumer : Form
    {
        public Costumer()
        {
            InitializeComponent();

        }

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

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Hide();
            Form Mainform = new Mainform();
            Mainform.Show();
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

        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-96VRCH3;Initial Catalog=product_shop;Integrated Security=True");

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (costid_tb.Text == "" || cost_name_tb.Text == "" || cost_lastname_tb.Text == "" || costmail_tb.Text == "" || cost_phone_tb.Text == "" || cost_addres_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();

                    // Проверка, существует ли уже такой продукт
                    string checkQuery = "SELECT COUNT(*) FROM Costumer where costumer_id=" + costid_tb.Text + ";";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("costumer_id", costid_tb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Клиент с таким ID уже существует!");
                            Con.Close();
                        }

                        else
                        {
                            Con.Close();
                            Con.Open();
                            string query = "insert into Costumer values(" + costid_tb.Text + ", '" + cost_name_tb.Text + "', '" + cost_lastname_tb.Text + "','" + costmail_tb.Text + "','" + cost_phone_tb.Text + "','" + cost_addres_tb.Text + "')";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Покупатель добавлен успешно!");
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
            string query = "select * from Costumer";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            CostDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (costid_tb.Text == "" || cost_name_tb.Text == "" || cost_lastname_tb.Text == "" || costmail_tb.Text == "" || cost_phone_tb.Text == "" || cost_addres_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();
                    string query = "update Costumer set firs_name='" + cost_name_tb.Text + "',last_name='" + cost_lastname_tb.Text + "',email='" + costmail_tb.Text + "',phone='" + cost_phone_tb.Text + "', address='" + cost_addres_tb.Text + "' WHERE costumer_id=" + costid_tb.Text+";";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Покупатель Успешно обновлен");
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
                if (costid_tb.Text == "")
                {
                    MessageBox.Show("Выберите Покупателя для удаления");
                }
                else
                {
                    Con.Open();
                    string query = "delete from Costumer where costumer_id=" + costid_tb.Text + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Покупатель удален Успешно!");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CostDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            costid_tb.Text = CostDGV.SelectedRows[0].Cells[0].Value.ToString();
            cost_name_tb.Text = CostDGV.SelectedRows[0].Cells[1].Value.ToString();
            cost_lastname_tb.Text = CostDGV.SelectedRows[0].Cells[2].Value.ToString();
            costmail_tb.Text = CostDGV.SelectedRows[0].Cells[3].Value.ToString();
            cost_phone_tb.Text = CostDGV.SelectedRows[0].Cells[4].Value.ToString();
            cost_addres_tb.Text = CostDGV.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void Costumer_Load(object sender, EventArgs e)
        {
            populate();
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
