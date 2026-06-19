using SIFMe.DBConnector;
using SIFMe.ExternalClasses;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIFMe
{
    public partial class MainWindow : Window
    {
        List<form> forms = new List<form>();
        form dragged_form;
        public browser_controller bc;
        public notification_controller nc;
        public MainWindow()
        {
            InitializeComponent();
            FBar.parent = this;
            FSelector.parent = this;
            nc = new notification_controller();
            nc.nl = nl;
            bc = new browser_controller();
            this.PreviewMouseMove += MouseMove;
        }
        public void CreateForm(int form_type)
        {
            NewForm(form_type);
        }
        private void NewForm(int form_type)
        {
            int future_id = 0;
            for (int i = 1; i < 8; i++)
            {
                bool check = forms.Any(x => x.ID == i);
                future_id = check ? 0 : i;
                if (check == false) break;
            }
            if (future_id == 0) return;
            form test = new form(form_type);
            test.parent = this;
            test.ID = future_id;
            forms.Add(test);
            FBar.AddContent(forms);
        }
        private void MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(Space);
            Canvas.SetLeft(DragBox, pos.X - 25);
            Canvas.SetTop(DragBox, pos.Y - 25);
        }
        public void StartDrag(form sender, Style Icon)
        {
            dragged_form = sender;
            bool is_form_in_bar = (dragged_form.state == 0);
            dragged_form.state = 2;
            if (is_form_in_bar) FBar.UpdateContent();
            DragSpace.Visibility = Visibility.Visible;
            DragBox.Style = Icon;
            DragBox.MouseLeftButtonUp += FinishDrag;
        }
        public void DeleteForm(form sender)
        {
            try
            {
                forms.Remove(sender);
                FBar.AddContent(forms);
            }
            catch { MessageBox.Show("Найдено необработанное исключение! Ошибка закрытия формы."); }
        }
        private void FinishDrag(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(Space);
            DragSpace.Visibility = Visibility.Collapsed;
            DragBox.MouseLeftButtonUp -= FinishDrag;
            if (pos.X < FBar.Width + 10) { FBar.OnDragEnded(pos, dragged_form); }
            else
            {
                dragged_form.state = 1;
                Point new_form_position = new Point(pos.X - 25, pos.Y - dragged_form.height / 2);
                dragged_form.OnDragEnded(new_form_position);
            }
        }
        public void ResortLayers()
        {
            try
            {
                int[] layer_list = new int[8]; layer_list[0] = 1;
                for (int i = 1; i < 8; i++)
                {
                    if (forms.Any(x => x.layer == i)) { layer_list[i] = forms.First(p => p.layer == i).ID; }
                    else layer_list[i] = 0;
                }
                int j = 1;
                int last_layer = 1;
                while (j < 8)
                {
                    if (layer_list[j] == 0 && layer_list[j - 1] != 0) { last_layer = j; }
                    if (layer_list[j] != 0 && layer_list[j - 1] == 0) { layer_list[last_layer] = layer_list[j]; layer_list[j] = 0; j = last_layer; }
                    if (layer_list[j] != 0 && layer_list[j - 1] != 0) forms.First(x => x.ID == layer_list[j]).layer = j;
                    j++;
                }
                forms.First(x => x.layer == -1).layer = last_layer;
            }
            catch { MessageBox.Show("Найдено необработанное исключение! Ошибка сортировки слоёв."); }
        }
    }
}
