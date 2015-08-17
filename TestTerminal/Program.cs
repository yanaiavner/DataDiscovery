using System;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using DataDiscovery.Core;


namespace TestTerminal
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            

//            Directory.SetCurrentDirectory(@"/Users/ayanai/Documents/");
//            var localDir = System.Environment.CurrentDirectory;
//            Console.WriteLine("You are at: {0}{1}", localDir, System.Environment.NewLine);
//
//            Console.Write("File To Open:");
//            var fileName = Console.ReadLine();
            string fileName = string.Empty;

            if (args.Length > 0)
                fileName = args[0];
//            fileName = @"/Users/ayanai/Documents/Test_Data.csv";
//            fileName = @"/Users/ayanai/Documents/Test_Data.xml";
//            fileName = @"/Users/ayanai/Documents/Test_List.xml";

            var fileExists = File.Exists(fileName);

            Console.WriteLine("{0}Your file {1} {2}{0}", Environment.NewLine, fileName, fileExists ? "exists" : "dous not exists");

            if (!fileExists)
                return;


            var xmlFile = XElement.Load(fileName,
                LoadOptions.SetBaseUri | LoadOptions.SetLineInfo);


            var newXMLFile = new XMLFile("test", true);
            newXMLFile.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => 
                {
                    if (e.PropertyName != "AnalyzedElmentsCounter")
                        return;

                    Console.Write("{0} of {1} lines", newXMLFile.AnalyzedElmentsCounter, newXMLFile.ElmentCounter);
                    Console.CursorLeft = 0;
                };


            Console.WriteLine("{0}{1} lins in file{0}", Environment.NewLine, newXMLFile.GetNodeCount(xmlFile));
            Console.WriteLine("{0}{1}{0}", Environment.NewLine, newXMLFile.GetElementColumns(xmlFile));

            Console.CursorVisible = false;
            var analyzeXMLResult = newXMLFile.AnalyzeFileAsync(xmlFile);
            analyzeXMLResult.Wait();
            Console.CursorVisible = true;

            Console.WriteLine("{0}{0}File {1}", Environment.NewLine, analyzeXMLResult.Result ? "analyzed" : "not analyzed");

            //newXMLFile.AnalyzeFileAsync(xmlFile);
            //Console.WriteLine("{0}{0}{1}", Environment.NewLine, newXMLFile.ColumnsNames);

            return;

            var newCSVFile = new CSVFile("test_file");
            newCSVFile.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) =>
                {
                    if (e.PropertyName != "AnalyzedElmentsCounter")
                        return;

                    Console.Write("{0} of {1} lines", newCSVFile.AnalyzedElmentsCounter, newCSVFile.ElmentCounter);
                    Console.CursorLeft = 0;
                };
            
            using (var reader = new StreamReader(fileName))
            { 
                Console.WriteLine("{0}{1} lins in file{0}", Environment.NewLine, newCSVFile.GetLineCount(reader));
            }
             
            using (var reader = new StreamReader(fileName))
            {
                Console.WriteLine("{0}{1}{0}", Environment.NewLine, newCSVFile.GetElementColumns(reader));
            }

            using (var reader = new StreamReader(fileName))
            {
                Console.CursorVisible = false;
                var analyzeResult = newCSVFile.AnalyzeFileAsync(reader);
                analyzeResult.Wait();
                Console.CursorVisible = true;

                Console.WriteLine("{0}{0}File {1}", Environment.NewLine, analyzeResult.Result ? "analyzed" : "not analyzed");
        
            }
        }
    }
}
