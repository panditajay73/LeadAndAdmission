<%@ Page Language="VB" AutoEventWireup="true" CodeFile="AdmissionApprove.aspx.vb" Inherits="AuthorityPages_AdmissionApprove" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Admission Approval</title>
            <link href="../Bootstrap5/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../Bootstrap5/js/bootstrap.min.js" type="text/javascript"></script>
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
    <%--<style>
        body {
            background-color: #f5f5f5;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
        }

        .container {
            max-width: 1200px;
            margin: 50px auto;
            padding: 30px;
            background-color: #ffffff;
            border-radius: 10px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        }

        h2 {
            color: #006666;
            font-weight: bold;
            letter-spacing: 1px;
            font-size: 25px;
            margin: 0;
            display: flex;
            align-items: center;
        }

        .search-row {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 20px;
        }

        .search-row .form-control {
            width: 250px;
            margin-right: 10px;
            border: 2px solid #008080;
            border-radius: 30px;
        }

        .search-row .form-control:focus {
            border-color: #004d4d;
            box-shadow: none;
        }

        .search-row .btn-primary {
            background-color: #008080;
            border-color: #006666;
            border-radius: 30px;
            padding: 10px 20px;
        }

        .search-row .btn-primary:hover {
            background-color: #006666;
            border-color: #004d4d;
        }

        .form-control {
            border: 2px solid #008080;
            border-radius: 30px;
            padding: 10px 15px;
        }

        .btn-primary {
            background-color: #008080;
            border-color: #006666;
            border-radius: 30px;
        }

        .btn-primary:hover {
            background-color: #006666;
            border-color: #004d4d;
        }

        .back-link {
            color: #008080;
            text-decoration: none;
            font-weight: 600;
        }

        .back-link:hover {
            color: #006666;
        }

        .student-link {
            color: #008080;
            font-weight: 600;
        }

        .student-link:hover {
            color: #006666;
            text-decoration: underline;
        }

        .table-hover tbody tr:hover {
            background-color: #e6f7f7;
        }

        th {
            background-color: #008080;
            color: white;
        }

        .sr-no {
            background-color: #f0f8f8;
            font-weight: bold;
        }

        @media (max-width: 768px) {
            .container {
                padding: 20px;
            }

            h2 {
                font-size: 20px;
            }

            th, td {
                padding: 10px;
                font-size: 14px;
            }

            .search-row {
                flex-direction: column;
            }

            .search-row .form-control {
                margin-bottom: 10px;
            }

            .search-row .btn {
                width: 100%;
            }
        }
  /* Simple dropdown styling */
select.form-control {
    padding: 5px 10px; /* Standard padding */
    font-size: 16px; /* Standard font size */
    border: 1px solid #008080; /* Teal border */
    border-radius: 4px; /* Standard dropdown border-radius */
    color: #333; /* Text color */
    background-color: #ffffff; /* White background */
    width: 100%; /* Full width of container */
}

/* Option styling (optional, to maintain text visibility) */
select.form-control option {
    color: #333;
    padding: 5px 10px;
}

    </style>--%>
    <style>
    body {
    background-color: #fff;
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    margin: 0;
    padding: 0;
}

.container {
    max-width: 1070px;
    margin: 50px auto;
    padding: 30px;
    background-color: #ffffff;
}

h2 {
    color: black;
    font-size: 25px;
    margin: 0;
    display: flex;
    align-items: center;
}

.search-row {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-bottom: 20px;
}

.search-row .form-control {
    width: 250px;
    margin-right: 10px;
    border: 1px solid #cccccc;
    border-radius: 0;
}

.search-row .form-control:focus {
    border-color: #cccccc;
    box-shadow: none;
}

.search-row .btn-primary {
    background-color: #cccccc;
    border-color: #cccccc;
    border-radius: 0;
    padding: 10px 20px;
}

.search-row .btn-primary:hover {
    background-color: #b3b3b3;
    border-color: #b3b3b3;
}

.form-control {
    border: 1px solid #cccccc;
    border-radius: 0;
    padding: 10px 15px;
}

