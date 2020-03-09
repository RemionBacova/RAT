using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    static class Browser
    {
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr h, out uint dwVolume);
        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr h, uint dwVolume);
        static  WebBrowser HiddenBrowser = new WebBrowser();
        public  static string urlToLoad = "";
        static uint _savedVolume = 100;//max value 65535

        static Timer t1 = new Timer();
        public static bool running = false;

        public static void StartBrowser()
        {
            if (urlToLoad != "")
            {
                running = true;
                t1.Interval = 120000;
                t1.Tick += t1_Tick;
                t1.Start();
                waveOutGetVolume(IntPtr.Zero, out _savedVolume);
                waveOutSetVolume(IntPtr.Zero, 0);
                HiddenBrowser.Navigate(new Uri(urlToLoad));
            }
        }

        static void t1_Tick(object sender, EventArgs e)
        {
            t1.Stop();
            StopBrowser();
        }

        public static void StopBrowser()
        {
            running = false;
            urlToLoad = "";
            HiddenBrowser.Stop();
            waveOutSetVolume(IntPtr.Zero, _savedVolume);
            Program.A.SetForNoUrlOpen(Program.myName);
        }
    }
}
