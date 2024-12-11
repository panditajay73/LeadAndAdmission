<%@ Page Language="VB" AutoEventWireup="false" CodeFile="StudentDetails.aspx.vb" Inherits="AuthorityPages_StudentDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Student Application Details</title>
    <script src="https://kit.fontawesome.com/a429e5bfb6.js" crossorigin="anonymous"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
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
       body {
            background-color: #fff;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
        }
.container {
  max-width: 1000px;
  margin: 30px auto;
  padding: 20px;
  background-color: #fff;
  border-radius: 12px;
  border:1px solid grey;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  transition: transform 0.3s ease-in-out;
}

.container:hover {
  transform: scale(1.01);
}

header, .section, .footer {
  margin-bottom: 30px;
}

header h1 {
  font-size: 2.2em;
  margin-bottom: 10px;
  color: #004d40; /* Teal color for header text */
}

header p {
  font-size: 1em;
  color: #004d40; /* Teal color for header text */
}

.section h2 {
  font-size: 1.6em;
  color: #004d40; /* Teal color for section headers */
  margin-bottom: 15px;
  padding-bottom: 10px;
  position: relative;
}

.section h2::before {
  content: '';
  width: 5px;
  height: 100%;
  background-color: #004d40; /* Teal color for section header border */
  position: absolute;
  left: -20px;
  top: 0;
  border-radius: 5px;
}

.section h3 {
  font-size: 1.4em;
  color: #004d40; /* Teal color for subsection headers */
  margin-bottom: 10px;
}

.details-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px 0;
  border-bottom: 1px solid #e9ecef;
  
}

.details-row:last-child {
  border-bottom: none;
}

.details-row label {
  font-weight: bold;
  flex: 1;
  color: #004d40; /* Teal color for labels */
}

.details-row span {
  flex: 2;
  text-align: left;
  color: #212529;
}

.personal-details-container {
  display: flex;
  justify-content: space-between;
}

.personal-details-info {
  flex: 2;
}

.photo-sign-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  flex: 0.5;
}

.photo-sign-container img {
  width: 120px;
  height: 120px;
  border-radius: 6px;
  border: 2px solid #dee2e6;
  margin-bottom: 10px;
  object-fit: cover;
}

.photo-sign-container label {
  margin-bottom: 5px;
  font-weight: bold;
}

.education-block {
  margin-bottom: 20px;
  padding: 15px;
  border: 1px solid #ced4da;
  border-radius: 8px;
  background-color: #f1f3f5;
}

.document-list {
  list-style: none;
  padding: 0;
}

.document-list li {
  display: flex;
  justify-content: space-between;
  padding: 10px 0;
  border-bottom: 1px solid #e9ecef;
}

.document-list li:last-child {
  border-bottom: none;
}

.edit-btn, .show-btn, .back-btn, .submit-btn {
  display: inline-block;
  padding: 10px 20px;
  margin: 10px 0;
  font-size: 1em;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

.edit-btn, .show-btn {
  background-color: #004d40; /* Teal color for buttons */
  color: white;
}

.edit-btn:hover, .show-btn:hover {
  background-color: #003d33; /* Darker teal for hover effect */
}

.back-btn {
  background-color: #6c757d;
  color: white;
}

.back-btn:hover {
  background-color: #565e64;
}

.submit-btn {
  background-color: #28a745;
  color: white;
}

.submit-btn:hover {
  background-color: #218838;
}

@media (max-width: 768px) {
  .container {
    margin: 20px;
    padding: 15px;
  }

  header h1 {
    font-size: 1.8em;
  }

  header p {
    font-size: 0.9em;
  }

  .section h2 {
    font-size: 1.4em;
  }

  .section h3 {
    font-size: 1.2em;
  }

  .details-row {
    flex-direction: column;
    align-items: flex-start;
  }

  .details-row label {
    margin-bottom: 5px;
  }

  .details-row span {
    text-align: left;
  }

  .personal-details-container {
    flex-direction: column;
  }

  .photo-sign-container {
    flex-direction: row;
    justify-content: space-between;
  }

  .photo-sign-container img {
    width: 100px;
    height: 100px;
  }

  .edit-btn, .show-btn, .back-btn, .submit-btn {
    width: 100%;
    text-align: center;
  }
}

@media (max-width: 480px) {
  .container {
    padding: 10px;
  }

  header h1 {
    font-size: 1.5em;
  }

  header p {
    font-size: 0.8em;
  }

  .section h2 {
    font-size: 1.2em;
  }

  .section h3 {
    font-size: 1.1em;
  }

  .edit-btn, .show-btn, .back-btn, .submit-btn {
    font-size: 0.9em;
  }
}

.topic-header {
  display: flex; 
  justify-content: space-between;
  border-bottom: 2px solid #e9ecef;
}
.details-row1 label
{
    font-weight:bold;
    }
    .details-row1{padding:10px 10px;}
    
    .details-row-address {
  display: flex;
gap:10px;
  padding: 10px 0;
  border-bottom: 1px solid #e9ecef;
  width:400px;
}
.details-row-address label{font-weight:bold;font-size:15px;}
#documents {
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

#documents h2 {
  margin-bottom: 20px;
  font-size: 24px;
  color: #333;
}

