using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ScottishGlenAssetTracking.Services;
using ScottishGlenAssetTracking.Views.Asset;
using ScottishGlenAssetTracking.Views.Employee;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScottishGlenAssetTracking
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Event handler for the item invoked event of the navigation view.
        /// </summary>
        /// <param name="sender">The control that triggered the event.</param>
        /// <param name="e">Event data that provides information about the item invoked event.</param>
        private void Nav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.Tag is not null)
            {
                var itemTag = args.InvokedItemContainer.Tag.ToString();
                NavigateToPage(itemTag);
            }
        }

        /// <summary>
        /// Navigates to the selected page.
        /// </summary>
        /// <param name="pageTag">Tag for the invoked item.</param>
        private void NavigateToPage(string pageTag)
        {
            switch (pageTag)
            {
                // Asset
                case "AddAsset":
                    MainFrame.Navigate(typeof(AddAsset));
                    break;

                case "ViewAsset":
                    MainFrame.Navigate(typeof(ViewAsset));
                    break;

                // Employee
                case "AddEmployee":
                    MainFrame.Navigate(typeof(AddEmployee));
                    break;

                case "ViewEmployee":
                    MainFrame.Navigate(typeof(ViewEmployee));
                    break;

                default:
                    break;
            }
        }
    }
}
