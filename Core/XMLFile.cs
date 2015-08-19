using System;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace DataDiscovery.Core
{

    public class XMLFile: FileBase
    {
        #region Constructor

        public XMLFile(string name, bool header = false): base (name, header)
        {

        }

        #endregion


        public string GetElementColumns(XElement doc)
        {
            try
            {
                ClearElemntColumnsNames();

                foreach (var nodes in doc.Elements())
                {
                    foreach (var item in nodes.Elements())
                    {
                        if(IsFirstElemntHeader)
                        {
                            AddElemntColumnsName(item.Value.ToString());
                        }
                        else 
                        {
                            AddElemntColumnsName(item.Name.ToString());
                        }

                    }

                    break;
                }
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return ex.Message;
            }

            return ElemntNames;
        }

        public int AnalyzeElement(XElement doc)
        {
            foreach (var item in doc.Elements())
            {

            }

            return ++AnalyzedElmentsCounter;
        }

        public async Task<bool>  AnalyzeFileAsync(XElement doc)
        {
            try
            {
                AnalyzedElmentsCounter = 0;

                foreach (var elment in doc.Elements())
                {
                    await Task.Run(() => {  AnalyzeElement(elment);});
                }
            }

            catch (Exception ex)
            {
                LastError = ex.Message;
                return false;
            }

            return true;
        }

        public int GetNodeCount(XElement doc )
        {
            try
            {
                foreach (var row in doc.Elements())
                {
                    ElmentCounter++;

                }   
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return -1;
            }

            return ElmentCounter;
        }
    }
       
}

