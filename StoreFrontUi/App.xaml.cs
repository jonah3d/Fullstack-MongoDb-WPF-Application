﻿using System.Configuration;
using System.Data;
using System.Windows;
using QuestPDF.Infrastructure;

namespace StoreFrontUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            base.OnStartup(e);
        }
    }

}
