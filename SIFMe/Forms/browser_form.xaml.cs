using SIFMe.ExternalClasses;
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

namespace SIFMe.Forms
{
    public partial class browser_form : form_controls
    {
        public browser_form()
        {
            InitializeComponent();
            BottomBorder.MouseLeftButtonDown += (s, e) => { };
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
    }
}
