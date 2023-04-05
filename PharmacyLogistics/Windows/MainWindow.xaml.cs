using Castle.Core.Internal;
using PharmacyLogistics.Entities;
using PharmacyLogistics.UserControls;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PharmacyLogistics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        private User User { get; set; }
        private List<Product> displayProduct { get; set; }
        private List<Request> displayRequest { get; set; }
        private Product currentProduct { get; set; }
        private static MainWindow MW { get; set; }
        public MainWindow(User user)
        {
            InitializeComponent();
            User = user;
            Fio_Label.Content = string.Join(' ', User.Surname, User.Name, User.Patryonomic);           
            UpdateSupplier_Comboboxes();
            SearchReleaseForm_Combobox.Items.Add("Все");
            foreach(var item in AptContext.aptContext.Releaseforms.ToList())
            {
                SearchReleaseForm_Combobox.Items.Add(item.Name);
            }
            MW = this;
            SearchReleaseForm_Combobox.SelectedIndex = 0;
            AddOrEditSupplier_Grid.Visibility = Visibility.Hidden;
            CreateAkt_Button.Visibility = Visibility.Hidden;
            AddOrEditProd_Grid.Visibility = Visibility.Hidden;
            Acceptance_TabItem.Visibility = Visibility.Hidden;
            SearchSort_Combobox.Items.Add("По умолчанию");
            SearchSort_Combobox.Items.Add("По релевантности");
            SearchSort_Combobox.SelectedIndex = 0;
            UpdateRequest();
            if (User.UserRoleId == 1)
            {
                AddProduct_Button.Visibility = Visibility.Visible;
                AddSupplier_Button.Visibility = Visibility.Visible;
                SearchFilter_Combobox.Items.Add("Все");
                SearchFilter_Combobox.Items.Add("Не назначенные");
                SearchFilter_Combobox.Items.Add("Назначенные");
                SearchFilter_Combobox.SelectedIndex = 0;
            }
            if(User.UserRoleId == 2)
            {
                SearchFilter_Combobox.Items.Add("Все");
                SearchFilter_Combobox.Items.Add("На рассмотрении");
                SearchFilter_Combobox.Items.Add("Рассмотренные");
                Product_Tabitem.Visibility = Visibility.Hidden;
                Request_Tabitem.IsSelected = true;
                SearchFilter_Combobox.SelectedIndex = 0;
            }
            if (User.UserRoleId == 3)
            {
                SearchFilter_Combobox.Items.Add("Все");
                SearchFilter_Combobox.Items.Add("Не отрпавленные");
                SearchFilter_Combobox.Items.Add("Отправленные");
                Acceptance_TabItem.Visibility = Visibility.Visible;
                SearchFilter_Combobox.SelectedIndex = 0;
            }
            
        }

        private void UpdateProduct()
        {
            Product_ListView.Items.Clear();
            displayProduct = AptContext.aptContext.Products.ToList();
            if(!string.IsNullOrEmpty(Search_TextBox.Text))
            {
                string search = Search_TextBox.Text.Replace(" ", "").ToLower();
                displayProduct = displayProduct.Where(p => p.Name.Replace(" ","").ToLower().Contains(search) || p.Article.Contains(search)).ToList();
            }
            if(SearchSup_Combobox.SelectedIndex > 0)
            {
                displayProduct = displayProduct.Where(p => p.Supplier.Name == SearchSup_Combobox.SelectedItem.ToString()).ToList();
            }
            if (SearchReleaseForm_Combobox.SelectedIndex > 0)
            {
                displayProduct = displayProduct.Where(p => p.ReleaseForm.Name == SearchReleaseForm_Combobox.SelectedItem.ToString()).ToList();
            }
            if(!string.IsNullOrEmpty(CostFrom_TextBox.Text))
            {
                displayProduct = displayProduct.Where(p => p.Cost >= Int32.Parse(CostFrom_TextBox.Text)).ToList();
            }
            if (!string.IsNullOrEmpty(CostTo_TextBox.Text))
            {
                displayProduct = displayProduct.Where(p => p.Cost <= Int32.Parse(CostTo_TextBox.Text)).ToList();
            }
            if(!string.IsNullOrEmpty(AmountInPackage_TextBox.Text))
            {
                displayProduct = displayProduct.Where(p => p.Quantityinthepackage == Int32.Parse(AmountInPackage_TextBox.Text)).ToList();
            }    
            if(!string.IsNullOrEmpty(SearchDose_Textbox.Text))
            {
                displayProduct = displayProduct.Where(p => p.Dose.Replace(" ", "").ToLower().Contains(SearchDose_Textbox.Text.Replace(" ", "").ToLower())).ToList();
            }
            if (displayProduct.Count > 0)
            {
                NotFoundProduct_Label.Visibility = Visibility.Hidden;
                foreach (Product product in displayProduct)
                {
                    Product_ListView.Items.Add(new ProductControl(User, product) { Width = GetNormalWidth()});
                }
            }
            else
            {
                NotFoundProduct_Label.Visibility = Visibility.Visible;
            }
        }

        public static void Upd()
        {
            MW.UpdateRequest();
        }

        private void UpdateRequest()
        {
            displayRequest = AptContext.aptContext.Requests.ToList();
            Request_ListView.Items.Clear();
            switch (SearchSort_Combobox.SelectedIndex)
            {
                case 0:
                    displayRequest = displayRequest.OrderByDescending(r => r.Id).ToList();
                    break;
                case 1:
                    displayRequest = displayRequest.OrderBy(r => r.Id).ToList();
                    break;
            }
            if(User.UserRoleId == 1)
            {
                displayRequest = displayRequest.Where(r => r.StatusId == 2).ToList();
                switch(SearchFilter_Combobox.SelectedIndex)
                {
                    case 1:
                        displayRequest = displayRequest.Where(r => r.User == null).ToList();
                        break;
                    case 2:
                        displayRequest = displayRequest.Where(r => r.User != null).ToList();
                        break;
                }
                if(!string.IsNullOrEmpty(SearchReqPharmacy_TextBox.Text))
                {
                    displayRequest = displayRequest.Where(r => r.PharmacyId.ToString().Contains(SearchReqPharmacy_TextBox.Text)).ToList();
                }
                if (displayRequest.Count > 0)
                {
                    NotFoundRequest_Label.Visibility = Visibility.Hidden;
                    Request_ListView.Items.Clear();
                    foreach (Request request in displayRequest)
                    {
                        Request_ListView.Items.Add(new RequestControl(request, User) { Width = GetNormalWidth() });
                    }
                }
                else
                {
                    NotFoundRequest_Label.Visibility = Visibility.Visible;
                }
            }
            if(User.UserRoleId == 2)
            {
                displayRequest = displayRequest.Where(r => r.User == User).ToList();
                CreateAkt_Button.Visibility = Visibility.Visible;
                switch (SearchFilter_Combobox.SelectedIndex)
                {

                    case 1:
                        displayRequest = displayRequest.Where(r => r.StatusId == 2).ToList();
                        break;
                    case 2:
                        displayRequest = displayRequest.Where(r => r.StatusId == 3).ToList();
                        break;
                }
                if (!string.IsNullOrEmpty(SearchReqPharmacy_TextBox.Text))
                {
                    displayRequest = displayRequest.Where(r => r.PharmacyId.ToString().Contains(SearchReqPharmacy_TextBox.Text)).ToList();
                }
                if (displayRequest.Count > 0)
                {
                    NotFoundRequest_Label.Visibility = Visibility.Hidden;                   
                    foreach (Request request in displayRequest)
                    {
                        Request_ListView.Items.Add(new RequestControl(request, User) { Width = GetNormalWidth() });
                    }
                }
                else
                {
                    NotFoundRequest_Label.Visibility = Visibility.Visible;
                }
            }
            if(User.UserRoleId == 3)
            {
                displayRequest = displayRequest.Where(r => r.PharmacyId == User.PharmacyId).ToList();
                switch (SearchFilter_Combobox.SelectedIndex)
                {
                    case 1:
                        displayRequest = displayRequest.Where(r => r.StatusId == 1).ToList();
                        break;
                    case 2:
                        displayRequest = displayRequest.Where(r => r.StatusId == 2).ToList();
                        break;
                }
                if (displayRequest.Count > 0)
                {
                    NotFoundRequest_Label.Visibility = Visibility.Hidden;
                    foreach (Request request in displayRequest)
                    {
                        Request_ListView.Items.Add(new RequestControl(request, User) { Width = GetNormalWidth() });
                    }
                }
                else
                {
                    NotFoundRequest_Label.Visibility = Visibility.Visible;
                }
               
            }
        }

        private double GetNormalWidth()
        {   if (AdvancedSearsh_Column.Width == new GridLength(0))
            {
                if (WindowState == WindowState.Maximized)
                {
                    return RenderSize.Width - 50;
                }
                else
                {
                    return Width - 50;
                }
            }
            else
            {
                if (WindowState == WindowState.Maximized)
                {
                    return RenderSize.Width - 250;
                }
                else
                {
                    return Width - 250;
                }
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateSizeProdListViewItem();
            UpdateSizeRequestListViewItem();
        }

        private void Exit_Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            AddOrChangeProd_Button.Content = "Добавить";
            AddOrEditProd_Grid.DataContext = null;
            RemoveProduct_Button.Visibility = Visibility.Hidden;
            UpdateReleaseForm_Combobox();
            UpdateSupplier_Comboboxes();

        }

        private void BackToProd_Button_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditProd_Grid.Visibility = Visibility.Hidden;
            AddOrEditSupplier_Grid.Visibility = Visibility.Hidden;
            ProdView_Grid.Visibility = Visibility.Visible;

        }

        private void AddOrChangeProd_Button_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(ProdArticle_Textbox.Text) && !string.IsNullOrEmpty(ProdName_TextBox.Text) &&
                !string.IsNullOrEmpty(ProdQuantityInPackage_Textbox.Text) && !string.IsNullOrEmpty(ProdCost_TextBox.Text))
            {
                if (AddOrChangeProd_Button.Content.ToString() == "Добавить")
                {
                    Product product = new Product();
                    product.Article = ProdArticle_Textbox.Text;
                    product.Name = ProdName_TextBox.Text;
                    product.Dose = ProdDose_TextBox.Text;
                    product.Quantityinthepackage = Int32.Parse(ProdQuantityInPackage_Textbox.Text);
                    product.ReleaseForm = (Releaseform)ProdReleaseForm_Combobox.SelectedItem;
                    product.Supplier = (Supplier)ProdSupplier_Combobox.SelectedItem;
                    product.Cost = Int32.Parse(ProdCost_TextBox.Text);
                    AptContext.aptContext.Add(product);
                    AptContext.aptContext.SaveChanges();
                    MessageBox.Show("Товар успешно добавлен", "Уведомление");
                    BackToProd_Button_Click(sender, e);
                    UpdateProduct();
                }
                else
                {
                    AptContext.aptContext.SaveChanges();
                    UpdateProduct();
                    BackToProd_Button_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Уведомление");
            }
        }

        private void Product_ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (currentProduct != null && User.UserRoleId == 1)
            {
                UpdateReleaseForm_Combobox();
                UpdateSupplier_Comboboxes();
                AddOrEditProd_Grid.DataContext = currentProduct;
                AddOrChangeProd_Button.Content = "Изменить";
                RemoveProduct_Button.Visibility = Visibility.Visible;               
                ProdReleaseForm_Combobox.SelectedItem = currentProduct.ReleaseForm;
                ProdSupplier_Combobox.SelectedItem = currentProduct.Supplier;
                
            }
        }

        private void Product_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Product_ListView.SelectedItem != null)
            {
                currentProduct = ((ProductControl)Product_ListView.SelectedItem).Product;
            }
            
        }

        private void RemoveProduct_Button_Click(object sender, RoutedEventArgs e)
        {
            if (AptContext.aptContext.Products.Contains(currentProduct))
            {
                Pharmacyproduct pharmprod = AptContext.aptContext.Pharmacyproducts.FirstOrDefault(p => p.ProductId == currentProduct.Id);
                if (pharmprod == null)
                {
                    AptContext.aptContext.Remove(currentProduct);
                    AptContext.aptContext.SaveChanges();
                    UpdateProduct();
                    BackToProd_Button_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Товар содержится в Аптеках", "Уведомление");
                }
            }
        }
        
        private void UpdateReleaseForm_Combobox()
        {
            ProdReleaseForm_Combobox.Items.Clear();           
            AddOrEditProd_Grid.Visibility = Visibility.Visible;
            ProdView_Grid.Visibility = Visibility.Hidden;
            foreach (Releaseform releaseform in AptContext.aptContext.Releaseforms.ToList())
            {
                ProdReleaseForm_Combobox.Items.Add(releaseform);
            }
            ProdReleaseForm_Combobox.SelectedIndex = 0;
        }

        private void AddSupplier_Button_Click(object sender, RoutedEventArgs e)
        {
            UpdateSupCountry_Combobox();
            if (SupEdit_RadioButton.IsChecked == false)
            {
                RemoveSupplier_Button.Visibility = Visibility.Hidden;
            }
            AddOrEditProd_Grid.Visibility = Visibility.Hidden;
            ProdView_Grid.Visibility = Visibility.Hidden;
            AddOrEditSupplier_Grid.Visibility = Visibility.Visible;
        }

        private void Supp_EditRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if(SupAdd_RadioButton.IsChecked == true)
            {
                Supplier_Combobox.Visibility = Visibility.Hidden;
                RemoveSupplier_Button.Visibility = Visibility.Hidden;
                AddOrEditSupplier_Button.Content = "Добавить";
                Supplier supplier = new Supplier();
                AddOrEditSupplier_Grid.DataContext = supplier;
                SupCountry_Combobox.SelectedIndex = 0;
            }
            else
            {

                UpdateSupplier_Comboboxes();
                Supplier_Combobox.Visibility = Visibility.Visible;
                RemoveSupplier_Button.Visibility = Visibility.Visible;
                AddOrEditSupplier_Button.Content = "Изменить";       
                
            }
        }      

        private void UpdateSupCountry_Combobox()
        {
            SupCountry_Combobox.Items.Clear();
            foreach (Country country in AptContext.aptContext.Countries.ToList())
            {
                SupCountry_Combobox.Items.Add(country);
            }
            SupCountry_Combobox.SelectedIndex = 0;
        }
        private void UpdateSupplier_Comboboxes()
        {
            ProdSupplier_Combobox.Items.Clear();
            foreach (Supplier supplier in AptContext.aptContext.Suppliers.ToList())
            {
                ProdSupplier_Combobox.Items.Add(supplier);
            }
            ProdSupplier_Combobox.SelectedIndex = 0;
            Supplier_Combobox.Items.Clear();
            foreach (Supplier supplier in AptContext.aptContext.Suppliers.ToList())
            {
                Supplier_Combobox.Items.Add(supplier);
            }
            Supplier_Combobox.SelectedIndex = 0;
            SearchSup_Combobox.Items.Clear();
            SearchSup_Combobox.Items.Add("Все");
            foreach(Supplier supplier in AptContext.aptContext.Suppliers.ToList())
            {
                SearchSup_Combobox.Items.Add(supplier.Name);
            }
            SearchSup_Combobox.SelectedIndex = 0;

        }

        private void AddOrEditSupplier_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SupName_TextBox.Text) && !string.IsNullOrEmpty(SupINN_TextBox.Text))
            {

                if(AddOrEditSupplier_Button.Content.ToString() == "Добавить")
                {
                    Supplier supplier = new Supplier();
                    supplier.Country = (Country)SupCountry_Combobox.SelectedItem;
                    supplier.Name = SupName_TextBox.Text;
                    supplier.Inn = Int32.Parse(SupINN_TextBox.Text);
                    AptContext.aptContext.Add(supplier);
                    AptContext.aptContext.SaveChanges();
                    AddOrEditSupplier_Grid.DataContext = null;
                    UpdateSupplier_Comboboxes();
                    MessageBox.Show("Поставщик успешно добавлен", "Уведомление");
                }
                else
                {
                    AptContext.aptContext.SaveChanges();
                    UpdateProduct();
                    UpdateSupplier_Comboboxes();
                    MessageBox.Show("Поставщик успешно изменён", "Уведомление");
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Уведомление");
            }
        }

        private void RemoveSupplier_Button_Click(object sender, RoutedEventArgs e)
        {
            Supplier supplier = (Supplier)Supplier_Combobox.SelectedItem;
            Product product = AptContext.aptContext.Products.FirstOrDefault(p => p.Supplier == supplier );
            if (product == null)
            {
                AptContext.aptContext.Remove(supplier);
                AptContext.aptContext.SaveChanges();
                UpdateSupplier_Comboboxes();
                MessageBox.Show("Поставщик успешно удалён", "Уведомление");
            }
            else
            {
                MessageBox.Show("У поставщика есть товары", "Уведомление");
            }

        }

        private void Supplier_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Supplier supplier = (Supplier)Supplier_Combobox.SelectedItem;
            if(SupEdit_RadioButton.IsChecked == true)
            {
                AddOrEditSupplier_Grid.DataContext = supplier;
                if (supplier != null)             
                    SupCountry_Combobox.SelectedItem = supplier.Country;
            }
            
        }

        private void AdvancedSearch_Button_Click(object sender, RoutedEventArgs e)
        {
            AdvancedSearsh_Column.Width = new GridLength(200);
            AdvancedSearch_Button.Visibility = Visibility.Hidden;
            UpdateSizeProdListViewItem();
            UpdateSizeRequestListViewItem();

        }

        private void UpdateSizeRequestListViewItem()
        {
            foreach (RequestControl item in Request_ListView.Items)
            {
                item.Width = GetNormalWidth();
            }
        }

        private void UpdateSizeProdListViewItem()
        {
            foreach (ProductControl item in Product_ListView.Items)
            {
                item.Width = GetNormalWidth();
            }
        }

        private void HideAdvSearch_Button_Click(object sender, RoutedEventArgs e)
        {
            AdvancedSearsh_Column.Width = new GridLength(0);
            AdvancedSearch_Button.Visibility = Visibility.Visible;
            if(Product_Tabitem.IsSelected == true)
            {
                UpdateSizeProdListViewItem();
            }
            if(Request_Tabitem.IsSelected == true)
            {
                UpdateSizeRequestListViewItem();
            }
        }

        private void Search_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void SearchSup_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void CostFrom_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void CostTo_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void AmountInPackage_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void SearchDose_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProduct();
        }

        private void ResetSearch_Button_Click(object sender, RoutedEventArgs e)
        {
            SearchSup_Combobox.SelectedIndex = 0;
            SearchReleaseForm_Combobox.SelectedIndex = 0;
            CostFrom_TextBox.Text = default;
            CostTo_TextBox.Text = default;
            AmountInPackage_TextBox.Text = default;
            ProdDose_TextBox.Text = default;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Product_Tabitem.IsSelected == true) 
            {
                SearchReq_StackPanel.Visibility = Visibility.Hidden;
                SearhProd_StackPanel.Visibility = Visibility.Visible;               
            }
            if(Request_Tabitem.IsSelected == true)
            {
                SearchReq_StackPanel.Visibility = Visibility.Visible;
                SearhProd_StackPanel.Visibility = Visibility.Hidden;
                if (User.UserRoleId != 3)
                {
                    SearchReqLogist_StackPanel.Visibility = Visibility.Visible;
                }
            }
            UpdateSizeProdListViewItem();
            UpdateSizeRequestListViewItem();
        }

        private void SearchSort_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRequest();
        }

        private void SearchFilter_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRequest();
        }

        private void SearchReqPharmacy_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRequest();
        }
    }
}
