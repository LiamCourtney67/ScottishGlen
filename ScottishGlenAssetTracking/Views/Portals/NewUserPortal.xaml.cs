using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ScottishGlenAssetTracking.Services;
using ScottishGlenAssetTracking.Views.Account;
using ScottishGlenAssetTracking.Views.Employee;
using ScottishGlenAssetTracking.Views.HardwareAsset;
using ScottishGlenAssetTracking.Views.SoftwareAsset;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScottishGlenAssetTracking.Views.Portals
{
    /// <summary>
    /// Page for the new user portal.
    /// </summary>
    public sealed partial class NewUserPortal : Page
    {
        /// <summary>
        /// Constructor for the NewUserPortal class.
        /// </summary>
        public NewUserPortal()
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
            // Check if the invoked item has a tag and navigate to the corresponding page.
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
            // Check the tag of the invoked item and navigate to the corresponding page using the NewUserFrame and dependency injection.
            switch (pageTag)
            {
                // Account
                case "ViewAccount":
                    var viewAccountPage = App.AppHost.Services.GetRequiredService<ViewAccount>();
                    NewUserFrame.Navigate(viewAccountPage.GetType());
                    break;

                // Logout
                case "Logout":
                    // Log out and navigate to the login page.
                    App.AppHost.Services.GetRequiredService<AccountManager>().Logout();

                    var loginPage = App.AppHost.Services.GetRequiredService<Login>();
                    MainWindow.Frame.Navigate(loginPage.GetType());
                    break;

                default:
                    break;
            }
        }
    }
}
