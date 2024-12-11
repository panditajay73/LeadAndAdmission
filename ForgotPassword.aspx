<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ForgotPassword.aspx.vb" Inherits="ForgotPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Forgot Password</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            color: #333;
            margin: 0;
            padding: 0;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
        }
        .container {
            background-color: #ffffff;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            max-width: 400px;
            width: 100%;
        }
        h2 {
            color: black; /* Teal color */
            margin-bottom: 20px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-control {
            width: 100%;
            padding: 10px;
            border-radius: 4px;
            border: 1px solid #ccc;
            box-sizing: border-box;
        }
        .btn {
            background-color: #1ed085; /* Teal color */
            color: #fff;
            border: none;
            padding: 10px 15px;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }
        .btn:hover {
            background-color: #1ed085; /* Darker teal */
        }
        .text-danger {
            color: #e74c3c;
            font-size: 14px;
        }
        .hidden {
            display: none;
        }
        .visible {
            display: block;
        }
    </style>
    <script>
        function showOtpSection() {
            var email = document.getElementById('<%= txtEmail.ClientID %>').value;
            if (email === '') {
                document.getElementById('<%= lblMessage.ClientID %>').innerText = "Please enter your email.";
                return;
            }
            document.getElementById('<%= btnSendOtp.ClientID %>').click();
        }

        function verifyOtp() {
            var otp = document.getElementById('<%= txtOtpCode.ClientID %>').value;
            if (otp === '') {
                document.getElementById('<%= lblOtpMessage.ClientID %>').innerText = "Please enter the OTP.";
                return;
            }
            document.getElementById('<%= btnVerifyOtp.ClientID %>').click();
        }

        function resetPassword() {
            var newPassword = document.getElementById('<%= txtNewPassword.ClientID %>').value;
            var confirmNewPassword = document.getElementById('<%= txtConfirmNewPassword.ClientID %>').value;
            if (newPassword === '' || confirmNewPassword === '') {
                document.getElementById('<%= lblPasswordMessage.ClientID %>').innerText = "Please fill in both password fields.";
                return;
            }
            if (newPassword !== confirmNewPassword) {
                document.getElementById('<%= lblPasswordMessage.ClientID %>').innerText = "Passwords do not match.";
                return;
            }
            document.getElementById('<%= btnResetPassword.ClientID %>').click();
        }
    </script>
</head>
<body>
    <div class="container">
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

            <!-- Forgot Password Section -->
            <div id="forgotPasswordSection" class="visible" runat="server">
                <h2>Forgot Password</h2>
                <div class="form-group">
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter your email" TextMode="Email" />
                </div>
                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                <button type="button" class="btn" onclick="showOtpSection()">Send OTP</button>
            </div>

            <!-- OTP Verification Section -->
            <div id="otpSection" class="hidden" runat="server">
                <h2>OTP Verification</h2>
                <div class="form-group">
                    <asp:TextBox ID="txtOtpCode" runat="server" CssClass="form-control" placeholder="Enter OTP" />
                </div>
                <asp:Label ID="lblOtpMessage" runat="server" CssClass="text-danger"></asp:Label>
                <button type="button" class="btn" onclick="verifyOtp()">Verify OTP</button>
            </div>

            <!-- Reset Password Section -->
            <div id="resetPasswordSection" class="hidden" runat="server">
                <h2>Reset Password</h2>
                <div class="form-group">
                    <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="New Password" />
                </div>
                <div class="form-group">
                    <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Confirm New Password" />
                </div>
                <asp:Label ID="lblPasswordMessage" runat="server" CssClass="text-danger"></asp:Label>
                <button type="button" class="btn" onclick="resetPassword()">Reset Password</button>
            </div>

            <!-- Hidden buttons to trigger server-side events -->
            <asp:Button ID="btnSendOtp" runat="server" Text="Send OTP" OnClick="btnSendOtp_Click" style="display:none;" />
            <asp:Button ID="btnVerifyOtp" runat="server" Text="Verify OTP" OnClick="btnVerifyOtp_Click" style="display:none;" />
            <asp:Button ID="btnResetPassword" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click" style="display:none;" />
        </form>
    </div>
</body>
</html>
