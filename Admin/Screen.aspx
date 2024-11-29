<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Screen.aspx.cs" Inherits="MovieTicketBooking.Admin.Screen" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Screens</title>
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
            var addForm = document.getElementById("addScreenForm");
            var screenTable = document.getElementById("screenTable");
            var toggleButton = document.getElementById("btnToggleForm");
            var backButton = document.getElementById("btnBackToList");

            if (addForm.classList.contains("hidden")) {
                addForm.classList.remove("hidden");
                screenTable.classList.add("hidden");
                toggleButton.classList.add("hidden");
                backButton.classList.remove("hidden");
            } else {
                addForm.classList.add("hidden");
                screenTable.classList.remove("hidden");
                toggleButton.classList.remove("hidden");
                backButton.classList.add("hidden");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Manage Screens</h2>

            <!-- Button to toggle between add screen form and screen list -->
            <p><a href="/Admin/Theater.aspx">Back to Theater List</a></p>
            <asp:Button ID="btnToggleForm" runat="server" Text="Add New Screen" OnClientClick="toggleForm(); return false;" CssClass="btn" />
            <asp:Button ID="btnBackToList" runat="server" Text="Back to Screen List" OnClientClick="toggleForm(); return false;" CssClass="btn hidden" />

            <br /><br />
            <!-- GridView for Screens (initially visible) -->
            <div id="screenTable">
                <asp:GridView ID="gvScreens" runat="server" AutoGenerateColumns="False" DataKeyNames="ScreenID, TheaterID" CssClass="gridview"
                    OnRowEditing="gvScreens_RowEditing" OnRowDeleting="gvScreens_RowDeleting" 
                    OnRowUpdating="gvScreens_RowUpdating" OnRowCancelingEdit="gvScreens_RowCancelingEdit">
                    <Columns>
                        <asp:BoundField DataField="ScreenID" HeaderText="Screen ID" ReadOnly="True" />
                        <asp:BoundField DataField="TheaterID" HeaderText="Theater ID" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("Name") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Capacity">
                            <ItemTemplate>
                                <asp:Label ID="lblCapacity" runat="server" Text='<%# Eval("Capacity") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditCapacity" runat="server" Text='<%# Bind("Capacity") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Remaining">
                            <ItemTemplate>
                                <asp:Label ID="lblRemaining" runat="server" Text='<%# Eval("Remaining") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditRemaining" runat="server" Text='<%# Bind("Remaining") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>

                <!-- Message when no data is present -->
                <asp:Label ID="lblNoData" runat="server" CssClass="no-data" Text="No screens data available." Visible="false"></asp:Label>
            </div>

            <!-- Add Screen Section (initially hidden) -->
            <div id="addScreenForm" class="hidden">
                <div class="form-group">
                    <label for="txtName">Name:</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtCapacity">Capacity:</label>
                    <asp:TextBox ID="txtCapacity" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtRemaining">Remaining:</label>
                    <asp:TextBox ID="txtRemaining" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:Button ID="btnAddScreen" runat="server" Text="Add Screen" OnClick="btnAddScreen_Click" CssClass="btn btn-primary" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
