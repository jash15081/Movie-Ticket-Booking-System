using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MovieTicketBooking.Admin
{
    public partial class AdminHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminName"] != null)
            {
                litUserName.Text = Session["AdminName"].ToString();
            }
            else
            {
                Response.Redirect("../Front.aspx"); // Redirect to login if session is not available
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
    }
}