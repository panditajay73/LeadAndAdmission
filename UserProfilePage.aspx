<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserProfilePage.aspx.vb" Inherits="UserProfilePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>User Profile</title>
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap" rel="stylesheet">
    <script src="https://kit.fontawesome.com/a429e5bfb6.js" crossorigin="anonymous"></script>
<link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>

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
        </div>--%>
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
        <div class="content" style="background-color:#fff;">
        <div class="left-content" style="background-color:#fff;">
         <h1 style="text-decoration:underline;margin-bottom:20px;text-align:center;">Edit Profile</h1>
    <div class="profile-section" style="width:98%;">
    <div class="profile-container">
        <h2 style="margin-bottom:20px;"></h2>

        <!-- Menu Bar for Personal Details and Change Password -->
        <div class="menu-bar">
    <button class="menu-item active" onclick="openTab(event, 'PersonalData', 0)" type="button">Personal Details</button>
    <button class="menu-item" onclick="openTab(event, 'EducationalData', 1)" type="button">Educational Details</button>
    <button class="menu-item" onclick="openTab(event, 'PhotoSignData', 2)" type="button">Photo/Sign Upload</button>
    <button class="menu-item" onclick="openTab(event, 'ContactData', 3)" type="button">Contact Details</button>
</div>
<hr />
<br />
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
                            <label for="Gender">Gender</label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="DOB">Date of Birth</label>
                            <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="FatherName">Father's Name</label>
                            <asp:TextBox ID="txtFatherName" runat="server" placeholder="Enter Father Name" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Second Column -->
                    <div class="form-column">
                        <div class="form-group">
                            <label for="MotherName">Mother's Name</label>
                            <asp:TextBox ID="txtMotherName" runat="server" placeholder="Enter Mother Name" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="GuardianMobile">Guardian's Mobile No</label>
                            <asp:TextBox ID="txtGuardianMobile" runat="server" placeholder="Guardian's Mobile No" CssClass="form-control"></asp:TextBox>
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
                            <label for="Religion">Religion</label>
                            <asp:DropDownList ID="ddlReligion" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="Category">Category</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"></asp:DropDownList>
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

                <asp:HiddenField ID="hfFormStatusProfile" runat="server" Value="0" />
                <%--<asp:Label ID = "label1" runat="server" Text="AJAY"></asp:Label>--%>
                <!-- Save Button -->
                <div style="width: 100%; text-align: center;">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="menu-item active" Onclick="btnPersonalDetailsSave_Click" />
                    <asp:Button ID="btnPersonalNext" runat="server" Text="Next" CssClass="menu-item active" Onclick="btnPersonalNext_Click" />
                </div>

            </div>
            <script>
    // Function to ensure that only numbers are allowed for mobile input fields
    function allowOnlyNumbers(input) {
        input.value = input.value.replace(/[^0-9]/g, ''); // Replace non-digit characters
    }

    // Function to validate the mobile number length
    function validateMobileLength(input) {
        if (input.value.length > 10) {
            alert("Mobile number cannot be greater than 10 digits.");
            input.value = input.value.slice(0, 10); // Trim the value to 10 digits
        }
    }

    // Function to prevent numbers in text fields
    function preventNumbersInText(input) {
        input.value = input.value.replace(/[0-9]/g, ''); // Remove numeric characters
    }

    // Attach event listeners to relevant fields when the page loads
    window.onload = function () {
        // Get the elements for the mobile number fields
        const mobileField = document.getElementById('<%= txtMobile.ClientID %>');
        const guardianMobileField = document.getElementById('<%= txtGuardianMobile.ClientID %>');

        // Get the elements for text fields where numbers should not be allowed
        const fullNameField = document.getElementById('<%= txtFullName.ClientID %>');
        const fatherNameField = document.getElementById('<%= txtFatherName.ClientID %>');
        const motherNameField = document.getElementById('<%= txtMotherName.ClientID %>');

        // Add event listeners for mobile number validation (numeric input and length check)
        mobileField.addEventListener('input', function() {
            allowOnlyNumbers(mobileField);
            validateMobileLength(mobileField);
        });
        
        guardianMobileField.addEventListener('input', function() {
            allowOnlyNumbers(guardianMobileField);
            validateMobileLength(guardianMobileField);
        });

        // Add event listeners for text fields (prevent numbers)
        fullNameField.addEventListener('input', function() {
            preventNumbersInText(fullNameField);
        });
        
        fatherNameField.addEventListener('input', function() {
            preventNumbersInText(fatherNameField);
        });
        
        motherNameField.addEventListener('input', function() {
            preventNumbersInText(motherNameField);
        });

        resPinCode.addEventListener('input', function() {
            allowOnlyNumbers(resPinCode);
        });
        permPinCode.addEventListener('input', function() {
            allowOnlyNumbers(permPinCode);
        });
    };
</script>
    
