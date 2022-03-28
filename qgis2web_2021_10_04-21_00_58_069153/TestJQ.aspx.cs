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

public partial class qgis2web_2021_10_04_21_00_58_069153_TestJQ : System.Web.UI.Page
{
    DBConnection DBcon = new DBConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        BuildRelationShips();
        GetSituation();
       //TuneRelationShipsAttrib1("14");
    }

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
            String DomainKnowledge = dtLoadDomainKnowledge.Rows[i]["DomainKnowledge"].ToString()+" "+"Trail";

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


    /// <summary>
    /// Tune RelationShip for Column 1
    /// </summary>
    /// <param name="ID"></param>
    public void TuneRelationShipsAttrib1(Int32 ID)
    {
        Int32 MaxID1 = ID + 1;
        DBcon.dataBaseConnection();
        DataTable dt10 = new DataTable();
        DataColumn Meanings = dt10.Columns.Add("Meanings", typeof(string));
        String GetData = "Select * from tbl_RelationShips where ID='"+ID+"' ";
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

        for (int i=0;i< dtGetData.Rows.Count;i++)
        {
             Attrib1 = dtGetData.Rows[i]["Attrib1"].ToString();
             Attrib2 = dtGetData.Rows[i]["Attrib2"].ToString();
             Attrib3 = dtGetData.Rows[i]["Attrib3"].ToString();
             Attrib4 = dtGetData.Rows[i]["Attrib4"].ToString();
             Attrib5 = dtGetData.Rows[i]["Attrib5"].ToString();
             Attrib6 = dtGetData.Rows[i]["Attrib6"].ToString();
             Result =  dtGetData.Rows[i]["Result"].ToString();

            String GetMeaning = "Select Meaning from tbl_MeaningList where Word='" + Attrib1 + "'";
            SqlCommand cmdGetMeaning = new SqlCommand(GetMeaning, DBcon.con);
            String Meaning = Convert.ToString(cmdGetMeaning.ExecuteScalar());

            if(Meaning=="")
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

        for(int j=0;j< dt10.Rows.Count;j++)
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

        GridView2.Visible = true;
        GridView2.DataSource = dt10;
        GridView2.DataBind();


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

    /// <summary>
    /// /////////////////////////////////////
    /// </summary>
    /// <returns></returns>
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
        for (int i=0;i< dtLoadSites.Rows.Count;i++)
        {
            String Site = dtLoadSites.Rows[i]["Deployment"].ToString();
            String LoadConcepts = " Select Concept from [viewMap1] where Site='" + Site + "' ";
            SqlDataAdapter adaptLoadConcepts = new SqlDataAdapter(LoadConcepts, DBcon.con);
            adaptLoadConcepts.Fill(dtLoadConcepts);

            for (int j = 0; j < dtLoadConcepts.Rows.Count; j++)
            {
                String Concept = dtLoadConcepts.Rows[j]["Concept"].ToString();

                String CheckRelationShips = "Select ID, Result from [tbl_RelationShips] where Attrib1='"+ Concept + "' or Attrib2='" + Concept + "' or Attrib3='" + Concept + "' or Attrib4='" + Concept + "' or Attrib5='" + Concept + "' or Attrib6='" + Concept + "'";
                SqlDataAdapter adaptCheckRelationShips = new SqlDataAdapter(CheckRelationShips, DBcon.con);
                DataTable dtCheckRelationShips = new DataTable();
                adaptCheckRelationShips.Fill(dtCheckRelationShips);

                if(dtCheckRelationShips.Rows.Count>0)
                {
                    for(int m=0;m< dtCheckRelationShips.Rows.Count;m++)
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




    //public string ToJson1()
    //{
    //    DBcon.dataBaseConnection();
    //    String LoadDomainKnowledge = "Select DomainKnowledge from tbl_DomainKnowledge";
    //    SqlDataAdapter adaptLoadDomainKnowledge = new SqlDataAdapter(LoadDomainKnowledge, DBcon.con);
    //    DataTable dtLoadDomainKnowledge = new DataTable();
    //    adaptLoadDomainKnowledge.Fill(dtLoadDomainKnowledge);

    //    DataTable dt2 = new DataTable();
    //    DataColumn SplitedWords = dt2.Columns.Add("SplitedWords", typeof(string));

    //    DataTable dt3 = new DataTable();
    //    DataColumn ConcatinateWords = dt3.Columns.Add("ConcatinateWords", typeof(string));

    //    DataTable dt4 = new DataTable();
    //    DataColumn DomainThing = dt4.Columns.Add("DomainThing", typeof(string));


    //    for (int i = 0; i < dtLoadDomainKnowledge.Rows.Count; i++)
    //    {
    //        String DomainKnowledge = dtLoadDomainKnowledge.Rows[i]["DomainKnowledge"].ToString();

    //        string[] lines = DomainKnowledge.Split(' ');


    //        foreach (var line in lines)
    //        {
    //            string[] split = line.Split(' ');
    //            DataRow row = dt2.NewRow();
    //            row.SetField(SplitedWords, split[0]);
    //            dt2.Rows.Add(row);


    //            for (int j = 0; j < dt2.Rows.Count - 1; j++)
    //            {
    //                String Word = dt2.Rows[j]["SplitedWords"].ToString();
    //                String Word1 = dt2.Rows[j + 1]["SplitedWords"].ToString();

    //                //Response.Write(Word+"_"+Word1+"<br>");
    //                String ConcatinatedString = Word + "_" + Word1;
    //                DataRow row2 = dt3.NewRow();
    //                row2.SetField(ConcatinateWords, Word);
    //                dt3.Rows.Add(Word);

    //                DataRow row1 = dt3.NewRow();
    //                row1.SetField(ConcatinateWords, ConcatinatedString);
    //                dt3.Rows.Add(ConcatinatedString);

    //            }


    //            for (int k = 0; k < dt3.Rows.Count; k++)
    //            {
    //                String Word = dt3.Rows[k]["ConcatinateWords"].ToString();
    //                String CheckDomainThing = "Select DomainThing from tbl_DomainThing where DomainThing='" + Word + "'";
    //                SqlDataAdapter adaptCheckDomainThing = new SqlDataAdapter(CheckDomainThing, DBcon.con);
    //                DataTable dtCheckDomainThing = new DataTable();
    //                adaptCheckDomainThing.Fill(dtCheckDomainThing);
    //                if (dtCheckDomainThing.Rows.Count > 0)
    //                {
    //                    String GetDomainThing = "Select DomainThing from tbl_DomainThing where DomainThing='" + Word + "'";
    //                    SqlCommand cmdGetDomainThing = new SqlCommand(GetDomainThing, DBcon.con);
    //                    String DomainThing1 = Convert.ToString(cmdGetDomainThing.ExecuteScalar());


    //                    DataRow row5 = dt4.NewRow();
    //                    row5.SetField(DomainThing, DomainThing1);
    //                    dt4.Rows.Add(Word);
    //                }

    //                else
    //                {

    //                }



    //            }


















    //        }





    //    }


    //    GridView1.DataSource = dt4;
    //    GridView1.DataBind();








    //    return JsonConvert.SerializeObject(dtLoadDomainKnowledge, Newtonsoft.Json.Formatting.Indented);

    //}
}