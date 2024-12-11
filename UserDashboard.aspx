<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserDashboard.aspx.vb" Inherits="UserDashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Dashboard</title>
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
    .notification-remarks {
    display: inline-block;
    background-color: #ffefc1;  /* Light yellow background like a notification */
    padding: 8px 12px;
    border-radius: 15px;        /* Rounded shape for notification badge style */
    font-weight: bold;          /* Bold text */
    color: #856404;             /* Darker brownish text, typical for notifications */
    border: 1px solid #ffeeba;  /* Border matching the notification background */
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);  /* Soft shadow for depth */
    text-align: center;
    font-size: 14px;            /* Keeps the size small but readable */
    transition: background-color 0.3s ease;
    cursor:pointer;
}

.notification-remarks:hover {
    background-color: #ffe8a1;  /* Darker yellow on hover */
    color: #795c00;             /* Darker text color on hover */
}


@media (max-width:900px)
{
.right-content
{
flex-direction:column;  
}    
}
@media (max-width:600px)
{
.right-content
{
flex-direction:column;  
}     
}
@media (max-width:300px)
{
.right-content
{
flex-direction:column;  
}     
}
.reset-btn
{
bavkground-color:#1ed085;
color:White
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
    <div id="loader" class="loader-overlay">
            <div class="loader">
                <div class="dot"></div>
                <div class="dot"></div>
                <div class="dot"></div>
                <div class="dot"></div>
            </div>
        </div>
        <!-- Top bar -->
<div class="top-bar">
    <div class="logo-section">
        <img src="images/saralicon.png" alt="Logo" />
    </div>

 <div class="welcome-section">
        <% If Request.QueryString("UserName") IsNot Nothing Then%>
            <span>Welcome, <asp:Label ID ="UserNameLaabel" runat="server"></asp:Label>!</span>
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
        
        <div class="content">
            <!-- Left content section -->
            <div class="left-content">
                <div class="upper-smaller-boxes">
    <div class="boxes">
        <span><asp:Literal ID="litPercentage" runat="server" /></span>
        <span>Application Completed</span>
    </div>
    <div class="boxes">
        <span>Feedback/Remarks</span>
       <span class="" id="remarks" runat="server" >
    <asp:Literal ID="litRemarks" runat="server" Text="No Feedback Available"></asp:Literal>
</span>


    </div>
    <div class="boxes">
        <span><asp:Literal ID="profilePercentage" runat="server" /></span>
        <span>Profile Completed</span>
    </div>
</div>



                <div class="lower-content">
    <div class="activity-log">
    <h3 style="font-size:22px; text-align:center;text-decoration:underline;margin-bottom:30px;">Activity Log</h3>
        <div class="log-header">
            
            <div class="log-filters">
                <input type="text" id="searchInput" placeholder="Search Log" />
                <input type="date" id="startDate" />
                <input type="date" id="endDate" />
                <button class="reset-btn" onclick="resetFilters()">Reset</button>
                <button class="apply-btn" onclick="applyDateFilter()">Apply</button>
            </div>
        </div>
        <div class="log-list" id="logList">
            <!-- Log Items here -->
            <div class="log-item">
                <img src="images/user.png" alt="User Image" class="user-image" />
                <div class="log-details">
                    <div class="log-user">John Doe <span class="user-role">Admissions Counselor</span></div>
                    <div class="log-action">Captured a new lead: Jane Smith</div>
                    
                </div>
                <div class="log-timestamp">
                    <span>2024-09-01 09:15:30</span>
                    <div class="log-actions">
                    <div class="log-meta">Source: Online Form</div>
                        <button title="Mail Copy">✉️</button>
                        <button title="View Details">🔍</button>
                        
                    </div>
                </div>
            </div>

                    <!-- Log Item 2 -->
        <div class="log-item">
            <img src="images/user.png" alt="User Image" class="user-image" />
            <div class="log-details">
                <div class="log-user">Jane Smith <span class="user-role">Applicant</span></div>
                <div class="log-action">Submitted an application for B.Sc. Computer Science</div>
                
            </div>
            <div class="log-timestamp">
                <span>2024-09-02 11:45:22</span>
                <div class="log-actions">
                <div class="log-meta">Application No: 12345</div>
                    <button title="Mail Copy">✉️</button>
                    <button title="View Details">🔍</button>
                </div>
            </div>
        </div>

                <!-- Log Item 3 -->
        <div class="log-item">
            <img src="images/user.png" alt="User Image" class="user-image" />
            <div class="log-details">
                <div class="log-user">Dr. Sarah Lee <span class="user-role">Head of Department</span></div>
                <div class="log-action">Verified documents for applicant: Jane Smith</div>
                
            </div>
            <div class="log-timestamp">
                <span>2024-09-03 14:20:10</span>
                <div class="log-actions">
                <div class="log-meta">All documents verified and approved</div>
                    <button title="Mail Copy">✉️</button>
                    <button title="View Details">🔍</button>
                </div>
            </div>
        </div>
        </div>
        <a href="#">Read More...</a>
    </div>
</div>

            </div>

            <!-- Right content section: Notification box -->
            <div class="right-content">
            <div id="DiscussionNotification">
                            <h3>Discussion Forum Notifications</h3>
                            <div class="notification-item">
                    <p><i class="fa-solid fa-bell"></i>&nbsp;New message in your forum: "Admission Process Questions"</p>
                    <small>Posted 2 hours ago</small>
                </div>
                <div class="notification-item">
                    <p> <i class="fa-solid fa-bell"></i>&nbsp;New reply to your comment on "Documents Required"</p>
                    <small>Posted 1 day ago</small>
                </div>
                <div class="notification-item">
                    <p><i class="fa-solid fa-bell"></i>&nbsp;Moderator posted an announcement: "New Deadlines"</p>
                    <small>Posted 3 days ago</small>
                </div>
                <div class="notification-item">
                    <p><i class="fa-solid fa-bell"></i>&nbsp;New message in your forum: "Admission Process Questions"</p>
                    <small>Posted 2 hours ago</small>
                </div>
                <div class="notification-item">
                    <p><i class="fa-solid fa-bell"></i>&nbsp;New reply to your comment on "Documents Required"</p>
                    <small>Posted 1 day ago</small>
                </div>
                <div class="notification-item">
                    <p><i class="fa-solid fa-bell"></i>&nbsp;Moderator posted an announcement: "New Deadlines"</p>
                    <small>Posted 3 days ago</small>
                </div>
            </div>

            <div>
            <br />
            <br />
             <button class="readmore-btn" >Read More...</button>
            </div>
            </div>
        </div>
       <div class="content">
    <!-- Left content section (Profile Details) -->
    <div class="left-content">
        <div class="profile-section">
            <div class="profile-header">
                <h2>User Profile</h2>
                <asp:Button ID="btnEditProfile" runat="server" CssClass="edit-profile-btn" Text="Complete Profile"  OnClick="EditProfileBtn_click"/>


            </div>
            <div class="profile-details">
    <div class="profile-field">
        <label>Full Name:</label>
        <span>&nbsp;&nbsp;<asp:Label ID = "FullnameP" runat="server"></asp:Label></span>
    </div>
    <div class="profile-field">
        <label>Email:</label>
        <span>&nbsp;&nbsp;<asp:Label ID = "emailP" runat="server"></asp:Label></span>
    </div>
    <div class="profile-field">
        <label>Phone Number:</label>
        <span>&nbsp;&nbsp;<asp:Label ID = "mobileP" runat="server"></asp:Label></span>
    </div>
    <div class="profile-field">
        <label>Selected Course:</label>
        <span>&nbsp;&nbsp;<asp:Label ID = "courseP" runat="server"></asp:Label></span>
    </div>
</div>

        </div>
    </div>

    <!-- Right content section (Verification Status) -->
    <div class="right-content">
        <h3>Authentication Status</h3>
        <div class="notification-item">
            <p>Email Verified: <strong style="color: green;">Yes</strong></p>
            <button class="verify-btn">Verified</button>
        </div>
        <div class="notification-item">
            <p>Phone Number Verified: <strong style="color: red;">No</strong></p>
            <button class="verify-btn">Verify Phone Number</button>
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
    <script>
    // Live search functionality
    document.getElementById('searchInput').addEventListener('input', function() {
        const searchInput = this.value.toLowerCase();
        const logItems = document.querySelectorAll('.log-item');

        logItems.forEach(item => {
            const logText = item.innerText.toLowerCase();
            item.style.display = logText.includes(searchInput) ? 'flex' : 'none';
        });
    });

    // Function to apply date filter
    function applyDateFilter() {
        const startDate = new Date(document.getElementById('startDate').value);
        const endDate = new Date(document.getElementById('endDate').value);
        const logItems = document.querySelectorAll('.log-item');

        logItems.forEach(item => {
            const logDate = new Date(item.querySelector('.log-timestamp span').textContent);

            if (
                (!isNaN(startDate.getTime()) && logDate < startDate) ||
                (!isNaN(endDate.getTime()) && logDate > endDate)
            ) {
                item.style.display = 'none';
            } else {
                item.style.display = 'flex';
            }
        });
    }

    // Function to reset filters
    function resetFilters() {
        document.getElementById('searchInput').value = '';
        document.getElementById('startDate').value = '';
        document.getElementById('endDate').value = '';

        const logItems = document.querySelectorAll('.log-item');
        logItems.forEach(item => {
            item.style.display = 'flex';
        });
    }
</script>
       <%-- <script type="text/javascript" src="//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>--%>
</body>
</html>
