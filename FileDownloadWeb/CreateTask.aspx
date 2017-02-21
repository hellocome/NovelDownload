<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateTask.aspx.cs" Inherits="FileDownloadWeb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <div>
        <asp:Table ID="Table1" runat="server" Width="800px" HorizontalAlign="Center" BackColor="Azure" Font-Names="Courier New">
            <asp:TableRow ID="TableRow7" runat="server">
                <asp:TableCell ID="TableCell7" runat="server" HorizontalAlign="Right" VerticalAlign="Top">
                    Novel Name:
                </asp:TableCell>
                <asp:TableCell ID="TableCell8" runat="server">
                    <asp:TextBox ID="tbNovelName" runat="server" Width="90%"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" Width="20%" HorizontalAlign="Right" VerticalAlign="Top">
                    Host:
                </asp:TableCell>
                <asp:TableCell runat="server" Width="80%">
                    <asp:ListBox ID="lbHost" runat="server" Rows="4" Width="50%">
                        <asp:ListItem Text="BIQUGE" Value="BIQUGE" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="BIQUGE.CO" Value="BIQUGE.CO"></asp:ListItem>
                        <asp:ListItem Text="DAYANWENXUE" Value="DAYANWENXUE"></asp:ListItem>
                        <asp:ListItem Text="XIAOSHUOM" Value="XIAOSHUOM"></asp:ListItem>
                        <asp:ListItem Text="SHUKEJU" Value="SHUKEJU"></asp:ListItem>
                        <asp:ListItem Text="Biquge" Value="Biquge"></asp:ListItem>
                        <asp:ListItem Text="WENXUELOU" Value="WENXUELOU"></asp:ListItem>
                        <asp:ListItem Text="BIQUGE.CC" Value="BIQUGE.CC"></asp:ListItem>
                    </asp:ListBox>
                 </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" HorizontalAlign="Right" VerticalAlign="Top">
                    Novel Url:
                </asp:TableCell>
                <asp:TableCell runat="server">
                    <asp:TextBox ID="tbNovelUrl" runat="server" Width="90%"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow runat="server">
                <asp:TableCell runat="server" HorizontalAlign="Right" VerticalAlign="Top">
                    Max Pages:
                </asp:TableCell>
                <asp:TableCell runat="server">
                    <asp:ListBox ID="lbMaxPages" runat="server" Rows="5" Width="50%">
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        <asp:ListItem Text="200" Value="200"></asp:ListItem>
                        <asp:ListItem Text="500" Value="500" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="1000" Value="1000"></asp:ListItem>
                        <asp:ListItem Text="2000" Value="2000"></asp:ListItem>
                        <asp:ListItem Text="5000" Value="5000"></asp:ListItem>
                        <asp:ListItem Text="10000" Value="10000"></asp:ListItem>
                    </asp:ListBox>
                </asp:TableCell>
            </asp:TableRow>

            <asp:TableRow ID="TableRow_NewTable" runat="server">
                <asp:TableCell ID="TableCell9" runat="server" ColumnSpan="2" HorizontalAlign="Center">
                    <asp:UpdatePanel runat="server" id="UpdatePanel1">
                    <ContentTemplate>
                        <asp:Timer ID="timer" runat="server" Interval="500" OnTick="timer_Tick" />
                            <asp:Table ID="Table2" runat="server" Width="100%" HorizontalAlign="Center" BackColor="Azure" Font-Names="Courier New">
                                    <asp:TableRow ID="TableRow_Blank" runat="server">
                                        <asp:TableCell ID="TableCell6" runat="server" ColumnSpan="2" HorizontalAlign="Center">
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow ID="TableRow_Info" runat="server">
                                        <asp:TableCell ID="TableCell3LabelInfo" runat="server" ColumnSpan="2" HorizontalAlign="Center">
                                            <asp:Label ID="LabelInfo" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow ID="TableRow5_Blank" runat="server">
                                        <asp:TableCell ID="TableCell5" runat="server" ColumnSpan="2" HorizontalAlign="Center">
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow ID="TableRow_Btn" runat="server">
                                        <asp:TableCell ID="TableCell4" runat="server" HorizontalAlign="Center" ColumnSpan="2">
                                            <asp:Button ID="btnStart" runat="server" Text="Start" Width="100" OnClick="btnStart_Click" />
                    
                                            <asp:Button ID="btnStop" runat="server" Text="Stop" Width="100" OnClick="btnStop_Click" />

                                            <asp:Button ID="btnCreateWebIndex" runat="server" Text="Create Index" Width="100" OnClick="btnCreateWebIndex_Click" />
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow ID="TableRow7_Blank" runat="server">
                                        <asp:TableCell ID="TableCell1" runat="server"></asp:TableCell>
                                    </asp:TableRow>

                                     <asp:TableRow ID="TableRow9" runat="server">
                                        <asp:TableCell ID="TableCell3LabelProgress" runat="server" ColumnSpan="2" HorizontalAlign="Center">
                                        <asp:Label ID="LabelProgress" runat="server"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                    <asp:TableRow ID="TableRow1" runat="server">
                                        <asp:TableCell ID="TableCell2" runat="server" ColumnSpan="2" HorizontalAlign="Center">
                                            <asp:HyperLink ID="HyperLinkLog" runat="server" Target="_blank">Log File</asp:HyperLink>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </form>
</body>
</html>
