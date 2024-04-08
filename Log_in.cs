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
using System.Web.UI.WebControls;

namespace shop
{
    public partial class Log_in : Form
    {
        DataBase database = new DataBase();
        public Log_in()
        {
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            var loginuser = textBox1_login.Text;
            var passuser = textBox2_password.Text;
            
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string qwerysting = $"select login password from Register where login = '{loginuser}' and password = '{passuser}'";
            SqlCommand command= new SqlCommand(qwerysting, database.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);


            if (table.Rows.Count == 1)
            {

                SqlDataAdapter adapter1 = new SqlDataAdapter();
                DataTable table1 = new DataTable();

                string querystring1 = $"SELECT login, password, admin_or_employee FROM Register WHERE login = '{loginuser}' AND password = '{passuser}' AND admin_or_employee = 'admin'";

                var Login = loginuser;
                var password = passuser;
                SqlCommand command1 = new SqlCommand(querystring1, database.getConnection());

                adapter1.SelectCommand = command1;
                adapter1.Fill(table1);

                if (table1.Rows.Count == 1)
                {
                    MessageBox.Show("Вы успешно вошли как админ!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();
                    Form MainForm = new Mainform();
                    MainForm.Show();
                    textBox1_login.Text = "";
                    textBox2_password.Text = "";
                }
                else
                {
                    MessageBox.Show("Вы успешно вошли как сотрудник!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();
                    Form employee_product = new employee_product();
                    employee_product.Show();
                    textBox1_login.Text = "";
                    textBox2_password.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void lbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Log_in_Load(object sender, EventArgs e)
        {
            textBox2_password.PasswordChar = '•';

        }

        private void offpb_Click(object sender, EventArgs e)
        {
            textBox2_password.PasswordChar = '\0';
            offpb.Visible = false;
            onpb.Visible = true;
        }

    

        private void onpb_Click_1(object sender, EventArgs e)
        {
 textBox2_password.PasswordChar = '•';
            offpb.Visible = true;
            onpb.Visible = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Hide();
            Form gost = new gost();
            gost.Show();
        }
    }
}
