<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Movie.aspx.cs" Inherits="MovieTicketBooking.Admin.Movie" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Movies</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
        }
        .container {
            width: 90%;
            margin: 50px auto;
            padding: 20px;
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }
        h2 {
            text-align: center;
            margin-bottom: 20px;
            color: #333;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }
        .form-group input, .form-group select, .form-group textarea {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 16px;
        }
        .btn {
            padding: 10px 20px;
            background-color: #007bff;
            color: white;
            border: none;
            cursor: pointer;
            border-radius: 4px;
            font-size: 16px;
        }
        .btn:hover {
            background-color: #0056b3;
        }
        .hidden {
            display: none;
        }
        .gridview {
            width: 100%;
            border-collapse: collapse;
        }
        .gridview th, .gridview td {
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
        }
        .gridview th {
            background-color: #f2f2f2;
            font-weight: bold;
        }
        .gridview tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .gridview tr:hover {
            background-color: #f1f1f1;
        }
        .gridview .edit-button, .gridview .delete-button {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 4px;
            cursor: pointer;
        }
        .gridview .edit-button:hover, .gridview .delete-button:hover {
            background-color: #0056b3;
        }
        .no-data {
            text-align: center;
            color: #666;
            font-size: 18px;
        }
    </style>
    <script type="text/javascript">
        function toggleForm() {
            var addForm = document.getElementById("addMovieForm");
            var movieTable = document.getElementById("movieTable");
            var toggleButton = document.getElementById("btnToggleForm");
            var backButton = document.getElementById("btnBackToList");

            if (addForm.classList.contains("hidden")) {
                // Show the form, hide the movie list
                addForm.classList.remove("hidden");
                movieTable.classList.add("hidden");
                toggleButton.classList.add("hidden");
                backButton.classList.remove("hidden");

                // Clear the form fields when the form is shown
                document.getElementById("<%= txtTitle.ClientID %>").value = '';
                document.getElementById("<%= txtDescription.ClientID %>").value = '';
                document.getElementById("<%= txtReleaseDate.ClientID %>").value = '';
                document.getElementById("<%= txtDuration.ClientID %>").value = '';
                document.getElementById("<%= txtGenre.ClientID %>").value = '';
                document.getElementById("<%= txtLanguage.ClientID %>").value = '';
            } else {
                // Hide the form, show the movie list
                addForm.classList.add("hidden");
                movieTable.classList.remove("hidden");
                toggleButton.classList.remove("hidden");
                backButton.classList.add("hidden");
            }
        }
        //document.addEventListener("DOMContentLoaded", function () {
        //    toggleForm(false); // Ensure the form is hidden initially
        //});
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <asp:Button ID="backHome" runat="server" Text="Back To Home Page" OnClick="BackHome_Click"/>
            <h2>Movie Management</h2>

            <!-- Button to toggle between add movie form and movie list -->
            <asp:Button ID="btnToggleForm" runat="server" Text="Add New Movie" OnClientClick="toggleForm(); return false;" CssClass="btn" />
            <asp:Button ID="btnBackToList" runat="server" Text="Back to Movie List" OnClientClick="toggleForm(); return false;" CssClass="btn hidden" />

            <br /><br />
            <!-- GridView for Movies (initially visible) -->
            <div id="movieTable">
                <asp:GridView ID="gvMovies" runat="server" AutoGenerateColumns="False" DataKeyNames="MovieID" CssClass="gridview"
                    OnRowEditing="gvMovies_RowEditing" OnRowDeleting="gvMovies_RowDeleting" 
                    OnRowUpdating="gvMovies_RowUpdating" OnRowCancelingEdit="gvMovies_RowCancelingEdit" >
                    <Columns>
                        <asp:BoundField DataField="MovieID" HeaderText="Movie ID" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Title">
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditTitle" runat="server" Text='<%# Bind("Title") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditDescription" runat="server" Text='<%# Bind("Description") %>' TextMode="MultiLine" Rows="3" CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Release Date">
                            <ItemTemplate>
                                <asp:Label ID="lblReleaseDate" runat="server" Text='<%# Eval("ReleaseDate", "{0:yyyy-MM-dd}") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditReleaseDate" runat="server" Text='<%# Bind("ReleaseDate", "{0:yyyy-MM-dd}") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Duration">
                            <ItemTemplate>
                                <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("Duration") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditDuration" runat="server" Text='<%# Bind("Duration") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Genre">
                            <ItemTemplate>
                                <asp:Label ID="lblGenre" runat="server" Text='<%# Eval("Genre") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditGenre" runat="server" Text='<%# Bind("Genre") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Language">
                            <ItemTemplate>
                                <asp:Label ID="lblLanguage" runat="server" Text='<%# Eval("Language") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditLanguage" runat="server" Text='<%# Bind("Language") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>

                <!-- Message when no data is present -->
                <asp:Label ID="lblNoData" runat="server" CssClass="no-data" Text="No movies data available." Visible="false"></asp:Label>
            </div>
               
            <!-- Add Movie Section (initially hidden) -->
            <!-- Add Movie Section (initially hidden) -->
<div id="addMovieForm" class="hidden">
    <div class="form-group">
        <label for="txtTitle">Title:</label>
        <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <label for="txtDescription">Description:</label>
        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
    </div>

    <div class="form-group">
        <label for="txtReleaseDate">Release Date:</label>
        <input type="date" id="txtReleaseDate" runat="server" class="form-control" />
    </div>

    <div class="form-group">
        <label for="txtDuration">Duration (in minutes):</label>
        <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <label for="txtGenre">Genre:</label>
        <asp:TextBox ID="txtGenre" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <label for="txtLanguage">Language:</label>
        <asp:TextBox ID="txtLanguage" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Button ID="btnAddMovie" runat="server" Text="Add Movie" OnClick="btnAddMovie_Click" CssClass="btn btn-primary" />
    </div>
</div>

        </div>
    </form>
</body>
</html>
