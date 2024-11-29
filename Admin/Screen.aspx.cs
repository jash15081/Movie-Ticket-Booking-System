using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Admin
{
    public partial class Screen : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;
        private int theaterID;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["AdminName"] == null)
                {
                    Response.Redirect("../Front.aspx");
                }
                RetrieveTheaterID();
                if (theaterID > 0)
                {
                    BindGrid();
                }
            }
        }





        private void BindGrid()
        {

            RetrieveTheaterID();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ScreenID, TheaterID, Name, Capacity, Remaining FROM Screens WHERE TheaterID = @TheaterID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TheaterID", theaterID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvScreens.DataSource = dt;
                gvScreens.DataBind();

                lblNoData.Visible = dt.Rows.Count == 0;
            }
        }


        protected void btnAddScreen_Click(object sender, EventArgs e)
        {
            RetrieveTheaterID();
            // Check if TheaterID is valid
            if (theaterID <= 0)
            {
                // Display an error message or handle invalid TheaterID
                lblNoData.Text = "Invalid Theater ID. Please ensure the Theater ID is correct." + theaterID;
                lblNoData.Visible = true;
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Screens (TheaterID, Name, Capacity, Remaining) VALUES (@TheaterID, @Name, @Capacity, @Remaining)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                cmd.Parameters.AddWithValue("@Name", txtName.Text);
                cmd.Parameters.AddWithValue("@Capacity", txtCapacity.Text);
                cmd.Parameters.AddWithValue("@Remaining", txtRemaining.Text);

                string que = "UPDATE Theaters SET TotalScreens = TotalScreens + 1 WHERE TheaterID = @TheaterID";
                SqlCommand cmmd = new SqlCommand(que, conn);
                cmmd.Parameters.AddWithValue("@TheaterID", theaterID);
                

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    cmmd.ExecuteNonQuery();
                    lblNoData.Visible = false; // Hide no data message if insert is successful
                }
                catch (SqlException ex)
                {
                    // Display the exception message
                    lblNoData.Text = "Error: " + ex.Message;
                    lblNoData.Visible = true;
                }
                finally
                {
                    conn.Close();
                }
            }

            // Clear the form fields
            txtName.Text = "";
            txtCapacity.Text = "";
            txtRemaining.Text = "";

            // Refresh the GridView
            BindGrid();
        }


        protected void gvScreens_RowEditing(object sender, GridViewEditEventArgs e)
        {
            RetrieveTheaterID();
            gvScreens.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvScreens_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = gvScreens.Rows[e.RowIndex];
            int screenID = Convert.ToInt32(gvScreens.DataKeys[e.RowIndex].Values[0]);
            int theaterID = Convert.ToInt32(gvScreens.DataKeys[e.RowIndex].Values[1]);
            string name = ((TextBox)row.FindControl("txtEditName")).Text;
            string capacity = ((TextBox)row.FindControl("txtEditCapacity")).Text;
            string remaining = ((TextBox)row.FindControl("txtEditRemaining")).Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Screens SET Name = @Name, Capacity = @Capacity, Remaining = @Remaining WHERE ScreenID = @ScreenID AND TheaterID = @TheaterID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ScreenID", screenID);
                cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Capacity", capacity);
                cmd.Parameters.AddWithValue("@Remaining", remaining);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            gvScreens.EditIndex = -1;
            BindGrid();
        }

        protected void gvScreens_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            RetrieveTheaterID();
            gvScreens.EditIndex = -1;
            BindGrid();
        }

        protected void gvScreens_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int screenID = Convert.ToInt32(gvScreens.DataKeys[e.RowIndex].Values[0]);
            int theaterID = Convert.ToInt32(gvScreens.DataKeys[e.RowIndex].Values[1]);

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
                (SELECT ShowID FROM Showtimes WHERE ScreenID = @ScreenID)";
                    using (SqlCommand cmd = new SqlCommand(deleteBookingsQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ScreenID", screenID);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 2: Delete related showtimes
                    string deleteShowtimesQuery = "DELETE FROM Showtimes WHERE ScreenID = @ScreenID";
                    using (SqlCommand cmd = new SqlCommand(deleteShowtimesQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ScreenID", screenID);
                        cmd.ExecuteNonQuery();
                    }

                    // Step 3: Delete the screen
                    string deleteScreenQuery = "DELETE FROM Screens WHERE ScreenID = @ScreenID AND TheaterID = @TheaterID";
                    using (SqlCommand cmd = new SqlCommand(deleteScreenQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@ScreenID", screenID);
                        cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                        cmd.ExecuteNonQuery();
                    }

                    // Commit the transaction if everything succeeded
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

            BindGrid();
        }


        private void RetrieveTheaterID()
        {
            if (Session["TheaterID"] != null)
            {
                int parsedTheaterID;
                if (int.TryParse(Session["TheaterID"].ToString(), out parsedTheaterID))
                {
                    theaterID = parsedTheaterID;
                }
                else
                {
                    lblNoData.Text = "Invalid Theater ID provided.";
                    lblNoData.Visible = true;
                }
            }
            else
            {
                Response.Redirect("Theater.aspx");
                lblNoData.Text = "Theater ID is required.";
                lblNoData.Visible = true;
            }
        }

    }
}
