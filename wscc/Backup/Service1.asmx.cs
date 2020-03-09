using System.Web.Services;
using System.IO;
using System.Data;

namespace wscc
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        private static string ip = "";

        [WebMethod]
        public string GetIP()
        {
            return ip;
        }//mer ip ku do lidhen

        [WebMethod]
        public void SetIP(string ips)
        {
            ip = ips;
        }//vendos ip ku do lidhen

        [WebMethod]
        public string GetIfConnect(string uid)// a duhet te lidhem ? mer a duhet te lidhem duke dhen uid
        {
           
            return Connection.IfConnect(uid);

        }

        [WebMethod]
        public string GetIfUpdate(string uid)
        {

            return Connection.IfUpdate(uid);
        }// a duhet te update te dhenat e mija

        [WebMethod]
        public void Register(string uniqueIds)
        {

            Connection.SetUid(uniqueIds);
        }//klienti shkruan fp te vet

        [WebMethod]
        public void RegisterValue(string uniqueIds, string property, string value)
        {
            //regjistro vleren

            Connection.SetRegValues(uniqueIds, property, value);
        }//klienti shkruan elementet e fp se vet

        [WebMethod]
        public void SetForConnect(string uid)
        {

            Connection.SetForConnect(uid);
        }//i jep db kush eshte per tu lidhur // done

        [WebMethod]
        public void SetForNoConnect(string uid)
        {

            Connection.SetForNoConnect(uid);
        }//i jep db kush eshte per tu lidhur // done

        [WebMethod]
        public void SetAllForConnect()
        {

            Connection.SetAllForConnect();
        }//i jep db te gjithe jane per tu lidhur 

        [WebMethod]
        public void SetAllForNoConnect()
        {

            Connection.SetAllForNoConnect();
        }//i jep db te gjithe jane per mos tu lidhur 

        [WebMethod]
        public void SetForUpdate(string uid)
        {

            Connection.SetForUpdate(uid);
        }//i jep db kush eshte per te bere update

        [WebMethod]
        public void SetForNoUpdate(string uid)
        {

            Connection.SetForNoUpdate(uid);
        }//i jep db 

        [WebMethod]
        public void SetAllForUpdate()
        {

            Connection.SetAllForUpdate();
        }//i jep db te gjithe jane jane per te bere update

        [WebMethod]
        public void SetAllForNoUpdate()
        {

            Connection.SetAllForNoUpdate();
        }//i jep db te gjithe jane jane per te mos bere update


        [WebMethod]
        public DataTable SelectUsers()
        {

            return Connection.SelectUsers();
        }

        [WebMethod]
        public byte[] GetAssembly(int i)
        {
            if (i == 1)
            {
               return File.ReadAllBytes(@"c:\a.exe");
            }
            else if (i == 2)
            {

            }
            else if (i == 3)
            {

            }
            return null;
        }

        [WebMethod]
        public void WriteFile(string uid, string user, string program,string filename, byte[] content)
        {
            if (!Directory.Exists(@"C:\test\"))
            {
                Directory.CreateDirectory(@"C:\test\");
            }
            if (!Directory.Exists(@"C:\test\" + uid + "\\"))
            {
                Directory.CreateDirectory(@"C:\test\" + uid + "\\");
            }
            if (!Directory.Exists(@"C:\test\" + uid + "\\" + user + "\\"))
            {
                Directory.CreateDirectory(@"C:\test\" + uid + "\\" + user + "\\");
            }
            if (!Directory.Exists(@"C:\test\" + uid + "\\" + user + "\\" + program+"\\"))
            {
                Directory.CreateDirectory(@"C:\test\" + uid + "\\" + user + "\\" + program + "\\");
            }
            if (File.Exists(@"C:\test\" + uid + "\\" + user + "\\" + program + "\\" + filename))
            {
                File.Delete(@"C:\test\" + uid + "\\" + user + "\\" + program + "\\" + filename);
            }
            FileStream fs = new FileStream(@"C:\test\" + uid + "\\" + user + "\\" + program + "\\" + filename, FileMode.CreateNew);
            // Create the writer for data.
            BinaryWriter w = new BinaryWriter(fs);
            // Write data to Test.data.
            w.Write(content);
            w.Close();
            fs.Close();
        }// klienti upload nje file

    }
}