<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login Here</title>
    <script src="https://kit.fontawesome.com/a429e5bfb6.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="Styles/LoginAndRegister.css">
    <script>
        function showCustomAlert(message) {
            var alertBox = document.getElementById('customAlert');
            var alertMessage = document.getElementById('customAlertMessage');
            var alertIcon = alertBox.querySelector('i'); // Assuming there's an <i> tag for the icon

            // Set the custom message
            alertMessage.innerHTML = message;

            // Check if message contains the word "success"
            if (message.toLowerCase().includes('success')) {
                alertBox.style.backgroundColor = '#28a745'; // Green background
                alertIcon.className = 'fa fa-check-circle'; // Update icon to right-tick
            } else {
                alertBox.style.backgroundColor = '#c00'; // Default red background
                alertIcon.className = 'fa fa-exclamation-triangle'; // Default warning icon
            }

            // Make the alert visible with transition
            alertBox.style.display = 'flex'; // Ensure display is flex to align items
            setTimeout(function () {
                alertBox.classList.add('show'); // Add class to slide it in
            }, 100); // Delay for smooth transition

            // Hide the alert after 3 seconds
            setTimeout(function () {
                alertBox.classList.remove('show'); // Slide it out
                setTimeout(function () {
                    alertBox.style.display = 'none'; // Hide the alert completely
                }, 500); // Wait for the slide-out animation to complete
            }, 3000); // Show for 3 seconds
        }

        // Function to manually close the alert
        function closeCustomAlert() {
            var alertBox = document.getElementById('customAlert');
            alertBox.classList.remove('show'); // Slide it out
            setTimeout(function () {
                alertBox.style.display = 'none'; // Hide the alert completely
            }, 500); // Wait for the slide-out animation to complete
        }
</script>
    <style>


.input-container i {
    position: absolute;
    right: 10px;
    top: 50%;
    transform: translateY(-50%);
    cursor: pointer;
}
.custom-alert {
        position: fixed;
        top: 100px;
        right: -350px; /* Initially hidden offscreen */
        background-color: #c00; /* Strong red background similar to the image */
        color: white;
        padding: 14px 20px; /* More padding for a compact look */
        border-radius: 3px; /* Slight rounding of corners */
        z-index: 1000;
        box-shadow: 0px 2px 8px rgba(0, 0, 0, 0.2); /* Soft shadow */
        transition: right 0.5s ease-in-out, opacity 0.5s ease; /* Smooth transition */
        opacity: 0;
        font-family: 'Arial', sans-serif;
        font-size: 20px;
        padding:10px 20px;
        display: flex;
        align-items: center; /* Vertically align content */
        justify-content: space-between; /* Space between icon/text and close button */
        width: 350px; /* Fixed width */
    }

    .custom-alert.show {
        right: 20px; /* Bring into view */
        opacity: 1;
    }

    .custom-alert i {
        margin-right: 10px;
        font-size: 18px;
    }

    .close-btn {
        margin-left: 10px;
        cursor: pointer;
        font-size: 18px;
        color: white;
    }
    </style>
    <script type="text/javascript">
        function refreshCaptcha() {
            __doPostBack('<%= btnRefresh.ClientID %>', '');
        }
    </script>
<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function () {
        // Toggle password visibility and icon for the password field
        const togglePassword = document.getElementById('togglePassword');
        const password = document.getElementById('<%= txtPassword.ClientID %>');
        
        togglePassword.addEventListener('click', function () {
            const type = password.type === 'password' ? 'text' : 'password';
            password.type = type;
            
            // Toggle the eye icon
            if (type === 'password') {
                this.classList.remove('fa-eye-slash');
                this.classList.add('fa-eye');
            } else {
                this.classList.remove('fa-eye');
                this.classList.add('fa-eye-slash');
            }
        });

        // Toggle password visibility and icon for the confirm password field
        const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');
        const confirmPassword = document.getElementById('<%= txtConfirmPassword.ClientID %>');
        
        toggleConfirmPassword.addEventListener('click', function () {
            const type = confirmPassword.type === 'password' ? 'text' : 'password';
            confirmPassword.type = type;
            
            // Toggle the eye icon
            if (type === 'password') {
                this.classList.remove('fa-eye-slash');
                this.classList.add('fa-eye');
            } else {
                this.classList.remove('fa-eye');
                this.classList.add('fa-eye-slash');
            }
        });
    });
</script>


</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <div id="customAlert" class="custom-alert">
    <i class="fa fa-exclamation-triangle"></i>
    <span id="customAlertMessage">This is a custom alert!</span>
    <span class="close-btn" onclick="closeCustomAlert()">&times;</span>
    </div>
                <div class="container">
                    <h2>Sign In</h2>
                    <asp:Label ID="LabelMessage" runat="server" CssClass="text-danger"></asp:Label>

                    <div class="input-container">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control mb-2" placeholder=" " TextMode="Email" />
                        <label class="placeholder" for="txtEmail">Enter Email</label>
                    </div>

<div class="input-container">
    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control mb-2" placeholder=" " />
    <label class="placeholder" for="txtPassword">Enter Password</label>
    <i class="fa-solid fa-eye" id="togglePassword"></i>
</div>

<div class="input-container" style="margin-bottom:0px;">
    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control mb-2" placeholder=" " />
    <label class="placeholder" for="txtConfirmPassword">Confirm Password</label>
    <i class="fa-solid fa-eye" id="toggleConfirmPassword"></i>
</div>


                    <div style="margin-bottom:20px;margin-top:5px;text-align:right;margin-right:20px;">
                     <a href="ForgotPassword.aspx" style="font-size: 13px;">Forgot Password</a>
                    </div>
                   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                    <div style="display:flex;justify-content:space-between;align-items:start;margin-bottom:20px;" >
                        <div><asp:Image ID="CaptchaImage" runat="server" CssClass="mb-2" /></div>
                        <div><asp:LinkButton ID="btnRefresh" runat="server" OnClientClick="refreshCaptcha(); return false;" OnClick="btnRefresh_Click" CssClass="btn btn-secondary mb-2"><i class="fa-solid fa-arrows-rotate refresh"></i></asp:LinkButton></div>
                        <div class="input2-container">
                            <asp:TextBox ID="txtCaptcha" runat="server" CssClass="form-control mb-2" style="width: 85%;margin-left:20px;" placeholder=" "></asp:TextBox>
                            <label class="placeholder2" for="txtCaptcha">Enter Captcha</label>
                        </div>
                    </div>
                    </ContentTemplate>
        </asp:UpdatePanel>
                    <asp:Label ID="lblResult" runat="server" CssClass="text-danger" ForeColor="Red"></asp:Label>

                    <asp:HiddenField ID="hfStudentKey" runat="server" Value="default_student_key" />
                    <div style="text-align:center;">
                    <asp:Button ID="btnSignIn" runat="server" Text="Sign In" CssClass="btnRegister" OnClick="btnSignIn_Click"/>
                    <br /><br /><span>not registered? <a href="Signup.aspx" style="font-size: 13px;">register here.</a></span>
                    </div>
                    <div class="or-divider">
                        <div class="line"></div>
                        <span>OR</span>
                        <div class="line"></div>
                    </div>

                     <div class="google-login-container" style="text-align: center; margin: 20px 0;">
                        <asp:LinkButton type="button" CssClass="google-login-btn" ID="btnLogin" runat="server" OnClick="Login" style="text-decoration:none;">
                            <img src="images/pngegg.png" alt="Google logo" class="google-logo">
                            Login with Google
                        </asp:LinkButton>
                    </div>
                    
                </div>
            
    </form>
</body>
</html>
