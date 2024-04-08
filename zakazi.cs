using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace shop
{
    public partial class zakazi : Form
    {
        public zakazi()
        {
            InitializeComponent();
            data_tp.Format = DateTimePickerFormat.Custom;
            data_tp.CustomFormat = "MM/dd/yyyy hh:mm:ss";
            data_tp.ShowUpDown = true;
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-96VRCH3;Initial Catalog=product_shop;Integrated Security=True");

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (zakazid_tb.Text == "" || cost_tb.Text == "" || prod_id_tb.Text == "" || data_tp.Text == "" || quantiti_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();

                    // Проверка, существует ли уже такой продукт
                    string checkQuery = "SELECT COUNT(*) FROM [Order] where order_id=" + zakazid_tb.Text + ";";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, Con))
                    {
                        checkCmd.Parameters.AddWithValue("order_id", zakazid_tb.Text);

                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Заказ с таким ID уже существует!");
                            Con.Close();
                        }

                        else
                        {
                            Con.Close();
                            Con.Open();
                            string formattedDate = data_tp.Value.ToString("yyyy-MM-ddTHH:mm:ss");


                            string query = "INSERT INTO [Order] VALUES(" + zakazid_tb.Text + ", '" + cost_tb.Text + "', '" + prod_id_tb.Text + "', '" + formattedDate + "','" + quantiti_tb.Text + "')";
                            SqlCommand cmd = new SqlCommand(query, Con);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Заказ добавлен успешно!");
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
            string query = "select * from [Order]";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            zakaziDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (zakazid_tb.Text == "" || cost_tb.Text == "" || prod_id_tb.Text == "" || data_tp.Text == "" || quantiti_tb.Text == "")
                    MessageBox.Show("Не во всех полях есть данные");
                else
                {
                    Con.Open();
                    DateTime orderdate;
                    

                    if (!DateTime.TryParseExact(data_tp.Text, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out orderdate))
                    {
                        MessageBox.Show("Недопустимые значения даты и времени.");
                        Con.Close();
                        return;
                    }

                    if (orderdate < SqlDateTime.MinValue.Value || orderdate > SqlDateTime.MaxValue.Value )
                    {
                        MessageBox.Show("Значения даты и времени выходят за пределы допустимого диапазона.");
                        Con.Close();
                        return;
                    }
                    string formattedDate = data_tp.Value.ToString("yyyy-MM-ddTHH:mm:ss");

                    string query = "update [Order] set costumer_id=" + cost_tb.Text + ", product_id=" + prod_id_tb.Text + ", order_date='" + formattedDate + "', quantiti=" + quantiti_tb.Text + " WHERE order_id=" + zakazid_tb.Text + ";";

                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Заказ Успешно обновлен");
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
                if (zakazid_tb.Text == "")
                {
                    MessageBox.Show("Выберите Заказ для удаления");
                }
                else
                {
                    Con.Open();
                    string query = "delete from [Order] where order_id=" + zakazid_tb.Text + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Заказ удален Успешно!");
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
            zakazid_tb.Text = zakaziDGV.SelectedRows[0].Cells[0].Value.ToString();
            cost_tb.Text = zakaziDGV.SelectedRows[0].Cells[1].Value.ToString();
            prod_id_tb.Text = zakaziDGV.SelectedRows[0].Cells[2].Value.ToString();
            data_tp.Text = zakaziDGV.SelectedRows[0].Cells[3].Value.ToString();
            quantiti_tb.Text = zakaziDGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void lbItems_Click(object sender, EventArgs e)
        {
            Hide();
            Form Products = new Products();
            Products.Show();
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

        private void label5_Click(object sender, EventArgs e)
        {
            Hide();
            Form Categories = new Categories();
            Categories.Show();
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

        private void zakazi_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void zakaziDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (zakaziDGV.Columns[e.ColumnIndex].Name == "order_date" && e.Value != null)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("MM.dd.yyyy HH:mm:ss");
                e.FormattingApplied = true;
            }
        }
    }
}
