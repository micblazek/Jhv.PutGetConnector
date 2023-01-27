using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Jhv.PutGetConnector
{
    public class JhvVariablePutGet : IEquatable<JhvVariablePutGet>, ISerializable, ICloneable
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public DataTypes DataType { get; set; }
        public int DbbAdress { get; set; }
        public int DbxAdress { get; set; }
        public int Lenght { get; set; }
        public enum DataTypes
        {
            Unknown = 0,
            Boolean = 1,
            Int16 = 2,
            Int32 = 3,
            Int64 = 4,
            String = 5,
            Float = 6,
            DateTime = 7,
            Byte = 8,
            Char = 9,
            DWord = 10,
            Date = 11,
            USInt = 12,
            Double = 13,
            Sint = 14,
            UInt32 = 15,
            UInt16 = 16,
            UInt64 = 17,
            Word = 18,
            LWord = 19
        }
        public JhvVariablePutGet(string Name, string Value, DataTypes DataType, int DbbAdress, int DbxAdress, int Lenght)
        {
            this.Name = Name;
            this.Value = Value;
            this.DataType = DataType;
            this.DbbAdress = DbbAdress;
            this.DbxAdress = DbxAdress;
            this.Lenght = Lenght;
        }
        public JhvVariablePutGet()
        {
            Name = "Unknown";
            DbbAdress = 0;
            DbxAdress = 0;
            Lenght = 1;
            Value = "0";
            DataType = DataTypes.String;
        }

        public void setDataType(String name)
        {
            switch (name)
            {
                case "smallint":
                    DataType = DataTypes.Int16;
                    break;
                case "DateTime":
                    DataType = DataTypes.DateTime;
                    break;
                case "real":
                    DataType = DataTypes.Double;
                    break;
                case "uint":
                    DataType = DataTypes.UInt32;
                    break;
                case "bit":
                    DataType = DataTypes.Boolean;
                    break;
                case "int":
                    DataType = DataTypes.Int32;
                    break;
                case "tinyint":
                    DataType = DataTypes.Byte;
                    break;
                case "bigint":
                    DataType = DataTypes.Int64;
                    break;
                default:
                    if (name.Contains("nchar"))
                    {
                        DataType = DataTypes.String;
                        int startInex = name.IndexOf("(");
                        int endInex = name.IndexOf(")");
                        Lenght = Convert.ToInt16(name.Substring(startInex + 1, endInex - startInex - 1));
                    }
                    break;
            }
        }
        public object Clone()
        {
            return new JhvVariablePutGet(Name, Value, DataType, DbbAdress, DbxAdress, Lenght);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ((JhvVariablePutGet)this).GetObjectData(info, context);
            info.AddValue("DbbAdress", DbbAdress);
            info.AddValue("DbxAdress", DbxAdress);
            info.AddValue("Lenght", Lenght);
        }

        public bool Equals( JhvVariablePutGet other)
        {
            return ((JhvVariablePutGet)this).Equals(other) && DbbAdress.Equals(other.DbbAdress) && DbxAdress.Equals(DbxAdress);
        }
    }
}
