<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Booking.aspx.cs" Inherits="MovieTicketBooking.Customer.Booking" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Booking</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }
        .booking-container {
            background-color: #fff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0px 0px 20px rgba(0, 0, 0, 0.15);
            width: 100%;
            max-width: 700px;

            
        }
        .booking-container h1 {
            margin-bottom: 20px;
            color: #007bff;
            text-align: center;
            font-size: 28px;
        }
        .booking-container .form-group {
            margin-bottom: 20px;
        }
        .booking-container label {
            display: block;
            margin-bottom: 10px;
            font-weight: bold;
            font-size: 16px;
        }
        .booking-container input[type="text"],
        .booking-container select {
            width: 100%;
            padding: 12px;
            font-size: 16px;
            border: 1px solid #ccc;
            border-radius: 6px;
        }
        .booking-container input[type="text"]::placeholder {
            color: #aaa;
        }
        .booking-container .form-group p {
            margin: 0;
            font-size: 16px;
            color: #333;
        }
        .booking-container .button-container {
            display: flex;
            justify-content: space-between;
            margin-top: 30px;
        }
        .booking-container .button-container button {
            background-color: #007bff;
            color: #fff;
            border: none;
            padding: 12px 25px;
            border-radius: 6px;
            cursor: pointer;
            font-size: 18px;
            transition: background-color 0.3s ease, box-shadow 0.3s ease;
        }
        .booking-container .button-container button:hover {
            background-color: #0056b3;
            box-shadow: 0px 6px 10px rgba(0, 0, 0, 0.2);
        }
        .booking-container .button-container button:disabled {
            background-color: #aaa;
            cursor: not-allowed;
        }
        .booking-container .message {
            color: #d9534f;
            font-size: 16px;
            text-align: center;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="booking-container">
            <h1>Book Your Tickets</h1>
            
            <asp:Literal ID="litMovieDetails" runat="server" />

            <div class="form-group">
                <label for="ddlTheater">Select Theater</label>
                <asp:DropDownList ID="ddlTheater" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTheater_SelectedIndexChanged">
                    <asp:ListItem Text="Select Theater" Value="" />
                </asp:DropDownList>
            </div>

           

            <div class="form-group">
                <label for="ddlShowtime">Select Showtime</label>
                <asp:DropDownList ID="ddlShowtime" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShowtime_SelectedIndexChanged">
                    <asp:ListItem Text="Select Showtime" Value="" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <asp:Literal ID="litAvailableSeats" runat="server" />
                <asp:Literal ID="litTicketPrice" runat="server" />
            </div>

            <div class="form-group">
                <label for="txtSeats">Number of Seats</label>
                <asp:TextBox ID="txtSeats" runat="server" placeholder="Enter number of seats"></asp:TextBox>
            </div>

            <div class="form-group">
                <asp:Literal ID="litMessage" runat="server" />
            </div>

            <div class="button-container">
                <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
                <asp:Button ID="btnBookNow" runat="server" Text="Book Now" OnClick="btnBookNow_Click" />
                <asp:Button ID="btnBookingDetails" runat="server" Text="Show Details" OnClick="btnBookingDetails_Click" Enabled="false" />
            </div>
        </div>
    </form>
</body>
</html>
