using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace MovieTicketBooking.Admin
{
    public partial class Movie : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketBooking"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (Session["AdminName"] == null)
                {
                    Response.Redirect("../Front.aspx");
                }
                BindMovies();
            }
        }

        private void BindMovies()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT MovieID, Title, Description, ReleaseDate, Duration, Genre, Language FROM Movies";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvMovies.DataSource = dt;
                    gvMovies.DataBind();

                    lblNoData.Visible = dt.Rows.Count == 0;
                }
            }
        }

        protected void btnAddMovie_Click(object sender, EventArgs e)
        {
            try
            {
                string title = txtTitle.Text.Trim();
                string description = txtDescription.Text.Trim();
                DateTime releaseDate = DateTime.Parse(txtReleaseDate.Value.Trim());
                int duration = int.Parse(txtDuration.Text.Trim());
                string genre = txtGenre.Text.Trim();
                string language = txtLanguage.Text.Trim();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Movies (Title, Description, ReleaseDate, Duration, Genre, Language) VALUES (@Title, @Description, @ReleaseDate, @Duration, @Genre, @Language)";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ReleaseDate", releaseDate);
                        cmd.Parameters.AddWithValue("@Duration", duration);
                        cmd.Parameters.AddWithValue("@Genre", genre);
                        cmd.Parameters.AddWithValue("@Language", language);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                // Rebind the movie list and hide the form
                BindMovies();
                
            }
            catch (Exception ex)
            {
                lblNoData.Text = $"Error: {ex.Message}";
                lblNoData.Visible = true;
            }
        }


        protected void gvMovies_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMovies.EditIndex = e.NewEditIndex;
            BindMovies();
        }

        protected void gvMovies_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMovies.EditIndex = -1;
            BindMovies();
        }

        protected void gvMovies_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int movieID = Convert.ToInt32(gvMovies.DataKeys[e.RowIndex].Value);

                GridViewRow row = gvMovies.Rows[e.RowIndex];

                string title = ((TextBox)row.FindControl("txtEditTitle")).Text.Trim();
                string description = ((TextBox)row.FindControl("txtEditDescription")).Text.Trim();
                DateTime releaseDate = DateTime.Parse(((TextBox)row.FindControl("txtEditReleaseDate")).Text.Trim());
                int duration = int.Parse(((TextBox)row.FindControl("txtEditDuration")).Text.Trim());
                string genre = ((TextBox)row.FindControl("txtEditGenre")).Text.Trim();
                string language = ((TextBox)row.FindControl("txtEditLanguage")).Text.Trim();

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Movies SET Title = @Title, Description = @Description, ReleaseDate = @ReleaseDate, Duration = @Duration, Genre = @Genre, Language = @Language WHERE MovieID = @MovieID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Title", title);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@ReleaseDate", releaseDate);
                        cmd.Parameters.AddWithValue("@Duration", duration);
                        cmd.Parameters.AddWithValue("@Genre", genre);
                        cmd.Parameters.AddWithValue("@Language", language);
                        cmd.Parameters.AddWithValue("@MovieID", movieID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                gvMovies.EditIndex = -1;
                BindMovies();
            }
            catch (Exception ex)
            {
                lblNoData.Text = $"Error: {ex.Message}";
                lblNoData.Visible = true;
            }
        }

        protected void gvMovies_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int movieID = Convert.ToInt32(gvMovies.DataKeys[e.RowIndex].Value);

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();

                    try
                    {
                        // Delete related bookings
                        string deleteBookingsQuery = "DELETE FROM Bookings WHERE ShowID IN (SELECT ShowID FROM Showtimes WHERE MovieID = @MovieID)";
                        using (SqlCommand cmd = new SqlCommand(deleteBookingsQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MovieID", movieID);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete related showtimes
                        string deleteShowtimesQuery = "DELETE FROM Showtimes WHERE MovieID = @MovieID";
                        using (SqlCommand cmd = new SqlCommand(deleteShowtimesQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MovieID", movieID);
                            cmd.ExecuteNonQuery();
                        }

                        // Delete the movie
                        string deleteMovieQuery = "DELETE FROM Movies WHERE MovieID = @MovieID";
                        using (SqlCommand cmd = new SqlCommand(deleteMovieQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@MovieID", movieID);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        lblNoData.Text = $"Error: {ex.Message}";
                        lblNoData.Visible = true;
                    }
                }

                BindMovies();
            }
            catch (Exception ex)
            {
                lblNoData.Text = $"Error: {ex.Message}";
                lblNoData.Visible = true;
            }
        }



        protected void BackHome_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminHome.aspx");
        }

    }


}
