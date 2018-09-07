using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ThreeTierReview
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                btnSubmit.Enabled = false;
                String fileEntry =
                    $"{tbFirstName.Text} {tbLastName.Text} {tbEmployeeId.Text} {tbDepartment.Text} {tbSalary.Text}; ";
                File.AppendAllText(@"C:\Users\Ryan\Desktop\Employee.txt", fileEntry.ToString() + "\r\n");

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString))
                {
                    String query =
                        "INSERT INTO Employee(FirstName, LastName, EmployeeId, Department, Salary) VALUES(@param1, @param2, @param3, @param4, @param5)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.Add("@param1", SqlDbType.Char).Value = tbFirstName.Text;
                    cmd.Parameters.Add("@param2", SqlDbType.Char).Value = tbLastName.Text;
                    cmd.Parameters.Add("@param3", SqlDbType.Char).Value = tbEmployeeId.Text;
                    cmd.Parameters.Add("@param4", SqlDbType.Char).Value = tbDepartment.Text;
                    cmd.Parameters.Add("@param5", SqlDbType.Money).Value = decimal.Parse(tbSalary.Text, NumberStyles.Currency);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

                btnSubmit.Enabled = true;

                foreach (var control in Enumerable.OfType<TextBox>(Controls))
                    control.Text = String.Empty;
            }
            catch (Exception error)
            {
                if (error is SqlException)
                    MessageBox.Show("An error occured while trying to update the database:\r\n" + error.Message);
                else if (error is FileNotFoundException)
                    MessageBox.Show("The file that you are trying to access is unaccessable:\r\n" + error.Message);
                else if (error is FileNotFoundException)
                    MessageBox.Show("The file that you are trying to access could not be found:\r\n" + error.Message);
                else
                    MessageBox.Show("An error has occured:\r\n" + error.Message);
                throw;
            }                            

        }
    }
}
