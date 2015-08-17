using System;
using System.IO;
using System.Threading.Tasks;

namespace DataDiscovery.Core
{
    public class CSVFile: FileBase
    {
        #region Constructer

        public CSVFile(string name, bool header = true): base(name,header)
        {
        }

        #endregion

        public int AnalyzeLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return ElmentCounter;

            return ++AnalyzedElmentsCounter;
        }

        public int GetLineCount(TextReader reader)
        {
            try
            {
                var buffer = new char[32 * 1024];
                ElmentCounter = 1;
                int read;
                
                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < read; i++)
                    {
                        if (buffer[i] == '\n')
                            ElmentCounter++;
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return -1;
            }

            return ElmentCounter;
        }

        public string GetElementColumns(TextReader reader)
        {


            try
            {
                ClearElemntColumnsNames();
                string line = string.Empty;

                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        break;
                }

                var lineElemnets = line.Split(',');

                if(IsFirstElemntHeader)
                {
                    foreach (var name in lineElemnets) 
                    {
                        AddElemntColumnsName(name);
                    }
                }
                else
                {
                    for (int i = 0; i < lineElemnets.Length; i++) {
                        AddElemntColumnsName(string.Format("Column{0}", i+1));
                    }
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return ex.Message;
            }



            return ElemntNames;
        }

        public async Task<bool> AnalyzeFileAsync(TextReader reader)
        {
            var line = string.Empty;
           AnalyzedElmentsCounter = 0;

            try
            {
                
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    AnalyzeLine(line);
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }
                
            return true;
        }
    }
}

