using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Admin
{
    public partial class Bookings : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindBookingsGrid();
            }
        }

        private void BindBookingsGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT BookingID, ProfileID, ShowID, TotalAmount, BookingDate, PaymentStatus, Seats, CancellationStatus FROM Bookings";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvBookings.DataSource = dt;
                gvBookings.DataBind();
            }
        }

        protected void gvBookings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                int bookingID = Convert.ToInt32(e.CommandArgument);
                // Implement logic to view booking details, e.g., redirect to a booking details page
                // For example:
                Session["BookingID"] = bookingID;
                Response.Redirect("/Customer/BookingDetails.aspx?Adm=1");
            }
        }
    }
}
