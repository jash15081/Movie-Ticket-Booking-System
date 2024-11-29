using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Admin
{
    public partial class Profiles : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProfilesGrid();
            }
        }

        private void BindProfilesGrid()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT ProfileID, FirstName, LastName, Email, MobileNumber, Address, DateOfBirth, Gender FROM Profiles";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvProfiles.DataSource = dt;
                gvProfiles.DataBind();
            }
        }

        protected void gvProfiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                int profileID = Convert.ToInt32(e.CommandArgument);
                // Implement logic to view profile details, e.g., redirect to a profile details page
                // For example:
                // Session["ProfileID"] = profileID;
                // Response.Redirect("ProfileDetails.aspx");
            }
        }
    }
}
