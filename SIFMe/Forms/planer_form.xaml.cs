using SIFMe.DBConnector;
using SIFMe.ExternalClasses;
using SIFMe.Objects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace SIFMe.Forms
{
    /// <summary>
    /// Логика взаимодействия для planer_form.xaml
    /// </summary>
    public partial class planer_form : form_controls
    {
        List<notification> not_list;
        public notification_controller controller;
        string[] types = new string[] { "Каждые:", "Будет в:" };
        notification current_notification = null;
        public planer_form()
        {
            InitializeComponent();
            BottomBorder.MouseLeftButtonDown += (s, e) => { resizable = true; };
            Dragger = Dragger_;
            TypeSelector.ItemsSource = types;
            EditPanel.DataContextChanged += EditPanel_DataContextChanged;
        }
        private void EditPanel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (current_notification == null) return;
            if (current_notification.ID == 0) DelBtn.Visibility = Visibility.Collapsed; 
            else DelBtn.Visibility = Visibility.Visible;
        }
        protected override POINT GetBottomBorderPos()
        {
            var wpfPoint = BottomBorder.TranslatePoint(new System.Windows.Point(0, 0), this);
            return new POINT
            {
                X = (int)wpfPoint.X,
                Y = (int)wpfPoint.Y
            };
        }
        private void NewNotification(object sender, RoutedEventArgs e)
        {
            if (current_notification != null) { MessageBox.Show("Редактор сейчас занят!"); return; }
            current_notification = new notification();
            EditPanel.DataContext = current_notification;
            EditPanel.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Collapsed;
            TypeSelector.SelectedIndex = 0;
        }
        private void UpdateNotification(object sender, MouseButtonEventArgs e)
        {
            notification noti = (sender as Control).DataContext as notification;
            if (current_notification != null) { MessageBox.Show("Редактор сейчас занят!"); return; }
            current_notification = new notification();
            current_notification.ID = noti.ID;
            current_notification.title = noti.title;
            current_notification.description = noti.description;
            current_notification.play_type = noti.play_type;
            current_notification.interval = noti.interval;
            EditPanel.DataContext = current_notification;
            EditPanel.Visibility = Visibility.Visible;
            AddButton.Visibility = Visibility.Collapsed;
        }
        public void DeleteNotification(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Ты уверен что хочешь удалить напоминание '" + current_notification.title+"'?", "Удаление напоминаия", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                controller.del_not(current_notification);
                EditorClose();
                UpdateNotList();
            }
        }
        private void SaveNotification(object sender, RoutedEventArgs e)
        {
            if (current_notification == null) return;
            if (string.IsNullOrWhiteSpace(current_notification.title)) { MessageBox.Show("Название не указано! Укажите Название и попробуйте снова."); return; }
            try
            {
                var t = TimeSpan.ParseExact(current_notification.interval, "hh\\:mm", CultureInfo.InvariantCulture);
                int hours = t.Hours;
                int minutes = t.Minutes;
                if (hours <= 0 && minutes <= 0) { MessageBox.Show("Неверный формат указания времени или установленное время меньше 00:01. Должно быть: 'ЧЧ:ММ'"); return; }
            } catch { MessageBox.Show("Неверный формат указания времени. Должно быть: 'ЧЧ:ММ'"); return; }
            controller.save_not(current_notification);
            EditorClose();
            UpdateNotList();
        }
        private void EditorClose()
        {
            current_notification = null;
            EditPanel.DataContext = null;
            EditPanel.Visibility = Visibility.Collapsed;
            AddButton.Visibility= Visibility.Visible;
        }
        private void EditorClose(object sender, RoutedEventArgs e)
        {
            EditorClose();
        }
        public void UpdateNotList()
        {
            not_list = controller.not_list.ToList();
            if (not_list.Count > 0) { Placeholder.Visibility = Visibility.Collapsed; ResultList.Visibility = Visibility.Visible; }
            else {  Placeholder.Visibility = Visibility.Visible; ResultList.Visibility = Visibility.Collapsed; }
            ResultList.ItemsSource = null;
            ResultList.ItemsSource = not_list;
        }
    }
}
