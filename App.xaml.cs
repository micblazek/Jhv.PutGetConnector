﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Jhv.PutGetConnector
{
    /// <summary>
    /// Interakční logika pro App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string Commit()
        {
            return ThisAssembly.Git.Commit;
        }

        public static string CommitDate()
        {
            return ThisAssembly.Git.CommitDate;
        }

        public static string Branch()
        {
            return ThisAssembly.Git.Branch;
        }
    }
}
