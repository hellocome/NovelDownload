<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowLog.aspx.cs" Inherits="FileDownloadWeb.ShowLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="Table1" runat="server" Width="1200px" HorizontalAlign="Center" 
            BackColor="Azure" Font-Names="Courier New" Height="261px">
        <asp:TableRow>
            <asp:TableCell>
                <asp:TextBox ID="TextBoxLog" runat="server" TextMode="MultiLine" ReadOnly="True" Width="100%" Height="100%" Wrap="False" Rows="50"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>    
    </div>
    </form>
</body>
</html>
