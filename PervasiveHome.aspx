<%@ Page Language="C#" Debug="true"  AutoEventWireup="true"  EnableEventValidation="false"  CodeFile="PervasiveHome.aspx.cs" Inherits="PervasiveHome" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Simulate Wildfire Prevention System</title>
    <link rel="stylesheet" href="CSS/Main.css"/>
    <link rel="icon" href="../Image/wf.ico"/>

  <link rel="stylesheet" href="CSS/jquery-ui.css"/>
  <script src="Javascript/jquery-1.12.4.js"></script>
  <script src="Javascript/jquery-ui.js"></script>


  <link rel="stylesheet" href="CSS/bootstrap.min.css">
  <%--<script src="Javascript/jquery.min.js"></script>--%>
  <script src="Javascript/bootstrap.min.js"></script>

    <script>
        $(function () {
            $("#tabs").tabs();
        });
  </script>
    <style type="text/css">
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table border="1" class="maintable">
        <tr style="background-color:#580d0d; text-align: left;height:46px">
<td >
    <asp:Image ID="Image1" runat="server" ImageUrl="../Image/WFIcon.jpg" Height="50px" Width="100px"/>
            </td> 

<td style="text-align: right">
<asp:Label ID="Label1" runat="server" Text="Simulate Wildfire Prevention" Font-Bold="True" Font-Names="Arial" Font-Size="20px" ForeColor="White"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>

        <tr style="width:80%">
<td style="height:803px; vertical-align: top;" colspan="2">
<div id="tabs">
  <ul>
    <li><a href="#tabs-1">Create Virtual Sensors</a></li>
    <li><a href="#tabs-2">Sensor Deployment</a></li>
    <li><a href="#tabs-3">Sensor Data Collection</a></li>
  </ul>
  <div id="tabs-1" style="height:710px;opacity:10; vertical-align: top; text-align: left;">
<table style="width:70%">
    <tr>
        <td>
            <asp:ImageButton ID="btnCreateDevice1" ImageUrl="~/Image/ADD.png" runat="server" width="73px" height="50px" OnClick="btnCreateDevice_Click"/>
               <%--<button type="button"  data-toggle="modal" data-target="#myModal" style="width: 73px; height: 50px"><img src="Image/ADD.png" style="height: 40px; width: 46px" /></button>--%>
               

        </td>
    </tr>
    <tr>
        <td>         
             
                <asp:Label ID="lblInfo" runat="server" Text=""></asp:Label>
            <br />
            <div class="table-responsive" style="overflow-y:scroll; height:600px">
          <asp:GridView ID="gvDevice" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvDevice_RowDataBound" CssClass="table table-striped">
                    <Columns>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>'>
                                        </asp:Label>
                            </ItemTemplate>
                           
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Device ID" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblID1" runat="server" Text='<%# "Device0"+Eval("ID") %>'>
                                </asp:Label>
                            </ItemTemplate>
                           
                        </asp:TemplateField>


                         <asp:TemplateField HeaderText="" Visible="true">
                            <ItemTemplate>
                                <asp:Image ID="imgDeviceIcon" runat="server" Width="50px" Height="25px"/>
                            </ItemTemplate>
                           
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Device">
                            <ItemTemplate>
                                <asp:Label ID="lblDevice" runat="server" Text='<%# Eval("Sensors") %>'>
                                        </asp:Label>
                            </ItemTemplate>
                            
                        </asp:TemplateField>                      

                    <asp:TemplateField HeaderText="Periodic">
                            <ItemTemplate>
                                <asp:Label ID="lblPeriodic" runat="server" Text='<%# Eval("Periodic") %>'>
                                        </asp:Label>
                            </ItemTemplate>
                            
                        </asp:TemplateField>  
                    <asp:TemplateField HeaderText="Staus">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'>
                                        </asp:Label>
                            </ItemTemplate>
                           
                        </asp:TemplateField>  


                         <asp:TemplateField HeaderText="Site">
                            <ItemTemplate>
                                <asp:Label ID="lblDeployment" runat="server" Text='<%# Eval("Site") %>'>
                                        </asp:Label>
                            </ItemTemplate>
                            
                        </asp:TemplateField> 

                         <asp:TemplateField HeaderText="Topic" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTopic" runat="server" Text='<%# Eval("Topic") %>'>
                                        </asp:Label>
                            </ItemTemplate>
                            
                        </asp:TemplateField> 


                  <asp:TemplateField HeaderText="">
                            <ItemTemplate>                             
                                <asp:ImageButton ID="ImgbtnPower" runat="server" OnClick="ImgbtnPower_Click" Height="25px" Width="55px" />
                                
                                

                            </ItemTemplate>
                           <ItemStyle Width="10px"/>
                        </asp:TemplateField> 
                 <asp:TemplateField HeaderText="">
                            <ItemTemplate>                             
                              <asp:ImageButton ID="btnConfig" runat="server" Width="50px" Height="25px"  ImageUrl="~/Image/Setting.png" OnClick="btnConfig_Click" />
                               
                            </ItemTemplate>
                           <ItemStyle Width="10px"/>
                        </asp:TemplateField> 

                  <asp:TemplateField HeaderText="">
                            <ItemTemplate>                             
                               <asp:ImageButton ID="btnDelete" runat="server" Width="50px" Height="25px"  ImageUrl="~/Image/Delete.png" OnClick="btnDelete_Click" />
                               
                            </ItemTemplate>
                           <ItemStyle Width="10px"/>
                        </asp:TemplateField> 

                    </Columns>
                   <HeaderStyle BackColor="#3596f0" />
                </asp:GridView>
                </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

 
        </td>
    </tr>

