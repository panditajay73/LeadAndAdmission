<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnLogin" Text="Login" runat="server" OnClick="Login" />
<asp:Panel ID="pnlProfile" runat="server" Visible="false">
    <hr />
    <table>
        <tr>
            <td rowspan="6" valign="top">
                <asp:Image ID="imgProfile" runat="server" Width="50" Height="50" />
            </td>
        </tr>
        <tr>
            <td>ID: <asp:Label ID="lblId" runat="server" Text=""></asp:Label><td>
        </tr>
        <tr>
            <td>Name: <asp:Label ID="lblName" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Email: <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Mobile: <asp:Label ID="lblMobile" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Verified Email: <asp:Label ID="lblVerified" runat="server" Text=""></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
    </div>
    </form>
</body>
</html>
