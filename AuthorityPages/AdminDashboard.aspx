<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdminDashboard.aspx.vb" Inherits="AuthorityPages_AdminDashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Admin Dashboard</title>
    <script src="https://canvasjs.com/assets/script/canvasjs.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css">

    <style>
        body {
            background-color: #f5f5f5;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
        }
        .container {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    max-width: 1200px;
    margin: 20px auto;
    padding: 10px;
    background-color: #fff;
    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
    border-radius: 5px; /* Rounded corners for the container */
}
        .column {
            width: 48%;
            padding: 10px;
            box-sizing: border-box;
        }

        h2 {
            text-align: center;
            color: #333;
            font-size: 18px;
            margin-bottom: 15px;
        }
        hr {
            border: none;
            border-top: 1px solid #ccc;
            margin: 20px 0;
        }
        @media (max-width: 768px) {
            .column {
                width: 100%;
            }
        }
        
.canvasjs-chart-credit
{
    display:none;
    }
    .main-box1 {
  display: flex;
  justify-content: space-between;
  padding: 20px;
}

.main-box {
  flex: 1; /* Ensures equal width */
  margin: 0 35px; /* Space between columns */
  background-color: #f8f9fa; /* Light background for a professional look */
  border: 1px solid black; /* Subtle border */
  border-radius: 5px; /* Rounded corners */
  padding: 20px; /* Inner padding */
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); /* Soft shadow */
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center; /* Center content vertically */
  cursor:pointer;
}


.main-box2 {
    display: flex;
    flex-wrap: nowrap; /* Prevent wrapping to a new line */
    justify-content: space-between;
    margin: 20px auto;
    padding: 10px;
}

.chart-section {
    
    margin: 10px; /* Margin around each chart section */
    background-color: #f9f9f9; /* Light background */
    padding: 20px; /* Inner padding */
    border-radius: 5px; /* Rounded corners */
    box-shadow: 0 0 5px rgba(0, 0, 0, 0.1); /* Soft shadow */
    border: 1px solid #333; /* Subtle border */
    display: flex;
    flex-direction: column;
}

h2 {
    text-align: center;
    color: #333;
    font-size: 18px;
    margin-bottom: 15px;
}

/* Media Queries */
@media (max-width: 768px) {
    .main-box2 {
        flex-wrap: wrap; /* Allow wrapping on smaller screens */
    }
    .chart-section {
        flex: 1 1 100%; /* Full width on smaller screens */
    }
   
}
@media (max-width: 768px) {
 .main-box1{flex-direction:column;}
 .gogo{margin-bottom:20px;}
 }
 @media (max-width: 1090px) {
 .main-box1{flex-direction:column;}
  .gogo{margin-bottom:20px;}
 }
.main-box2 .chart-section
{
   width:520px; 
    }
    canvas{currsor:pointer; }
    .canvasjs-chart-canvas{height:300px;}
    </style>
</head>
<body>  
    <form id="form1" runat="server">
        <div style="padding:30px 20px 30px 20px">
            <p style="text-align:center; font-weight:bold;font-size:30px">Admin Dashboard</p>
            <div class="container">

                <div class="main-box1 d-flex justify-content-between">

                    <div style="display:flex;justify-content:space-between;" class="gogo">
                        <div class="main-box" onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=All'">
    <h2>Total Applications</h2>
    <div id="totalApplicationsDiv" runat="server" class="chart"></div>
</div>
                        <%--<div class="main-box"  onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=NewList'">
                            <h2>New &emsp; Applications</h2>
                            <div id="pendingApplicationsDiv" class="chart" runat="server"></div>
                        </div>--%>
                        <div class="main-box"  onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=DocVerified'">
                            <h2>Documnent Verified</h2>
                            <div id="verifiedApplicationsDiv" runat="server" class="chart"></div>
                        </div>
                    </div>
                    <div style="display:flex;justify-content:space-between;">
                    <div class="main-box"  onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=Verified'">
                            <h2>Approved Applications</h2>
                            <div id="docVerifiedApplicationsDiv" runat="server" class="chart"></div>
                        </div>
                        <div class="main-box"  onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=Approved'">
                            <h2>Admission Approved</h2>
                            <div id="approvedApplicationsDiv" runat="server" class="chart"></div>
                        </div>
                        
                        <div class="main-box" onclick="window.location.href = 'Applications.aspx'">
    <h2>Total Programs</h2>
    <div id="totalCourses" runat="server" class="chart" ></div>
