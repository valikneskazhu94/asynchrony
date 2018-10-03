using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace asynchronyHW
{
    public partial class Form1 : Form
    {
        SqlCommand comand1 = null;
        static SqlConnection sql_connection = null;
        static SqlConnection sql_connection2 = null;
        static SqlDataReader data_reader = null;
        static SqlDataReader data_reader2 = null;
        static string connectionString;
        public Form1()
        {
            InitializeComponent();

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //Первый поток должен вычислить суммарное для всех строк количество символов в поле title в таблице Books. 
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(Method);
            Thread thread1 = new Thread(threadStart);
            thread1.Start();
            //Второй поток должен вычислить суммарное количество символов в полях FirsName и LastName в таблице Authors
            ParameterizedThreadStart threadStart2 = new ParameterizedThreadStart(Method2);
            Thread thread2 = new Thread(threadStart2);
            thread2.Start();
        }

        private void Method2(object obj)
        {
            sql_connection2 = new SqlConnection(connectionString);
            string query2 = "select * from Authors;";
            int quantity_Authors_FirstName = 0;
            int quantity_Authors_LastName = 0;
            try
            {
                sql_connection2.Open();
                SqlCommand command4 = new SqlCommand(query2, sql_connection2);
                data_reader2 = command4.ExecuteReader();
                while (data_reader2.Read())
                {
                    quantity_Authors_FirstName += data_reader2["FirstName"].ToString().Count();
                    quantity_Authors_LastName += data_reader2["LastName"].ToString().Count();
                    textBox2.Invoke(new Action(() =>
                    {
                        textBox2.Text = $"{(quantity_Authors_FirstName+quantity_Authors_LastName).ToString()} символов!akq";
                    }));
                    Thread.Sleep(200);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Method(object obj)
        {
            sql_connection = new SqlConnection(connectionString);
            string query1 = "select * from Books;";
            int quantity=0;
            try
            {
                sql_connection.Open();
                SqlCommand command3 = new SqlCommand(query1, sql_connection);
                data_reader = command3.ExecuteReader();
                while(data_reader.Read())
                {
                    quantity += data_reader["NameBook"].ToString().Count();
                    textBox1.Invoke(new Action(() =>
                    {
                        textBox1.Text = $"{quantity} символов!";
                    }));
                    Thread.Sleep(200);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connectionString = @"Data Source=COMP409\SQLEXPRESS;Initial Catalog=bookshops;Integrated Security=True;";
           
        }
    }
}