.document-item {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 10px;
  margin-bottom: 10px;
  background-color: #fff;
  border: 1px solid #ddd;
  border-radius: 4px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
}

.document-item:hover {
  background-color: #f1f1f1;
}

.document-name {
  font-size: 18px;
  color: #555;
}
.profile-container {
  padding:0px 20px;
  background-color: #f5f5f5;
  border-radius: 8px;
  transition: max-height 0.5s ease-out;
  overflow: hidden;
  max-height: none;
}
.profile-content {
  display: flex;
  align-items: flex-start;
}

.photo-sign-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-right: 20px;
}

.profile-photo {
  border-radius: 8px;
  border: 1px solid #ddd;
}

.profile-signature {
  margin-top: 10px;
  border-radius: 8px;
  border: 1px solid #ddd;
}

.details-container {
  flex: 1;
}

.details-row {
  margin-bottom: 15px;
  display: flex;
  align-items: center;
}

.details-label {
  font-weight: bold;
  margin-right: 10px;
  width: 200px;
  color: #333;
}

.details-value {
  color: #555;
}
.back-link {
            color: #006666;
            text-decoration: none;
            font-weight: bold;
            font-size:25px;
        }

        .back-link:hover {
            color: #008080;
        }
          .btn-sexy-dull {
        background-color: #f5f5f5; /* Soft gray background */
        color: #999; /* Gray text */
        border: 1px solid #ccc; /* Light gray border */
        cursor: not-allowed; /* Change cursor to indicate disabled state */
        text-shadow: 0 1px 0 rgba(255, 255, 255, 0.75); /* Light text shadow for depth */
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* Subtle shadow for depth */
        border-radius: 4px; /* Rounded corners */
        padding: 8px 16px; /* Padding for better size */
        font-size: 17px; /* Font size */
        font-weight: 500; /* Font weight for a slight bold effect */
        transition: background-color 0.3s, color 0.3s, border-color 0.3s; /* Smooth transition for color changes */
    }
    
    .btn-dull:hover {
        background-color: #e0e0e0; /* Slightly darker gray on hover */
        color: #888; /* Slightly darker text on hover */
    }

    .btn-dull:focus {
        outline: none; /* Remove default focus outline */
    }
    @media print {
        #printPage {
            padding: 200px; /* Add your desired padding here */
        }
    }
    .no-scroll {
        overflow: hidden;
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
   <script>
       function printDiv(printPage) {
           var divContent = document.getElementById(printPage).innerHTML;
           var originalContent = document.body.innerHTML;

           // Change content
           document.body.innerHTML = divContent;

           // Print the content
           window.print();

           // Restore the original content
           document.body.innerHTML = originalContent;

           return false;
       }
</script>
<script>
    // Object to track visibility of each section
    const sectionVisibility = {
        profile: true,
        payments: true,
        educations: true,
        addresses: true,
        docs: true
    };

    window.onload = function() {
        // Initialize maxHeight for all sections
        ['profile', 'payments', 'educations', 'addresses', 'docs'].forEach(sectionId => {
            const section = document.getElementById(sectionId);
            section.style.maxHeight = section.scrollHeight + 'px';  // Set initial maxHeight based on content
        });
    };

    function toggleSection(sectionId) {
        const section = document.getElementById(sectionId);
        const toggleIcon = document.getElementById(`toggle-icon-${sectionId}`);

        if (sectionVisibility[sectionId]) {
            // Hide section content
            section.style.maxHeight = '0';
            section.style.overflow = 'hidden';
            toggleIcon.classList.remove('fa-chevron-up');
            toggleIcon.classList.add('fa-chevron-down');
        } else {
            // Show section content
            section.style.maxHeight = section.scrollHeight + 'px';
            toggleIcon.classList.remove('fa-chevron-down');
            toggleIcon.classList.add('fa-chevron-up');
        }

        sectionVisibility[sectionId] = !sectionVisibility[sectionId];
    }
</script>
</head>
<body>
   <form id="form1" runat="server">
   <div id="StudentDetailsInstructions" runat="server">
<div style="position: fixed; top: 10%; right: 0%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
    <small style ="position:absolute ;" > Instructions !<br /></br>
    <i class="fa-solid fa-star" style="color: #1ed085;"></i>
  View applicant and his details.</br>  
     <i class="fa-solid fa-star" style="color: #1ed085;"></i> &nbsp;"Verify" promotes applicants to the next step: New Application → Document Verified → Registration Approved → Admission Approved.</br>
       <i class="fa-solid fa-star" style="color: #1ed085;"></i> Use the remarks textbox for feedback.</br>
</small>
</div>
</div>
    <!-- Application Preview Container -->
    <div class="container">
    
    <div id="customAlert" class="custom-alert">
    <i class="fa fa-exclamation-triangle"></i>
    <span id="customAlertMessage">This is a custom alert!</span>
    <span class="close-btn" onclick="closeCustomAlert()">&times;</span>
    </div>
    <header>
    <div Style="display:flex;justify-content:space-between;">
    <div style="display:flex;align-items:center;">
    <a href="#" class="back-link" onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=All'"; return false;">
    <span><i class="fa-solid fa-arrow-left"></i></span>
</a>
      <h1 style="margin-left:20px;">Student Details <span style="font-size:25px;">( <asp:Label ID="UserStatusLabel" runat="Server" ForeColor="Green"></asp:Label> )</span></h1>
    </div>
    <div>
    <asp:LinkButton ID="lnkPrint" runat="server" style="color:Black;font-size:25px;" OnClientClick="printDiv('div_print'); return false;">
            <i class="fa fa-print"></i> 
        </asp:LinkButton>
    </div>
    </div>
    
      <p>Please review students details before verify.</p>
    </header>
<div id="div_print">
    <section id="personal-details" class="section">
      <div class="topic-header">
        <h2  onclick="toggleSection('profile')" style="  cursor:pointer;">Personal Details <i id="toggle-icon-profile" class="fa fa-chevron-up" style="font-size:18px;"></i></h2>
      </div>
     <div class="profile-container" id="profile" style="transition: max-height 0.5s ease-out;
  overflow: hidden;
  max-height: none;">
  <table style="width: 100%; table-layout: fixed;">
    <tr>
      <!-- Left Column: Details -->
      <td style="width: 70%; vertical-align: top; padding-right: 20px;">
        <table style="width: 100%;">
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Registration No:</td>
            <td><asp:Label ID="RegistrationNo" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Name:</td>
            <td><asp:Label ID="Name" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Email:</td>
            <td><asp:Label ID="txtEmail" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Mobile:</td>
            <td><asp:Label ID="Mobile" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Apply For:</td>
            <td><asp:Label ID="ApplyFor" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Gender:</td>
            <td><asp:Label ID="Gender" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Date of Birth:</td>
            <td><asp:Label ID="DateOfBirth" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Father's Name:</td>
            <td><asp:Label ID="FatherName" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Mother's Name:</td>
            <td><asp:Label ID="MotherName" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Guardian's Mobile:</td>
            <td><asp:Label ID="GuardianMobile" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Religion:</td>
            <td><asp:Label ID="Religion" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
          <tr>
            <td style="font-weight: bold; padding: 5px 0;">Category:</td>
            <td><asp:Label ID="Category" runat="server" CssClass="details-value" style="color: #333;"></asp:Label></td>
          </tr>
        </table>
      </td>

      <!-- Right Column: Photo and Signature -->
      <td style="width: 30%; text-align: center; vertical-align: top;">
        <asp:Image ID="imgPhoto" runat="server" Height="150" Width="150" ImageUrl="images/sample-photo.jpg" CssClass="profile-photo" style="margin-bottom: 20px;" />
        <asp:Image ID="imgSignature" runat="server" Height="50" Width="150" ImageUrl="images/sample-signature.png" CssClass="profile-signature" />
      </td>
    </tr>
  </table>
</div>


    </section>
    </script>
    <section id="payment-details" class="section">
      <div class="topic-header" >
        <h2 onclick="toggleSection('payments')" style=" cursor:pointer;">Payment Details <i id="toggle-icon-payments" class="fa fa-chevron-up" style="font-size:18px;"></i></h2>
      </div>
 <div id="payments" style="transition: max-height 0.5s ease-out;
  overflow: hidden;
  max-height: none;">
          <div style="border: 2px solid #ddd; padding: 20px; border-radius: 10px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); background-color: #f9f9f9;margin-top:20px;">
   <div class="details-row" 
       style="display: flex; justify-content: space-between; margin-bottom: 15px;">
    <label style="font-weight: bold; color: #555;">Transaction ID:</label>
    <asp:Label ID="lblTransactionID" runat="server" style="color: #007BFF; font-size: 16px;"></asp:Label>
  </div>
  <hr style="border: none; border-top: 1px solid #ddd; margin: 10px 0;" />
  <div class="details-row" 
       style="display: flex; justify-content: space-between; margin-bottom: 15px;">
    <label style="font-weight: bold; color: #555;">Amount Paid:</label>
    <asp:Label ID="lblAmountPaid" runat="server" style="color: #28A745; font-size: 16px;"></asp:Label>
  </div>
  <hr style="border: none; border-top: 1px solid #ddd; margin: 10px 0;" />
  <div class="details-row" 
       style="display: flex; justify-content: space-between; margin-bottom: 15px;">
    <label style="font-weight: bold; color: #555;">Payment Date:</label>
    <asp:Label ID="lblPaymentDate" runat="server" style="color: #6C757D; font-size: 16px;"></asp:Label>
  </div>
  </div>
      </div> 
    </section>

   <section id="education-details" class="section">
   <div class="topic-header">
        <h2  onclick="toggleSection('educations')" style=" cursor:pointer;">Education Details <i id="toggle-icon-educations" class="fa fa-chevron-up" style="font-size:18px;"></i></h2>
      </div>
  <div id="educations" style="transition: max-height 0.5s ease-out;
  overflow: hidden;
  max-height: none;">
  <asp:Repeater ID="rptEducationDetails" runat="server">
    <ItemTemplate>
        <div style="display: flex; flex-wrap: wrap; gap: 20px; border: 2px solid #ddd; padding: 20px; border-radius: 10px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); background-color: #f9f9f9;margin-top:20px;">
            
            <!-- Column 1 -->
            <div style="flex: 1; min-width: 300px;">
                <div style="margin-bottom: 15px;">
                    <label style="font-weight: bold; color: #333;">Education Name: </label>
                    <span style="color: #555;"><%# Eval("EducationName") %></span>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="font-weight: bold; color: #333;">Roll No: </label>
                    <span style="color: #555;"><%# Eval("RollNo") %></span>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="font-weight: bold; color: #333;">Year of Passing: </label>
                    <span style="color: #555;"><%# Eval("YearOfPassing") %></span>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="font-weight: bold; color: #333;">College/University Name: </label>
                    <span style="color: #555;"><%# Eval("CollegeName") %></span>
                </div>
            </div>

            <!-- Column 2 -->
            <div style="flex: 1; min-width: 300px;">
                <div style="margin-bottom: 15px;">
                    <label style="font-weight: bold; color: #333;">Total Marks: </label>
                    <span style="color: #555;"><%# Eval("TotalMarks") %></span>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="font-weight: bold; color: #333;">Obtained Marks: </label>
                    <span style="color: #555;"><%# Eval("ObtainedMarks") %></span>
                </div>
                <div style="margin-bottom: 15px;">
                    <label style="font-weight: bold; color: #333;">Percentage: </label>
                    <span style="color: #555;"><%# Eval("Percentage") %>%</span>
                </div>
            </div>
        </div>
    </ItemTemplate>
</asp:Repeater>
  </div>
  
</section>


<section id="contact-details" class="section">
   <div class="topic-header">
          <h2  onclick="toggleSection('addresses')" style=" cursor:pointer;">Contact Details <i id="toggle-icon-addresses" class="fa fa-chevron-up" style="font-size:18px;"></i></h2>
      </div>

  <div style="display:flex; transition: max-height 0.5s ease-out; overflow: hidden; max-height: none;" id="addresses">
    <div style="padding:10px 50px;">
        <h3>Residential Address</h3>
        <asp:Repeater ID="rptResidentialAddress" runat="server">
            <ItemTemplate>
                <div class="details-row-address"><label>Address:</label><asp:Label ID="lblResAddress" runat="server" Text='<%# Eval("ResidentialAddress") %>'></asp:Label></div>
                <div class="details-row-address"><label>Country:</label><asp:Label ID="lblResCountry" runat="server" Text='<%# Eval("ResidentialCountry") %>'></asp:Label></div>
                <div class="details-row-address"><label>State:</label><asp:Label ID="lblResState" runat="server" Text='<%# Eval("ResidentialState") %>'></asp:Label></div>
                <div class="details-row-address"><label>City:</label><asp:Label ID="lblResCity" runat="server" Text='<%# Eval("ResidentialCity") %>'></asp:Label></div>
                <div class="details-row-address"><label>Pincode:</label><asp:Label ID="lblResPincode" runat="server" Text='<%# Eval("ResidentialPincode") %>'></asp:Label></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <div style="padding:10px 50px;">
        <h3>Permanent Address</h3>
        <asp:Repeater ID="rptPermanentAddress" runat="server">
            <ItemTemplate>
                <div class="details-row-address"><label>Address:</label><asp:Label ID="lblPermAddress" runat="server" Text='<%# Eval("PermanentAddress") %>'></asp:Label></div>
                <div class="details-row-address"><label>Country:</label><asp:Label ID="lblPermCountry" runat="server" Text='<%# Eval("PermanentCountry") %>'></asp:Label></div>
                <div class="details-row-address"><label>State:</label><asp:Label ID="lblPermState" runat="server" Text='<%# Eval("PermanentState") %>'></asp:Label></div>
                <div class="details-row-address"><label>City:</label><asp:Label ID="lblPermCity" runat="server" Text='<%# Eval("PermanentCity") %>'></asp:Label></div>
                <div class="details-row-address"><label>Pincode:</label><asp:Label ID="lblPermPincode" runat="server" Text='<%# Eval("PermanentPincode") %>'></asp:Label></div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
</section>


<section  class="section">
<div class="topic-header">
            <h2  onclick="toggleSection('documents')" style=" cursor:pointer;">Documents Details <i id="toggle-icon-documents" class="fa fa-chevron-up" style="font-size:18px;"></i></h2>
      </div>
<div id="documents" style="transition: max-height 0.5s ease-out;
  overflow: hidden;
  max-height: none;padding:0px 20px;">
  <asp:Repeater ID="rptDocuments" runat="server">
    <ItemTemplate>
        <div class="document-item" style="display: flex; justify-content: space-between; align-items: center; padding: 10px; border: 1px solid #ddd; border-radius: 8px; margin-bottom: 10px; background-color: #f9f9f9;margin-top:10px;">
    <span class="document-name" style="font-weight: bold; color: #333; font-size: 17px;">
        <%# Eval("DocumentName") %>
    </span>
    <div style="display: flex; gap: 15px; align-items: center;">
        <!-- Download Link -->
        <asp:LinkButton ID="lnkDownload" CssClass="action-link" runat="server" CommandArgument='<%# Eval("DocumentPath") %>' OnClick="btnDownload_Click">
            <i class="fa fa-download" style="margin-right: 5px; color: #007bff;"></i>
        </asp:LinkButton>

        <!-- Show Link -->
        <asp:LinkButton ID="lnkShow" CssClass="action-link" runat="server" CommandArgument='<%# Eval("DocumentPath") %>' OnClick="btnShow_Click">
            <i class="fa fa-eye" style="margin-right: 5px; color: #28a745;"></i>
        </asp:LinkButton>
    </div>
</div>

    </ItemTemplate>
</asp:Repeater>

</div>

</section>


</div>
   <div class="footer" runat="server" id="footerT">
   <h4>Actions:</h4>
    <div class="search-row" style="display:flex;justify-content:space-between;gap:20px;">
                <asp:DropDownList ID="ddlAction" runat="server" CssClass="form-control" ToolTip="Select Action">
                    <asp:ListItem Value="Verify">Verify</asp:ListItem>
                    <asp:ListItem Value="Reject">Reject</asp:ListItem>
                    <asp:ListItem Value="Update">Update</asp:ListItem>
                </asp:DropDownList>

                <asp:TextBox ID="remarks" runat="server" CssClass="form-control" placeholder="Enter Remarks..." ToolTip="Enter Feedback/Remarks"></asp:TextBox>

                <asp:Button ID="btnApprove" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnApprove_Click"/>
            </div>
</div>

  </div>
  <script>
      function printPage() {
          window.print();
      }
  </script>
</form>
    </script>
       <%-- <script type="text/javascript" src="//translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>--%>
</body>
</html>