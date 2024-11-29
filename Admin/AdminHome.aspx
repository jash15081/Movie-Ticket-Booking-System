<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="MovieTicketBooking.Admin.AdminHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Page</title>
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
        .admin-buttons {
            margin-top: 40px;
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 20px;
            justify-content: center;
        }
        .admin-button {
            background-color: #007bff;
            color: #fff;
            border: none;
            padding: 15px 20px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 20px;
            font-weight:bolder;
            text-decoration: none;
            display: inline-block;
        }
        .admin-button:hover {
            background-color: #0056b3;
        }
        .admin-button span {
            display: block;
            font-size: 14px;
            margin-top: 10px;
            color: #ddd;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="welcome-container">
            <h2>Admin Page</h2>
            <h1>Welcome, <asp:Literal ID="litUserName" runat="server"></asp:Literal>!</h1>
            <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="logout-button" OnClick="btnLogout_Click" />

            <div class="admin-buttons">
                <!-- Profiles Button -->
                <a href="/Admin/Profiles.aspx" class="admin-button" >
                    Manage User Profiles
                    <span>View and manage all user profiles</span>
                </a>
                
                <!-- Movies Button -->
                <a href="/Admin/Movie.aspx" class="admin-button">
                    Manage Movies
                    <span>Add, update, and remove movies</span>
                </a>

                <!-- Bookings Button -->
                <a href="/Admin/Bookings.aspx" class="admin-button">
                    Manage Bookings
                    <span>View all bookings and details</span>
                </a>

                <!-- Showtimes Button -->
                <a href="/Admin/ShowTime.aspx" class="admin-button">
                    Manage Showtimes
                    <span>Set and update showtimes for theaters</span>
                </a>

                <!-- Theaters Button -->
                <a href="/Admin/Theater.aspx" class="admin-button">
                    Manage Theaters
                    <span>View and manage theater details</span>
                </a>
            </div>
        </div>
    </form>
</body>
</html>
