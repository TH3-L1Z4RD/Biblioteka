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

namespace login
{
    public partial class add_books : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=biblioteka;Integrated Security=True;Pooling=False;Encrypt=False"); //połączenie z bazą
        public add_books()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO books_info VALUES('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + dateTimePicker1.Value + "'," + textBox5.Text + "," + textBox6.Text + "," + textBox6.Text + ")"; //wrzucanie danych do bazy danych
                var v = cmd.ExecuteNonQuery();
                con.Close();

                textBox1.Text = "";   //zmiana, żeby pola po dodaniu były od nowa puste
                textBox2.Text = "";
                textBox3.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";

                MessageBox.Show("Book(s) added successfully"); //komunikat po dodadniu książki
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);  //jeśli w kodzie w try jest błąd, to wyświetli co jest nie tak
            }
            finally
            {
                con.Close();
            }
        }
    }
}
