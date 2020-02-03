using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sample.Common.Helper
{
    public class XMLHandler
    {
        public XDocument xDoc;
        public XNamespace nameSpace;
        public string pathFileName;

        public XMLHandler() { }
        public XMLHandler LoadXMLFile(string pathFile)
        {
            xDoc = XDocument.Load(pathFile);
            if (xDoc.Root.Attribute("xmlns") != null)
                nameSpace = xDoc.Root.Attribute("xmlns").Value;
            else
                nameSpace = "";
            pathFileName = pathFile;
            return this;

        }

        public string GetValueOfNodeByName(string elementPath)
        {
            XElement reqElement = null;
            string[] elementName = elementPath.Split('/');

            XElement pElement = xDoc.Root;
            foreach (string name in elementName)
            {
                reqElement = GetChildElement(pElement, name);
                pElement = reqElement;
            }
            if (reqElement == null)
                return null;
            return reqElement.Value;
        }

        public string GetValueOfNodeByNameSkipElementNotExist(string elementPath, bool skip = true)
        {

            XElement reqElement = null;
            string[] elementName = elementPath.Split('/');
            if (skip)
            {
                foreach (var element in elementName)
                {
                    if (!CheckElementExistByName(element))
                    {
                        return "";
                    }
                }

            }

            XElement pElement = xDoc.Root;
            foreach (string name in elementName)
            {
                reqElement = GetChildElement(pElement, name);
                pElement = reqElement;
            }

            return reqElement.Value;
        }

        public string GetValueOfNodeByName(XElement parentElement, string elementPath)
        {
            XElement reqElement = null;
            string[] elementName = elementPath.Split('/');

            foreach (string name in elementName)
            {
                reqElement = GetChildElement(parentElement, name);
                parentElement = reqElement;
            }

            return reqElement.Value;
        }


        public XElement GetChildElement(XElement parentElement, string name)
        {
            var childElement = parentElement.Element(nameSpace + name);
            return childElement == null ? null : childElement;
        }
        
        public bool CheckElementExistByName(string elementName)
        {
            XElement pElement = xDoc.Root;
            var nodeList = pElement.Descendants();
            foreach (var node in nodeList)
            {
                if (node.Name == elementName)
                    return true;
            }
            return false;
        }

        public T XMLDeserialize<T>(string input) where T : class
        {
            var mappingClass = (T)Activator.CreateInstance(typeof(T));
            XmlSerializer ser = new XmlSerializer(typeof(T));

            using (Stream reader = new FileStream(input, FileMode.Open))
            {
                mappingClass = (T)ser.Deserialize(reader);
                reader.Close();
            }
            return mappingClass;
        }

        public string XMLSerialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }

        public void EditNodes(List<KeyValuePair<string, string>> fieldChangesList)
        {
            XElement pElement = xDoc.Root;
            var nodeList = pElement.Descendants();
            foreach (var node in nodeList)
                if (fieldChangesList.Exists(field => field.Key == node.Name.LocalName))
                {
                    node.SetValue(fieldChangesList.Find(field => field.Key == node.Name.LocalName).Value);
                }

            xDoc.Save(pathFileName);
        }

        private void WriteElementString(XmlWriter xmlWriter, string element, string value = null)
        {
            try
            {
                xmlWriter.WriteElementString(element, value.Trim());
            }
            catch (Exception)
            {
                xmlWriter.WriteElementString(element, "");
            }
        }

        //public string ConvertToXML(string ppFilePath)
        //{
        //    //Read File
        //    var lines = File.ReadAllLines(ppFilePath);

        //    //Convert to XML file
        //    string newXMLFile = ppFilePath.GetFileNameFromFilePath().GetFileNameWithoutExtension() + ".xml";
        //    string xmlFilePath = ppFilePath.GetDirectoryFromFullFilePath() + "\\" + newXMLFile;
        //    XmlWriter xmlWriter = XmlWriter.Create(xmlFilePath);
        //    xmlWriter.WriteStartElement("FNBO_PositivePay_Record");
        //    xmlWriter.WriteStartElement("PP_Envelope");

        //    //Line 1 to penultimate line
        //    for (int i = 0; i <= lines.Length - 2; i++)
        //    {
        //        xmlWriter.WriteStartElement("DetailRecords_Envelope");
        //        xmlWriter.WriteStartElement("DetailRecord");
        //        WriteElementString(xmlWriter, "RecordType", lines[i].Substring(0, 1));
        //        WriteElementString(xmlWriter, "AccountNumber", lines[i].Substring(1, 10));
        //        WriteElementString(xmlWriter, "CheckNumber", lines[i].Substring(11, 10));
        //        WriteElementString(xmlWriter, "IssueDate", lines[i].Substring(21, 8));
        //        WriteElementString(xmlWriter, "IssueAmount", lines[i].Substring(29, 10));
        //        WriteElementString(xmlWriter, "PayeeName1", lines[i].Substring(39, 60));
        //        WriteElementString(xmlWriter, "PayeeName2", lines[i].Substring(99, 60));
        //        xmlWriter.WriteEndElement();
        //        xmlWriter.WriteEndElement();
        //    }

        //    //Last line
        //    int lastLine = lines.Length - 1;
        //    xmlWriter.WriteStartElement("TrailerRecord");
        //    WriteElementString(xmlWriter, "RecordType", lines[lastLine].Substring(0, 1));
        //    WriteElementString(xmlWriter, "NetTotalAmount", lines[lastLine].Substring(1, 12));
        //    WriteElementString(xmlWriter, "Filler", lines[lastLine].Substring(13, Math.Min(146, lines[lastLine].Length - 13)));
        //    xmlWriter.WriteEndElement();

        //    //End xmlWriter.WriteEndElement;
        //    xmlWriter.WriteEndElement();

        //    //End FNBO_PositivePay_Record
        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Close();

        //    return xmlFilePath;
        //}

        public List<string> EditNodeByNameWithExpectedValue(string nodeName, string expectedValue)
        {
            XElement pElement = xDoc.Root;
            List<string> valueList = new List<string>();
            var nodeList = pElement.Descendants();
            foreach (var node in nodeList)
                if (node.Name.LocalName == nodeName)
                {
                    if (node.Value != expectedValue)
                        node.SetValue(expectedValue);
                    valueList.Add(expectedValue);
                }

            xDoc.Save(pathFileName);

            return valueList;
        }

        public List<string> GetListValueNodes(string nodeName)
        {
            XElement pElement = xDoc.Root;
            List<string> valueList = new List<string>();
            var nodeList = pElement.Descendants();
            foreach (var node in nodeList)
            {
                if (node.Name.LocalName == nodeName)
                {
                    valueList.Add(node.Value);
                }
            }
            return valueList;
        }

        public int GetNumberOfNodes(string nodeName)
        {
            XElement pElement = xDoc.Root;
            var nodeList = pElement.Descendants();
            int count = nodeList.Select(n => n.Name.LocalName).Count(n => n == nodeName);
            return count;
        }

    }
}
