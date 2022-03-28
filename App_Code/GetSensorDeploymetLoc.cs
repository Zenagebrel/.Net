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
/// Summary description for GetSensorDeploymetLoc
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.ComponentModel.ToolboxItem(false)]
[ScriptService]



// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class GetSensorDeploymetLoc : System.Web.Services.WebService
{
    DBConnection DBcon = new DBConnection();
    public GetSensorDeploymetLoc()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
        ToJson();
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public string ToJson()
    {
        DBcon.dataBaseConnection();
        String GetData = "Select X,Y,Status,Deployment,Sensors from ViewSensorDeployment where Flag='1'";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);

        return JsonConvert.SerializeObject(dtGetData, Newtonsoft.Json.Formatting.Indented);
        //var JSONString = new StringBuilder();
        //if (dtGetData.Rows.Count > 0)
        //{
        //    JSONString.Append("[");
        //    for (int i = 0; i < dtGetData.Rows.Count; i++)
        //    {
        //        JSONString.Append("{");
        //        for (int j = 0; j < dtGetData.Columns.Count; j++)
        //        {
        //            if (j < dtGetData.Columns.Count - 1)
        //            {
        //                JSONString.Append("\"" + dtGetData.Columns[j].ColumnName.ToString() + "\":" + "\"" + dtGetData.Rows[i][j].ToString() + "\",");
        //            }
        //            else if (j == dtGetData.Columns.Count - 1)
        //            {
        //                JSONString.Append("\"" + dtGetData.Columns[j].ColumnName.ToString() + "\":" + "\"" + dtGetData.Rows[i][j].ToString() + "\"");
        //            }
        //        }
        //        if (i == dtGetData.Rows.Count - 1)
        //        {
        //            JSONString.Append("}");
        //        }
        //        else
        //        {
        //            JSONString.Append("},");
        //        }
        //    }
        //    JSONString.Append("]");
        //}
        //return JSONString.ToString();















       // System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
       // List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
       // Dictionary<string, object> row;
       // foreach (DataRow dr in dtGetData.Rows)
       // {
       //     row = new Dictionary<string, object>();
       //     foreach (DataColumn col in dtGetData.Columns)
       //     {
       //         row.Add(col.ColumnName, dr[col]);
       //     }
       //     rows.Add(row);
       // }
       // this.Context.Response.ContentType = "application/json; charset=utf-8";
       //// this.Context.Response.Write(serializer.Serialize(rows));
       // return serializer.Serialize(rows);







        //List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
        //Dictionary<string, object> item;
        //foreach (DataRow row in dtGetData.Rows)
        //{
        //    item = new Dictionary<string, object>();
        //    foreach (DataColumn col in dtGetData.Columns)
        //    {
        //        item.Add(col.ColumnName, (Convert.IsDBNull(row[col]) ? null : row[col]));
        //    }
        //    lst.Add(item);
        //}

        //JavaScriptSerializer js = new JavaScriptSerializer();
        //Context.Response.Write(js.Serialize(lst));
        ////  var Result = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
        ////Console.Write(Result);
        ////return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
        //return js.Serialize(lst);
    }


}
