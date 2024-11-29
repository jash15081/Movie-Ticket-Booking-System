using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Customer
{
    public partial class Home : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] != null)
            {
                litUserName.Text = Session["UserName"].ToString();
                if (!IsPostBack)
                {
                    BindMovies();
                    BindBookingDetails();
                }
            }
            else
            {
                Response.Redirect("Login.aspx"); // Redirect to login if session is not available
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("../Front.aspx");
        }

        private void BindMovies()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT MovieID, Title, Genre, Language, Description, ReleaseDate, Duration FROM Movies";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptMovies.DataSource = dt;
                        rptMovies.DataBind();
                    }
                    else
                    {
                        rptMovies.DataSource = null;
                        rptMovies.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (replace Console.WriteLine with proper logging)
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private void BindBookingDetails()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT b.BookingID, m.Title AS MovieName, b.Seats AS TotalSeats, b.TotalAmount AS Amount, t.Name AS TheaterName
                        FROM Bookings b
                        INNER JOIN Showtimes s ON b.ShowID = s.ShowID
                        INNER JOIN Screens sc ON s.ScreenID = sc.ScreenID
                        INNER JOIN Theaters t ON sc.TheaterID = t.TheaterID
                        INNER JOIN Movies m ON s.MovieID = m.MovieID
                        WHERE b.ProfileID = @ProfileID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProfileID", Session["ProfileID"]);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptBookingDetails.DataSource = dt;
                        rptBookingDetails.DataBind();
                    }
                    else
                    {
                        rptBookingDetails.DataSource = null;
                        rptBookingDetails.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error (replace Console.WriteLine with proper logging)
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        protected void rptMovies_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Book")
            {
                int movieID = Convert.ToInt32(e.CommandArgument);
                int profileID = Convert.ToInt32(Session["ProfileID"]);

                // Store MovieID and ProfileID in session
                Session["SelectedMovieID"] = movieID;

                // Redirect to Booking.aspx
                Response.Redirect("Booking.aspx");
            }
        }

        protected void rptBookingDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewDetails")
            {
                int bookingID = Convert.ToInt32(e.CommandArgument);

                // Store BookingID in session
                Session["SelectedBookingID"] = bookingID;

                // Redirect to BookingDetails.aspx
                Response.Redirect("BookingDetails.aspx");
            }
        }
    }
}