.btn-primary {
    background-color: #cccccc;
    border-color: #cccccc;
    border-radius: 0;
}

.btn-primary:hover {
    background-color: #b3b3b3;
    border-color: #b3b3b3;
}

.back-link {
    color: black;
    text-decoration: none;
    font-weight: 600;
}

.back-link:hover {
    color: black;
}

.student-link {
    color: navyblue;
    font-weight: 600;
}

.student-link:hover {
    color: blue;
    text-decoration: underline;
}



.sr-no {
    background-color: #fff;
    font-weight: bold;
}

@media (max-width: 768px) {
    .container {
        padding: 20px;
    }

    h2 {
        font-size: 20px;
    }
    .search-row {
        flex-direction: column;
    }

    .search-row .form-control {
        margin-bottom: 10px;
    }

    .search-row .btn {
        width: 100%;
    }
}

select.form-control {
    padding: 5px 10px;
    font-size: 16px;
    border: 1px solid #cccccc;
    border-radius: 0;
    color: #333;
    background-color: #ffffff;
    width: 100%;
}

select.form-control option {
    color: #333;
    padding: 5px 10px;
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
      .backbotton
{
    font-size:22px;
    font-weight:600;
    color:#7c858f;
    
}

.backbotton:hover
{
    color:#15283c;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
      <div id="AdmissionApproveInstructions" runat="server">
<div style="position: fixed; top: 10%; right: 0%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
    <small style ="position:absolute ;" > Instructions !<br /></br>
    <i class="fa-solid fa-star" style="color: #1ed085;"></i>
  View applicants and their status by course.</br>  
   <i class="fa-solid fa-star" style="color: #1ed085;"></i> Filter by course, status, or search in the grid.</br>  
    <i class="fa-solid fa-star" style="color: #1ed085;"></i> Use checkboxes to verify, update, or reject selected rows.</br>
     <i class="fa-solid fa-star" style="color: #1ed085;"></i> &nbsp;"Verify" promotes applicants to the next step: New Application → Document Verified → Registration Approved → Admission Approved.</br>
      <i class="fa-solid fa-star" style="color: #1ed085;"></i> Click applicant names for detailed info and actions.</br>
       <i class="fa-solid fa-star" style="color: #1ed085;"></i> Use the remarks textbox for feedback.</br>
</small>
</div>
</div>
    <asp:ScriptManager ID="script1" runat="server"></asp:ScriptManager>
        <div class="container">
        <div id="customAlert" class="custom-alert">
    <i class="fa fa-exclamation-triangle"></i>
    <span id="customAlertMessage">This is a custom alert!</span>
    <span class="close-btn" onclick="closeCustomAlert()">&times;</span>
    </div>
            <div style="display:flex; align-items: center;">
                <div class="col-md-4" style="display:flex; align-items: center;">
                    <div style="display:flex;">
            <asp:LinkButton ID="backbotton" class="backbotton" runat="server"><i class="fa-solid fa-arrow-left"></i></asp:LinkButton> &nbsp; &nbsp;
                <h2>Approve Admissions</h2>
            </div>
                </div>
                <div class="col-md-1"></div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlCourses" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCourses_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                
                <div class="col-md-1"></div>
               <div class="col-md-3"> 
                 <input type="text" id="searchBox" class="form-control" placeholder="Search..." onkeyup="searchTable()" />
                 
                 </div>
                
                
            </div>
            <hr />
            <div id = "gridContent" runat="server" style="margin-top:30px;">
            <!-- Search Box, Action Dropdown, Remarks TextBox, and Approve Button in the Same Row -->
            <div class="search-row">
               <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="True"  CssClass="form-control" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                        <asp:ListItem Text="All Applicants" Value="All"></asp:ListItem>
                        <asp:ListItem Text="New List" Value="NewList"></asp:ListItem>
                        <asp:ListItem Text="Document Verified" Value="DocVerified"></asp:ListItem>
                        <asp:ListItem Text="Registration Approved" Value="Verified"></asp:ListItem>
                        <asp:ListItem Text="Admission Approved" Value="Approved"></asp:ListItem>
                         <asp:ListItem Text="Application Rejected" Value="Rejected"></asp:ListItem>
                    </asp:DropDownList>

                <asp:DropDownList ID="ddlAction" runat="server" CssClass="form-control" ToolTip="Select Action">
                    <asp:ListItem Value="Verify">Verify</asp:ListItem>
                    <asp:ListItem Value="Reject">Reject</asp:ListItem>
                    <asp:ListItem Value="Update">Update</asp:ListItem>
                </asp:DropDownList>

                <asp:TextBox ID="remarks" runat="server" CssClass="form-control" placeholder="Enter Remarks..." ToolTip="Enter Feedback/Remarks"></asp:TextBox>

                <asp:Button ID="btnApprove" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btnApprove_Click"/>
            </div>
            
            <!-- GridView with Header Checkbox and Row Checkboxes -->
            <asp:GridView ID="GridViewStudents" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
    <Columns>
        <asp:TemplateField HeaderText="Select">
            <HeaderTemplate>
                <asp:CheckBox ID="selectAll" runat="server" OnClick="selectAllRows(this)" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="rowCheckbox" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Sr. No">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
            <ItemStyle CssClass="sr-no" />
        </asp:TemplateField>

        <asp:BoundField DataField="RegistrationID" HeaderText="Registration No" />

        <asp:TemplateField HeaderText="Name">
            <ItemTemplate>
                <asp:HyperLink ID="lnkName" runat="server" 
                               Text='<%# Eval("Student") %>' 
                               NavigateUrl='<%# Eval("RegistrationID", "StudentDetails.aspx?registrationid={0}") %>' 
                               CssClass="student-link">
                </asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="Course" HeaderText="Program" />
        <asp:BoundField DataField="FatherName" HeaderText="Father's Name" />
        <asp:BoundField DataField="MobileNo" HeaderText="Mobile" />
        <asp:BoundField DataField="Email" HeaderText="Email" />
        <asp:BoundField DataField="Gender" HeaderText="Gender" />
    </Columns>
</asp:GridView>

        </div>
        <div id="confirmation" runat="server" style="display:none;">
        <h1>Nothing is here!! Choose another options from dropdown</h1>
        </div>
        </div>
    </form>
    <%--<script type="text/javascript">
        function updateVisibility(rowCount) {
            var gridContent = document.getElementById('<%= gridContent.ClientID %>');
            var confirmation = document.getElementById('<%= confirmation.ClientID %>');

            if (rowCount > 0) {
                gridContent.style.display = 'block';
                confirmation.style.display = 'none';
            } else {
                gridContent.style.display = 'none';
                confirmation.style.display = 'block';
            }
        }
</script>--%>

    <script>
  function selectAllRows(headerCheckbox) {
    // Find all checkboxes within the GridView by finding inputs of type checkbox
    var checkboxes = document.querySelectorAll('#<%= GridViewStudents.ClientID %> input[type="checkbox"]');
    checkboxes.forEach(checkbox => {
        checkbox.checked = headerCheckbox.checked;
    });
}

    </script>
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
             }, 2500); // Wait for the slide-out animation to complete
         }
</script>

<script type="text/javascript">
    function searchTable() {
        var input = document.getElementById("searchBox");
        var filter = input.value.toLowerCase();
        var table = document.getElementById("GridViewStudents");
        var rows = table.getElementsByTagName("tr");

        for (var i = 1; i < rows.length; i++) {
            var cells = rows[i].getElementsByTagName("td");
            var match = false;

            // Loop through all the cells in the current row
            for (var j = 0; j < cells.length; j++) {
                if (cells[j]) {
                    var cellText = cells[j].textContent || cells[j].innerText;

                    // Check if any cell contains the search term
                    if (cellText.toLowerCase().indexOf(filter) > -1) {
                        match = true;
                        break;  // No need to check other cells if a match is found
                    }
                }
            }

            // Show the row if a match is found, otherwise hide it
            if (match) {
                rows[i].style.display = "";
            } else {
                rows[i].style.display = "none";
            }
        }
    }
</script>

</body>

</html>
