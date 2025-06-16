using System.IO.Pipes;
using System.IO;
using System;
using System.Diagnostics;

class master
{
    static void Main()
    {
        using var server = new NamedPipeServerStream("scannerA", PipeDirection.In);
        Console.WriteLine("Laukiama skaneriam prisijungti...");
        server.WaitForConnection();

        using var reader = new StreamReader(server);
        string message = reader.ReadToEnd();
        Console.WriteLine("Gautas pranešimas:");
        Console.WriteLine(message);
    }
}