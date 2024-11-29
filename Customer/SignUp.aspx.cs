using System;
using System.Configuration; // For accessing the connection string
using System.Data.SqlClient; // For SQL operations
using System.Web.UI;

namespace MovieTicketBooking.Customer
{
    public partial class SignUp : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Optional: Enable unobtrusive validation if necessary
            // ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Retrieve user input
                    string firstName = txtFirstName.Text.Trim();
                    string lastName = txtLastName.Text.Trim();
                    string email = txtEmail.Text.Trim();
                    string password = txtPassword.Text.Trim();
                    string mobileNumber = txtMobile.Text.Trim();
                    string address = txtAddress.Text.Trim();
                    string dateOfBirth = txtDateOfBirth.Text.Trim();
                    string gender = ddlGender.SelectedValue;

                    // Insert into the database
                    string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "INSERT INTO Profiles (FirstName, LastName, Email, Password, MobileNumber, Address, DateOfBirth, Gender) " +
                                       "VALUES (@FirstName, @LastName, @Email, @Password, @MobileNumber, @Address, @DateOfBirth, @Gender)";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            // Add parameters to avoid SQL injection
                            cmd.Parameters.AddWithValue("@FirstName", firstName);
                            cmd.Parameters.AddWithValue("@LastName", lastName);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Password", password); // Hash password in a real-world scenario
                            cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);
                            cmd.Parameters.AddWithValue("@Address", address);
                            cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                            cmd.Parameters.AddWithValue("@Gender", gender);

                            con.Open();
                            cmd.ExecuteNonQuery(); // Execute the SQL query
                        }
                    }

                    // Set success message and redirect to login page
                    lblMessage.Text = "Sign Up Successful! Redirecting to login page...";
                    lblMessage.CssClass = "success-message"; // Optional: style for success message

                    // Redirect to Login.aspx after 2 seconds
                    Response.Redirect("Login.aspx");
                }
                catch (Exception ex)
                {
                    // Handle exceptions and show an error message
                    lblMessage.Text = "An error occurred: " + ex.Message;
                    lblMessage.CssClass = "error-message";
                }
            }
        }
    }
}
