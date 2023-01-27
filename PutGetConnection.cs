using Jhv.PutGetConnector;
using Jhv.PutGetConnector.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JHV.PutGetConnection
{
    public class PutGetConnection
    {

        // General communication definition
        //public Communication comm;
        //public PlcParam plcParam;
        //public General general;

        // ****************************
        // --------STATIC PART---------
        // ****************************
        #region


        //public static void ReadDataBlockFromPLC(S7Client client, PlcDataBlock db)
        //{
        //    ReadDataBlockFromPLC(client, db, 0);
        //}

        public static void ReadDataBlockFromPLC(S7Client client, int DbNumber, PutGetVariable ActVariable)
        {
            int size = 0;
            int result = 0;

            byte[] dbSpace = new byte[ActVariable.Lenght];
            result = client.ReadArea(S7Consts.S7AreaDB, DbNumber, ActVariable.DbbAdress, ActVariable.Lenght, S7Consts.S7WLByte, dbSpace, ref size);
            ActVariable.DecodeValues(dbSpace);
        }

        public static void ReadDataBlockFromPLC(S7Client client, int DbNumber, List<PutGetVariable> ActVariable)
        {
            int size = 0;
            int result = 0;

            int StartAdress = Int32.MaxValue;
            int EndAdress = Int32.MinValue;
            int LastVariableLenght = 0;

            foreach(PutGetVariable v in ActVariable)
            {
                if(v.DbbAdress< StartAdress)
                    StartAdress = v.DbbAdress;
                if (v.DbbAdress > EndAdress)
                {
                    EndAdress = v.DbbAdress;
                    LastVariableLenght = v.Lenght;
                }                   
            }

            int Lenght = EndAdress + LastVariableLenght - StartAdress;


            byte[] dbSpace = new byte[Lenght];
            result = client.ReadArea(S7Consts.S7AreaDB, DbNumber, StartAdress, Lenght, S7Consts.S7WLByte, dbSpace, ref size);
            Decode(dbSpace, ActVariable, StartAdress);
        }

        private static void Decode(byte[] dbSpace, List<PutGetVariable> ActVariable, int Offset = 0)
        {
            foreach(PutGetVariable p in ActVariable)
            {
                byte[] ActSpace = dbSpace.Skip(p.DbbAdress - Offset).Take(p.Lenght).ToArray();
                p.DecodeValues(ActSpace);
            }
        }

        //public static void ReadDataBlocksFromPLC(S7Client client, List<PlcDataBlock> dbList, int offsetStar)
        //{
        //    foreach (PlcDataBlock db in dbList)
        //    {
        //        ReadDataBlockFromPLC(client, db, offsetStar);
        //    }
        //}

        //public static void ReadDataBlocksFromPLC(S7Client client, List<PlcDataBlock> dbList)
        //{
        //    ReadDataBlocksFromPLC(client, dbList, 0);
        //}

        //public static List<PlcDataBlock> ReadProcessData(S7Client client)
        //{
        //    ReadDataBlocksFromPLC(client, Constants.PROCESS_DATA_BLOCK_PLC);
        //    return Constants.PROCESS_DATA_BLOCK_PLC;
        //}

        //public static void WriteConcreateTypList(S7Client client, S7MultiVar writer, ArrayList typeList)
        //{
        //    List<PutGet_PlcVariable> typeVariable = XmlParser.GetDataBlockList(Constants.TYPE_DATA_SOURCE_FILE)[0].getVariableList();
        //    VarParams var = XmlParser.GetVariableOffsets(Constants.TYPE_DATA_SOURCE_FILE);

        //    byte[] db = new byte[var.dataLength];
        //    WriteConcreateTyp(client, writer, typeList, typeVariable, 0, typeList.Count, db, Constants.DATA_TYPES_START_INDEX, var.dataLength);
        //}

        //public static void WriteConcreateTypList(S7Client client, S7MultiVar writer, PlcDataBlock typeList)
        //{
        //    VarParams var = XmlParser.GetVariableOffsets(Constants.TYPE_DATA_SOURCE_FILE);

        //    byte[] db = new byte[var.dataLength];
        //    WriteConcreateTyp(client, writer, typeList.getVariableList(), 0, typeList.getVariableList().Count, db, Constants.DATA_TYPES_START_INDEX, var.dataLength);
        //}

        //private static int LoadConcreteProduct(ArrayList list, List<PutGet_PlcVariable> typeVariable, byte[] dbBuffer, int dataTypeStartIndex, int dataTypeLenght, int startIndex)
        //{

        //    for (int i = startIndex; i < typeVariable.Count; i++)
        //    {
        //        switch (typeVariable[i].getType())
        //        {
        //            //TODO: Doplnit 
        //            case PutGet_PlcVariable.plcDataType.Real:
        //                list.Add(S7.GetRealAt(dbBuffer, typeVariable[i].getDBB()));
        //                break;
        //            case PutGet_PlcVariable.plcDataType.String:
        //                list.Add(S7.GetStringAt(dbBuffer, typeVariable[i].getDBB())); ;
        //                break;
        //            case PutGet_PlcVariable.plcDataType.Byte:
        //                list.Add(S7.GetByteAt(dbBuffer, typeVariable[i].getDBB()));
        //                break;
        //            case PutGet_PlcVariable.plcDataType.Int:
        //                list.Add(S7.GetIntAt(dbBuffer, typeVariable[i].getDBB()));
        //                break;
        //            case PutGet_PlcVariable.plcDataType.Bool:
        //                list.Add(S7.GetBitAt(dbBuffer, typeVariable[i].getDBB(), typeVariable[i].getDBX()));
        //                startIndex = i;
        //                break;
        //            case PutGet_PlcVariable.plcDataType.BigInt:
        //                list.Add(S7.GetLIntAt(dbBuffer, typeVariable[i].getDBB()));
        //                break;
        //        }
        //    }
        //    return startIndex + 1;
        //}

        //public static int WriteConcreateTyp(S7Client client, S7MultiVar writer, ArrayList typeList, List<PutGet_PlcVariable> typeVariable, int startIndexInList, int lenght, byte[] dbBuffer, int startIndexInDB, int dataLenghtDB)
        //{
        //    int result = 0;
        //    int actIndex = 0;

        //    if (typeList.Count > 0)
        //    {

        //        for (int i = startIndexInList; i < startIndexInList + lenght; i++)
        //        {
        //            if (Convert.ToString(typeList[i]).Length <= 0)
        //            {
        //                typeList[i] = 0;
        //            }
        //            if (Convert.ToString(typeList[i]).Length > 0)
        //            {
        //                Type actVariableTyp = typeList[i].GetType();

        //                switch (typeVariable[i].getType())
        //                {
        //                    //TODO: Doplnit 
        //                    case PutGet_PlcVariable.plcDataType.Real:
        //                        //Real
        //                        S7.SetRealAt(dbBuffer, typeVariable[i].getDBB(), (float)Convert.ToDouble(typeList[i]));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.String:
        //                        //String
        //                        String s = Convert.ToString(typeList[i]).Replace(" ", "");
        //                        S7.SetStringAt(dbBuffer, typeVariable[i].getDBB(), s.Length, s);
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.Byte:
        //                        //Byte
        //                        S7.SetByteAt(dbBuffer, typeVariable[i].getDBB(), Convert.ToByte(typeList[i]));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.Int:
        //                        //Small int
        //                        S7.SetIntAt(dbBuffer, typeVariable[i].getDBB(), Convert.ToInt16(typeList[i]));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.Bool:
        //                        S7.SetBitAt(ref dbBuffer, typeVariable[i].getDBB(), typeVariable[i].getDBX(), Convert.ToBoolean(typeList[i]));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.BigInt:
        //                        S7.SetLIntAt(dbBuffer, typeVariable[i].getDBB(), Convert.ToInt16(typeList[i]));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.DWord:
        //                        S7.SetDWordAt(dbBuffer, typeVariable[i].getDBB(), Convert.ToUInt32(typeList[i]));
        //                        break;
        //                }
        //            }
        //            actIndex = typeVariable[i].getDBX();
        //        }

        //        AppSettings tmpAppSettings = new AppSettings();
        //        writer.Add(S7Consts.S7AreaDB, S7Consts.S7WLByte, Convert.ToInt16(tmpAppSettings.DBTypData), startIndexInDB, dataLenghtDB, ref dbBuffer);
        //    }
        //    result = writer.Write();
        //    //ShowResult(client, result, "WriteConcreteType:");
        //    return actIndex;
        //}

        //public static int WriteConcreateTyp(S7Client client, S7MultiVar writer, List<PutGet_PlcVariable> typeVariable, int startIndexInList, int lenght, byte[] dbBuffer, int startIndexInDB, int dataLenghtDB)
        //{
        //    int result = 0;
        //    int actIndex = 0;

        //    if (typeVariable.Count > 0)
        //    {

        //        for (int i = startIndexInList; i < startIndexInList + lenght; i++)
        //        {
        //            if (Convert.ToString(typeVariable[i].getValue()).Length <= 0)
        //            {
        //                typeVariable[i].setValue(0);
        //            }
        //            if (Convert.ToString(typeVariable[i].getValue()).Length > 0)
        //            {
        //                Type actVariableTyp = typeVariable[i].GetType();

        //                switch (typeVariable[i].getType())
        //                {
        //                    //TODO: Doplnit 
        //                    case PutGet_PlcVariable.plcDataType.Real:
        //                        //Real
        //                        S7.SetRealAt(dbBuffer, typeVariable[i].getDBB(), (float)Convert.ToDouble(typeVariable[i].getValue()));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.String:
        //                        //String
        //                        String s = Convert.ToString(typeVariable[i].getValue()).Replace(" ", "");
        //                        S7.SetStringAt(dbBuffer, typeVariable[i].getDBB(), s.Length, s);
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.Byte:
        //                        //Byte
        //                        S7.SetByteAt(dbBuffer, typeVariable[i].getDBB(), Convert.ToByte(typeVariable[i].getValue()));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.Int:
        //                        //Small int
        //                        S7.SetIntAt(dbBuffer, typeVariable[i].getDBB(), Convert.ToInt16(typeVariable[i].getValue()));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.Bool:
        //                        S7.SetBitAt(ref dbBuffer, typeVariable[i].getDBB(), typeVariable[i].getDBX(), Convert.ToBoolean(typeVariable[i].getValue()));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.BigInt:
        //                        S7.SetLIntAt(dbBuffer, typeVariable[i].getDBB(), Convert.ToInt16(typeVariable[i].getValue()));
        //                        break;
        //                    case PutGet_PlcVariable.plcDataType.DWord:
        //                        S7.SetDWordAt(dbBuffer, typeVariable[i].getDBB(), Convert.ToUInt32(typeVariable[i].getValue()));
        //                        break;
        //                }
        //            }
        //            actIndex = typeVariable[i].getDBX();
        //        }

        //        AppSettings tmpAppSettings = new AppSettings();
        //        writer.Add(S7Consts.S7AreaDB, S7Consts.S7WLByte, Convert.ToInt16(tmpAppSettings.DBTypData), startIndexInDB, dataLenghtDB, ref dbBuffer);
        //    }
        //    result = writer.Write();
        //    //ShowResult(client, result, "WriteConcreteType:");
        //    return actIndex;
        //}

        //public static void WriteTypeNameList(S7Client client, S7MultiVar writer, ArrayList names, int startNameIndex, int numberOfCharacters, int maxNumberOfStrings)
        //{
        //    int result = 0;
        //    int oneStringLength = numberOfCharacters + 2;
        //    int index = 0;
        //    int oneLoop = 20;
        //    int totalLength = names.Count;
        //    int size = 0;
        //    int actStart = startNameIndex;
        //    byte[] db = new byte[oneStringLength * oneLoop];

        //    //Zapisuje se to po 20ti hodnotách proto, že když je v 
        //    //writer.Add(S7Consts.S7AreaDB, S7Consts.S7WLByte, plcParam.dbTypList, startNameIndex + 36, 444, ref db);
        //    //číslo větší než 444 tak to nezapíše, v metodě writeTypeNameList2 je další info

        //    for (int j = 0; j * oneLoop < totalLength; j++)
        //    {
        //        AppSettings tmpAppSettings = new AppSettings();
        //        client.ReadArea(S7Consts.S7AreaDB, Convert.ToInt16(tmpAppSettings.DBTypData), actStart, db.Length, S7Consts.S7WLByte, db, ref size);
        //        index = 0;

        //        for (int i = j * oneLoop; i < (j + 1) * oneLoop && i < names.Count; i++)
        //        {
        //            String s = Convert.ToString(names[i]).Replace(" ", "");
        //            S7.SetStringAt(db, index, oneStringLength, s);
        //            index += numberOfCharacters + 2;
        //        }

        //        writer.Add(S7Consts.S7AreaDB, S7Consts.S7WLByte, Convert.ToInt16(tmpAppSettings.DBTypData), actStart, db.Length, ref db);
        //        result = writer.Write();
        //        //ShowResult(client, result, "WriteTypesNameList,  loop = " + j + ":");
        //        actStart += db.Length;
        //    }
        //}
        #endregion

    }
}
