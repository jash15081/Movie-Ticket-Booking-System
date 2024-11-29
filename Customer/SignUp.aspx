<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="MovieTicketBooking.Customer.SignUp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }
        .container {
            max-width: 400px;
            width: 100%;
            background-color: #fff;
            padding: 20px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            border-radius: 8px;
            position: relative; /* Ensure relative positioning for inner elements */
        }
        .container h2 {
            text-align: center;
            margin-bottom: 20px;
            color: #007bff;
            font-size: 24px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
            color: #333;
        }
        .form-group input, .form-group select {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }
        .form-group input[type="submit"] {
            background-color: #28a745;
            color: #fff;
            border: none;
            cursor: pointer;
            padding: 10px;
            font-size: 16px;
        }
        .form-group input[type="submit"]:hover {
            background-color: #218838;
        }
        .error-message {
            color: red;
            font-size: 12px;
            margin-top: 5px;
        }
        .success-message {
            color: green;
            font-size: 14px;
            text-align: center;
            margin-bottom: 20px;
        }
        .redirect-message {
            text-align: center;
            margin-top: 20px;
        }
        .redirect-message a {
            color: #007bff;
            text-decoration: none;
            font-weight: bold;
        }
        .redirect-message a:hover {
            text-decoration: underline;
        }
        /* Button for 'Back to Index' */
        .back-button {
            display: inline-block;
            padding: 10px 20px;
            font-size: 16px;
            color: #fff;
            background-color: #007bff;
            border: none;
            border-radius: 4px;
            text-decoration: none;
            text-align: center;
            cursor: pointer;
            position: absolute;
            top: 10px;
            left: 10px;
        }
        .back-button:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <a href="../Front.aspx" class="back-button">Back to Index</a>
            <h2>Sign Up</h2>

            <!-- First Name -->
            <div class="form-group">
                <label for="txtFirstName">First Name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="Enter your first name"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required" CssClass="error-message" />
            </div>

            <!-- Last Name -->
            <div class="form-group">
                <label for="txtLastName">Last Name</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Enter your last name"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name is required" CssClass="error-message" />
            </div>

            <!-- Email -->
            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter your email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="error-message" />
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ErrorMessage="Enter a valid email address" CssClass="error-message" />
            </div>

            <!-- Password -->
            <div class="form-group">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter your password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" CssClass="error-message" />
            </div>

            <!-- Confirm Password -->
            <div class="form-group">
                <label for="txtConfirmPassword">Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirm your password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Please confirm your password" CssClass="error-message" />
                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ErrorMessage="Passwords do not match" CssClass="error-message" />
            </div>

            <!-- Date of Birth -->
            <div class="form-group">
                <label for="txtDateOfBirth">Date of Birth</label>
                <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDateOfBirth" runat="server" ControlToValidate="txtDateOfBirth" ErrorMessage="Date of birth is required" CssClass="error-message" />
            </div>

            <!-- Mobile Number -->
            <div class="form-group">
                <label for="txtMobile">Mobile Number</label>
                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Enter your mobile number"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile" ValidationExpression="^\d{10}$" ErrorMessage="Enter a valid 10-digit mobile number" CssClass="error-message" />
            </div>

            <!-- Address -->
            <div class="form-group">
                <label for="txtAddress">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter your address"></asp:TextBox>
            </div>

            <!-- Gender -->
            <div class="form-group">
                <label for="ddlGender">Gender</label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                    <asp:ListItem Value="">Select Gender</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="O">Other</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvGender" runat="server" ControlToValidate="ddlGender" InitialValue="" ErrorMessage="Please select your gender" CssClass="error-message" />
            </div>

            <!-- Message Label -->
            <div class="form-group">
                <asp:Label ID="lblMessage" runat="server" CssClass="success-message" />
            </div>

            <!-- Submit Button -->
            <div class="form-group">
                <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" CssClass="form-control" OnClick="btnSignUp_Click" />
            </div>

            <!-- Redirect to Login -->
            <div class="redirect-message">
                <p>Already have an account? <a href="Login.aspx?role=customer">Login here</a>.</p>
            </div>
        </div>
    </form>
</body>
</html>