</div>




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
</script>
    <!-- Buttons -->
    <div class="form-buttons">
      <%--<asp:Button ID="btnEducationPrev" runat="server" Text="Previous" CssClass="menu-item active" Onclick="btnEducationPrev_Click" />--%>
              <asp:Button ID="Button4" runat="server" Text="Previous" OnClick="btnEducationPrev_Click" CssClass="menu-item active"/>
        <asp:Button ID="Button1" runat="server" Text="Save" OnClick="SaveEducation" CssClass="menu-item active"/>
        <asp:Button ID="Button3" runat="server" Text="Next" OnClick="btnEducationNext_Click" CssClass="menu-item active"/>

       <%-- <asp:Button ID="btnEducationNext" runat="server" Text="Next" CssClass="menu-item active" Onclick="btnEducationNext_Click" />--%>
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
        <asp:Button ID="Button5" runat="server" Text="Previous" CssClass="menu-item active" OnClick="btnPhotoSignPrev_Click" />
            <asp:Button ID="Button2" runat="server" Text="Save" CssClass="menu-item active" OnClick="btnPhotoSignSave_Click" />
            <asp:Button ID="Button6" runat="server" Text="Next" CssClass="menu-item active" OnClick="btnPhotoSignNext_Click" />
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
        <%--<asp:Button ID="btnContactNext" runat="server" Text="Next" CssClass="menu-item active" Onclick="btnContactNext_Click" />--%>
    </div>

    <asp:HiddenField ID="HiddenContactStatus" runat="server" Value="0" />
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

       
<%--       <div class="right-content">
        <h3>Authentication Status</h3>
        <div class="notification-item">
            <p>Email Verified: <strong style="color: green;">Yes</strong></p>
            <button class="verify-btn">Verified</button>
        </div>
        <div class="notification-item">
            <p>Phone Number Verified: <strong style="color: red;">No</strong></p>
            <button class="verify-btn">Verify Phone Number</button>
        </div>
    </div>--%>
        </div>
         <script type="text/javascript">
             // Simulate a load time and hide the loader
             window.onload = function () {
                 setTimeout(function () {
                     document.getElementById("loader").style.display = "none";
                     document.getElementById("content").style.display = "block";
                 }, 1000); // Loader will display for 2 seconds
             };
    </script>
<script>
    // Function to copy Residential Address to Permanent Address
//    function copyAddress() {
//        if (document.getElementById("sameAddress").checked) {
//            // Copy values from residential to permanent fields
//            document.getElementById("permAddress").value = document.getElementById("resAddress").value;
//            document.getElementById("permCountry").value = document.getElementById("resCountry").value;
//            document.getElementById("permState").value = document.getElementById("resState").value;
//            document.getElementById("permCity").value = document.getElementById("resCity").value;
//            document.getElementById("permPinCode").value = document.getElementById("resPinCode").value;

//            // Set the text field as non-editable (read-only)
//            document.getElementById("permAddress").readOnly = true;
//            document.getElementById("permPinCode").readOnly = true;

//            // Set the dropdowns as non-editable (disabled)
//            document.getElementById("permCountry").disabled = true;
//            document.getElementById("permState").disabled = true;
//            document.getElementById("permCity").disabled = true;


//        } else {
//            document.getElementById("permAddress").value = "";
//            document.getElementById("permCountry").value = "";
//            document.getElementById("permState").value = "";
//            document.getElementById("permCity").value = "";
//            document.getElementById("permPinCode").value = "";

//            document.getElementById("permAddress").readOnly = false;
//            document.getElementById("permPinCode").readOnly = false;

//            // Set the dropdowns as non-editable (disabled)
//            document.getElementById("permCountry").disabled = false;
//            document.getElementById("permState").disabled = false;
//            document.getElementById("permCity").disabled = false;
//        }
//    }

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
<!-- Script for handling tabs -->
<script>
// Function to handle the tab switching and updating the query string
function openTab(evt, tabName, tabIndex) {
    // Hide all tab contents
    var tabcontents = document.getElementsByClassName("tabcontent");
    for (var i = 0; i < tabcontents.length; i++) {
        tabcontents[i].style.display = "none";
    }

    // Hide all instruction divs
    var instructionDivs = document.querySelectorAll('[id$="Instructions"]');
    instructionDivs.forEach(function (div) {
        div.style.display = "none";
    });

    // Remove active class from all buttons
    var menuItems = document.getElementsByClassName("menu-item");
    for (var i = 0; i < menuItems.length; i++) {
        menuItems[i].classList.remove("active");
    }

    // Show the clicked tab content and add active class to the clicked button
    document.getElementById(tabName).style.display = "block";
    evt.currentTarget.classList.add("active");

    // Show the corresponding instruction div
    if (tabIndex == 0) {
        document.getElementById("PersonalInstructions").style.display = "block";
    } else if (tabIndex == 1) {
        document.getElementById("educationalInstruction").style.display = "block";
    } else if (tabIndex == 2) {
        document.getElementById("photoSignInstruction").style.display = "block";
    } else if (tabIndex == 3) {
        document.getElementById("contactInstructions").style.display = "block";
    }

    // Update the URL query string with the tab index
    const currentUrl = new URL(window.location.href);
    currentUrl.searchParams.set('tab', tabIndex); // Add or update the 'tab' parameter
    window.history.pushState({}, '', currentUrl.toString()); // Update the URL without reloading
}

// Function to open the tab based on the query string value
function openTabFromQuery() {
    const urlParams = new URLSearchParams(window.location.search);
    const tabIndex = parseInt(urlParams.get('tab')) || 0; // Default to tab 0 if no parameter is present

    // Map tab indexes to corresponding tab names
    const tabMap = {
        0: 'PersonalData',
        1: 'EducationalData',
        2: 'PhotoSignData',
        3: 'ContactData'
    };

    const tabName = tabMap[tabIndex];

    // Simulate a click on the corresponding tab button to open it
    const menuItems = document.querySelectorAll('.menu-item');
    if (menuItems[tabIndex]) {
        menuItems[tabIndex].click();
    }
}

// On page load, open the tab based on query string or default to PersonalData
document.addEventListener("DOMContentLoaded", function () {
    openTabFromQuery();
});


</script>
          
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
