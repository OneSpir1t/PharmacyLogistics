using Castle.Core.Internal;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using PharmacyLogistics.Entities;
using PharmacyLogistics.UserControls;
using System;
using System.IO;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WinForms = System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Diagnostics;
namespace PharmacyLogistics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    { 
        private User User { get; set; }
        private List<Product> displayProduct { get; set; }
        private List<Request> displayRequest { get; set; }
        private Product currentProduct { get; set; }
        private static UserWindow MW { get; set; }
        public UserWindow(User user)
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
            foreach(Manufacturer manufacturer in AptContext.aptContext.Manufacturers.ToList())
            {
                Manufacturer_Combobox.Items.Add(manufacturer);
            }
            Manufacturer_Combobox.SelectedIndex = 0;
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
            }
            if(User.UserRoleId == 2)
            {
                displayRequest = displayRequest.Where(r => r.User == User && r.StatusId < 5).ToList();
                CreateAkt_Button.Visibility = Visibility.Visible;
                switch (SearchFilter_Combobox.SelectedIndex)
                {

                    case 1:
                        displayRequest = displayRequest.Where(r => r.StatusId == 3).ToList();
                        break;
                    case 2:
                        displayRequest = displayRequest.Where(r => r.StatusId == 4).ToList();
                        break;
                }
                if (!string.IsNullOrEmpty(SearchReqPharmacy_TextBox.Text))
                {
                    displayRequest = displayRequest.Where(r => r.PharmacyId.ToString().Contains(SearchReqPharmacy_TextBox.Text)).ToList();
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
                    product.Manufacturer = (Manufacturer)Manufacturer_Combobox.SelectedItem;
                    product.Cost = Int32.Parse(ProdCost_TextBox.Text);
                    AptContext.aptContext.Add(product);
                    AptContext.aptContext.SaveChanges();
                    MessageBox.Show("Товар успешно добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    BackToProd_Button_Click(sender, e);
                    UpdateProduct();
                }
                else
                {
                    MessageBox.Show("Товар успешно изменён", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    currentProduct.Supplier = (Supplier)ProdSupplier_Combobox.SelectedItem;
                    currentProduct.ReleaseForm = (Releaseform)ProdReleaseForm_Combobox.SelectedItem;
                    currentProduct.Manufacturer = (Manufacturer)Manufacturer_Combobox.SelectedItem;
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
                Manufacturer_Combobox.SelectedItem = currentProduct.Manufacturer;
                
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
                Requestproduct requestproduct = AptContext.aptContext.Requestproducts.FirstOrDefault(p => p.ProductId == currentProduct.Id);
                if (pharmprod == null && requestproduct == null)
                {
                    AptContext.aptContext.Remove(currentProduct);
                    AptContext.aptContext.SaveChanges();
                    UpdateProduct();
                    BackToProd_Button_Click(sender, e);
                }
                else
                {
                    MessageBox.Show("Товар содержится в аптеках или заявках", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show("Поставщик успешно добавлен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    AptContext.aptContext.SaveChanges();
                    UpdateProduct();
                    UpdateSupplier_Comboboxes();
                    MessageBox.Show("Поставщик успешно изменён", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RemoveSupplier_Button_Click(object sender, RoutedEventArgs e)
        {
            Supplier supplier = (Supplier)Supplier_Combobox.SelectedItem;
            Product product = AptContext.aptContext.Products.FirstOrDefault(p => p.Supplier == supplier );
            if (product == null)
            {
                var msg = MessageBox.Show("Вы действительно хотите удалить поставщика?", "Уведомление", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (msg == MessageBoxResult.Yes)
                {
                    AptContext.aptContext.Remove(supplier);
                    AptContext.aptContext.SaveChanges();
                    UpdateSupplier_Comboboxes();
                    MessageBox.Show("Поставщик успешно удалён", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("У поставщика есть товары", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            if (CostFrom_TextBox.Text.Length > 0)
            {
                bool ckeck = Int32.TryParse(CostFrom_TextBox.Text, out int num);
                if (!ckeck || CostFrom_TextBox.Text == "0" || CostFrom_TextBox.Text == "00")
                {
                    CostFrom_TextBox.Text = "";
                }
            }
            UpdateProduct();
        }

        private void CostTo_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CostTo_TextBox.Text.Length > 0)
            {
                bool ckeck = Int32.TryParse(CostTo_TextBox.Text, out int num);
                if (!ckeck || CostTo_TextBox.Text == "0" || CostTo_TextBox.Text == "00")
                {
                    CostTo_TextBox.Text = "";
                }
            }
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

        private void CreateAkt_Button_Click(object sender, RoutedEventArgs e)
        {                        
            WinForms.FolderBrowserDialog fbd = new WinForms.FolderBrowserDialog();
            List<Request> requests = new List<Request>();
            requests = RequestControl.AktReq;
            if (RequestControl.AktReq.Count() > 0)
            {                
                if (fbd.ShowDialog() == WinForms.DialogResult.OK)
                {
                    string path = fbd.SelectedPath + "/";                                       
                    foreach (Supplier supplier in AptContext.aptContext.Suppliers.ToList())
                    {
                        List<Requestproduct> requestproduct = new List<Requestproduct>();
                        for(int i = 0; i < requests.Count; i++)
                        {
                            for(int j = 0; j < requests[i].Requestproducts.Count; j++)
                            {
                                if (supplier == requests[i].Requestproducts.ElementAt(j).Product.Supplier)
                                {
                                    bool flag = true;
                                    Request request = AptContext.aptContext.Requests.FirstOrDefault(r => r.Id == requests[i].Id);
                                    request.Status = AptContext.aptContext.Statuses.FirstOrDefault(s => s.Id == 4);
                                    AptContext.aptContext.SaveChanges();
                                    requestproduct.Add(requests[i].Requestproducts.ElementAt(j));
                                }                               
                            }
                        }
                        for (int i = 0; i < requestproduct.Count; i++)
                        {   
                            for (int j = 1; j < requestproduct.Count; j++)
                            {
                                if (requestproduct[i].Product == requestproduct[j].Product && i != j)
                                {
                                    requestproduct[i].Amount = requestproduct[i].Amount + requestproduct[j].Amount;
                                    requestproduct.Remove(requestproduct[j]);
                                    i = 0; j = 0;
                                }                               
                            }                                 
                        }
                        if(requestproduct.Count > 0)
                        {
                            CreateDocument(requestproduct, path);
                        }
                    }                
                }
            }
            UpdateRequest();
        }

        private void CreateDocument(List<Requestproduct> requestproducts, string path)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string[] files = Directory.GetFiles(path);
            int number = 0;
            if (files.Length > 0)
            {
                for (int i = 0; files.Length > i; i++)
                {
                    number++;
                }
            }
            else
            {
                number = 1;
            }
            string filename = Path.GetRandomFileName() + ".pdf";
            string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
            var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            var font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
            Document doc = new Document(PageSize.LETTER, 40f, 40f, 60f, 60f);
            PdfWriter.GetInstance(doc, new FileStream(path + filename, FileMode.Create));
            doc.Open();
            Paragraph p1 = new Paragraph($"Утверждаю", font);
            p1.Alignment = Element.ALIGN_RIGHT;
            doc.Add(p1);
            User user = AptContext.aptContext.Users.FirstOrDefault(u => u.UserRoleId == 1);
            Paragraph p2 = new Paragraph($"{string.Join(" ", user.Surname, user.Name, user.Patryonomic)}", font);
            p2.Alignment = Element.ALIGN_RIGHT;
            doc.Add(p2);
            Paragraph p3 = new Paragraph($"Акт приёма-передачи №{number}", font);
            p3.Alignment = Element.ALIGN_CENTER;
            doc.Add(p3);
            int year = Int32.Parse(DateTime.Now.ToString("yyyy"));
            int month = Int32.Parse(DateTime.Now.ToString("MM"));
            int day = Int32.Parse(DateTime.Now.ToString("dd"));
            Paragraph p4 = new Paragraph($"от {new DateOnly(year, month, day)}", font);
            p4.Alignment = Element.ALIGN_CENTER;
            doc.Add(p4);
            Supplier supplier = AptContext.aptContext.Suppliers.FirstOrDefault(s => s.Id == requestproducts[0].Product.SupplierId);
            Paragraph p5 = new Paragraph($"Поставщик: {supplier.Name}", font);
            p5.Alignment = Element.ALIGN_LEFT;
            doc.Add(p5);
            Paragraph p6 = new Paragraph($"ИНН: {supplier.Inn}", font);
            p6.Alignment = Element.ALIGN_LEFT;
            doc.Add(p6);
            PdfPTable table = new PdfPTable(5);
            table.SpacingBefore = 10f;
            table.SpacingAfter = 10f;
            table.AddCell(new PdfPCell(new Phrase("№ п/п", font)));
            table.AddCell(new PdfPCell(new Phrase("Наименование товара", font)));
            table.AddCell(new PdfPCell(new Phrase("Количество товара", font)));
            table.AddCell(new PdfPCell(new Phrase("Цена за единицу", font)));
            table.AddCell(new PdfPCell(new Phrase("Стоимость товара", font)));
            int sum = 0;
            int count = 0;
            for (int i = 0; i <= requestproducts.Count - 1; i++)
            {
                table.AddCell(new PdfPCell(new Phrase($"{i+1}", font)));
                table.AddCell(new PdfPCell(new Phrase($"{requestproducts[i].Product.Name}", font)));
                table.AddCell(new PdfPCell(new Phrase($"{requestproducts[i].Amount}", font)));
                table.AddCell(new PdfPCell(new Phrase($"{requestproducts[i].Product.Cost}", font)));
                table.AddCell(new PdfPCell(new Phrase($"{requestproducts[i].Amount * requestproducts[i].Product.Cost}", font)));
                sum += requestproducts[i].Amount * requestproducts[i].Product.Cost;
                count += requestproducts[i].Amount;

            }
            table.AddCell(new PdfPCell(new Phrase("", font)));
            table.AddCell(new PdfPCell(new Phrase("", font)));
            table.AddCell(new PdfPCell(new Phrase("Общее кол-во", font)));
            table.AddCell(new PdfPCell(new Phrase("", font)));
            table.AddCell(new PdfPCell(new Phrase("Итого:", font)));
            table.AddCell(new PdfPCell(new Phrase("", font)));
            table.AddCell(new PdfPCell(new Phrase("", font)));
            table.AddCell(new PdfPCell(new Phrase($"{count}", font)));
            table.AddCell(new PdfPCell(new Phrase("", font)));
            table.AddCell(new PdfPCell(new Phrase($"{sum}", font)));
            doc.Add(table);
            Paragraph p7 = new Paragraph($"Подпись поставщика:____________                                  Подпись управляющего:____________", font);
            p7.Alignment = Element.ALIGN_CENTER;
            doc.Add(p7);
            doc.Close();                               
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });      
            while(File.Exists(path + filename))
            {
                break;
            }
        }

        private void ProdSupplier_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ProdReleaseForm_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchReleaseForm_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProduct();            
        }

        private void AddUser_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProdArticle_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
        }

        private void ProdQuantityInPackage_Textbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (ProdQuantityInPackage_Textbox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }
        private void ProdQuantityInPackage_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ProdQuantityInPackage_Textbox.Text.Length > 0)
            {
                bool ckeck = Int32.TryParse(ProdCost_TextBox.Text, out int num);
                if (!ckeck || ProdQuantityInPackage_Textbox.Text == "0" || ProdQuantityInPackage_Textbox.Text == "00")
                {
                    ProdQuantityInPackage_Textbox.Text = "";
                }
            }
        }

        private void ProdCost_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (ProdCost_TextBox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }

        private void ProdCost_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ProdCost_TextBox.Text.Length > 0)
            {
                bool ckeck = Int32.TryParse(ProdCost_TextBox.Text, out int num);
                if (!ckeck || ProdCost_TextBox.Text == "0" || ProdCost_TextBox.Text == "00")
                {
                    ProdCost_TextBox.Text = "";
                }
            }
        }


        private void SupINN_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SupINN_TextBox.Text.Length > 0)
            {
                bool ckeck = Int32.TryParse(SupINN_TextBox.Text, out int num);
                if (!ckeck || SupINN_TextBox.Text == "0" || SupINN_TextBox.Text == "00")
                {
                    SupINN_TextBox.Text = "";
                }
            }
        }

        private void SupINN_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (SupINN_TextBox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }

        private void CostFrom_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (CostFrom_TextBox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }

        private void CostTo_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (CostTo_TextBox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }

        private void AmountInPackage_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (AmountInPackage_TextBox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }

        private void SearchReqPharmacy_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (SearchReqPharmacy_TextBox.Text.Length == 0 && e.Text == "0")
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = "0123456789".IndexOf(e.Text) < 0;
            }
        }
    }
}
