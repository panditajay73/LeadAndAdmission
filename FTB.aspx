<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FTB.aspx.vb" Inherits="FTB" ValidateRequest="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rich Text Box Example</title>
    <!-- Include CKEditor from CDN -->
    <script src="https://cdn.ckeditor.com/4.21.0/standard/ckeditor.js"></script>
    <style>
    
    .cke_notification_warning
    {
        display:none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label for="txtRichText">Enter your rich text:</label>
            <textarea id="txtRichText" name="txtRichText" rows="10" cols="80"></textarea>
            <br /><br />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
            <br /><br />
            <asp:Label ID="lblOutput" runat="server" ForeColor="Blue"></asp:Label>
        </div>
    </form>

    <!-- Initialize CKEditor -->
    <script>
        CKEDITOR.replace('txtRichText');
    </script>
</body>
</html>
</html>
