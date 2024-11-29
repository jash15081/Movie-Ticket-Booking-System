<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MovieTicketBooking.Customer.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
     <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            text-align: center;
            padding: 50px;
            margin: 0;
        }
        .login-container {
            background-color: #fff;
            padding: 30px;
            border-radius: 8px;
            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);
            display: inline-block;
            max-width: 400px;
            width: 100%;
            box-sizing: border-box;
        }
        .login-container h2 {
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
        }
        .form-group input {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }
        .form-group input[type="submit"] {
            background-color: #007bff;
            color: #fff;
            border: none;
            cursor: pointer;
            padding: 10px;
        }
        .form-group input[type="submit"]:hover {
            background-color: #0056b3;
        }
        .form-group .error-message {
            color: red;
            font-size: 12px;
            margin-top: 5px;
        }
        .redirect-message {
            margin-top: 20px;
            font-size: 14px;

        }
        .redirect-message a {
            color: #007bff;
            text-decoration: none;
        }
        .redirect-message a:hover {
            text-decoration: underline;
        }
        .customer {
            display: none;
            text-decoration: none;
        }
        .role-label {
            font-size: 28px; /* Big font size */
            font-weight: bold; /* Bold text */
            color: #333; /* Optional: customize the color */
            margin-bottom: 20px; /* Add some space below the label */
        }
         .btn-link {
            display: inline-block;
            padding: 10px 20px;
            font-size: 15px;
            color: #fff;
            background-color: #007bff;
            border: none;
            border-radius: 4px;
            text-decoration: none;
            text-align: center;
            cursor: pointer;
        }
        .btn-link:hover {
            background-color: #0056b3;
        }
       
    </style>
    <script type="text/javascript">
        window.onload = function () {
            showRoleSection();
        };

        function showRoleSection() {
            var role = '<%= Request.QueryString["role"] %>'; // Embed server-side value into the JavaScript variable
            if (role === 'customer') {
                document.getElementById('customer').style.display = 'block';
            } else {
                document.getElementById('customer').style.display = 'none';
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <p><a href="../Front.aspx" class="btn-link">Back to Index</a></p>

           <div class="form-group">
                <asp:Label ID="lblRole" runat="server" CssClass="role-label"></asp:Label>
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

            <!-- Login Button -->
            <div class="form-group">
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="form-control" OnClick="btnLogin_Click" />
            </div>

            <!-- Message Label -->
            <div class="form-group">
                <asp:Label ID="lblMessage" runat="server" CssClass="error-message"></asp:Label>
            </div>

            <!-- Redirect to Sign Up -->
                <div class="customer" style="display:none;" id="customer">
                    <p>Don't have an account?<a href="SignUp.aspx">Sign Up</a></p>
                </div>
                  
            </div>
        
    </form>
</body>
</html>
