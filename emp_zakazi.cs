using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheArtOfDevHtmlRenderer.Adapters;

namespace shop
{
    public partial class emp_zakazi : Form
    {
        public emp_zakazi()
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
            ezakaziDGV.DataSource = ds.Tables[0];
            Con.Close();


            Con.Open();
            query = "select * from Product";
            sda = new SqlDataAdapter(query, Con);
            builder = new SqlCommandBuilder(sda);
            ds = new DataSet();
            sda.Fill(ds);
            Con.Close();

            DataView dv = new DataView(ds.Tables[0]);

            

            // Создаем новый DataTable, содержащий только выбранные столбцы
            DataTable filteredTable = dv.ToTable(false, "product_id", "category_id", "product_name", "price", "stock_quantity");

            // Назначаем новый DataTable источником данных для DataGridView
            ezProdDGV.DataSource = filteredTable;


            Con.Open();
            query = "select * from Costumer";
            sda = new SqlDataAdapter(query, Con);
            builder = new SqlCommandBuilder(sda);
            ds = new DataSet();
            sda.Fill(ds);
            ezCostDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void emp_zakazi_Load(object sender, EventArgs e)
        {
            populate();
            fillcombo();
        }

        private void zakaziDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            zakazid_tb.Text = ezakaziDGV.SelectedRows[0].Cells[0].Value.ToString();
            cost_tb.Text = ezakaziDGV.SelectedRows[0].Cells[1].Value.ToString();
            prod_id_tb.Text = ezakaziDGV.SelectedRows[0].Cells[2].Value.ToString();
            data_tp.Text = ezakaziDGV.SelectedRows[0].Cells[3].Value.ToString();
            quantiti_tb.Text = ezakaziDGV.SelectedRows[0].Cells[4].Value.ToString();
        }

        private void lbItems_Click(object sender, EventArgs e)
        {
            Hide();
            Form employee_product = new employee_product();
            employee_product.Show();
        }

        private void lbKlienti_Click(object sender, EventArgs e)
        {
            Hide();
            Form emp_clients = new emp_clients();
            emp_clients.Show();
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

        private void button2_Click(object sender, EventArgs e)
        {
            zakazid_tb.Text = "";
            cost_tb.Text = "";
            prod_id_tb.Text = "";
            quantiti_tb.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Con.Open();

         
            string customerId = ezakaziDGV.SelectedRows[0].Cells["costumer_id"].Value.ToString();
            string query = $"SELECT * FROM Costumer WHERE costumer_id = '{customerId}'";
            SqlCommand cmd = new SqlCommand(query, Con);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lbF.Text = reader["firs_name"].ToString();
                lbI.Text = reader["last_name"].ToString();            
            }
            Con.Close();
            Con.Open();
            string productId = ezakaziDGV.SelectedRows[0].Cells["product_id"].Value.ToString();
            query = $"SELECT * FROM Product WHERE product_id = '{productId}'";
            cmd = new SqlCommand(query, Con);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lbCost.Text = reader["price"].ToString();
                lbName.Text = reader["product_name"].ToString();
                byte[] imageData = (byte[])reader["image"];
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    imagepic.Image = Image.FromStream(ms);
                }
            }
            int quantiti = (int)ezakaziDGV.SelectedRows[0].Cells["quantiti"].Value;
            lbquantiti.Text = quantiti.ToString();
            if (double.TryParse(lbCost.Text, out double value1) && double.TryParse(lbquantiti.Text, out double value2))
            {
                double result = value1 * value2;
                lbResult.Text = result.ToString();
            }
            Con.Close();
            Con.Open();
            query = $"SELECT * FROM Costumer WHERE firs_name = '{lbF.Text}' and last_name = '{lbI.Text}'";
            cmd = new SqlCommand(query, Con);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lbF.Text = reader["firs_name"].ToString();
                lbI.Text = reader["last_name"].ToString();
                lbO.Text = reader["phone"].ToString();
            }
            Con.Close();
            string Фио = $"{lbF.Text} {lbI.Text}";
            string txtQrCode = "Продуктовый магазин 'Магнит'" + "\nЧек покупки товара" + $"\nПокупатель: {lbF.Text} {lbI.Text}" + "\nНомер заказа: " + ezakaziDGV.SelectedRows[0].Cells[0].Value.ToString() + "\nНазвание товара: " + lbName.Text +
    "\nКоличество: " + lbquantiti.Text + "\nДата: " + ezakaziDGV.SelectedRows[0].Cells[3].Value.ToString() + $"\nСтоимость: {lbResult.Text}";
            QRCodeGenerator qr = new QRCodeGenerator();
            QRCodeData data = qr.CreateQrCode(txtQrCode, QRCodeGenerator.ECCLevel.Q);
            QRCode code = new QRCode(data);
            picQR.Image = code.GetGraphic(5);
            if (zakazid_tb.Text == "" || cost_tb.Text == "")
            {
                MessageBox.Show("Не выбран заказ");
            }
            else
            {
                printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
                printDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 415, 500);
                printPreviewDialog1.Size = new Size(500, 600);
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string Фио = $"{lbF.Text.Trim()} {lbI.Text.Trim()}";
            e.Graphics.DrawString("Продуктовый магазин 'МАГНИТ'", new Font("Century Gothic", 12, FontStyle.Bold), Brushes.IndianRed, new Point(70));
            e.Graphics.DrawString("Чек покупки товара:", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 25));
            e.Graphics.DrawString("Покупатель: " + Фио, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 50));
            e.Graphics.DrawString("Номер заказа: " + ezakaziDGV.SelectedRows[0].Cells[0].Value.ToString(), new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 70));
            e.Graphics.DrawString("Название товара: " + lbName.Text, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 90));
            e.Graphics.DrawString("Фото: ", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(250, 50));
            Image image = imagepic.Image;
            Rectangle destinationRect = new Rectangle(250, 70, 120, 120);
            e.Graphics.DrawImage(image, destinationRect);



            e.Graphics.DrawString("Количество: " + lbquantiti.Text, new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 110));
            
            e.Graphics.DrawString("Дата: " + ezakaziDGV.SelectedRows[0].Cells[3].Value.ToString(), new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 150));
            e.Graphics.DrawString("Стоимость: " + lbResult.Text + " руб.", new Font("Century Gothic", 10, FontStyle.Bold), Brushes.Black, new Point(10, 170));
            int newWidth = picQR.Width / 2;
            int newHeight = picQR.Height / 2;
            e.Graphics.DrawImage(picQR.Image, new Rectangle(80, 207, 250, 250));

        }

        private void prodCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Con.Open();


            string query = $"SELECT * FROM [Order] WHERE order_date = '{prodCB.Text}'";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ezakaziDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void fillcombo()
        {

            Con.Open();
            SqlCommand cmd = new SqlCommand("select order_date from [Order]", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("order_date", typeof(string));
            dt.Load(rdr);
            prodCB.ValueMember = "order_date";
            prodCB.DataSource = dt;
            Con.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            populate();
        }

        private void ezakaziDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (ezakaziDGV.Columns[e.ColumnIndex].Name == "order_date" && e.Value != null)
            {
                e.Value = DateTime.Parse(e.Value.ToString()).ToString("MM.dd.yyyy HH:mm:ss");
                e.FormattingApplied = true;
            }
        }
    }
}