</div>

                    </div>
                </div>



                <div class="main-box2 d-flex flex-wrap justify-content-between">
                   <div class="chart-section">
    <div style="display:flex; justify-content: space-between; align-items: center;">
        <h2>Applications Status Summary</h2>
        <div style="display: flex;">
            <p onclick="renderAppChart('doughnut')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-pie-chart"></i>
            </p>
            <p onclick="renderAppChart('column')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-bar-chart"></i>
            </p>
            <p onclick="renderAppGrid()" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-table" aria-hidden="true"></i>
            </p>
        </div>
    </div>
    <div id="applicationChart" style="height: 300px;"></div>
    <div id="applicationGrid" style="display: none;"></div>
</div>

                 
  <div class="chart-section">
    <div style="display:flex; justify-content: space-between; align-items: center;">
        <h2>Document Verification Status</h2>
        <div style="display: flex;">
            <p onclick="renderChart('pie')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-pie-chart"></i>
            </p>
            <p onclick="renderChart('column')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-bar-chart"></i>
            </p>
            <p onclick="renderGrid()" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-table" aria-hidden="true"></i>
            </p>
        </div>
    </div>
    <div id="documentChart" style="height: 300px;"></div>
    <div id="documentGrid" style="display: none;"></div>
</div>
</div>

<div class="main-box2 d-flex flex-wrap justify-content-between p-4 bg-light" style="border: 1px solid black;">
    <div class="summary-box p-4 mb-3 bg-white shadow-sm rounded flex-fill">
        <h2 class="text-black mb-3">
            <i class="fas fa-check-circle"></i> Verified and Rejected Documents Summary
        </h2>
        <div class="d-flex flex-column">
            <div id="verifiedDocumentsDiv" runat="server">
                <!-- Dynamic content for Verified Documents will be inserted here -->
            </div>
            <div id="pendingDocumentsDiv" runat="server">
                <!-- Dynamic content for Pending Documents will be inserted here -->
            </div>
            <div id="rejectedDocumentsDiv" runat="server">
                <!-- Dynamic content for Rejected Documents will be inserted here -->
            </div>
        </div>
    </div>

    <div style="width: 120px; height: 1px;"></div>

    <div class="summary-box p-4 mb-3 bg-white shadow-sm rounded flex-fill">
        <h2 class="text-black mb-3">
            <i class="fas fa-thumbs-up"></i> Summary of Approved and Rejected Applications
        </h2>
        <div class="d-flex flex-column">
            <div id="approvedApplicationsDiv2" runat="server">
                <!-- Dynamic content for Approved Applications will be inserted here -->
            </div>
            <div id="rejectedApplicationsDiv2" runat="server">
                <!-- Dynamic content for Rejected Applications will be inserted here -->
            </div>
            <div id="pendingApplicationsDiv2" runat="server">
                <!-- Dynamic content for Pending Applications will be inserted here -->
            </div>
        </div>
    </div>
</div>

                                <div class="main-box2 d-flex flex-wrap justify-content-between">
<div class="chart-section">
    <div style="display:flex; justify-content: space-between; align-items: center;">
        <h2>Admission Funnel Report</h2>
        <div style="display: flex;">
            <p onclick="renderFunnelChart('funnel')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-filter"></i>
            </p>
            <p onclick="renderFunnelChart('pie')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-pie-chart"></i>
            </p>
            <p onclick="renderFunnelChart('column')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-bar-chart"></i>
            </p>
            <p onclick="renderFunnelGrid()" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-table" aria-hidden="true"></i>
            </p>
        </div>
    </div>
    <div id="admissionFunnelChart" style="height: 300px;"></div>
    <div id="admissionGrid" style="display: none;"></div>
</div>
                <div class="chart-section">
    <div style="display:flex; justify-content: space-between; align-items: center;">
        <h2>Program Specific Applications</h2>
        <div style="display: flex;">
            <p onclick="renderProgramChart('column')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-bar-chart"></i>
            </p>
            <p onclick="renderProgramChart('pie')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-pie-chart"></i>
            </p>
            <p onclick="renderProgramGrid()" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-table" aria-hidden="true"></i>
            </p>
        </div>
    </div>
    <div id="leadManagementChart" style="height: 300px;"></div>
    <div id="leadManagementGrid" style="display: none;"></div>
