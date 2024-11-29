<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profiles.aspx.cs" Inherits="MovieTicketBooking.Admin.Profiles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Profiles</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            padding: 50px;
            text-align: center;
        }
        .container {
            background-color: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            max-width: 1200px;
            margin: 0 auto;
        }
        .gridview-header {
            background-color: #007bff;
            color: white;
            font-weight: bold;
        }
        .gridview-row {
            background-color: #fff;
            color: #333;
        }
        .gridview-row:nth-child(odd) {
            background-color: #f9f9f9;
        }
        .gridview-footer {
            background-color: #e9ecef;
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>Profiles List</h1>
            <asp:GridView ID="gvProfiles" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowCommand="gvProfiles_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ProfileID" HeaderText="Profile ID" />
                    <asp:BoundField DataField="FirstName" HeaderText="First Name" />
                    <asp:BoundField DataField="LastName" HeaderText="Last Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
                    <asp:BoundField DataField="Address" HeaderText="Address" />
                    <asp:BoundField DataField="DateOfBirth" HeaderText="Date of Birth" DataFormatString="{0:MMMM dd, yyyy}" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" />
                   
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
