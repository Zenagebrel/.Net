<%@ Page Language="C#" AutoEventWireup="true"  Debug="true" CodeFile="Collect.aspx.cs" Inherits="Collect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Collect Layer</title>
    <style type="text/css">
        

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="border:solid;width:70%;height:600px;border-width:thin;width:100%">
            
            <table style="widows:100%">
                <tr>
                    <td class="auto-style2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Size="24pt" ForeColor="#993300" Text="Event Manager"></asp:Label>
                        <br />
                        <hr />
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2"><asp:DropDownList ID="ddnSite" runat="server" Font-Size="20px" BackColor="#66CCFF" DataSourceID="SqlDataSource1" DataTextField="Deployment" DataValueField="Deployment" Width="200px">

                   </asp:DropDownList>
                       
                    &nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddnEvent" CssClass="ddnEvent" runat="server" Font-Size="20px" BackColor="#66CCFF" DataSourceID="SqlDataSource2" DataTextField="Event" DataValueField="Event" Width="200px">

                   </asp:DropDownList>
                       
                    &nbsp;
                        <asp:Button ID="btnADD" runat="server" OnClick="btnADD_Click" Text="Add Event" BorderColor="#66CCFF" BorderStyle="Solid" Height="30px" />
&nbsp;<asp:Button ID="btnTruncate" runat="server" OnClick="btnTruncate_Click" Text="Truncate Event" BorderColor="#66CCFF" BorderStyle="Solid" Height="30px" />
                    </td>
                    <td>&nbsp;&nbsp;
                         <div style="border-style: solid; border-color: inherit; border-width: thin;  text-align: center; vertical-align: middle;" class="auto-style4">
                             <br />
                        <asp:Button ID="btnTruncate0" runat="server" OnClick="btnTruncate0_Click" Text="Simulate Environment Data" BorderColor="#66CCFF" BorderStyle="Solid" Height="30px" />
                             &nbsp;&nbsp;&nbsp;&nbsp;
                             <asp:Button ID="btnPreprocess" runat="server" BorderColor="#66CCFF" Height="30px" OnClick="btnPreprocess_Click" Text="Save and Preprocess" />
                             </div>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">
                        <asp:GridView ID="gridEvent" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" />
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>
                        <br />
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:WildfireSimulator %>" SelectCommand="SELECT * FROM [tbl_Sites]"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:WildfireSimulator %>" SelectCommand="select stimuli +'('+ State + ')'  As Event from tbl_Threashold"></asp:SqlDataSource>
                    </td>
                    <td style="vertical-align: top;width:50%">&nbsp;
                        <div style="border-style: solid; border-color: inherit; width:112%; height:450px; border-width: thin; background-color: #336699; overflow-y:scroll">

                            <asp:Label ID="lblSensorData" runat="server" Font-Italic="True" Font-Size="20px" ForeColor="White"></asp:Label>
                            <br />
                            <asp:GridView ID="gridPreprocess" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>

                            <br />
                            <asp:GridView ID="gridProcess2" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>

                        </div>


                    </td>
                </tr>
                <tr>
                    <td>

                    </td>
                    <td>

                        <asp:Button ID="btnContextualize" runat="server" BorderColor="#66CCFF" BorderStyle="Solid" OnClick="btnNext1_Click" Text="Contextualize" Width="120px" Height="30px" />
&nbsp;
                        <asp:Button ID="btnMap" runat="server" BorderColor="#66CCFF" BorderStyle="Solid" Text="Map" Width="120px" Height="30px" OnClick="btnMap_Click" />

                    </td>

                </tr>
            </table>
            
        </div>
    </form>
</body>
</html>
