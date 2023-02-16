using Jhv.PutGetConnector;
using JHV.Core.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using static JHV.Core.Abstract.AJhvVariable;

namespace JHV.PutGetConnector.Convertors
{
    public class LenghtConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(Dictionary<ConnectionTypeOption, AJhvConnection>))
                {
                    Dictionary<ConnectionTypeOption, AJhvConnection> Connections = value as Dictionary<ConnectionTypeOption, AJhvConnection>;

                    if (Connections.Count > 0)
                    {
                        AJhvConnection ActVar;
                        if (Connections.TryGetValue(ConnectionTypeOption.PutGet, out ActVar))
                        {
                            if (ActVar.GetType() == typeof(PutGetVariable))
                                return (ActVar as PutGetVariable).Lenght;
                        }
                        if (Connections.TryGetValue(ConnectionTypeOption.PutGet_2, out ActVar))
                        {
                            if (ActVar.GetType() == typeof(PutGetVariable))
                                return (ActVar as PutGetVariable).Lenght;
                        }
                        if (Connections.TryGetValue(ConnectionTypeOption.PutGet_3, out ActVar))
                        {
                            if (ActVar.GetType() == typeof(PutGetVariable))
                                return (ActVar as PutGetVariable).Lenght;
                        }
                        if (Connections.TryGetValue(ConnectionTypeOption.PutGet_4, out ActVar))
                        {
                            if (ActVar.GetType() == typeof(PutGetVariable))
                                return (ActVar as PutGetVariable).Lenght;
                        }
                        if (Connections.TryGetValue(ConnectionTypeOption.PutGet_5, out ActVar))
                        {
                            if (ActVar.GetType() == typeof(PutGetVariable))
                                return (ActVar as PutGetVariable).Lenght;
                        }
                        return "Nevybráno";
                    }
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
