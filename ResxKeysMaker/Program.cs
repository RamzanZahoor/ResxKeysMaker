using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Hardcoded input and output file paths
        string inputFile = @"C:\path\to\your\input.resx";  // Replace with your actual input .resx file path
        string outputFile = @"C:\path\to\your\output.resx";  // Replace with your actual output .resx file path

        // Validate if the input file exists
        if (!File.Exists(inputFile))
        {
            Console.WriteLine("Input file not found. Please check the path.");
            return;
        }

        // Load the .resx file as XML
        XDocument resxDocument = XDocument.Load(inputFile);

        // Iterate over all <data> elements which contain key-value pairs
        foreach (var dataElement in resxDocument.Descendants("data"))
        {
            // Get the 'name' attribute which is the key
            var nameAttribute = dataElement.Attribute("name");
            if (nameAttribute != null)
            {
                string originalKey = nameAttribute.Value;

                // Modify the key: lowercase and replace spaces/special characters with underscores
                string modifiedKey = Regex.Replace(originalKey.ToLower(), @"[\s\W]+", "_");

                // Update the 'name' attribute with the modified key
                nameAttribute.Value = modifiedKey;
            }
        }

        // Save the modified .resx file to the hardcoded output path
        resxDocument.Save(outputFile);

        Console.WriteLine($"File successfully updated and saved as: {outputFile}");
    }
}
