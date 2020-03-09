using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
  static   class VoiceRoom
    {
      static private Socket ClientSocket;
      static private DirectSoundHelper sound;
      static private byte[] buffer = new byte[2205];
      static private Thread th;
      static public bool started = false;
      public static void VoiceRoomStart(string ip , int port)
      {
            Control.CheckForIllegalCrossThreadCalls = false;
            sound = new DirectSoundHelper();
            sound.OnBufferFulfill += new EventHandler(SendVoiceBuffer);
            
            Connect(ip, port);
            sound.StopLoop = false;
            started = true;
      }

      static void Connect(string ServerIP, int Port)
        {
            try
            {
                if (ClientSocket != null && ClientSocket.Connected)
                {
                    ClientSocket.Shutdown(SocketShutdown.Both);
                    System.Threading.Thread.Sleep(10);
                    ClientSocket.Close();
                }

                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint Server_EndPoint = new IPEndPoint(IPAddress.Parse(ServerIP), Port);
                ClientSocket.Blocking = false;

                ClientSocket.BeginConnect(Server_EndPoint, new AsyncCallback(OnConnect), ClientSocket);

                th = new Thread(new ThreadStart(sound.StartCapturing));
                th.IsBackground = true;
                th.Start();
            }
            catch (Exception) { }
        }

      public static void OnConnect(IAsyncResult ar)
        {
            Socket sock = (Socket)ar.AsyncState;

            try
            {
                if (sock.Connected)
                {
                    
                }
                else
                {
                    Disconncet();
                }
            }
            catch (Exception) { }
        }

      static void SendBuffer(byte[] buffer)
        {
            ClientSocket.Send(buffer, SocketFlags.None);
        }

      public static void Disconncet()
        {
            try
            {
                if (ClientSocket != null & ClientSocket.Connected)
                {
                    ClientSocket.Close();
                }
            }
            catch (Exception) { }
            started = false;
        }

      static void SendVoiceBuffer(object VoiceBuffer, EventArgs e)
        {
            byte[] Buffer = (byte[])VoiceBuffer;

            SendBuffer(Buffer);

        }
    }
}
