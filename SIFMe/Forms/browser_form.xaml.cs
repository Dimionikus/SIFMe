using Microsoft.Win32;
using SIFMe.DBConnector;
using SIFMe.ExternalClasses;
using SIFMe.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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

namespace SIFMe.Forms
{
    public partial class browser_form : form_controls
    {
        List<datatag_list> filters = new List<datatag_list>();
        public browser_controller controller;
        BitmapImage getImageSource(string name)
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("pack://application:,,,/SIFMe;component/Sprites/"+name);
            img.EndInit();
            return img;
        }
        int _current_page = 0;
        int current_page { get => _current_page; set { 
                if (_current_page != value) _current_page = value;
                SB2.Source = getImageSource("switch1_d.png");
                SB1.Source = getImageSource("switch2_d.png");
                Browser.Visibility = Visibility.Collapsed; Redactor.Visibility = Visibility.Collapsed; SBW1.Width = 40; SBW2.Width = 40;
                if (_current_page == 1) { Browser.Visibility = Visibility.Visible; SBW1.Width = 20; SB1.Source = getImageSource("switch2_e.png"); if (filters.Count <= 0 && controller != null) UpdateLinkList(); }
                if (_current_page == 2) { Redactor.Visibility = Visibility.Visible; SBW2.Width = 20; SB2.Source = getImageSource("switch1_e.png"); }
            } 
        }
        public browser_form()
        {
            InitializeComponent();
            BottomBorder.MouseLeftButtonDown += (s, e) => { resizable = true; };
            Dragger = Dragger_;
            current_page = 1;
            controller = null;
            selector.tb.TextChanged += UpdateTagList;
        }
        protected override POINT GetBottomBorderPos()
        {
            var wpfPoint = BottomBorder.TranslatePoint(new System.Windows.Point(0, 0), this);
            FilterViewCompress();
            return new POINT
            {
                X = (int)wpfPoint.X,
                Y = (int)wpfPoint.Y
            };
        }
        private void Switch(object sender, MouseButtonEventArgs e)
        {
            Image btn = sender as Image;
            if (btn.Name == "SB1") current_page = 1;
            if (btn.Name == "SB2") current_page = 2;
        }
        private void Switch(object sender, MouseEventArgs e)
        {
            Image btn = sender as Image;
            if (btn.Name == "SB1" && current_page == 2) { 
                if (e.RoutedEvent.Name == "MouseEnter") { btn.Source = getImageSource("switch2_s.png"); }
                else { btn.Source = getImageSource("switch2_d.png"); }
            }
            if (btn.Name == "SB2" && current_page == 1) {
                if (e.RoutedEvent.Name == "MouseEnter") { btn.Source = getImageSource("switch1_s.png"); }
                else { btn.Source = getImageSource("switch1_d.png"); }
            }
        }
        int switcher = 1;
        int editor_switch {  get=>switcher; set {
                if (value != switcher) { switcher = value;
                    TagManager.Visibility = Visibility.Collapsed; TagList.Visibility = Visibility.Collapsed; LinkManager.Visibility = Visibility.Collapsed; LinkEditor.Visibility = Visibility.Collapsed;
                    if (switcher == 1) { TagManager.Visibility = Visibility.Visible; TagList.Visibility = Visibility.Visible; }
                    if (switcher == 2) { LinkManager.Visibility = Visibility.Visible; if (editor_is_busy == true) { LinkEditor.Visibility = Visibility.Visible; selector.tb.DataContext = GetDatatagList(); } }
                }
            }
        }
        private void Switch2(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "TBtn") editor_switch = 1;
            if (btn.Name == "FBtn") editor_switch = 2;
        }
        int a = 0;
        private void GetLink(object sender, RoutedEventArgs e)
        {
            Button obj = sender as Button;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Выберите файл";
            if (openFileDialog.ShowDialog() == true) {
                string link = openFileDialog.FileName;
                if (obj.Name != "UpdLink") LinkBox.Text = link;
                else { redacting_link.link = link; LinkEditor.DataContext = null; LinkEditor.DataContext = redacting_link; }
            }
            else
            {
                MessageBox.Show("Файл не выбран.");
            }
        }
        bool eib = false;
        bool editor_is_busy {  get=>eib; set {
                if (eib != value) { eib = value;
                    if (eib==false) LinkEditor.Visibility = Visibility.Collapsed;
                    else LinkEditor.Visibility = Visibility.Visible;
                }
            } 
        }
        private void ItemHover(object sender, MouseEventArgs e)
        {
            (sender as Grid).Children[1].Visibility = Visibility.Visible;
        }
        private void ItemHoverEnd(object sender, MouseEventArgs e)
        {
            (sender as Grid).Children[1].Visibility = Visibility.Collapsed;
        }
        file_link redacting_link = null;
        private void NewLink(object sender, RoutedEventArgs e)
        {
            if (editor_is_busy == false) { 
                if (LinkBox.Text != "" || !string.IsNullOrWhiteSpace(LinkBox.Text)) {
                    editor_is_busy=true;
                    redacting_link = new file_link();
                    redacting_link.link = LinkBox.Text;
                    LinkBox.Text = "";
                    LinkEditor.DataContext = redacting_link;
                }
                else MessageBox.Show("Отсутствует ссылка на файл");
            } else MessageBox.Show("Редактор занят!");
        }
        public void DeleteLink(object sender, RoutedEventArgs e)
        {
            file_link link = (sender as Control).DataContext as file_link;
            controller.del_link(link);
            UpdateLinkList();
        }
        public void UpdateLink(object sender, RoutedEventArgs e)
        {
            file_link link = (sender as Control).DataContext as file_link;
            if (editor_is_busy == false)
            {
                editor_is_busy = true;
                redacting_link = link;
                LinkBox.Text = "";
                LinkEditor.DataContext = redacting_link;
                current_page = 2;
                editor_switch = 2;
                
            }
            else MessageBox.Show("Редактор занят!");
        }
        private void SaveLink(object sender, RoutedEventArgs e)
        {
            if (redacting_link == null) return;
            if (editor_tags.Count == 0) { MessageBox.Show("Указано 0 тегов  для этого файла. Далжен быть минимум 1."); return; }
            string tags_row = "";
            foreach (datatag tag in editor_tags)
            {
                tags_row += "/"+ tag.ID.ToString()+"/";
            }
            redacting_link.tags = tags_row;
            controller.save_link(redacting_link);
            CloseLinkEditor();
        }
        private void CloseLinkEditor()
        {
            LinkEditor.DataContext = null;
            editor_is_busy = false;
            LinkEditor.Visibility = Visibility.Collapsed;
            editor_tags.Clear();
            EditorTagList.ItemsSource = null;
        }
        private void CloseLinkEditor(object sender, RoutedEventArgs e)
        {
            CloseLinkEditor();
        }
        private void NewTag(object sender, RoutedEventArgs e)
        {
            if (TagName.Text == "" || string.IsNullOrWhiteSpace(TagName.Text)) { MessageBox.Show("Отсутствует название тега"); return; }
            string result = controller.new_tag(TagName.Text);
            if (result != "") { MessageBox.Show(result); return; }
            TagList.ItemsSource = null;
            TagList.ItemsSource = controller.tag_list;
            UpdateAllTagLists();
        }
        List<datatag> editor_tags = new List<datatag>();
        private void AddTag(object sender,  RoutedEventArgs e)
        {
            if (selector.current_datatag == null) { MessageBox.Show("Тег не найден"); return; }
            if (editor_tags.Exists(t => t.ID == selector.current_datatag.ID)) { MessageBox.Show("Данный тег уже присутствует в списке"); return; }
            datatag tag = selector.current_datatag;
            editor_tags.Add(tag);
            EditorTagList.ItemsSource = null;
            EditorTagList.ItemsSource = editor_tags;
        }
        private void UpdateAllTagLists()
        {
            selector.tb.DataContext = GetDatatagList();
            foreach (datatag_list filter in FilterList.Children) 
            { 
                filter.tb.DataContext = GetDatatagList();
            }
        }
        private void UpdateTagList(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            textBox.DataContext = GetDatatagList(textBox.Text);
        }
        private List<datatag> GetDatatagList()
        {
            return controller.tag_list.ToList();
        }
        private List<datatag> GetDatatagList(string s)
        {
            if (!controller.tag_list.ToList().Exists(tag => tag.tag.ToLower().Contains(s.ToLower()))) return GetDatatagList();
            else return controller.tag_list.ToList().Where(tag => tag.tag.ToLower().Contains(s.ToLower())).ToList();
        }
        private void UpdateLinkList()
        {
            ResultList.ItemsSource = null;
            ResultList.ItemsSource = controller.link_list.ToList();
        }
        private void AddFilter(object sender, RoutedEventArgs e)
        {
            datatag_list filter = new datatag_list();
            filters.Add(filter);
            FilterList.Children.Add(filter);
            filter.tb.DataContext = GetDatatagList();
            filter.tb.TextChanged += UpdateTagList;
            if (FilterList.ActualWidth < filter.MaxWidth * filters.Count) { FilterViewCompress(filter); }
            filter.MouseEnter += (s, erg) => { if (filter.select_status != 2) filter.select_status = 1; FilterViewCompress(filter); };
            filter.MouseLeave += (s, erg) => { if (filter.select_status != 2) filter.select_status = 0; FilterViewCompress(filter); };
            filter.MouseLeftButtonDown += (s, erg) => { filter.select_status = 2; FilterViewCompress(filter); };
        }
        private void RemoveFilterModeSwitch(object sender, RoutedEventArgs e)
        {
            filters.Clear();
            FilterList.Children.Clear();
            UpdateLinkList();
        }
        private void ApplyFilter(object sender, RoutedEventArgs e)
        {
            List<file_link> results = controller.link_list.ToList();
            foreach (datatag_list filter in filters)
            {
                if (filter.current_datatag != null)
                results = results.Where(res => res.tags.Contains("/"+filter.current_datatag.ID+"/")).ToList();
            }
            ResultList.ItemsSource = null;
            ResultList.ItemsSource = results;
        }
        public void FilterViewCompress()
        {
            foreach (datatag_list f in filters)
            {
                if (FilterList.ActualWidth < f.MaxWidth * filters.Count) f.Width = (FilterList.ActualWidth - f.MaxWidth - 5f * filters.Count) / filters.Count;
                f.select_status = 0;
            }
        }
        public void FilterViewCompress(datatag_list exept)
        {
            int status_store = exept.select_status;
            FilterViewCompress();
            exept.Width = exept.MaxWidth;
            exept.select_status = status_store;
        }
    }
}
