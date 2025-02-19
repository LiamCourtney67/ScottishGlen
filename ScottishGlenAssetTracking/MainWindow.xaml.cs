using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ScottishGlenAssetTracking.Services;
using ScottishGlenAssetTracking.ViewModels;
using ScottishGlenAssetTracking.Views.HardwareAsset;
using ScottishGlenAssetTracking.Views.Employee;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using ScottishGlenAssetTracking.Views.SoftwareAsset;
using ScottishGlenAssetTracking.Views.Account;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScottishGlenAssetTracking
{
    /// <summary>
    /// Main window for the application and navigation view.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Static Frame for the application.
        /// </summary>
        public static Frame Frame { get; set; }

        /// <summary>
        /// Constructor for the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            Frame = MainFrame;

            // Navigate to the Login page when the MainWindow is initialized.
            Frame.Navigate(typeof(Login));
        }
    }
}
