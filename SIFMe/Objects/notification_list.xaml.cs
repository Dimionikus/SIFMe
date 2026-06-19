using SIFMe.DBConnector;
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

namespace SIFMe.Objects
{
    /// <summary>
    /// Логика взаимодействия для notification_list.xaml
    /// </summary>
    public partial class notification_list : UserControl
    {
        public notification_list()
        {
            InitializeComponent();
        }
        List<notification> _notifications = new List<notification>();
        public List<notification> notifications { get => _notifications; set
            {
                _notifications = value;
                if (_notifications.Count <= 0) { OpenCloseBtn.Visibility = Visibility.Collapsed; isOpened = true; OpenClose(); }
                else OpenCloseBtn.Visibility = Visibility.Visible;
                Counter.Text = _notifications.Count.ToString();
                this.Height = 100 + _notifications.Count*100;
                NotList.Children.Clear();
                foreach (var noti in _notifications) {
                    notification_popup popup = new notification_popup();
                    popup.DataContext = noti;
                    popup.cancel.Click += (s, e) => { _notifications.Remove(popup.DataContext as notification); notifications = _notifications; NotList.Children.Remove(popup); popup = null; };
                    NotList.Children.Add(popup);
                }
            } 
        }
        bool isOpened = false;
        private void OpenClose(object sender, RoutedEventArgs e)
        {
            OpenClose();
        }
        private void OpenClose()
        {
            if (isOpened == false) { isOpened = true; Canvas.SetRight(this, 10); }
            else { isOpened = false; Canvas.SetRight(this, -240); }
        }
    }
}
