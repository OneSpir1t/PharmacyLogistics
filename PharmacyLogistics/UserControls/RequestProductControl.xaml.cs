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
        bool flag = true;
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
            if (request.StatusId == 2 || request.StatusId > 3 && request.User != null)
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
            int a;
            if (Amount_TextBox.Text.Length > 0)
            {
                a = Int32.Parse(Amount_TextBox.Text);
            }
            else 
            {
                a = 1;
            }
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
                        UserWindow.Upd();


                    }
                }
            }
        }

        private void PlusAmount_Button_Click(object sender, RoutedEventArgs e)
        {
            int a;
            if(Amount_TextBox.Text.Length > 0)
            {
                
                a = Int32.Parse(Amount_TextBox.Text);
            }
            else
            {
                a = 0;
            }
            if (a < 99)
            {
                a = a + 1;
                Amount_TextBox.Text = a.ToString();
            }

        }

        private void Amount_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Amount_TextBox.Text.Length > 0)
            {
                bool ckeck = Int32.TryParse(Amount_TextBox.Text, out int num);
                if (ckeck && Amount_TextBox.Text != "0" && Amount_TextBox.Text != "00")
                {
                    Requestproduct.Amount = Int32.Parse(Amount_TextBox.Text);
                }
                else
                {
                    Amount_TextBox.Text = default;
                }
                AptContext.aptContext.SaveChanges();
            }
            
        }

        private void Amount_TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(Amount_TextBox.Text.Length == 0 &&  e.Text == "0")
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
