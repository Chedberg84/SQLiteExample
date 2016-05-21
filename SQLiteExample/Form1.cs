using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace SQLiteExample
{
    public partial class Form1 : Form
    {
        SQLiteConnection sqLiteConnection2 = new SQLiteConnection(@"data source=\\mgfs01\home$\CHedberg\My Documents\Visual Studio 2010\Projects\SQLiteExample\TEST.db3");
            
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Insert_ADONET();
            //Insert_LINQ();
        }

        private void Insert_ADONET()
        {
            SQLiteTransaction trans;
            string SQL = "INSERT INTO TestTable (ID, NAME, DESCRIPTION) VALUES";
            SQL += "(@ID, @Name, @Description)";

            SQLiteCommand cmd = new SQLiteCommand(SQL);
            cmd.Parameters.AddWithValue("@ID", 0);
            cmd.Parameters.AddWithValue("@Name", this.textBox1.Text);
            cmd.Parameters.AddWithValue("@Description", this.textBox2.Text);

            cmd.Connection = sqLiteConnection2;
            sqLiteConnection2.Open();
            trans = sqLiteConnection2.BeginTransaction();
            int retval = 0;
            try
            {
                retval = cmd.ExecuteNonQuery();
                if (retval == 1)
                    MessageBox.Show("Row inserted!");
                else
                    MessageBox.Show("Row NOT inserted.");
            }
            catch (Exception ex)
            {
                trans.Rollback();
                Console.WriteLine(ex.Message);
            }
            finally
            {
                trans.Commit();
                cmd.Dispose();
                sqLiteConnection2.Close();
            }
        }

        private void Get_ADONET()
        {
            string SQL = "SELECT * FROM TestTable";
            SQLiteCommand cmd = new SQLiteCommand(SQL);
            cmd.Connection = sqLiteConnection2;
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
                DataTable dt = ds.Tables[0];
                //this.dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Dispose();
                sqLiteConnection2.Close();
            }
        }
    }
}
