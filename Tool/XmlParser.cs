using Jhv.Core.Tool;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace Jhv.PutGetConnector.Tool
{
    public static class XmlParser
    {

        public static VarParams GetVariableOffsets(String fileName)
        {
            VarParams varParams;
            varParams.variableOffset = 0;
            varParams.dataLength = 0;

            XmlDocument variablesDoc;

            variablesDoc = new XmlDocument();
            variablesDoc.Load(fileName);

            try
            {
                foreach (XmlNode variableNode in variablesDoc.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode dataBlockNode in variablesDoc.DocumentElement.ChildNodes)
                    {
                        if (dataBlockNode.Name == "dataBlock")
                        {
                            varParams.dataLength = Convert.ToInt16(dataBlockNode.Attributes.GetNamedItem("length").Value);
                            varParams.variableOffset = Convert.ToInt16(dataBlockNode.Attributes.GetNamedItem("offset").Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string explanation = "Exception: -> ";
                JhvConsole.catchExeption(explanation, ex.ToString());

            }
            return varParams;
        }

        public static List<PlcDataBlock> GetDataBlockList(string sourceFile, List<JhvVariablePutGet> defaultColumn)
        {
            List<PlcDataBlock> fromXML = GetDataBlockList(sourceFile);
            List<PlcDataBlock> outArray = new List<PlcDataBlock>();

            outArray.AddRange(fromXML);

            if (outArray.Count > 0)
            {
                for (int i = defaultColumn.Count - 1; i >= 0; i--)
                {
                    outArray[0].VariableList.Insert(0, defaultColumn[i]);
                }
            }
            return outArray;
        }

        public static List<PlcDataBlock> GetDataBlockList(String fileName)
        {
            List<PlcDataBlock> list = new List<PlcDataBlock>();
            XmlDocument variablesDoc = new XmlDocument();
            variablesDoc.Load(fileName);

            try
            {
                foreach (XmlNode dataBlockNode in variablesDoc.DocumentElement.ChildNodes)
                {
                    if (dataBlockNode.Name == "dataBlock")
                    {
                        PlcDataBlock plcDB = new PlcDataBlock(dataBlockNode.Attributes.GetNamedItem("name").Value, Convert.ToInt16(dataBlockNode.Attributes.GetNamedItem("length").Value));
                        String dbNumber = dataBlockNode.Attributes.GetNamedItem("number").Value;

                        plcDB.DbNumber = (Convert.ToInt16(dbNumber.Substring(dbNumber.IndexOf(Constants.DATA_BLOCK_PREFIX) + 
                            Constants.DATA_BLOCK_PREFIX.Length, dbNumber.Length - dbNumber.IndexOf(Constants.DATA_BLOCK_PREFIX) - Constants.DATA_BLOCK_PREFIX.Length)));
                        JhvConsole.WriteLine("Name=" + dataBlockNode.Attributes.GetNamedItem("name").Value + ", Length=" + dataBlockNode.Attributes.GetNamedItem("length").Value, JhvConsole.STATUS_TIP.DEBUG_INFO);
                        foreach (XmlNode variable in dataBlockNode)
                        {
                            JhvVariablePutGet plcVar = new JhvVariablePutGet();
                            if (variable.Attributes.GetNamedItem("name").Name == "name")
                            {
                                plcVar.Name = variable.Attributes.GetNamedItem("name").Value;
                                foreach (XmlNode node in variable)
                                {
                                    switch (node.Name)
                                    {
                                        case "dbb":
                                            try
                                            {
                                                plcVar.DbbAdress = Convert.ToInt16(node.InnerText);
                                            }
                                            catch (Exception e)
                                            {
                                                JhvConsole.catchExeption(e);
                                            }

                                            break;
                                        case "dbx":
                                            try
                                            {
                                                plcVar.DbxAdress=Convert.ToInt16(node.InnerText);
                                            }
                                            catch (Exception e)
                                            {
                                                JhvConsole.catchExeption(e);
                                            }
                                            break;
                                        case "length":
                                            try
                                            {
                                                plcVar.Lenght = Convert.ToInt16(node.InnerText);
                                            }
                                            catch
                                            {
                                                plcVar.Lenght = 1;
                                            }
                                            break;
                                        case "type":
                                            try
                                            {
                                                plcVar.setDataType(node.InnerText);
                                            }
                                            catch (Exception e)
                                            {
                                                JhvConsole.catchExeption(e);
                                            }
                                            break;
                                    }
                                }

                            }
                            plcDB.VariableList.Add(plcVar);
                        }
                        list.Add(plcDB);
                    }
                }
                JhvConsole.WriteLine("List count is " + list.Capacity, JhvConsole.STATUS_TIP.DEBUG_INFO);
            }
            catch (Exception ex)
            {
                string explanation = "Exception: -> ";
                JhvConsole.catchExeption(explanation, ex.ToString());

            }
            return list;
        }

        public static string[] GetColumnNamesFromXML(string tableName, string sourceFile)
        {
            return GetColumnNamesFromXML(tableName, sourceFile, 0);
        }

        public static string[] GetColumnNamesFromXML(string tableName, string sourceFile, int datablockIndex)
        {
            List<JhvVariablePutGet> variables = new List<JhvVariablePutGet>();
            List<string> columns = new List<string>();

            variables = XmlParser.GetDataBlockList(sourceFile)[datablockIndex].VariableList;

            foreach (JhvVariablePutGet variable in variables)
            {
                columns.Add(variable.Name);
            }

            return columns.ToArray();
        }

        public static string[] GetColumnNamesFromXML(string tableName, string sourceFile, string[] defaultColumn)
        {
            string[] fromXML = GetColumnNamesFromXML(tableName, sourceFile);
            string[] outArray = new String[fromXML.Length + defaultColumn.Length];

            for (int i = 0; i < fromXML.Length + defaultColumn.Length; i++)
            {
                if (i < defaultColumn.Length)
                {
                    outArray[i] = defaultColumn[i];
                }
                else
                {
                    outArray[i] = fromXML[i - defaultColumn.Length];
                }
            }
            return outArray;
        }

        //public static string GetXmlFileByTable(String tableName)
        //{
        //    switch (tableName)
        //    {
        //        case Constants.PROCESS_DATA_TABLE_NAME:
        //        case Constants.PROCESS_DATA_REMOTE_TABLE_NAME:
        //            return Constants.PROCESS_DATA_SOURCE_FILE;
        //        case Constants.TYPE_DATA_TABLE_NAME:
        //            return Constants.TYPE_DATA_SOURCE_FILE;
        //        case Constants.MACHINE_DATA_TABLE_NAME:
        //            return Constants.MACHINE_DATA_SOURCE_FILE;
        //        default:
        //            return "";
        //    }
        //}
    }

    public struct VarParams
    {
        public int variableOffset;
        public int dataLength;
    }
}
