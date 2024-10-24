using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using OfficeOpenXml; // EPPlus namespace

class Program
{
    static void Main(string[] args)
    {
        // Hardcoded Excel file input path and .resx output path
        string inputExcelFile = @"C:\path\to\your\input.xlsx";  // Replace with actual input Excel file path
        string outputResxFile = @"C:\path\to\your\output.resx";  // Replace with actual output .resx file path

        // Validate if the input Excel file exists
        if (!File.Exists(inputExcelFile))
        {
            Console.WriteLine("Input Excel file not found. Please check the path.");
            return;
        }

        // Create a new .resx file as an XDocument
        XDocument resxDocument = new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("root")
        );

        // Read the Excel file
        using (var package = new ExcelPackage(new FileInfo(inputExcelFile)))
        {
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];  // Read the first sheet

            int rowCount = worksheet.Dimension.Rows;  // Total rows in the sheet

            for (int row = 1; row <= rowCount; row++)  // Loop through each row
            {
                string originalKey = worksheet.Cells[row, 1].Text;  // Read the first column (Key)

                if (!string.IsNullOrWhiteSpace(originalKey))  // Only process non-empty keys
                {
                    // Convert key to lowercase and replace spaces/special characters with underscores
                    string modifiedKey = Regex.Replace(originalKey.ToLower(), @"[\s\W]+", "_");

                    // Create a new <data> element for the .resx file
                    XElement dataElement = new XElement("data",
                        new XAttribute("name", modifiedKey),
                        new XAttribute(XNamespace.Xml + "space", "preserve"),
                        new XElement("value", originalKey)
                    );

                    // Add the <data> element to the root of the .resx file
                    resxDocument.Root.Add(dataElement);
                }
            }
        }

        // Save the generated .resx file
        resxDocument.Save(outputResxFile);

        Console.WriteLine($"Excel data successfully converted and saved to: {outputResxFile}");
    }
}
