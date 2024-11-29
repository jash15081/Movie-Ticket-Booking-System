using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Customer
{
    
    public partial class Booking : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ProfileID"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                int movieID = Convert.ToInt32(Session["SelectedMovieID"]);
                LoadMovieDetails(movieID);
                BindTheaters(movieID); // Only bind theaters with showtimes for the specific movie
            }
        }

        private void LoadMovieDetails(int movieID)
        {
            // Fetch movie details from database and show in litMovieDetails
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Title, Genre, Language, Description FROM Movies WHERE MovieID = @MovieID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MovieID", movieID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    litMovieDetails.Text = $"<h2>{reader["Title"]}</h2><p>{reader["Description"]}</p><p>Genre: {reader["Genre"]}</p><p>Language: {reader["Language"]}</p>";
                }
            }
        }

        private void BindTheaters(int movieID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Query to get only those theaters with showtimes for the specific movie
                string query = @"
                    SELECT DISTINCT t.TheaterID, t.Name 
                    FROM Theaters t
                    INNER JOIN Showtimes s ON t.TheaterID = s.TheaterID
                    WHERE s.MovieID = @MovieID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@MovieID", movieID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlTheater.DataSource = dt;
                ddlTheater.DataTextField = "Name";
                ddlTheater.DataValueField = "TheaterID";
                ddlTheater.DataBind();
                ddlTheater.Items.Insert(0, new ListItem("Select Theater", ""));
            }
        }

        protected void ddlTheater_SelectedIndexChanged(object sender, EventArgs e)
        {
            int theaterID = Convert.ToInt32(ddlTheater.SelectedValue);
            int movieID = Convert.ToInt32(Session["SelectedMovieID"]);
            BindShowtimes(theaterID, movieID);
        }

        
        

        private void BindShowtimes(int theaterID, int movieID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Query to get only those showtimes for the specific movie in the selected screen
                string query = @"
                    SELECT ShowID, ShowTime 
                    FROM Showtimes 
                    WHERE TheaterID = @TheaterID AND MovieID = @MovieID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TheaterID", theaterID);
                cmd.Parameters.AddWithValue("@MovieID", movieID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlShowtime.DataSource = dt;
                ddlShowtime.DataTextField = "ShowTime";
                ddlShowtime.DataValueField = "ShowID";
                ddlShowtime.DataBind();
                ddlShowtime.Items.Insert(0, new ListItem("Select Showtime", ""));
            }
        }

        protected void ddlShowtime_SelectedIndexChanged(object sender, EventArgs e)
        {
            int showID = Convert.ToInt32(ddlShowtime.SelectedValue);
            BindAvailableSeats(showID); // Bind available seats for the selected showtime
        }

        private void BindAvailableSeats(int showID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Query to get available seats for the selected showtime
                string query = @"
                    SELECT AvailableSeats, TicketPrice
                    FROM Showtimes 
                    WHERE ShowID = @ShowID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ShowID", showID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    litAvailableSeats.Text = $"<p>Available Seats: {reader["AvailableSeats"]}</p>";
                    litTicketPrice.Text = $"<p>Ticket Price: Rs.{reader["TicketPrice"]}</p>";
                }
                else
                {
                    litAvailableSeats.Text = "<p>No available seats found for this showtime.</p>";
                }

            }
        }

        protected void btnBookNow_Click(object sender, EventArgs e)
        {
            if (Session["ProfileID"] == null)
            {
                litMessage.Text = "<p style='color:red;'>Please log in to book tickets.</p>";
                return;
            }

            int profileID = Convert.ToInt32(Session["ProfileID"]);
            int showID = Convert.ToInt32(ddlShowtime.SelectedValue);
            int seatsToBook = Convert.ToInt32(txtSeats.Text); // Number of seats entered by the user
            decimal ticketPrice = 0;
            decimal totalAmount = 0;
            int availableSeats = 0;
            DateTime ShowDate = DateTime.Now;


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Start a SQL transaction
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // 1. Fetch the ticket price and available seats for the selected show
                    string query = @"
                SELECT TicketPrice, AvailableSeats, ShowDate 
                FROM Showtimes 
                WHERE ShowID = @ShowID";

                    SqlCommand cmd = new SqlCommand(query, conn, transaction);
                    cmd.Parameters.AddWithValue("@ShowID", showID);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ticketPrice = Convert.ToDecimal(reader["TicketPrice"]);
                        availableSeats = Convert.ToInt32(reader["AvailableSeats"]);
                        ShowDate = Convert.ToDateTime(reader["ShowDate"]);
                    }
                    reader.Close();

                    // Check if enough seats are available
                    if (seatsToBook > availableSeats)
                    {
                        litMessage.Text = "<p style='color:red;'>Not enough available seats for this showtime.</p>";
                        return;
                    }

                    if (DateTime.Now > ShowDate)
                    {
                        litMessage.Text = "<p style='color:red;'>Show is now closed. choose another time or theater for book tickets.</p>";
                    }

                        // Calculate the total amount
                        totalAmount = seatsToBook * ticketPrice;

                    // 2. Insert a new booking record into the Bookings table
                    string insertQuery = @"
                INSERT INTO Bookings (ProfileID, ShowID, TotalAmount, PaymentStatus, CancellationStatus, seats, BookingDate) 
                VALUES (@ProfileID, @ShowID, @TotalAmount, 'Pending', 'Active', @seats, @BookingDate)";

                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn, transaction);
                    insertCmd.Parameters.AddWithValue("@ProfileID", profileID);
                    insertCmd.Parameters.AddWithValue("@ShowID", showID);
                    insertCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    insertCmd.Parameters.AddWithValue("@seats", seatsToBook);
                    insertCmd.Parameters.AddWithValue("@BookingDate", DateTime.Now);
                    insertCmd.ExecuteNonQuery();

                    // 3. Update the available seats in the Showtimes table
                    string updateQuery = @"
                UPDATE Showtimes 
                SET AvailableSeats = AvailableSeats - @SeatsToBook 
                WHERE ShowID = @ShowID";

                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@SeatsToBook", seatsToBook);
                    updateCmd.Parameters.AddWithValue("@ShowID", showID);
                    updateCmd.ExecuteNonQuery();

 
                    // Commit the transaction if both operations succeed
                    
                    transaction.Commit();
                        
                        litMessage.Text = "<p style='color:green;'>Booking successful! Your total amount is Rs." + totalAmount + "</p>";
                        btnBookNow.Enabled = false;
                        btnBookingDetails.Enabled = true;
                }
                catch (Exception ex)
                {
                    // Roll back the transaction in case of an error
                    transaction.Rollback();
                    litMessage.Text = $"<p style='color:red;'>An error occurred: {ex.Message} {profileID}</p>";
                }
            }
        }

        protected void btnBookingDetails_Click(object sender, EventArgs e)
        {
            
            int profileID = Convert.ToInt32(Session["ProfileID"]);
            int showID = Convert.ToInt32(ddlShowtime.SelectedValue);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Query to get available seats for the selected showtime
                string query = @"
                    SELECT BookingID
                    FROM Bookings 
                    WHERE ShowID = @ShowID AND ProfileID = @profileID" ;

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ShowID", showID);
                cmd.Parameters.AddWithValue("@profileID", profileID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Session["SelectedBookingID"] = reader["BookingID"];
                    Response.Redirect("BookingDetails.aspx");
                }
               
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Home.aspx");
        }

    }
}
