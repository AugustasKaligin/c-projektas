using System;
using System.IO;

class scannerB
{
    static void Main()
    {
        string folderPath = @"C:\Users\augustas\Desktop\katalogas2";

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Katalogas nesrastas.");
            return;
        }

        string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

        foreach (string file in txtFiles)
        {
            Console.WriteLine($"Skaito: {Path.GetFileName(file)}");
            string content = File.ReadAllText(file);
            Console.WriteLine(content);
            Console.WriteLine("-----");
        }
    }
}