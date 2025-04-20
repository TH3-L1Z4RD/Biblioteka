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

namespace login
{
    public partial class view_borrowed_books : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=localhost;Initial Catalog=biblioteka;Integrated Security=True;Pooling=False;Encrypt=False");

        public view_borrowed_books()
        {
            InitializeComponent();
        }

        private void view_borrowed_books_Load(object sender, EventArgs e)
        {
            display_borrowed_books();
        }

        public void display_borrowed_books()
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
                cmd.CommandText = "SELECT * FROM issue_books";
                var n = cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM issue_books WHERE student_name LIKE ('%" + textBox1.Text + "%')";   //wybranie z tabeli konkretnego studenta
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM issue_books WHERE books_name LIKE ('%" + textBox2.Text + "%')";   //wybranie z tabeli konkretnej książki
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

            try
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT * FROM issue_books WHERE id = "+ i +"";
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows) //wrzucenie wartości z tabeli do poszczególnych komórek w formularzu
                {
                    textBox3.Text = dr["student_name"].ToString();
                    textBox4.Text = dr["student_number"].ToString();
                    textBox5.Text = dr["student_department"].ToString();
                    textBox6.Text = dr["student_contact"].ToString();
                    textBox7.Text = dr["student_email"].ToString();
                    textBox8.Text = dr["books_name"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(dr["books_issue_date"].ToString());
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int i;
            i = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());

            try
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "UPDATE issue_books SET student_name='" + textBox3.Text + "', student_number='" + textBox4.Text + "', student_department='" + textBox5.Text + "', student_contact='" + textBox6 + "', student_email='" + textBox7.Text + "', books_name='" + textBox8.Text + "', books_issue_date='"+ dateTimePicker1.Value +"' WHERE id=" + i + "";  //zmiana wartości pól w bazie
                cmd.ExecuteNonQuery();
                con.Close();
                display_borrowed_books();      //wyświetla już ze zmianami po update
                MessageBox.Show("Updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                listBox1.Focus();
                listBox1.SelectedIndex = 0;
            }
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textBox8.Text = listBox1.SelectedItem.ToString();
                listBox1.Visible = false;
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox8.Text = listBox1.SelectedItem.ToString();
            listBox1.Visible = false;
        }

        

        private void textBox8_KeyUp(object sender, KeyEventArgs e)
        {
            int count = 0;

            if (e.KeyCode != Keys.Enter)
            {
                listBox1.Items.Clear();

                try
                {
                    con.Open();

                    SqlCommand cmd = con.CreateCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM books_info WHERE books_name LIKE('%" + textBox8.Text + "%')";
                    int h = cmd.ExecuteNonQuery();
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

                    con.Close();
                }
                catch(Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }
    }
}
