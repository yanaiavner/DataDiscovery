using System;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Linq;

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

                var firstElement = doc.Elements().FirstOrDefault();

                EterateXMLElmentColumn(firstElement,1);
            }
            catch (Exception ex)
            {
                LastError = ex.Message;
                return ex.Message;
            }

            return ElemntNames;
        }

        private void EterateXMLElmentColumn(XElement doc, int level)
        {
            if(IsFirstElemntHeader)
            {
                AddElemntColumnsName(doc.Value.ToString(),level);
            }
            else 
            {
                AddElemntColumnsName(doc.Name.ToString(),level);
            }

            var nextLevel = ++level;

            foreach (var item in doc.Elements())
            {

                EterateXMLElmentColumn(item, nextLevel);
            }
        }

        public int AnalyzeElement(XElement doc)
        {
            EterateXMLElment(doc, 1);

            return ++AnalyzedElmentsCounter;
        }

        private void EterateXMLElment(XElement doc, int level)
        {
            AddElemntColumnsName(doc.Name.ToString(), level);

            var nextLevel = level++;

            foreach (var item in doc.Elements())
            {
                EterateXMLElment(item, nextLevel);
            }
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

