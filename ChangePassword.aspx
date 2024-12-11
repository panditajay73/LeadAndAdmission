<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ChangePassword.aspx.vb" Inherits="ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Change Password</title>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet">
    <script src="https://kit.fontawesome.com/a429e5bfb6.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="Styles/UserDashboard.css">
      <%--<script type="text/javascript">
          function googleTranslateElementInit() {
              new google.translate.TranslateElement({
                  pageLanguage: 'en',
                  includedLanguages: 'en,hi',
                  layout: google.translate.TranslateElement.InlineLayout.SIMPLE,
                  autoDisplay: true
              }, 'google_element');
          }
    </script>--%>
    <style>
    .container {
    background-color: #ffffff;
    padding: 50px 650px 158px 650px;
    margin: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
}

.input-field {
    width: 100%;
    padding: 10px;
    margin: 10px 0;
    border: 1px solid #00796b;
    border-radius: 4px;
}

.btn-submit {
    background-color: #1ed085; /* Teal */
    color: #ffffff;
    font-weight:bold;
    padding: 10px 20px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

.btn-submit:hover {
    background-color: #1ed085; /* Darker teal */
}
footer{position:fixed;  }
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
        </div>
--%>    <!-- Top bar -->
    <div class="top-bar">
        <div class="logo-section">
            <img src="images/saralicon.png" alt="Logo"  />
        </div>
         <div class="welcome-section">
        <% If Request.QueryString("UserName") IsNot Nothing Then %>
            <span>Request.QueryString("UserName") %>!</span>
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
    <!-- Change Password Section -->
        <div class="container" >
            <h2>Change Password</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />
            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password" CssClass="input-field" Placeholder="Current Password" />
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" CssClass="input-field" Placeholder="New Password" />
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="input-field" Placeholder="Confirm New Password" />
            <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn-submit" OnClick="btnChangePassword_Click" />
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
