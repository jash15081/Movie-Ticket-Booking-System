using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace MovieTicketBooking.Customer
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String role = Request.QueryString["role"];

                if (role == "admin")
                {
                    lblRole.Text = "Admin Login";
                }
                else if (role == "customer")
                {
                    lblRole.Text = "Customer Login";
                    
                }
                else
                {
                    // Default case or redirect if no role is provided
                    lblRole.Text = "Login";
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Retrieve user input
                    string email = txtEmail.Text.Trim();
                    string password = txtPassword.Text.Trim(); // In a real-world scenario, hash the password

                    // Check if the login is for customer or admin
                    string role = Request.QueryString["role"];

                    // Verify user credentials
                    string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        string query = "";
                        bool isAdmin = false;

                        // If the role is "admin", query the Admins table, otherwise query the Profiles table for customers
                        if (role == "admin")
                        {
                            query = "SELECT AdminID, Name FROM Admins WHERE Email = @Email AND Password = @Password";
                            isAdmin = true;
                        }
                        else
                        {
                            query = "SELECT ProfileID, FirstName, LastName FROM Profiles WHERE Email = @Email AND Password = @Password";
                        }

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Password", password);

                            con.Open();
                            SqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                if (isAdmin)
                                {
                                    // Admin login successful
                                    string adminName = reader["Name"].ToString();
                                    int adminID = Convert.ToInt32(reader["AdminID"]);

                                    // Store admin details in session
                                    Session["AdminName"] = adminName;
                                    Session["AdminID"] = adminID;

                                    // Redirect to the admin home page
                                    Response.Redirect("/Admin/AdminHome.aspx");
                                }
                                else
                                {
                                    // Customer login successful
                                    string firstName = reader["FirstName"].ToString();
                                    string lastName = reader["LastName"].ToString();
                                    int ProfileID = Convert.ToInt32(reader["ProfileID"]);

                                    // Store customer details in session
                                    Session["UserName"] = $"{firstName} {lastName}";
                                    Session["ProfileID"] = ProfileID;

                                    // Redirect to the customer home page
                                    Response.Redirect("Home.aspx");
                                }
                            }
                            else
                            {
                                // Login failed
                                lblMessage.Text = "Invalid email or password.";
                                lblMessage.CssClass = "error-message";
                            }
                        }
                    }
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
