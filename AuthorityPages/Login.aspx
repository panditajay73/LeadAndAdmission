<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="AuthorityPages_Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login Page</title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/jquery.validate/1.19.3/jquery.validate.min.js"></script>

    <style>
        body {
            background-color: #f0f8f8;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .container {
            max-width: 400px;
            margin-top: 80px;
            padding: 30px;
            background-color: white;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 128, 128, 0.2);
        }

        h2 {
            color: #008080;
            text-align: center;
            margin-bottom: 30px;
            font-weight: bold;
        }

        .form-control {
            border-radius: 30px;
            padding: 15px;
            font-size: 16px;
        }

        .btn {
            background-color: #008080;
            color: white;
            border-radius: 30px;
            padding: 10px 20px;
            width: 100%;
            font-size: 18px;
            font-weight: bold;
        }

        .btn:hover {
            background-color: #006666;
            color:#fff;
        }

        .modal-header {
            background-color: #008080;
            color: white;
        }

        .modal-title {
            font-size: 24px;
        }

        .close {
            color: white;
        }

        .modal-content {
            height:400px;
            overflow-y: auto;
        }

        .modal-dialog {
            max-width: 400px;
        }

        .modal-body {
            max-height: 300px; /* Adjust this value for a larger or smaller modal height */
            overflow-y: auto;
        }

        .form-group {
            margin-bottom: 20px;
        }
        table #rblRoles .form-control
        {
         display:none;
         }
        /* Small screen responsiveness */
        @media (max-width: 576px) {
            .container {
                margin-top: 50px;
                padding: 20px;
            }

            h2 {
                font-size: 24px;
            }

            .btn {
                font-size: 16px;
            }

            .modal-body {
                max-height: 200px; /* Adjust for smaller screens */
            }
        }
        
    </style>

    <script type="text/javascript">
        // Open the role selection popup on login click
        function openRolePopup() {
            $('#roleModal').modal('show');
        }

        // Trigger the postback when a radio button (role) is selected
        function onRoleSelected() {
            __doPostBack('<%= rblRoles.ClientID %>', '');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Sign In</h2>
            <div class="form-group">
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email" />
            </div>
            <div class="form-group">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password" />
            </div>

            <div class="form-group">
                <asp:Button ID="btnSignIn" runat="server" Text="Sign In" CssClass="btn" OnClick="btnSignIn_Click" />
            </div>

            <!-- Role Selection Modal -->
            <div class="modal " id="roleModal">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title">Select Role</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>
                        <div class="modal-body">
                            <asp:RadioButtonList ID="rblRoles" runat="server" RepeatDirection="Vertical" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="rblRoles_SelectedIndexChanged" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
