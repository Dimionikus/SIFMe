using SIFMe.DBConnector;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;

namespace SIFMe.Objects
{
    public partial class datatag_list : UserControl
    {
        public int select_status
        {
            get => _select_status;
            set
            {
                if (_select_status != value)
                {
                    _select_status = value;
                    if (_select_status < 2) { tb.IsEnabled = false; cb.IsEnabled = false; } else { tb.IsEnabled = true; cb.IsEnabled = true; }
                    if (_select_status == 0) { tb.Width = 150; tb.Margin = new Thickness(0, 0, 0, 0); } 
                    else { tb.Width = 130; tb.Margin = new Thickness(0, 0, 16, 0); }
                }
            }
        }
        int _select_status = 0;
        List<datatag> datatags = new List<datatag>();
        public datatag current_datatag = null;
        public datatag_list()
        {
            InitializeComponent();
            tb.DataContextChanged += SourcesUpdate;
            cb.SelectionChanged += SelChanged;
        }
        private void SelChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb.SelectedIndex < 0) return;
            current_datatag = (datatags)[cb.SelectedIndex];
            tb.Text = current_datatag.tag;
        }
        private void SourcesUpdate(object sender, DependencyPropertyChangedEventArgs e)
        {
            cb.ItemsSource = null;
            datatags = tb.DataContext as List<datatag>;
            cb.ItemsSource = datatags;
            if (datatags.Exists(tag => tag.tag.ToLower() == tb.Text.ToLower()))
            {
                cb.SelectedIndex = datatags.IndexOf(datatags.First(t => t.tag.ToLower() == tb.Text.ToLower()));
            }
            else current_datatag = null;
        }
    }
}
