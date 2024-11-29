<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bookings.aspx.cs" Inherits="MovieTicketBooking.Admin.Bookings" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bookings</title>
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
            <h1>Bookings List</h1>
            <asp:GridView ID="gvBookings" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowCommand="gvBookings_RowCommand">
                <Columns>
                    <asp:BoundField DataField="BookingID" HeaderText="Booking ID" />
                    <asp:BoundField DataField="ProfileID" HeaderText="Profile ID" />
                    <asp:BoundField DataField="ShowID" HeaderText="Show ID" />
                    <asp:BoundField DataField="TotalAmount" HeaderText="Total Amount" DataFormatString="${0:F2}" />
                    <asp:BoundField DataField="BookingDate" HeaderText="Booking Date" DataFormatString="{0:MMMM dd, yyyy HH:mm}" />
                    <asp:BoundField DataField="PaymentStatus" HeaderText="Payment Status" />
                    <asp:BoundField DataField="Seats" HeaderText="Seats" />
                    <asp:BoundField DataField="CancellationStatus" HeaderText="Cancellation Status" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnView" runat="server" Text="View" CommandName="View" CommandArgument='<%# Eval("BookingID") %>' CssClass="btn btn-info" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
