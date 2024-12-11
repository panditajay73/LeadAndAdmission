<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OTPVerificationRegister.aspx.vb" Inherits="OTPVerificationRegister" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OTP Verification</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="Styles/LoginAndRegister.css" />
    <link rel="stylesheet" href="Styles/OTPVerification.css" />
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
        #btnResend {
            display: none; /* Initially hidden */
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
</head>
<body>
    <form id="form1" runat="server">
        <div class="otp-container">
               <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <div id="customAlert" class="custom-alert">
    <i class="fa fa-exclamation-triangle"></i>
    <span id="customAlertMessage">This is a custom alert!</span>
    <span class="close-btn" onclick="closeCustomAlert()">&times;</span>
    </div>
            <h2 style="color:Black">OTP Verification</h2>
            <p>Enter the code from the Email we sent to <br /><strong><asp:Label ID= "emailLabel" runat="server"></asp:Label></strong></p>
            <div class="timer" id="timer">01:30</div>
            
            <div class="otp-input-container">
                <asp:TextBox ID="txtOtp1" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp2')" oninput="validateNumber(this)"></asp:TextBox>
                <asp:TextBox ID="txtOtp2" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp3')" onkeydown="moveBack(this, event, 'txtOtp1')" oninput="validateNumber(this)"></asp:TextBox>
                <asp:TextBox ID="txtOtp3" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp4')" onkeydown="moveBack(this, event, 'txtOtp2')" oninput="validateNumber(this)"></asp:TextBox>
                <asp:TextBox ID="txtOtp4" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp5')" onkeydown="moveBack(this, event, 'txtOtp3')" oninput="validateNumber(this)"></asp:TextBox>
                <asp:TextBox ID="txtOtp5" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp6')" onkeydown="moveBack(this, event, 'txtOtp4')" oninput="validateNumber(this)"></asp:TextBox>
                <asp:TextBox ID="txtOtp6" runat="server" CssClass="otp-input" MaxLength="1" onkeydown="moveBack(this, event, 'txtOtp5')" oninput="validateNumber(this)"></asp:TextBox>
            </div>
            
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>

            <asp:Button ID="btnSubmit" runat="server" CssClass="submit-btn" Text="Submit" />

            <asp:Button ID="btnResend" runat="server" CssClass="submit-btn" Text="Resend OTP" OnClick="ResendOTPClick" style="width:50%;padding:10px; background-color:White;color:Black;font-weight:bold;text-decoration:underline;"/>

            <p class="resend-link" id="resendNotice">Don’t receive the OTP? <strong>Wait for the timer to end.</strong></p>
        </div>
    </form>

    <script>
        function validateNumber(input) {
            input.value = input.value.replace(/[^0-9]/g, '');
        }

        function moveNext(current, nextFieldId) {
            if (current.value.length === 1) {
                document.getElementById(nextFieldId).focus();
            }
        }

        function moveBack(current, event, prevFieldId) {
            if (event.key === "Backspace" && current.value === "") {
                document.getElementById(prevFieldId).focus();
            }
        }

        function startTimer(duration, display, resendButton, resendNotice) {
            let timer = duration, minutes, seconds;
            const interval = setInterval(function () {
                minutes = parseInt(timer / 60, 10);
                seconds = parseInt(timer % 60, 10);

                minutes = minutes < 10 ? "0" + minutes : minutes;
                seconds = seconds < 10 ? "0" + seconds : seconds;

                display.textContent = minutes + ":" + seconds;

                if (--timer < 0) {
                    clearInterval(interval);
                    display.textContent = "00:00";
                    resendButton.style.display = "block";
                    resendNotice.style.display = "none"; // Hide Resend notice
                }
            }, 1000);
        }

        window.onload = function () {
            const duration = 1 * 60 + 30; // 1 minutes 30 seconds
            const display = document.querySelector('#timer');
            const resendButton = document.getElementById('btnResend');
            const resendNotice = document.getElementById('resendNotice');
            startTimer(duration, display, resendButton, resendNotice);
        };
    </script>
</body>
</html>
