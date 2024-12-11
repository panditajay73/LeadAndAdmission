<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Applications.aspx.vb" Inherits="AuthorityPages_Applications" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Application - Programs</title>
        <link href="../Bootstrap5/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="../Bootstrap5/js/bootstrap.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://kit.fontawesome.com/a429e5bfb6.js" crossorigin="anonymous"></script>
 <%--   <style>
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

        .back-link {
            display: inline-flex;
            align-items: center;
            text-decoration: none;
            font-weight: 600;
            color: #333;
            text-align: left;
            font-weight: bold;
            letter-spacing: 1px;
        }

        .back-link:hover {
            color: #333;
            text-decoration: none;
        }

        .back-link span {
            margin-right: 10px;
            font-size: 18px;
        }

        h2 {
            color: #333;
            text-align: left;
            font-weight: bold;
            letter-spacing: 1px;
            font-size: 25px;
            margin-left:20px;
        }

        .table {
            width: 100%;
            border-radius: 10px;
            overflow: hidden;
            border: none;
            background-color: #ffffff;
        }

        th {
            background-color: #008080;
            color: white;
            font-weight: 600;
            text-align: center;
            padding: 15px;
            border: none;
        }

        td {
            text-align: center;
            padding: 12px;
            font-size: 16px;
            color: #333;
            border: none;
        }

        .sr-no {
            background-color: #f0f8f8;
            font-weight: bold;
        }

        .table-hover tbody tr:hover {
            background-color: #f0f8f8;
            transition: background-color 0.3s ease;
        }

        .program-link {
            color: #008080;
            font-weight: 600;
            text-decoration: none;
        }

        .program-link:hover {
            text-decoration: underline;
            color: #006666;
        }

        .no-applicants {
            color: red;
        }

        .has-applicants {
            color: #333;
        }

        .filter-container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }

        .search-box {
            flex: 1;
            margin-right: 15px;
        }

        .search-box input {
            width: 30%;
            padding: 10px 15px;
            border: 2px solid #008080;
            border-radius: 30px;
            font-size: 14px;
            transition: border 0.3s ease;
            
        }
        .dropDown
        {
             width: 100%;
            padding: 10px 15px;
            font-size: 12px;
            transition: border 0.3s ease;
        }
        .search-box input:focus {
            border-color: #006666;
            outline: none;
        }

        .form-group {
            flex-basis: 300px;
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

            .filter-container {
                flex-direction: column;
                align-items: flex-start;
            }

            .search-box {
                width: 100%;
                margin-bottom: 10px;
            }

            .form-group {
                width: 100%;
            }
        }
    </style>
--%>
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

.back-link {
    display: inline-flex;
    align-items: center;
    text-decoration: none;
    font-weight: 600;
    color: #666666;
    text-align: left;
    letter-spacing: 1px;
}

.back-link:hover {
    color: #333333;
    text-decoration: none;
}

.back-link span {
    margin-right: 10px;
    font-size: 18px;
}

h2 {
    color: black;
    text-align: left;
    font-weight: normal;
    letter-spacing: 1px;
    font-size: 25px;
    margin-left: 20px;
}



.program-link {
    color: #666666;
    font-weight: 600;
    text-decoration: none;
}

.program-link:hover {
    text-decoration: underline;
    color: #333333;
}

.no-applicants {
    color: red;
}

.has-applicants {
    color: #333333;
}

.filter-container {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
}



.search-box input {
    width: 100%;
    padding: 10px 15px;
    border: 1px solid #cccccc;
    border-radius: 0;
    font-size: 14px;
    transition: border 0.3s ease;
}

.dropDown {
    width: 100%;
    padding: 10px 15px;
    font-size: 12px;
    transition: border 0.3s ease;
}

.search-box input:focus {
    border-color: #cccccc;
    outline: none;
}

