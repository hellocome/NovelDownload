<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuildIndex.aspx.cs" Inherits="BuildIndexWeb.BuildIndex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="TextBoxCMD" runat="server" Height="22px" Width="286px"></asp:TextBox>
        <asp:Button ID="BtnExecute" runat="server" onclick="BtnExecute_Click" 
            Text="Execute" />
    
    </div>
    <br />
    <asp:Literal ID="LiteralViewContent" runat="server"></asp:Literal>
    </form>
</body>
</html>
