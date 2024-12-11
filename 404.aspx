<%@ Page Language="VB" AutoEventWireup="false" CodeFile="404.aspx.vb" Inherits="_404" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <title>Error 404 - Page Not Found!</title>
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

.buton {
    background: #008080;
    padding: 10px 20px;
    color: #fff;
    font-weight: bold;
    text-align: center;
    border-radius: 3px;
    text-decoration: none;
}
.buton:hover
{
background:#006666;
}
.ops{height:290px;
     width:auto;
     
     }

@media screen and (max-width: 500px) {
    img {
        width: 70%;
    }
    .container {
        padding: 70px 10px 10px 10px;
    }
    h3 {
        font-size: 14px;
    }
}
    </style>
</head>
<body>
   <form id="form1" runat="server">
    <!-- Top bar -->
    <div class="top-bar">
        <div class="logo-section">
            <img src="images/saralicon.png" alt="Logo" style="filter: invert(1) grayscale(100%) brightness(100%);" />
        </div>
         <div class="welcome-section">
        <% If Session("UserName") IsNot Nothing Then %>
            <span>Welcome, <%= Session("UserName") %>!</span>
        <% Else %>
            <span>Welcome, Guest!</span>
        <% End If %>

        <div class="user-menu">
            <asp:Image ID="userIcon" runat="server" ImageUrl="~/images/user.png" AlternateText="User Icon" CssClass="user-icon" />
            <div class="dropdown-content" id="dropdown-menu">
                <a href="UserProfilePage.aspx"><i class="fa-solid fa-user"></i> &nbsp;Profile</a>
                <a href="#"><i class="fa-solid fa-video"></i>&nbsp;Video Guide</a>
                <a href="ChangePassword.aspx"><i class="fa-solid fa-pen"></i> &nbsp;Change Password</a>
                <a href="Logout.aspx"><i class="fa-solid fa-right-from-bracket"></i> &nbsp;Logout</a>
            </div>
        </div>
    </div>
    </div>

    <!-- Navigation bar -->
    <div class="navbar">
        <a href="UserDashboard.aspx" class="active" id="dashboard-link"><i class="fa-solid fa-house"></i>Dashboard</a>
        <a href="#" id="instructions-link"><i class="fa-solid fa-chalkboard-user"></i>Instructions</a>
        <a href="ApplicationStatus.aspx" id="status-link"><i class="fa-solid fa-bars-progress"></i>Status</a>
        <a href="DiscussionForum.aspx" id="forum-link"><i class="fa-solid fa-comment"></i>Discussion Forum</a>
        <a href="ApplicationFormPreview.aspx" id="preview-link"><i class="fa-solid fa-magnifying-glass"></i>Preview</a>
        <a href="UserProfilePage.aspx" id="profile-link"><i class="fa-solid fa-user"></i>Profile</a>
        <a href="ApplicationForm.aspx"> 
            <div class="fillApplication-button"><i class="fa-solid fa-fill"></i>&nbsp;Fill Application Form</div>
        </a>
    </div>

    <div class="container">
        <img class="ops" src="images/404.jpg" />
        <br /><br />
        <h3>The page you are looking for was not found.
            <br /> It might be because the URL is incorrect or the page is not available.</h3>
        <br />
        <a class="buton" href="UserDashboard.aspx">Go Back to Home</a>
    </div>
</form>
    <!-- Footer -->
    <footer>
    &copy; 2024 Saral ERP Solutions Pvt Ltd. All Rights Reserved.
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
