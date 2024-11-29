using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Admin
{
    public partial class Theater : System.Web.UI.Page
    {
        // Connection string to the database (replace with your actual connection string)
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                if (Session["AdminName"] == null)
                {
                    Response.Redirect("../Front.aspx");
                }
                BindTheaterGrid(); // Bind the theater grid on initial page load
            }
        }

        // Bind data to GridView from the database
        private void BindTheaterGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Theaters", conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvTheaters.DataSource = dt;
                    gvTheaters.DataBind();

                    lblNoData.Visible = dt.Rows.Count == 0; // Show no data label if there are no rows
                }
            }
        }

        // Add a new theater record
        protected void btnAddTheater_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string address = txtAddress.Text.Trim();
            string city = txtCity.Text.Trim();
            string contactNumber = txtContactNumber.Text.Trim();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Theaters (Name, Address, City, ContactNumber, TotalScreens) VALUES (@Name, @Address, @City, @ContactNumber, @TotalScreens)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                    cmd.Parameters.AddWithValue("@TotalScreens", 0);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Rebind the GridView to reflect the new entry
            BindTheaterGrid();

            // Clear form fields
            ClearFormFields();

            // Toggle form back to the list view
            //ClientScript.RegisterStartupScript(this.GetType(), "toggleForm", "toggleForm();", true);
        }

        // Edit theater record
        protected void gvTheaters_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTheaters.EditIndex = e.NewEditIndex;
            BindTheaterGrid();
        }

        // Update theater record
        protected void gvTheaters_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int theaterID = Convert.ToInt32(gvTheaters.DataKeys[e.RowIndex].Value);
            string name = ((TextBox)gvTheaters.Rows[e.RowIndex].FindControl("txtEditName")).Text;
            string address = ((TextBox)gvTheaters.Rows[e.RowIndex].FindControl("txtEditAddress")).Text;
            string city = ((TextBox)gvTheaters.Rows[e.RowIndex].FindControl("txtEditCity")).Text;
            string contactNumber = ((TextBox)gvTheaters.Rows[e.RowIndex].FindControl("txtEditContactNumber")).Text;
            int totalScreens = int.Parse(((TextBox)gvTheaters.Rows[e.RowIndex].FindControl("txtEditTotalScreens")).Text);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Theaters SET Name = @Name, Address = @Address, City = @City, ContactNumber = @ContactNumber, TotalScreens = @TotalScreens WHERE TheaterID = @TheaterID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@ContactNumber", contactNumber);
                    cmd.Parameters.AddWithValue("@TotalScreens", totalScreens);
                    cmd.Parameters.AddWithValue("@TheaterID", theaterID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            gvTheaters.EditIndex = -1; // Exit edit mode
            BindTheaterGrid();
        }

        // Cancel edit mode
        protected void gvTheaters_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTheaters.EditIndex = -1;
            BindTheaterGrid();
        }

        // Delete theater record
        protected void gvTheaters_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int theaterID = Convert.ToInt32(gvTheaters.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Step 1: Delete related bookings
                    string deleteBookingsQuery = @"
                DELETE FROM Bookings 
                WHERE ShowID IN 
                (SELECT ShowID FROM Showtimes WHERE ScreenID IN 
                (SELECT ScreenID FROM Screens WHERE TheaterID = @TheaterID))";
                    using (SqlCommand cmd = new SqlCommand(deleteBookingsQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 2: Delete related showtimes
                    string deleteShowtimesQuery = @"
                DELETE FROM Showtimes 
                WHERE ScreenID IN 
                (SELECT ScreenID FROM Screens WHERE TheaterID = @TheaterID)";
                    using (SqlCommand cmd = new SqlCommand(deleteShowtimesQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 3: Delete related screens
                    string deleteScreensQuery = "DELETE FROM Screens WHERE TheaterID = @TheaterID";
                    using (SqlCommand cmd = new SqlCommand(deleteScreensQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 4: Delete the theater
                    string deleteTheaterQuery = "DELETE FROM Theaters WHERE TheaterID = @TheaterID";
                    using (SqlCommand cmd = new SqlCommand(deleteTheaterQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                        cmd.ExecuteNonQuery();
                    }

                    // Commit the transaction if all deletions succeed
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Rollback the transaction if any error occurs
                    transaction.Rollback();
                    lblNoData.Text = $"Error: {ex.Message}";
                    lblNoData.Visible = true;
                }
            }

            BindTheaterGrid();
        }

        // Clear the form fields after adding or editing a theater
        private void ClearFormFields()
        {
            txtName.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtContactNumber.Text = "";
        }

        protected void btnManageScreens_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int theaterID = Convert.ToInt32(btn.CommandArgument);

            // Store TheaterID in session
            Session["TheaterID"] = theaterID;
            System.Diagnostics.Debug.WriteLine("Stored TheaterID in Session: " + theaterID);

            // Redirect to Screen.aspx page
            Response.Redirect("Screen.aspx");
        }

        protected void BackHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx");
        }
    }
}
