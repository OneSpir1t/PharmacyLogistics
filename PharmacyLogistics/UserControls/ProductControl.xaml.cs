using Castle.Core.Internal;
using PharmacyLogistics.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
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

namespace PharmacyLogistics.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        public Product Product { get; set; }
        private User User { get; set; }
        private DateOnly date { get; set; }
        public ProductControl(User user, Product product)
        {
            InitializeComponent();
            Product = product;
            User = user;
            int year = Int32.Parse(DateTime.Now.ToString("yyyy"));
            int month = Int32.Parse(DateTime.Now.ToString("MM"));
            int day = Int32.Parse(DateTime.Now.ToString("dd"));
            if (user.UserRoleId == 3)
            {
                ProdDose_Label.Visibility = Visibility.Visible;
                HasProd_Grid.Visibility = Visibility.Visible;
                AddProdToReq_Button.Visibility = Visibility.Visible;
                Pharmacy pharmacy = AptContext.aptContext.Pharmacies.FirstOrDefault(p => p.Id == user.PharmacyId);
                List<Pharmacyproduct> pharmacyproducts = new List<Pharmacyproduct>();
                HasProd_StackPanel.Visibility = Visibility.Visible;
                foreach (var PharmProd in AptContext.aptContext.Pharmacyproducts.ToList())
                {
                    
                    if (PharmProd.PharmacyId == pharmacy.Id && PharmProd.ProductId == product.Id)
                    {
                        pharmacyproducts.Add(PharmProd);
                    }
                }
                int sum = 0;
                if(pharmacyproducts.Count > 0)
                {                   
                    date = new DateOnly(year, month, day);
                    for (int i = 0; i < pharmacyproducts.Count; i++)
                    {
                        sum += pharmacyproducts[i].Amount;

                    }                   
                    foreach(var Product in pharmacyproducts)
                    {
                        if (Product.Expirydate < date)
                        {
                            ProdInStock_ListBox.Items.Add(new ListBoxItem() { Content = $"Годен до: {Product.Expirydate} В количестве: {Product.Amount} ", Background = Brushes.Red });
                        }
                        else
                        {
                            ProdInStock_ListBox.Items.Add("Годен до: " + Product.Expirydate + " В количестве: " + Product.Amount);
                        }
                        
                    }
                   
                    if(sum > 9)
                    {
                        HasProd_Border.Background = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    }
                    if(sum < 5)
                    {
                        HasProd_Border.Background = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                    }
                }
                else
                {
                    HasProd_Border.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    HasProd_Grid.Visibility = Visibility.Hidden;
                    HasProd_Grid.Height = 0;
                }
                ProdAmount_Label.Content = sum;
            }
            else
            {
                HasProd_Grid.Height = 0;
            }
            if (!string.IsNullOrEmpty(product.Dose))
            {
                ProdDose_Label.Content = "Дозировка: " + product.Dose;
            }
            ProdArticle_Label.Content = product.Article;
            ProdName_Label.Content = product.Name;
            ProdSupplier_Label.Content = product.Supplier.Name;
            ProdReleaseForm_Label.Content = product.ReleaseForm.Name;
            ProdCost_Label.Content = product.Cost;
            ProdQuantityInPackage_Label.Content = "В упаковке: " + product.Quantityinthepackage + " шт.";


        }

        private void ShowAllProd_Button_Click(object sender, RoutedEventArgs e)
        {
            if(ShowAllProd_Button.Content.ToString() == "Показать")
            {
                ProdInStock_ListBox.Height = 100;
                ShowAllProd_Button.Content = "Скрыть";
            }
            else
            {
                ShowAllProd_Button.Content = "Показать";
                ProdInStock_ListBox.Height = 0;
            }
        }

        private void AddProdToReq_Button_Click(object sender, RoutedEventArgs e)
        {
            Request request1 = AptContext.aptContext.Requests.FirstOrDefault(r => r.PharmacyId == User.Pharmacy.Id && r.StatusId == 1);
            if (request1 != null)
            {
                Requestproduct requestproduct1 = AptContext.aptContext.Requestproducts.FirstOrDefault(r => r.Product == Product && r.Request == request1);
                if(requestproduct1 == null)
                {
                    Requestproduct requestproduct = new Requestproduct();
                    requestproduct.Product = Product;
                    requestproduct.Request = request1;
                    requestproduct.Amount = 1;
                    AptContext.aptContext.Add(requestproduct);
                    AptContext.aptContext.SaveChanges();
                    MessageBox.Show("Товар добавлен в заявку", "Уведомление");
                    MainWindow.Upd();
                }
                else
                {
                    MessageBox.Show("Товар уже есть в заявке", "Уведомление");
                }

            }
            else
            {
                var msg = MessageBox.Show("У вас нет открытой заявки, хотите открыть новую?", "Уведомление", MessageBoxButton.YesNo);
                if(msg == MessageBoxResult.Yes)
                {
                    Request request = new Request();
                    request.Pharmacy = User.Pharmacy;
                    request.DateOfRequest = date;
                    Status status = AptContext.aptContext.Statuses.FirstOrDefault(s => s.Id == 1);
                    request.Status = status;
                    Requestproduct requestproduct = new Requestproduct();
                    requestproduct.Product = Product;
                    requestproduct.Request = request;
                    requestproduct.Amount = 1;
                    AptContext.aptContext.Add(request);
                    AptContext.aptContext.Add(requestproduct);
                    AptContext.aptContext.SaveChanges();
                    MessageBox.Show("Товар успешно добавлен в заявку");
                    MainWindow.Upd();
                    
                }
            }
        }
    }
}
