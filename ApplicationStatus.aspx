<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ApplicationStatus.aspx.vb" Inherits="ApplicationStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Application Status</title>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet">
    <script src="https://kit.fontawesome.com/a429e5bfb6.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="Styles/UserDashboard.css">
    <style>
    .container {
    background-color: #fff;
    margin: 0 auto;
    text-align: center;
    padding:50px 200px;
}
footer{position:fixed;}

.container {
    margin: 60px auto;
    text-align: center;
    padding: 50px;
    background-color: #ffffff;
    border-radius: 10px;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
    max-width: 1000px;
}

#bar-progress {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-top: 50px;
    position: relative;
}

.step {
    flex: 1;
    text-align: center;
    font-size: 14px;
    font-weight: 500;
    text-transform: uppercase;
    color: #333;
    position: relative;
}

.step .number-container {
    background-color: #edf6f9;
    color: #006d77;
    font-size: 24px;
    width: 50px;
    height: 50px;
    margin: 0 auto;
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    font-weight: 700;
    transition: background-color 0.3s, color 0.3s;
    position: relative;
    z-index: 1; /* Keep circles on top */
}

.step.step-active .number-container {
    background-color: #006d77;
    color: white;
}

.step.step-disabled .number-container {
    background-color: red;
    color: white;
}

.step .number-container i {
    font-size: 20px;
}

.seperator {
    width: 100px;
    height: 4px;
    background-color: #83c5be;
    margin-top: -32px;
    top: 50%;

    transform: translateY(-50%);
    z-index: 0;
}

.step:last-child .seperator {
    display: none; /* No separator after the last step */
}

.step h5 {
    margin-top: 10px;
    color: #333;
    font-weight: 500;
}

@media screen and (max-width: 768px) {
    #bar-progress {
        flex-direction: column;
    }

    .seperator {
        width: 4px;
        height: 60px;
        top: 100%; /* Moves the separator to the bottom of the step */
        left: 50%;
        transform: translate(-50%, 0); /* Adjusts to vertical separation */
        margin: 15px 0;
    }
}



    </style>
    <script type="text/javascript">
        // Simulate a load time and hide the loader
        window.onload = function () {
            setTimeout(function () {
                document.getElementById("loader").style.display = "none";
                document.getElementById("content").style.display = "block";
            }, 2000); // Loader will display for 2 seconds
        };
    </script>
</head>
<body>
   <form id="form1" runat="server">
   <%--<div id="loader" class="loader-overlay">
            <div class="loader">
                <div class="dot"></div>
                <div class="dot"></div>
                <div class="dot"></div>
                <div class="dot"></div>
            </div>
        </div>--style="filter: invert(1) grayscale(100%) brightness(100%);"---%>
    <!-- Top bar -->
    <div class="top-bar">
        <div class="logo-section">
            <img src="images/saralicon.png" alt="Logo"  />
        </div>
         <div class="welcome-section">
        <% If Request.QueryString("UserName") IsNot Nothing Then %>
            <span>Welcome, <%= Request.QueryString("UserName")%>!</span>
        <% Else %>
            <span>Welcome, Guest!</span>
        <% End If %>

        <div class="user-menu">
            <asp:Image ID="userIcon" runat="server" ImageUrl="~/images/user.png" AlternateText="User Icon" CssClass="user-icon" />
            <div class="dropdown-content" id="dropdown-menu">
                <a href="#" id="profileLink2" runat="server"><i class="fa-solid fa-user"></i> &nbsp;Profile</a>
                <a href="#" ><i class="fa-solid fa-video"></i>&nbsp;Video Guide</a>
                <a href="#" id="changePass" runat="server"><i class="fa-solid fa-pen"></i> &nbsp;Change Password</a>
                <a href="Logout.aspx"><i class="fa-solid fa-right-from-bracket"></i> &nbsp;Logout</a>
            </div>
        </div>
    </div>
    </div>

    <!-- Navigation bar -->
    <div class="navbar">
        <a href="#" class="active" id="dashboardLink" runat="server"><i class="fa-solid fa-house"></i>Dashboard</a>