</div>

</div>


                <div class="main-box2 d-flex flex-wrap justify-content-between">

                <%--<div class="chart-section">
                <h2>Concession Reports</h2>
                <div id="concessionChart" style="height: 300px;"></div>
            </div>--%>
                  <div class="chart-section">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="True"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="display: flex; justify-content: space-between; align-items: center;">
                <h2><label for="ddlItemCount">New Application</label></h2>
                <asp:DropDownList ID="ddlItemCount" runat="server" AutoPostBack="True" 
                    OnSelectedIndexChanged="ddlItemCount_SelectedIndexChanged" style="margin-left: 10px;">
                    <asp:ListItem Value="5" Selected="True">5</asp:ListItem>
                    <asp:ListItem Value="1">1</asp:ListItem>
                    <asp:ListItem Value="2">2</asp:ListItem>
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="6">6</asp:ListItem>
                    <asp:ListItem Value="7">7</asp:ListItem>
                    <asp:ListItem Value="8">8</asp:ListItem>
                    <asp:ListItem Value="9">9</asp:ListItem>
                    <asp:ListItem Value="10">10</asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:Label ID="lblMessage" runat="server" Text="There is no application yet." 
           Visible="False" 
           Style="display: block; margin: 10px 0; color: #ff0000; font-weight: bold; text-align: center;">
</asp:Label>

            <asp:GridView ID="gvApplications" runat="server" AutoGenerateColumns="False" CssClass="table" style="margin-top: 10px;">
                <Columns>
                    <asp:BoundField HeaderText="Sr. No" DataField="SrNo" />
                    <asp:BoundField HeaderText="Name" DataField="Name" />
                    <asp:BoundField HeaderText="Program" DataField="Program" />
                    <asp:BoundField HeaderText="Date" DataField="Date" DataFormatString="{0:dd-MM-yyyy}" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="chart-section">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="display: flex; justify-content: space-between; align-items: center;">
                <h2><label for="ddlItemCountLatest">Latest Approved Applications</label></h2>
                <asp:DropDownList ID="ddlItemCountLatest" runat="server" AutoPostBack="True" 
                    OnSelectedIndexChanged="ddlItemCountLatest_SelectedIndexChanged" style="margin-left: 10px;">
                    <asp:ListItem Value="5" Selected="True">5</asp:ListItem>
                    <asp:ListItem Value="1">1</asp:ListItem>
                    <asp:ListItem Value="2">2</asp:ListItem>
                    <asp:ListItem Value="3">3</asp:ListItem>
                    <asp:ListItem Value="4">4</asp:ListItem>
                    <asp:ListItem Value="6">6</asp:ListItem>
                    <asp:ListItem Value="7">7</asp:ListItem>
                    <asp:ListItem Value="8">8</asp:ListItem>
                    <asp:ListItem Value="9">9</asp:ListItem>
                    <asp:ListItem Value="10">10</asp:ListItem>
                </asp:DropDownList>
            </div>

            <asp:GridView ID="gvApplications_ddlItemCountLatest" runat="server" AutoGenerateColumns="False" CssClass="table" style="margin-top: 10px;">
                <Columns>
                    <asp:BoundField HeaderText="Sr. No" DataField="SrNo" />
                    <asp:BoundField HeaderText="Name" DataField="Name" />
                    <asp:BoundField HeaderText="Program" DataField="Program" />
                    <asp:BoundField HeaderText="Date" DataField="Date" DataFormatString="{0:dd-MM-yyyy}" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

       </div>

        <div class="main-box2 d-flex flex-wrap justify-content-between">

                <div class="chart-section">
    <div style="display: flex; justify-content: space-between; align-items: center;">
        <h2>Concession Reports(Upcoming)</h2>
        <div style="display: flex;">
            <p onclick="renderConcessionChart('pie')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-pie-chart"></i>
            </p>
            <p onclick="renderConcessionChart('column')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-bar-chart"></i>
            </p>
            <p onclick="renderConcessionGrid()" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-table" aria-hidden="true"></i>
            </p>
        </div>
    </div>
    <div id="concessionChart" style="height: 300px;"></div>
    <div id="concessionGrid" style="display: none;"></div>