</table>


  </div>



  <div id="tabs-2" style="height:760px;opacity:10; vertical-align: top; text-align: left;">

<iframe style="width:100%;height:100%"  src="http://localhost:57608/qgis2web_2021_10_04-21_00_58_069153/index.html#12/6.6045/37.3642"></iframe>

  </div>



  <div id="tabs-3" style="height:760px;opacity:10; vertical-align: top; text-align: left;width:1500px">
     <iframe style="width:100%;height:100%"  src="Collect.aspx"></iframe>

  </div>
</div>
       

</td>

        </tr>

        <tr style="width:10%">
            <td style="background-color: #e25822" colspan="2">

<asp:Label ID="Label2" runat="server" Text="Powered By: Natan IT Solution" Font-Bold="False" Font-Names="Arial" Font-Size="15px" ForeColor="White" Font-Italic="True"></asp:Label>

            </td>

       </tr>

    </table>
    </div>
<%--================================================ Delete Confirmation popUp ==============================================================================--%>

  <cc1:ModalPopupExtender ID="MODDeleteDevice" runat="server" TargetControlID="HiddenField8" PopupControlID="PdeleteDevice"></cc1:ModalPopupExtender>
      <asp:HiddenField ID="HiddenField8" runat="server" />
        <asp:Panel ID="PdeleteDevice" runat="server" Width="500px" Height="179px" display="none">

<div style="background-color:#ffffff; border:4px solid #2faede; height: 181px;border-radius:15px;text-align:center;">
    <table style="width:99%;">
    <tr>
        <td style="width:1%"></td>
        <td style="width:97%;text-align: center; font-family: Nyala; background-color: #922020; height:40px; font-weight: bold; font-size: 14px; ">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" style="vertical-align: middle; font-family: Nyala; font-size: 20px; font-style: italic;">

                <asp:Label ID="Label9" runat="server" Text="Are you sure?, you are going to delete Device!" Font-Italic="true" Font-Size="25px"></asp:Label>
                
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
                <asp:Button ID="btnNo" runat="server" Text="No" Width="100px" Height="40px" BackColor="#66ccff" Font-Bold="true" Font-Size="16px" ForeColor="White"/>
                &nbsp;&nbsp;
                <asp:Button ID="btnYes" runat="server" Text="Yes" Width="100px" Height="40px" BackColor="#993366" Font-Bold="true" Font-Size="16px" ForeColor="White" OnClick="btnYes_Click"/>
                
                <asp:Label ID="lblIdDelete" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    
</div>


        </asp:Panel>


