using PharmacyLogistics.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для RequestProductControl.xaml
    /// </summary>
    public partial class RequestProductControl : UserControl
    {
        Requestproduct Requestproduct { get; set; }
        private static RequestProductControl RPC { get; set; }
        public RequestProductControl(Requestproduct requestproduct)
        {
            RPC = this;
            InitializeComponent();
            Requestproduct = requestproduct;
            UpdateReqProd();
        }

        private void UpdateReqProd()
        {
            ReqProduct_Label.Content = string.Join(" ", Requestproduct.Product.Article, Requestproduct.Product.Name,
            Requestproduct.Product.ReleaseForm.Name, Requestproduct.Product.Dose, Requestproduct.Product.Quantityinthepackage + " шт.");
            Request request = AptContext.aptContext.Requests.FirstOrDefault(r => r.Id == Requestproduct.RequestId);
            if (request.StatusId > 3)
            {
                MinusAmount_Button.Visibility = Visibility.Hidden;
                PlusAmount_Button.Visibility = Visibility.Hidden;
                Amount_TextBox.IsEnabled = false;
            }
            Amount_TextBox.Text = Requestproduct.Amount.ToString();
        }

        public static void Upd()
        {
            RPC.UpdateReqProd();
        }

        private void MinusAmount_Button_Click(object sender, RoutedEventArgs e)
        {
            int a = Int32.Parse(Amount_TextBox.Text);
            if (a > 1)
            {
                a = a - 1;
                Amount_TextBox.Text = a.ToString();                
            }
            else
            {
                
                int id = Requestproduct.RequestId;
                Request request = AptContext.aptContext.Requests.FirstOrDefault (r => r.Id == id);
                if(request.User == null)
                {
                    var msg = MessageBox.Show("Вы дейтсвилеьно хотите удалить товар?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (msg == MessageBoxResult.Yes)
                    {
                        AptContext.aptContext.Remove(Requestproduct);                   
                        AptContext.aptContext.SaveChanges();
                        Requestproduct requestproduct = AptContext.aptContext.Requestproducts.FirstOrDefault(rp => rp.RequestId == id);
                        MainWindow.Upd();


                    }
                }
            }
        }

        private void PlusAmount_Button_Click(object sender, RoutedEventArgs e)
        {
            int a = Int32.Parse(Amount_TextBox.Text);
            if (a < 99)
            {
                a = a + 1;
                Amount_TextBox.Text = a.ToString();
            }

        }

        private void Amount_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Requestproduct.Amount = Int32.Parse(Amount_TextBox.Text);
            AptContext.aptContext.SaveChanges();
        }
    }
}
