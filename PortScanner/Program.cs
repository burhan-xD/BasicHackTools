using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;

string host = args[0];
int startPort = Int32.Parse(args[1]);
int endPort = Int32.Parse(args[2]);
var tasks = new List<Task>();

for(int port = startPort; port <= endPort; port++)
{
    int p = port;
    tasks.Add(Task.Run(()=> ScanPort(host, p)));
}
await Task.WhenAll(tasks);

static async Task ScanPort(string host, int port)
{

    using var client = new TcpClient();
    try
    {
        await client.ConnectAsync(host, port);
        Console.WriteLine($"Port {port} is open.");
    }
    catch
    {
        Console.WriteLine($"Port {port} is closed.");
    }
}



