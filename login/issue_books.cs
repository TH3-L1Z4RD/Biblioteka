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

namespace login
{
    public partial class issue_books : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=biblioteka;Integrated Security=True;Pooling=False;Encrypt=False");
        public issue_books()
        {
            InitializeComponent();
        }

        private void issue_books_Load(object sender, EventArgs e)
        {                            
            if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            con.Open();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM student_info WHERE student_number='" + textBox1.Text + "'";
                var z = cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                i = Convert.ToInt32(dt.Rows.Count.ToString());

                if (i == 0)
                {
                    MessageBox.Show("This number was not found");
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        textBox2.Text = dr["student_name"].ToString();
                        textBox3.Text = dr["student_department"].ToString();
                        textBox4.Text = dr["student_contact"].ToString();
                        textBox5.Text = dr["student_email"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            int count = 0;

            if (e.KeyCode != Keys.Enter)
            {
                listBox1.Items.Clear();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM books_info WHERE books_name LIKE('%" + textBox6.Text + "%')";
                var h = cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                count = Convert.ToInt32(dt.Rows.Count.ToString());

                if (count > 0)
                {
                    listBox1.Visible = true;
                    foreach (DataRow dr in dt.Rows)
                    {
                        int g = listBox1.Items.Add(dr["books_name"].ToString());
                    }
                }
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listBox1.Focus();
                listBox1.SelectedIndex = 0;
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                textBox6.Text = listBox1.SelectedItem.ToString();
                listBox1.Visible = false;
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox6.Text = listBox1.SelectedItem.ToString();
            listBox1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int books_qty = 0;
            SqlCommand cmd2 = con.CreateCommand();
            cmd2.CommandType = CommandType.Text;
            cmd2.CommandText = "SELECT * FROM books_info WHERE books_name ='"+ textBox6.Text +"'";
            var o = cmd2.ExecuteNonQuery();
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            da2.Fill(dt2);

            foreach (DataRow dr2 in dt2.Rows)
            {
                books_qty = Convert.ToInt32(dr2["books_availability"].ToString());
            }

            if( books_qty > 0 )
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO issue_books VALUES ('" + textBox2.Text + "', '" + textBox1.Text + "', '" + textBox3.Text + "', '" + textBox4.Text + "', '" + textBox5.Text + "', '" + textBox6.Text + "', '" + dateTimePicker1.Value.ToShortDateString() + "', '')";
                var h = cmd.ExecuteNonQuery();

                SqlCommand cmd1 = con.CreateCommand();
                cmd1.CommandType = CommandType.Text;
                cmd1.CommandText = "UPDATE books_info SET books_availability = books_availability - 1 WHERE books_name = '" + textBox6.Text + "'";
                var t = cmd1.ExecuteNonQuery();

                MessageBox.Show("Book issued successfully");
            }     
            else
            {
                MessageBox.Show("Book(s) not available");
            }
        }
    }
}