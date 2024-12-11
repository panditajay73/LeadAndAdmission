<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ApplicationForm.aspx.vb" Inherits="ApplicationForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Application Form</title>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet">
    <script src="https://kit.fontawesome.com/a429e5bfb6.js" crossorigin="anonymous"></script>
    <link rel="stylesheet" href="Styles/UserDashboard.css">
     <link rel="stylesheet" href="Styles/ApplicationForm.css">
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
        <script>
    // Restrict input to numbers and set maxlength dynamically
document.addEventListener("DOMContentLoaded", function () {
    const mobileInput = document.getElementById('<%= txtGuardianMobile.ClientID %>');
    const permPinCode = document.getElementById('<%= permPinCode.ClientID %>');
    const resPinCode = document.getElementById('<%= resPinCode.ClientID %>');
    if (mobileInput) {
        // Restrict input to numbers only
        mobileInput.addEventListener("input", function (event) {
            this.value = this.value.replace(/[^0-9]/g, ''); // Allow only digits
        });

        // Enforce maximum length of 10
        mobileInput.addEventListener("keypress", function (event) {
            if (this.value.length >= 10) {
                event.preventDefault(); // Prevent further input if max length is reached
            }
        });

        // Set input mode to numeric for mobile devices
        mobileInput.setAttribute("inputmode", "numeric");
    }
     if (permPinCode) {
        // Restrict input to numbers only
        permPinCode.addEventListener("input", function (event) {
            this.value = this.value.replace(/[^0-9]/g, ''); // Allow only digits
        });

        // Enforce maximum length of 10
        permPinCode.addEventListener("keypress", function (event) {
            if (this.value.length >= 6) {
                event.preventDefault(); // Prevent further input if max length is reached
            }
        });

        // Set input mode to numeric for mobile devices
        permPinCode.setAttribute("inputmode", "numeric");
    }
     if (resPinCode) {
        // Restrict input to numbers only
        resPinCode.addEventListener("input", function (event) {
            this.value = this.value.replace(/[^0-9]/g, ''); // Allow only digits
        });

        // Enforce maximum length of 10
        resPinCode.addEventListener("keypress", function (event) {
            if (this.value.length >= 6) {
                event.preventDefault(); // Prevent further input if max length is reached
            }
        });

        // Set input mode to numeric for mobile devices
        resPinCode.setAttribute("inputmode", "numeric");
    }
});

    </script>
    <style>
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
.required {
    color: red;
    font-weight: bold;
    margin-left: 5px;
}

.error-message {
    color: red;
    font-size: 12px;
    margin-top: 5px;
    display: block;
}

