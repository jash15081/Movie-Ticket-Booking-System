<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="MovieTicketBooking.Customer.Home" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            text-align: center;
            padding: 50px;
        }
        .welcome-container {
            background-color: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            display: inline-block;
        }
        .welcome-container h1 {
            margin-bottom: 20px;
            color: #007bff;
        }
        .welcome-container p {
            font-size: 18px;
            color: #333;
        }
        .logout-button {
            margin-top: 20px;
            background-color: #dc3545;
            color: #fff;
            border: none;
            padding: 10px 20px;
            border-radius: 4px;
            cursor: pointer;
            text-decoration: none;
        }
        .logout-button:hover {
            background-color: #c82333;
        }
        .movie-card {
            display: inline-block;
            width: 300px;
            margin: 10px;
            padding: 10px;
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0px 0px 5px rgba(0, 0, 0, 0.1);
            text-align: left;
        }
        .movie-card button {
            display: block;
            width: 100%;
            margin-top: 10px;
            background-color: #007bff;
            color: #fff;
            border: none;
            padding: 10px;
            border-radius: 4px;
            cursor: pointer;
        }
        .movie-card button:hover {
            background-color: #0056b3;
        }
        .booking-table {
            margin-top: 30px;
            width: 100%;
            max-width: 1000px;
            margin-left: auto;
            margin-right: auto;
            border-collapse: collapse;
        }
        .booking-table th, .booking-table td {
            border: 1px solid #ddd;
            padding: 8px;
        }
        .booking-table th {
            background-color: #007bff;
            color: white;
        }
        .booking-table tr:nth-child(even) {
            background-color: #f2f2f2;
        }
        .booking-table tr:hover {
            background-color: #ddd;
        }
        .view-details-button {
            background-color: #007bff;
            color: #fff;
            border: none;
            padding: 8px 16px;
            border-radius: 4px;
            cursor: pointer;
            text-decoration: none;
        }
        .view-details-button:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="welcome-container">
            <h1>Welcome, <asp:Literal ID="litUserName" runat="server"></asp:Literal>!</h1>
            <p>Thank you for logging in. We hope you enjoy your time here.</p>
            <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="logout-button" OnClick="btnLogout_Click" />
        </div>
        <br /><br />
        <asp:Repeater ID="rptMovies" runat="server" OnItemCommand="rptMovies_ItemCommand">
            <ItemTemplate>
                <div class="movie-card">
                    <h2><%# Eval("Title") %></h2>
                    <hr />           
                    <p><strong>Genre:</strong> <%# Eval("Genre") %></p>
                    <p><strong>Language:</strong> <%# Eval("Language") %></p>
                    <p><strong>Description:</strong> <%# Eval("Description") ?? "Empty" %></p>
                    <p><strong>Release Date:</strong> <%# Eval("ReleaseDate", "{0:MMMM dd, yyyy}") %></p>
                    <p><strong>Duration:</strong> <%# Eval("Duration") %> minutes</p>
                    <asp:Button ID="btnBook" runat="server" Text="Book Now" CommandName="Book" CommandArgument='<%# Eval("MovieID") %>' />
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <br /><br />
        <div>
            <table class="booking-table">
                <thead>
                    <tr>
                        <th>Movie Name</th>
                        <th>Total Seats</th>
                        <th>Amount</th>
                        <th>Theater Name</th>
                        <th>Details</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptBookingDetails" runat="server" OnItemCommand="rptBookingDetails_ItemCommand">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("MovieName") %></td>
                                <td><%# Eval("TotalSeats") %></td>
                                <td><%# Eval("Amount", "{0:C}") %></td>
                                <td><%# Eval("TheaterName") %></td>
                                <td><asp:Button ID="btnViewDetails" runat="server" Text="View Details" CommandName="ViewDetails" CommandArgument='<%# Eval("BookingID") %>' CssClass="view-details-button" /></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
