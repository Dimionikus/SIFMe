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
                    if (_select_status == 0) { tb.Width = 150; tb.Margin = new Thickness(0, 0, 0, 0); } else { tb.Width = 130; tb.Margin = new Thickness(0, 0, 16, 0); }
                }
            }
        }
        int _select_status = 0;
        public datatag_list()
        {
            InitializeComponent();
            this.MouseEnter += (s, e) => { select_status = 1; };
            this.MouseLeave += (s, e) => { if (select_status != 2) select_status = 0; };
            this.MouseLeftButtonUp += (s, e) => { if (select_status != 2) select_status = 2; else select_status = 1; };
        }
    }
}
