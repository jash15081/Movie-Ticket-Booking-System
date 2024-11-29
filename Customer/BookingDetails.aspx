<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingDetails.aspx.cs" Inherits="MovieTicketBooking.Customer.BookingDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Booking Details</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f0f0f0;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            margin: 0;
        }

        .booking-details-container {
            background-color: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0px 4px 12px rgba(0, 0, 0, 0.1);
            max-width: 600px;
            width: 100%;
            box-sizing: border-box;
        }

        h2 {
            text-align: center;
            color: #007bff;
            margin-bottom: 20px;
        }

        .details-section {
            margin-bottom: 15px;
        }

        .details-section strong {
            color: #333;
            font-size: 16px;
        }

        .details-section p {
            margin: 5px 0;
            font-size: 14px;
            color: #555;
        }

        .form-group {
            margin-top: 20px;
        }

        input[type="text"] {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .button-container {
            margin-top: 20px;
            display: flex;
            justify-content: space-between;
        }

        button {
            background-color: #007bff;
            color: #fff;
            border: none;
            padding: 10px 20px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.3s ease, box-shadow 0.3s ease;
        }

        button:hover {
            background-color: #0056b3;
            box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.2);
        }

        button:disabled {
            background-color: #aaa;
            cursor: not-allowed;
        }

        .message {
            margin-top: 20px;
            text-align: center;
            font-size: 14px;
        }

        .message p {
            color: #d9534f;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="booking-details-container">
            <asp:Button ID="btnBack" OnClick="btnBack_Click" runat="server" Text="Back" />
            <asp:Literal ID="litBookingDetails" runat="server"></asp:Literal>

            <div class="form-group">
                <asp:TextBox ID="txtCancel" runat="server" Enabled="false" placeholder="Enter number of seats to cancel"></asp:TextBox>
            </div>

            <div class="button-container">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel Tickets" OnClick="btnCancel_Click" Enabled="false" />
            </div>

            <asp:Literal ID="litMessage" runat="server" ></asp:Literal>
        </div>
    </form>
</body>
</html>
