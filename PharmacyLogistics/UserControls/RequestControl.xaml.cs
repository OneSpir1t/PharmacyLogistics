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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PharmacyLogistics.UserControls
{
    /// <summary>
    /// Логика взаимодействия для RequestControl.xaml
    /// </summary>
    public partial class RequestControl : UserControl
    {
        public Request Request { get; set; }
        private List<Requestproduct> displayRequestProducts { get; set; }
        private static RequestControl RC { get; set; }
        private User User { get; set; }
        public static List<Request> AktReq { get; set; } = new List<Request>();
        public RequestControl(Request request, User user)
        {
            InitializeComponent();
            Request = request;
            User = user;
            RC = this;
            AktReq = new List<Request> { };
            if (user.UserRoleId == 1)
            {
                
                ReqId_Label.HorizontalAlignment = HorizontalAlignment.Center;
                ReqStatus_Label.HorizontalAlignment = HorizontalAlignment.Center;
                PhReq_Border.Height = 200;
                Admin_Stackpanel.Height = 120;
                PharmacyManager_Stackpanel.Height = 0;
                ShowReqProd_Button.Visibility = Visibility.Hidden;
                ReqSend_Button.Visibility = Visibility.Hidden;
                User user1 = new User();
                user1.Name = "Не назначен";
                ReqManager_Combobox.Items.Add(user1);
                foreach(User item in AptContext.aptContext.Users.Where(u => u.UserRoleId == 2).ToList())
                {
                    ReqManager_Combobox.Items.Add(item);
                }
                ReqManager_Combobox.SelectedIndex = 0;
            }
            if(user.UserRoleId != 2) 
            {
                AddToAkt_Checkbox.Height = 0;
            }
            UpdateReqProduct();
        }       

        private void UpdateReqProduct()
        {         
            
            if(User.UserRoleId == 3)
            {
                ReqId_Label.Content = "Заявка № " + Request.Id;
            }
            else
            {
                if(Request.User != null)
                {
                    ReqManager_Combobox.SelectedItem = Request.User;
                }
                ReqId_Label.Content = "Заявка № " + Request.Id + " от аптеки № " + Request.PharmacyId;
            }                         
            ReqStatus_Label.Content = "Статус: " + Request.Status.Name;

            if (Request.StatusId != 1)
            {
                ReqSend_Button.Visibility = Visibility.Hidden;
            }
            if (Request.StatusId == 4)
            {
                AddToAkt_Checkbox.Height = 0;
            }
            ReqProduct_ListView.Items.Clear();
            displayRequestProducts = AptContext.aptContext.Requestproducts.Where(rp => rp.Request == Request).ToList();
            foreach (Requestproduct requestproduct in displayRequestProducts)
            {
                ReqProduct_ListView.Items.Add(new RequestProductControl(requestproduct) {Width = 700});
            }
            if (displayRequestProducts.Count == 0)
            {
                ShowReqProd_Button.IsEnabled = false;
                ProductView_Grid.Height = 300;
                ShowReqProd_Button.Content = "Показать товары";
                ProductView_Grid.Height = 0;
                PhReq_Border.Height = 200;
            }
            else
            {
                ShowReqProd_Button.IsEnabled = true;
            }
        }

        private void GetNormalWidth()
        {
            
        }

        private void ShowReqProd_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ShowReqProd_Button.Content.ToString() == "Показать товары")
            {
                ProductView_Grid.Height = 300;
                PhReq_Border.Height = 500;
                ShowReqProd_Button.Content = "Скрыть";
            }
            else
            {
                ShowReqProd_Button.Content = "Показать товары";
                ProductView_Grid.Height = 0;
                PhReq_Border.Height = 200;
            }
            
        }

        private void ReqSend_Button_Click(object sender, RoutedEventArgs e)
        {
            Requestproduct requestproduct = AptContext.aptContext.Requestproducts.FirstOrDefault(rp => rp.Request == Request);
            if (requestproduct != null)
            {
                int year = Int32.Parse(DateTime.Now.ToString("yyyy"));
                int month = Int32.Parse(DateTime.Now.ToString("MM"));
                int day = Int32.Parse(DateTime.Now.ToString("dd"));
                Request.Status = AptContext.aptContext.Statuses.FirstOrDefault(s => s.Id == 2);
                Request.DateOfRequest = new DateOnly(year, month, day);
                AptContext.aptContext.SaveChanges();
                UpdateReqProduct();
            }
            else
            {
                MessageBox.Show("В заявке нет товаров!", "Увдедолмение");
            }
        }

        private void ReqAddManager_Button_Click(object sender, RoutedEventArgs e)
        {
            if(ReqManager_Combobox.SelectedIndex > 0)
            {
                Request.User = (User)ReqManager_Combobox.SelectedItem;
                Request.Status = AptContext.aptContext.Statuses.FirstOrDefault(s => s.Id == 3);
                AptContext.aptContext.SaveChanges();
                MessageBox.Show("Менеджер назначен", "Уведомление");
            }
            else
            {
                Request.User = null;
                AptContext.aptContext.SaveChanges();
                MessageBox.Show("Менеджер не назначен", "Уведомление");
            }
        }

        private void AddToAkt_Checkbox_Click(object sender, RoutedEventArgs e)
        {
            if (AddToAkt_Checkbox.IsChecked == true)
            {
                AktReq.Add(Request);
            }
            else
            {
                AktReq.Remove(Request);
            }
        }
    }
}
