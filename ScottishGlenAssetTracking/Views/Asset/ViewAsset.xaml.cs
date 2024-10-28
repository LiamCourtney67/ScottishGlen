using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using ScottishGlenAssetTracking.Models;
using ScottishGlenAssetTracking.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ScottishGlenAssetTracking.Views.Asset
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewAsset : Page
    {
        private Models.Asset _selectedAsset;

        public ViewAsset()
        {
            this.InitializeComponent();
            List<Department> departments = new DepartmentService().GetDepartments();
            DepartmentSelect.ItemsSource = departments;
            AssetDepartmentSelect.ItemsSource = departments;
        }
        private void DepartmentSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DepartmentSelect.SelectedItem != null)
            {
                List<Models.Employee> employees = new EmployeeService().GetEmployees(((Department)DepartmentSelect.SelectedItem).Id);
                EmployeeSelect.ItemsSource = employees;
                AssetEmployeeSelect.ItemsSource = employees;
            }
        }

        private void EmployeeSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeSelect.SelectedItem != null)
            {
                AssetSelect.ItemsSource = new AssetService().GetAssets(((Models.Employee)EmployeeSelect.SelectedItem).Id);
            }
        }

        private void AssetSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateAssetDetails();
            ViewAssetView.Visibility = Visibility.Visible;
        }

        private void PopulateAssetDetails()
        {
            var asset = (Models.Asset)AssetSelect.SelectedItem;
            if (asset != null)
            {
                AssetName.Text = $"Name: {asset.Name}";
                AssetModel.Text = $"Model: {asset.Model}";
                AssetManufacturer.Text = $"Manufacturer: {asset.Manufacturer}";
                AssetType.Text = $"Type: {asset.Type}";
                AssetIpAddress.Text = $"IP Address: {asset.IpAddress}";
                AssetPurchaseDate.Text = $"Purchase Date: {asset.PurchaseDate}";
                AssetNotes.Text = $"Notes: {asset.Notes}";
                AssetDepartment.Text = $"Department: {asset.Employee.Department.Name}";
                AssetEmployee.Text = $"Employee: {asset.Employee.FirstName} {asset.Employee.LastName}";

                _selectedAsset = asset;
            }
            else
            {
                AssetName.Text = "Name: ";
                AssetModel.Text = "Model: ";
                AssetManufacturer.Text = "Manufacturer: ";
                AssetType.Text = "Type: ";
                AssetIpAddress.Text = "IP Address: ";
                AssetPurchaseDate.Text = "Purchase Date: ";
                AssetNotes.Text = "Notes: ";
                AssetDepartment.Text = "Department: ";
                AssetEmployee.Text = "Employee: ";

                _selectedAsset = null;
            }
            PopulateInputDetails(asset);
        }

        private void PopulateInputDetails(Models.Asset asset)
        {
            if (asset != null)
            {
                AssetNameInput.Text = asset.Name;
                AssetModelInput.Text = asset.Model;
                AssetManufacturerInput.Text = asset.Manufacturer;
                AssetTypeInput.Text = asset.Type;
                AssetIpAddressInput.Text = asset.IpAddress;
                AssetPurchaseDateInput.Date = (DateTime)asset.PurchaseDate;
                AssetNotesInput.Text = asset.Notes;

                // Set the asset department in the dropdown.
                AssetDepartmentSelect.SelectedItem = ((List<Department>)AssetDepartmentSelect.ItemsSource)
                    .FirstOrDefault(d => d.Id == asset.Employee.Department.Id);

                // Set the asset employee in the dropdown.
                AssetEmployeeSelect.SelectedItem = ((List<Models.Employee>)AssetEmployeeSelect.ItemsSource)
                    .FirstOrDefault(e => e.Id == asset.Employee.Id);
            }
            else
            {
                AssetNameInput.Text = string.Empty;
                AssetModelInput.Text = string.Empty;
                AssetManufacturerInput.Text = string.Empty;
                AssetTypeInput.Text = string.Empty;
                AssetIpAddressInput.Text = string.Empty;
                AssetPurchaseDateInput.Date = DateTime.Now;
                AssetNotesInput.Text = string.Empty;
                AssetDepartmentSelect.SelectedItem = null;
                AssetEmployeeSelect.SelectedItem = null;
            }
        }

        private void DeleteAssetButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedAsset != null)
            {
                new AssetService().DeleteAsset(_selectedAsset.Id);
                Status.Text = "Asset Deleted";
                PopulateAssetDetails();
                AssetSelect.ItemsSource = new AssetService().GetAssets(((Models.Employee)EmployeeSelect.SelectedItem).Id);
            }
            else
            {
                Status.Text = "No Asset Selected";
            }
        }

        private void EditAssetButton_Click(object sender, RoutedEventArgs e)
        {
            ViewAssetView.Visibility = Visibility.Collapsed;
            EditAssetView.Visibility = Visibility.Visible;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            EditAssetView.Visibility = Visibility.Collapsed;
            ViewAssetView.Visibility = Visibility.Visible;
        }

        private void UpdateAssetButton_Click(object sender, RoutedEventArgs e)
        {
            _selectedAsset.Name = AssetNameInput.Text;
            _selectedAsset.Model = AssetModelInput.Text;
            _selectedAsset.Manufacturer = AssetManufacturerInput.Text;
            _selectedAsset.Type = AssetTypeInput.Text;
            _selectedAsset.IpAddress = AssetIpAddressInput.Text;
            _selectedAsset.PurchaseDate = AssetPurchaseDateInput.Date.Date;
            _selectedAsset.Notes = AssetNotesInput.Text;
            _selectedAsset.Employee = (Models.Employee)AssetEmployeeSelect.SelectedItem;
            new AssetService().UpdateAsset(_selectedAsset);
            Status.Text = "Asset Updated";
            PopulateAssetDetails();
            EditAssetView.Visibility = Visibility.Collapsed;
            ViewAssetView.Visibility = Visibility.Visible;
            PopulateInputDetails(_selectedAsset);
        }

        private void AssetDepartmentSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AssetDepartmentSelect.SelectedItem != null)
            {
                AssetEmployeeSelect.ItemsSource = new EmployeeService().GetEmployees(((Department)AssetDepartmentSelect.SelectedItem).Id);
            }
        }

        private void AssetEmployeeSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
