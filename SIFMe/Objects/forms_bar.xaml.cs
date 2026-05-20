using SIFMe.ExternalClasses;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SIFMe.Objects
{
    public partial class forms_bar : UserControl
    {
        public MainWindow parent;
        /// <summary>
        /// 0-drag; 1-close.
        /// </summary>
        public int selection_mode
        {
            get => _selection_mode; set
            {
                if (_selection_mode != value)
                {
                    _selection_mode = value;
                    if (selection_mode == 1) { foreach (Border c in content) { c.MouseEnter += OnMouseHovered; c.MouseLeave += OnMouseExit; } }
                    else { foreach (Border c in content) { c.MouseEnter -= OnMouseHovered; c.MouseLeave -= OnMouseExit; } }
                }
            }
        }
        private int _selection_mode = 0;
        List<form> forms = new List<form>();
        List<Border> content = new List<Border>();
        public forms_bar()
        {
            InitializeComponent();
            AddButton.RenderTransform = new RotateTransform(0);
            AddButton.RenderTransformOrigin = new Point(0.5, 0.5);
        }
        public void AddContent(List<form> updatedList)
        {
            ClearContent();
            forms = updatedList;
            foreach (var item in forms)
            {
                if (item.state == 0) AddObject(item as form);
            }
        }
        public void UpdateContent()
        {
            ClearContent();
            foreach (var item in forms)
            {
                if (item.state == 0) AddObject(item as form);
            }
        }
        private void MoveContent(form form, double position)
        {

        }
        private void AddObject(form item)
        {
            Border con_elem = new Border();
            con_elem.Width = 50; con_elem.Height = 50;
            con_elem.Margin = new Thickness(0, 5, 0, 0);
            con_elem.Resources = item.GetResources();
            con_elem.Style = (Style)con_elem.FindResource("Icon");
            con_elem.DataContext = item;
            if (selection_mode == 1) { con_elem.MouseEnter += OnMouseHovered; con_elem.MouseLeave += OnMouseExit; }
            con_elem.MouseLeftButtonDown += OnDragStarted;
            Stacks.Children.Add(con_elem);
            content.Add(con_elem);
        }
        private void ClearContent()
        {
            Stacks.Children.Clear();
            content.Clear();
        }
        private void OnDragStarted(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Border con_elem = (Border)sender;
            form f = (form)con_elem.DataContext;
            if (selection_mode == 1) { parent.DeleteForm(f); return; }
            parent.StartDrag(f, con_elem.Style);
        }
        public void OnDragEnded(Point position, form f)
        {
            f.state = 0;
            UpdateContent();
        }
        private void OnMouseHovered(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border obj = (Border)sender;
            obj.Background = (VisualBrush)obj.FindResource("ClosePicture");
            obj.BorderBrush = (SolidColorBrush)obj.FindResource("CloseBorder");
        }
        private void OnMouseExit(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Border obj = (Border)sender;
            obj.Background = (VisualBrush)obj.FindResource("IconPicture");
            obj.BorderBrush = (SolidColorBrush)obj.FindResource("BrushBorder");
        }
        private void OpenCloseSelector(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bool opening = parent.FSelector.Visibility == Visibility.Collapsed;
            double targetTop = opening ? 5 : 8;
            double targetLeft = opening ? 9 : 13;
            double targetSize = opening ? 45 : 35;
            double targetAngle = opening ? 45 : 0;
            selection_mode = opening ? 1 : 0;
            Animator.AnimateDouble(AddButton, Canvas.TopProperty, targetTop);
            Animator.AnimateDouble(AddButton, Canvas.LeftProperty, targetLeft);
            Animator.AnimateDouble(AddButton, Border.WidthProperty, targetSize);
            Animator.AnimateDouble(AddButton, Border.HeightProperty, targetSize);
            AddButton.RenderTransform = new RotateTransform(targetAngle);
            parent.FSelector.Visibility = opening ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
