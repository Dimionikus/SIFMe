using SIFMe.DBConnector;
using SIFMe.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class link_view : UserControl
    {
        public link_view()
        {
            InitializeComponent();
            this.MouseEnter += Link_view_MouseEnter;
            this.MouseLeave += Link_view_MouseLeave;
            this.MouseDoubleClick += Link_view_MouseDoubleClick;
        }

        private void Link_view_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try { Process.Start((this.DataContext as file_link).link); } catch { MessageBox.Show("Неудалось запустить(открыть) файл, возможно неверно указан путь"); } 
        }

        private void Link_view_MouseLeave(object sender, MouseEventArgs e)
        {
            labl.Visibility = Visibility.Visible;
        }
        private void Link_view_MouseEnter(object sender, MouseEventArgs e)
        {
            labl.Visibility = Visibility.Collapsed;
        }
    }
}