</div>
                   <div class="chart-section">
    <div style="display: flex; justify-content: space-between; align-items: center;">
        <h2>Fee Payment Report</h2>
        <div style="display: flex;">
            <p onclick="renderFeeChart('pie')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-pie-chart"></i>
            </p>
            <p onclick="renderFeeChart('column')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-bar-chart"></i>
            </p>
            <p onclick="renderFeeGrid()" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-table" aria-hidden="true"></i>
            </p>
        </div>
    </div>
    <div id="feeChart" style="height: 300px;"></div>
    <div id="feeGrid" style="display: none;"></div>
</div>
         </div>

            </div>
        </div>

        <script>
var feeChart;
var admissionFeePaid = 0;
var applicationFeePaid = 0;
var admissionFeeNotPaid = 0;

// Function to render the fee chart based on the selected type
function renderFeeChart(type) {
    if (feeChart) {
        feeChart.destroy();
    }

    // Set display styles
    document.getElementById("feeGrid").style.display = "none";
    document.getElementById("feeChart").style.display = "block";

    // Create the chart
    feeChart = new CanvasJS.Chart("feeChart", {
        theme: "light2",
        exportEnabled: true,
        animationEnabled: true,
        data: [{
            type: type,
            indexLabel: "{label} - {y}",
            toolTipContent: "{label}: <strong>{y}</strong>",
            dataPoints: [
                { label: "Application Fee Paid", y: applicationFeePaid },
                { label: "Admission Fee Paid", y: admissionFeePaid },
                { label: "Adimssion Fee Pending", y: admissionFeeNotPaid }
            ]
        }]
    });

    feeChart.render();
}

// Function to render the fee grid
function renderFeeGrid() {
    // Hide the chart and show the grid
    document.getElementById("feeChart").style.display = "none";
    document.getElementById("feeGrid").style.display = "block";

    // Populate the grid with data
    document.getElementById("feeGrid").innerHTML = `
        <table style="width:100%;border-collapse:collapse;">
            <tr><th style="border:1px solid #ccc;padding:8px;">Status</th><th style="border:1px solid #ccc;padding:8px;">Count</th></tr>
            <tr>
                <td style="border:1px solid #ccc;padding:8px;">Application Fee Paid</td>
                <td style="border:1px solid #ccc;padding:8px;">${applicationFeePaid}</td>
            </tr>
            <tr>
                <td style="border:1px solid #ccc;padding:8px;">Admission Fee Paid</td>
                <td style="border:1px solid #ccc;padding:8px;">${admissionFeePaid}</td>
            </tr>
            <tr>
                <td style="border:1px solid #ccc;padding:8px;">Admission Fee Pending</td>
                <td style="border:1px solid #ccc;padding:8px;">${admissionFeeNotPaid}</td>
            </tr>
        </table>
    `;
}

renderFeeChart('pie')
</script>
        <script>

var documentChart;

function renderChart(type) {
    if (documentChart) {
        documentChart.destroy();
    }
    document.getElementById("documentGrid").style.display = "none";
    document.getElementById("documentChart").style.display = "block";

    documentChart = new CanvasJS.Chart("documentChart", {
        theme: "light2",
        exportEnabled: true,
        animationEnabled: true,
        data: [{
            type: type,
            indexLabel: "{label} - {y}",
            toolTipContent: "{label}: <strong>{y}</strong>",
            dataPoints: [
                { label: "Verifed", y: verifiedCount, legendText: "Verified", click: function(e) { navigateToApprovalPage(e.dataPoint.label); }},
                { label: "Rejected", y: rejectedCount, legendText: "Rejected", click: function(e) { navigateToApprovalPage(e.dataPoint.label); }},
                { label: "Pending", y: pendingCount, legendText: "Pending", click: function(e) { navigateToApprovalPage(e.dataPoint.label); }}
            ]
        }]
    });

    documentChart.render();
}

