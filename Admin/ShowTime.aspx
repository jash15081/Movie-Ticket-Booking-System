<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowTime.aspx.cs" Inherits="MovieTicketBooking.Admin.ShowTime" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Showtimes</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
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
        .form-group input, .form-group select {
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
        .gvShowTimes {
            width: 100%;
            border-collapse: collapse;
        }
        .gvShowTimes th, .gvShowTimes td {
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
        }
        .gvShowTimes th {
            background-color: #f2f2f2;
            font-weight: bold;
        }
        .gvShowTimes tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .gvShowTimes tr:hover {
            background-color: #f1f1f1;
        }
        .gvShowTimes .edit-button, .gvShowTimes .delete-button {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 4px;
            cursor: pointer;
        }
        .gvShowTimes .edit-button:hover, .gvShowTimes .delete-button:hover {
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
           var addForm = document.getElementById("showtimeForm");
           var showTimeTable = document.getElementById("showTimeTable");
           var toggleButton = document.getElementById("btnToggleForm");
           var backButton = document.getElementById("btnBackToList");

           // Toggle visibility of the form and the GridView
           if (addForm.classList.contains("hidden")) {
               addForm.classList.remove("hidden");
               showTimeTable.classList.add("hidden");
               toggleButton.classList.add("hidden");
               backButton.classList.remove("hidden");
           }
           else {
               // Hide the form, show the movie list
               addForm.classList.add("hidden");
               movieTable.classList.remove("hidden");
               toggleButton.classList.remove("hidden");
               backButton.classList.add("hidden");
           }

           
       }

       //document.addEventListener("DOMContentLoaded", function () {
       //    // Initialize the form and back button states
       //    var addForm = document.getElementById("showtimeForm");
       //    var backButton = document.getElementById("btnBackToList");
       //    addForm.classList.add("hidden");
       //    backButton.classList.add("hidden");
       //});

   </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <asp:Button ID="backHome" runat="server" Text="Back To Home Page" OnClick="BackHome_Click"/>
            <h2>Manage Showtimes</h2>
             <asp:Button ID="btnToggleForm" runat="server" Text="Add New ShowTime" OnClientClick="toggleForm(); return false;" CssClass="btn" />
             <asp:Button ID="btnBackToList" runat="server" Text="Back to ShowTime List" OnClientClick="toggleForm(); return false;" CssClass="btn hidden" />

            <br /> <br />
            <div id="showtimeForm" runat="server" class="hidden">
                <div class="row mb-3">
                    <div class="col-md-6">
                        <!-- Movie Dropdown -->
                        <label for="ddlMovies">Select Movie</label>
                        <asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-control"  OnSelectedIndexChanged="ddlMovies_SelectedIndexChanged">
                            <asp:ListItem Text="Select Movie" Value="" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <!-- Theater Dropdown -->
                        <label for="ddlTheaters">Select Theater</label>
                        <asp:DropDownList ID="ddlTheaters" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTheaters_SelectedIndexChanged" Enabled="False">
                            <asp:ListItem Text="Select Theater" Value="" />
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-6">
                        <!-- Screen Dropdown -->
                        <label for="ddlScreens">Select Screen</label>
                        <asp:DropDownList ID="ddlScreens" runat="server" CssClass="form-control" Enabled="false">
                            <asp:ListItem Text="Select Screen" Value="" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <!-- Show Time Input -->
                        <label for="txtShowTime">Show Time (HH:MM)</label>
                        <asp:TextBox ID="txtShowTime" runat="server" CssClass="form-control" placeholder="Show Time (HH:MM)" TextMode="Time" />
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-md-6">
                        <!-- Show Date Input -->
                        <label for="txtShowDate">Show Date</label>
                        <asp:TextBox ID="txtShowDate" runat="server" CssClass="form-control" placeholder="YYYY-MM-DD" TextMode="Date" />
                    </div>
                    <div class="col-md-6">
                        <!-- Ticket Price Input -->
                        <label for="txtTicketPrice">Ticket Price</label>
                        <asp:TextBox ID="txtTicketPrice" runat="server" CssClass="form-control" placeholder="Enter Ticket Price" TextMode="Number" />
                    </div>
                </div>

                <!-- Submit Button -->
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnAddShowTime" runat="server" Text="Add Showtime" CssClass="btn btn-primary" OnClick="btnAddShowTime_Click" />
                    </div>
                </div>
            </div>

            <!-- GridView to display showtimes -->
            <div id="showTimeTable">
                <asp:GridView ID="gvShowTimes" runat="server" AutoGenerateColumns="False" CssClass="gvShowTimes" DataKeyNames="ShowID"
                    OnRowEditing="gvShowTimes_RowEditing" 
                    OnRowUpdating="gvShowTimes_RowUpdating" 
                    OnRowCancelingEdit="gvShowTimes_RowCancelingEdit" 
                    OnRowDeleting="gvShowTimes_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="ShowID" HeaderText="Show ID" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Movie Title">
                            <ItemTemplate>
                                <asp:Label ID="lblMovieTitle" runat="server" Text='<%# GetMovieTitle(Eval("MovieID")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Theater Name">
                            <ItemTemplate>
                                <asp:Label ID="lblTheaterName" runat="server" Text='<%# GetTheaterName(Eval("TheaterID")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Screen Name">
                            <ItemTemplate>
                                <asp:Label ID="lblScreenName" runat="server" Text='<%# GetScreenName(Eval("ScreenID")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Show Time">
                            <ItemTemplate>
                                <asp:Label ID="lblShowTime" runat="server" Text='<%# Eval("ShowTime") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtShowTimeEdit" runat="server" Text='<%# Bind("ShowTime") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Show Date">
                            <ItemTemplate>
                                <asp:Label ID="lblShowDate" runat="server" Text='<%# Eval("ShowDate", "{0:yyyy-MM-dd}") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtShowDateEdit" runat="server" Text='<%# Bind("ShowDate", "{0:yyyy-MM-dd}") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Available Seats">
                            <ItemTemplate>
                                <asp:Label ID="lblAvailableSeats" runat="server" Text='<%# Eval("AvailableSeats") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ticket Price">
                            <ItemTemplate>
                                <asp:Label ID="lblTicketPrice" runat="server" Text='<%# Eval("TicketPrice") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTicketPriceEdit" runat="server" Text='<%# Bind("TicketPrice") %>' CssClass="form-control" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </div>

            <asp:Label ID="lblError" runat="server" CssClass="text-danger" />
            <asp:Label ID="lblNoData" runat="server" CssClass="no-data" Text="No showtimes data available." Visible="false"></asp:Label>

        </div>
    </form>
</body>
</html>
