using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Customer
{
    public partial class BookingDetails : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;
        int bookingID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                

                // Check if BookingID or SelectedBookingID is available in the session
                if (Request.QueryString["Adm"] == "1")
                {
                    btnCancel.Enabled = false;
                    txtCancel.Enabled = false;
                }
                else
                {
                    btnCancel.Enabled = true;
                    txtCancel.Enabled = true;
                }


                if (Session["BookingID"] != null)
                {
                    bookingID = Convert.ToInt32(Session["BookingID"]);
                }
                else if (Session["SelectedBookingID"] != null)
                {
                    bookingID = Convert.ToInt32(Session["SelectedBookingID"]);
                }
                else if (Session["ProfileID"] == null)
                {
                    litBookingDetails.Text = "<p>No booking details available.</p>";
                    Response.Redirect("Login.aspx");
                    return;
                }
                else
                {
                    Response.Redirect("Home.aspx");
                }

                LoadBookingDetails(bookingID);
            }
        }

        private void LoadBookingDetails(int bookingID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT b.BookingDate, b.TotalAmount, b.Seats, s.ShowTime, s.ShowDate, sc.Name AS ScreenName, t.Name AS TheaterName, t.Address, t.City, t.ContactNumber, m.Title, m.Description, m.Duration, m.Genre, m.Language, m.ReleaseDate
                    FROM Bookings b
                    INNER JOIN Showtimes s ON b.ShowID = s.ShowID
                    INNER JOIN Screens sc ON s.ScreenID = sc.ScreenID
                    INNER JOIN Theaters t ON sc.TheaterID = t.TheaterID
                    INNER JOIN Movies m ON s.MovieID = m.MovieID
                    WHERE b.BookingID = @BookingID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookingID", bookingID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    litBookingDetails.Text = $@"
                        <h2>Booking Details</h2>
                        <p><strong>Booking Date:</strong> {reader["BookingDate"]}</p>
                        <p><strong>Total Amount:</strong> Rs. {reader["TotalAmount"]}</p>
                        <p><strong>Number of Seats:</strong> {reader["Seats"]}</p>
                        <p><strong>Show Time:</strong> {reader["ShowTime"]}</p>
                        <p><strong>Show Date:</strong> {reader["ShowDate"]}</p>
                        <p><strong>Screen Name:</strong> {reader["ScreenName"]}</p>
                        <p><strong>Theater Name:</strong> {reader["TheaterName"]}</p>
                        <p><strong>Theater Address:</strong> {reader["Address"]}</p>
                        <p><strong>Theater City:</strong> {reader["City"]}</p>
                        <p><strong>Theater Contact Number:</strong> {reader["ContactNumber"]}</p>
                        <p><strong>Movie Title:</strong> {reader["Title"]}</p>
                        <p><strong>Movie Description:</strong> {reader["Description"]}</p>
                        <p><strong>Movie Duration:</strong> {reader["Duration"]} minutes</p>
                        <p><strong>Movie Genre:</strong> {reader["Genre"]}</p>
                        <p><strong>Movie Language:</strong> {reader["Language"]}</p>
                        <p><strong>Release Date:</strong> {reader["ReleaseDate"]}</p>";
                }
                else
                {
                    litBookingDetails.Text = "<p>No booking details found.</p>";
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Adm"] == "1")
            {
                Response.Redirect("/Admin/Bookings.aspx");
            }
            Response.Redirect("Home.aspx");
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            int seatsToCancel;

            if (!int.TryParse(txtCancel.Text, out seatsToCancel) || seatsToCancel <= 0)
            {
                litMessage.Text = "<p style='color:red;'>Please enter a valid number of tickets to cancel.</p>";
                return;
            }

            int ShowID = 0;
            DateTime showDate = DateTime.Now;
            int BookingID = Convert.ToInt32(Session["SelectedBookingID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Fetch the ShowID and ShowDate for the given BookingID
                string query = @"SELECT b.ShowID, s.ShowDate 
                         FROM Bookings b 
                         INNER JOIN Showtimes s ON b.ShowID = s.ShowID 
                         WHERE b.BookingID = @BookingID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BookingID", BookingID);

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    ShowID = Convert.ToInt32(rdr["ShowID"]);
                    showDate = Convert.ToDateTime(rdr["ShowDate"]);
                    litMessage.Text = "<p>Debug: Fetched Show Date is " + showDate.ToString() + "</p>";  // Debug message
                }
                rdr.Close();

                // If today's date is greater than the show date, display an error message
                if (DateTime.Now.Date > showDate)
                {
                    litMessage.Text = "<p style='color:red;'>Error: You cannot cancel tickets for a past showtime.</p>";
                    return;
                }

                // Step 1: Get the current booked seats from the database
                string queryCheckBookingSeats = "SELECT seats FROM Bookings WHERE BookingID = @BookingID";
                SqlCommand cmdCheckBookingSeats = new SqlCommand(queryCheckBookingSeats, conn);
                cmdCheckBookingSeats.Parameters.AddWithValue("@BookingID", BookingID);
                int bookedSeats = Convert.ToInt32(cmdCheckBookingSeats.ExecuteScalar());


                // Step 2: Get the available seats from the Showtimes table
                string queryCheckShowtimeSeats = "SELECT AvailableSeats FROM Showtimes WHERE ShowID = @ShowID";
                SqlCommand cmdCheckShowtimeSeats = new SqlCommand(queryCheckShowtimeSeats, conn);
                cmdCheckShowtimeSeats.Parameters.AddWithValue("@ShowID", ShowID);
                int availableSeats = Convert.ToInt32(cmdCheckShowtimeSeats.ExecuteScalar());

                string queryCheckAmount = "SELECT TicketPrice FROM Showtimes WHERE ShowID = @ShowID";
                SqlCommand cmdQueryCheckAmount = new SqlCommand(queryCheckAmount, conn);
                cmdQueryCheckAmount.Parameters.AddWithValue("@ShowID", ShowID);
                int Amount = Convert.ToInt32(cmdQueryCheckAmount.ExecuteScalar());

                litMessage.Text = "<p>Debug: Seats to cancel: " + seatsToCancel + ", Booked seats: " + bookedSeats + bookingID + "</p>";

                if (seatsToCancel > bookedSeats)
                {
                    // Display an error message to the user
                    litMessage.Text += "<p style='color:red;'>Error: You cannot cancel more seats than you have booked! You booked " + bookedSeats + " seat(s).</p>";
                    return;
                }
                else
                {
                    litMessage.Text += "<p style='color:green;'>Cancellation is valid. Proceeding...</p>";
                }
                // Step 3: Update the bookings table by reducing the number of seats
                string queryUpdateBooking = "UPDATE Bookings SET seats = seats - @SeatsToCancel WHERE BookingID = @BookingID";
                SqlCommand cmdUpdateBooking = new SqlCommand(queryUpdateBooking, conn);
                cmdUpdateBooking.Parameters.AddWithValue("@SeatsToCancel", seatsToCancel);
                cmdUpdateBooking.Parameters.AddWithValue("@BookingID", BookingID);
                cmdUpdateBooking.ExecuteNonQuery();

                // Step 4: Update the showtimes table by increasing the available seats
                string queryUpdateShowtime = "UPDATE Showtimes SET AvailableSeats = AvailableSeats + @SeatsToCancel WHERE ShowID = @ShowID";
                SqlCommand cmdUpdateShowtime = new SqlCommand(queryUpdateShowtime, conn);
                cmdUpdateShowtime.Parameters.AddWithValue("@SeatsToCancel", seatsToCancel);
                cmdUpdateShowtime.Parameters.AddWithValue("@ShowID", ShowID);
                cmdUpdateShowtime.ExecuteNonQuery();

                string queryUpdateAmount= "UPDATE Bookings SET TotalAmount = TotalAmount - @Amount WHERE ShowID = @ShowID";
                SqlCommand cmdUpdateAmount = new SqlCommand(queryUpdateAmount, conn);
                cmdUpdateAmount.Parameters.AddWithValue("@Amount", seatsToCancel*Amount);
                cmdUpdateAmount.Parameters.AddWithValue("@ShowID", ShowID);
                cmdUpdateAmount.ExecuteNonQuery();

                // Display success message and disable further actions
                litMessage.Text = "<p style='color:green;'>Cancellation successful. You have cancelled " + seatsToCancel + " seat(s).</p>" +
                    "<p> Remaining seats: " + (bookedSeats - seatsToCancel) + "  </p>" +
                    "<p> Total Amount to be paid: "+ ((Amount*bookedSeats) - (Amount*seatsToCancel)) + "</P>";
                btnCancel.Enabled = false;
                txtCancel.Enabled = false;
            }
        }
    }
}