<a href="#" id="instructionsLink" runat="server" visible="false"><i class="fa-solid fa-chalkboard-user"></i>Instructions</a>
<a href="#" id="statusLink" runat="server"><i class="fa-solid fa-bars-progress"></i>Status</a>
<a href="#" id="forumLink" runat="server"><i class="fa-solid fa-comment"></i>Discussion Forum</a>
<a href="#" id="previewLink" runat="server"><i class="fa-solid fa-magnifying-glass"></i>Preview</a>
<a href="#" id="profileLink" runat="server"><i class="fa-solid fa-user"></i>Profile</a>
<a href="#" id="fillApplicationButton" runat="server">
    <div class="fillApplication-button"><i class="fa-solid fa-fill"></i>&nbsp;Fill Application Form</div>
</a>
    </div>

     <div class="container">
    <h1>Application Status</h1>
    <div id="bar-progress" class="mt-5">
        <div id="step1" class="step" runat="server">
            <span class="number-container">
                <i id="iconStep1" class="fa-solid fa-file-signature" runat="server"></i>
            </span>
            <h5>Application Start</h5>
        </div>
        <div class="seperator"></div>
        <div id="step2" class="step" runat="server">
            <span class="number-container">
                <i id="iconStep2" class="fa-solid fa-money-check" runat="server"></i>
            </span>
            <h5>Application Fee Paid</h5>
        </div>
        <div class="seperator"></div>
        <div id="step3" class="step" runat="server">
            <span class="number-container">
                <i id="iconStep3" class="fa-solid fa-paper-plane" runat="server"></i>
            </span>
            <h5>Application Submitted</h5>
        </div>
        <div class="seperator"></div>
        <div id="step4" class="step" runat="server">
            <span class="number-container">
                <i id="iconStep4" class="fa-solid fa-file" runat="server"></i>
            </span>
            <h5>Document Verified</h5>
        </div>
        <div class="seperator"></div>
        <div id="step5" class="step" runat="server">
            <span class="number-container">
                <i id="iconStep5" class="fa-solid fa-check" runat="server"></i>
            </span>
            <h5>Registration Approved</h5>
        </div>
        <div class="seperator"></div>
        <div id="step6" class="step" runat="server">
            <span class="number-container">
                <i id="iconStep6" class="fa-solid fa-wallet" runat="server"></i>
            </span>
            <h5>Admission Fee Paid</h5>
        </div>
        <div class="seperator"></div>
        <div id="step7" class="step" runat="server">
            <span class="number-container">
                <i id="iconStep7" class="fa-solid fa-thumbs-up" runat="server"></i>
            </span>
            <h5>Admission Approved</h5>
        </div>
    </div>
</div>

</form>
    <!-- Footer -->
    <footer>
   All Rights Reserved. <i class="fa-regular fa-copyright fa-spin"></i> 2024 Saral ERP Solutions Pvt Ltd. 
    </footer>

    <script>
        // Script to handle bottom border on active links
const navbarLinks = document.querySelectorAll('.navbar a');
navbarLinks.forEach(link => {
    link.addEventListener('click', function() {
        // Remove active class from all links
        navbarLinks.forEach(item => item.classList.remove('active'));

        // Add active class to the clicked link
        if (!this.classList.contains('fillApplication-button')) {
            this.classList.add('active');
        }
    });
});
    </script>
    <script>
    document.addEventListener('DOMContentLoaded', function() {
    const userIcon = document.getElementById('userIcon');
    const dropdownMenu = document.getElementById('dropdown-menu');

    userIcon.addEventListener('click', function() {
        dropdownMenu.classList.toggle('show');
    });

    // Close the dropdown if clicked outside
    window.addEventListener('click', function(event) {
        if (!userIcon.contains(event.target) && !dropdownMenu.contains(event.target)) {
            dropdownMenu.classList.remove('show');
        }
    });
});

    </script>
       <%-- <script type="text/javascript" src="//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>--%>
</body>
</html>
