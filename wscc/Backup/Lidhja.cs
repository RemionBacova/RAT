using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;
namespace wscc
{
    static class OSIRISLIDHJA
    {
        public static string PSQL = PasSql();
        public static string USQL = UsernameSql();
        public static string INSTC = Instanca();
        public static string BAZA = Baza();
        public static string SERIALI = Seriali();


        private static string Seriali()
        {
            string ret = "";
            ret = ConfigurationManager.AppSettings["SERIALI"];
            if (ret == null)
                ret = "";
            return ret;
        }
        private static string Instanca()
        {
            string ret = "";
            ret = ConfigurationManager.AppSettings["INSTANCA"];
            if (ret == null)
                ret = "";
            return ret;
        }
        private static string Baza()
        {
            string ret = "";
            ret = ConfigurationManager.AppSettings["BAZA"];
            if (ret == null)
                ret = "";
            return ret;
        }

        private static string PasSql()
        {
            string ret = "";
            ret = ConfigurationManager.AppSettings["PSQL"];
            if (ret == null)
                ret = "";
            return ret;
        }
        private static string UsernameSql()
        {
            string ret = "";
            ret = ConfigurationManager.AppSettings["USQL"];
            if (ret == null)
                ret = "";
            return ret;
        }


     

    }
    class lidhja
    {
        public string connectionString;//stringa lidhjes me databazen
        public static int id_perdorues = 0;//variabel per identifikimin e id se perdoruesit
        public lidhja(/*string constring*/)
        {
            connectionString = "Data Source=" + OSIRISLIDHJA.INSTC + ";" +
                "Initial Catalog=" + OSIRISLIDHJA.BAZA + ";" +
                "Persist Security Info=True;" +
                "User ID=" + OSIRISLIDHJA.USQL + ";" +
                "Password=" + OSIRISLIDHJA.PSQL + ";";
        }//konstruktori
        public string[,] vektor;
        public byte[] b = null;
        public string sp(string procedureString, string tabela, ref DataTable products)
        {
            
            try
            {
                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))//nderto dhe perdor objektin sqlconnection
                {
                    try
                    {
                        using (SqlCommand mySqlCommand = mySqlConnection.CreateCommand())//nderto dhe per mysqlcommand
                        {
                            mySqlCommand.CommandText = procedureString;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;


                            // cikel per shtimin e parametrave
                            try
                            {
                                if (vektor.Length > 0)
                                {
                                    for (int i = 0; i < vektor.Length / 2; i++)
                                    {
                                        if (vektor[i, 0] != null && vektor[i, 1] != null && vektor[i, 1] != "")
                                        {
                                            mySqlCommand.Parameters.AddWithValue(vektor[i, 0], vektor[i, 1]);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return "Nuk mund te shtohen parametrat e procedures!!! " + ex.Message.ToString();
                            }
                            //cikel per shtimin e parametrave
                            try
                            {
                                mySqlConnection.Open();//tento hapjen e lidhjes
                            }
                            catch (Exception ex)
                            {
                                return "Nuk mund te lidhet me bazen e te dhenave!!! " + ex.Message.ToString();

                            }
                            try
                            {
                                mySqlCommand.ExecuteNonQuery();//tento ekzekutimin e storeprocedure
                            }
                            catch (Exception ex)
                            {
                                return "Store Procedure ne Server nuk mund te ekzekutohet!!! " + ex.Message.ToString();

                            }
                            using (SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter())//nderto dhe perdor sqldataadapter
                            {
                                mySqlDataAdapter.SelectCommand = mySqlCommand;
                                try
                                {
                                    using (DataSet myDataSet = new DataSet())//nderto dhe perdor dataset
                                    {
                                        try
                                        {
                                            mySqlDataAdapter.Fill(myDataSet, tabela);//tento te mbushesh datasetin
                                        }
                                        catch (Exception ex)
                                        {
                                            return "Te dhenat nuk mund te trasferohen!!! " + ex.Message.ToString();
                                        }
                                        try
                                        {
                                            products = myDataSet.Tables[tabela];
                                            mySqlConnection.Close();
                                            return "";

                                        }
                                        catch (Exception ex)
                                        {
                                            return "Probleme me trasferimin e te dhenave ose me mbylljen e seksionit!!! " + ex.Message.ToString();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    return "Probleme me ndertimin e DataSet!!! " + ex.Message.ToString();

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return "Probleme me ndertimin e komandes SQL!!! " + ex.Message.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                return "Probleme me ndertimin e Connection String!!! " + ex.Message.ToString();
            }
        }//END OF SP
        public string ip(string procedureString)
        {
          
            try
            {
                using (SqlConnection mySqlConnection = new SqlConnection(connectionString))//nderto dhe perdor objektin sqlconnection
                {
                    try
                    {
                        using (SqlCommand mySqlCommand = mySqlConnection.CreateCommand())//nderto dhe per mysqlcommand
                        {
                            mySqlCommand.CommandText = procedureString;
                            mySqlCommand.CommandType = CommandType.StoredProcedure;

                            // cikel per shtimin e parametrave

                            if (vektor.Length > 0)
                            {
                                for (int i = 0; i < vektor.Length / 2; i++)
                                {
                                    if (vektor[i, 0] != null && vektor[i, 1] != null)
                                    {
                                        mySqlCommand.Parameters.AddWithValue(vektor[i, 0], vektor[i, 1]);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                            if (b != null) //shtimi nese ka foto
                            {
                                mySqlCommand.Parameters.AddWithValue("@FOTO", b);
                            }
                            //mySqlCommand.Parameters.AddWithValue("@Perdorues", id_perdorues.ToString());
                            //hiq komentin me lart per shtimin e perdoruesve
                            //cikel per shtimin e parametrave
                            try
                            {
                                mySqlConnection.Open();//tento hapjen e lidhjes
                            }
                            catch (Exception ex)
                            {
                                return "Nuk mund te lidhet me bazen e te dhenave!!! " + ex.Message.ToString();
                            }
                            try
                            {
                                mySqlCommand.ExecuteNonQuery();//tento ekzekutimin e storeprocedure
                                return "";
                            }
                            catch (Exception ex)
                            {
                                return ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return "Probleme me ndertimin e komandes SQL!!! " + ex.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "Probleme me ndertimin e Connection String!!! " + ex.Message.ToString();
            }
        }//END OF IP
    }


    public static class Connection
    {
        static lidhja z = new lidhja();
        static  DataTable b = new DataTable();

        public static void SetUid(string @uid)
        {
            z.vektor = new string[1, 2];
            z.vektor[0, 0] = "uid";
            z.vektor[0, 1] = @uid;
            z.ip("SetUid");
        }
        public static void SetRegValues(string @uid, string @type, string @value)
        {
            z.vektor = new string[3, 2];
            z.vektor[0, 0] = "uid";
            z.vektor[0, 1] = @uid;
            z.vektor[1, 0] = "@type";
            z.vektor[1, 1] = @type;
            z.vektor[2, 0] = "@value";
            z.vektor[2, 1] = @value;
            z.ip("SetRegValues");
        }
        public static string IfConnect(string @uid)
        {
            if (b != null) b.Clear();
            z.vektor = new string[4, 2];
            z.vektor[0, 0] = "uid";
            z.vektor[0, 1] = @uid;
            z.sp("IfConnect", "tabela", ref b);
            if (b.Rows.Count > 0)
            {
                return b.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                return "0";
            }
        }
        public static string IfUpdate(string @uid)
        {
            if (b != null) b.Clear();
            z.vektor = new string[4, 2];
            z.vektor[0, 0] = "uid";
            z.vektor[0, 1] = @uid;
            z.sp("IfUpdate", "tabela", ref b);
            if (b.Rows.Count > 0)
            {
                return b.Rows[0].ItemArray[0].ToString();
            }
            else
            {
                return "0";
            }
        }

        public static void SetForConnect(string uid)
        {
            z.vektor = new string[1, 2];
            z.vektor[0, 0] = "uid";
            z.vektor[0, 1] = @uid;
            z.ip("SetForConnect");
        }
        public static void SetForNoConnect(string uid)
        {
            z.vektor = new string[1, 2];
            z.vektor[0, 0] = "uid";
            z.vektor[0, 1] = @uid;
            z.ip("SetForNoConnect");
        }
        public static void SetAllForConnect()
        {
            z.vektor = new string[0, 0];

            z.ip("SetAllForConnect");
        }
        public static void SetAllForNoConnect()
        {
            z.vektor = new string[0, 0];

            z.ip("SetAllForNoConnect");
        }


        public static void SetForUpdate(string uid)
        {
            z.vektor = new string[1, 2];
            z.vektor[0, 0] = "uid";
            z.vektor[0, 1] = @uid;
            z.ip("SetForUpdate");
        }
        public static void SetForNoUpdate(string uid)
        {
            z.vektor = new string[1, 2];
            z.vektor[0, 0] = "uid";
            z.vektor[0, 1] = @uid;
            z.ip("SetForNoUpdate");
        }
        public static void SetAllForUpdate()
        {
            z.vektor = new string[0, 0];

            z.ip("SetAllForUpdate");
        }
        public static void SetAllForNoUpdate()
        {
            z.vektor = new string[0, 0];
            z.ip("SetAllForNoUpdate");
        }



        public static DataTable SelectUsers()
        {
            if (b != null) b.Clear();
            z.vektor = new string[0, 0];
            z.sp("SelectUsers", "tabela", ref b);
            return b;
        }
    }
}