.form-group {
    flex-basis: 300px;
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

    .filter-container {
        flex-direction: column;
        align-items: flex-start;
    }

    .search-box {
        width: 100%;
        margin-bottom: 10px;
    }

    .form-group {
        width: 100%;
    }
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
   <script type="text/javascript">
       function filterTable() {
           var input = document.getElementById("txtSearch");
           var filter = input.value.toLowerCase();
           var table = document.getElementById("GridViewCourses");
           var rows = table.getElementsByTagName("tr");

           for (var i = 1; i < rows.length; i++) {
               // Get the Program Name and Program Prefix cells
               var programCell = rows[i].getElementsByTagName("td")[1]; // Program Name column
               var prefixCell = rows[i].getElementsByTagName("td")[2];  // Program Prefix column

               if (programCell && prefixCell) {
                   var programText = programCell.textContent || programCell.innerText;
                   var prefixText = prefixCell.textContent || prefixCell.innerText;

                   // Check if the search input matches either Program Name or Program Prefix
                   if (programText.toLowerCase().indexOf(filter) > -1 || prefixText.toLowerCase().indexOf(filter) > -1) {
                       rows[i].style.display = "";
                   } else {
                       rows[i].style.display = "none";
                   }
               }
           }
       }
</script>

</head>
<body>
    <form id="form1" runat="server">
     <div id="ApplicationsInstructions" runat="server">
<div style="position: fixed; top: 10%; right: 0%; width: 250px; background: #fff; border: 1px solid #fff; border-radius: 8px; padding: 16px; font-family: Arial, sans-serif;">
    <small style ="position:absolute ;" > Instructions !<br /></br>
    <i class="fa-solid fa-star" style="color: #1ed085;"></i>Click on the No. of applicants for specific programs to see them all.</br>  
   <i class="fa-solid fa-star" style="color: #1ed085;"></i>If No. of applicants is "0" then it means that is not clickable and there is no applicant for this program.</br>  
    <i class="fa-solid fa-star" style="color: #1ed085;"></i> You can also search in above search bar by program or no. of applicants.</br>
     <i class="fa-solid fa-star" style="color: #1ed085;"></i> Click on "Submit" button to submit the application.</br>
      <i class="fa-solid fa-star" style="color: #1ed085;"></i> By default it shows the All Applicants but for specific Status like new registration, document Verified, etc select from dropdown.</br>
</small>
</div>
</div>
        <div class="container">
           <div style="display:flex;justify-content:space-between;">
            <div style="display:flex;">
            <asp:LinkButton ID="backbotton" class="backbotton" runat="server"><i class="fa-solid fa-arrow-left"></i></asp:LinkButton> &nbsp;
                <h2>Applications (Programs)</h2>
            </div>
                
                 <div class="search-box">
                    <input type="text" id="txtSearch" placeholder="Search Programs..." onkeyup="filterTable()" />
                </div>
            </div>
            <script>
                function goBack() {
                    window.history.back();
                }
            </script>
            <hr />

            <!-- Search Box and Dropdown in one row -->
            <div class="filter-container">
                <!-- Dropdown for filtering applicants -->
                <div class="form-group">
                 <%--   <label for="ddlStatusFilter">Filter by Application Status:</label>--%>

                 <div class="dropDown"> 
                 <asp:DropDownList ID="ddlStatusFilter" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged" CssClass="form-control">
                        <asp:ListItem Text="All Applicants" Value="All"></asp:ListItem>
                        <asp:ListItem Text="New List" Value="NewList"></asp:ListItem>
                        <asp:ListItem Text="Document Verified" Value="DocVerified"></asp:ListItem>
                        <asp:ListItem Text="Registration Approved" Value="Verified"></asp:ListItem>
                        <asp:ListItem Text="Admission Approved" Value="Approved"></asp:ListItem>
                        <asp:ListItem Text="Application Rejected" Value="Rejected"></asp:ListItem>
                    </asp:DropDownList>
                 </div>
                    
                </div>
            </div>

            <!-- Programs GridView -->
 <asp:GridView ID="GridViewCourses" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" OnRowDataBound="GridViewCourses_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Sr. No">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
            <ItemStyle CssClass="sr-no" />
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Programs">
            <ItemTemplate>
                <asp:HyperLink ID="lnkProgram" runat="server" 
    NavigateUrl='<%# Eval("Courseid", "AdmissionApprove.aspx?courseid={0}&status=" + ddlStatusFilter.SelectedValue) %>' 
    CssClass="program-link" 
    Enabled='<%# Convert.ToInt32(Eval("NoOfApplicants")) > 0 %>'>
    <%# Eval("Course") %>
</asp:HyperLink>

            </ItemTemplate>
        </asp:TemplateField>

       <%-- <asp:BoundField DataField="Coursecode" HeaderText="Programs Shortcuts" />--%>

        <asp:TemplateField HeaderText="No. of Applicants">
            <ItemTemplate>
               <asp:HyperLink ID="lnkApplicants" runat="server" 
    NavigateUrl='<%# Eval("Courseid", "AdmissionApprove.aspx?courseid={0}&status=" + ddlStatusFilter.SelectedValue) %>' 
    Text='<%# Eval("NoOfApplicants") %>' 
    CssClass='<%# If(Convert.ToInt32(Eval("NoOfApplicants")) > 0, "has-applicants", "no-applicants") %>' 
    Enabled='<%# Convert.ToInt32(Eval("NoOfApplicants")) > 0 %>'>
</asp:HyperLink>

            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

        </div>
    </form>
</body>
</html>
