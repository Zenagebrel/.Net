using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

public partial class qgis2web_2021_10_04_21_00_58_069153_SensorDeployJSON : System.Web.UI.Page
{
    DBConnection DBcon = new DBConnection();
    protected void Page_Load(object sender, EventArgs e)
    {


 
        Response.Write(ToJson());
    }

    public  string ToJson()
    {
        DBcon.dataBaseConnection();
        String GetData = "Select X,Y from tbl_CoordinatesList";
        SqlDataAdapter adaptGetData = new SqlDataAdapter(GetData, DBcon.con);
        DataTable dtGetData = new DataTable();
        adaptGetData.Fill(dtGetData);


        List<Dictionary<string, object>> lst = new List<Dictionary<string, object>>();
        Dictionary<string, object> item;
        foreach (DataRow row in dtGetData.Rows)
        {
            item = new Dictionary<string, object>();
            foreach (DataColumn col in dtGetData.Columns)
            {
                item.Add(col.ColumnName, (Convert.IsDBNull(row[col]) ? null : row[col]));
            }
            lst.Add(item);
        }
        var Result = Newtonsoft.Json.JsonConvert.SerializeObject(lst);
        Console.Write(Result);
        return Newtonsoft.Json.JsonConvert.SerializeObject(lst);
        
    }

    
}