function renderGrid() {
    document.getElementById("documentChart").style.display = "none";
    document.getElementById("documentGrid").style.display = "block";

    document.getElementById("documentGrid").innerHTML = `
        <table style="width:100%;border-collapse:collapse;">
            <tr><th style="border:1px solid #ccc;padding:8px;">Status</th><th style="border:1px solid #ccc;padding:8px;">Count</th></tr>
            <tr>
                <td style="border:1px solid #ccc;padding:8px;">Verified</td>
                <td style="border:1px solid #ccc;padding:8px;">
                    ${verifiedCount > 0 ? `<a href="#" onclick="navigateToApprovalPage('Verified')">${verifiedCount}</a>` : verifiedCount}
                </td>
            </tr>
            <tr>
                <td style="border:1px solid #ccc;padding:8px;">Rejected</td>
                <td style="border:1px solid #ccc;padding:8px;">
                    ${rejectedCount > 0 ? `<a href="#" onclick="navigateToApprovalPage('Rejected')">${rejectedCount}</a>` : rejectedCount}
                </td>
            </tr>
            <tr>
                <td style="border:1px solid #ccc;padding:8px;">Pending</td>
                <td style="border:1px solid #ccc;padding:8px;">
                    ${pendingCount > 0 ? `<a href="#" onclick="navigateToApprovalPage('Pending')">${pendingCount}</a>` : pendingCount}
                </td>
            </tr>
        </table>
    `;
}

// Navigation function to redirect to the AdmissionApprove page
function navigateToApprovalPage(status) {

    window.location.href = 'AdmissionApprove.aspx?courseid=All&status=' + approvedStatus;
}

// Initially render the chart as a pie chart
renderChart('pie');

</script>
<script>

// Stages of Applications
var applicationChart;

// Variables to hold counts from the backend
var pendingCount = <%= GetPendingApplications() %>;
var approvedCount = <%= GetApprovedApplications() %>;
var rejectedCount = <%= GetRejectedApplications() %>;
var verifiedCount = <%= GetVerifiedApplications() %>;
var docverifiedCount = <%= GetDocVerifiedApplications() %>;

function renderAppChart(type) {
    if (applicationChart) {
        applicationChart.destroy();
    }

    document.getElementById("applicationGrid").style.display = "none";
    document.getElementById("applicationChart").style.display = "block";

    applicationChart = new CanvasJS.Chart("applicationChart", {
        theme: "light2",
        exportEnabled: true,
        animationEnabled: true,
        data: [{
            type: type,
            indexLabel: "{label} - {y}",
            toolTipContent: "{label}: <strong>{y}</strong>",
            dataPoints: [
                { label: "Pending", y: pendingCount, legendText: "Pending", click: function() { navigateToApprovalPage("NewList"); }},
                { label: "Approved", y: approvedCount, legendText: "Approved", click: function() { navigateToApprovalPage("Approved"); }},
                { label: "Rejected", y: rejectedCount, legendText: "Rejected", click: function() { navigateToApprovalPage("Rejected"); }},
                { label: "Document Verified", y: docverifiedCount, legendText: "Document Verified", click: function() { navigateToApprovalPage("DocVerified"); }},
                { label: "Verified", y: verifiedCount, legendText: "Verified", click: function() { navigateToApprovalPage("Verified"); }}
            ]
        }]
    });

    applicationChart.render();
}

