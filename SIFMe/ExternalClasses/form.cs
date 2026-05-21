using SIFMe.Forms;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SIFMe.ExternalClasses
{
    public class form
    {
        public int ID = 0;
        public double height = 450;
        public int layer
        {
            get => _layer; set
            {
                if (_layer != value) { _layer = value; if (window != null) Panel.SetZIndex(window, _layer); }
            }
        }
        private int _layer = 0;
        public string icon_path = "";
        /// <summary>
        /// 0-wraped; 1-showed; 2-dragging.
        /// </summary>
        public int state
        {
            get => _state; set
            {
                if (_state != value) { _state = value; OnChangeState(); }
            }
        }
        private int _state = 0;
        form_controls window;
        public Canvas formsapce;
        public MainWindow parent;
        public form(int index)
        {
            switch (index)
            {
                case 0: window = new browser_form(); break;
            }
            window.MouseLeftButtonDown += OnWindowClick;
            window.Dragger.MouseLeftButtonDown += OnDragStarted;
        }
        private void OnWindowClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            layer = -1;
            parent.ResortLayers();
        }
        private void OnChangeState()
        {
            if (formsapce == null && parent != null) formsapce = parent.Formspace;
            if (window == null) return;
            switch (_state)
            {
                //case 0: { layer = 0; formsapce.Children.Remove(window); window.parent = null; } break;
                //case 1: { if (!formsapce.Children.Contains(window)) formsapce.Children.Add(window); window.parent = formsapce; window.Visibility = System.Windows.Visibility.Visible; layer = -1; parent.ResortLayers(); } break;
                case 2: window.Visibility = System.Windows.Visibility.Collapsed; break;
            }
        }
        private void OnDragStarted(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine(1);
            Style icon_style = (Style)window.FindResource("Icon");
            parent.StartDrag(this, icon_style);
        }
        public void OnDragEnded(Point position)
        {
            Canvas.SetLeft(window, position.X);
            Canvas.SetTop(window, position.Y);
        }
        public ResourceDictionary GetResources()
        {
            if (window == null) return null;
            ResourceDictionary rd = window.Resources;
            return rd;
        }
    }
}
