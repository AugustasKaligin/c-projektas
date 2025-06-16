using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Reflection.PortableExecutable;
using System.Threading;

class master
{
    static void Main()
    {

        Process currentProcess = Process.GetCurrentProcess();
        currentProcess.ProcessorAffinity = (IntPtr)0x4;

        string[] scannerPaths = { "C:\\Users\\augustas\\Desktop\\c#_projektas\\ScannerA\\bin\\Debug\\net8.0\\ScannerA.exe", "C:\\Users\\augustas\\Desktop\\c#_projektas\\ScannerB\\bin\\Debug\\net8.0\\ScannerB.exe" };

        string[] pipeNames = { "scannerA", "scannerB" };

        Thread[] threads = new Thread[pipeNames.Length];

        for (int i = 0;  i < pipeNames.Length; i++)
        {
            string pipeName = pipeNames[i];
            threads[i] = new Thread(() => HandlePipe(pipeName));
            threads[i].Start();

            Process.Start(scannerPaths[i]);
        }

        foreach (Thread t in threads)
        {
            t.Join();
        }

        Console.WriteLine("Gauti atsakai iš visų skanerių.");
    }

    static void HandlePipe(string pipeName)
    {
        using var server = new NamedPipeServerStream(pipeName, PipeDirection.In);
        Console.WriteLine($"Laukiama informacijos iš: {pipeName}");
        server.WaitForConnection();

        using var reader = new StreamReader(server);
        string message = reader.ReadToEnd();
        Console.WriteLine("Gautas pranešimas:");
        Console.WriteLine(message);
    }
}