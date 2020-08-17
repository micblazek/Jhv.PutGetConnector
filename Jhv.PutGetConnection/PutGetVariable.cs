﻿using Jhv.Core.Data;
using JHV.Core.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Jhv.PutGetConnector
{
    public class PutGetVariable : IJhvVariable, IEquatable<PutGetVariable>, ISerializable, ICloneable
    {
        public JhvVariable Parrent { get; }
        public JhvVariable.ConnectionTypeOption MyConnectionType { get; }
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
        public object Clone()
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
            return ((PutGetVariable)this).Equals(other) && DbbAdress.Equals(other.DbbAdress) && DbxAdress.Equals(DbxAdress);
        }

        public string ToString(JhvVariable var)
        {
            return Parrent.Name + " (" + Parrent.DataType + ") = " + Parrent.Value + "\nAdress: " + DbbAdress+"."+DbxAdress;
        }
    }
}
