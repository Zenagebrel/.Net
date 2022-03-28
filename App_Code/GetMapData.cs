using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Text;
using System.Web.Script.Services;
using Newtonsoft.Json;

/// <summary>
/// Summary description for GetMapData
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
[ScriptService]

// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class GetMapData : System.Web.Services.WebService
{
    DBConnection DBcon = new DBConnection();
    public GetMapData()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
       ToJson();
       ToJson1();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ToJson()
    {
        DBcon.dataBaseConnection();
        String GetData = "Select Site,DeviceID,Sensors,Concept,X,Y from viewMap1";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);

        return JsonConvert.SerializeObject(dtGetData, Newtonsoft.Json.Formatting.Indented);

    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ToJson1()
    {
        DBcon.dataBaseConnection();
        String LoadSites = "Select Deployment from tbl_Sites where ID !='1'";
        SqlDataAdapter adaptLoadSites = new SqlDataAdapter(LoadSites, DBcon.con);
        DataTable dtLoadSites = new DataTable();
        adaptLoadSites.Fill(dtLoadSites);
        DataTable dtLoadSituations = new DataTable();

        DataTable Situation = new DataTable();
        Situation.Columns.Add("Site", typeof(string));
        Situation.Columns.Add("Situations", typeof(string));
        Situation.Columns.Add("X", typeof(string));
        Situation.Columns.Add("Y", typeof(string));




        for (int i = 0; i < dtLoadSites.Rows.Count; i++)
        {
            String Site = dtLoadSites.Rows[i]["Deployment"].ToString();

            String CheckData = "Select Site from [viewSituationCounter] where Site='" + Site + "'";
            SqlDataAdapter adaptCheckData = new SqlDataAdapter(CheckData, DBcon.con);
            DataTable dtCheckData = new DataTable();
            adaptCheckData.Fill(dtCheckData);

            if(dtCheckData.Rows.Count>0)
            { 

            String GetXCoordinate = "  Select X from ViewCoordinateList where Deployment='" + Site + "' and Flag='0'";
            SqlCommand cmdGetXCoordinate = new SqlCommand(GetXCoordinate, DBcon.con);
            String XCoordinate = Convert.ToString(cmdGetXCoordinate.ExecuteScalar());

            String GetYCoordinate = "  Select Y from ViewCoordinateList where Deployment='" + Site + "' and Flag='0'";
            SqlCommand cmdGetYCoordinate = new SqlCommand(GetYCoordinate, DBcon.con);
            String YCoordinate = Convert.ToString(cmdGetYCoordinate.ExecuteScalar());

            String LoadSituation = " Select Site from [viewSituationCounter] where Site='" + Site + "' ";
            SqlDataAdapter adaptLoadSituation = new SqlDataAdapter(LoadSituation, DBcon.con);
            adaptLoadSituation.Fill(dtLoadSituations);

            if (dtLoadSituations.Rows.Count > 0)
            {
                String GetMaxCounter = "  Select max(SituationCounter) from viewSituationCounter where Site='" + Site + "'";
                SqlCommand cmdGetMaxCounter = new SqlCommand(GetMaxCounter, DBcon.con);
                String MaxCounter = Convert.ToString(cmdGetMaxCounter.ExecuteScalar());

                String GetResult = "  Select Result from viewSituationCounter where SituationCounter='" + MaxCounter + "'";
                SqlCommand cmdGetResult = new SqlCommand(GetResult, DBcon.con);
                String Result = Convert.ToString(cmdGetResult.ExecuteScalar());



                Situation.Rows.Add(Site, Result, XCoordinate, YCoordinate);

            }

        }



        }


            return JsonConvert.SerializeObject(Situation, Newtonsoft.Json.Formatting.Indented);

    }
}
