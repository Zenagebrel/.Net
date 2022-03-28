using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class PervasiveHome : System.Web.UI.Page
{
    DBConnection DBcon = new DBConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
      //gvDevice.RowDataBound += new GridViewRowEventHandler(gvDevice_RowDataBound);
      // PopulateDevice();
      // gvDevice.HeaderRow.TableSection = TableRowSection.TableHeader;
        
        if (!IsPostBack)
        {
            lblDeviceIDtoUpdate.Visible = false;
            lblIdDelete.Visible = false;
            Populateddn();
            PopulateDevice();
            imgDevice.ImageUrl = "/Image/TempBlue.png";
           

        }
    }

    public void Populateddn()
    {
        DBcon.dataBaseConnection();
        String GetDeviceList = " Select ID,Sensors from tbl_Sensors";
        SqlDataAdapter adaptGetDeviceList = new SqlDataAdapter(GetDeviceList, DBcon.con);
        DataTable dtGetDeviceList = new DataTable();
        adaptGetDeviceList.Fill(dtGetDeviceList);
        ddnDevice.DataSource = dtGetDeviceList;
        ddnDevice.DataTextField = "Sensors";
        ddnDevice.DataValueField = "ID";
        ddnDevice.DataBind();

        ddnDeviceTypeUpdate.DataSource = dtGetDeviceList;
        ddnDeviceTypeUpdate.DataTextField = "Sensors";
        ddnDeviceTypeUpdate.DataValueField = "ID";
        ddnDeviceTypeUpdate.DataBind();

        

        String GetRegionList = " Select ID,Deployment from tbl_Sites";
        SqlDataAdapter adaptGetRegionList = new SqlDataAdapter(GetRegionList, DBcon.con);
        DataTable dtGetRegionList = new DataTable();
        adaptGetRegionList.Fill(dtGetRegionList);
        ddnDeployment.DataSource = dtGetRegionList;
        ddnDeployment.DataTextField = "Deployment";
        ddnDeployment.DataValueField = "ID";
        ddnDeployment.DataBind();

        ddnDeploymentUpdate.DataSource = dtGetRegionList;
        ddnDeploymentUpdate.DataTextField = "Deployment";
        ddnDeploymentUpdate.DataValueField = "ID";
        ddnDeploymentUpdate.DataBind();



    }

    protected void ddnDevice_SelectedIndexChanged(object sender, EventArgs e)
    {
        MODCreateDevice.Show();
        //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "$('#myModal').modal('show')", true);
        
        String Selected = ddnDevice.SelectedValue.ToString();
       

        if(Selected=="2")
        {
            imgDevice.ImageUrl = "/Image/HumidityBlue.png";
        }

        else if(Selected=="3")
        {
            imgDevice.ImageUrl = "/Image/PressureBlue.png";
        }
        else if(Selected=="4")
        {
            imgDevice.ImageUrl = "/Image/MotionSensor.png";
        }

        else if (Selected == "1")
        {
            imgDevice.ImageUrl = "/Image/TempBlue.png";
        }


    }

    public void PopulateDevice()
    {
        DBcon.dataBaseConnection();

        String GetDevice = " Select * from viewSensorList order by ID DESC";
        SqlDataAdapter adaptGetDevice = new SqlDataAdapter(GetDevice, DBcon.con);
        DataTable dtGetDevice = new DataTable();
        adaptGetDevice.Fill(dtGetDevice);
        gvDevice.DataSource = dtGetDevice;
        gvDevice.DataBind();


    }
    protected void CreateNewDevice_Click(object sender, EventArgs e)
    {
        DBcon.dataBaseConnection();
        String Sensor = ddnDevice.SelectedValue.ToString();
        String Periodic = ddnPeriodic.SelectedValue.ToString();
        String Status = ddnstatus.SelectedValue.ToString();
        String Deployment = ddnDeployment.SelectedValue.ToString();
        String Topic = txtTopic.Text.ToString();


        String Getcoordinate = "SELECT TOP 1 ID FROM tbl_CoordinatesList where Flag=0 and SiteID='"+ Deployment + "' ORDER BY NEWID()";
        SqlCommand cmdGetcoordinate = new SqlCommand(Getcoordinate,DBcon.con);
        String DeploymentID =Convert.ToString(cmdGetcoordinate.ExecuteScalar());



        String SaveData = "INSERT INTO [tbl_Sensorslist] VALUES(@Sensor,@Periodic,@Status,@DeploymentID,@Topic,getdate(),getdate())";
        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
        cmdSaveData.Parameters.AddWithValue("@Sensor", Sensor);
        cmdSaveData.Parameters.AddWithValue("@Periodic", Periodic);
        cmdSaveData.Parameters.AddWithValue("@Status", Status);
        cmdSaveData.Parameters.AddWithValue("@DeploymentID", DeploymentID);
        cmdSaveData.Parameters.AddWithValue("@Topic", Topic);
        cmdSaveData.ExecuteNonQuery();

        String UpdateCoordinateStatus = "Update tbl_CoordinatesList set Flag='"+1+"' where ID='"+ DeploymentID + "'";
        SqlCommand cmdUpdateCoordinateStatus = new SqlCommand(UpdateCoordinateStatus,DBcon.con);
        cmdUpdateCoordinateStatus.ExecuteNonQuery();
        DBcon.con.Close();
        PopulateDevice();

        }

      protected void gvDevice_RowDataBound(object sender, GridViewRowEventArgs e)
    {
          if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image DeviceImage = (Image)e.Row.FindControl("imgDeviceIcon");
            ImageButton btnPower = (ImageButton)e.Row.FindControl("ImgbtnPower");
            Label DeviceStatus = (Label)e.Row.FindControl("lblStatus");

            Label Device = (Label)e.Row.FindControl("lblDevice");

            String DeviceName = Device.Text.Trim();



            if (DeviceStatus.Text=="OFF")
            {
                DeviceStatus.ForeColor = System.Drawing.Color.Red;
                btnPower.ImageUrl = "/Image/PowerOFF.png";

                if (DeviceName == "Temprature")
                {
                    DeviceImage.ImageUrl = "~/Image/TempGray1.png";
                }

                else if (DeviceName == "Humidity")
                {
                    DeviceImage.ImageUrl = "~/Image/Humidity Grey.png";
                }

                else if (DeviceName == "Air_pressure")
                {
                    DeviceImage.ImageUrl = "~/Image/PressureGrey.png";
                }
                else if (DeviceName == "Light")
                {
                    DeviceImage.ImageUrl = "~/Image/Humidity Grey.png";
                }

                else if (DeviceName == "Smoke")
                {
                    DeviceImage.ImageUrl = "~/Image/MotionGrey.jpg";
                }
                else if (DeviceName == "Wind_Speed")
                {
                    DeviceImage.ImageUrl = "~/Image/MotionOrange.jpg";
                }
                else if (DeviceName == "Motion")
                {
                    DeviceImage.ImageUrl = "~/Image/MotionRed.jpg";
                }
                else
                {
                    DeviceImage.ImageUrl = "~/Image/MotionGrey.jpg";
                }

                
            }

            else if (DeviceStatus.Text == "ON")
            {
                DeviceStatus.ForeColor = System.Drawing.Color.Green;
                btnPower.ImageUrl = "/Image/PowerON.png";
                if (DeviceName == "Temprature")
                {
                    DeviceImage.ImageUrl = "~/Image/TempBlue.png";
                    
                }

                else if (DeviceName == "Humidity")
                {
                    DeviceImage.ImageUrl = "~/Image/HumidityBlue.png";
                }

                else if (DeviceName == "Air_pressure")
                {
                    DeviceImage.ImageUrl = "~/Image/PressureBlue.png";
                }
                else if (DeviceName == "Motion Sensor")
                {
                    DeviceImage.ImageUrl = "~/Image/MotionSensor.png";
                }

                else if (DeviceName == "Smoke")
                {
                    DeviceImage.ImageUrl = "~/Image/MotionOrange.jpg";
                }

                else if (DeviceName == "Motion")
                {
                    DeviceImage.ImageUrl = "~/Image/MotionSensor.png";
                }

                else if (DeviceName == "Wind_Speed")
                {
                    DeviceImage.ImageUrl = "~/Image/MotionGreen.jpg";
                }
                else if (DeviceName == "Light")
                {
                    DeviceImage.ImageUrl = "~/Image/HumidityGreen.png";
                }

            }
        }
    }
      protected void ImgbtnPower_Click(object sender, ImageClickEventArgs e)
      {
          DBcon.dataBaseConnection();
          var Powerbutton = (Control)sender;
          GridViewRow row = (GridViewRow)Powerbutton.NamingContainer;
          Label lblID = (Label)row.FindControl("lblID");
          Label lblStatus = (Label)row.FindControl("lblStatus");

          String ID=lblID.Text.ToString().Trim();
          String Status=lblStatus.Text.ToString().Trim();

          String GetStatus="Select ID from tbl_Status where Status='"+Status+"'";
          SqlCommand cmdGetStatus=new SqlCommand(GetStatus,DBcon.con);
          string StatusID=cmdGetStatus.ExecuteScalar().ToString();



          if (StatusID == "1")
          {

              String UpdateStatus = "Update tbl_Sensorslist set Status='2' where ID='" + ID + "'";
              SqlCommand cmdUpdateStatus = new SqlCommand(UpdateStatus, DBcon.con);
              cmdUpdateStatus.ExecuteNonQuery();
              lblStatus.ForeColor = System.Drawing.Color.Green;
          }

          if (StatusID == "2")
          {

              String UpdateStatus = "Update tbl_Sensorslist set Status='1' where ID='" + ID + "'";
              SqlCommand cmdUpdateStatus = new SqlCommand(UpdateStatus, DBcon.con);
              cmdUpdateStatus.ExecuteNonQuery();
              lblStatus.ForeColor = System.Drawing.Color.Red;
          }

          DBcon.con.Close();

          PopulateDevice();

      }
      protected void btnDelete_Click(object sender, ImageClickEventArgs e)
      {
          var Deletebutton = (Control)sender;
          GridViewRow row = (GridViewRow)Deletebutton.NamingContainer;
          Label lblIDDelelete = (Label)row.FindControl("lblID");

          lblIdDelete.Text = lblIDDelelete.Text;
          MODDeleteDevice.Show();
      }
      protected void btnYes_Click(object sender, EventArgs e)
      {
          MODDeleteDevice.Show();
          DBcon.dataBaseConnection();
          String DeleteID = lblIdDelete.Text.ToString().Trim();

          Response.Write(DeleteID);
          String DeleteQuery = "Delete tbl_Sensorslist where ID='" + DeleteID + "'";
          SqlCommand cmdDeleteQuery = new SqlCommand(DeleteQuery, DBcon.con);
          cmdDeleteQuery.ExecuteNonQuery();
          DBcon.con.Close();
          MODDeleteDevice.Dispose();
          PopulateDevice();



      }
      protected void btnCreateDevice_Click(object sender, ImageClickEventArgs e)
      {
          MODCreateDevice.Show();
      }

  
      protected void Button1_Click(object sender, EventArgs e)
      {
         
      }
      protected void btnConfig_Click(object sender, ImageClickEventArgs e)
      {
          DBcon.dataBaseConnection();
          var Configurebutton = (Control)sender;
          GridViewRow row = (GridViewRow)Configurebutton.NamingContainer;
          Label lblIDToConfigure1 = (Label)row.FindControl("lblID");
          String IDUPdate = lblIDToConfigure1.Text.ToString();
          lblDeviceIDtoUpdate.Text = IDUPdate;

          String GetData = "Select * from viewSensorList where ID='" + IDUPdate + "'";
          SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
          DataTable dtGetData = new DataTable();
          adaptGetData.Fill(dtGetData);

          String DeviceType = dtGetData.Rows[0]["Sensors"].ToString().Trim();
          String Periodic = dtGetData.Rows[0]["Periodic"].ToString().Trim();
          String Deployment = dtGetData.Rows[0]["Site"].ToString().Trim();
          String Topic = dtGetData.Rows[0]["Topic"].ToString().Trim();

          String GetDiviceType = "Select ID from tbl_Sensors where Sensors='" + DeviceType + "' ";
          SqlCommand cmdGetDiviceType = new SqlCommand(GetDiviceType, DBcon.con);
          String DeviceTypeValue = Convert.ToString(cmdGetDiviceType.ExecuteScalar());

          ddnDeviceTypeUpdate.SelectedValue = DeviceTypeValue;

          ddnPeriodic1.SelectedValue = Periodic;

        String GetSiteID = "Select ID from tbl_Sites where Deployment='"+ Deployment + "'";
        SqlCommand cmdGetSiteID = new SqlCommand(GetSiteID,DBcon.con);
        String SiteID = Convert.ToString(cmdGetSiteID.ExecuteScalar());

        ddnDeploymentUpdate.SelectedValue = SiteID;
        txtTopicUpdate.Text = Topic;

              


          if (DeviceType == "Temprature")
          {
              ImgDeviceUpdate.ImageUrl = "/Image/TempBlue.png";
          }

          else if (DeviceType == "Humidity")
          {
              ImgDeviceUpdate.ImageUrl = "/Image/HumidityBlue.png";
          }

          else if (DeviceType == "Pressure")
          {
              ImgDeviceUpdate.ImageUrl = "/Image/PressureBlue.png";
          }
          else if (DeviceType == "Motion")
          {
              ImgDeviceUpdate.ImageUrl = "/Image/MotionSensor.png";
          }

          else if (DeviceType == "Gas")
          {
              ImgDeviceUpdate.ImageUrl = "/Image/GasBlue.png";
          }

          MODUpdateDevice.Show();
      }
      protected void btnUPdateDevice_Click(object sender, EventArgs e)
      {

          DBcon.dataBaseConnection();
         
          String Periodic = ddnPeriodic1.SelectedValue.ToString();        
          String Deployment = ddnDeploymentUpdate.SelectedValue.ToString();
          String Topic = txtTopicUpdate.Text.ToString();
          String ID = lblDeviceIDtoUpdate.Text.ToString();

        if (Deployment == "1")
        {
            String Coordinate = "6";

            String GetDeploymentiD = "Select Deployment from tbl_Sensorslist where ID ='" + ID + "'";
            SqlCommand cmdGetDeploymentiD = new SqlCommand(GetDeploymentiD, DBcon.con);
            String DeployemntID = Convert.ToString(cmdGetDeploymentiD.ExecuteScalar());

            String Updatecoordinate = "update tbl_CoordinatesList set Flag='0' where ID='" + DeployemntID + "'";
            SqlCommand cmdUpdatecoordinate = new SqlCommand(Updatecoordinate, DBcon.con);
            cmdUpdatecoordinate.ExecuteNonQuery();

            String SaveData = "Update [tbl_Sensorslist] set Periodic=@Periodic,Deployment=@Coordinate,Topic=@Topic Where ID='" + ID + "' ";
            SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
            cmdSaveData.Parameters.AddWithValue("@Periodic", Periodic);
            cmdSaveData.Parameters.AddWithValue("@Coordinate", Coordinate);
            cmdSaveData.Parameters.AddWithValue("@Topic", Topic);
            cmdSaveData.ExecuteNonQuery();
            DBcon.con.Close();
            PopulateDevice();


        }


        else
        {
            String GetSiteID = "Select distinct SiteID from viewSensorList where ID ='" + ID + "'";
            SqlCommand cmdGetSiteID = new SqlCommand(GetSiteID, DBcon.con);
            String SiteID = Convert.ToString(cmdGetSiteID.ExecuteScalar());

            if (SiteID == Deployment)

            {
                //String GetCoordinateID = "Select Deployment from tbl_Sensorslist where ID ='" + ID + "'";
                //SqlCommand cmdGetCoordinateID = new SqlCommand(GetCoordinateID, DBcon.con);
                //String CoordinateID = Convert.ToString(cmdGetCoordinateID.ExecuteScalar());


                String SaveData = "Update [tbl_Sensorslist] set Periodic=@Periodic,Topic=@Topic Where ID='" + ID + "' ";
                SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                cmdSaveData.Parameters.AddWithValue("@Periodic", Periodic);
                cmdSaveData.Parameters.AddWithValue("@Topic", Topic);
                cmdSaveData.ExecuteNonQuery();
                DBcon.con.Close();
                PopulateDevice();
            }
            else
            {
                String GetDeploymentiD = "Select Deployment from tbl_Sensorslist where ID ='" + ID + "'";
                SqlCommand cmdGetDeploymentiD = new SqlCommand(GetDeploymentiD, DBcon.con);
                String DeployemntID = Convert.ToString(cmdGetDeploymentiD.ExecuteScalar());

                String Updatecoordinate = "update tbl_CoordinatesList set Flag ='0' where ID='" + DeployemntID + "'";
                SqlCommand cmdUpdatecoordinate = new SqlCommand(Updatecoordinate, DBcon.con);
                cmdUpdatecoordinate.ExecuteNonQuery();


                String Getcoordinate = "SELECT TOP 1 ID FROM tbl_CoordinatesList where Flag=0 and SiteID='" + Deployment + "' ORDER BY NEWID()";
                SqlCommand cmdGetcoordinate = new SqlCommand(Getcoordinate, DBcon.con);
                String DeploymentID = Convert.ToString(cmdGetcoordinate.ExecuteScalar());


                String SaveData = "Update [tbl_Sensorslist] set Periodic=@Periodic,Deployment=@DeploymentID,Topic=@Topic Where ID='" + ID + "' ";
                SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                cmdSaveData.Parameters.AddWithValue("@Periodic", Periodic);
                cmdSaveData.Parameters.AddWithValue("@DeploymentID", DeploymentID);
                cmdSaveData.Parameters.AddWithValue("@Topic", Topic);
                cmdSaveData.ExecuteNonQuery();


                String GetDeploymentiD1 = "Select Deployment from tbl_Sensorslist where ID ='" + ID + "'";
                SqlCommand cmdGetDeploymentiD1 = new SqlCommand(GetDeploymentiD1, DBcon.con);
                String DeployemntID2 = Convert.ToString(cmdGetDeploymentiD1.ExecuteScalar());

                String Updatecoordinate1 = "update tbl_CoordinatesList set Flag='1' where ID='" + DeployemntID2 + "'";
                SqlCommand cmdUpdatecoordinate1 = new SqlCommand(Updatecoordinate1, DBcon.con);
                cmdUpdatecoordinate1.ExecuteNonQuery();

                DBcon.con.Close();
                PopulateDevice();
            }
        }






      }
}