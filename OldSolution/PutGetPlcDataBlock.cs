using Jhv.PutGetConnector.Tool;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jhv.PutGetConnector
{
    public class PlcDataBlock : ICloneable
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public int DbNumber { get; set; }
        public List<JhvVariablePutGet> VariableList { get; set; }

        public PlcDataBlock(string Name, int Length, int dbNumber)
        {
            this.Name = Name;
            this.Length = Length;
            this.DbNumber = DbNumber;
            this.VariableList = new List<JhvVariablePutGet>();
        }

        public PlcDataBlock(string Name, int Length)
        {
            this.Name = Name;
            this.Length = Length;
            this.DbNumber = 1;
            this.VariableList = new List<JhvVariablePutGet>();
        }
        public void inserLoadedData(byte[] dbBuffer)
        {
            InserLoadedData(dbBuffer, 0);
        }

        public void InserLoadedData(byte[] dbBuffer, int startIndex)
        {
            for (int i = startIndex; i < VariableList.Count; i++)
            {
                switch (VariableList[i].DataType)
                {
                    case JhvVariablePutGet.DataTypes.Boolean:
                        VariableList[i].Value = S7.GetBitAt(dbBuffer, VariableList[i].DbbAdress, VariableList[i].DbxAdress).ToString();
                        break;
                    case JhvVariablePutGet.DataTypes.Double:
                        VariableList[i].Value = S7.GetRealAt(dbBuffer, VariableList[i].DbbAdress).ToString();
                        break;
                    case JhvVariablePutGet.DataTypes.Byte:
                         VariableList[i].Value = S7.GetByteAt(dbBuffer, VariableList[i].DbbAdress).ToString();
                        break;
                    case JhvVariablePutGet.DataTypes.Int32:
                         VariableList[i].Value = S7.GetIntAt(dbBuffer, VariableList[i].DbbAdress).ToString();
                        break;
                    case JhvVariablePutGet.DataTypes.String:
                         VariableList[i].Value = S7.GetStringAt(dbBuffer, VariableList[i].DbbAdress).ToString();
                        break;
                    case JhvVariablePutGet.DataTypes.DateTime:
                         VariableList[i].Value = S7.GetDateTimeAt(dbBuffer, VariableList[i].DbbAdress).ToString();
                        break;
                    case JhvVariablePutGet.DataTypes.Int64:
                         VariableList[i].Value = S7.GetStringAt(dbBuffer, VariableList[i].DbbAdress).ToString();
                        break;
                    case JhvVariablePutGet.DataTypes.DWord:
                         VariableList[i].Value = S7.GetStringAt(dbBuffer, VariableList[i].DbbAdress).ToString();
            break;
                }
                startIndex = i;
            }
        }
        public static string GetValueByName(List<JhvVariablePutGet> tmpList, string varName)
        {
            for (int i = 0; i < tmpList.Count; i++)
            {
                if (tmpList[i].Name == varName)
                {
                    return tmpList[i].Value;
                }
            }
            return null;
        }

        public string GetValueByName(string varName)
        {
            for (int i = 0; i < VariableList.Count; i++)
            {
                if (VariableList[i].Name == varName)
                {
                    return VariableList[i].Value;
                }
            }
            return null;
        }

        public string GetActValueByName(string varName)
        {
            for (int i = 0; i < VariableList.Count; i++)
            {
                if (VariableList[i].Name.Contains(varName))
                {
                    return VariableList[i].Value;
                }
            }
            return null;
        }
        public JhvVariablePutGet GetVariableByName(string varName)
        {
            for (int i = 0; i < VariableList.Count; i++)
            {
                if (VariableList[i].Name.Contains(varName))
                {
                    return VariableList[i];
                }
            }
            return null;
        }

        public int GetVariableIndexByName(string varName)
        {
            for (int i = 0; i < VariableList.Count; i++)
            {
                if (VariableList[i].Name.Equals(varName))
                {
                    return i;
                }
            }
            return -1;
        }

        public void SetVariableList(List<JhvVariablePutGet> varList)
        {
            VariableList = varList;
        }

        public void RemoveVariables(List<JhvVariablePutGet> removeList)
        {
            foreach (JhvVariablePutGet removedVariable in removeList)
            {
                //getVariableList().Remove(removedVariable);
                for (int i = 0; i < VariableList.Count; i++)
                {
                    if (VariableList[i].Name.Equals(removedVariable.Name))
                    {
                        VariableList.Remove(VariableList[i]);
                        i--;
                    }
                }
            }
        }

        public object Clone()
        {
            PlcDataBlock newInstance = new PlcDataBlock(Name, Length, DbNumber);
            this.VariableList.ForEach((item) =>
            {
                newInstance.VariableList.Add((JhvVariablePutGet)item.Clone());
            });

            return newInstance;
        }
    }
}