function renderAppGrid() {
    document.getElementById("applicationChart").style.display = "none";
    document.getElementById("applicationGrid").style.display = "block";

    document.getElementById("applicationGrid").innerHTML = 
        "<table style='width:100%;border-collapse:collapse;'>" +
            "<tr><th style='border:1px solid #ccc;padding:8px;'>Stage</th><th style='border:1px solid #ccc;padding:8px;'>Count</th></tr>" +
            `<tr><td style='border:1px solid #ccc;padding:8px;'>Pending</td><td style='border:1px solid #ccc;padding:8px;'>
                ${pendingCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=NewList' style='text-decoration:none;'>${pendingCount}</a>` : pendingCount}
            </td></tr>` +
            `<tr><td style='border:1px solid #ccc;padding:8px;'>Approved</td><td style='border:1px solid #ccc;padding:8px;'>
                ${approvedCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=Approved' style='text-decoration:none;'>${approvedCount}</a>` : approvedCount}
            </td></tr>` +
            `<tr><td style='border:1px solid #ccc;padding:8px;'>Rejected</td><td style='border:1px solid #ccc;padding:8px;'>
                ${rejectedCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=Rejected' style='text-decoration:none;'>${rejectedCount}</a>` : rejectedCount}
            </td></tr>` +
            `<tr><td style='border:1px solid #ccc;padding:8px;'>Verified</td><td style='border:1px solid #ccc;padding:8px;'>
                ${verifiedCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=Verified' style='text-decoration:none;'>${verifiedCount}</a>` : verifiedCount}
            </td></tr>` +
            `<tr><td style='border:1px solid #ccc;padding:8px;'>Verified</td><td style='border:1px solid #ccc;padding:8px;'>
                ${docverifiedCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=DocVerified' style='text-decoration:none;'>${docverifiedCount}</a>` : docverifiedCount}
            </td></tr>` +
        "</table>";
}
function navigateToApprovalPage(status) {
    window.location.href = 'AdmissionApprove.aspx?courseid=All&status=' + status;
}
// Initialize the chart on page load
renderAppChart('doughnut');
</script>
<script>
// Concession Reports Chart
function renderConcessionChart(chartType) {
    const concessionChart = new CanvasJS.Chart("concessionChart", {
        theme: "light2",
        animationEnabled: true,
        exportEnabled: true,
        data: [{
            type: chartType, // Chart type passed dynamically
            dataPoints: [
                { label: "Granted", y: 40 },
                { label: "Pending", y: 60 }
            ]
        }]
    });
    concessionChart.render();
}

// Concession Reports Grid
function renderConcessionGrid() {
    const gridContainer = document.getElementById("concessionGrid");
    const data = [
        { Status: "Granted", Count: 40 },
        { Status: "Pending", Count: 60 }
    ];

    let tableHTML = `<table border="1" style="width: 100%; border-collapse: collapse; text-align: left;">
        <thead>
            <tr>
                <th>Status</th>
                <th>Count</th>
            </tr>
        </thead>
        <tbody>`;
    
    data.forEach(row => {
        tableHTML += `<tr>
            <td>${row.Status}</td>
            <td>${row.Count}</td>
        </tr>`;
    });

    tableHTML += `</tbody></table>`;
    gridContainer.innerHTML = tableHTML;

    // Show grid and hide chart
    document.getElementById("concessionChart").style.display = "none";
    document.getElementById("concessionGrid").style.display = "block";
}

// Initialize the chart view by default
document.addEventListener("DOMContentLoaded", () => {
    renderConcessionChart("pie"); // Default chart type is pie
});
</script>
<script>
    var programChart;

    function renderProgramChart(type) {
        if (!programDataPoints || programDataPoints.length === 0) {
            document.getElementById("leadManagementChart").style.display = "none";
            document.getElementById("leadManagementGrid").style.display = "none";
            return;
        }

        document.getElementById("leadManagementChart").style.display = "block";
        document.getElementById("leadManagementGrid").style.display = "none";

        if (programChart) {
            programChart.destroy();
        }

        programChart = new CanvasJS.Chart("leadManagementChart", {
            theme: "light2",
            exportEnabled: true,
            animationEnabled: true,
            data: [{
                type: type,
                indexLabel: "{label} - {y}",
                toolTipContent: "{label}: <strong>{y}</strong>",
                dataPoints: programDataPoints
            }]
        });

        programChart.render();
    }

    function renderProgramGrid() {
        if (!programDataPoints || programDataPoints.length === 0) {
            document.getElementById("leadManagementChart").style.display = "none";
            document.getElementById("leadManagementGrid").style.display = "none";
            return;
        }

        document.getElementById("leadManagementChart").style.display = "none";
        document.getElementById("leadManagementGrid").style.display = "block";

        const gridHTML = `
            <table style="width:100%;border-collapse:collapse;">
                <tr><th style="border:1px solid #ccc;padding:8px;">Program</th><th style="border:1px solid #ccc;padding:8px;">Applications</th></tr>
                ${programDataPoints.map(dp => `
                    <tr>
                        <td style="border:1px solid #ccc;padding:8px;cursor:pointer;">${dp.label}</td>
                        <td style="border:1px solid #ccc;padding:8px;">
                            <a href="#" onclick="redirectToAdmissionApprove('${encodeURIComponent(dp.label)}')">${dp.y}</a>
                        </td>
                    </tr>
                `).join('')}
            </table>
        `;

        document.getElementById("leadManagementGrid").innerHTML = gridHTML;
    }

    function redirectToAdmissionApprove(courseLabel) {
        window.location.href = "AdmissionApprove.aspx?courseid=" + courseLabel + "&status=All";
    }

window.onload = function () {
    renderProgramChart('column'); // Default to rendering a pie chart on load.
};

</script>
<%--<script>
    var demographicChart;

    // Function to render the chart
    function renderDemographicChart(type) {
        // Destroy previous chart if it exists
        if (demographicChart) {
            demographicChart.destroy();
        }

        // Ensure that the grid view is hidden and chart view is visible
        document.getElementById("Div2").style.display = "none";
        document.getElementById("Div1").style.display = "block";

        // Create a new chart with the specified type (pie or column)
        demographicChart = new CanvasJS.Chart("Div1", {
            theme: "light2",
            exportEnabled: true,
            animationEnabled: true,
            data: [{
                type: type,
                showInLegend: true,
                indexLabel: "{label} - {y}",
                toolTipContent: "{label}: <strong>{y}</strong>",
                dataPoints: [
                    { label: "Male", y: window.maleCount },
                    { label: "Female", y: window.femaleCount },
                    { label: "Other", y: window.otherCount }
                ]
            }]
        });

        // Render the chart
        demographicChart.render();
    }

    // Function to render the data in a table/grid format
    function renderDemographicGrid() {
        // Hide the chart and show the grid
        document.getElementById("Div1").style.display = "none";
        document.getElementById("Div2").style.display = "block";

        // Populate the grid with demographic data
        document.getElementById("Div2").innerHTML =
            "<table style='width:100%;border-collapse:collapse;'>" +
                "<tr><th style='border:1px solid #ccc;padding:8px;'>Gender</th><th style='border:1px solid #ccc;padding:8px;'>Count</th></tr>" +
                "<tr><td style='border:1px solid #ccc;padding:8px;'>Male</td><td style='border:1px solid #ccc;padding:8px;'>" + maleCount + "</td></tr>" +
                "<tr><td style='border:1px solid #ccc;padding:8px;'>Female</td><td style='border:1px solid #ccc;padding:8px;'>" + femaleCount + "</td></tr>" +
                "<tr><td style='border:1px solid #ccc;padding:8px;'>Other</td><td style='border:1px solid #ccc;padding:8px;'>" + otherCount + "</td></tr>" +
            "</table>";
    }

    // Initially render the chart as a pie chart on page load
    renderDemographicChart('pie');

</script>--%>
<script>
var chart;
    function renderFunnelChart(type) {
     document.getElementById("admissionFunnelChart").style.display = "block";
        document.getElementById("admissionGrid").style.display = "none";

        if(chart){
        chart.destroy();
        }
        chart = new CanvasJS.Chart("admissionFunnelChart", {
            animationEnabled: true,
            exportEnabled: true,
            theme: "light2",
            data: [{
                type: type,
                legendText: "{label}",
                indexLabel: "{label} - {y}",
                toolTipContent: "<b>{label}</b>: {y}",
                dataPoints: chartData // Use the dynamic data
            }]
        });
        chart.render();
       
    }

function renderFunnelGrid() {
    document.getElementById("admissionFunnelChart").style.display = "none";
    document.getElementById("admissionGrid").style.display = "block";

    document.getElementById("admissionGrid").innerHTML = 
        "<table style='width:100%;border-collapse:collapse;height:250px;'>" +
            "<thead>" +
                "<tr>" +
                    "<th style='border:1px solid #ddd;padding:8px;'>Stage</th>" +
                    "<th style='border:1px solid #ddd;padding:8px;'>Count</th>" +
                "</tr>" +
            "</thead>" +
            "<tbody>" +
                `<tr><td style='border:1px solid #ddd;padding:8px;'>Applications</td><td style='border:1px solid #ddd;padding:8px;'>${chartData[0].y}</td></tr>` +
                `<tr><td style='border:1px solid #ddd;padding:8px;'>Payment</td><td style='border:1px solid #ddd;padding:8px;'>${chartData[1].y}</td></tr>` +
                `<tr><td style='border:1px solid #ddd;padding:8px;'>Admission</td><td style='border:1px solid #ddd;padding:8px;'>${chartData[2].y}</td></tr>` +
            "</tbody>" +
        "</table>";
}

    renderFunnelChart('funnel'); // Initial render
</script>
    </form>
</body>

</html>
