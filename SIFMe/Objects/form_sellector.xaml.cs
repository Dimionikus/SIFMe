using System.Collections.Generic;
using System.Windows.Controls;

namespace SIFMe.Objects
{
    /// <summary>
    /// Логика взаимодействия для form_sellector.xaml
    /// </summary>
    public partial class form_sellector : UserControl
    {
        public MainWindow parent;
        private List<Border> buttons = new List<Border>();
        public form_sellector()
        {
            InitializeComponent();
            buttons.Add(CreaF0);
            buttons.Add(CreaF1);
        }
        private void Create(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            int type = buttons.IndexOf((Border)sender);
            parent.CreateForm(type);
        }
    }
}