<%--================================================ Create Device PopUp ==============================================================================--%>
  <cc1:ModalPopupExtender ID="MODCreateDevice" runat="server" TargetControlID="HiddenField1" PopupControlID="PCreate"></cc1:ModalPopupExtender>
      <asp:HiddenField ID="HiddenField1" runat="server" />
        <asp:Panel ID="PCreate" runat="server" Width="800px" Height="700px" display="none">

<div style="background-color:#ffffff; border:2px solid #922020; height: 610px;border-radius:15px;text-align:center;width:800px">
    <br />
        <table style="width:100%">
            <tr>
                
                <td colspan="3" style="text-align:left;background-color:#3596f0"">
                    <asp:Label ID="Label3" runat="server" Text="     ADD New Device" ForeColor="Maroon" Font-Size="25px" Font-Bold="true"></asp:Label>
                </td>
            </tr>
              <tr>
                  <td style="width:15%">
                      <asp:Label ID="Label4" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Device Type"></asp:Label></td>
                  <td style="width:45%">
                      <asp:DropDownList CssClass="ddn"   AutoPostBack="true"  ID="ddnDevice" runat="server" OnSelectedIndexChanged="ddnDevice_SelectedIndexChanged" Font-Size="20px"></asp:DropDownList></td>
                  <td style="width:40%"> <asp:Image ID="imgDevice" Width="250px" Height="150px" runat="server" /></td>

              </tr>
          </table>

            <table style="width:100%">
                <tr>
                    <td style="width:15%"><asp:Label ID="Label5" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Periodic"></asp:Label></td>
                    <td style="width:45%"><asp:DropDownList ID="ddnPeriodic" CssClass="ddn" runat="server" Font-Size="20px">
                        <asp:ListItem>0</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>30</asp:ListItem>
                        <asp:ListItem>60</asp:ListItem>
                        <asp:ListItem>300</asp:ListItem>
                        <asp:ListItem>600</asp:ListItem>

                   </asp:DropDownList>
                       
                    </td>
                    <td style="width:40%;text-align:left"> <asp:Label ID="Label6" runat="server" Text="Second" ForeColor="#006600" Font-Size="15px" Font-Italic="true"></asp:Label></td>

                </tr>


            </table>
            <br />
            <br />
              <table style="width:100%">
                <tr>
                    <td style="width:15%"><asp:Label ID="Label7" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Status"></asp:Label></td>
                    <td style="width:45%"><asp:DropDownList ID="ddnstatus" CssClass="ddn" runat="server" Font-Size="20px">
                        <asp:ListItem Text="OFF" Value="1"></asp:ListItem>
                        <asp:ListItem Text="ON" Value="2"></asp:ListItem>
                     </asp:DropDownList>
                    </td>
                    <td style="width:40%"></td>

                </tr>


            </table>

                        <br />
            <br />
              <table style="width:100%">
                <tr>
                    <td style="width:15%"><asp:Label ID="Label8" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Deployment"></asp:Label></td>
                    <td style="width:45%"><asp:DropDownList ID="ddnDeployment" CssClass="ddn" runat="server" Font-Size="20px">
                       
                     </asp:DropDownList>
                    </td>
                    <td style="width:40%"></td>
                
                </tr>
       
            </table>
                 <br />
            <br />

      <table style="width:100%">
                <tr>
                    <td style="width:15%"><asp:Label ID="Label10" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Name/Topic"></asp:Label></td>
                    <td style="width:45%;text-align:left">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtTopic" runat="server" BackColor="White" BorderColor="#66ccff" border-radius="10px" Width="300px" Height="35px"></asp:TextBox>
                    </td>
                    <td style="width:40%"></td>
                
                </tr>
       
            </table>




                                    <br />
            <br />
              <table style="width:100%">
                <tr>
                    <td style="width:15%"></td>
                    <td style="width:45%">
                        <asp:Button CssClass="btn"  ID="btnCreateDevice" runat="server" Text="Create Device" OnClick="CreateNewDevice_Click" />
                     
                    </td>
                    <td style="width:40%">
                        <asp:Button ID="Button1"   class="btn btn-default"  runat="server" Text="Close" OnClick="Button1_Click" />
                    </td>
                
                </tr>
       
            </table>

    
