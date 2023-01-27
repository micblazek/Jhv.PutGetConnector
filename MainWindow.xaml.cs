
using Jhv.Core.Data;
using Jhv.Core.Tool;
using Jhv.PutGetConnector.Tool;
using JHV.PutGetConnection;
using System.Windows;

namespace Jhv.PutGetConnector
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            JhvVariable DbUIntTest = new JhvVariable("Test_UDInt", "0", JhvVariable.DataTypesOption.UInt64);
            PutGetVariable PutGet_DbUIntTest = new PutGetVariable(DbUIntTest, 346, 0, 4);


            S7Client mS7Client = new S7Client();
            S7MultiVar mS7MultiVar = new S7MultiVar(mS7Client);

            int resultCode = mS7Client.ConnectTo("192.168.1.1",
                0, 1);
            if (resultCode == 0)
            {
                PutGetConnection.ReadDataBlockFromPLC(mS7Client, 801, PutGet_DbUIntTest);
            }
            else
            {
                JhvConsole.WriteLine("PutGet not ready", JhvConsole.STATUS_TIP.ALWAYS);
            }

            string s = App.Commit();
            string s2 = App.CommitDate();
            string s3 = App.Branch();
        }
    }
}
