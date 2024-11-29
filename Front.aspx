<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Front.aspx.cs" Inherits="MovieTicketBooking.Front" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Role</title>
    <style>
        .container {
            text-align: center;
            margin-top: 100px;
        }
        .role-btn {
            padding: 10px 20px;
            margin: 20px;
            font-size: 16px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Select Your Role</h2>
            <asp:Button ID="btnCustomer" runat="server" CssClass="role-btn" Text="Customer" OnClick="btnCustomer_Click" />
            <asp:Button ID="btnAdmin" runat="server" CssClass="role-btn" Text="Admin" OnClick="btnAdmin_Click" />
        </div>
    </form>
</body>
</html>
