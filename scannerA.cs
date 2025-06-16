using System;
using System.IO;
using System.IO.Pipes;
using System.Reflection.Metadata.Ecma335;
using System.Text;

class scannerA
{
    static void Main()
    {
        string folderPath = @"C:\Users\augustas\Desktop\katalogas1";

        if (!Directory.Exists(folderPath))
        {
            Console.WriteLine("Katalogas nerastas.");
            return;
        }

        StringBuilder dataBuilder = new StringBuilder();
        string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

        foreach (string file in txtFiles)
        {
            string content = File.ReadAllText(file);
            Dictionary<string, int> wordCount = content
                .ToLower()
                .Split(new char[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?', '"', '(', ')', '[', ']', '{', '}', '-', '_', '/' }, StringSplitOptions.RemoveEmptyEntries)
                .GroupBy(word => word)
                .ToDictionary(group => group.Key, group => group.Count());
            dataBuilder.AppendLine($"Filename: {Path.GetFileName(file)}");
            foreach (var pair in wordCount)
            {
                dataBuilder.AppendLine($"{pair.Key}:{pair.Value}");
            }
        }

        using var client = new NamedPipeClientStream(".", "scannerA", PipeDirection.Out);

        client.Connect();
        using (var writer = new StreamWriter(client))
        {
            writer.Write(dataBuilder.ToString());
        }

    }
}