</style>
<script>
    function showCustomAlert(message) {
        var alertBox = document.getElementById('customAlert');
        var alertMessage = document.getElementById('customAlertMessage');

        // Set the custom message
        alertMessage.innerHTML = message;

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

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
      
        <!-- Top bar -->
<div class="top-bar">
    <div class="logo-section">
        <img src="images/saralicon.png" alt="Logo"/>
    </div>

   <div class="welcome-section">
        <% If Request.QueryString("UserName") IsNot Nothing Then%>
            <span>Welcome, <%= Request.QueryString("UserName") %>!</span>
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



         <div class="content" id="content" runat="server">
        <div class="left-content">
        <h1 style="text-decoration:underline;margin-bottom:20px;text-align:center;">Application Form</h1>


             <!-- Alert box container -->
<div id="customAlert" class="custom-alert">
    <i class="fa fa-exclamation-triangle"></i>
    <span id="customAlertMessage">This is a custom alert!</span>
    <span class="close-btn" onclick="closeCustomAlert()">&times;</span>
</div>





    <div class="profile-section" style="width:98%;">
    <div class="profile-container">
        <h2 style="margin-bottom:20px;"></h2>

        <!-- Menu Bar for Personal Details and Change Password -->
<div class="progress-bar">
    <div class="progress-step" data-label="Personal Details" onclick="openTab(event, 'PersonalData', 0)">1</div>
    <div class="progress-line"></div>

    <div class="progress-step" data-label="Educational Details" onclick="openTab(event, 'EducationalData', 1)">2</div>
    <div class="progress-line"></div>

    <div class="progress-step" data-label="Photo/Sign Upload" onclick="openTab(event, 'PhotoSignData', 2)">3</div>
    <div class="progress-line"></div>

    <div class="progress-step" data-label="Contact Details" onclick="openTab(event, 'ContactData', 3)">4</div>
    <div class="progress-line"></div>

    <div class="progress-step" data-label="Documents Upload" onclick="openTab(event, 'DocumentData', 4)">5</div>
    <div class="progress-line"></div>

    <div class="progress-step" data-label="Pay Application Fee" onclick="openTab(event, 'PaymentData', 5)">6</div>
</div>


        <!-- Personal Data Form in Two Columns -->
        <div id="PersonalData" class="tabcontent" style="display: none;">
            <div class="profile-form">
                <div style="display:flex;justify-content:space-between;">
                    <!-- First Column -->
                    <div class="form-column">
                        <div class="form-group">
                            <label for="FullName">Full Name</label>
                            <asp:TextBox ID="txtFullName" runat="server" placeholder="Enter Fullname" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="Email">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter Email" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="Mobile">Mobile No</label>
                            <asp:TextBox ID="txtMobile" runat="server" placeholder="Enter Mobile No" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="ApplyFor">Apply for</label>
                            <asp:TextBox ID="txtApplyFor" runat="server" placeholder="Enter Program" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="form-group">
                <label for="Gender">Gender <span class="required">*</span></label>
                <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                    <asp:ListItem Text="--Select Gender--" Value=""></asp:ListItem>
                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="genderL" runat="server" Text="Gender is required" CssClass="error-message" Visible="false"></asp:Label>
            </div>
                        <div class="form-group">
                <label for="DOB">Date of Birth <span class="required">*</span></label>
                <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="dobL" runat="server" Text="Date of Birth is required" CssClass="error-message" Visible="false"></asp:Label>
            </div>
                        <div class="form-group">
                <label for="FatherName">Father's Name <span class="required">*</span></label>
                <asp:TextBox ID="txtFatherName" runat="server" placeholder="Enter Father Name" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="fatherL" runat="server" Text="Father's Name is required" CssClass="error-message" Visible="false"></asp:Label>
            </div>
                    </div>

                    <!-- Second Column -->
                    <div class="form-column">
                         <div class="form-group">
                <label for="MotherName">Mother's Name <span class="required">*</span></label>
                <asp:TextBox ID="txtMotherName" runat="server" placeholder="Enter Mother Name" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="motherL" runat="server" Text="Mother's Name is required" CssClass="error-message" Visible="false"></asp:Label>
               
            </div>
                         <div class="form-group">
                <label for="GuardianMobile">Guardian's Mobile No <span class="required">*</span></label>
                <asp:TextBox ID="txtGuardianMobile" runat="server" placeholder="Guardian's Mobile No" CssClass="form-control"></asp:TextBox>
                <asp:Label ID="gMobileL" runat="server" Text="Guardian's Mobile No is required" CssClass="error-message" Visible="false"></asp:Label>
            </div>
                        <div class="form-group">
                            <label for="GuardianIncome">Guardian's Annual Income</label>
                            <asp:DropDownList ID="ddlGuardianIncome" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Below 1 Lakh" Value="1Lakh"></asp:ListItem>
                                <asp:ListItem Text="1-5 Lakhs" Value="1-5Lakh"></asp:ListItem>
                                <asp:ListItem Text="5-10 Lakhs" Value="5-10Lakh"></asp:ListItem>
                                <asp:ListItem Text="Above 10 Lakhs" Value="10Lakh+"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="Religion">Religion<span class="required">*</span></label>
                            <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control"></asp:DropDownList>
                           <asp:Label ID="religionL" runat="server" Text="Religion is required" CssClass="error-message" Visible="false"></asp:Label>
                        </div>
                        <div class="form-group">
                            <label for="Category">Category<span class="required">*</span></label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
                            <asp:Label ID="categoryL" runat="server" Text="Religion is required" CssClass="error-message" Visible="false"></asp:Label>
            </div>
                        <div class="form-group">
                            <label for="SubCategory">Sub Category</label>
                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control">
                                <asp:ListItem Text="None" Value="None"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                        <!-- Status Message -->
                        <div class="status-message" style="text-align: center; margin-top: 10px;">
                            <i class="fa-solid fa-check"></i> All changes are saved
                        </div>
                    </div>
                </div>

                <!-- Hidden Field to store status -->
                <asp:HiddenField ID="hfFormStatus" runat="server" Value="0" />
                <%--<asp:Label ID = "label1" runat="server" Text="AJAY"></asp:Label>--%>
                <!-- Save Button -->
                <div style="width: 100%; text-align: center;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="menu-item active" Onclick="btnPersonalDetailsSave_Click" />
                     <asp:Button ID="btnPersonalNext" runat="server" Text="Next" CssClass="menu-item active" Onclick="btnPersonalNext_Click" />
                </div>
            </div>
        </div>


<!-- Educational Data Form -->
<div id="EducationalData" class="tabcontent" style="display: none;">
      <asp:GridView ID="gvEducation" runat="server" AutoGenerateColumns="False" CssClass="education-grid" OnRowCommand="gvEducation_RowCommand">
        <Columns>
            <asp:BoundField DataField="ExamName" HeaderText="Qualification" />
            <asp:BoundField DataField="InstituteName" HeaderText="Institute Name" />
            <asp:BoundField DataField="RollNumber" HeaderText="Roll Number" />
            <asp:BoundField DataField="PassingYear" HeaderText="Passing Year" />
            <asp:BoundField DataField="BoardID" HeaderText="Board/University Name" />
            <asp:BoundField DataField="ObtainedMarks" HeaderText="Obtained Marks" />
            <asp:BoundField DataField="MaxMarks" HeaderText="Max. Marks" />
            <asp:BoundField DataField="Percentage" HeaderText="Percentage" DataFormatString="{0:N2}" />

            <asp:TemplateField HeaderText="Action">
    <ItemTemplate>
        <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteRow" CommandArgument='<%# Eval("ExamName") %>' CssClass="delete-btn" />
    </ItemTemplate>
</asp:TemplateField>

        </Columns>
    </asp:GridView>
    <asp:Button ID="btnAddMore" runat="server" Text="+Add New Education" 
    OnClientClick="showAddMoreForm(); return false;" CssClass="add-more-btn" />
    <!-- Add More Form - Initially Hidden -->
    <div id="addMoreForm" style="display:none;" runat="server">
        <div style="display: flex; flex-wrap: wrap; gap: 10px; margin: 20px 0;">
    <!-- Exam Name -->
    <div style="flex: 1; display: flex; flex-direction: column;">
        <label for="ddlExamName" style="margin-bottom: 5px; font-weight: bold;font-size:13px;">Exam Name</label>
        <asp:DropDownList ID="ddlExamName" runat="server" CssClass="form-control">
            <asp:ListItem Text="Select Exam" Value="" />
            <asp:ListItem Text="High School" Value="High School" />
            <asp:ListItem Text="Diploma" Value="Diploma" />
            <asp:ListItem Text="Intermediate" Value="Intermediate" />
            <asp:ListItem Text="Graduation" Value="Graduation" />
            <asp:ListItem Text="Post Graduation" Value="Post Graduation" />
            <asp:ListItem Text="Other" Value="Other" />
        </asp:DropDownList>
    </div>

    <!-- Institute Name -->
    <div style="flex: 1; display: flex; flex-direction: column;">
        <label for="txtInstituteName" style="margin-bottom: 5px; font-weight: bold;font-size:13px;">Institute Name</label>
        <asp:TextBox ID="txtInstituteName" runat="server" CssClass="form-control" placeholder="Institute Name" />
    </div>

    <!-- Roll Number -->
    <div style="flex: 1; display: flex; flex-direction: column;">
        <label for="txtRollNumber" style="margin-bottom: 5px; font-weight: bold;font-size:13px;">Roll Number</label>
        <asp:TextBox ID="txtRollNumber" runat="server" CssClass="form-control" placeholder="Roll Number" />
    </div>

    <!-- Passing Year -->
    <div style="flex: 1; display: flex; flex-direction: column;">
        <label for="ddlYear" style="margin-bottom: 5px; font-weight: bold;font-size:13px;">Passing Year</label>
        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>

    <!-- Board/University -->
    <div style="flex: 1; display: flex; flex-direction: column;">
        <label for="ddlBoard" style="margin-bottom: 5px; font-weight: bold;font-size:13px;">Board/University</label>
        <asp:DropDownList ID="ddlBoard" runat="server" CssClass="form-control"></asp:DropDownList>
    </div>

    <!-- Obtained Marks -->
    <div style="flex: 1; display: flex; flex-direction: column;">
        <label for="txtObtainedMarks" style="margin-bottom: 5px; font-weight: bold;font-size:13px;">Obtained Marks</label>
        <asp:TextBox ID="txtObtainedMarks" runat="server" CssClass="form-control" placeholder="Obtained Marks" />
    </div>

    <!-- Max Marks -->
    <div style="flex: 1; display: flex; flex-direction: column;">
        <label for="txtMaxMarks" style="margin-bottom: 5px; font-weight: bold;font-size:13px;">Max Marks</label>
        <asp:TextBox ID="txtMaxMarks" runat="server" CssClass="form-control" placeholder="Max Marks" />
    </div>

    <!-- Percentage -->
  <%--  <div style="flex: 1; display: flex; flex-direction: column;">
        <asp:TextBox ID="txtPercentage" runat="server" CssClass="form-control" ReadOnly="True" placeholder="Percentage" Visible="false" />
    </div>--%>
</div>

    </div>
      <script type="text/javascript">
    function validateNumericInput(event) {
        const key = event.which || event.keyCode;
        if (key < 48 || key > 57) { // Allow only numeric keys (0-9)
            event.preventDefault();
        }
    }

    document.getElementById('<%= txtRollNumber.ClientID %>').addEventListener('keypress', validateNumericInput);
    document.getElementById('<%= txtMaxMarks.ClientID %>').addEventListener('keypress', validateNumericInput);
    document.getElementById('<%= txtObtainedMarks.ClientID %>').addEventListener('keypress', validateNumericInput);
    document.getElementById('<%= permPinCode.ClientID %>').addEventListener('keypress', validateNumericInput);
    document.getElementById('<%= resPinCode.ClientID %>').addEventListener('keypress', validateNumericInput);
</script>
    <!-- Buttons -->
    <div class="form-buttons">
      <asp:Button ID="btnEducationPrev" runat="server" Text="Previous" CssClass="menu-item active" Onclick="btnEducationPrev_Click" />
        <asp:Button ID="Button1" runat="server" Text="Save" OnClick="SaveEducation" CssClass="menu-item active"/>
        <asp:Button ID="btnEducationNext" runat="server" Text="Next" CssClass="menu-item active" Onclick="btnEducationNext_Click" />
    </div>
    <asp:HiddenField ID="hiddenEducationStatus" runat="server" Value="0" />
    <asp:Label ID="lblStatusMessage" runat="server" CssClass="status-message" />    
</div>
<script type="text/javascript">
    function showAddMoreForm() {
        document.getElementById('addMoreForm').style.display = 'block';
    }
</script>
        <!-- Photo/Sign Data Form -->
<div id="PhotoSignData" class="tabcontent" style="display: none;">
    <div class="profile-form">
        <!-- Photo Column -->
        <div class="form-column">
            <h3>Photo</h3>
            <div class="form-group image-upload">
                <asp:Image ID="PhotoPreview" runat="server" CssClass="preview-img" />
                <asp:FileUpload ID="PhotoUpload" runat="server" />
                <asp:Label ID="PhotoUploadLabel" Text="Choose File" AssociatedControlID="PhotoUpload" runat="server" />
            </div>
        </div>

        <!-- Signature Column -->
        <div class="form-column">
            <h3>Signature</h3>
            <div class="form-group image-upload">
                <asp:Image ID="SignaturePreview" runat="server" CssClass="preview-img" />
                <asp:FileUpload ID="SignatureUpload" runat="server" />
                <asp:Label ID="SignatureUploadLabel" Text="Choose File" AssociatedControlID="SignatureUpload" runat="server" />
            </div>
        </div>
        <!-- Save Button -->
        <div class="form-buttons">
          <asp:Button ID="btnPhotoSignPrev" runat="server" Text="Previous" CssClass="menu-item active" Onclick="btnPhotoSignPrev_Click" />
            <asp:Button ID="Button2" runat="server" Text="Save" CssClass="menu-item active" OnClick="btnPhotoSignSave_Click" />
            <asp:Button ID="btnPhotoSignNext" runat="server" Text="Next" CssClass="menu-item active" Onclick="btnPhotoSignNext_Click" />
        </div>
    </div>

    <asp:HiddenField ID="HiddenPhotoSignStatus" runat="server" Value="0" />
</div>

        <!-- Contact Data Form -->
<div id="ContactData" class="tabcontent" style="display: none;">
    <div class="profile-form">
        <!-- Residential Address Section -->
        <div class="form-column">
            <h3 style="text-decoration:underline;margin-bottom:10px;">Residential Address</h3>
            <!-- Clear Link -->
            <div class="form-group">
                <a class="clear-link" onclick="clearResidentialAddress()" style="padding:5px 10px;border:1px solid #1ed085;text-decoration:none;border-radius:5px;background-color:#1ed085;color:#fff;cursor:pointer;">Clear Residential</a>
                <a class="clear-link" onclick="clearPermanetAddress()" style="padding:5px 10px;border:1px solid #1ed085;text-decoration:none;border-radius:5px;background-color:#1ed085;color:#fff;cursor:pointer;">Clear Permanent</a>
            </div>
            
            <div class="form-group">
                <asp:Label ID="resAddressLabel" runat="server" Text="Address">
                    <i class="fa-solid fa-home"></i> Address
                </asp:Label>
                <asp:TextBox ID="resAddress" runat="server" TextMode="MultiLine" Rows="4" Columns="50" Placeholder="Enter Full Address"></asp:TextBox>
            </div>
              <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="form-group">
            <asp:Label ID="resCountryLabel" runat="server" Text="Country"></asp:Label>
           <asp:DropDownList ID="resCountry" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="resCountry_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="resStateLabel" runat="server" Text="State"></asp:Label>
            <asp:DropDownList ID="resState" runat="server" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="resState_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="resCityLabel" runat="server" Text="City"></asp:Label>
            <asp:DropDownList ID="resCity" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
            <div class="form-group">
                <asp:Label ID="resPinCodeLabel" runat="server" Text="Pin code"></asp:Label>
                <asp:TextBox ID="resPinCode" runat="server" Placeholder="Enter Pincode"></asp:TextBox>
            </div>
        </div>

        <!-- Permanent Address Section -->
        <div class="form-column">
            <h3 style="text-decoration:underline;margin-bottom:10px;">Permanent Address</h3>

            <%--<div class="checkbox-group">
                <input type="checkbox" id="sameAddress" onchange="copyAddress()">
                <label for="sameAddress">If same as Residential address</label>
            </div>--%>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
            <div class="checkbox-group">
    <asp:CheckBox ID="chkSameAddress" runat="server" AutoPostBack="True" OnCheckedChanged="chkSameAddress_CheckedChanged" Text="If same as Residential address" />
</div>

            <div class="form-group">
                <asp:Label ID="permAddressLabel" runat="server" Text="Address">
                    <i class="fa-solid fa-home"></i> Address
                </asp:Label>
                <asp:TextBox ID="permAddress" runat="server" TextMode="MultiLine" Rows="4" Columns="50" Placeholder="Enter Full Address"></asp:TextBox>
            </div>


        <div class="form-group">
            <asp:Label ID="permCountryLabel" runat="server" Text="Country"></asp:Label>
            <asp:DropDownList ID="permCountry" runat="server" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="permStateLabel" runat="server" Text="State"></asp:Label>
            <asp:DropDownList ID="permState" runat="server" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label ID="permCityLabel" runat="server" Text="City"></asp:Label>
            <asp:DropDownList ID="permCity" runat="server" CssClass="form-control"></asp:DropDownList>
        </div>
  
            <div class="form-group">
                <asp:Label ID="permPinCodeLabel" runat="server" Text="Pin code"></asp:Label>
                <asp:TextBox ID="permPinCode" runat="server" Placeholder="Enter Pincode"></asp:TextBox>
            </div>
              </ContentTemplate>
</asp:UpdatePanel>
        </div>
    </div>

    <!-- Save Button -->
    <div class="form-buttons">
    <asp:Button ID="btnContactPrev" runat="server" Text="Previous" CssClass="menu-item active" Onclick="btnContactPrev_Click" />
        <asp:Button ID="SaveButton" runat="server" Text="Save " CssClass="menu-item active" OnClick="ContactSaveButton_Click"/>
        <asp:Button ID="btnContactNext" runat="server" Text="Next" CssClass="menu-item active" Onclick="btnContactNext_Click" />
    </div>

    <asp:HiddenField ID="HiddenContactStatus" runat="server" Value="0" />
</div>


 <div id="DocumentData" class="tabcontent" style="display: none;">
            <div class="profile-form" style="padding: 10px; display: flex; justify-content: space-between;">
    <!-- Left Section -->
    <div id="first" style="flex: 1; max-width: 45%;">
        <!-- Document Dropdown -->
        <div class="form-row">
            <label for="document-type">Select Document</label>
            <asp:DropDownList ID="documentType" runat="server" CssClass="document-select" style="width:235px;">
                <asp:ListItem Value="10">10th Certificate</asp:ListItem>
                <asp:ListItem Value="12">12th Certificate</asp:ListItem>
                <asp:ListItem Value="Diploma">Diploma Certificate</asp:ListItem>
                <asp:ListItem Value="Graduation">Graduation Certificate</asp:ListItem>
                <asp:ListItem Value="postGraduation">Post Graduation Certificate</asp:ListItem>
                <asp:ListItem Value="aadhar">Aadhar Card</asp:ListItem>
                <asp:ListItem Value="pan">Pan Card</asp:ListItem>
            </asp:DropDownList>
        </div>
        <!-- File Upload Section -->
        <div class="form-row">
            <label for="document-type">Select File</label>
            <asp:FileUpload ID="fileUpload" runat="server" style="width:235px;" />
        </div>
        <div class="form-row form-buttons">
            <asp:LinkButton ID="btnUpload" runat="server" OnClick="btnUpload_Click" Style="background-color:#1ed085; color:White; padding:10px 10px; border-radius:50%; margin-left:120px;">
                <i class="fa fa-upload"></i>
            </asp:LinkButton>
        </div>
        <hr />
        <div class="form-row guidelines" style="margin-top:20px;">
            <ul>
                <li>You can upload only in .pdf format.</li>
                <li>Each document must be less than 200 KB.</li>
            </ul>
        </div>
    </div>

    <!-- Right Section -->
    <div id="right" style="flex: 1; max-width: 45%;">
        <!-- Uploaded Documents Table -->
        <div class="form-row">
            <asp:Panel ID="docPanel" runat="server" Visible="False" CssClass="uploaded-documents">
                <table style="width: 100%; border-collapse: collapse;">
                    <thead style="background-color: #1ed085; border: 2px solid #1ed085; color: black;">
                        <tr>
                            <th>Sr. No.</th>
                            <th>Document Name</th>
                            <th>Download</th>
                            <th>Delete</th>
                        </tr>
                    </thead>
                    <tbody style="background-color: #fff; border: 2px solid #1ed085; color: black;">
                        <asp:Repeater ID="docRepeater" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Container.ItemIndex + 1 %></td>
                                    <td><%# Eval("Docxname") %></td>
                                    <td>
                                        <asp:LinkButton ID="lnkDownload" CssClass="show-btn" runat="server" CommandArgument='<%# Eval("Docxpath") %>' OnClick="btnDownload_Click">
                                            <i class="fa fa-download"></i>
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("Studocxid") %>' OnClick="DeleteDocument">
                                            <i class="fa fa-times delete-icon"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </asp:Panel>
        </div>
    </div>
</div>

             <asp:HiddenField ID="HiddenDocumentsStatus" runat="server" Value="0" />
            <div class="form-buttons">
    <asp:Button ID="btnDocumentPrev" runat="server" Text="Previous" CssClass="menu-item active" Onclick="btnDocumentPrev_Click" />
        <asp:Button ID="btnDocumentNext" runat="server" Text="Next" CssClass="menu-item active" Onclick="btnDocumentNext_Click" />
    </div>
        </div>
  


<div id="PaymentData"  >
            <div class="payment-form" id="PaymentSectionMain" class="tabcontent" style="display:none;">
                <h2>Payment Details</h2>
                <div class="form-row">
                    <label for="ddlPaymentMethod">Select Payment Method</label>
                    <asp:DropDownList ID="ddlPaymentMethod" runat="server">
                        <asp:ListItem Text="Credit/Debit Card" Value="credit-card" />
                        <asp:ListItem Text="UPI" Value="upi" />
                        <asp:ListItem Text="Net Banking" Value="net-banking" />
                        <asp:ListItem Text="Wallet" Value="wallet" />
                    </asp:DropDownList>
                </div>

                <div class="form-row">
                    <label for="txtCardNumber">Card Number</label>
                    <asp:TextBox ID="txtCardNumber" runat="server" MaxLength="19" placeholder="1234 5678 9123 4567"></asp:TextBox>
                </div>
                <div class="form-row">
                    <label for="txtExpiry">Expiry Date</label>
                    <asp:TextBox ID="txtExpiry" runat="server" MaxLength="5" placeholder="MM/YY"></asp:TextBox>
                </div>
                <div class="form-row">
                    <label for="txtCVV">CVV</label>
                    <asp:TextBox ID="txtCVV" runat="server" MaxLength="3" placeholder="123"></asp:TextBox>
                </div>
                <div class="form-row">
                    <label for="txtCardName">Cardholder Name</label>
                    <asp:TextBox ID="txtCardName" runat="server" placeholder="John Doe"></asp:TextBox>
                </div>

                <div class="form-buttons">
                <asp:Button ID="Button3" runat="server" Text="Previous" CssClass="menu-item active" Onclick="btnPaymentPrev_Click" />
                    <asp:Button ID="btnPayApplicationFee" runat="server" Text="Pay Application Fee" CssClass="menu-item active"  OnClick="btnPayApplicationFee_Click" />
                </div>
                <asp:HiddenField ID="HiddenPaymentStatus" runat="server" Value="0" />
            </div>

                <!-- Fee Already Submitted Message -->
                
        <div id="FeeSubmittedMessage" class="tabcontent" style="display:none;" >
            <div class="confirmation-container" >
                <div class="confirmation-message">
                    <h1>Fee Already Submitted Successfully</h1>
                    <p>Go to preview and submit your Application.</p>
                </div>
                  
            </div>
          <div class="form-buttons">
    <asp:Button ID="btnPaymentPrev" runat="server" Text="Previous" CssClass="menu-item active" Onclick="btnPaymentPrev_Click" />
                    <asp:Button ID="btnPreviewApplication" runat="server" CssClass="menu-item active"  Text="Preview Application" OnClick="btnPreviewApplication_Click" />
                </div>
        </div>

       
</div>

    

    </div>
</div>
</div>

<div id = "PersonalInstructions" style="display: none;">
<div style="position: fixed; top: 30%; right: -1%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
    <h4 style="text-align: left; color: #555; margin-bottom: 8px; font-size: 16px;">Personal Details Instructions&nbsp;<i class="fa fa-exclamation-circle"></i></h4>
<ul style="list-style-type: '*'; padding-left: 16px; line-height: 1.6; font-size: 12px; color: teal; margin: 0;font-weight:bold;">
         <li>Fill all the details carefully.</li>
    <li>Click "Save" to save data successfully.</li>
    <li>Click "Next" to navigate to the next section.</li>
    <li>You can navigate from above title buttons also.</li>
        <li>Double-check your submissions.</li>
    <li>Maintain clarity and precision.</li>

    
    </ul>
</div>
</div>
<div id="educationalInstruction" style="display: none;">
<div style="position: fixed; top: 30%; right: -1%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
  <h4 style="text-align: left; color: #555; margin-bottom: 8px; font-size: 16px;">Educational Details Instructions&nbsp;<i class="fa fa-exclamation-circle"></i></h4>
<ul style="list-style-type: '*'; padding-left: 16px; line-height: 1.6; font-size: 12px; color: teal; margin: 0;font-weight:bold;">
    <li>Click on "+Add New Education" to add new education details.</li>
    <li>Fill all the education details carefully.</li>
    <li>Click "Save" to save the education details.</li>
    <li>If you want to delete education then click on "Delete".</li>
    <li>Click "Next" or "Previous" to navigate.</li>
<%--    <li>Communicate proactively.</li>--%>
</ul>

</div>
</div>
<div id="photoSignInstruction" style="display: none;">
<div style="position: fixed; top: 30%; right: -1%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
    <h4 style="text-align: left; color: #555; margin-bottom: 8px; font-size: 16px;">Photo/Sign Details Instructions&nbsp;<i class="fa fa-exclamation-circle"></i></h4>
<ul style="list-style-type: '*'; padding-left: 16px; line-height: 1.6; font-size: 12px; color: teal; margin: 0;font-weight:bold;">
        <li>This photo is reflected to your main profile photo and application form.</li>
        <li>Upload only jpeg/jpg/png.</li>
        <li>No other files are allowed.</li>
        <li>Images size must be less then 200KB.</li>
        <li>Click on "Save" to save the images.</li>
        <li>Click "Next" or "Previous" to navigate.</li>
    </ul>
</div>
</div> 
<div id="contactInstructions" style="display: none;">
<div style="position: fixed; top: 30%; right: -1%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
   <h4 style="text-align: left; color: #555; margin-bottom: 8px; font-size: 16px;">Contact Details Instructions&nbsp;<i class="fa fa-exclamation-circle"></i></h4>
<ul style="list-style-type: '*'; padding-left: 16px; line-height: 1.6; font-size: 12px; color: teal; margin: 0;font-weight:bold;">
        <li>Fill all the details carefully.</li>
        <li>Double-check your submissions.</li>
        <li>Click on the check box if Residential is same as Permanent.</li>
        <li>Click on "Clear Residential" to clear residential details.</li>
        <li>Click on "Clear Permanent " to clear Permanent  details.</li>
        <li>Click on "Save" to save the details.</li>
    </ul>
</div>
</div>
<div id="documentInstructions" style="display: none;">
<div style="position: fixed; top: 30%; right: -1%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
   <h4 style="text-align: left; color: #555; margin-bottom: 8px; font-size: 16px;">Document Details Instructions&nbsp;<i class="fa fa-exclamation-circle"></i></h4>
<ul style="list-style-type: '*'; padding-left: 16px; line-height: 1.6; font-size: 12px; color: teal; margin: 0;font-weight:bold;">
        <li>Please select the respected documnet and then cick on "Choose File".</li>
        <li>Click on upload icon to upload the document.</li>
        <li>You can upload only in .pdf format.</li>
        <li>Each document must be less than 200 KB.</li>
        <li>Click on download icon to download the document.</li>
        <li>Click on cross icon to delete the document.</li>
    </ul>
</div>
</div>
<div id="paymentInstructions" style="display: none;" >
<div style="position: fixed; top: 30%; right: -1%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
   <h4 style="text-align: left; color: #555; margin-bottom: 8px; font-size: 16px;">Payment Details Instructions&nbsp;<i class="fa fa-exclamation-circle"></i></h4>
<ul style="list-style-type: '*'; padding-left: 16px; line-height: 1.6; font-size: 12px; color: teal; margin: 0;font-weight:bold;">
        <li>Click on pay to pay the application fee.</li>
        <li>After successfull completion application fee payment click on "Preview Application".</li>
        <li>Click on the check box if Residential is same as Permanent.</li>
        <li>After going on Application Preview Form check all the details precisely and then click on "Submit" to submit application.</li>
    </ul>
</div>
</div>
        </div>
           <div id="confirmationMessage"  runat="server" style="display:none;">
        
         <div class="confirmation-container"  >
        <div class="confirmation-message">
            <h1>Application Form Submitted Successfully</h1>
            <p>We are currently reviewing your application. Please wait for further verification.</p>
        </div>
    </div>
        </div> 
    </form>

    <!-- Footer -->
    <footer>
   All Rights Reserved. <i class="fa-regular fa-copyright fa-spin"></i> 2024 Saral ERP Solutions Pvt Ltd. 
    </footer>
       <script type="text/javascript">
           // Simulate a load time and hide the loader
           window.onload = function () {
               setTimeout(function () {
                   document.getElementById("loader").style.display = "none";
                   document.getElementById("content").style.display = "block";
               }, 2000); // Loader will display for 2 seconds
           };
    </script>
    <script>
        // Function to copy Residential Address to Permanent Address
//        function copyAddress() {
//            if (document.getElementById("sameAddress").checked) {
//                // Copy values from residential to permanent fields
//                document.getElementById("permAddress").value = document.getElementById("resAddress").value;
//                document.getElementById("permCountry").value = document.getElementById("resCountry").value;
//                document.getElementById("permState").value = document.getElementById("resState").value;
//                document.getElementById("permCity").value = document.getElementById("resCity").value;
//                document.getElementById("permPinCode").value = document.getElementById("resPinCode").value;

//                // Set the text field as non-editable (read-only)
//                document.getElementById("permAddress").readOnly = true;
//                document.getElementById("permPinCode").readOnly = true;

//                // Set the dropdowns as non-editable (disabled)
//                document.getElementById("permCountry").disabled = true;
//                document.getElementById("permState").disabled = true;
//                document.getElementById("permCity").disabled = true;


//            } else {
//                document.getElementById("permAddress").value = "";
//                document.getElementById("permCountry").value = "";
//                document.getElementById("permState").value = "";
//                document.getElementById("permCity").value = "";
//                document.getElementById("permPinCode").value = "";

//                document.getElementById("permAddress").readOnly = false;
//                document.getElementById("permPinCode").readOnly = false;

//                // Set the dropdowns as non-editable (disabled)
//                document.getElementById("permCountry").disabled = false;
//                document.getElementById("permState").disabled = false;
//                document.getElementById("permCity").disabled = false;
//            }
//        }

        // Function to clear Residential Address fields
        function clearResidentialAddress() {
            document.getElementById("resAddress").value = "";
            document.getElementById("resCountry").value = "";
            document.getElementById("resState").value = "";
            document.getElementById("resCity").value = "";
            document.getElementById("resPinCode").value = "";
            document.getElementById("sameAddress").checked = false;
            //            const checkbox = document.getElementById('sameAddress');
            //            // Set the checked attribute to false
            //            checkbox.setAttribute('checked', false);
        }

        function clearPermanetAddress() {
            document.getElementById("permAddress").value = "";
            document.getElementById("permCountry").value = "";
            document.getElementById("permState").value = "";
            document.getElementById("permCity").value = "";
            document.getElementById("permPinCode").value = "";
            document.getElementById("sameAddress").checked = false;
            //            const checkbox = document.getElementById('sameAddress');
            //            // Set the checked attribute to false
            //            checkbox.setAttribute('checked', false);
        }
</script>
   <script>
    function openTab(evt, tabName, stepIndex) {
        // Hide all tab contents
        var tabcontents = document.getElementsByClassName("tabcontent");
        for (var i = 0; i < tabcontents.length; i++) {
            tabcontents[i].style.display = "none";
        }

        // Show the clicked tab content
        document.getElementById(tabName).style.display = "block";

        // Display the corresponding instructions based on stepIndex
        var instructions = [
            "PersonalInstructions",
            "educationalInstruction",
            "photoSignInstruction",
            "contactInstructions",
            "documentInstructions",
            "paymentInstructions"
        ];

        instructions.forEach((instruction, index) => {
            document.getElementById(instruction).style.display = (index === stepIndex) ? "block" : "none";
        });

        if (stepIndex === 5) {
            var paymentStatus = document.getElementById("HiddenPaymentStatus").value;

            if (paymentStatus === "1") {
                document.getElementById("PaymentSectionMain").style.display = "none";
                document.getElementById("FeeSubmittedMessage").style.display = "block";
            } else {
                document.getElementById("PaymentSectionMain").style.display = "block";
                document.getElementById("FeeSubmittedMessage").style.display = "none";
            }
        }

        // Update the progress steps and lines
        updateProgressBar(stepIndex);

        // Update the URL query string with the tab index
        const currentUrl = new URL(window.location.href);
        currentUrl.searchParams.set('tab', stepIndex); // Add or update the 'tab' parameter
        window.history.pushState({}, '', currentUrl.toString()); // Update the URL without reloading
    }

    function updateProgressBar(stepIndex) {
        var progressSteps = document.getElementsByClassName("progress-step");
        var progressLines = document.getElementsByClassName("progress-line");

        for (var i = 0; i < progressSteps.length; i++) {
            if (i <= stepIndex) {
                progressSteps[i].classList.add("active");
                progressSteps[i].innerHTML = (i < stepIndex) ? "&#10003;" : i + 1;
                if (i < progressLines.length) {
                    progressLines[i].style.backgroundColor = "#1ed085";
                }
            } else {
                progressSteps[i].classList.remove("active");
                progressSteps[i].innerHTML = i + 1;
                if (i < progressLines.length) {
                    progressLines[i].style.backgroundColor = "#ddd";
                }
            }
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        const urlParams = new URLSearchParams(window.location.search);
        const activeTab = parseInt(urlParams.get('tab')) || 0;
        const tabNames = [
            "PersonalData",
            "EducationalData",
            "PhotoSignData",
            "ContactData",
            "DocumentData",
            "PaymentData"
        ];
        const tabName = tabNames[activeTab] || tabNames[0];
        openTab(null, tabName, activeTab);
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

