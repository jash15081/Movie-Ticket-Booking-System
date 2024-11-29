<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Theater.aspx.cs" Inherits="MovieTicketBooking.Admin.Theater" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Theaters</title>
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
            var addForm = document.getElementById("addTheaterForm");
            var theaterTable = document.getElementById("theaterTable");
            var toggleButton = document.getElementById("btnToggleForm");
            var backButton = document.getElementById("btnBackToList");

            if (addForm.classList.contains("hidden")) {
                addForm.classList.remove("hidden");
                theaterTable.classList.add("hidden");
                toggleButton.classList.add("hidden");
                backButton.classList.remove("hidden");
            } else {
                addForm.classList.add("hidden");
                theaterTable.classList.remove("hidden");
                toggleButton.classList.remove("hidden");
                backButton.classList.add("hidden");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <asp:Button ID="backHome" runat="server" Text="Back To Home Page" OnClick="BackHome_Click"/>
            <h2>Theater Management</h2>

            <!-- Button to toggle between add theater form and theater list -->
            <asp:Button ID="btnToggleForm" runat="server" Text="Add New Theater" OnClientClick="toggleForm(); return false;" CssClass="btn" />
            <asp:Button ID="btnBackToList" runat="server" Text="Back to Theater List" OnClientClick="toggleForm(); return false;" CssClass="btn hidden" />

            <br /><br />
            <!-- GridView for Theaters (initially visible) -->
            <div id="theaterTable">
                <asp:GridView ID="gvTheaters" runat="server" AutoGenerateColumns="False" DataKeyNames="TheaterID" CssClass="gridview"
                    OnRowEditing="gvTheaters_RowEditing" OnRowDeleting="gvTheaters_RowDeleting" 
                    OnRowUpdating="gvTheaters_RowUpdating" OnRowCancelingEdit="gvTheaters_RowCancelingEdit">
                    <Columns>
                        <asp:BoundField DataField="TheaterID" HeaderText="Theater ID" ReadOnly="True" />
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditName" runat="server" Text='<%# Bind("Name") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditAddress" runat="server" Text='<%# Bind("Address") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City">
                            <ItemTemplate>
                                <asp:Label ID="lblCity" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditCity" runat="server" Text='<%# Bind("City") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contact Number">
                            <ItemTemplate>
                                <asp:Label ID="lblContactNumber" runat="server" Text='<%# Eval("ContactNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditContactNumber" runat="server" Text='<%# Bind("ContactNumber") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Screens">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalScreens" runat="server" Text='<%# Eval("TotalScreens") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtEditTotalScreens" runat="server" Text='<%# Bind("TotalScreens") %>' CssClass="form-control"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        
                        
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnManageScreens" runat="server" Text="Manage Screens" 
                                    CommandArgument='<%# Bind("TheaterID") %>' 
                                    OnClick="btnManageScreens_Click" CssClass="btn" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>

                <!-- Message when no data is present -->
                <asp:Label ID="lblNoData" runat="server" CssClass="no-data" Text="No theaters data available." Visible="false"></asp:Label>
            </div>

            <!-- Add Theater Section (initially hidden) -->
            <div id="addTheaterForm" class="hidden">
                <div class="form-group">
                    <label for="txtName">Name:</label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtAddress">Address:</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtCity">City:</label>
                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label for="txtContactNumber">Contact Number:</label>
                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control"></asp:TextBox>
                </div>

                
                <div class="form-group">
                    <asp:Button ID="btnAddTheater" runat="server" Text="Add Theater" CssClass="btn" OnClick="btnAddTheater_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
