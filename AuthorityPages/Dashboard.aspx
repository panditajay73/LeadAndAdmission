<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Dashboard.aspx.vb" Inherits="AuthorityPages_Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>HoD Dashboard Reports</title>
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
    </style>
</head>
<body>  
    <form id="form1" runat="server">
        <div style="padding:30px 20px 30px 20px">
            <p style="text-align:center; font-weight:bold;font-size:30px">Head of Departments 
                Dashboard</p>
            <div class="container">

                <div class="main-box1 d-flex justify-content-between">
                    <div style="display:flex;justify-content:space-between;" class="gogo">
                        <div class="main-box" onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=All'">
    <h2>Total Applications</h2>
    <div id="totalApplicationsDiv" runat="server" class="chart"></div>
</div>
                        <div class="main-box"  onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=NewList'">
                            <h2>Pending Applications</h2>
                            <div id="pendingApplicationsDiv" class="chart" runat="server"></div>
                        </div>
                    </div>
                    <div style="display:flex;justify-content:space-between;">
                        <div class="main-box"  onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=Approved'">
                            <h2>Approved Applications</h2>
                            <div id="approvedApplicationsDiv" runat="server" class="chart"></div>
                        </div>
                        <div class="main-box"  onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=Verified'">
                            <h2>Verified Applications</h2>
                            <div id="verifiedApplicationsDiv" runat="server" class="chart"></div>
                        </div>
                        <div class="main-box" onclick="window.location.href = 'AdmissionApprove.aspx?courseid=All&status=Rejected'">
    <h2>Rejected Applications</h2>
    <div id="rejectedApplicationsDiv" runat="server" class="chart" ></div>
</div>

                    </div>
                </div>

                <div class="main-box2 d-flex flex-wrap justify-content-between">
                 <div class="chart-section">
    <div style="display:flex; justify-content: space-between; align-items: center;">
        <h2>Document Verification Report</h2>
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
        <h2>Stages of Applications</h2>
        <div style="display: flex;">
        <p onclick="renderAppGrid()" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-table" aria-hidden="true"></i>
            </p>
            <p onclick="renderAppChart('pie')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-pie-chart"></i>
            </p>
            <p onclick="renderAppChart('column')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-bar-chart"></i>
            </p>
            
        </div>
    </div>
    <div id="applicationChart" style="height: 300px;"></div>
    <div id="applicationGrid" style="display: none;"></div>
</div>

                  <div class="chart-section">
    <div style="display:flex; justify-content: space-between; align-items: center;">
        <h2>Applicant Demographics</h2>
        <div style="display: flex;">
            <p onclick="renderDemographicChart('pie')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-pie-chart"></i>
            </p>
            <p onclick="renderDemographicChart('column')" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-bar-chart"></i>
            </p>
            <p onclick="renderDemographicGrid()" style="cursor: pointer; margin-left: 5px;">
                <i class="fa fa-table" aria-hidden="true"></i>
            </p>
        </div>
    </div>
    <div id="Div1" style="height: 300px;"></div>
    <div id="Div2" style="display: none;"></div>
</div>
                </div>

                <%--<div class="main-box2 d-flex flex-wrap justify-content-between">

                <div class="chart-section">
                <h2>Concession Reports</h2>
                <div id="concessionChart" style="height: 300px;"></div>
            </div>
                    <div class="chart-section">
    <div style="display: flex; justify-content: space-between; align-items: center;">
        <h2>Concession Distribution</h2>
        
    </div>
    <div id="Div3" style="height: 300px;">
    Report coming soon..
    </div>
