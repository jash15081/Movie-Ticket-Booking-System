using System;

namespace MovieTicketBooking
{
    public partial class Front : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // No specific logic for Page_Load needed in this case
        }

        protected void btnCustomer_Click(object sender, EventArgs e)
        {
            // Redirect to Login.aspx with a query string indicating the customer role
            Response.Redirect("/Customer/Login.aspx?role=customer");
        }

        protected void btnAdmin_Click(object sender, EventArgs e)
        {
            // Redirect to Login.aspx with a query string indicating the admin role
            Response.Redirect("/Customer/Login.aspx?role=admin");
        }
    }
}
