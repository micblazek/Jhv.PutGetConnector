using Jhv.Core.Data;
using Jhv.PutGetConnector.Tool;
using JHV.Core.Abstract;
using JHV.Core.Data;
using JHV.Core.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using static JHV.Core.Abstract.AJhvVariable;
using System.Xml.Serialization;

namespace Jhv.PutGetConnector
{
    public class PutGetVariable : AJhvConnection, IEquatable<PutGetVariable>, ISerializable, ICloneable
    {
        [XmlIgnore]
        public override AJhvVariable Parrent { get; set; }
        public override ConnectionTypeOption MyConnectionType { get; set; }
        public int DbbAdress { get; set; }
        public int DbxAdress { get; set; }
        public int Lenght { get; set; }

        public PutGetVariable(string Name, string Value, JhvVariable.DataTypesOption DataType, int DbbAdress, int DbxAdress, int Lenght, JhvVariable.ConnectionTypeOption ConnectionType = JhvVariable.ConnectionTypeOption.PutGet)
        {
            Parrent = new JhvVariable(Name, Value, DataType);
            this.DbbAdress = DbbAdress;
            this.DbxAdress = DbxAdress;
            this.Lenght = Lenght;
            MyConnectionType = ConnectionType;
            if (!Parrent.Connections.ContainsKey(ConnectionType))
                Parrent.Connections.Add(ConnectionType, this);
        }
        public PutGetVariable(AJhvVariable Parrent, int DbbAdress, int DbxAdress, int Lenght, JhvVariable.ConnectionTypeOption ConnectionType = JhvVariable.ConnectionTypeOption.PutGet)
        {
            this.Parrent = Parrent;
            this.DbbAdress = DbbAdress;
            this.DbxAdress = DbxAdress;
            this.Lenght = Lenght;
            MyConnectionType = ConnectionType;
            if (!Parrent.Connections.ContainsKey(ConnectionType))
                Parrent.Connections.Add(ConnectionType, this);
        }
        public PutGetVariable()
        {
            Parrent = new JhvVariable();
            DbbAdress = 0;
            DbxAdress = 0;
            Lenght = 1;
            MyConnectionType = JhvVariable.ConnectionTypeOption.PutGet;
            Parrent.Connections.Add(JhvVariable.ConnectionTypeOption.PutGet, this);
        }

        public PutGetVariable(JhvVariable Parrent, int DbbAdress, int DbxAdress, int Lenght, JhvVariable.ConnectionTypeOption ConnectionType = JhvVariable.ConnectionTypeOption.PutGet)
        {
            this.Parrent = Parrent;
            this.DbbAdress = DbbAdress;
            this.DbxAdress = DbxAdress;
            this.Lenght = Lenght;
            MyConnectionType = ConnectionType;
            Parrent.Connections.Add(ConnectionType, this);
        }

        private void SetPutGetStringToDataTypes(String name)
        {
            switch (name)
            {
                case "smallint":
                    Parrent.DataType = JhvVariable.DataTypesOption.Int16;
                    break;
                case "DateTime":
                    Parrent.DataType = JhvVariable.DataTypesOption.DateTime;
                    break;
                case "real":
                    Parrent.DataType = JhvVariable.DataTypesOption.Double;
                    break;
                case "uint":
                    Parrent.DataType = JhvVariable.DataTypesOption.UInt32;
                    break;
                case "bit":
                    Parrent.DataType = JhvVariable.DataTypesOption.Boolean;
                    break;
                case "int":
                    Parrent.DataType = JhvVariable.DataTypesOption.Int32;
                    break;
                case "tinyint":
                    Parrent.DataType = JhvVariable.DataTypesOption.Byte;
                    break;
                case "bigint":
                    Parrent.DataType = JhvVariable.DataTypesOption.Int64;
                    break;
                default:
                    if (name.Contains("nchar"))
                    {
                        Parrent.DataType = JhvVariable.DataTypesOption.String;
                        int startInex = name.IndexOf("(");
                        int endInex = name.IndexOf(")");
                        Lenght = Convert.ToInt16(name.Substring(startInex + 1, endInex - startInex - 1));
                    }
                    break;
            }
        }

        public void DecodeValues(byte[] dbBuffer, int Offset = 0)
        {
            switch (Parrent.DataType)
            {
                case JhvVariable.DataTypesOption.Boolean:
                    Parrent.Value = S7.GetBitAt(dbBuffer, Offset, DbxAdress).ToString();
                    break;                
                case JhvVariable.DataTypesOption.Byte:
                    Parrent.Value = S7.GetByteAt(dbBuffer, Offset).ToString();
                    break;               
                case JhvVariable.DataTypesOption.String:
                    Parrent.Value = S7.GetStringAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.DateTime:
                    Parrent.Value = S7.GetDateTimeAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.Int16:
                    Parrent.Value = S7.GetSIntAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.Int32:
                    Parrent.Value = S7.GetIntAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.Int64:
                    Parrent.Value = S7.GetDIntAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.UInt16:
                    Parrent.Value = S7.GetUSIntAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.UInt32:
                    Parrent.Value = S7.GetUIntAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.UInt64:
                    Parrent.Value = S7.GetUDIntAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.Word:
                    Parrent.Value = S7.GetWordAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.DWord:
                    Parrent.Value = S7.GetDWordAt(dbBuffer, Offset).ToString();
                    break;
                case JhvVariable.DataTypesOption.Double:
                    Parrent.Value = S7.GetRealAt(dbBuffer, Offset).ToString();
                    break;
            }
        }

        public override object Clone()
        {
            return new PutGetVariable(Parrent, DbbAdress, DbxAdress, Lenght);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((PutGetVariable)this).GetObjectData(info, context);
            info.AddValue("DbbAdress", DbbAdress);
            info.AddValue("DbxAdress", DbxAdress);
            info.AddValue("Lenght", Lenght);
        }

        public bool Equals(PutGetVariable other)
        {
            if (other == null)
                return false;

            return (Parrent).Equals(other.Parrent) && DbbAdress.Equals(other.DbbAdress) && DbbAdress.Equals(other.DbxAdress) && DbbAdress.Equals(other.Lenght);
        }

        public override bool Equals(AJhvConnection other)
        {
            if (other is PutGetVariable)
            {
                return DbbAdress.Equals((other as PutGetVariable).DbbAdress) && DbxAdress.Equals((other as PutGetVariable).DbxAdress) && Lenght.Equals((other as PutGetVariable).Lenght); 
            }
            else
            {
                return false;
            }
        }

        public string ToString(JhvVariable var)
        {
            return Parrent.Name + " (" + Parrent.DataType + ") = " + Parrent.Value + "\nAdress: " + DbbAdress + "." + DbxAdress;
        }
    }
}
