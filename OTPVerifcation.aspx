<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OTPVerifcation.aspx.vb" Inherits="OTPVerification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OTP Verification</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet" />
        <link rel="stylesheet" href="Styles/LoginAndRegister.css">
         <link rel="stylesheet" href="Styles/OTPVerification.css">
</head>
<body>
    <form id="form1" runat="server">
        <div class="otp-container">
            <h2 style="color:Black">OTP Verification</h2>
            <p>Enter the code from the SMS we sent to <br /><strong>+8801774280874</strong></p>
            
            <div class="timer" id="timer">02:32</div>
            
          <div class="otp-input-container">
    <asp:TextBox ID="txtOtp1" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp2')" oninput="validateNumber(this)"></asp:TextBox>
    <asp:TextBox ID="txtOtp2" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp3')" onkeydown="moveBack(this, event, 'txtOtp1')" oninput="validateNumber(this)"></asp:TextBox>
    <asp:TextBox ID="txtOtp3" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp4')" onkeydown="moveBack(this, event, 'txtOtp2')" oninput="validateNumber(this)"></asp:TextBox>
    <asp:TextBox ID="txtOtp4" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp5')" onkeydown="moveBack(this, event, 'txtOtp3')" oninput="validateNumber(this)"></asp:TextBox>
    <asp:TextBox ID="txtOtp5" runat="server" CssClass="otp-input" MaxLength="1" onkeyup="moveNext(this, 'txtOtp6')" onkeydown="moveBack(this, event, 'txtOtp4')" oninput="validateNumber(this)"></asp:TextBox>
    <asp:TextBox ID="txtOtp6" runat="server" CssClass="otp-input" MaxLength="1" onkeydown="moveBack(this, event, 'txtOtp5')" oninput="validateNumber(this)"></asp:TextBox>
</div>
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>

            <asp:Button ID="btnSubmit" runat="server" CssClass="submit-btn" Text="Submit" OnClick="btnSubmit_Click" />
            
            <p class="resend-link">Don’t receive the OTP? <a href="#">RESEND</a></p>
        </div>
    </form>

    <script>
        function validateNumber(input) {
            input.value = input.value.replace(/[^0-9]/g, '');  // Replace non-numeric characters
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
        function startTimer(duration, display) {
            var timer = duration, minutes, seconds;
            var interval = setInterval(function () {
                minutes = parseInt(timer / 60, 10);
                seconds = parseInt(timer % 60, 10);

                minutes = minutes < 10 ? "0" + minutes : minutes;
                seconds = seconds < 10 ? "0" + seconds : seconds;

                display.textContent = minutes + ":" + seconds;

                if (--timer < 0) {
                    clearInterval(interval);
                    display.textContent = "00:00";
                }
            }, 1000);
        }

        window.onload = function () {
            var twoMinutes = 2 * 60 + 32, // 2 minutes and 32 seconds countdown
                display = document.querySelector('#timer');
            startTimer(twoMinutes, display);
        };
    </script>
</body>
</html>