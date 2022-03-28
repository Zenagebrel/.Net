using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


using System.Web.Services;

using System.Web.Script.Serialization;
using System.Text;
using System.Web.Script.Services;
using Newtonsoft.Json;








public partial class Collect : System.Web.UI.Page
{
    DBConnection DBcon = new DBConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DBcon.dataBaseConnection();
            String GetData = "Select * from tbl_temporaryEvent order by ID DESC";
            SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
            DataTable dtGetData = new DataTable();
            adaptGetData.Fill(dtGetData);
            gridEvent.DataSource = dtGetData;
            gridEvent.DataBind();
            DBcon.con.Close();

            btnContextualize.Visible = false;
            btnMap.Visible = false;
        }
    }

    protected void btnADD_Click(object sender, EventArgs e)
    {
        DBcon.dataBaseConnection();
        String Site = ddnSite.SelectedValue.ToString();
        String Event = ddnEvent.SelectedValue.ToString();

        String CheckData = "Select * from tbl_temporaryEvent where Site='"+Site+ "' and Event='"+Event+"'";
        SqlDataAdapter adaptCheckData = new SqlDataAdapter(CheckData,DBcon.con);
        DataTable dtCheckData = new DataTable();
        adaptCheckData.Fill(dtCheckData);
        if(dtCheckData.Rows.Count>0)
        {

        }
        else
        {
            String SaveData = "INSERT INTO [tbl_temporaryEvent] VALUES(@Site,@Event)";
            SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
            cmdSaveData.Parameters.AddWithValue("@Site", Site);
            cmdSaveData.Parameters.AddWithValue("@Event", Event);
            cmdSaveData.ExecuteNonQuery();


            String GetData = "Select * from tbl_temporaryEvent order by ID DESC";
            SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
            DataTable dtGetData = new DataTable();
            adaptGetData.Fill(dtGetData);
            gridEvent.DataSource = dtGetData;
            gridEvent.DataBind();


        }

    }

    protected void btnTruncate_Click(object sender, EventArgs e)
    {
        DBcon.dataBaseConnection();
        String TruncateData = "Truncate table [tbl_temporaryEvent]";
        SqlCommand cmdTruncateData = new SqlCommand(TruncateData, DBcon.con);
        cmdTruncateData.ExecuteNonQuery();

        String TruncateProcessed1 = "Truncate table [tblProcessed1]";
        SqlCommand cmdTruncateProcessed1 = new SqlCommand(TruncateProcessed1, DBcon.con);
        cmdTruncateProcessed1.ExecuteNonQuery();

        String GetData = "Select * from tbl_temporaryEvent order by ID DESC";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);
        gridEvent.DataSource = dtGetData;
        gridEvent.DataBind();

        DBcon.con.Close();
    }

    public void SimulateSensor()
    {
        DBcon.dataBaseConnection();
        String GetAllSites = "Select Deployment from tbl_Sites where ID>1";
        SqlDataAdapter adaptGetAllSites = new SqlDataAdapter(GetAllSites,DBcon.con);
        DataTable dtGetAllSites = new DataTable();
        adaptGetAllSites.Fill(dtGetAllSites);

        for(int i=0;i< dtGetAllSites.Rows.Count;i++)
        {

            String Site = dtGetAllSites.Rows[i]["Deployment"].ToString();

            ////////////////////// ///Check for events//////////////////////////////////////////////////////////

            String CheckForEvents = "Select distinct Site from tbl_temporaryEvent where Site='"+ Site + "'";
            SqlDataAdapter adaptCheckForEvents = new SqlDataAdapter(CheckForEvents,DBcon.con);
            DataTable dtCheckForEvents = new DataTable();
            adaptCheckForEvents.Fill(dtCheckForEvents);

            if (dtCheckForEvents.Rows.Count > 0)
            {
                ///////////////////////////////////Read Event Data////////////////////////////////////
                for (int j = 0; j < dtCheckForEvents.Rows.Count; j++)
                {
                    String EventSite = dtCheckForEvents.Rows[j]["Site"].ToString();
                    lblSensorData.Text = lblSensorData.Text + EventSite + "*";

                    String GetEventList = "Select Event from tbl_temporaryEvent where Site='" + EventSite + "'";
                    SqlDataAdapter adaptGetEventList = new SqlDataAdapter(GetEventList, DBcon.con);
                    DataTable dtGetEventList = new DataTable();
                    adaptGetEventList.Fill(dtGetEventList);

                    for (int k = 0; k < dtGetEventList.Rows.Count; k++)
                    {
                        String CapturedEvent = dtGetEventList.Rows[k]["Event"].ToString();

                        String GetStimuli = "Select Stimuli from viewThreashold where SampleEvent='" + CapturedEvent + "'";
                        SqlCommand cmdGetStimuli = new SqlCommand(GetStimuli, DBcon.con);
                        String Stimuli = Convert.ToString(cmdGetStimuli.ExecuteScalar());

                        String GetState = "Select State from viewThreashold where SampleEvent='" + CapturedEvent + "'";
                        SqlCommand cmdGetState = new SqlCommand(GetState, DBcon.con);
                        String State = Convert.ToString(cmdGetState.ExecuteScalar());


                        String GetDeviceList = "Select distinct DeviceID from [ViewSensorDeployment] where Sensors='" + Stimuli + "' and Status='ON' and Deployment='" + EventSite + "'";
                        SqlDataAdapter adaptGetDeviceList = new SqlDataAdapter(GetDeviceList, DBcon.con);
                        DataTable dtGetDeviceList = new DataTable();
                        adaptGetDeviceList.Fill(dtGetDeviceList);

                        for (int l = 0; l < dtGetDeviceList.Rows.Count; l++)
                        {
                            String DeviceID = dtGetDeviceList.Rows[l]["DeviceID"].ToString();

                            lblSensorData.Text = lblSensorData.Text +"#"+ DeviceID+"-";

                            String GetSensor = "Select distinct Sensors from ViewSensorDeployment where DeviceID='" + DeviceID + "'";
                            SqlCommand cmdGetSensor = new SqlCommand(GetSensor, DBcon.con);
                            String Sensor1 = Convert.ToString(cmdGetSensor.ExecuteScalar());

                            String GetNormalStartThreshold = "Select ThreasholdStart from viewThreashold where State='Normal' and Stimuli='" + Stimuli + "'";
                            SqlCommand cmdGetNormalStartThreshold = new SqlCommand(GetNormalStartThreshold, DBcon.con);
                            Int32 NormalThreashold = Convert.ToInt32(cmdGetNormalStartThreshold.ExecuteScalar());

                            String GetRiskyStartThreshold = "Select ThreasholdStart from viewThreashold where State='Risky' and Stimuli='" + Stimuli + "'";
                            SqlCommand cmdGetRiskyStartThreshold = new SqlCommand(GetRiskyStartThreshold, DBcon.con);
                            Int32 RiskyThreashold = Convert.ToInt32(cmdGetRiskyStartThreshold.ExecuteScalar());

                            String GetBadStartThreshold = "Select ThreasholdStart from viewThreashold where State='Bad' and Stimuli='" + Stimuli + "'";
                            SqlCommand cmdGetBadStartThreshold = new SqlCommand(GetBadStartThreshold, DBcon.con);
                            Int32 BadThreashold = Convert.ToInt32(cmdGetBadStartThreshold.ExecuteScalar());

                            DateTime now = DateTime.Now;

                            if (Sensor1 == "Temprature")
                            {


                                if (State == "Normal")
                                {
                                    String ReadValue = "Select TOP 1 Temprature from tbl_Readings where Temprature <='" + NormalThreashold + "' ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String NormalTemp = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = NormalTemp + "." + Fraction + "°C";
                                    String ReadingValue = NormalTemp + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                                else if (State == "Risky")
                                {
                                    String ReadValue = "Select TOP 1 Temprature from tbl_Readings where Temprature >'" + NormalThreashold + "' and Temprature < '" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String RiskylTemp = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = RiskylTemp + "." + Fraction + "°C";
                                    String ReadingValue = RiskylTemp + "." + Fraction;

                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();



                                }

                                else if (State == "Bad")
                                {
                                    String ReadValue = "Select TOP 1 Temprature from tbl_Readings where Temprature >'" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String BadlTemp = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = BadlTemp + "." + Fraction + "°C";

                                    String ReadingValue = BadlTemp + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                            }

                            else if (Sensor1 == "Humidity")
                            {

                                if (State == "Normal")
                                {
                                    String ReadValue = "Select TOP 1 Humidity from tbl_Readings where Humidity >='" + NormalThreashold + "' ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String NormalHumid = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = NormalHumid + "." + Fraction + "%";
                                    String ReadingValue = NormalHumid + "." + Fraction;

                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                                else if (State == "Risky")
                                {
                                    String ReadValue = "Select TOP 1 Humidity from tbl_Readings where Humidity <'" + NormalThreashold + "' and Humidity > '" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String RiskyHumid = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = RiskyHumid + "." + Fraction + "%";
                                    String ReadingValue = RiskyHumid + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);
                                    cmdSaveData.ExecuteNonQuery();

                                }

                                else if (State == "Bad")
                                {
                                    String ReadValue = "Select TOP 1 Humidity from tbl_Readings where Humidity <'" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String BadlHumid = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = BadlHumid + "." + Fraction + "%";

                                    String ReadingValue = BadlHumid + "." + Fraction;

                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                            }

                            else if (Sensor1 == "Air_pressure")
                            {
                                if (State == "Normal")
                                {
                                    String ReadValue = "Select TOP 1 Air_Pressure from tbl_Readings where Air_Pressure <='" + NormalThreashold + "' ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String NormalAirPressure = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = NormalAirPressure + "." + Fraction + "mbar";
                                    String ReadingValue = NormalAirPressure + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                                else if (State == "Risky")
                                {
                                    String ReadValue = "Select TOP 1 Air_Pressure from tbl_Readings where Air_Pressure >'" + NormalThreashold + "' and Air_Pressure < '" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String RiskyAirPressure = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = RiskyAirPressure + "." + Fraction + "mbar";
                                    String ReadingValue = RiskyAirPressure + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();



                                }

                                else if (State == "Bad")
                                {
                                    String ReadValue = "Select TOP 1 Air_Pressure from tbl_Readings where Air_Pressure >'" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String BadlAirPressure = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                   // String ReadingValue = BadlAirPressure + "." + Fraction + "mbar";
                                    String ReadingValue = BadlAirPressure + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }

                            }
                            else if (Sensor1 == "Light")
                            {
                                if (State == "Normal")
                                {
                                    String ReadValue = "Select TOP 1 Light from tbl_Readings where Light='1' or Light='0' ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String NormalLight = Convert.ToString(cmdReadValue.ExecuteScalar());



                                    //String ReadingValue = NormalLight + "Light";
                                    String ReadingValue = NormalLight;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }
                            }
                            else if (Sensor1 == "Smoke")
                            {
                                if (State == "Normal")
                                {
                                    String ReadValue = "Select TOP 1 Smoke from tbl_Readings where Smoke <='" + NormalThreashold + "' ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String NormalSmoke = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = NormalSmoke + "." + Fraction + "opacity";
                                    String ReadingValue = NormalSmoke + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                                else if (State == "Risky")
                                {
                                    String ReadValue = "Select TOP 1 Smoke from tbl_Readings where Smoke >'" + NormalThreashold + "' and Smoke < '" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String RiskySmoke = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = RiskySmoke + "." + Fraction + "opacity";
                                    String ReadingValue = RiskySmoke + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();



                                }

                                else if (State == "Bad")
                                {
                                    String ReadValue = "Select TOP 1 Smoke from tbl_Readings where Smoke >'" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String BadSmoke = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = BadSmoke + "." + Fraction + "opacity";
                                    String ReadingValue = BadSmoke + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }
                            }

                            else if (Sensor1 == "Motion")
                            {

                                if (State == "Normal")
                                {


                                    String ReadingValue = "0";

                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                                else if (State == "Risky")
                                {
                                    String ReadingValue = "1";

                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();



                                }


                            }

                            else if (Sensor1 == "Wind_Speed")
                            {
                                if (State == "Normal")
                                {
                                    String ReadValue = "Select TOP 1 Wind_Speed from tbl_Readings where Wind_Speed <='" + NormalThreashold + "' ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String NormalWind_Speed = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    // String ReadingValue = NormalWind_Speed + "." + Fraction + "km/h";
                                    String ReadingValue = NormalWind_Speed + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                                else if (State == "Risky")
                                {
                                    String ReadValue = "Select TOP 1 Wind_Speed from tbl_Readings where Wind_Speed >'" + NormalThreashold + "' and Wind_Speed < '" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String RiskyWind_Speed = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    // String ReadingValue = RiskyWind_Speed + "." + Fraction + "km/h";
                                    String ReadingValue = RiskyWind_Speed + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();



                                }

                                else if (State == "Bad")
                                {
                                    String ReadValue = "Select TOP 1 Wind_Speed from tbl_Readings where Wind_Speed >'" + BadThreashold + "'  ORDER BY NEWID()";
                                    SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                                    String BadWind_Speed = Convert.ToString(cmdReadValue.ExecuteScalar());

                                    String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                                    SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                                    String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                                    //String ReadingValue = BadWind_Speed + "." + Fraction + "km/h";
                                    String ReadingValue = BadWind_Speed + "." + Fraction;
                                    lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                                    String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                                    SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                                    cmdSaveData.Parameters.AddWithValue("@Site", Site);
                                    cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                                    cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                                    cmdSaveData.Parameters.AddWithValue("@now", now);

                                    cmdSaveData.ExecuteNonQuery();

                                }


                            }
                            else
                            {

                            }
                        }


                    }




                    //lblSensorData.Text = lblSensorData.Text + ";";




                }

                lblSensorData.Text = lblSensorData.Text + ";";
            }

            

            else
            {
                /////////////////////////////////Read Normal Data////////////////////
                String GetDeviceList = "Select distinct DeviceID from [ViewSensorDeployment] where Status='ON' and Deployment='" + Site + "'";
                SqlDataAdapter adaptGetDeviceList = new SqlDataAdapter(GetDeviceList, DBcon.con);
                DataTable dtGetDeviceList = new DataTable();
                adaptGetDeviceList.Fill(dtGetDeviceList);
                

                if(dtGetDeviceList.Rows.Count>0)
                {
                    lblSensorData.Text = lblSensorData.Text + Site + "*";
                    for (int l = 0; l < dtGetDeviceList.Rows.Count; l++)
                {
                    String DeviceID = dtGetDeviceList.Rows[l]["DeviceID"].ToString();

                    String GetSensor = "Select distinct Sensors from ViewSensorDeployment where DeviceID='" + DeviceID + "'";
                    SqlCommand cmdGetSensor = new SqlCommand(GetSensor, DBcon.con);
                    String Sensor1 = Convert.ToString(cmdGetSensor.ExecuteScalar());

                    lblSensorData.Text = lblSensorData.Text +"#"+ DeviceID+"-";

                    String GetNormalStartThreshold = "Select ThreasholdStart from viewThreashold where State='Normal' and Stimuli='" + Sensor1 + "'";
                    SqlCommand cmdGetNormalStartThreshold = new SqlCommand(GetNormalStartThreshold, DBcon.con);
                    Int32 NormalThreashold = Convert.ToInt32(cmdGetNormalStartThreshold.ExecuteScalar());
                    DateTime now = DateTime.Now;
                    if (Sensor1 == "Temprature")
                    {
                        String ReadValue = "Select TOP 1 Temprature from tbl_Readings where Temprature <='" + NormalThreashold + "' ORDER BY NEWID()";
                        SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                        String NormalTemp = Convert.ToString(cmdReadValue.ExecuteScalar());

                        String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                        SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                        String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                            //String ReadingValue = NormalTemp + "." + Fraction + "°C";
                            String ReadingValue = NormalTemp + "." + Fraction;

                            lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                        String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                        cmdSaveData.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                        cmdSaveData.Parameters.AddWithValue("@now", now);

                        cmdSaveData.ExecuteNonQuery();
                    }

                    else if (Sensor1 == "Humidity")
                    {
                        String ReadValue = "Select TOP 1 Humidity from tbl_Readings where Humidity >='" + NormalThreashold + "' ORDER BY NEWID()";
                        SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                        String NormalHumid = Convert.ToString(cmdReadValue.ExecuteScalar());

                        String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                        SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                        String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                       // String ReadingValue = NormalHumid + "." + Fraction + "%";
                            String ReadingValue = NormalHumid + "." + Fraction;
                            lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                        String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                        cmdSaveData.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                        cmdSaveData.Parameters.AddWithValue("@now", now);

                        cmdSaveData.ExecuteNonQuery();
                    }
                    else if (Sensor1 == "Air_pressure")
                    {
                        String ReadValue = "Select TOP 1 Air_Pressure from tbl_Readings where Air_Pressure <='" + NormalThreashold + "' ORDER BY NEWID()";
                        SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                        String NormalAirPressure = Convert.ToString(cmdReadValue.ExecuteScalar());

                        String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                        SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                        String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                       // String ReadingValue = NormalAirPressure + "." + Fraction + "mbar";
                            String ReadingValue = NormalAirPressure + "." + Fraction;

                            lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                        String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                        cmdSaveData.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                        cmdSaveData.Parameters.AddWithValue("@now", now);

                        cmdSaveData.ExecuteNonQuery();
                    }

                    else if (Sensor1 == "Light")
                    {
                        String ReadValue = "Select TOP 1 Light from tbl_Readings where Light='1' or Light='0' ORDER BY NEWID()";
                        SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                        String NormalLight = Convert.ToString(cmdReadValue.ExecuteScalar());



                            //String ReadingValue = NormalLight + "Light";
                            String ReadingValue = NormalLight;
                            lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                        String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                        cmdSaveData.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                        cmdSaveData.Parameters.AddWithValue("@now", now);

                        cmdSaveData.ExecuteNonQuery();


                    }
                    else if (Sensor1 == "Smoke")
                    {
                        String ReadValue = "Select TOP 1 Smoke from tbl_Readings where Smoke <='" + NormalThreashold + "' ORDER BY NEWID()";
                        SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                        String NormalSmoke = Convert.ToString(cmdReadValue.ExecuteScalar());

                        String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                        SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                        String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                        //String ReadingValue = NormalSmoke + "." + Fraction + "opacity";
                            String ReadingValue = NormalSmoke + "." + Fraction;
                            lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                        String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                        cmdSaveData.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                        cmdSaveData.Parameters.AddWithValue("@now", now);

                        cmdSaveData.ExecuteNonQuery();
                    }
                    else if (Sensor1 == "Motion")
                    {
                        String ReadingValue = "0";

                        lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                        String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                        cmdSaveData.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                        cmdSaveData.Parameters.AddWithValue("@now", now);

                        cmdSaveData.ExecuteNonQuery();
                    }
                    else if (Sensor1 == "Wind_Speed")
                    {
                        String ReadValue = "Select TOP 1 Wind_Speed from tbl_Readings where Wind_Speed <='" + NormalThreashold + "' ORDER BY NEWID()";
                        SqlCommand cmdReadValue = new SqlCommand(ReadValue, DBcon.con);
                        String NormalWind_Speed = Convert.ToString(cmdReadValue.ExecuteScalar());

                        String GetFraction = "Select TOP 1 Fraction from tbl_Readings  ORDER BY NEWID()";
                        SqlCommand cmdGetFraction = new SqlCommand(GetFraction, DBcon.con);
                        String Fraction = Convert.ToString(cmdGetFraction.ExecuteScalar());

                        //String ReadingValue = NormalWind_Speed + "." + Fraction + "km/h";
                            String ReadingValue = NormalWind_Speed + "." + Fraction;
                            lblSensorData.Text = lblSensorData.Text + ReadingValue + "," + now + ",";

                        String SaveData = "INSERT INTO [tblProcessed1] VALUES(@Site,@DeviceID,@ReadingValue,@now)";
                        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
                        cmdSaveData.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData.Parameters.AddWithValue("@ReadingValue", ReadingValue);
                        cmdSaveData.Parameters.AddWithValue("@now", now);

                        cmdSaveData.ExecuteNonQuery();
                    }
                    else
                    {
                            
                        }

                }
                    lblSensorData.Text = lblSensorData.Text + ";";

            }////////////
                
            }
                                            


        }

        lblSensorData.Text = lblSensorData.Text + ";";




    }
    protected void btnTruncate0_Click(object sender, EventArgs e)
    {
        gridPreprocess.Visible = false;
        lblSensorData.Visible = true;

        for(int i=0;i<10;i++)
        {
            SimulateSensor();
        }

    }

    protected void btnPreprocess_Click(object sender, EventArgs e)
    {
        gridPreprocess.Visible = true;
        lblSensorData.Visible = false;

        DBcon.dataBaseConnection();

        String TruncateData = "Truncate table [tbl_RawSensorData]";
        SqlCommand cmdTruncateData = new SqlCommand(TruncateData, DBcon.con);
        cmdTruncateData.ExecuteNonQuery();


    
        String TruncateData1 = "Truncate table [tbl_Preprocessing]";
        SqlCommand cmdTruncateData1 = new SqlCommand(TruncateData1, DBcon.con);
        cmdTruncateData1.ExecuteNonQuery();


        String SensorData = lblSensorData.Text.ToString();

        String SaveData = "INSERT INTO [tbl_RawSensorData] VALUES(@SensorData)";
        SqlCommand cmdSaveData = new SqlCommand(SaveData, DBcon.con);
        cmdSaveData.Parameters.AddWithValue("@SensorData", SensorData);
        cmdSaveData.ExecuteNonQuery();

        DataTable dt1 = new DataTable();
        DataColumn SplitBySite = dt1.Columns.Add("Preprocessed", typeof(string));

        DataTable dt2 = new DataTable();

        dt2.Columns.AddRange(new DataColumn[] { new DataColumn("Site", typeof(string)), new DataColumn("Data", typeof(string)) });
       


        // string[] lines = SensorData.Split('\n');

        string[] lines = SensorData.Split(';');
       // Response.Write(SensorData);

        foreach (var line in lines)
        {
            string[] split = line.Split(';');
            DataRow row = dt1.NewRow();

            
            row.SetField(SplitBySite, split[0]);
           

            dt1.Rows.Add(row);
            
        }



        foreach (DataRow row1 in dt1.Rows)
        {
            String dtRow = row1["Preprocessed"].ToString()+"*";

            string[] strsplit = dtRow.Split('*');
            string Site = strsplit[0];
            string Data = strsplit[1];

            string[] strsplit1 = Data.Split('#');
            
            for(int i=0; i< strsplit1.Length;i++)
            {
                String Data1 = strsplit1[i] + "-";
                string[] strsplit2 = Data1.Split('-');
                for (int j = 0; j < strsplit2.Length; j++)
                {
                    string DeviceID = strsplit2[0];
                    string NextData = strsplit2[1]+",";
                   // Response.Write(NextData + "<br>");

                    string[] strsplit3 = NextData.Split(',');
                    for (int k = 0; k < strsplit3.Length; k++)
                    {
                        string Reading = strsplit3[0];
                        string Time = strsplit3[1];

                        if (DeviceID != "")

                        {


                            String CheckData = "Select * from tbl_Preprocessing where Site='" + Site + "' and DeviceID='" + DeviceID + "' and Reading='" + Reading + "' and Time='" + Time + "'";
                            SqlDataAdapter adaptCheckData = new SqlDataAdapter(CheckData,DBcon.con);
                            DataTable dtCheckData = new DataTable();
                            adaptCheckData.Fill(dtCheckData);

                            if (dtCheckData.Rows.Count > 0)
                            {

                            }

                            else
                            {
                                String SaveData1 = "INSERT INTO [tbl_Preprocessing] VALUES(@Site,@DeviceID,@Reading,@Time)";
                                SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                                cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                                cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);
                                cmdSaveData1.Parameters.AddWithValue("@Reading", Reading);
                                cmdSaveData1.Parameters.AddWithValue("@Time", Time);
                                cmdSaveData1.ExecuteNonQuery();

                            }
                        }
                        else
                        {

                        }

                    }



                    }
                }






           // Response.Write(Data + "<br>");

            

        }


        //gridPreprocess.DataSource = dt2;
        //gridPreprocess.DataBind();
                              
        String GetData = "Select * from viewPreprocessed order by ID DESC";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);
        gridPreprocess.DataSource = dtGetData;
        gridPreprocess.DataBind();



        btnContextualize.Visible = true;
        btnMap.Visible = true;

    }

    protected void btnNext1_Click(object sender, EventArgs e)
    {
        DBcon.dataBaseConnection();
        String GetProcessed1Data = "Select * from viewProcessed1";
        SqlDataAdapter adaptGetProcessed1Data = new SqlDataAdapter(GetProcessed1Data, DBcon.con);
        DataTable dtGetProcessed1Data = new DataTable();
        adaptGetProcessed1Data.Fill(dtGetProcessed1Data);
        DataTable dtProcessed2 = new DataTable();
        dtProcessed2.Columns.Add("Site", typeof(string));
        dtProcessed2.Columns.Add("DeviceID", typeof(string));
        dtProcessed2.Columns.Add("Reading", typeof(string));
        dtProcessed2.Columns.Add("Unit", typeof(string));
        dtProcessed2.Columns.Add("Sensors", typeof(string));

        String TruncateData = "Truncate table [tbl_Contextualize]";
        SqlCommand cmdTruncateData = new SqlCommand(TruncateData, DBcon.con);
        cmdTruncateData.ExecuteNonQuery();

        for (int i=0;i< dtGetProcessed1Data.Rows.Count;i++)
        {
            String Site = dtGetProcessed1Data.Rows[i]["Site"].ToString();
            String DeviceID = dtGetProcessed1Data.Rows[i]["DeviceID"].ToString();
            String Reading = dtGetProcessed1Data.Rows[i]["Reading"].ToString();
            String Unit = dtGetProcessed1Data.Rows[i]["Unit"].ToString();
            String Sensors = dtGetProcessed1Data.Rows[i]["Sensors"].ToString();

            

            double Reading1 = Convert.ToDouble(Reading);

            if(Sensors == "Temprature")
            {
                String getData = "Select ID, Meaning, Max from [tbl_ContextKB1] where Sensor='Temprature'";
                SqlDataAdapter adaptgetData = new SqlDataAdapter(getData, DBcon.con);
                DataTable dtgetData = new DataTable();
                adaptgetData.Fill(dtgetData);

                for(int j=0;j< dtgetData.Rows.Count;j++)
                {
                    String ID = dtgetData.Rows[j]["ID"].ToString();
                    double Meaning = Convert.ToDouble(dtgetData.Rows[j]["Meaning"].ToString());
                    double Max = Convert.ToDouble(dtgetData.Rows[j]["Max"].ToString());

                    if(Reading1>= Meaning && Reading1 <= Max)
                    {
                        String GetConcept = "Select Concept from tbl_ContextKB1 where ID='"+ ID + "'";
                        SqlCommand cmdGetConcept = new SqlCommand(GetConcept,DBcon.con);
                        String Concept = cmdGetConcept.ExecuteScalar().ToString();

                        dtProcessed2.Rows.Add(Site, DeviceID, Concept, Unit, Sensors);

                        String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,@Concept,@Unit,@Sensors)";
                        SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                        cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData1.Parameters.AddWithValue("@Concept", Concept);
                        cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                        cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                        cmdSaveData1.ExecuteNonQuery();
                    }



                }

            }

            else if (Sensors == "Humidity")
            {
                String getData = "Select ID, Meaning, Max from [tbl_ContextKB1] where Sensor='Humidity'";
                SqlDataAdapter adaptgetData = new SqlDataAdapter(getData, DBcon.con);
                DataTable dtgetData = new DataTable();
                adaptgetData.Fill(dtgetData);

                for (int j = 0; j < dtgetData.Rows.Count; j++)
                {
                    String ID = dtgetData.Rows[j]["ID"].ToString();
                    double Meaning = Convert.ToDouble(dtgetData.Rows[j]["Meaning"].ToString());
                    double Max = Convert.ToDouble(dtgetData.Rows[j]["Max"].ToString());

                    if (Reading1 >= Meaning && Reading1 <= Max)
                    {
                        String GetConcept = "Select Concept from tbl_ContextKB1 where ID='" + ID + "'";
                        SqlCommand cmdGetConcept = new SqlCommand(GetConcept, DBcon.con);
                        String Concept = cmdGetConcept.ExecuteScalar().ToString();

                        dtProcessed2.Rows.Add(Site, DeviceID, Concept, Unit, Sensors);

                        String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,@Concept,@Unit,@Sensors)";
                        SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                        cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData1.Parameters.AddWithValue("@Concept", Concept);
                        cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                        cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                        cmdSaveData1.ExecuteNonQuery();

                    }



                }

            }

            else if (Sensors == "Air_pressure")
            {
                String getData = "Select ID, Meaning, Max from [tbl_ContextKB1] where Sensor='Air_pressure'";
                SqlDataAdapter adaptgetData = new SqlDataAdapter(getData, DBcon.con);
                DataTable dtgetData = new DataTable();
                adaptgetData.Fill(dtgetData);

                for (int j = 0; j < dtgetData.Rows.Count; j++)
                {
                    String ID = dtgetData.Rows[j]["ID"].ToString();
                    double Meaning = Convert.ToDouble(dtgetData.Rows[j]["Meaning"].ToString());
                    double Max = Convert.ToDouble(dtgetData.Rows[j]["Max"].ToString());

                    if (Reading1 >= Meaning && Reading1 <= Max)
                    {
                        String GetConcept = "Select Concept from tbl_ContextKB1 where ID='" + ID + "'";
                        SqlCommand cmdGetConcept = new SqlCommand(GetConcept, DBcon.con);
                        String Concept = cmdGetConcept.ExecuteScalar().ToString();

                        dtProcessed2.Rows.Add(Site, DeviceID, Concept, Unit, Sensors);
                        String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,@Concept,@Unit,@Sensors)";
                        SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                        cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData1.Parameters.AddWithValue("@Concept", Concept);
                        cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                        cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                        cmdSaveData1.ExecuteNonQuery();

                    }



                }

            }

            else if (Sensors == "Wind_Speed")
            {
                String getData = "Select ID, Meaning, Max from [tbl_ContextKB1] where Sensor='Wind_Speed'";
                SqlDataAdapter adaptgetData = new SqlDataAdapter(getData, DBcon.con);
                DataTable dtgetData = new DataTable();
                adaptgetData.Fill(dtgetData);

                for (int j = 0; j < dtgetData.Rows.Count; j++)
                {
                    String ID = dtgetData.Rows[j]["ID"].ToString();
                    double Meaning = Convert.ToDouble(dtgetData.Rows[j]["Meaning"].ToString());
                    double Max = Convert.ToDouble(dtgetData.Rows[j]["Max"].ToString());

                    if (Reading1 >= Meaning && Reading1 <= Max)
                    {
                        String GetConcept = "Select Concept from tbl_ContextKB1 where ID='" + ID + "'";
                        SqlCommand cmdGetConcept = new SqlCommand(GetConcept, DBcon.con);
                        String Concept = cmdGetConcept.ExecuteScalar().ToString();

                        dtProcessed2.Rows.Add(Site, DeviceID, Concept, Unit, Sensors);
                        String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,@Concept,@Unit,@Sensors)";
                        SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                        cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);
                        cmdSaveData1.Parameters.AddWithValue("@Concept", Concept);
                        cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                        cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                        cmdSaveData1.ExecuteNonQuery();

                    }



                }

            }

            else if (Sensors == "Smoke")
            {
                if(Reading1>0 && Reading1 <1)
                {
                    dtProcessed2.Rows.Add(Site, DeviceID, "Low_Smoke", Unit, Sensors);
                    String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,'Low_Smoke',@Unit,@Sensors)";
                    SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                    cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                    cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);
                    
                    cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                    cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                    cmdSaveData1.ExecuteNonQuery();

                }
                else if (Reading1 > 1)
                {
                    dtProcessed2.Rows.Add(Site, DeviceID, "High_Smoke", Unit, Sensors);

                    String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,'High_Smoke',@Unit,@Sensors)";
                    SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                    cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                    cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);

                    cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                    cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                    cmdSaveData1.ExecuteNonQuery();

                }
                else
                {
                    dtProcessed2.Rows.Add(Site, DeviceID, "No_Smoke", Unit, Sensors);
                    String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,'No_Smoke',@Unit,@Sensors)";
                    SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                    cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                    cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);

                    cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                    cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                    cmdSaveData1.ExecuteNonQuery();

                }

            }

            else if (Sensors == "Light")
            {
                if (Reading1 == 0)
                {
                    dtProcessed2.Rows.Add(Site, DeviceID, "No_Light", Unit, Sensors);

                    String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,'No_Light',@Unit,@Sensors)";
                    SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                    cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                    cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);

                    cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                    cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                    cmdSaveData1.ExecuteNonQuery();

                }
                else if (Reading1 > 0)
                {
                    dtProcessed2.Rows.Add(Site, DeviceID, "Light", Unit, Sensors);
                    String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,'Light',@Unit,@Sensors)";
                    SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                    cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                    cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);

                    cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                    cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                    cmdSaveData1.ExecuteNonQuery();

                }
                

            }
            else if (Sensors == "Motion")
            {
                if (Reading1 == 0)
                {
                    dtProcessed2.Rows.Add(Site, DeviceID, "No_Motion", Unit, Sensors);
                    dtProcessed2.Rows.Add(Site, DeviceID, "Light", Unit, Sensors);
                    String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,'No_Motion',@Unit,@Sensors)";
                    SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                    cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                    cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);

                    cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                    cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                    cmdSaveData1.ExecuteNonQuery();

                }
                else if (Reading1 > 0)
                {
                    dtProcessed2.Rows.Add(Site, DeviceID, "Anonymous_Motion", Unit, Sensors);

                    String SaveData1 = "INSERT INTO [tbl_Contextualize] VALUES(@Site,@DeviceID,'Anonymous_Motion',@Unit,@Sensors)";
                    SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                    cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                    cmdSaveData1.Parameters.AddWithValue("@DeviceID", DeviceID);

                    cmdSaveData1.Parameters.AddWithValue("@Unit", Unit);
                    cmdSaveData1.Parameters.AddWithValue("@Sensors", Sensors);
                    cmdSaveData1.ExecuteNonQuery();

                }


            }





        }
        gridPreprocess.Visible = false;
        gridProcess2.DataSource = dtProcessed2;
        gridProcess2.DataBind();


    }

    protected void btnMap_Click(object sender, EventArgs e)
    {
        BuildRelationShips();
        GetSituation();

        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('qgis2web_2021_10_04-21_00_58_069153/Map.html','_newtab');", true);
    }


    ////////////////////////////////////////////////////////////Build RelationShips//////////////////////////////////////////////////////////////////////
    public void BuildRelationShips()
    {
        DataTable dt2 = new DataTable();
        DataColumn SplitedWords = dt2.Columns.Add("DomainThings", typeof(string));

        DataTable dt3 = new DataTable();
        DataColumn ConcatinateWords = dt3.Columns.Add("ConcatinateWords", typeof(string));

        DataTable dt4 = new DataTable();
        DataColumn DomainThing = dt4.Columns.Add("DomainThing", typeof(string));

        DataTable dt5 = new DataTable();
        DataColumn AllSituations = dt5.Columns.Add("AllSituation", typeof(string));

        DataTable dt6 = new DataTable();
        DataColumn AllAttributes = dt6.Columns.Add("AllAttributes", typeof(string));


        DBcon.dataBaseConnection();
        String LoadDomainKnowledge = "Select DomainKnowledge from tbl_DomainKnowledge order by ID ASC";
        SqlDataAdapter adaptLoadDomainKnowledge = new SqlDataAdapter(LoadDomainKnowledge, DBcon.con);
        DataTable dtLoadDomainKnowledge = new DataTable();
        adaptLoadDomainKnowledge.Fill(dtLoadDomainKnowledge);


        for (int i = 0; i < dtLoadDomainKnowledge.Rows.Count; i++)
        {
            String DomainKnowledge = dtLoadDomainKnowledge.Rows[i]["DomainKnowledge"].ToString() + " " + "Trail";

            string[] lines = DomainKnowledge.Split(' ');

            //////////////////////////////////////////////////////ADD Splited words//////////////////////////////////////////////////////////////
            foreach (var line in lines)
            {
                string[] split = line.Split(' ');
                DataRow row = dt2.NewRow();
                row.SetField(SplitedWords, split[0]);
                dt2.Rows.Add(row);
            }

            ///////////////////////////////////////ADD Splited and Concatinated Words////////////////////////////////////////////////////////////////
            for (int j = 0; j < dt2.Rows.Count - 1; j++)
            {
                String Word = dt2.Rows[j]["DomainThings"].ToString();
                String Word1 = dt2.Rows[j + 1]["DomainThings"].ToString();

                //Response.Write(Word+"_"+Word1+"<br>");
                String ConcatinatedString = Word + "_" + Word1;
                DataRow row2 = dt3.NewRow();
                row2.SetField(ConcatinateWords, Word);
                dt3.Rows.Add(Word);

                DataRow row1 = dt3.NewRow();
                row1.SetField(ConcatinateWords, ConcatinatedString);
                dt3.Rows.Add(ConcatinatedString);

            }

            dt2.Rows.Clear();


            var UniqueRows = dt3.AsEnumerable().Distinct(DataRowComparer.Default);
            DataTable dt8 = UniqueRows.CopyToDataTable();










            ///////////////////////////////Check the Word Existence from the Domain Thing List and ADD to List///////////////////////////////////////////////////////////
            for (int k = 0; k < dt8.Rows.Count; k++)
            {
                String Word = dt8.Rows[k]["ConcatinateWords"].ToString().Trim();

                String CheckDomainThing = "Select DomainThing from tbl_DomainThing where DomainThing='" + Word + "'";
                SqlDataAdapter adaptCheckDomainThing = new SqlDataAdapter(CheckDomainThing, DBcon.con);
                DataTable dtCheckDomainThing = new DataTable();
                adaptCheckDomainThing.Fill(dtCheckDomainThing);
                if (dtCheckDomainThing.Rows.Count > 0)
                {
                    String GetDomainThing = "Select DomainThing from tbl_DomainThing where DomainThing='" + Word + "'";
                    SqlCommand cmdGetDomainThing = new SqlCommand(GetDomainThing, DBcon.con);
                    String DomainThing1 = Convert.ToString(cmdGetDomainThing.ExecuteScalar());


                    DataRow row5 = dt4.NewRow();
                    row5.SetField(DomainThing, DomainThing1);
                    dt4.Rows.Add(Word);
                }

                else
                {

                }



            }



            String Attrib1 = "";
            String Attrib2 = "";
            String Attrib3 = "";
            String Attrib4 = "";
            String Attrib5 = "";
            String Attrib6 = "";
            String Situation = "";

            for (int m = 0; m < dt4.Rows.Count; m++)
            {


                String DomainThingWord = dt4.Rows[m]["DomainThing"].ToString();

                String CheckSituation = "Select DomainThing from tbl_DomainThing where DomainThing='" + DomainThingWord + "' and Score='1'";
                SqlDataAdapter adapterCheckSituation = new SqlDataAdapter(CheckSituation, DBcon.con);
                DataTable dtCheckSituation = new DataTable();
                adapterCheckSituation.Fill(dtCheckSituation);


                if (dtCheckSituation.Rows.Count > 0)
                {

                    DataRow row6 = dt5.NewRow();
                    row6.SetField(AllSituations, DomainThingWord);
                    dt5.Rows.Add(DomainThingWord);

                    // Situation = DomainThingWord;

                }

                else
                {

                    DataRow row7 = dt6.NewRow();
                    row7.SetField(AllAttributes, DomainThingWord);
                    dt6.Rows.Add(DomainThingWord);



                }


            }


            for (int y = 0; y < dt5.Rows.Count; y++)
            {
                Situation = dt5.Rows[y]["AllSituation"].ToString();
                Attrib1 = "";
                Attrib2 = "";
                Attrib3 = "";
                Attrib4 = "";
                Attrib5 = "";
                Attrib6 = "";

                for (int z = 0; z < dt6.Rows.Count; z++)
                {
                    String Attribute = dt6.Rows[z]["AllAttributes"].ToString();

                    if (Attrib1 == "")
                    {
                        Attrib1 = Attribute;
                    }

                    else if (Attrib2 == "")
                    {
                        Attrib2 = Attribute;
                    }
                    else if (Attrib3 == "")
                    {
                        Attrib3 = Attribute;
                    }
                    else if (Attrib4 == "")
                    {
                        Attrib4 = Attribute;
                    }
                    else if (Attrib5 == "")
                    {
                        Attrib5 = Attribute;
                    }
                    else if (Attrib6 == "")
                    {
                        Attrib6 = Attribute;
                    }

                }

                String CheckDataExistence = "Select * from tbl_RelationShips where Attrib1 ='" + Attrib1 + "' and Attrib2 ='" + Attrib2 + "' and Attrib3 ='" + Attrib3 + "' and Attrib4 ='" + Attrib4 + "' and Attrib5 ='" + Attrib5 + "' and Attrib6 ='" + Attrib6 + "' and Result ='" + Situation + "' ";
                SqlDataAdapter adaptCheckDataExistence = new SqlDataAdapter(CheckDataExistence, DBcon.con);
                DataTable dtCheckDataExistence = new DataTable();
                adaptCheckDataExistence.Fill(dtCheckDataExistence);

                if (dtCheckDataExistence.Rows.Count > 0)
                {
                    Attrib1 = "";
                    Attrib2 = "";
                    Attrib3 = "";
                    Attrib4 = "";
                    Attrib5 = "";
                    Attrib6 = "";




                }
                else
                {
                    String GetMaxID = "Select max(ID) from [tbl_RelationShips]";
                    SqlCommand cmdGetMaxID = new SqlCommand(GetMaxID, DBcon.con);
                    Int32 MaxID = Convert.ToInt32(cmdGetMaxID.ExecuteScalar());
                    Int32 MaxID1 = MaxID + 1;


                    String SaveData1 = "INSERT INTO [tbl_RelationShips] VALUES(@MaxID1,@Attrib1,@Attrib2,@Attrib3,@Attrib4,@Attrib5,@Attrib6,@Situation)";
                    SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                    cmdSaveData1.Parameters.AddWithValue("@MaxID1", MaxID1);
                    cmdSaveData1.Parameters.AddWithValue("@Attrib1", Attrib1);
                    cmdSaveData1.Parameters.AddWithValue("@Attrib2", Attrib2);
                    cmdSaveData1.Parameters.AddWithValue("@Attrib3", Attrib3);

                    cmdSaveData1.Parameters.AddWithValue("@Attrib4", Attrib4);
                    cmdSaveData1.Parameters.AddWithValue("@Attrib5", Attrib5);
                    cmdSaveData1.Parameters.AddWithValue("@Attrib6", Attrib6);
                    cmdSaveData1.Parameters.AddWithValue("@Situation", Situation);
                    cmdSaveData1.ExecuteNonQuery();


                    String GetMaxID1 = "Select max(ID) from [tbl_RelationShips]";
                    SqlCommand cmdGetMaxID1 = new SqlCommand(GetMaxID1, DBcon.con);
                    Int32 MaxID2 = Convert.ToInt32(cmdGetMaxID1.ExecuteScalar());



                    TuneRelationShipsAttrib1(MaxID2);
                    TuneRelationShipsAttrib2(MaxID2);
                    TuneRelationShipsAttrib3(MaxID2);
                    TuneRelationShipsAttrib4(MaxID2);
                    TuneRelationShipsAttrib5(MaxID2);
                    TuneRelationShipsAttrib6(MaxID2);
                }



            }
            //Attrib1 = "";
            //Attrib2 = "";
            //Attrib3 = "";
            //Attrib4 = "";
            //Attrib5 = "";
            //Attrib6 = "";
            //Situation = "";


            //  Response.Write(dt4.Rows.Count + "<br>");
            dt3.Rows.Clear();
            dt4.Rows.Clear();
            dt5.Rows.Clear();
            dt6.Rows.Clear();
            dt8.Rows.Clear();


        }





    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////Start RelationShips Tuning///////////////////////////////////////////////////////////////////////////////////
    public void TuneRelationShipsAttrib1(Int32 ID)
    {
        Int32 MaxID1 = ID + 1;
        DBcon.dataBaseConnection();
        DataTable dt10 = new DataTable();
        DataColumn Meanings = dt10.Columns.Add("Meanings", typeof(string));
        String GetData = "Select * from tbl_RelationShips where ID='" + ID + "' ";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);

        String Attrib1 = "";
        String Attrib2 = "";
        String Attrib3 = "";
        String Attrib4 = "";
        String Attrib5 = "";
        String Attrib6 = "";
        String Result = "";

        for (int i = 0; i < dtGetData.Rows.Count; i++)
        {
            Attrib1 = dtGetData.Rows[i]["Attrib1"].ToString();
            Attrib2 = dtGetData.Rows[i]["Attrib2"].ToString();
            Attrib3 = dtGetData.Rows[i]["Attrib3"].ToString();
            Attrib4 = dtGetData.Rows[i]["Attrib4"].ToString();
            Attrib5 = dtGetData.Rows[i]["Attrib5"].ToString();
            Attrib6 = dtGetData.Rows[i]["Attrib6"].ToString();
            Result = dtGetData.Rows[i]["Result"].ToString();

            String GetMeaning = "Select Meaning from tbl_MeaningList where Word='" + Attrib1 + "'";
            SqlCommand cmdGetMeaning = new SqlCommand(GetMeaning, DBcon.con);
            String Meaning = Convert.ToString(cmdGetMeaning.ExecuteScalar());

            if (Meaning == "")
            {

            }
            else
            {
                string[] lines = Meaning.Split(',');


                foreach (var line in lines)
                {
                    string[] split = line.Split(',');
                    DataRow row = dt10.NewRow();
                    row.SetField(Meanings, split[0]);
                    dt10.Rows.Add(row);
                }
            }


        }

        for (int j = 0; j < dt10.Rows.Count; j++)
        {
            Attrib1 = dt10.Rows[j]["Meanings"].ToString();


            String CheckDataExistence = "Select * from tbl_RelationShips where Attrib1 ='" + Attrib1 + "' and Attrib2 ='" + Attrib2 + "' and Attrib3 ='" + Attrib3 + "' and Attrib4 ='" + Attrib4 + "' and Attrib5 ='" + Attrib5 + "' and Attrib6 ='" + Attrib6 + "' and Result ='" + Result + "' ";
            SqlDataAdapter adaptCheckDataExistence = new SqlDataAdapter(CheckDataExistence, DBcon.con);
            DataTable dtCheckDataExistence = new DataTable();
            adaptCheckDataExistence.Fill(dtCheckDataExistence);

            if (dtCheckDataExistence.Rows.Count > 0)
            {
                Attrib1 = "";


            }
            else
            {
                String SaveData1 = "INSERT INTO [tbl_RelationShips] VALUES(@MaxID1,@Attrib1,@Attrib2,@Attrib3,@Attrib4,@Attrib5,@Attrib6,@Result)";
                SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                cmdSaveData1.Parameters.AddWithValue("@MaxID1", MaxID1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib1", Attrib1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib2", Attrib2);
                cmdSaveData1.Parameters.AddWithValue("@Attrib3", Attrib3);

                cmdSaveData1.Parameters.AddWithValue("@Attrib4", Attrib4);
                cmdSaveData1.Parameters.AddWithValue("@Attrib5", Attrib5);
                cmdSaveData1.Parameters.AddWithValue("@Attrib6", Attrib6);
                cmdSaveData1.Parameters.AddWithValue("@Result", Result);
                cmdSaveData1.ExecuteNonQuery();


            }




        }

      


    }


    /// <summary>
    /// Tune RelationShips for Column 2
    /// </summary>
    /// <returns></returns>
    public void TuneRelationShipsAttrib2(Int32 ID)
    {
        Int32 MaxID1 = ID + 1;
        DBcon.dataBaseConnection();
        DataTable dt10 = new DataTable();
        DataColumn Meanings = dt10.Columns.Add("Meanings", typeof(string));
        String GetData = "Select * from tbl_RelationShips where ID='" + ID + "' ";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);

        String Attrib1 = "";
        String Attrib2 = "";
        String Attrib3 = "";
        String Attrib4 = "";
        String Attrib5 = "";
        String Attrib6 = "";
        String Result = "";

        for (int i = 0; i < dtGetData.Rows.Count; i++)
        {
            Attrib1 = dtGetData.Rows[i]["Attrib1"].ToString();
            Attrib2 = dtGetData.Rows[i]["Attrib2"].ToString();
            Attrib3 = dtGetData.Rows[i]["Attrib3"].ToString();
            Attrib4 = dtGetData.Rows[i]["Attrib4"].ToString();
            Attrib5 = dtGetData.Rows[i]["Attrib5"].ToString();
            Attrib6 = dtGetData.Rows[i]["Attrib6"].ToString();
            Result = dtGetData.Rows[i]["Result"].ToString();

            String GetMeaning = "Select Meaning from tbl_MeaningList where Word='" + Attrib2 + "'";
            SqlCommand cmdGetMeaning = new SqlCommand(GetMeaning, DBcon.con);
            String Meaning = Convert.ToString(cmdGetMeaning.ExecuteScalar());

            if (Meaning == "")
            {

            }
            else
            {
                string[] lines = Meaning.Split(',');


                foreach (var line in lines)
                {
                    string[] split = line.Split(',');
                    DataRow row = dt10.NewRow();
                    row.SetField(Meanings, split[0]);
                    dt10.Rows.Add(row);
                }
            }


        }

        for (int j = 0; j < dt10.Rows.Count; j++)
        {
            Attrib2 = dt10.Rows[j]["Meanings"].ToString();


            String CheckDataExistence = "Select * from tbl_RelationShips where Attrib1 ='" + Attrib1 + "' and Attrib2 ='" + Attrib2 + "' and Attrib3 ='" + Attrib3 + "' and Attrib4 ='" + Attrib4 + "' and Attrib5 ='" + Attrib5 + "' and Attrib6 ='" + Attrib6 + "' and Result ='" + Result + "' ";
            SqlDataAdapter adaptCheckDataExistence = new SqlDataAdapter(CheckDataExistence, DBcon.con);
            DataTable dtCheckDataExistence = new DataTable();
            adaptCheckDataExistence.Fill(dtCheckDataExistence);

            if (dtCheckDataExistence.Rows.Count > 0)
            {
                Attrib2 = "";


            }
            else
            {
                String SaveData1 = "INSERT INTO [tbl_RelationShips] VALUES(@MaxID1,@Attrib1,@Attrib2,@Attrib3,@Attrib4,@Attrib5,@Attrib6,@Result)";
                SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                cmdSaveData1.Parameters.AddWithValue("@MaxID1", MaxID1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib1", Attrib1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib2", Attrib2);
                cmdSaveData1.Parameters.AddWithValue("@Attrib3", Attrib3);

                cmdSaveData1.Parameters.AddWithValue("@Attrib4", Attrib4);
                cmdSaveData1.Parameters.AddWithValue("@Attrib5", Attrib5);
                cmdSaveData1.Parameters.AddWithValue("@Attrib6", Attrib6);
                cmdSaveData1.Parameters.AddWithValue("@Result", Result);
                cmdSaveData1.ExecuteNonQuery();


            }




        }




    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Tune RelationShips for Column 2
    /// </summary>
    /// <returns></returns>
    public void TuneRelationShipsAttrib3(Int32 ID)
    {
        Int32 MaxID1 = ID + 1;
        DBcon.dataBaseConnection();
        DataTable dt10 = new DataTable();
        DataColumn Meanings = dt10.Columns.Add("Meanings", typeof(string));
        String GetData = "Select * from tbl_RelationShips where ID='" + ID + "' ";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);

        String Attrib1 = "";
        String Attrib2 = "";
        String Attrib3 = "";
        String Attrib4 = "";
        String Attrib5 = "";
        String Attrib6 = "";
        String Result = "";

        for (int i = 0; i < dtGetData.Rows.Count; i++)
        {
            Attrib1 = dtGetData.Rows[i]["Attrib1"].ToString();
            Attrib2 = dtGetData.Rows[i]["Attrib2"].ToString();
            Attrib3 = dtGetData.Rows[i]["Attrib3"].ToString();
            Attrib4 = dtGetData.Rows[i]["Attrib4"].ToString();
            Attrib5 = dtGetData.Rows[i]["Attrib5"].ToString();
            Attrib6 = dtGetData.Rows[i]["Attrib6"].ToString();
            Result = dtGetData.Rows[i]["Result"].ToString();

            String GetMeaning = "Select Meaning from tbl_MeaningList where Word='" + Attrib3 + "'";
            SqlCommand cmdGetMeaning = new SqlCommand(GetMeaning, DBcon.con);
            String Meaning = Convert.ToString(cmdGetMeaning.ExecuteScalar());

            if (Meaning == "")
            {

            }
            else
            {
                string[] lines = Meaning.Split(',');


                foreach (var line in lines)
                {
                    string[] split = line.Split(',');
                    DataRow row = dt10.NewRow();
                    row.SetField(Meanings, split[0]);
                    dt10.Rows.Add(row);
                }
            }


        }

        for (int j = 0; j < dt10.Rows.Count; j++)
        {
            Attrib3 = dt10.Rows[j]["Meanings"].ToString();


            String CheckDataExistence = "Select * from tbl_RelationShips where Attrib1 ='" + Attrib1 + "' and Attrib2 ='" + Attrib2 + "' and Attrib3 ='" + Attrib3 + "' and Attrib4 ='" + Attrib4 + "' and Attrib5 ='" + Attrib5 + "' and Attrib6 ='" + Attrib6 + "' and Result ='" + Result + "' ";
            SqlDataAdapter adaptCheckDataExistence = new SqlDataAdapter(CheckDataExistence, DBcon.con);
            DataTable dtCheckDataExistence = new DataTable();
            adaptCheckDataExistence.Fill(dtCheckDataExistence);

            if (dtCheckDataExistence.Rows.Count > 0)
            {
                Attrib3 = "";


            }
            else
            {
                String SaveData1 = "INSERT INTO [tbl_RelationShips] VALUES(@MaxID1,@Attrib1,@Attrib2,@Attrib3,@Attrib4,@Attrib5,@Attrib6,@Result)";
                SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                cmdSaveData1.Parameters.AddWithValue("@MaxID1", MaxID1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib1", Attrib1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib2", Attrib2);
                cmdSaveData1.Parameters.AddWithValue("@Attrib3", Attrib3);

                cmdSaveData1.Parameters.AddWithValue("@Attrib4", Attrib4);
                cmdSaveData1.Parameters.AddWithValue("@Attrib5", Attrib5);
                cmdSaveData1.Parameters.AddWithValue("@Attrib6", Attrib6);
                cmdSaveData1.Parameters.AddWithValue("@Result", Result);
                cmdSaveData1.ExecuteNonQuery();


            }




        }




    }

    ///////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// Tune RelationShips for Column 2
    /// </summary>
    /// <returns></returns>
    public void TuneRelationShipsAttrib4(Int32 ID)
    {
        Int32 MaxID1 = ID + 1;
        DBcon.dataBaseConnection();
        DataTable dt10 = new DataTable();
        DataColumn Meanings = dt10.Columns.Add("Meanings", typeof(string));
        String GetData = "Select * from tbl_RelationShips where ID='" + ID + "' ";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);

        String Attrib1 = "";
        String Attrib2 = "";
        String Attrib3 = "";
        String Attrib4 = "";
        String Attrib5 = "";
        String Attrib6 = "";
        String Result = "";

        for (int i = 0; i < dtGetData.Rows.Count; i++)
        {
            Attrib1 = dtGetData.Rows[i]["Attrib1"].ToString();
            Attrib2 = dtGetData.Rows[i]["Attrib2"].ToString();
            Attrib3 = dtGetData.Rows[i]["Attrib3"].ToString();
            Attrib4 = dtGetData.Rows[i]["Attrib4"].ToString();
            Attrib5 = dtGetData.Rows[i]["Attrib5"].ToString();
            Attrib6 = dtGetData.Rows[i]["Attrib6"].ToString();
            Result = dtGetData.Rows[i]["Result"].ToString();

            String GetMeaning = "Select Meaning from tbl_MeaningList where Word='" + Attrib4 + "'";
            SqlCommand cmdGetMeaning = new SqlCommand(GetMeaning, DBcon.con);
            String Meaning = Convert.ToString(cmdGetMeaning.ExecuteScalar());

            if (Meaning == "")
            {

            }
            else
            {
                string[] lines = Meaning.Split(',');


                foreach (var line in lines)
                {
                    string[] split = line.Split(',');
                    DataRow row = dt10.NewRow();
                    row.SetField(Meanings, split[0]);
                    dt10.Rows.Add(row);
                }
            }


        }

        for (int j = 0; j < dt10.Rows.Count; j++)
        {
            Attrib4 = dt10.Rows[j]["Meanings"].ToString();


            String CheckDataExistence = "Select * from tbl_RelationShips where Attrib1 ='" + Attrib1 + "' and Attrib2 ='" + Attrib2 + "' and Attrib3 ='" + Attrib3 + "' and Attrib4 ='" + Attrib4 + "' and Attrib5 ='" + Attrib5 + "' and Attrib6 ='" + Attrib6 + "' and Result ='" + Result + "' ";
            SqlDataAdapter adaptCheckDataExistence = new SqlDataAdapter(CheckDataExistence, DBcon.con);
            DataTable dtCheckDataExistence = new DataTable();
            adaptCheckDataExistence.Fill(dtCheckDataExistence);

            if (dtCheckDataExistence.Rows.Count > 0)
            {
                Attrib4 = "";


            }
            else
            {
                String SaveData1 = "INSERT INTO [tbl_RelationShips] VALUES(@MaxID1,@Attrib1,@Attrib2,@Attrib3,@Attrib4,@Attrib5,@Attrib6,@Result)";
                SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                cmdSaveData1.Parameters.AddWithValue("@MaxID1", MaxID1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib1", Attrib1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib2", Attrib2);
                cmdSaveData1.Parameters.AddWithValue("@Attrib3", Attrib3);

                cmdSaveData1.Parameters.AddWithValue("@Attrib4", Attrib4);
                cmdSaveData1.Parameters.AddWithValue("@Attrib5", Attrib5);
                cmdSaveData1.Parameters.AddWithValue("@Attrib6", Attrib6);
                cmdSaveData1.Parameters.AddWithValue("@Result", Result);
                cmdSaveData1.ExecuteNonQuery();


            }




        }




    }

    /////////////////////////////////////////////////////
    /// /// <summary>
    /// Tune RelationShips for Column 2
    /// </summary>
    /// <returns></returns>
    public void TuneRelationShipsAttrib5(Int32 ID)
    {
        Int32 MaxID1 = ID + 1;
        DBcon.dataBaseConnection();
        DataTable dt10 = new DataTable();
        DataColumn Meanings = dt10.Columns.Add("Meanings", typeof(string));
        String GetData = "Select * from tbl_RelationShips where ID='" + ID + "' ";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);

        String Attrib1 = "";
        String Attrib2 = "";
        String Attrib3 = "";
        String Attrib4 = "";
        String Attrib5 = "";
        String Attrib6 = "";
        String Result = "";

        for (int i = 0; i < dtGetData.Rows.Count; i++)
        {
            Attrib1 = dtGetData.Rows[i]["Attrib1"].ToString();
            Attrib2 = dtGetData.Rows[i]["Attrib2"].ToString();
            Attrib3 = dtGetData.Rows[i]["Attrib3"].ToString();
            Attrib4 = dtGetData.Rows[i]["Attrib4"].ToString();
            Attrib5 = dtGetData.Rows[i]["Attrib5"].ToString();
            Attrib6 = dtGetData.Rows[i]["Attrib6"].ToString();
            Result = dtGetData.Rows[i]["Result"].ToString();

            String GetMeaning = "Select Meaning from tbl_MeaningList where Word='" + Attrib5 + "'";
            SqlCommand cmdGetMeaning = new SqlCommand(GetMeaning, DBcon.con);
            String Meaning = Convert.ToString(cmdGetMeaning.ExecuteScalar());

            if (Meaning == "")
            {

            }
            else
            {
                string[] lines = Meaning.Split(',');


                foreach (var line in lines)
                {
                    string[] split = line.Split(',');
                    DataRow row = dt10.NewRow();
                    row.SetField(Meanings, split[0]);
                    dt10.Rows.Add(row);
                }
            }


        }

        for (int j = 0; j < dt10.Rows.Count; j++)
        {
            Attrib5 = dt10.Rows[j]["Meanings"].ToString();


            String CheckDataExistence = "Select * from tbl_RelationShips where Attrib1 ='" + Attrib1 + "' and Attrib2 ='" + Attrib2 + "' and Attrib3 ='" + Attrib3 + "' and Attrib4 ='" + Attrib4 + "' and Attrib5 ='" + Attrib5 + "' and Attrib6 ='" + Attrib6 + "' and Result ='" + Result + "' ";
            SqlDataAdapter adaptCheckDataExistence = new SqlDataAdapter(CheckDataExistence, DBcon.con);
            DataTable dtCheckDataExistence = new DataTable();
            adaptCheckDataExistence.Fill(dtCheckDataExistence);

            if (dtCheckDataExistence.Rows.Count > 0)
            {
                Attrib5 = "";


            }
            else
            {
                String SaveData1 = "INSERT INTO [tbl_RelationShips] VALUES(@MaxID1,@Attrib1,@Attrib2,@Attrib3,@Attrib4,@Attrib5,@Attrib6,@Result)";
                SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                cmdSaveData1.Parameters.AddWithValue("@MaxID1", MaxID1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib1", Attrib1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib2", Attrib2);
                cmdSaveData1.Parameters.AddWithValue("@Attrib3", Attrib3);

                cmdSaveData1.Parameters.AddWithValue("@Attrib4", Attrib4);
                cmdSaveData1.Parameters.AddWithValue("@Attrib5", Attrib5);
                cmdSaveData1.Parameters.AddWithValue("@Attrib6", Attrib6);
                cmdSaveData1.Parameters.AddWithValue("@Result", Result);
                cmdSaveData1.ExecuteNonQuery();


            }




        }




    }

    //////////////////////////////////////////////////////////////////////////////////


    /// /// <summary>
    /// Tune RelationShips for Column 2
    /// </summary>
    /// <returns></returns>
    public void TuneRelationShipsAttrib6(Int32 ID)
    {
        Int32 MaxID1 = ID + 1;
        DBcon.dataBaseConnection();
        DataTable dt10 = new DataTable();
        DataColumn Meanings = dt10.Columns.Add("Meanings", typeof(string));
        String GetData = "Select * from tbl_RelationShips where ID='" + ID + "' ";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);

        String Attrib1 = "";
        String Attrib2 = "";
        String Attrib3 = "";
        String Attrib4 = "";
        String Attrib5 = "";
        String Attrib6 = "";
        String Result = "";

        for (int i = 0; i < dtGetData.Rows.Count; i++)
        {
            Attrib1 = dtGetData.Rows[i]["Attrib1"].ToString();
            Attrib2 = dtGetData.Rows[i]["Attrib2"].ToString();
            Attrib3 = dtGetData.Rows[i]["Attrib3"].ToString();
            Attrib4 = dtGetData.Rows[i]["Attrib4"].ToString();
            Attrib5 = dtGetData.Rows[i]["Attrib5"].ToString();
            Attrib6 = dtGetData.Rows[i]["Attrib6"].ToString();
            Result = dtGetData.Rows[i]["Result"].ToString();

            String GetMeaning = "Select Meaning from tbl_MeaningList where Word='" + Attrib6 + "'";
            SqlCommand cmdGetMeaning = new SqlCommand(GetMeaning, DBcon.con);
            String Meaning = Convert.ToString(cmdGetMeaning.ExecuteScalar());

            if (Meaning == "")
            {

            }
            else
            {
                string[] lines = Meaning.Split(',');


                foreach (var line in lines)
                {
                    string[] split = line.Split(',');
                    DataRow row = dt10.NewRow();
                    row.SetField(Meanings, split[0]);
                    dt10.Rows.Add(row);
                }
            }


        }

        for (int j = 0; j < dt10.Rows.Count; j++)
        {
            Attrib6 = dt10.Rows[j]["Meanings"].ToString();


            String CheckDataExistence = "Select * from tbl_RelationShips where Attrib1 ='" + Attrib1 + "' and Attrib2 ='" + Attrib2 + "' and Attrib3 ='" + Attrib3 + "' and Attrib4 ='" + Attrib4 + "' and Attrib5 ='" + Attrib5 + "' and Attrib6 ='" + Attrib6 + "' and Result ='" + Result + "' ";
            SqlDataAdapter adaptCheckDataExistence = new SqlDataAdapter(CheckDataExistence, DBcon.con);
            DataTable dtCheckDataExistence = new DataTable();
            adaptCheckDataExistence.Fill(dtCheckDataExistence);

            if (dtCheckDataExistence.Rows.Count > 0)
            {
                Attrib6 = "";


            }
            else
            {
                String SaveData1 = "INSERT INTO [tbl_RelationShips] VALUES(@MaxID1,@Attrib1,@Attrib2,@Attrib3,@Attrib4,@Attrib5,@Attrib6,@Result)";
                SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                cmdSaveData1.Parameters.AddWithValue("@MaxID1", MaxID1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib1", Attrib1);
                cmdSaveData1.Parameters.AddWithValue("@Attrib2", Attrib2);
                cmdSaveData1.Parameters.AddWithValue("@Attrib3", Attrib3);

                cmdSaveData1.Parameters.AddWithValue("@Attrib4", Attrib4);
                cmdSaveData1.Parameters.AddWithValue("@Attrib5", Attrib5);
                cmdSaveData1.Parameters.AddWithValue("@Attrib6", Attrib6);
                cmdSaveData1.Parameters.AddWithValue("@Result", Result);
                cmdSaveData1.ExecuteNonQuery();


            }




        }




    }

    ///////////////////////////////////////////////////////END OF Tunings./////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////////////Start Get Situation///////////////////////////////////////////////////////////////////////////////////////////
    public string GetSituation()
    {
        DBcon.dataBaseConnection();


        String TruncateData = "Truncate table [tbl_TempSituationMapper]";
        SqlCommand cmdTruncateData = new SqlCommand(TruncateData, DBcon.con);
        cmdTruncateData.ExecuteNonQuery();

        String LoadSites = "Select Deployment from tbl_Sites where ID !='1'";
        SqlDataAdapter adaptLoadSites = new SqlDataAdapter(LoadSites, DBcon.con);
        DataTable dtLoadSites = new DataTable();
        adaptLoadSites.Fill(dtLoadSites);
        DataTable dtLoadConcepts = new DataTable();
        for (int i = 0; i < dtLoadSites.Rows.Count; i++)
        {
            String Site = dtLoadSites.Rows[i]["Deployment"].ToString();
            String LoadConcepts = " Select Concept from [viewMap1] where Site='" + Site + "' ";
            SqlDataAdapter adaptLoadConcepts = new SqlDataAdapter(LoadConcepts, DBcon.con);
            adaptLoadConcepts.Fill(dtLoadConcepts);

            for (int j = 0; j < dtLoadConcepts.Rows.Count; j++)
            {
                String Concept = dtLoadConcepts.Rows[j]["Concept"].ToString();

                String CheckRelationShips = "Select ID, Result from [tbl_RelationShips] where Attrib1='" + Concept + "' or Attrib2='" + Concept + "' or Attrib3='" + Concept + "' or Attrib4='" + Concept + "' or Attrib5='" + Concept + "' or Attrib6='" + Concept + "'";
                SqlDataAdapter adaptCheckRelationShips = new SqlDataAdapter(CheckRelationShips, DBcon.con);
                DataTable dtCheckRelationShips = new DataTable();
                adaptCheckRelationShips.Fill(dtCheckRelationShips);

                if (dtCheckRelationShips.Rows.Count > 0)
                {
                    for (int m = 0; m < dtCheckRelationShips.Rows.Count; m++)
                    {
                        String RelationID = dtCheckRelationShips.Rows[m]["ID"].ToString();
                        String Result = dtCheckRelationShips.Rows[m]["Result"].ToString();

                        String SaveData1 = "INSERT INTO [tbl_TempSituationMapper] VALUES(@Site,@RelationID,@Result)";
                        SqlCommand cmdSaveData1 = new SqlCommand(SaveData1, DBcon.con);
                        cmdSaveData1.Parameters.AddWithValue("@Site", Site);
                        cmdSaveData1.Parameters.AddWithValue("@RelationID", RelationID);
                        cmdSaveData1.Parameters.AddWithValue("@Result", Result);
                        cmdSaveData1.ExecuteNonQuery();


                    }


                    dtCheckRelationShips.Rows.Clear();
                }




            }

            dtLoadConcepts.Rows.Clear();

        }






        //GridView1.DataSource = dtLoadConcepts;
        //GridView1.DataBind();



        return JsonConvert.SerializeObject(LoadSites, Newtonsoft.Json.Formatting.Indented);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



}