</div>


        </asp:Panel>


<%--=========================================================UPdate Device Popup==============================================================================================--%>
  <cc1:ModalPopupExtender ID="MODUpdateDevice" runat="server" TargetControlID="HiddenField2" PopupControlID="PUpdate"></cc1:ModalPopupExtender>
      <asp:HiddenField ID="HiddenField2" runat="server" />
        <asp:Panel ID="PUpdate" runat="server" Width="800px" Height="700px" display="none">

<div style="background-color:#ffffff; border:2px solid #5ebfe4; height: 560px;border-radius:15px;text-align:center;width:800px">
    <br />
        <table style="width:100%">
            
            <tr>
                <td colspan="3" style="text-align:left;background-color:#3596f0">
                    &nbsp;&nbsp<asp:Label ID="Label11" runat="server" Text="Configure Device" ForeColor="Maroon" Font-Size="25px" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblDeviceIDtoUpdate" runat="server" Text=""></asp:Label>
                </td>
            </tr>
              <tr>
                  <td style="width:15%">
                      <asp:Label ID="Label12" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Device Type"></asp:Label></td>
                  <td style="width:45%">
                      <asp:DropDownList CssClass="ddn"   AutoPostBack="true"  ID="ddnDeviceTypeUpdate" Enabled="false" BackColor="#e9e9e9" runat="server" Font-Size="20px"></asp:DropDownList></td>
                  <td style="width:40%"> <asp:Image ID="ImgDeviceUpdate" Width="250px" Height="150px" runat="server" /></td>

              </tr>
          </table>

            <table style="width:100%">
                <tr>
                    <td style="width:15%"><asp:Label ID="Label13" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Periodic"></asp:Label></td>
                    <td style="width:45%"><asp:DropDownList ID="ddnPeriodic1" CssClass="ddn" runat="server" Font-Size="20px">
                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        <asp:ListItem Text="60" Value="60"></asp:ListItem>
                        <asp:ListItem Text="300" Value="300"></asp:ListItem>
                        <asp:ListItem Text="600" Value="600"></asp:ListItem>

                   </asp:DropDownList>
                       
                    </td>
                    <td style="width:40%;text-align:left"> <asp:Label ID="Label14" runat="server" Text="Second" ForeColor="#006600" Font-Size="15px" Font-Italic="true"></asp:Label></td>

                </tr>


            </table>
            <br />
            <br />

                        <br />
            <br />
              <table style="width:100%">
                <tr>
                    <td style="width:15%"><asp:Label ID="Label16" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Deployment"></asp:Label></td>
                    <td style="width:45%"><asp:DropDownList ID="ddnDeploymentUpdate" CssClass="ddn" runat="server" Font-Size="20px">
                       
                     </asp:DropDownList>
                    </td>
                    <td style="width:40%"></td>
                
                </tr>
       
            </table>
                 <br />
            <br />

      <table style="width:100%">
                <tr>
                    <td style="width:15%"><asp:Label ID="Label17" ForeColor="#990000" Font-Size="15px" Font-Bold="true" runat="server" Text="Name/Topic"></asp:Label></td>
                    <td style="width:45%;text-align:left">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtTopicUpdate" runat="server" BackColor="White" BorderColor="#66ccff" border-radius="10px" Width="300px" Height="35px"></asp:TextBox>
                    </td>
                    <td style="width:40%"></td>
                
                </tr>
       
            </table>




                                    <br />
            <br />
              <table style="width:100%">
                <tr>
                    <td style="width:15%"></td>
                    <td style="width:45%">
                        <asp:Button CssClass="btn"  ID="btnUPdateDevice" runat="server" Text="Save Device" OnClick="btnUPdateDevice_Click" />
                     
                    </td>
                    <td style="width:40%">
                        <asp:Button ID="Button3"   class="btn btn-default"  runat="server" Text="Close" OnClick="Button1_Click" />
                    </td>
                
                </tr>
       
            </table>

    
</div>


        </asp:Panel>































    </form>
</body>
</html>
