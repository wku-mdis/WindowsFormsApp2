using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        int selectedID = 0;
        int selectedRow;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            displayForm();
        }

      

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] queries = { 
                "SELECT * FROM soft562.facebook_users",
                "SELECT * FROM soft562.customers",
                "SELECT * FROM soft562.facebook_users where gender = 'male'",
            };

            string query = queries[comboBox1.SelectedIndex];
            //MessageBox.Show(query);

            performSQLCommand(query);

        }
        
        private void displayForm()
        {
            performSQLCommand("SELECT * FROM soft562.facebook_users");
            buttonAdd.Enabled = false;
            buttonEdit.Enabled = false;
            buttonDelete.Enabled = false;
            dataGridView1.Columns[0].ReadOnly = true;
        }

        private void performSQLCommand(string query)
        {
            /////////////////////////////////////////////////////////////////
            //
            // The basic format of a connection string consists of a series
            // of keyword/value pairs separated by semicolons. The equal
            // sign (=) connects each keyword and its value. Specifically,
            // our connection string MUST refer to xServe as the server, and
            // to our username and password to access xServe.
            //
            /////////////////////////////////////////////////////////////////
            string connectionString = "SERVER=" + DBConnect.SERVER + ";" +
                "DATABASE=" + DBConnect.DATABASE_NAME + ";" + "UID=" +
                DBConnect.USER_NAME + ";" + "PASSWORD=" +
                DBConnect.PASSWORD + ";" + "SslMode=" +
                DBConnect.SslMode + ";";
            ///////////////////////////////////////////////////////////////
            //
            // Create an open connection to a MySQL Server database. In our
            // case, the connection allows us to interact with xServe.
            //
            ///////////////////////////////////////////////////////////////
            using (MySqlConnection connection =
                new MySqlConnection(connectionString))
            {
                ///////////////////////////////////////////////////////////
                //
                // The query which retrieves all the records in the
                // customers table.
                //
                ///////////////////////////////////////////////////////////

                ///////////////////////////////////////////////////////////
                //
                // Opens a database connection with the property settings
                // specified by the ConnectionString.
                //
                ///////////////////////////////////////////////////////////
                connection.Open();

                ///////////////////////////////////////////////////////////
                //
                // Submit the SQL statement to be executed against the
                // MySQL database.
                //
                ///////////////////////////////////////////////////////////
                MySqlCommand cmd = new MySqlCommand(query, connection);

                ///////////////////////////////////////////////////////////
                //
                // The MySqlDataAdapter represents a set of data commands
                // and a database connection that are used to fill a
                // dataset and update a MySQL database.
                //
                ///////////////////////////////////////////////////////////
                MySqlDataAdapter sqlDA = new MySqlDataAdapter(cmd);
                DataTable customerDataTable = new DataTable();
                try
                {
                    sqlDA.Fill(customerDataTable);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }

                ///////////////////////////////////////////////////////////
                //
                // Bind the ACME customer table to the Data Grid View.
                //
                ///////////////////////////////////////////////////////////
                dataGridView1.DataSource = customerDataTable;

            } // End of using (MySqlConnection connection = ...
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

          private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            try
            {
                selectedID = Convert.ToInt32(row.Cells[0].Value.ToString());

            } catch(Exception ex)
            {
                selectedID = 0;
            }

            if (selectedID > 0)
            {
                buttonAdd.Enabled = false;
                buttonEdit.Enabled = true;
                buttonDelete.Enabled = true;
            } else
            {
                buttonAdd.Enabled = true;
                buttonEdit.Enabled = false;
                buttonDelete.Enabled = false;
            }
            //MessageBox.Show($"{selectedID}");
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {/*
            int ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            string firstname = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string lastname = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            string query = $"update soft562.facebook_users set FirstName = '{firstname}' where UserID = {ID}";
            performSQLCommand(query);
            performSQLCommand("SELECT * FROM soft562.facebook_users");
            */
        }

      

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            /*
            int ID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            string firstname = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            string lastname = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            string query = $"INSERT INTO soft562.facebook_users (FirstName, LastName) VALUES({firstname},{lastname}";
            performSQLCommand(query);
            performSQLCommand("SELECT * FROM soft562.facebook_users");
            */
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            MessageBox.Show("UserAddedRow");
            /*
            DataGridViewRow r = e.Row;
            
            string firstname = r.Cells[1].Value.ToString();
            string lastname =r.Cells[2].Value.ToString();
            string query = $"INSERT INTO soft562.facebook_users (FirstName, LastName) VALUES({firstname},{lastname}";
            performSQLCommand(query);
            performSQLCommand("SELECT * FROM soft562.facebook_users");
            */
            
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            MessageBox.Show("dataGridView1_UserDeletingRow");

            DataGridViewRow r = e.Row;
            string ID = r.Cells[0].Value.ToString();
            string query = $"DELETE FROM soft562.facebook_users WHERE UserID = {ID}";
            performSQLCommand(query);
            performSQLCommand("SELECT * FROM soft562.facebook_users");
        }

        private void dataGridView1_NewRowNeeded(object sender, DataGridViewRowEventArgs e)
        {
            MessageBox.Show("NewRowNeeded");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string firstName = dataGridView1.Rows[selectedRow].Cells[1].Value.ToString();
            string lastName = dataGridView1.Rows[selectedRow].Cells[2].Value.ToString();

            string message = $"Are you sure you want to add this record? First name: { firstName }";
            string title = "Add record";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string query = $"INSERT INTO soft562.facebook_users (FirstName, LastName) VALUES('{firstName}', '{lastName}')";
                performSQLCommand(query);
                displayForm();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            int userID = Convert.ToInt32(dataGridView1.Rows[selectedRow].Cells[0].Value.ToString());
            string firstName = dataGridView1.Rows[selectedRow].Cells[1].Value.ToString();
            string lastName = dataGridView1.Rows[selectedRow].Cells[2].Value.ToString();

            string message = $"Are you sure you want to update this record? First name: { firstName }";
            string title = $"Update record for userID {userID}";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string query = $"UPDATE soft562.facebook_users SET FirstName = '{firstName}', LastName = '{lastName}' WHERE UserID = {userID}";
                performSQLCommand(query);
                displayForm();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string message = "Are you sure you want to delete this record?";
            string title = "Delete record";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                string query = $"DELETE from soft562.facebook_users WHERE UserID = {selectedID}";
                performSQLCommand(query);
                displayForm();
            }
            
        }
    }
}
