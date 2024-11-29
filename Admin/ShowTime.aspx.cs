using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Admin
{
    public partial class ShowTime : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;
        int movieID, screenID, theaterID;
        int availableSeats = 0, capacity = 0, remaining = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["AdminName"] == null)
                {
                    Response.Redirect("../Front.aspx");
                }
                BindMovies();
                BindTheaters();

                BindGrid();
            }
        }

        private void BindMovies()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT MovieID, Title FROM Movies";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlMovies.DataSource = dt;
                ddlMovies.DataTextField = "Title";
                ddlMovies.DataValueField = "MovieID";
                ddlMovies.DataBind();
                ddlMovies.Items.Insert(0, new ListItem("Select Movie", ""));
            }
        }

        private void BindTheaters()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT TheaterID, Name FROM Theaters";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlTheaters.Enabled = true;
                ddlTheaters.DataSource = dt;
                ddlTheaters.DataTextField = "Name";
                ddlTheaters.DataValueField = "TheaterID";
                ddlTheaters.DataBind();
                ddlTheaters.Items.Insert(0, new ListItem("Select Theater", ""));
            }
        }

        private void BindScreens(int theaterID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ScreenID, Name FROM Screens WHERE TheaterID = @TheaterID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    ddlScreens.Enabled = true;
                    ddlScreens.DataSource = dt;
                    ddlScreens.DataTextField = "Name"; // Screen name
                    ddlScreens.DataValueField = "ScreenID"; // Screen ID only
                    ddlScreens.DataBind();
                    ddlScreens.Items.Insert(0, new ListItem("Select Screen", ""));
                }
                else
                {
                    ddlScreens.Items.Clear();
                    ddlScreens.Items.Insert(0, new ListItem("No screens available", ""));
                    ddlScreens.Enabled = false; // Disable dropdown if no screens are found
                }
            }
        }

        private void BindGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Showtimes";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvShowTimes.DataSource = dt;
                gvShowTimes.DataBind();

                lblNoData.Visible = dt.Rows.Count == 0;
            }
        }

        protected void ddlMovies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMovies.SelectedValue != "")
            {
                // Clear the selected items in other dropdowns
                ddlTheaters.SelectedIndex = 0;
                ddlScreens.Items.Clear();
                ddlScreens.Items.Insert(0, new ListItem("Select Screen", ""));
                ddlTheaters.Enabled = true;
                ddlScreens.Enabled = false;

                // Bind theaters based on selected movie
                BindTheaters();
            }
        }

        protected void ddlTheaters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTheaters.SelectedValue != "")
            {
                int theaterID;
                if (int.TryParse(ddlTheaters.SelectedValue, out theaterID))
                {
                    ddlScreens.Enabled = true;
                    BindScreens(theaterID);
                }
            }
        }

        protected void btnAddShowTime_Click(object sender, EventArgs e)
        {
            decimal ticketPrice;
            TimeSpan showTime;
            DateTime showDate;

            if (int.TryParse(ddlMovies.SelectedValue, out movieID) &&
                int.TryParse(ddlScreens.SelectedValue, out screenID) &&
                int.TryParse(ddlTheaters.SelectedValue, out theaterID) &&
                decimal.TryParse(txtTicketPrice.Text, out ticketPrice) &&
                TimeSpan.TryParse(txtShowTime.Text, out showTime) &&
                DateTime.TryParse(txtShowDate.Text, out showDate))
            {
                DateTime releaseDate = GetMovieReleaseDate(movieID);
                if (showDate >= releaseDate)
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string query = "SELECT Remaining, Capacity FROM Screens WHERE ScreenID = @ScreenID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ScreenID", screenID);
                        conn.Open();

                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            remaining = rdr.GetInt32(0);
                            capacity = rdr.GetInt32(1);
                            availableSeats = remaining;
                        }
                        rdr.Close();
                        conn.Close();

                        query = "INSERT INTO Showtimes (MovieID, ScreenID, TheaterID, ShowTime, ShowDate, AvailableSeats, TicketPrice) " +
                                "VALUES (@MovieID, @ScreenID, @TheaterID, @ShowTime, @ShowDate, @AvailableSeats, @TicketPrice)";
                        cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@MovieID", movieID);
                        cmd.Parameters.AddWithValue("@ScreenID", screenID);
                        cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                        cmd.Parameters.AddWithValue("@ShowTime", showTime);
                        cmd.Parameters.AddWithValue("@ShowDate", showDate);
                        cmd.Parameters.AddWithValue("@AvailableSeats", availableSeats);
                        cmd.Parameters.AddWithValue("@TicketPrice", ticketPrice);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        ClearForm();
                        BindGrid();
                    }
                }
                else
                {
                    lblError.Text = "Show date cannot be before the movie release date.";
                    lblError.Visible = true;
                }
            }
        }

        private DateTime GetMovieReleaseDate(int movieID)
        {
            DateTime releaseDate = DateTime.MinValue;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ReleaseDate FROM Movies WHERE MovieID = @MovieID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MovieID", movieID);

                conn.Open();
                object result = cmd.ExecuteScalar();
                conn.Close();

                if (result != null && result != DBNull.Value)
                {
                    releaseDate = Convert.ToDateTime(result);
                }
            }

            return releaseDate;
        }

        protected void gvShowTimes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvShowTimes.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void gvShowTimes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int showID = Convert.ToInt32(gvShowTimes.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvShowTimes.Rows[e.RowIndex];

            decimal ticketPrice = Convert.ToDecimal((row.FindControl("txtTicketPriceEdit") as TextBox).Text);
            TimeSpan showTime = TimeSpan.Parse((row.FindControl("txtShowTimeEdit") as TextBox).Text);
            DateTime showDate = DateTime.Parse((row.FindControl("txtShowDateEdit") as TextBox).Text);

            if (showDate >= GetMovieReleaseDate(movieID))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Showtimes SET ShowTime = @ShowTime, ShowDate = @ShowDate, TicketPrice = @TicketPrice WHERE ShowID = @ShowID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ShowTime", showTime);
                    cmd.Parameters.AddWithValue("@ShowDate", showDate);
                    cmd.Parameters.AddWithValue("@TicketPrice", ticketPrice);
                    cmd.Parameters.AddWithValue("@ShowID", showID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    BindGrid();
                }
            }
            else
            {
                lblError.Text = "Show date cannot be before movie release date.";
                lblError.Visible = true;
            }

            gvShowTimes.EditIndex = -1;
            BindGrid();
        }

        protected void gvShowTimes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvShowTimes.EditIndex = -1;
            BindGrid();
        }

        protected void gvShowTimes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int showID = Convert.ToInt32(gvShowTimes.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Showtimes WHERE ShowID = @ShowID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ShowID", showID);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                BindGrid();
            }
        }

        private void ClearForm()
        {
            ddlMovies.SelectedIndex = 0;
            ddlTheaters.SelectedIndex = 0;
            ddlScreens.Items.Clear();
            ddlScreens.Items.Insert(0, new ListItem("Select Screen", ""));
            ddlScreens.Enabled = false;
            txtTicketPrice.Text = "";
            txtShowTime.Text = "";
            txtShowDate.Text = "";
            lblError.Visible = false;
        }


        public string GetMovieTitle(object movieID)
        {
            int id;
            if (int.TryParse(movieID.ToString(), out id))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Title FROM Movies WHERE MovieID = @MovieID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@MovieID", id);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();
                    return result != null ? result.ToString() : "Unknown";
                }
            }
            return "Unknown";
        }

        public string GetTheaterName(object theaterID)
        {
            int id;
            if (int.TryParse(theaterID.ToString(), out id))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Name FROM Theaters WHERE TheaterID = @TheaterID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TheaterID", id);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();
                    return result != null ? result.ToString() : "Unknown";
                }
            }
            return "Unknown";
        }

        public string GetScreenName(object screenID)
        {
            int id;
            if (int.TryParse(screenID.ToString(), out id))
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT Name FROM Screens WHERE ScreenID = @ScreenID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ScreenID", id);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    conn.Close();
                    return result != null ? result.ToString() : "Unknown";
                }
            }
            return "Unknown";
        }


        protected void BackHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx");
        }
    }
}
