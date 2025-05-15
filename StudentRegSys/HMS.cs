using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite; //SQL Connector

namespace StudentRegSys
{
    public partial class Form1 : Form
    {
        // creating DB variables
        SQLiteConnection con = new SQLiteConnection("Data Source=psut.db;Version=3;");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteDataAdapter da = new SQLiteDataAdapter();
        DataSet ds = new DataSet();
        DataTable dt;
        SQLiteDataReader dr;


        public Form1()
        {
            InitializeComponent(); 
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            // 1- Open the connection to DB
            con.Open();

            // 2- Build Command Text
            cmd.Connection = con; // set the connection to instance of SqlCommand 

            cmd.CommandText = "Insert Into Patients (patient_id, name, email, gender, age, address, insurance_id)" +
                "values (@ptid, @ptname, @ptemail, @ptgender, @ptage, @ptaddress, @ptinsurance)";

            cmd.Parameters.AddWithValue("ptid", IdTexbox.Text);
            cmd.Parameters.AddWithValue("ptname", NameTextBox.Text);
            cmd.Parameters.AddWithValue("ptemail", EmailTextBox.Text);
            cmd.Parameters.AddWithValue("ptgender", (MaleRadio.Checked) ? 1 : 0);
            cmd.Parameters.AddWithValue("ptage", AgeNumeric.Value);
            cmd.Parameters.AddWithValue("ptaddress", AddressTextBox.Text);
            cmd.Parameters.AddWithValue("ptinsurance", InsuranceComboBox.SelectedValue);

            // 3- execute the command
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
                MessageBox.Show("Error .. Record Not Added");
            else
                MessageBox.Show("Record Inserted Successfully");

            // 4- close DB connection
            con.Close();
            fillGridView();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // filling InsuranceComboBox
            con.Open();
            string strCmd = "SELECT num, title FROM Insurances ORDER BY num";
            cmd = new SQLiteCommand(strCmd, con);
            da = new SQLiteDataAdapter(strCmd, con);
            dt = new DataTable();
            da.Fill(dt);
            InsuranceComboBox.DisplayMember = "title";
            InsuranceComboBox.ValueMember = "num";
            InsuranceComboBox.DataSource = dt;
            con.Close();

            // filling PatientsGridView
            fillGridView();
        }

        private void RetrieveButton_Click(object sender, EventArgs e)
        {
            // 1- Open the connection to DB
            con.Open();

            // 2- Build Command Text
            cmd.Connection = con; // set the connection to instance of SqlCommand 
            cmd.CommandText = "SELECT * from Patients WHERE patient_id=@ptid";
            cmd.Parameters.AddWithValue("ptid", IdTexboxR.Text);

            dr = cmd.ExecuteReader();

            if (!dr.HasRows)
                MessageBox.Show("No Such ID");
            else
            {
                UpdateButton.Show();
                DeleteButton.Show();

                while (dr.Read())
                {
                    NameTextBoxR.Text = dr["name"].ToString();
                    EmailTextBoxR.Text = dr["email"].ToString();
                    int gender = Convert.ToInt32(dr["gender"].ToString());
                    if (gender == 1)
                        MaleRadioR.Checked = true;
                    else
                        FemaleRadioR.Checked = true;
                    AddressTextBoxR.Text = dr["address"].ToString();
                    int insurance = Convert.ToInt32(dr["insurance_id"].ToString());
                    InsuranceComboBoxR.SelectedValue = insurance;
                    AgeNumericR.Value = Convert.ToDecimal(dr["age"]);
                }
            }

            // 4- close DB connection
            dr.Close();
            con.Close();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            // 1- Open the connection to DB
            con.Open();
            try
            {
                // 2- Build Command Text
                cmd.Connection = con; // set the connection to instance of SqlCommand 
                cmd.CommandText = "Update Patients SET name=@ptname, email=@ptemail, gender=@ptgender, age=@ptage, address=@ptaddress, insurance_id=@ptinsurance WHERE patient_id=@ptid";

                cmd.Parameters.AddWithValue("ptid", IdTexboxR.Text);
                cmd.Parameters.AddWithValue("ptname", NameTextBoxR.Text);
                cmd.Parameters.AddWithValue("ptemail", EmailTextBoxR.Text);
                cmd.Parameters.AddWithValue("ptgender", (MaleRadioR.Checked) ? 1 : 0);
                cmd.Parameters.AddWithValue("ptage", AgeNumericR.Value);
                cmd.Parameters.AddWithValue("ptaddress", AddressTextBoxR.Text);
                cmd.Parameters.AddWithValue("ptinsurance", InsuranceComboBoxR.SelectedValue);

                // 3- execute the command
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    MessageBox.Show("Error .. Record Not Updated");
                else
                    MessageBox.Show("Record Updated Successfully");
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                // 4- close DB connection
                con.Close();
            }
            fillGridView();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // 1- Open the connection to DB
            con.Open();

            // 2- Build Command Text
            cmd.Connection = con; // set the connection to instance of SqlCommand 
            cmd.CommandText = "DELETE FROM Patients WHERE patient_id=@ptid";
            cmd.Parameters.AddWithValue("ptid", IdTexboxR.Text);

            // 3- execute the command
            DialogResult result = MessageBox.Show("Are you Sure you want to delete this Record?",
                "Delete Record", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0)
                    MessageBox.Show("Error .. Record Not Deleted");
                else
                    MessageBox.Show("Record Deleted Successfully");
            }
            // 4- close DB connection
            con.Close();
            fillGridView();
        }

        private void fillGridView()
        {
            con.Open();
            string strCmd2 = "SELECT * FROM Patients ORDER BY patient_id";
            cmd = new SQLiteCommand(strCmd2, con);
            da = new SQLiteDataAdapter(strCmd2, con);
            ds = new DataSet();
            da.Fill(ds, "Patients");
            PatientsGridView.DataSource = ds.Tables["Patients"].DefaultView;
            con.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