</div>
                </div>--%>
            </div>
        </div>

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
            showInLegend: true,
            indexLabel: "{label} - {y}",
            toolTipContent: "{label}: <strong>{y}</strong>",
            dataPoints: [
                { label: "Pending", y: pendingCount, legendText: "Pending", click: function() { navigateToApprovalPage("Pending"); }},
                { label: "Approved", y: approvedCount, legendText: "Approved", click: function() { navigateToApprovalPage("Approved"); }},
                { label: "Rejected", y: rejectedCount, legendText: "Rejected", click: function() { navigateToApprovalPage("Rejected"); }},
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
                ${pendingCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=Pending' style='text-decoration:none;'>${pendingCount}</a>` : pendingCount}
            </td></tr>` +
            `<tr><td style='border:1px solid #ccc;padding:8px;'>Approved</td><td style='border:1px solid #ccc;padding:8px;'>
                ${approvedCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=Approved' style='text-decoration:none;'>${approvedCount}</a>` : approvedCount}
            </td></tr>` +
            `<tr><td style='border:1px solid #ccc;padding:8px;'>Rejected</td><td style='border:1px solid #ccc;padding:8px;'>
                ${rejectedCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=Rejected' style='text-decoration:none;'>${rejectedCount}</a>` : rejectedCount}
            </td></tr>` +
            `<tr><td style='border:1px solid #ccc;padding:8px;'>Verified</td><td style='border:1px solid #ccc;padding:8px;'>
                ${verifiedCount > 0 ? `<a href='AdmissionApprove.aspx?courseid=All&status=Approved' style='text-decoration:none;'>${verifiedCount}</a>` : verifiedCount}
            </td></tr>` +
        "</table>";
}
function navigateToApprovalPage(status) {
var status = (status === "Verified") ? "Approved" : status;
    window.location.href = 'AdmissionApprove.aspx?courseid=All&status=' + status;
}
// Initialize the chart on page load
renderAppChart('pie');

</script>
<script>
    var demographicChart;

    // Function to render the chart
    function renderDemographicChart(type) {
        // Destroy the previous chart instance if it exists
        if (demographicChart) {
            demographicChart.destroy(); // Corrected: `destroy` isn't a method of CanvasJS; replaced with removing the existing chart element
        }

        // Ensure the grid view is hidden and chart view is visible
        document.getElementById("Div2").style.display = "none";
        document.getElementById("Div1").style.display = "block";

        // Create a new chart with the specified type (pie or column)
        demographicChart = new CanvasJS.Chart("Div1", {
            theme: "light2",
            exportEnabled: true,
            animationEnabled: true,
            data: [{
                type: type, // Chart type (pie or column)
                indexLabel: "{label} - {y}",
                toolTipContent: "{label}: <strong>{y}</strong>",
                dataPoints: [
                    { label: "Male", y: window.maleCount || 0 },
                    { label: "Female", y: window.femaleCount || 0 },
                    { label: "Other", y: window.otherCount || 0 }
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
            "<table style='width:100%;border-collapse:collapse;border:1px solid #ccc;'>" +
                "<thead>" +
                    "<tr>" +
                        "<th style='border:1px solid #ccc;padding:8px;'>Gender</th>" +
                        "<th style='border:1px solid #ccc;padding:8px;'>Count</th>" +
                    "</tr>" +
                "</thead>" +
                "<tbody>" +
                    "<tr>" +
                        "<td style='border:1px solid #ccc;padding:8px;'>Male</td>" +
                        "<td style='border:1px solid #ccc;padding:8px;'>" + (window.maleCount || 0) + "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td style='border:1px solid #ccc;padding:8px;'>Female</td>" +
                        "<td style='border:1px solid #ccc;padding:8px;'>" + (window.femaleCount || 0) + "</td>" +
                    "</tr>" +
                    "<tr>" +
                        "<td style='border:1px solid #ccc;padding:8px;'>Other</td>" +
                        "<td style='border:1px solid #ccc;padding:8px;'>" + (window.otherCount || 0) + "</td>" +
                    "</tr>" +
                "</tbody>" +
            "</table>";
    }

    // Add an event listener to ensure the chart is rendered as a pie chart on page load
    document.addEventListener("DOMContentLoaded", function () {
        renderDemographicChart('pie'); // Default to 'pie' chart on load
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
    </form>
</body>

</html>
