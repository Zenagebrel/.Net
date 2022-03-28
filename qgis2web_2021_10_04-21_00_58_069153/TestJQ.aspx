<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestJQ.aspx.cs" Inherits="qgis2web_2021_10_04_21_00_58_069153_TestJQ" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-3.2.1.min.js"></script>
   <%--<script type="text/javascript">
     $(document).ready(function(){

            $.ajax({
                type: "POST",
                url: "GetSensorDeploymetLoc.asmx/ToJson",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    alert(data.d);
                },
                error: function(e) {
                    alert("WebSerivce unreachable");
                }
            });
        });
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server">
            </asp:GridView>
            <br />
            <asp:GridView ID="GridView2" runat="server">
            </asp:GridView>
        </div>
    </form>
</body>
</html>
